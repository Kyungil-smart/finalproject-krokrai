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
    [SerializeField] private AutoSOGen_ContaineSO _notiSOData;              // 알림 정보 SO 컨테이너
    [SerializeField] private AutoSOGen_ContaineSO _stringSOData;            // 대사 SO 컨테이너
     
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
    public void AddNotification()
    {
        // SO 여부 확인
        if (_notiSOData == null)
        {
            Log.Message($"SO 연결안됨"); 
            return;
        }
     
        // 타입별 분류
        List<Notification_TableSO> likes = new List<Notification_TableSO>();
        List<Notification_TableSO> follows = new List<Notification_TableSO>();
        List<Notification_TableSO> comments = new List<Notification_TableSO>();
        
        // 인덱스 범위 확인
        for (int i = 0; i < _notiSOData.scriptableObjects.Length; i++)
        {
            // SO 배열에서 해당 인덱스의 SO를 Notification_TableSO로 캐스팅
            if (!(_notiSOData.scriptableObjects[i] is Notification_TableSO so)) continue;

            switch (so.notiType)
            {
                case Notification_TableEnum.LIKE: likes.Add(so); break;
                case Notification_TableEnum.FOLLOW: follows.Add(so); break;
                case Notification_TableEnum.COMMENT: comments.Add(so); break;
            }
        }

        // like -> Follow -> Comment 순으로 생성
        // SetAsLastSibling으로 순서대로 아래에 쌓임
       foreach (var so in likes) SpawnNotification(_likePrefab, so);
       foreach (var so in follows) SpawnNotification(_followPrefab, so);
       foreach (var so in comments) SpawnNotification(_commentPrefab, so);
        
        Log.Message($"알림 추가 완료");
        TurnOnRedNotice();
    }

    /// <summary>
    /// 알림 테이블 키값을 넣으면 스트링 SO 안에서 텍스트를 가져옵니다.
    /// </summary>
    public string GetLocalizedTextDirect(string stringId)
    {
        if (_stringSOData == null || _stringSOData.scriptableObjects == null)
        {
            Log.Message("스트링 SO 컨터이너 연결 안됨");
            return stringId;
        }

        // 키값의 앞뒤 공백 제거
        string cleanInputId = stringId.Trim();
        
        // 스트링 컨테이너 내부 배열 전부 탐색
        for (int i = 0; i < _stringSOData.scriptableObjects.Length; i++)
        {
            if (_stringSOData.scriptableObjects[i] is String_TableSO stringSO)
            {
                // 스트링 테이블 so에 들어있는 키값도 공백이 있을수 있으니 Trim() 사용
                string cleanTableId = stringSO.stringId.Trim();
                
                // 공백 없앤 순수 스트링 키값만 비교
                if (cleanTableId == cleanInputId)
                {
                    return stringSO.KR;
                }
            }
        }
        return stringId;
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
            string convertedText = GetLocalizedTextDirect(so.notiText);
            
            notificationData.Setup(so, convertedText);
        }
        else
        {
            Log.Message($"NotificationData 컴포넌트가 없습니다.");
        }
       
        // 최신 알림이 맨 위로 오도록 첫번째 온 알림이 밑으로 쌓임
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
