using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DMBubbleUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text messageText;

    [Header("Profile Images")]
    [SerializeField] private Image[] profileImages;

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
        float alpha = visible ? 1f : 0f;

        foreach (Image image in profileImages)
        {
            if (image == null) continue;

            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}