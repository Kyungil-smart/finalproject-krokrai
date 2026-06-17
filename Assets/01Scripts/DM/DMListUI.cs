/*
작성자 : 이종현
작성일 : 26-06-01
수정일 : 26-06-16

역할 : DM 목록 UI 생성 및 DM 클릭 시 대화창 전환
방식 : DB 저장 없이 로컬 Dictionary 기준으로 Dummy DM과 Quest DM을 생성, 진행, 완료, 삭제 처리
*/

using System;
using UnityEngine;
using System.Collections.Generic;

public class DMListUI : MonoBehaviour
{
    private class DMLocalProgress
    {
        public int DM_ID;
        public int DMType;
        public DateTime SentTime;
        public int ProgressState;
        public int SelectedChoiceNum;
        public int QuestRewardState;
        public string PreviewText;
    }

    private enum DMProgressState
    {
        Unread = 0,
        WaitingChoice = 1,
        Completed = 2
    }

    [SerializeField] private Transform content;
    [SerializeField] private GameObject dmListItemPrefab;

    [Header("Panels")]
    [SerializeField] private GameObject dmListPanel;
    [SerializeField] private GameObject dmChatPanel;

    [Header("DM Data")]
    [SerializeField] private DM_TableSO[] dmTables;
    [SerializeField] private Dialogue_TableSO[] dialogueSOs;
    [SerializeField] private Choice_TableSO[] choiceSOs;
    [SerializeField] private Npc_TableSO[] npcSOs;

    [Header("Local Quest Setting")]
    [SerializeField] private bool ignoreQuestGenerateTimeForTest = true;
    [SerializeField] private int maxQuestDMCount = 3;
    [SerializeField] private double questGenerateHour = 8;

    private IString_TableManager stringManager;

    private readonly Dictionary<int, DMLocalProgress> dmProgressTable = new();

    private DateTime dmGenerationTimestamp;

    private void Start()
    {
        stringManager = ServiceLocator.Get<IString_TableManager>();

        if (stringManager == null)
            Log.Message("String_TableManager를 찾을 수 없습니다.");

        if (dmChatPanel != null)
            dmChatPanel.SetActive(false);

        InitLocalProgressData();
        CreateDMList();
    }

    private void InitLocalProgressData()
    {
        dmProgressTable.Clear();
        CreateDummyDMProgress();
    }

    private void CreateDummyDMProgress()
    {
        if (dmTables == null)
        {
            Log.Message("DM_TableSO 배열이 연결되지 않았습니다.");
            return;
        }

        foreach (DM_TableSO dm in dmTables)
        {
            if (dm == null)
                continue;

            if (dm.dmQuestType != DMQuestTypeEnum.Dummy)
                continue;

            if (dmProgressTable.ContainsKey(dm.messageId))
                continue;

            DMLocalProgress progress = new DMLocalProgress
            {
                DM_ID = dm.messageId,
                DMType = (int)DMTypeEnum.Dummy,
                SentTime = DateTime.MinValue,
                ProgressState = (int)DMProgressState.Unread,
                SelectedChoiceNum = -1,
                QuestRewardState = (int)QuestRewardStateEnum.None,
                PreviewText = ""
            };

            dmProgressTable.Add(dm.messageId, progress);
            Log.Message($"Dummy DM 로컬 생성 : {dm.messageId}");
        }
    }

    private void CreateDMList()
    {
        if (content == null || dmListItemPrefab == null)
        {
            Log.Message("Content 또는 DMListItemPrefab이 연결되지 않았습니다.");
            return;
        }

        List<DMLocalProgress> progressList = new List<DMLocalProgress>(dmProgressTable.Values);

        progressList.Sort((a, b) => CompareDMListOrder(a, b));

        foreach (DMLocalProgress progress in progressList)
        {
            if (progress == null)
                continue;

            DM_TableSO dm = GetDMTable(progress.DM_ID);

            if (dm == null)
            {
                Log.Message($"DM_TableSO를 찾을 수 없습니다 : {progress.DM_ID}");
                continue;
            }

            GameObject item = Instantiate(dmListItemPrefab, content);

            DMListItemUI itemUI = item.GetComponentInChildren<DMListItemUI>();

            if (itemUI == null)
            {
                Log.Message("DMListItemUI를 찾을 수 없습니다.");
                continue;
            }

            string npcAccountName = GetNpcAccountName(dm.senderName);

            bool hasUnread = progress.ProgressState == (int)DMProgressState.Unread;
            string previewText = GetPreviewText(dm.messageId, npcAccountName, progress);

            string profileImageKey = GetNpcProfileImageKey(dm.senderName);

            itemUI.SetData(
                npcAccountName,
                previewText,
                hasUnread,
                profileImageKey,
                () => OnClickDM(dm)
            );
        }
    }

    private int CompareDMListOrder(DMLocalProgress a, DMLocalProgress b)
    {
        DM_TableSO aDM = GetDMTable(a.DM_ID);
        DM_TableSO bDM = GetDMTable(b.DM_ID);

        if (aDM == null || bDM == null)
            return 0;

        bool aQuest = aDM.dmQuestType != DMQuestTypeEnum.Dummy;
        bool bQuest = bDM.dmQuestType != DMQuestTypeEnum.Dummy;

        if (aQuest && bQuest)
            return b.SentTime.CompareTo(a.SentTime);

        if (aQuest && !bQuest)
            return -1;

        if (!aQuest && bQuest)
            return 1;

        return GetDMTableIndex(a.DM_ID).CompareTo(GetDMTableIndex(b.DM_ID));
    }

    private void OnClickDM(DM_TableSO dmData)
    {
        if (dmData == null)
        {
            Log.Message("선택된 DM 데이터가 없습니다.");
            return;
        }

        string profileImageKey = GetNpcProfileImageKey(dmData.senderName);

        DMChatUI chatUI = dmChatPanel.GetComponentInChildren<DMChatUI>();
        if (chatUI != null)
            chatUI.SetOpponentProfileImageKey(profileImageKey);

        if (dmListPanel != null)
            dmListPanel.SetActive(false);

        if (dmChatPanel != null)
            dmChatPanel.SetActive(true);

        DMLocalProgress progress = GetProgress(dmData.messageId);

        int openProgressState = progress.ProgressState;
        int openSelectedChoiceNum = progress.SelectedChoiceNum;

        DMConversationRunner runner = dmChatPanel.GetComponent<DMConversationRunner>();

        if (runner == null)
        {
            Log.Message("DMChatPanel에 DMConversationRunner가 없습니다.");
            return;
        }

        runner.OnProgressChanged = OnDMProgressChanged;
        runner.OnQuestDMCompleted = OnQuestDMCompleted;

        if (progress.ProgressState == (int)DMProgressState.Unread)
        {
            if (HasChoice(dmData.messageId))
            {
                progress.ProgressState = (int)DMProgressState.WaitingChoice;
                progress.PreviewText = GetChoicePointPreviewText(dmData.messageId);
            }
            else
            {
                progress.ProgressState = (int)DMProgressState.Completed;
                progress.PreviewText = GetCompletedPreviewText(
                    dmData.messageId,
                    progress.SelectedChoiceNum
                );
            }

            SaveLocalDMProgress(
                dmData.messageId,
                progress.ProgressState,
                progress.SelectedChoiceNum
            );
        }

        runner.OpenNpcDM(
            dmData,
            openProgressState,
            openSelectedChoiceNum
        );
    }

    ///<summary>
    /// 테스트용 Quest DM을 로컬 목록에 생성합니다.
    ///</summary>
    public void TestGenerateQuestDM()
    {
        TryGenerateQuestDMLocal();
    }

    ///<summary>
    /// Quest DM 생성 조건을 확인한 뒤 로컬 목록에 Quest DM을 추가합니다.
    ///</summary>
    public void TryGenerateQuestDMLocal()
    {
        int currentQuestCount = GetCurrentQuestDMCount();

        if (currentQuestCount >= maxQuestDMCount)
        {
            Log.Message("Quest DM 개수가 최대치입니다.");
            return;
        }

        DateTime now = DateTime.Now;

        if (!ignoreQuestGenerateTimeForTest)
        {
            if (dmGenerationTimestamp == default)
            {
                dmGenerationTimestamp = now;
                Log.Message("DM 퀘스트 생성 타이머 시작");
                return;
            }

            TimeSpan elapsedTime = now - dmGenerationTimestamp;

            if (elapsedTime.TotalHours < questGenerateHour)
            {
                Log.Message("DM 퀘스트 생성 시간이 부족합니다.");
                return;
            }
        }

        DM_TableSO targetDM = GetGenerateTargetQuestDM();

        if (targetDM == null)
        {
            Log.Message("생성 가능한 Quest DM이 없습니다.");
            return;
        }

        if (TryConnectSameNpcDM(targetDM))
        {
            dmGenerationTimestamp = default;
            RefreshDMList();
            return;
        }

        DMLocalProgress progress = new DMLocalProgress
        {
            DM_ID = targetDM.messageId,
            DMType = (int)DMTypeEnum.Quest,
            SentTime = now,
            ProgressState = (int)DMProgressState.Unread,
            SelectedChoiceNum = -1,
            QuestRewardState = (int)QuestRewardStateEnum.None,
            PreviewText = ""
        };

        dmProgressTable.Add(targetDM.messageId, progress);
        dmGenerationTimestamp = default;

        Log.Message($"Quest DM 로컬 생성 : {targetDM.messageId}");

        RefreshDMList();
    }

    ///<summary>
    /// 대화창에서 DM 목록으로 돌아갑니다.
    ///</summary>
    public void BackToDMList()
    {
        if (dmChatPanel == null || dmListPanel == null)
        {
            Log.Message("DM 패널이 연결되지 않았습니다.");
            return;
        }

        DMConversationRunner runner = dmChatPanel.GetComponent<DMConversationRunner>();

        if (runner != null)
            runner.StopConversation();

        dmChatPanel.SetActive(false);
        dmListPanel.SetActive(true);

        RefreshDMList();
    }

    ///<summary>
    /// 로컬 DMProgress에서 특정 DM을 삭제하고 목록을 갱신합니다.
    ///</summary>
    public void RemoveDMProgress(int messageId)
    {
        if (!dmProgressTable.ContainsKey(messageId))
        {
            Log.Message($"삭제할 로컬 DMProgress가 없습니다 : {messageId}");
            return;
        }

        dmProgressTable.Remove(messageId);

        Log.Message($"로컬 DMProgress 삭제 완료 : {messageId}");

        CreateDummyDMProgress();
        RefreshDMList();
    }

    private void OnDMProgressChanged(int messageId, int progressState, int selectedChoiceNum, string previewText)
    {
        DMLocalProgress progress = GetProgress(messageId);

        progress.ProgressState = progressState;
        progress.SelectedChoiceNum = selectedChoiceNum;

        if (progressState == (int)DMProgressState.WaitingChoice)
        {
            progress.PreviewText = GetChoicePointPreviewText(messageId);
        }
        else if (progressState == (int)DMProgressState.Completed)
        {
            progress.PreviewText = GetCompletedPreviewText(messageId, selectedChoiceNum);
        }
        else if (!string.IsNullOrEmpty(previewText))
        {
            progress.PreviewText = previewText;
        }

        SaveLocalDMProgress(
            messageId,
            progress.ProgressState,
            progress.SelectedChoiceNum
        );
    }

    private void OnQuestDMCompleted(int messageId)
    {
        DMLocalProgress progress = GetProgress(messageId);

        if (progress.QuestRewardState == (int)QuestRewardStateEnum.RewardMessagePrinted)
        {
            Log.Message($"이미 완료 처리된 Quest DM입니다 : {messageId}");
            return;
        }

        GiveQuestReward(messageId);

        progress.QuestRewardState = (int)QuestRewardStateEnum.RewardMessagePrinted;

        Log.Message($"Quest DM 로컬 완료 처리 : {messageId}");

        if (dmChatPanel != null)
            dmChatPanel.SetActive(false);

        if (dmListPanel != null)
            dmListPanel.SetActive(true);

        RemoveDMProgress(messageId);
    }

    private void SaveLocalDMProgress(int messageId, int progressState, int selectedChoiceNum)
    {
        DMLocalProgress progress = GetProgress(messageId);
        DM_TableSO dmData = GetDMTable(messageId);

        progress.DM_ID = messageId;
        progress.ProgressState = progressState;
        progress.SelectedChoiceNum = selectedChoiceNum;

        if (dmData != null && dmData.dmQuestType == DMQuestTypeEnum.Dummy)
            progress.DMType = (int)DMTypeEnum.Dummy;
        else
            progress.DMType = (int)DMTypeEnum.Quest;

        Log.Message(
            $"[로컬 DMProgress 저장] DM_ID:{messageId}, Type:{progress.DMType}, State:{progress.ProgressState}, Choice:{progress.SelectedChoiceNum}"
        );
    }

    private DMLocalProgress GetProgress(int messageId)
    {
        if (!dmProgressTable.TryGetValue(messageId, out DMLocalProgress progress))
        {
            DM_TableSO dmData = GetDMTable(messageId);

            progress = new DMLocalProgress
            {
                DM_ID = messageId,
                DMType = dmData != null && dmData.dmQuestType == DMQuestTypeEnum.Dummy
                    ? (int)DMTypeEnum.Dummy
                    : (int)DMTypeEnum.Quest,
                SentTime = dmData != null && dmData.dmQuestType == DMQuestTypeEnum.Dummy
                    ? DateTime.MinValue
                    : DateTime.Now,
                ProgressState = (int)DMProgressState.Unread,
                SelectedChoiceNum = -1,
                QuestRewardState = (int)QuestRewardStateEnum.None,
                PreviewText = ""
            };

            dmProgressTable.Add(messageId, progress);
        }

        return progress;
    }

    private int GetCurrentQuestDMCount()
    {
        int count = 0;

        foreach (DMLocalProgress progress in dmProgressTable.Values)
        {
            if (progress == null)
                continue;

            DM_TableSO dmData = GetDMTable(progress.DM_ID);

            if (dmData == null)
                continue;

            if (dmData.dmQuestType != DMQuestTypeEnum.Dummy)
                count++;
        }

        return count;
    }

    private DM_TableSO GetGenerateTargetQuestDM()
    {
        if (dmTables == null)
            return null;

        foreach (DM_TableSO dm in dmTables)
        {
            if (dm == null)
                continue;

            if (dm.dmQuestType == DMQuestTypeEnum.Dummy)
                continue;

            if (dmProgressTable.ContainsKey(dm.messageId))
                continue;

            return dm;
        }

        return null;
    }

    private bool TryConnectSameNpcDM(DM_TableSO targetDM)
    {
        int removeMessageId = 0;
        DMLocalProgress connectProgress = null;

        foreach (DMLocalProgress progress in dmProgressTable.Values)
        {
            if (progress == null)
                continue;

            DM_TableSO savedDM = GetDMTable(progress.DM_ID);

            if (savedDM == null)
                continue;

            if (savedDM.senderName != targetDM.senderName)
                continue;

            if (savedDM.dmQuestType != DMQuestTypeEnum.Dummy)
                continue;

            removeMessageId = savedDM.messageId;
            connectProgress = progress;
            break;
        }

        if (connectProgress == null)
            return false;

        dmProgressTable.Remove(removeMessageId);

        connectProgress.DM_ID = targetDM.messageId;
        connectProgress.DMType = (int)DMTypeEnum.Quest;
        connectProgress.SentTime = DateTime.Now;
        connectProgress.ProgressState = (int)DMProgressState.Unread;
        connectProgress.SelectedChoiceNum = -1;
        connectProgress.QuestRewardState = (int)QuestRewardStateEnum.None;
        connectProgress.PreviewText = "";

        dmProgressTable.Add(targetDM.messageId, connectProgress);

        Log.Message($"같은 NPC Dummy DM을 Quest DM으로 연결 : {removeMessageId} -> {targetDM.messageId}");
        return true;
    }

    private void GiveQuestReward(int messageId)
    {
        Log.Message($"Quest 보상 지급 준비 : {messageId}");

        // TODO:
        // Request_TableSO 조회
        // reqItemId / reqItemCount 검사
        // rewardFollower 지급
    }

    private DM_TableSO GetDMTable(int dmId)
    {
        if (dmTables == null)
            return null;

        foreach (DM_TableSO dm in dmTables)
        {
            if (dm == null)
                continue;

            if (dm.messageId == dmId)
                return dm;
        }

        return null;
    }

    private int GetDMTableIndex(int messageId)
    {
        if (dmTables == null)
            return int.MaxValue;

        for (int i = 0; i < dmTables.Length; i++)
        {
            if (dmTables[i] == null)
                continue;

            if (dmTables[i].messageId == messageId)
                return i;
        }

        return int.MaxValue;
    }

    private string GetPreviewText(int messageId, string npcAccountName, DMLocalProgress progress)
    {
        if (progress.ProgressState == (int)DMProgressState.Unread)
            return $"{npcAccountName}님이 메시지를 보내고 싶어합니다";

        if (!string.IsNullOrEmpty(progress.PreviewText))
            return progress.PreviewText;

        if (progress.ProgressState == (int)DMProgressState.WaitingChoice)
            return GetChoicePointPreviewText(messageId);

        if (progress.ProgressState == (int)DMProgressState.Completed)
            return GetCompletedPreviewText(messageId, progress.SelectedChoiceNum);

        return "";
    }

    private string GetChoicePointPreviewText(int messageId)
    {
        foreach (Dialogue_TableSO dialogue in dialogueSOs)
        {
            if (dialogue == null)
                continue;

            if (dialogue.messageId != messageId)
                continue;

            if (dialogue.choiceGroupId != 0)
                return GetString(dialogue.dialogText);
        }

        return "";
    }

    private string GetCompletedPreviewText(int messageId, int selectedChoiceNum)
    {
        int currentDialogId = GetStartDialogId(messageId);
        string lastText = "";

        while (currentDialogId != 0)
        {
            Dialogue_TableSO dialogue = GetDialogue(currentDialogId);

            if (dialogue == null)
                break;

            lastText = GetString(dialogue.dialogText);

            if (dialogue.choiceGroupId != 0)
            {
                Choice_TableSO selectedChoice =
                    GetChoiceByChoiceNum(dialogue.choiceGroupId, selectedChoiceNum);

                if (selectedChoice == null)
                    break;

                lastText = GetString(selectedChoice.choiceText);
                currentDialogId = selectedChoice.nextDialogId;
                continue;
            }

            if (dialogue.isEnd)
                break;

            currentDialogId = dialogue.nextDialogId;
        }

        return lastText;
    }

    private int GetStartDialogId(int messageId)
    {
        int startDialogId = 0;

        foreach (Dialogue_TableSO dialogue in dialogueSOs)
        {
            if (dialogue == null)
                continue;

            if (dialogue.messageId != messageId)
                continue;

            if (startDialogId == 0 || dialogue.dialogId < startDialogId)
                startDialogId = dialogue.dialogId;
        }

        return startDialogId;
    }

    private Dialogue_TableSO GetDialogue(int dialogId)
    {
        foreach (Dialogue_TableSO dialogue in dialogueSOs)
        {
            if (dialogue == null)
                continue;

            if (dialogue.dialogId == dialogId)
                return dialogue;
        }

        return null;
    }

    private Choice_TableSO GetChoiceByChoiceNum(int choiceGroupId, int choiceNum)
    {
        if (choiceSOs == null)
            return null;

        foreach (Choice_TableSO choice in choiceSOs)
        {
            if (choice == null)
                continue;

            if (choice.choiceGroupId == choiceGroupId && choice.choiceNum == choiceNum)
                return choice;
        }

        return null;
    }

    private string GetNpcAccountName(int npcId)
    {
        if (npcSOs == null)
        {
            Log.Message("Npc_TableSO 배열이 연결되지 않았습니다.");
            return "Unknown";
        }

        foreach (Npc_TableSO npc in npcSOs)
        {
            if (npc == null)
                continue;

            if (npc.npcId == npcId)
                return npc.npcAccountName;
        }

        Log.Message($"Npc ID를 찾을 수 없습니다 : {npcId}");
        return "Unknown";
    }

    private string GetString(string stringKey)
    {
        if (stringManager == null)
        {
            Log.Message("String_TableManager를 찾을 수 없습니다.");
            return stringKey;
        }

        string text = stringManager.GetString(stringKey, SystemLanguage.Korean);

        if (string.IsNullOrEmpty(text))
        {
            Log.Message($"String Key를 찾을 수 없습니다 : {stringKey}");
            return stringKey;
        }

        return text;
    }

    private void RefreshDMList()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        CreateDMList();
    }

    private bool HasChoice(int messageId)
    {
        foreach (Dialogue_TableSO dialogue in dialogueSOs)
        {
            if (dialogue == null)
                continue;

            if (dialogue.messageId != messageId)
                continue;

            if (dialogue.choiceGroupId != 0)
                return true;
        }

        return false;
    }

    private string GetNpcProfileImageKey(int npcId)
    {
        if (npcSOs == null)
        {
            Log.Message("Npc_TableSO 배열이 연결되지 않았습니다.");
            return "";
        }

        foreach (Npc_TableSO npc in npcSOs)
        {
            if (npc == null)
                continue;

            if (npc.npcId == npcId)
                return npc.npcImage.ToString();
        }

        Log.Message($"Npc Profile Image를 찾을 수 없습니다 : {npcId}");
        return "";
    }
}
