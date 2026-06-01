using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DMListItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text previewText;
    [SerializeField] private Button button;

    [Header("Unread")]
    [SerializeField] private GameObject unreadDot;

    public void SetData(string senderName, string preview, bool hasUnread, Action onClick)
    {
        nameText.text = senderName;
        previewText.text = preview;

        if (unreadDot != null)
            unreadDot.SetActive(hasUnread);
        else
            Debug.LogWarning("UnreadDot이 연결되지 않았습니다.");

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick?.Invoke());
    }
}