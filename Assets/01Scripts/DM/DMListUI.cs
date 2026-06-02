/*
작성자 : 이종현
작성일 : 26-06-01
수정일 : 26-06-02

역할 : DM 목록 UI 생성 및 DM 클릭 시 대화창 전환
방식 : DM_TableSO의 messageId를 기준으로 Dialogue_TableSO를 찾아 미리보기 텍스트를 표시
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
    [SerializeField] private DM_TableSO[] dmTables;
    [SerializeField] private Dialogue_TableSO[] dialogueSOs;

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

            string previewText = GetPreviewText(dm.messageId);

            itemUI.SetData(
                dm.senderName.ToString(),
                previewText,
                false,
                () => OnClickDM(dm)
            );
        }
    }

    private string GetPreviewText(int messageId)
    {
        if (dialogueSOs == null)
            return "";

        Dialogue_TableSO previewDialogue = null;

        foreach (Dialogue_TableSO dialogue in dialogueSOs)
        {
            if (dialogue == null)
                continue;

            if (dialogue.messageId != messageId)
                continue;

            if (previewDialogue == null || dialogue.dialogId < previewDialogue.dialogId)
                previewDialogue = dialogue;
        }

        if (previewDialogue != null)
            return previewDialogue.dialogText;

        Log.Message($"Message ID에 해당하는 Preview Dialogue를 찾을 수 없습니다 : {messageId}");
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
        if (dmChatPanel == null || dmListPanel == null)
        {
            Log.Message("DM 패널이 연결되지 않았습니다.");
            return;
        }

        DMConversationRunner runner =
            dmChatPanel.GetComponent<DMConversationRunner>();

        if (runner != null)
            runner.StopConversation();

        dmChatPanel.SetActive(false);
        dmListPanel.SetActive(true);
    }
}
