using System;
using System.Collections;
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

    [Header("Choice")]
    [SerializeField] private GameObject choiceArea;
    [SerializeField] private Transform choiceContent;
    [SerializeField] private GameObject choiceButtonPrefab;

    [Header("Scroll")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private VerticalLayoutGroup chatContentLayoutGroup;
    [SerializeField] private RectTransform choiceAreaRect;
    [SerializeField] private int defaultBottomPadding = 50;
    [SerializeField] private float choicePaddingOffset = 10f;

    private string previousSpeakerType = "";

    public void AddOpponentMessage(string message)
    {
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
        ScrollToBottom();
    }

    public void AddPlayerMessage(string message)
    {
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
        ScrollToBottom();
    }

    public void ShowChoices(string[] choices, Action<int> onChoiceSelected)
    {
        ClearChoices();

        if (choiceArea != null)
            choiceArea.SetActive(true);

        for (int i = 0; i < choices.Length; i++)
        {
            int index = i;

            GameObject buttonObj = Instantiate(choiceButtonPrefab, choiceContent);
            DMChoiceButtonUI choiceButton = buttonObj.GetComponentInChildren<DMChoiceButtonUI>();

            if (choiceButton == null)
            {
                Debug.LogError("ChoiceButtonPrefab에 DMChoiceButtonUI가 없습니다.");
                return;
            }

            choiceButton.SetData(choices[i], () =>
            {
                HideChoices();

                onChoiceSelected?.Invoke(index);

                RebuildChatLayout();
                ScrollToBottom();
            });
        }

        ApplyChoicePadding();
        RebuildChatLayout();
        ScrollToBottom();
    }

    public void ClearChoices()
    {
        if (choiceContent == null)
            return;

        foreach (Transform child in choiceContent)
        {
            Destroy(child.gameObject);
        }
    }

    public void HideChoices()
    {
        ClearChoices();

        if (choiceArea != null)
            choiceArea.SetActive(false);

        if (chatContentLayoutGroup != null)
            chatContentLayoutGroup.padding.bottom = defaultBottomPadding;

        RebuildChatLayout();
        ScrollToBottom();
    }

    public void ClearChat()
    {
        foreach (Transform child in chatContent)
        {
            Destroy(child.gameObject);
        }

        previousSpeakerType = "";

        HideChoices();

        RebuildChatLayout();
        ScrollToBottom();
    }

    private void ApplyChoicePadding()
    {
        if (chatContentLayoutGroup == null || choiceAreaRect == null)
            return;

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(choiceAreaRect);

        float choiceHeight = LayoutUtility.GetPreferredHeight(choiceAreaRect);

        if (choiceHeight <= 0)
            choiceHeight = choiceAreaRect.rect.height;

        chatContentLayoutGroup.padding.bottom =
            Mathf.CeilToInt(choiceHeight + choicePaddingOffset);
    }

    private void RebuildChatLayout()
    {
        Canvas.ForceUpdateCanvases();

        RectTransform contentRect = chatContent as RectTransform;
        if (contentRect != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
    }

    private void ScrollToBottom()
    {
        StartCoroutine(ScrollToBottomRoutine());
    }

    private IEnumerator ScrollToBottomRoutine()
    {
        yield return null;
        yield return null;

        Canvas.ForceUpdateCanvases();

        if (scrollRect != null)
            scrollRect.verticalNormalizedPosition = 0f;
    }
}