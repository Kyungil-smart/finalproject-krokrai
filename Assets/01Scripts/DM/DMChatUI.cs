using UnityEngine;
using UnityEngine.UI;

public class DMChatUI : MonoBehaviour
{
    [SerializeField] private Transform chatContent;

    [Header("Bubble Prefabs")]
    [SerializeField] private GameObject opponentBubblePrefab;
    [SerializeField] private GameObject playerBubblePrefab;

    [Header("Test Profile")]
    [SerializeField] private Sprite testProfileSprite;

    private string previousSpeakerType = "";

    public void AddOpponentMessage(string message)
    {
        if (chatContent == null || opponentBubblePrefab == null)
        {
            Debug.LogError("ChatContent 또는 OpponentBubblePrefab이 연결되지 않았습니다.");
            return;
        }

        GameObject bubble = Instantiate(opponentBubblePrefab, chatContent);

        DMBubbleUI bubbleUI = bubble.GetComponentInChildren<DMBubbleUI>();

        if (bubbleUI == null)
        {
            Debug.LogError("상대 말풍선 프리팹에 DMBubbleUI가 없습니다.");
            return;
        }

        bool showProfile = previousSpeakerType != "NPC";

        bubbleUI.SetData(message, testProfileSprite, showProfile);

        previousSpeakerType = "NPC";

        RebuildChatLayout();
    }

    public void AddPlayerMessage(string message)
    {
        if (chatContent == null || playerBubblePrefab == null)
        {
            Debug.LogError("ChatContent 또는 PlayerBubblePrefab이 연결되지 않았습니다.");
            return;
        }

        GameObject bubble = Instantiate(playerBubblePrefab, chatContent);

        DMBubbleUI bubbleUI = bubble.GetComponentInChildren<DMBubbleUI>();

        if (bubbleUI == null)
        {
            Debug.LogError("플레이어 말풍선 프리팹에 DMBubbleUI가 없습니다.");
            return;
        }

        bubbleUI.SetData(message, null, false);

        previousSpeakerType = "PLAYER";

        RebuildChatLayout();
    }

    public void ClearChat()
    {
        if (chatContent == null)
            return;

        foreach (Transform child in chatContent)
        {
            Destroy(child.gameObject);
        }

        previousSpeakerType = "";

        RebuildChatLayout();
    }

    private void RebuildChatLayout()
    {
        Canvas.ForceUpdateCanvases();

        RectTransform contentRect = chatContent as RectTransform;
        if (contentRect != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
    }
}