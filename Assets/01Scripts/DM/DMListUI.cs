/*
작성자 : 이종현
작성일 : 26-06-01
수정일 : 26-06-01

역할 : DM 목록 UI 생성 및 DM 클릭 시 대화창 전환
방식 : DMTableSO 배열을 읽고 previewDialogId로 DialogueSO를 찾아 미리보기 텍스트를 표시
*/

using UnityEngine;

public class DMListUI : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject dmListItemPrefab;

    [Header("Panels")]
    [SerializeField] private GameObject dmListPanel;
    [SerializeField] private GameObject dmChatPanel;

    [Header("DM Data")]
    [SerializeField] private DMTableSO[] dmTables;
    [SerializeField] private DMDialogueSO[] dialogueSOs;

    private void Start()
    {
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
            Log.Message("DMTableSO 데이터가 없습니다.");
            return;
        }

        foreach (DMTableSO dm in dmTables)
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

            string previewText = GetPreviewText(dm.previewDialogId);

            itemUI.SetData(
                dm.senderName,
                previewText,
                dm.hasUnread,
                () => OnClickDM(dm)
            );
        }
    }

    private string GetPreviewText(int previewDialogId)
    {
        if (dialogueSOs == null)
            return "";

        foreach (DMDialogueSO dialogue in dialogueSOs)
        {
            if (dialogue == null)
                continue;

            if (dialogue.dialogId == previewDialogId)
                return dialogue.dialogText;
        }

        Log.Message($"Preview Dialogue ID를 찾을 수 없습니다 : {previewDialogId}");
        return "";
    }

    private void OnClickDM(DMTableSO dmData)
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

        DMConversationRunner runner =
            dmChatPanel.GetComponent<DMConversationRunner>();

        if (runner == null)
        {
            Log.Message("DMChatPanel에 DMConversationRunner가 없습니다.");
            return;
        }

        runner.OpenNpcDM(dmData);
    }
    
    public void BackToDMList()
    {
        DMConversationRunner runner =
            dmChatPanel.GetComponent<DMConversationRunner>();

        if (runner != null)
            runner.StopConversation();

        dmChatPanel.SetActive(false);
        dmListPanel.SetActive(true);
    }
}