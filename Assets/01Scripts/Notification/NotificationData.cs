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

    private int _notiTempladteID;

    /// <summary>
    /// SO 데이터를 받아서 프리펩 UI 채우기
    /// </summary>
    /// <param name="so"></param>
    public void Setup(Notification_TableSO so)
    {
        _notiTempladteID = so.notiTemplateId;

        // 텍스트 세팅
        if (_textBox != null)
        {
            var stringManager = ServiceLocator.Get<IString_TableManager>();
            if (stringManager == null)
            {
                Log.Message($"초기화 안됨");
                _textBox.text = so.notiText; // 임시
            }
            else
            {
                _textBox.text = stringManager.GetString(so.notiText, SystemLanguage.Korean);
            }

            // string text = ServiceLocator.Get<IString_TableManager>().GetString(so.notiText, SystemLanguage.Korean);

            // _textBox.text = text;
        }

        // // NPC 프로필 이미지 세팅
        // if (_profileImage != null)
        // {
        //     // TODO: NPC의 프로필 이미지의 어드레서블 키 값을 대입해줘야함
        //     ServiceLocator.Get<AddressableManager>().LoadImageSprite("", _profileImage);
        // }
        
        // // 포스트 이미지 세팅 (팔로우 제외)
        // if (_postImage != null)
        // {
        //     // TODO: 포스트 이미지의 어드레서블 키 값을 대입해줘야함
        //     ServiceLocator.Get<AddressableManager>().LoadImageSprite("", _postImage);
        // }

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
        // TODO: 게시물 포스팅 시스템 끝나면 연결해서 포스팅 고유번호 생성 후 버튼에 넘겨주기
        // PostManger.Instance.OpenPost(_notiTemplateId);
        Log.Message($" 게시물 클릭: {_notiTempladteID}");
    }
}
