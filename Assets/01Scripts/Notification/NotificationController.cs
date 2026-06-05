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
    [SerializeField] private AutoSOGen_ContaineSO _notiSOData;              // SO 컨테이너
    
    [SerializeField] private GameObject _commentPrefab;
    [SerializeField] private GameObject _followPrefab;                      // 알림 프리펩 3개
    [SerializeField] private GameObject _likePrefab;
    
    [SerializeField] private GameObject _redNoticePrefab;
    [SerializeField] private GameObject _redExclamationmark;                // 알림 표시 UI
    [SerializeField] private Button _removeRedNoticeButton;

    [SerializeField] private Transform _contentView;                        // 스크롤 뷰 (알림 쌓이는곳)
    // [SerializeField] private Transform _redNoticePoint;
    
    private List<GameObject> _items = new List<GameObject>();               // 생성된 알림 축적

    
    /// <summary>
    /// 포스팅 버튼에 온클릭으로 연결할 함수
    /// </summary>
    /// <param name="soIndex"></param>
    public void AddNotification(int soIndex)
    {
        // SO 여부 확인
        if (_notiSOData == null)
        {
            Log.Message($"SO 연결안됨"); 
            return;
        }
        
        // 인덱스 범위 확인
        if (soIndex < 0 || soIndex >= _notiSOData.scriptableObjects.Length)
        {
            Log.Message($"SO 인덱스 범위 초과: {soIndex}");
            return;            
        }
        
        // SO 배열에서 해당 인덱스의 SO를 Notification_TableSO로 캐스팅
        if (!(_notiSOData.scriptableObjects[soIndex] is Notification_TableSO so))
        {
            Log.Message($"NotificationTavleSo 캐스팅 실패: {soIndex}");
            return;
        }

        // Like / Follow / Comment 프리펩 3개 전부 생성
        SpawnNotification(_likePrefab, so);
        SpawnNotification(_followPrefab, so);
        SpawnNotification(_commentPrefab, so);
        
        Log.Message($"알림 추가 3개 완료: {so.notiTemplateId}");
        
        TurnOnRedNotice();
    }
    
    // 프리펩 1개 생성하고 SO 데이터를 채워주는 내부 함수
    private void SpawnNotification(GameObject prefab, Notification_TableSO so)
    {
        // 프리펩 연결 확인
        if (prefab == null)
        {
            Log.Message($"프리펩이 연결되어있지 않습니다.");
            return;
        }

        // 프리펩을 Content 하위에 생성
        GameObject item = Instantiate(prefab, _contentView);
        
        // 생성된 프리팹의 NotificationData 컴포넌트에 SO 데이터 전달
        // NotificationData.SetUp()이 텍스트와 이미지를 채워줌
        var notificationData = item.GetComponent<NotificationData>();
        if (notificationData != null)
        {
            notificationData.Setup(so);
        }
        else
        {
            Log.Message($"NotificationData 컴포넌트가 없습니다.");
        }
        
        // 최신 알림이 맨 위로 오도록 첫번째 자식으로 이동
        item.transform.SetAsFirstSibling();
        
        // 축적되는 리스트에 추가
        _items.Add(item);
    }
    
    // 빨간 소형 알림 팝업 뜨게하기
    private void TurnOnRedNotice()
    {
        if (_redNoticePrefab != null) _redNoticePrefab.SetActive(true);
        if (_redExclamationmark != null) _redExclamationmark.SetActive(true);
    }
}
