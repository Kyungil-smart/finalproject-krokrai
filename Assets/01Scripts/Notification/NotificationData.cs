/*
작성자 : 23M-RFT68
작성일 : 26-06-04
수정일 : 26-06-04

역할 : SO로부터 데이터를 받아와서 프리펩의 이미지와 텍스트를 변경해줄 역할
방식 : SO와 Addressable로 텍스트와 이미지 변환
*/
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textBox;
    [SerializeField] private Image _profileImage;
    [SerializeField] private Image _postImage;
    [SerializeField] private Button _postImageButton;

    private PostController _postController;
    
    private int _notiTemplateID;
    private int _postId;            // 포스트 ID
    private int _postImgId;         // 포스트 이미지 ID

    /// <summary>
    /// SO 데이터를 받아서 프리펩 UI 채우기
    /// </summary>
    public void Setup(Notification_TableSO so, string finalText,
        string npcImageKey, string postImageKey, int postId, int postImgId)
    {
        _notiTemplateID = so.notiTemplateId;
        _postId = postId; 
        _postImgId = postImgId;
        
        // 텍스트 세팅
        if (_textBox != null)
            _textBox.text = finalText;

        // NPC 프로필 이미지 세팅
        if (_profileImage != null)
        {
            Log.Message($"Npc 이미지 키: {npcImageKey}");
            // if (_profileImage != null && !string.IsNullOrEmpty(npcImageKey))
            //     ServiceLocator.Get<AddressableManager>().LoadImageSprite(npcImageKey, _profileImage);
        }

        // 포스트 이미지 세팅 (팔로우 제외)
        if (_postImage != null)
        {
            Log.Message($"Post 이미지 키: {postImageKey}");
            if (_postImage != null && !string.IsNullOrEmpty(postImageKey))
               ServiceLocator.Get<AddressableManager>().LoadImageSprite(postImageKey, _postImage);
        }


        // 포스트 버튼 연결 (팔로우 제외)
        if (_postImageButton != null)
        {
            _postImageButton.onClick.RemoveAllListeners();
            _postImageButton.onClick.AddListener(OnPostButtonClicked);
        }
    }

    // TODO: 포스팅 시스템 끝나면 연결 - 포스트 이미지 눌렀을때 해당 게시물로 이동하기 위함
    private void OnPostButtonClicked()
    {
        if (_postController == null)
        {
            Log.Message(" PostController 연결 안됨");
            return;
        }

        _postController.SetPost(_postImgId, _postId);
        Log.Message($" 포스트 이동: postImg : {_postImgId}, postId : {_postId} ");
        
    }

    public void SetPostController(PostController controller)
    {
        _postController = controller;
    }
}
