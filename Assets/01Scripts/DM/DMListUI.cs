/*
작성자 : 이종현
작성일 : 26-06-01
수정일 : 26-06-10

역할 : DM 목록 UI 생성 및 DM 클릭 시 대화창 전환
방식 : 읽음 처리용 DialogId, 미리보기 표시용 DialogId, 선택지 진행 정보를 분리하여 NPC 이벤트형 DM을 관리
*/

using UnityEngine;
using System.Collections.Generic;

public class DMListUI : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject dmListItemPrefab;

    [Header("Panels")]
    [SerializeField] private GameObject dmListPanel;
    [SerializeField] private GameObject dmChatPanel;

    [Header("DM Data")]
    [SerializeField] private DM_TableSO[] dmTables;
    [SerializeField] private Dialogue_TableSO[] dialogueSOs;
    [SerializeField] private Npc_TableSO[] npcSOs;

    private IString_TableManager stringManager;

    // 읽음 처리용
    private readonly Dictionary<int, int> lastReadDialogIdTable = new();

    // 목록 미리보기 및 대화 이어보기용
    private readonly Dictionary<int, int> lastPreviewDialogIdTable = new();

    // 선택지 복원용
    private readonly Dictionary<int, int> selectedChoiceNextDialogIdTable = new();
    private readonly Dictionary<int, string> selectedChoiceTextTable = new();

    private readonly Dictionary<int, string> lastPreviewTextTable = new();
    
    private void Start()
    {
        stringManager = ServiceLocator.Get<IString_TableManager>();

        if (stringManager == null)
            Log.Message("String_TableManager를 찾을 수 없습니다.");

        if (dmChatPanel != null)
            dmChatPanel.SetActive(false);

        CreateDMList();
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

        foreach (DM_TableSO dm in dmTables)
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

            string npcAccountName = GetNpcAccountName(dm.senderName);

            int lastReadDialogId = 0;
            lastReadDialogIdTable.TryGetValue(dm.messageId, out lastReadDialogId);

            int nextEndDialogId = GetNextEndDialogId(dm.messageId, lastReadDialogId);
            bool hasUnread = nextEndDialogId != 0;

            string previewText = GetListPreviewText(dm.messageId, npcAccountName, hasUnread);

            itemUI.SetData(
                npcAccountName,
                previewText,
                hasUnread,
                () => OnClickDM(dm)
            );
        }
    }

    private string GetListPreviewText(int messageId, string npcAccountName, bool hasUnread)
    {
        if (lastPreviewTextTable.TryGetValue(messageId, out string savedPreviewText))
            return savedPreviewText;

        int previewDialogId = 0;
        lastPreviewDialogIdTable.TryGetValue(messageId, out previewDialogId);

        if (previewDialogId != 0)
            return GetPreviewTextByDialogId(previewDialogId);

        if (hasUnread)
            return $"{npcAccountName}님이 메시지를 보내고 싶어합니다";

        return "";
    }

    private void OnClickDM(DM_TableSO dmData)
    {
        if (dmData == null)
        {
            Log.Message("선택된 DM 데이터가 없습니다.");
            return;
        }

        if (dmListPanel != null)
            dmListPanel.SetActive(false);

        if (dmChatPanel != null)
            dmChatPanel.SetActive(true);

        int lastReadDialogId = 0;
        lastReadDialogIdTable.TryGetValue(dmData.messageId, out lastReadDialogId);

        int nextEndDialogId = GetNextEndDialogId(dmData.messageId, lastReadDialogId);

        int lastPreviewDialogId = 0;
        lastPreviewDialogIdTable.TryGetValue(dmData.messageId, out lastPreviewDialogId);

        DMConversationRunner runner = dmChatPanel.GetComponent<DMConversationRunner>();

        if (runner == null)
        {
            Log.Message("DMChatPanel에 DMConversationRunner가 없습니다.");
            return;
        }

        runner.SetChoiceSaveData(
            selectedChoiceNextDialogIdTable,
            selectedChoiceTextTable
        );

        runner.OpenNpcDM(dmData, lastPreviewDialogId);

        if (nextEndDialogId != 0)
            lastReadDialogIdTable[dmData.messageId] = nextEndDialogId;
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
        {
            runner.StopConversation();

            if (runner.CurrentMessageId != 0 && runner.LastProgressDialogId != 0)
            {
                lastPreviewDialogIdTable[runner.CurrentMessageId] =
                    runner.LastProgressDialogId;
            }
        }
        
        if (runner.CurrentMessageId != 0)
        {
            if (!string.IsNullOrEmpty(runner.LastPreviewText))
            {
                lastPreviewTextTable[runner.CurrentMessageId] = runner.LastPreviewText;
            }

            if (runner.LastProgressDialogId != 0)
            {
                lastPreviewDialogIdTable[runner.CurrentMessageId] =
                    runner.LastProgressDialogId;
            }
        }

        dmChatPanel.SetActive(false);
        dmListPanel.SetActive(true);

        RefreshDMList();
    }

    private void RefreshDMList()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        CreateDMList();
    }

    private int GetNextEndDialogId(int messageId, int lastReadDialogId)
    {
        int nextEndDialogId = 0;

        foreach (Dialogue_TableSO dialogue in dialogueSOs)
        {
            if (dialogue == null)
                continue;

            if (dialogue.messageId != messageId)
                continue;

            if (dialogue.dialogId <= lastReadDialogId)
                continue;

            if (!dialogue.isEnd)
                continue;

            if (nextEndDialogId == 0 || dialogue.dialogId < nextEndDialogId)
                nextEndDialogId = dialogue.dialogId;
        }

        return nextEndDialogId;
    }

    private string GetPreviewTextByDialogId(int dialogId)
    {
        foreach (Dialogue_TableSO dialogue in dialogueSOs)
        {
            if (dialogue == null)
                continue;

            if (dialogue.dialogId == dialogId)
                return GetString(dialogue.dialogText);
        }

        return "";
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
}