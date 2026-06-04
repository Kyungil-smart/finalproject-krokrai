using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationData : MonoBehaviour
{
    [SerializeField] private AutoSOGen_ContaineSO _textData;
    
    [SerializeField] private TextMeshProUGUI _textBox;
    [SerializeField] private Image _profileImage;
    [SerializeField] private Image _postImage;
    [SerializeField] private Button _postImageButton;
    
    private Notification_TableSO _notiSOData;

    private void Start()
    {
        PrefabCreated();
    }

    private void PrefabCreated()
    {
        SetNoticeImage();
        SetNoticeText();
    }

    private void SetNoticeImage()
    {
        if (_profileImage != null)
        {
            ServiceLocator.Get<AddressableManager>().LoadImageSprite(들어갈 이미지의 어드레스 키, _profileImage);
        }

        if (_postImage != null)
        {
            ServiceLocator.Get<AddressableManager>().LoadImageSprite(들어갈 이미지의 어드레스 키, _postImage);
        }
    }

    private void SetNoticeText()
    {
        if (_notiSOData.notiType == Notification_TableEnum.COMMENT)
        {
            _textBox.text = _notiSOData.notiText;
        }

        if (_notiSOData.notiType == Notification_TableEnum.FOLLOW)
        {
            _textBox.text = _notiSOData.notiText;
        }

        if (_notiSOData.notiType == Notification_TableEnum.LIKE)
        {
            _textBox.text = _notiSOData.notiText;
        }
    }

    private void SetNoticeImageButton()
    {
        if (_postImage != null) SetNoticeImage(); return;
        
        

    }



    // public void Test()
    // {
    //     
    //     
    //     if (_textData.scriptableObjects[0] is Notification_TableSO)
    //     {
    //         Notification_TableSO abc = _textData.scriptableObjects[0] as Notification_TableSO;   
    //         
    //         (_textData.scriptableObjects[0] as Notification_TableSO).notiText;
    //        
    //         ServiceLocator.Get<AddressableManager>().LoadImageSprite();
    //         
    //         
    //     }
    // }



}
