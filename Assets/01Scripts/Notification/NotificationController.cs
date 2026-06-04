/*
작성자 : 23M-RFT68
작성일 : 26-06-04
수정일 : 26-06-04

역할 : 게시물 포스팅 버튼이 눌렸을때 알림 UI 오브젝트를 활성화 하고 알림창에서 프리펩을 타입별로 생성
방식 : 프리펩화 된 알림 타입을 생성하고 알림 팝업 오브젝트를 활성화 해서 알림을 활성화함
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour
{
    [SerializeField] private AutoSOGen_ContaineSO _notiSOData;
        
    [SerializeField] private GameObject _commentPrefab;
    [SerializeField] private GameObject _followPrefab;
    [SerializeField] private GameObject _likePrefab;
    [SerializeField] private GameObject _redNoticePrefab;
    [SerializeField] private GameObject _redExclamationmark;
    [SerializeField] private Button _removeRedNoticeButton;

    [SerializeField] private Transform _contentView;
    // [SerializeField] private Transform _redNoticePoint;
    
    /// <summary>
    /// 게시물 포스팅 버튼을 눌렀을때 호출되면 알림이 시작되는 함수입니다. 버튼에 연결하시거나 함수를 호출해주세요
    /// </summary>
    public void ClickedPostingButton()
    {
        AddNotification();
    }

    // 알림 뜨게 하기
    private void AddNotification()
    {
        Instantiate(_likePrefab, _contentView);
        Instantiate(_followPrefab, _contentView);
        Instantiate(_commentPrefab, _contentView);
        TurnOnRedNotice();
    }
    
    // 빨간 소형 알림 팝업 뜨게하기
    private void TurnOnRedNotice()
    {
        if (_redNoticePrefab != null) _redNoticePrefab.SetActive(true);
        if (_redExclamationmark != null) _redExclamationmark.SetActive(true);
    }
}
