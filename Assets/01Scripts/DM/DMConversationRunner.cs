/*
작성자 : 이종현
작성일 : 26-06-01
수정일 : 26-06-16

역할 : DM 대화 진행 담당
방식 : 로컬 진행 상태의 ProgressState와 SelectedChoiceNum을 기준으로 대화 상태를 복원 및 진행
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DMConversationRunner : MonoBehaviour
{
    private enum DMProgressState
    {
        Unread = 0,
        WaitingChoice = 1,
        Completed = 2
    }

    [SerializeField] private DMChatUI chatUI;
    [SerializeField] private Button skipAreaButton;

    [Header("SO Data")]
    private DM_TableSO currentDM;
    [SerializeField] private Dialogue_TableSO[] dialogueSOs;
    [SerializeField] private Choice_TableSO[] choiceSOs;

    [Header("Setting")]
    [SerializeField] private float messageDelay = 1f;

    private readonly Dictionary<int, Dialogue_TableSO> dialogueTable = new();
    private readonly Dictionary<int, List<Dialogue_TableSO>> dialogueByMessageIdTable = new();
    private readonly Dictionary<int, List<Choice_TableSO>> choiceTable = new();

    private IString_TableManager stringTableManager;

    private Coroutine playRoutine;

    private bool isWaitingMessageDelay;
    private bool isSkipRequested;

    private int currentProgressState;
    private int currentSelectedChoiceNum;

    public string LastPreviewText { get; private set; }

    public int CurrentMessageId
    {
        get
        {
            if (currentDM == null)
                return 0;

            return currentDM.messageId;
        }
    }

    public Action<int, int, int, string> OnProgressChanged;
    public Action<int> OnQuestDMCompleted;

    private void Awake()
    {
        stringTableManager = ServiceLocator.Get<IString_TableManager>();
        BuildRuntimeTable();

        if (skipAreaButton != null)
            skipAreaButton.onClick.AddListener(OnClickSkipArea);
    }

    private void OnDestroy()
    {
        if (skipAreaButton != null)
            skipAreaButton.onClick.RemoveListener(OnClickSkipArea);
    }

    ///<summary>
    /// NPC DM 대화를 로컬 진행 상태 기준으로 엽니다.
    ///</summary>
    public void OpenNpcDM(DM_TableSO dmData, int progressState, int selectedChoiceNum)
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

        currentDM = dmData;
        currentProgressState = progressState;
        currentSelectedChoiceNum = selectedChoiceNum;
        LastPreviewText = "";
        isWaitingMessageDelay = false;
        isSkipRequested = false;

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

        bool instantRestore = progressState != (int)DMProgressState.Unread;

        playRoutine = StartCoroutine(
            PlayDialogueRoutine(startDialogId, instantRestore)
        );
    }

    private IEnumerator PlayDialogueRoutine(int startDialogId, bool instantRestore)
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
                if (currentProgressState == (int)DMProgressState.Completed)
                {
                    Choice_TableSO selectedChoice =
                        GetChoiceByChoiceNum(dialogue.choiceGroupId, currentSelectedChoiceNum);

                    if (selectedChoice == null)
                    {
                        Log.Message($"선택한 Choice를 찾을 수 없습니다. Group : {dialogue.choiceGroupId}, Num : {currentSelectedChoiceNum}");
                        yield break;
                    }

                    string choiceText = GetText(selectedChoice.choiceText);
                    chatUI.AddPlayerMessage(choiceText);
                    LastPreviewText = choiceText;

                    currentDialogId = selectedChoice.nextDialogId;
                    continue;
                }

                currentProgressState = (int)DMProgressState.WaitingChoice;
                NotifyProgressChanged();

                ShowChoices(dialogue.choiceGroupId);
                yield break;
            }

            if (dialogue.isEnd)
            {
                currentProgressState = (int)DMProgressState.Completed;
                NotifyProgressChanged();
                NotifyQuestCompleted();
                yield break;
            }

            if (!instantRestore)
                yield return WaitMessageDelayRoutine();

            currentDialogId = dialogue.nextDialogId;
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
            choiceTexts[i] = GetText(choices[i].choiceText);
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

        string choiceText = GetText(selectedChoice.choiceText);

        chatUI.AddPlayerMessage(choiceText);

        currentSelectedChoiceNum = selectedChoice.choiceNum;
        currentProgressState = (int)DMProgressState.Completed;

        OnProgressChanged?.Invoke(
            currentDM.messageId,
            currentProgressState,
            currentSelectedChoiceNum,
            ""
        );

        yield return WaitMessageDelayRoutine();

        yield return PlayDialogueRoutine(selectedChoice.nextDialogId, false);
    }

    private void ShowMessage(Dialogue_TableSO dialogue)
    {
        string senderType = dialogue.senderType.ToString();
        string text = GetText(dialogue.dialogText);

        LastPreviewText = text;

        if (senderType == "NPC")
            chatUI.AddOpponentMessage(text);
        else if (senderType == "USER")
            chatUI.AddPlayerMessage(text);
        else
            Log.Message($"알 수 없는 SenderType : {senderType}");
    }

    ///<summary>
    /// 메시지 딜레이 중일 때 대기 시간을 스킵합니다.
    ///</summary>
    public void OnClickSkipArea()
    {
        if (!isWaitingMessageDelay)
            return;

        isSkipRequested = true;
    }

    private IEnumerator WaitMessageDelayRoutine()
    {
        isWaitingMessageDelay = true;
        isSkipRequested = false;

        float timer = 0f;

        while (timer < messageDelay)
        {
            if (isSkipRequested)
                break;

            timer += Time.deltaTime;
            yield return null;
        }

        isWaitingMessageDelay = false;
        isSkipRequested = false;
    }

    private void NotifyProgressChanged()
    {
        if (currentDM == null)
            return;

        OnProgressChanged?.Invoke(
            currentDM.messageId,
            currentProgressState,
            currentSelectedChoiceNum,
            LastPreviewText
        );
    }

    private void NotifyQuestCompleted()
    {
        if (currentDM == null)
            return;

        if (currentDM.dmQuestType == DMQuestTypeEnum.Dummy)
            return;

        OnQuestDMCompleted?.Invoke(currentDM.messageId);
    }

    ///<summary>
    /// 진행 중인 DM 대화를 중단합니다.
    ///</summary>
    public void StopConversation()
    {
        if (playRoutine != null)
        {
            StopCoroutine(playRoutine);
            playRoutine = null;
        }

        isWaitingMessageDelay = false;
        isSkipRequested = false;

        if (chatUI != null)
            chatUI.HideChoices();
    }

    private Choice_TableSO GetChoiceByChoiceNum(int choiceGroupId, int choiceNum)
    {
        if (!choiceTable.TryGetValue(choiceGroupId, out List<Choice_TableSO> choices))
            return null;

        foreach (Choice_TableSO choice in choices)
        {
            if (choice == null)
                continue;

            if (choice.choiceNum == choiceNum)
                return choice;
        }

        return null;
    }

    private int GetStartDialogId(int messageId)
    {
        if (!dialogueByMessageIdTable.TryGetValue(messageId, out List<Dialogue_TableSO> dialogues))
            return 0;

        if (dialogues == null || dialogues.Count == 0)
            return 0;

        return dialogues[0].dialogId;
    }

    private string GetText(string stringKey)
    {
        if (stringTableManager == null)
        {
            Log.Message("String_TableManager를 찾을 수 없습니다.");
            return stringKey;
        }

        string text = stringTableManager.GetString(stringKey, SystemLanguage.Korean);

        if (string.IsNullOrEmpty(text))
        {
            Log.Message($"String Key를 찾을 수 없습니다 : {stringKey}");
            return stringKey;
        }

        return text;
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
}
