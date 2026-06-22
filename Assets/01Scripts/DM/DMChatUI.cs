/*
작성자 : 이종현
작성일 : 26-06-01
수정일 : 26-06-17

역할 : DM 채팅 UI 출력 담당
방식 : 말풍선 생성, 선택지 생성, 스크롤 처리 및 채팅 레이아웃 갱신
*/

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
    
    [SerializeField] private GameObject rewardMessagePrefab;
    [SerializeField] private Transform messageParent;
    
    [SerializeField] private GameObject requestCardPrefab;

    private string previousSpeakerType = "";
    
    private string opponentProfileImageKey;

    public void AddOpponentMessage(string message)
    {
        GameObject bubble = Instantiate(opponentBubblePrefab, chatContent);

        DMBubbleUI bubbleUI = bubble.GetComponentInChildren<DMBubbleUI>();

        if (bubbleUI == null)
        {
            Log.Message("상대 말풍선 프리팹에 DMBubbleUI가 없습니다.");
            return;
        }

        bool showProfile = previousSpeakerType != "NPC";

        bubbleUI.SetData(message, null, showProfile);

        if (showProfile)
        {
            bubbleUI.LoadProfileImage(opponentProfileImageKey);
        }

        previousSpeakerType = "NPC";

        RebuildChatLayout();
        ScrollToBottom();
    }
    
    ///<summary>
    /// 요청 카드 UI를 생성합니다.
    ///</summary>
    public void AddRequestCard(Request_TableSO requestData, string npcName)
    {
        GameObject item = Instantiate(requestCardPrefab, messageParent);

        DMRequestCardUI cardUI = item.GetComponentInChildren<DMRequestCardUI>();

        if (cardUI == null)
        {
            Log.Message("RequestCardPrefab에 DMRequestCardUI가 없습니다.");
            return;
        }

        cardUI.SetData(requestData, npcName);

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(item.transform as RectTransform);
    }
    
    ///<summary>
    /// 팔로워 보상 메시지를 전용 프리팹으로 생성합니다.
    ///</summary>
    public void AddRewardMessage(string rewardText)
    {
        if (rewardMessagePrefab == null || messageParent == null)
        {
            Log.Message("RewardMessagePrefab 또는 MessageParent가 연결되지 않았습니다.");
            return;
        }

        GameObject item = Instantiate(rewardMessagePrefab, messageParent);

        DMBubbleUI bubbleUI = item.GetComponent<DMBubbleUI>();

        if (bubbleUI == null)
        {
            Log.Message("RewardMessagePrefab에 DMBubbleUI가 없습니다.");
            return;
        }

        bubbleUI.SetData(
            rewardText,
            null,
            false
        );
    }
    
    public void SetOpponentProfileImageKey(string profileImageKey)
    {
        opponentProfileImageKey = profileImageKey;
    }

    public void AddPlayerMessage(string message)
    {
        GameObject bubble = Instantiate(playerBubblePrefab, chatContent);

        DMBubbleUI bubbleUI = bubble.GetComponentInChildren<DMBubbleUI>();

        if (bubbleUI == null)
        {
            Log.Message("플레이어 말풍선 프리팹에 DMBubbleUI가 없습니다.");
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
                Log.Message("ChoiceButtonPrefab에 DMChoiceButtonUI가 없습니다.");
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