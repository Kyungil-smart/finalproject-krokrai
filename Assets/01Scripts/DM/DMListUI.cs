/*
작성자 : 이종현
작성일 : 26-06-01
수정일 : 26-06-22

역할 : DM 목록 UI 생성 및 DM 클릭 시 대화창 전환
방식 : DB 저장과 로컬 Dictionary 기준으로 Dummy DM과 Quest DM을 생성, 진행, 완료, 삭제 처리
*/

using System;
using TMPro;
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

    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _dmListItemPrefab;

    [Header("Panels")]
    [SerializeField] private GameObject _dmListPanel;
    [SerializeField] private GameObject _dmChatPanel;

    [Header("DM Data")]
    [SerializeField] private DM_TableSO[] _dmTables;
    [SerializeField] private Dialogue_TableSO[] _dialogueSOs;
    [SerializeField] private Choice_TableSO[] _choiceSOs;
    [SerializeField] private Npc_TableSO[] _npcSOs;

    [Header("Local Quest Setting")]
    [SerializeField] private int _maxQuestDMCount = 3;
    [SerializeField] private double _questGenerateHour = 8;
    
    [Header("Unread Badge")]
    [SerializeField] private GameObject[] _unreadBadgeObjects;
    [SerializeField] private TMP_Text[] _unreadCountTexts;

    [SerializeField] private DMQuestHomeFeedPostLoad _post;


    private IString_TableManager _stringManager;

    private readonly Dictionary<int, DMLocalProgress> _dmProgressTable = new();
    
    private float _questGenerateCheckTimer;
    
    private void Start()
    {
        _stringManager = ServiceLocator.Get<IString_TableManager>();

        if (_stringManager == null)
            Log.Message("String_TableManager를 찾을 수 없습니다.");

        if (_dmChatPanel != null)
            _dmChatPanel.SetActive(false);

        InitLocalProgressData();

        LoadDMProgressFromDB();

        CheckQuestGenerateDelay();

        RefreshDMList();

        UpdateUnreadBadge();
    }
    
    private void Update()
    {
        _questGenerateCheckTimer += Time.deltaTime;

        if (_questGenerateCheckTimer < 1f)
            return;

        _questGenerateCheckTimer = 0f;
        CheckQuestGenerateDelay();
    }

    private void InitLocalProgressData()
    {
        _dmProgressTable.Clear();
        CreateDummyDMProgress();
    }
    
    ///<summary>
    /// 테스트용 Quest DM을 최대 개수까지 랜덤 생성합니다.
    ///</summary>
    public void FillQuestDMsForTest()
    {
        while (GetCurrentQuestDMCount() < _maxQuestDMCount)
        {
            DM_TableSO targetDM = GetRandomGenerateTargetQuestDM();

            if (targetDM == null)
            {
                Log.Message("생성 가능한 Quest DM이 없습니다.");
                break;
            }

            CreateQuestProgress(targetDM);
        }

        RefreshDMList();
    }
    
    private void CreateQuestProgress(DM_TableSO targetDM)
    {
        if (targetDM == null)
            return;

        DMLocalProgress progress = new DMLocalProgress
        {
            DM_ID = targetDM.messageId,
            DMType = (int)DMTypeEnum.Quest,
            SentTime = DateTime.Now,
            ProgressState = (int)DMProgressState.Unread,
            SelectedChoiceNum = -1,
            QuestRewardState = (int)QuestRewardStateEnum.None,
            PreviewText = ""
        };
        
        _dmProgressTable.Add(targetDM.messageId, progress);

        SaveLocalDMProgress(
            targetDM.messageId,
            progress.ProgressState,
            progress.SelectedChoiceNum
        );
        
        UpdateUnreadBadge();
    }
    
    private DM_TableSO GetRandomGenerateTargetQuestDM()
    {
        List<DM_TableSO> candidates = new List<DM_TableSO>();

        foreach (DM_TableSO dm in _dmTables)
        {
            if (dm == null)
                continue;

            if (dm.dmQuestType == DMQuestTypeEnum.Dummy)
                continue;

            if (_dmProgressTable.ContainsKey(dm.messageId))
                continue;

            candidates.Add(dm);
        }

        if (candidates.Count == 0)
            return null;

        int randomIndex = UnityEngine.Random.Range(0, candidates.Count);
        return candidates[randomIndex];
    }

    private void CreateDummyDMProgress()
    {
        if (_dmTables == null)
        {
            Log.Message("DM_TableSO 배열이 연결되지 않았습니다.");
            return;
        }

        foreach (DM_TableSO dm in _dmTables)
        {
            if (dm == null)
                continue;

            if (dm.dmQuestType != DMQuestTypeEnum.Dummy)
                continue;

            if (_dmProgressTable.ContainsKey(dm.messageId))
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

            _dmProgressTable.Add(dm.messageId, progress);
        }
    }

    private void CreateDMList()
    {
        if (_content == null || _dmListItemPrefab == null)
        {
            Log.Message("Content 또는 DMListItemPrefab이 연결되지 않았습니다.");
            return;
        }

        List<DMLocalProgress> progressList = new List<DMLocalProgress>(_dmProgressTable.Values);

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

            GameObject item = Instantiate(_dmListItemPrefab, _content);

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

        DMChatUI chatUI = _dmChatPanel.GetComponentInChildren<DMChatUI>();
        if (chatUI != null)
            chatUI.SetOpponentProfileImageKey(profileImageKey);

        if (_dmListPanel != null)
            _dmListPanel.SetActive(false);

        if (_dmChatPanel != null)
            _dmChatPanel.SetActive(true);

        DMLocalProgress progress = GetProgress(dmData.messageId);

        int openProgressState = progress.ProgressState;
        int openSelectedChoiceNum = progress.SelectedChoiceNum;

        DMConversationRunner runner = _dmChatPanel.GetComponent<DMConversationRunner>();

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
    /// Quest DM 생성 조건을 확인한 뒤 로컬 목록에 Quest DM을 1개 추가합니다.
    ///</summary>
    public void TryGenerateQuestDMLocal()
    {
        int currentQuestCount = GetCurrentQuestDMCount();

        if (currentQuestCount >= _maxQuestDMCount)
        {
            Log.Message("Quest DM 개수가 최대치입니다.");
            return;
        }

        DM_TableSO targetDM = GetRandomGenerateTargetQuestDM();

        if (targetDM == null)
        {
            Log.Message("생성 가능한 Quest DM이 없습니다.");
            return;
        }

        CreateQuestProgress(targetDM);
        RefreshDMList();
    }

    public void BackToDMList()
    {
        if (_dmChatPanel == null || _dmListPanel == null)
        {
            Log.Message("DM 패널이 연결되지 않았습니다.");
            return;
        }

        DMConversationRunner runner = _dmChatPanel.GetComponent<DMConversationRunner>();

        if (runner != null)
            runner.StopConversation();

        _dmChatPanel.SetActive(false);
        _dmListPanel.SetActive(true);

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

    private void OnQuestDMCompleted(int messageId, int feedPostId)
    {
        DMLocalProgress progress = GetProgress(messageId);

        if (progress.QuestRewardState == (int)QuestRewardStateEnum.RewardMessagePrinted)
            return;

        GiveQuestReward(messageId);

        if (feedPostId != 0)
            SendAfterStoryFeedToHome(feedPostId);

        progress.QuestRewardState = (int)QuestRewardStateEnum.RewardMessagePrinted;

        SaveLocalDMProgress(
            messageId,
            progress.ProgressState,
            progress.SelectedChoiceNum
        );

        SaveDMGenerationTimestamp();

        RemoveQuestDMFromListAndDB(messageId);
    }
    
    private void SendAfterStoryFeedToHome(int feedPostId)
    {
        _post.GetPost(feedPostId);
    }
    
    ///<summary>
    /// 완료된 Quest DM을 목록과 DB에서 제거합니다.
    ///</summary>
    private void RemoveQuestDMFromListAndDB(int messageId)
    {
        _dmProgressTable.Remove(messageId);

        IDataManager dataManager = ServiceLocator.Get<IDataManager>();

        if (dataManager == null || dataManager.UserDatas == null)
        {
            Log.Message("DMProgress 삭제 실패 : DataManager 또는 UserDatas가 없습니다.");
            return;
        }

        UserDatas userDatas = dataManager.UserDatas;

        if (userDatas.DMProgress != null)
            userDatas.DMProgress.Remove(messageId.ToString());

        dataManager.SaveData();

        Log.Message($"DMProgress 삭제 저장 완료 : {messageId}");
        
        UpdateUnreadBadge();
    }
    
    private void SaveDMGenerationTimestamp()
    {
        DateTime savedTimestamp = GetDMGenerationTimestamp();

        if (savedTimestamp != default)
            return;

        SetDMGenerationTimestamp(DateTime.Now);
    }
    
    public void CheckQuestGenerateDelay()
    {
        int currentQuestCount = GetCurrentQuestDMCount();

        if (currentQuestCount >= _maxQuestDMCount)
        {
            SetDMGenerationTimestamp(default);
            return;
        }

        DateTime now = DateTime.Now;
        DateTime savedTimestamp = GetDMGenerationTimestamp();

        if (savedTimestamp == default)
        {
            SetDMGenerationTimestamp(now);
            return;
        }

        TimeSpan elapsedTime = now - savedTimestamp;

        int generateCount = Mathf.FloorToInt(
            (float)(elapsedTime.TotalHours / _questGenerateHour)
        );

        if (generateCount <= 0)
            return;

        for (int i = 0; i < generateCount; i++)
        {
            if (GetCurrentQuestDMCount() >= _maxQuestDMCount)
                break;

            TryGenerateQuestDMLocal();
        }

        if (GetCurrentQuestDMCount() >= _maxQuestDMCount)
        {
            SetDMGenerationTimestamp(default);
            return;
        }

        double remainHours = elapsedTime.TotalHours % _questGenerateHour;
        DateTime nextTimestamp = now.AddHours(-remainHours);

        SetDMGenerationTimestamp(nextTimestamp);
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

        IDataManager dataManager = ServiceLocator.Get<IDataManager>();

        if (dataManager == null || dataManager.UserDatas == null)
        {
            Log.Message("DMProgress 저장 실패 : DataManager 또는 UserDatas가 없습니다.");
            return;
        }

        UserDatas userDatas = dataManager.UserDatas;

        if (userDatas.DMProgress == null)
            userDatas.DMProgress = new Dictionary<string, DMProgress>();

        DMProgress saveData = new DMProgress();

        saveData.DM_ID = progress.DM_ID;
        saveData.DMType = progress.DMType;
        saveData.ProgressState = progress.ProgressState;
        saveData.SelectedChoiceNum = progress.SelectedChoiceNum;
        saveData.QuestRewardState = progress.QuestRewardState;
        saveData.SentTime = progress.SentTime;

        userDatas.DMProgress[messageId.ToString()] = saveData;

        dataManager.SaveData();

        Log.Message($"DMProgress 저장 완료 : {messageId}");
        
        UpdateUnreadBadge();
    }

    private DMLocalProgress GetProgress(int messageId)
    {
        if (!_dmProgressTable.TryGetValue(messageId, out DMLocalProgress progress))
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

            _dmProgressTable.Add(messageId, progress);
        }

        return progress;
    }

    private int GetCurrentQuestDMCount()
    {
        int count = 0;

        foreach (DMLocalProgress progress in _dmProgressTable.Values)
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

    private void GiveQuestReward(int messageId)
    {
        int rewardFollower = GetRewardFollower(messageId);

        if (rewardFollower <= 0)
            return;

        IDataManager dataManager = ServiceLocator.Get<IDataManager>();

        if (dataManager == null || dataManager.UserDatas == null)
        {
            Log.Message("팔로워 보상 지급 실패 : DataManager 또는 UserDatas가 없습니다.");
            return;
        }

        UserDatas userDatas = dataManager.UserDatas;

        if (userDatas.Profile == null)
            userDatas.Profile = new ProFile();

        userDatas.Profile.followerCount += rewardFollower;

        dataManager.SaveData();

        Log.Message($"팔로워 보상 지급 완료 : +{rewardFollower}");
    }
    
    private int GetRewardFollower(int messageId)
    {
        foreach (Dialogue_TableSO dialogue in _dialogueSOs)
        {
            if (dialogue == null)
                continue;

            if (dialogue.messageId != messageId)
                continue;

            if (!dialogue.isEnd)
                continue;

            return dialogue.rewardFollower;
        }

        return 0;
    }

    private DM_TableSO GetDMTable(int dmId)
    {
        if (_dmTables == null)
            return null;

        foreach (DM_TableSO dm in _dmTables)
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
        if (_dmTables == null)
            return int.MaxValue;

        for (int i = 0; i < _dmTables.Length; i++)
        {
            if (_dmTables[i] == null)
                continue;

            if (_dmTables[i].messageId == messageId)
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
        foreach (Dialogue_TableSO dialogue in _dialogueSOs)
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

        foreach (Dialogue_TableSO dialogue in _dialogueSOs)
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
        foreach (Dialogue_TableSO dialogue in _dialogueSOs)
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
        if (_choiceSOs == null)
            return null;

        foreach (Choice_TableSO choice in _choiceSOs)
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
        if (_npcSOs == null)
        {
            Log.Message("Npc_TableSO 배열이 연결되지 않았습니다.");
            return "Unknown";
        }

        foreach (Npc_TableSO npc in _npcSOs)
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
        if (_stringManager == null)
        {
            Log.Message("String_TableManager를 찾을 수 없습니다.");
            return stringKey;
        }

        string text = _stringManager.GetString(stringKey, SystemLanguage.Korean);

        if (string.IsNullOrEmpty(text))
        {
            Log.Message($"String Key를 찾을 수 없습니다 : {stringKey}");
            return stringKey;
        }

        return text;
    }

    private void RefreshDMList()
    {
        foreach (Transform child in _content)
        {
            Destroy(child.gameObject);
        }

        CreateDMList();
    }

    private bool HasChoice(int messageId)
    {
        foreach (Dialogue_TableSO dialogue in _dialogueSOs)
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
        if (_npcSOs == null)
        {
            Log.Message("Npc_TableSO 배열이 연결되지 않았습니다.");
            return "";
        }

        foreach (Npc_TableSO npc in _npcSOs)
        {
            if (npc == null)
                continue;

            if (npc.npcId == npcId)
                return npc.npcImage.ToString();
        }

        Log.Message($"Npc Profile Image를 찾을 수 없습니다 : {npcId}");
        return "";
    }
    
    ///<summary>
    /// Firestore에 저장된 DMProgress를 로컬 Dictionary로 복원합니다.
    ///</summary>
    private void LoadDMProgressFromDB()
    {
        IDataManager dataManager = ServiceLocator.Get<IDataManager>();

        if (dataManager == null || dataManager.UserDatas == null)
        {
            Log.Message("DM DB 접근 실패 : DataManager 또는 UserDatas가 없습니다.");
            return;
        }

        UserDatas userDatas = dataManager.UserDatas;

        if (userDatas.DMProgress == null)
        {
            Log.Message("저장된 DMProgress가 없습니다.");
            return;
        }

        List<string> deleteKeys = new List<string>();

        foreach (var pair in userDatas.DMProgress)
        {
            DMProgress saveData = pair.Value;

            if (saveData == null)
                continue;

            if (saveData.QuestRewardState == (int)QuestRewardStateEnum.RewardMessagePrinted)
            {
                deleteKeys.Add(pair.Key);
                continue;
            }

            DMLocalProgress progress = new DMLocalProgress
            {
                DM_ID = saveData.DM_ID,
                DMType = saveData.DMType,
                SentTime = saveData.SentTime,
                ProgressState = saveData.ProgressState,
                SelectedChoiceNum = saveData.SelectedChoiceNum,
                QuestRewardState = saveData.QuestRewardState,
                PreviewText = ""
            };

            _dmProgressTable[progress.DM_ID] = progress;
        }

        foreach (string key in deleteKeys)
        {
            userDatas.DMProgress.Remove(key);
        }

        if (deleteKeys.Count > 0)
        {
            dataManager.SaveData();
            Log.Message($"완료된 DMProgress 삭제 저장 완료 : {deleteKeys.Count}개");
        }

        Log.Message($"DMProgress 복원 완료 : {userDatas.DMProgress.Count}");
        
        UpdateUnreadBadge();
    }
    
    private DateTime GetDMGenerationTimestamp()
    {
        IDataManager dataManager = ServiceLocator.Get<IDataManager>();

        if (dataManager == null || dataManager.UserDatas == null)
            return default;

        UserDatas userDatas = dataManager.UserDatas;

        if (userDatas.DMQuest == null)
            userDatas.DMQuest = new DMQuestData();

        return userDatas.DMQuest.DMGenerationTimestamp;
    }
    
    private void SetDMGenerationTimestamp(DateTime timestamp)
    {
        IDataManager dataManager = ServiceLocator.Get<IDataManager>();

        if (dataManager == null || dataManager.UserDatas == null)
        {
            Log.Message("DMGenerationTimestamp 저장 실패");
            return;
        }

        UserDatas userDatas = dataManager.UserDatas;

        if (userDatas.DMQuest == null)
            userDatas.DMQuest = new DMQuestData();

        userDatas.DMQuest.DMGenerationTimestamp = timestamp;

        dataManager.SaveData();

        Log.Message("DMGenerationTimestamp 저장 완료");
    }
    
    private int GetUnreadDMCount()
    {
        int count = 0;

        foreach (DMLocalProgress progress in _dmProgressTable.Values)
        {
            if (progress == null)
                continue;

            if (progress.ProgressState == (int)DMProgressState.Unread)
                count++;
        }

        return count;
    }
    
    private void UpdateUnreadBadge()
    {
        int unreadCount = GetUnreadDMCount();
        bool hasUnread = unreadCount > 0;

        if (_unreadBadgeObjects != null)
        {
            foreach (GameObject badgeObject in _unreadBadgeObjects)
            {
                if (badgeObject == null)
                    continue;

                badgeObject.SetActive(hasUnread);
            }
        }

        if (_unreadCountTexts != null)
        {
            foreach (TMPro.TMP_Text countText in _unreadCountTexts)
            {
                if (countText == null)
                    continue;

                countText.text = unreadCount.ToString();
            }
        }
    }
}
