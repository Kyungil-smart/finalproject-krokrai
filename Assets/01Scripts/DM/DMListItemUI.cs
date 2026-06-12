/*
작성자 : 이종현
작성일 : 26-06-01
수정일 : 26-06-12

역할 : DM 목록 아이템 UI 담당
방식 : 이름, 미리보기 텍스트, 읽지 않음 상태 표시 및 클릭 이벤트 연결
*/

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DMListItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text previewText;
    [SerializeField] private GameObject unreadDot;
    [SerializeField] private Button button;
    [SerializeField] private Image profileImage;

    public void SetData(
        string npcName,
        string preview,
        bool hasUnread,
        string profileImageKey,
        Action onClick
    )
    {
        nameText.text = npcName;
        previewText.text = preview;

        if (unreadDot != null)
            unreadDot.SetActive(hasUnread);

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick?.Invoke());
        }

        LoadProfileImage(profileImageKey);
    }

    private void LoadProfileImage(string profileImageKey)
    {
        if (profileImage == null)
            return;

        if (string.IsNullOrEmpty(profileImageKey))
            return;

        IAddressableManager addressableManager =
            ServiceLocator.Get<IAddressableManager>();

        if (addressableManager == null)
        {
            Log.Message("AddressableManager를 찾을 수 없습니다.");
            return;
        }

        addressableManager.LoadImageSprite(profileImageKey, profileImage);
    }
}