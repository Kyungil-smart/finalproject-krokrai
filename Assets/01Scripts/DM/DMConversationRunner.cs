/*
작성자 : 이종현
작성일 : 26-06-01
수정일 : 26-06-02

역할 : DM 대화 진행 담당
방식 : DM_TableSO의 messageId를 기준으로 Dialogue/Choice SO를 조회하여 말풍선/선택지 출력
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Firebase 연결 시 현재 출력된 dialogId 저장
// currentDialogId = dialogue.dialogId;

public class DMConversationRunner : MonoBehaviour
{
    [SerializeField] private DMChatUI chatUI;

    [Header("SO Data")]
    private DM_TableSO currentDM;
    [SerializeField] private Dialogue_TableSO[] dialogueSOs;
    [SerializeField] private Choice_TableSO[] choiceSOs;

    [Header("Setting")]
    [SerializeField] private float messageDelay = 1f;

    private readonly Dictionary<int, Dialogue_TableSO> dialogueTable = new();
    private readonly Dictionary<int, List<Dialogue_TableSO>> dialogueByMessageIdTable = new();
    private readonly Dictionary<int, List<Choice_TableSO>> choiceTable = new();

    private Coroutine playRoutine;

    private void Awake()
    {
        BuildRuntimeTable();
    }

    private void BuildRuntimeTable()
    {
        dialogueTable.Clear();
        dialogueByMessageIdTable.Clear();
        choiceTable.Clear();

        foreach (Dialogue_TableSO dialogue in dialogueSOs)
        {
            if (dialogue == null)
                continue;

            if (dialogueTable.ContainsKey(dialogue.dialogId))
            {
                Log.Message($"중복 Dialogue ID : {dialogue.dialogId}");
                continue;
            }

            dialogueTable.Add(dialogue.dialogId, dialogue);

            if (!dialogueByMessageIdTable.ContainsKey(dialogue.messageId))
                dialogueByMessageIdTable.Add(dialogue.messageId, new List<Dialogue_TableSO>());

            dialogueByMessageIdTable[dialogue.messageId].Add(dialogue);
        }

        foreach (var pair in dialogueByMessageIdTable)
        {
            pair.Value.Sort((a, b) => a.dialogId.CompareTo(b.dialogId));
        }

        foreach (Choice_TableSO choice in choiceSOs)
        {
            if (choice == null)
                continue;

            if (!choiceTable.ContainsKey(choice.choiceGroupId))
                choiceTable.Add(choice.choiceGroupId, new List<Choice_TableSO>());

            choiceTable[choice.choiceGroupId].Add(choice);
        }

        foreach (var pair in choiceTable)
        {
            pair.Value.Sort((a, b) => a.choiceNum.CompareTo(b.choiceNum));
        }
    }

    public void OpenNpcDM(DM_TableSO dmData)
    {
        if (chatUI == null)
        {
            Log.Message("ChatUI가 연결되지 않았습니다.");
            return;
        }

        if (dmData == null)
        {
            Log.Message("DM_TableSO 데이터가 없습니다.");
            return;
        }

        chatUI.ClearChat();
        chatUI.HideChoices();

        if (playRoutine != null)
            StopCoroutine(playRoutine);

        int startDialogId = GetStartDialogId(dmData.messageId);

        if (startDialogId == 0)
        {
            Log.Message($"Message ID에 해당하는 Dialogue가 없습니다 : {dmData.messageId}");
            return;
        }

        playRoutine = StartCoroutine(PlayDialogueRoutine(startDialogId));
    }

    private int GetStartDialogId(int messageId)
    {
        if (!dialogueByMessageIdTable.TryGetValue(messageId, out List<Dialogue_TableSO> dialogues))
            return 0;

        if (dialogues == null || dialogues.Count == 0)
            return 0;

        return dialogues[0].dialogId;
    }

    private IEnumerator PlayDialogueRoutine(int startDialogId)
    {
        int currentDialogId = startDialogId;

        while (currentDialogId != 0)
        {
            if (!dialogueTable.TryGetValue(currentDialogId, out Dialogue_TableSO dialogue))
            {
                Log.Message($"Dialogue ID를 찾을 수 없습니다 : {currentDialogId}");
                yield break;
            }

            ShowMessage(dialogue);

            if (dialogue.choiceGroupId != 0)
            {
                ShowChoices(dialogue.choiceGroupId);
                yield break;
            }

            if (dialogue.isEnd)
                yield break;

            yield return new WaitForSeconds(messageDelay);

            currentDialogId = dialogue.nextDialogId;
        }
    }

    private void ShowMessage(Dialogue_TableSO dialogue)
    {
        string senderType = dialogue.senderType.ToString();

        if (senderType == "NPC")
        {
            chatUI.AddOpponentMessage(dialogue.dialogText);
        }
        else if (senderType == "USER")
        {
            chatUI.AddPlayerMessage(dialogue.dialogText);
        }
        else
        {
            Log.Message($"알 수 없는 SenderType : {senderType}");
        }
    }

    private void ShowChoices(int choiceGroupId)
    {
        if (!choiceTable.TryGetValue(choiceGroupId, out List<Choice_TableSO> choices))
        {
            Log.Message($"ChoiceGroup ID를 찾을 수 없습니다 : {choiceGroupId}");
            return;
        }

        string[] choiceTexts = new string[choices.Count];

        for (int i = 0; i < choices.Count; i++)
        {
            choiceTexts[i] = choices[i].choiceText;
        }

        chatUI.ShowChoices(choiceTexts, selectedIndex =>
        {
            if (selectedIndex < 0 || selectedIndex >= choices.Count)
            {
                Log.Message($"잘못된 선택지 인덱스 : {selectedIndex}");
                return;
            }

            Choice_TableSO selectedChoice = choices[selectedIndex];

            if (playRoutine != null)
                StopCoroutine(playRoutine);

            playRoutine = StartCoroutine(ChoiceSelectedRoutine(selectedChoice));
        });
    }

    private IEnumerator ChoiceSelectedRoutine(Choice_TableSO selectedChoice)
    {
        chatUI.HideChoices();
        chatUI.AddPlayerMessage(selectedChoice.choiceText);

        yield return new WaitForSeconds(messageDelay);

        playRoutine = StartCoroutine(PlayDialogueRoutine(selectedChoice.nextDialogId));
    }

    public void StopConversation()
    {
        if (playRoutine != null)
        {
            StopCoroutine(playRoutine);
            playRoutine = null;
        }

        if (chatUI != null)
        {
            chatUI.HideChoices();
        }
    }
}
