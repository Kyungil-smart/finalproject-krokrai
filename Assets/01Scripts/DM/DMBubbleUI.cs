/*
작성자 : 이종현
작성일 : 26-06-01
수정일 : 26-06-12

역할 : 채팅 말풍선 UI 담당
방식 : 텍스트 길이에 따라 말풍선 크기를 조절하고 프로필 표시 여부를 처리
*/

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DMBubbleUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text messageText;

    [Header("Profile Images")]
    [SerializeField] private Image[] profileImages;
    
    [SerializeField] private CanvasGroup profileCanvasGroup;

    [Header("Layout")]
    [SerializeField] private LayoutElement rowLayoutElement;     // DM_Op 또는 DM_me 루트
    [SerializeField] private LayoutElement bubbleLayoutElement;  // 말풍선 배경

    [SerializeField] private float maxWidth = 600f;
    [SerializeField] private float paddingWidth = 30f;
    [SerializeField] private float paddingHeight = 18f;
    [SerializeField] private float minRowHeight = 45f;

    public void SetData(string message, Sprite profileSprite, bool showProfile)
    {
        messageText.text = message;

        float textWidth = messageText.GetPreferredValues(message).x;
        float bubbleWidth = Mathf.Min(textWidth + paddingWidth, maxWidth);

        bubbleLayoutElement.preferredWidth = bubbleWidth;

        float textHeight = messageText.GetPreferredValues(message, bubbleWidth - paddingWidth, 0).y;
        float bubbleHeight = textHeight + paddingHeight;

        bubbleLayoutElement.preferredHeight = bubbleHeight;
        rowLayoutElement.preferredHeight = Mathf.Max(bubbleHeight, minRowHeight);

        SetProfileVisible(showProfile);
        
        if (showProfile && profileSprite != null && profileImages.Length > 0)
            profileImages[0].sprite = profileSprite;

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    private void SetProfileVisible(bool visible)
    {
        if (profileCanvasGroup == null)
            return;

        profileCanvasGroup.alpha = visible ? 1f : 0f;
        profileCanvasGroup.interactable = false;
        profileCanvasGroup.blocksRaycasts = false;
    }
    
    public void LoadProfileImage(string profileImageKey)
    {
        if (profileImages == null || profileImages.Length < 2)
        {
            Log.Message("프로필 이미지 배열이 부족합니다.");
            return;
        }

        if (string.IsNullOrEmpty(profileImageKey))
            return;

        IAddressableManager addressableManager =
            ServiceLocator.Get<IAddressableManager>();

        if (addressableManager == null)
        {
            Log.Message("AddressableManager를 찾을 수 없습니다.");
            return;
        }

        Image profileImage = profileImages[1];

        if (profileImage == null)
            return;

        addressableManager.LoadImageSprite(profileImageKey, profileImage);
    }
}