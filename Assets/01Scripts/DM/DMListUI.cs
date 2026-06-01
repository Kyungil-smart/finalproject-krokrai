using UnityEngine;

public class DMListUI : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject dmListItemPrefab;

    [Header("Panels")]
    [SerializeField] private GameObject dmListPanel;
    [SerializeField] private GameObject dmChatPanel;

    private void Start()
    {
        dmChatPanel.SetActive(false);
        CreateDummyList();
    }

    private void CreateDummyList()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject item = Instantiate(dmListItemPrefab, content);

            DMListItemUI itemUI = item.GetComponentInChildren<DMListItemUI>();

            if (itemUI == null)
            {
                Debug.LogError("DMListItemUI를 찾을 수 없습니다.");
                return;
            }
            itemUI.SetData(
                "상대방 " + (i + 1),
                "이건 NPC가 이미 보내둔 DM이야.",
                true,
                OnClickDM
            );
        }
    }

    private void OnClickDM()
    {
        dmListPanel.SetActive(false);
        dmChatPanel.SetActive(true);

        DMConversationRunner runner =
            dmChatPanel.GetComponent<DMConversationRunner>();

        if (runner == null)
        {
            Debug.LogError("DMChatPanel에 DMConversationRunner가 없습니다.");
            return;
        }

        runner.OpenNpcDM();
    }
}