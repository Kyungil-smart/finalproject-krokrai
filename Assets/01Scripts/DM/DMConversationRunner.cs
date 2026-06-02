/*
작성자 : 이종현
작성일 : 26-06-01
수정일 : 26-06-01

역할 : DM 대화 진행 담당
방식 : SO 데이터를 읽기 전용으로 조회하여 말풍선/선택지 출력
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: Firebase 연결 시 현재 출력된 dialogueId 저장
// currentDialogId = dialogue.dialogId;

public class DMConversationRunner : MonoBehaviour
{
    [SerializeField] private DMChatUI chatUI;

    [Header("SO Data")]
    [SerializeField] private DMTableSO currentDM;
    [SerializeField] private DMDialogueSO[] dialogueSOs;
    [SerializeField] private DMChoiceSO[] choiceSOs;

    [Header("Setting")]
    [SerializeField] private float messageDelay = 1f;

    private readonly Dictionary<int, DMDialogueSO> dialogueTable = new();
    private readonly Dictionary<int, List<DMChoiceSO>> choiceTable = new();

    private Coroutine playRoutine;

    private void Awake()
    {
        BuildRuntimeTable();
    }

    private void BuildRuntimeTable()
    {
        dialogueTable.Clear();
        choiceTable.Clear();

        foreach (DMDialogueSO dialogue in dialogueSOs)
        {
            if (dialogue == null)
                continue;

            if (dialogueTable.ContainsKey(dialogue.dialogId))
            {
                Log.Message($"중복 Dialogue ID : {dialogue.dialogId}");
                continue;
            }

            dialogueTable.Add(dialogue.dialogId, dialogue);
        }

        foreach (DMChoiceSO choice in choiceSOs)
        {
            if (choice == null)
                continue;

            if (!choiceTable.ContainsKey(choice.choiceGroupId))
                choiceTable.Add(choice.choiceGroupId, new List<DMChoiceSO>());

            choiceTable[choice.choiceGroupId].Add(choice);
        }

        foreach (var pair in choiceTable)
        {
            pair.Value.Sort((a, b) => a.choiceNum.CompareTo(b.choiceNum));
        }
    }

    public void OpenNpcDM()
    {
        if (currentDM == null)
        {
            Log.Message("CurrentDM이 연결되지 않았습니다.");
            return;
        }

        OpenNpcDM(currentDM);
    }

    public void OpenNpcDM(DMTableSO dmData)
    {
        if (chatUI == null)
        {
            Log.Message("ChatUI가 연결되지 않았습니다.");
            return;
        }

        if (dmData == null)
        {
            Log.Message("DMTableSO 데이터가 없습니다.");
            return;
        }

        chatUI.ClearChat();

        if (playRoutine != null)
            StopCoroutine(playRoutine);

        LoadReceivedMessages(dmData.startDialogId, dmData.previewDialogId);
    }
    
    private void LoadReceivedMessages(int startDialogId, int previewDialogId)
    {
        int currentDialogId = startDialogId;

        while (currentDialogId != 0)
        {
            if (!dialogueTable.TryGetValue(currentDialogId, out DMDialogueSO dialogue))
            {
                Log.Message($"Dialogue ID를 찾을 수 없습니다 : {currentDialogId}");
                return;
            }

            ShowMessage(dialogue);

            if (currentDialogId == previewDialogId)
            {
                if (dialogue.choiceGroupId != 0)
                {
                    ShowChoices(dialogue.choiceGroupId);
                }

                return;
            }

            if (dialogue.isEnd)
                return;

            currentDialogId = dialogue.nextDialogId;
        }
    }

    private IEnumerator PlayDialogueRoutine(int startDialogId)
    {
        int currentDialogId = startDialogId;

        while (currentDialogId != 0)
        {
            if (!dialogueTable.TryGetValue(currentDialogId, out DMDialogueSO dialogue))
            {
                Log.Message($"Dialogue ID를 찾을 수 없습니다 : {currentDialogId}");
                yield break;
            }

            ShowMessage(dialogue);

            yield return new WaitForSeconds(messageDelay);

            if (dialogue.choiceGroupId != 0)
            {
                ShowChoices(dialogue.choiceGroupId);
                yield break;
            }

            if (dialogue.isEnd)
                yield break;

            currentDialogId = dialogue.nextDialogId;
        }
    }

    private void ShowMessage(DMDialogueSO dialogue)
    {
        if (dialogue.senderType == "NPC")
        {
            chatUI.AddOpponentMessage(dialogue.dialogText);
        }
        else if (dialogue.senderType == "USER")
        {
            chatUI.AddPlayerMessage(dialogue.dialogText);
        }
        else
        {
            Log.Message($"알 수 없는 SenderType : {dialogue.senderType}");
        }
    }

    private void ShowChoices(int choiceGroupId)
    {
        if (!choiceTable.TryGetValue(choiceGroupId, out List<DMChoiceSO> choices))
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

            DMChoiceSO selectedChoice = choices[selectedIndex];

            if (playRoutine != null)
                StopCoroutine(playRoutine);

            playRoutine = StartCoroutine(ChoiceSelectedRoutine(selectedChoice));
        });
    }
    
    private IEnumerator ChoiceSelectedRoutine(DMChoiceSO selectedChoice)
    {
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