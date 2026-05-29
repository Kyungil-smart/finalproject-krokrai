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
                "최근 메시지 미리보기입니다.",
                OnClickDM
            );
        }
    }

    private void OnClickDM()
    {
        dmListPanel.SetActive(false);
        dmChatPanel.SetActive(true);

        DMChatUI chatUI = dmChatPanel.GetComponent<DMChatUI>();

        if (chatUI == null)
        {
            Debug.LogError("DMChatPanel에 DMChatUI가 없습니다.");
            return;
        }

        chatUI.ClearChat();

        chatUI.AddOpponentMessage("안녕");
        chatUI.AddOpponentMessage("지금 메시지 보여?");
        chatUI.AddOpponentMessage("이건 연속 상대 메시지라 프로필이 안 보여야 해.");

        chatUI.AddPlayerMessage("응 보여.");

        chatUI.AddOpponentMessage("좋아. 그럼 내 프로필은 다시 보여야 정상이고");
        chatUI.AddOpponentMessage("이 메시지는 연속 메시지라 프로필이 없어야 해.");

        chatUI.AddPlayerMessage("짧은 답장");
        chatUI.AddPlayerMessage("이건 내 긴 답장 테스트야. 말풍선이 글자 길이에 맞춰서 늘어나다가 최대 폭을 넘으면 자동으로 줄바꿈이 되는지 확인하려고 일부러 길게 작성했어.");

        chatUI.AddOpponentMessage("상대 짧은 말");
        chatUI.AddOpponentMessage("상대방이 보내는 긴 메시지 테스트입니다. 이 문장은 말풍선 배경 이미지가 텍스트 길이에 맞게 늘어나고, 너무 길어지면 최대 폭에서 줄바꿈되는지 확인하기 위한 테스트 문장입니다.");
        chatUI.AddOpponentMessage("상대방이 보내는 긴 메시지 테스트입니다. 이 문장은 말풍선 배경 이미지가 텍스트 길이에 맞게 늘어나고, 너무 길어지면 최대 폭에서 줄바꿈되는지 확인하기 위한 테스트 문장입니다.");
        chatUI.AddOpponentMessage("상대방이 보내는 긴 메시지 테스트입니다. 이 문장은 말풍선 배경 이미지가 텍스트 길이에 맞게 늘어나고, 너무 길어지면 최대 폭에서 줄바꿈되는지 확인하기 위한 테스트 문장입니다.");

        
        
        chatUI.AddPlayerMessage("ㅇㅋ");
        chatUI.AddOpponentMessage("마지막 메시지입니다.");
    }
}