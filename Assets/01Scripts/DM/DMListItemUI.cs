/*
작성자 : 이종현
작성일 : 26-06-01
수정일 : 26-06-10

역할 : DM 목록 아이템 UI 담당
방식 : 이름, 미리보기 텍스트, 읽지 않음 상태 표시 및 클릭 이벤트 연결
*/

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DMListItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text previewText;
    [SerializeField] private Button button;
    [SerializeField] private Image profileImage;
    
    [Header("Unread")]
    [SerializeField] private GameObject unreadDot;

    private string GetPreviewText(string preview)
    {
        if (string.IsNullOrEmpty(preview))
            return "";

        if (preview.Length <= 20)
            return preview;

        return preview.Substring(0, 20) + "...";
    }
    
    public void SetData(string senderName, string preview, bool hasUnread, Action onClick)
    {
        nameText.text = senderName;
        previewText.text = GetPreviewText(preview);

        if (unreadDot != null)
            unreadDot.SetActive(hasUnread);
        else
            Log.Message("UnreadDot이 연결되지 않았습니다.");

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick?.Invoke());
    }
}