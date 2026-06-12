/*
작성자 : 이종현
작성일 : 26-06-01
수정일 : 26-06-12

역할 : DM 목록 UI 생성 및 DM 클릭 시 대화창 전환
방식 : DMProgress의 ProgressState와 SelectedChoiceNum을 기준으로 NPC 이벤트형 DM 상태를 관리
*/

using System;
using UnityEngine;
using System.Collections.Generic;

public class DMListUI : MonoBehaviour
{
    private class DMLocalProgress
    {
        public int DM_ID;
        public int ProgressState;
        public int SelectedChoiceNum;
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

    private IString_TableManager stringManager;

    private readonly Dictionary<int, DMLocalProgress> dmProgressTable = new();

    private void Start()
    {
        stringManager = ServiceLocator.Get<IString_TableManager>();

        if (stringManager == null)
            Log.Message("String_TableManager를 찾을 수 없습니다.");

        if (dmChatPanel != null)
            dmChatPanel.SetActive(false);

        InitProgressData();
        CreateDMList();
    }

    private void InitProgressData()
    {
        if (dmTables == null)
            return;

        foreach (DM_TableSO dm in dmTables)
        {
            if (dm == null)
                continue;

            if (dmProgressTable.ContainsKey(dm.messageId))
                continue;

            int progressState = (int)DMProgressState.Unread;
            int selectedChoiceNum = -1;

            global::DMProgress savedProgress = GetSavedDMProgress(dm.messageId);

            if (savedProgress != null)
            {
                progressState = savedProgress.ProgressState;
                selectedChoiceNum = savedProgress.SelectedChoiceNum;
            }

            dmProgressTable.Add(dm.messageId, new DMLocalProgress
            {
                DM_ID = dm.messageId,
                ProgressState = progressState,
                SelectedChoiceNum = selectedChoiceNum,
                PreviewText = ""
            });
        }
    }

    private void CreateDMList()
    {
        if (content == null || dmListItemPrefab == null)
        {
            Log.Message("Content 또는 DMListItemPrefab이 연결되지 않았습니다.");
            return;
        }

        if (dmTables == null || dmTables.Length == 0)
        {
            Log.Message("DM_TableSO 데이터가 없습니다.");
            return;
        }

        List<DM_TableSO> sortedDMs = new(dmTables);

        IDataManager dataManager = ServiceLocator.Get<IDataManager>();

        sortedDMs.Sort((a, b) =>
        {
            DateTime aTime = DateTime.MinValue;
            DateTime bTime = DateTime.MinValue;

            string aKey = a.messageId.ToString();
            string bKey = b.messageId.ToString();

            if (dataManager != null &&
                dataManager.UserDatas != null &&
                dataManager.UserDatas.DMProgress.ContainsKey(aKey))
            {
                aTime = dataManager.UserDatas.DMProgress[aKey].SentTime;
            }

            if (dataManager != null &&
                dataManager.UserDatas != null &&
                dataManager.UserDatas.DMProgress.ContainsKey(bKey))
            {
                bTime = dataManager.UserDatas.DMProgress[bKey].SentTime;
            }

            return bTime.CompareTo(aTime);
        });

        foreach (DM_TableSO dm in sortedDMs)
        {
            if (dm == null)
                continue;

            GameObject item = Instantiate(dmListItemPrefab, content);

            DMListItemUI itemUI = item.GetComponentInChildren<DMListItemUI>();

            if (itemUI == null)
            {
                Log.Message("DMListItemUI를 찾을 수 없습니다.");
                continue;
            }

            DMLocalProgress progress = GetProgress(dm.messageId);
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

        // 대화창에 넘길 값은 클릭 전 상태로 보관
        int openProgressState = progress.ProgressState;
        int openSelectedChoiceNum = progress.SelectedChoiceNum;

        DMConversationRunner runner = dmChatPanel.GetComponent<DMConversationRunner>();

        if (runner == null)
        {
            Log.Message("DMChatPanel에 DMConversationRunner가 없습니다.");
            return;
        }

        runner.OnProgressChanged = OnDMProgressChanged;

        // 저장 상태만 클릭 후 상태로 먼저 변경
        // 단, OpenNpcDM에는 클릭 전 상태(openProgressState)를 넘겨 최초 연출은 유지
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

            SaveDMProgress(
                dmData.messageId,
                progress.ProgressState,
                progress.SelectedChoiceNum
            );
        }

        // 실제 이번 입장에서는 클릭 전 상태로 열어야 최초 연출이 유지됨
        runner.OpenNpcDM(
            dmData,
            openProgressState,
            openSelectedChoiceNum
        );
    }

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

        SaveDMProgress(
            messageId,
            progress.ProgressState,
            progress.SelectedChoiceNum
        );
    }
    
    private void SaveDMProgress(int messageId, int progressState, int selectedChoiceNum)
    {
        Log.Message(
            $"[DMProgress 저장 요청] DM_ID:{messageId}, State:{progressState}, Choice:{selectedChoiceNum}"
        );
        
        IDataManager dataManager = ServiceLocator.Get<IDataManager>();

        if (dataManager == null)
        {
            Log.Message("DataManager를 찾을 수 없습니다.");
            return;
        }

        if (dataManager.UserDatas == null)
        {
            Log.Message("UserDatas가 없습니다.");
            return;
        }

        string dmKey = messageId.ToString();

        if (!dataManager.UserDatas.DMProgress.ContainsKey(dmKey))
        {
            dataManager.UserDatas.DMProgress.Add(dmKey, new global::DMProgress());
            Log.Message($"[DMProgress 신규 생성] {dmKey}");
        }

        global::DMProgress progress = dataManager.UserDatas.DMProgress[dmKey];

        progress.ProgressState = progressState;
        progress.SelectedChoiceNum = selectedChoiceNum;

        Log.Message(
            $"[DMProgress 저장 확인] Key:{dmKey}, State:{progress.ProgressState}, Choice:{progress.SelectedChoiceNum}"
        );
    }

    private global::DMProgress GetSavedDMProgress(int messageId)
    {
        IDataManager dataManager = ServiceLocator.Get<IDataManager>();

        if (dataManager == null)
            return null;

        if (dataManager.UserDatas == null)
            return null;

        string dmKey = messageId.ToString();

        if (!dataManager.UserDatas.DMProgress.TryGetValue(dmKey, out global::DMProgress progress))
            return null;

        return progress;
    }

    private DMLocalProgress GetProgress(int messageId)
    {
        if (!dmProgressTable.TryGetValue(messageId, out DMLocalProgress progress))
        {
            progress = new DMLocalProgress
            {
                DM_ID = messageId,
                ProgressState = (int)DMProgressState.Unread,
                SelectedChoiceNum = -1,
                PreviewText = ""
            };

            dmProgressTable.Add(messageId, progress);
        }

        return progress;
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