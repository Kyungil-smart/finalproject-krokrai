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
    [SerializeField] private AutoSOGen_ContaineSO _postNotiSOData;          // post_notification_ListSO 컨테이너
    [SerializeField] private AutoSOGen_ContaineSO _npcSOData;               // npc_TableSO 컨테이너
    [SerializeField] private AutoSOGen_ContaineSO _postSOData;              // Post_TableSO 컨테이너
    
    [SerializeField] private GameObject _commentPrefab;
    [SerializeField] private GameObject _followPrefab;                      // 알림 프리펩 3개
    [SerializeField] private GameObject _likePrefab;
    
    [SerializeField] private GameObject _redNoticePrefab;
    [SerializeField] private GameObject _redExclamationmark;                // 알림 표시 UI
    [SerializeField] private Button _removeRedNoticeButton;

    [SerializeField] private Transform _contentView;                        // 스크롤 뷰 (알림 쌓이는곳)
    
    private List<GameObject> _items = new List<GameObject>();               // 생성된 알림 축적

    
    /// <summary>
    /// 포스팅 버튼에 온클릭으로 연결할 함수
    /// </summary>
    public void AddNotification(int postId)
    {
        // SO 여부 확인
        if (_postNotiSOData == null)
        {
            Log.Message("PostNotiSO 연결안됨");
            return;
        }

        if (_notiSOData == null)
        {
            Log.Message("SO 연결안됨"); 
            return;
        }
     
        // postId에 해당하고 70000n 번대 Row만 찾기
        List<Post_Notification_ListSO> matchedRows = new List<Post_Notification_ListSO>();
        
        // 인덱스 범위 확인
        for (int i = 0; i < _postNotiSOData.scriptableObjects.Length; i++)
        {
            // SO 배열에서 해당 인덱스의 SO를 Notification_TableSO로 캐스팅
            if (_postNotiSOData.scriptableObjects[i] is Post_Notification_ListSO row)
            {
                bool isTargetPost = row.postId == postId;
                bool isNotiTemplate = row.notiTemplate >= 700000 && row.notiTemplate <= 799999;

                if (isTargetPost && isNotiTemplate)
                {
                    matchedRows.Add(row);
                }

            }
        }

        if (matchedRows.Count == 0)
        {
            Log.Message($"{postId}에 해당하는 알림 없음");
            return;
        }

        // displayOrder 순서대로 정렬
        matchedRows.Sort((a, b) => a.displayOrder.CompareTo(b.displayOrder));
        
        // 각 Row마다 알림 생성
        foreach (var row in matchedRows)
        {
            Notification_TableSO notiSO = FindNotiSO(row.notiTemplate);
            if (notiSO == null)
            {
                Log.Message($"{row.notiTemplate} 못찾음");
                continue;
            }
            
            // NPC 이름 가져오기 (referencedNpcId에서 직접)
            string npcName = FindNpcAccountName(row.referencedNpcId);
            
            // 기본 텍스트 가져오기
            string baseText = GetLocalizedText(notiSO.notiText);
            string finalText = baseText
                .Replace("{npcAccountName}", npcName);
            
            // Comment 타입만 commentText 치환 추가
            if (notiSO.notiType == Notification_TableEnum.COMMENT)
            {
                string commentText = GetLocalizedText(row.commentText);
                finalText = finalText.Replace("{commentText}", commentText);
            }
            
            GameObject prefab = GetPrefabByType(notiSO.notiType);
            SpawnNotification(prefab, notiSO, finalText, row.postId, row.referencedNpcId);
        }

        TurnOnRedNotice();
        Log.Message($"{postId} 알림 추가 완료");
    }

    private Notification_TableSO FindNotiSO(int notiTemplateId)
    {
        for (int i = 0; i < _notiSOData.scriptableObjects.Length; i++)
            if (_notiSOData.scriptableObjects[i] is Notification_TableSO so)
                if (so.notiTemplateId == notiTemplateId)
                    return so;
        return null;
    }

    private string FindNpcAccountName(int npcId)
    {
        if (_npcSOData == null) return "UnKnown";
        
        for (int i = 0; i < _npcSOData.scriptableObjects.Length; i++)
            if (_npcSOData.scriptableObjects[i] is Npc_TableSO npc)
                if (npc.npcId == npcId)
                    return npc.npcAccountName;
        return "UnKnown";
    }

    private string GetLocalizedText(string stringId)
    {
        if (_stringSOData == null) return stringId;
        
        string cleanId = stringId.Trim();
        
        for (int i = 0; i < _stringSOData.scriptableObjects.Length; i++)
            if (_stringSOData.scriptableObjects[i] is String_TableSO stringSO)
                if (stringSO.stringId.Trim() == cleanId)
                    return stringSO.KR;
        return stringId;
    }

    private GameObject GetPrefabByType(Notification_TableEnum notiType)
    {
        switch (notiType)
        {
            case Notification_TableEnum.LIKE: return _likePrefab;
            case Notification_TableEnum.FOLLOW: return _followPrefab;
            case Notification_TableEnum.COMMENT:  return _commentPrefab;
            default: return null;
        }
    }

    // 프리펩 1개 생성하고 SO 데이터를 채워주는 내부 함수
    private void SpawnNotification(GameObject prefab, Notification_TableSO so, string finalText,
        int postId, int npcId)
    {
        // 프리펩 연결 확인
        if (prefab == null) { Log.Message("프리펩이 연결 안됨"); return;}
        
        // Post_TableSO 찾기
        Post_TableSO postSO = FindPostSO(postId);
        
        // extraNumber 타입별로 분류
        string extraNumber = "";
        if (so.notiType == Notification_TableEnum.LIKE && postSO != null)
            extraNumber = postSO.likeCount.ToString();
        else if (so.notiType == Notification_TableEnum.FOLLOW && postSO != null)
            extraNumber = postSO.getFollower.ToString();
        
        finalText = finalText.Replace("{extraNumber}", extraNumber);
        
        string npcImageKey = FindNpcImageKey(npcId);
        string postImageKey = postSO != null ? postSO.postImage.ToString() : "";
        
        // 프리펩을 Content 하위에 생성
        GameObject item = Instantiate(prefab, _contentView);
        
        // 생성된 프리팹의 NotificationData 컴포넌트에 SO 데이터 전달
        // NotificationData.SetUp()이 텍스트와 이미지를 채워줌
        var notificationData = item.GetComponent<NotificationData>();
        if (notificationData != null)
            notificationData.Setup(so, finalText, npcImageKey, postImageKey);
        else
            Log.Message("NotificationData 컴포넌트가 없습니다.");
       
        // 최신 알림이 맨 위로 오도록 첫번째 온 알림이 밑으로 쌓임
        item.transform.SetAsFirstSibling();
        
        // 축적되는 리스트에 추가
        _items.Add(item);
    }

    private Post_TableSO FindPostSO(int postId)
    {
        if (_postSOData == null) return null;
        
        for (int i = 0; i < _postSOData.scriptableObjects.Length; i++)
            if  (_postSOData.scriptableObjects[i] is Post_TableSO post)
                if (post.postID == postId)
                    return post;
        return null;
    }

    private string FindNpcImageKey(int npcId)
    {
        if (_npcSOData == null) return "";
        
        for (int i = 0; i < _npcSOData.scriptableObjects.Length; i++)
            if (_npcSOData.scriptableObjects[i] is Npc_TableSO npc)
                if (npc.npcId == npcId)
                    return npc.npcImage.ToString();
        return "";
    }

    // 빨간 소형 알림 팝업 뜨게하기
    private void TurnOnRedNotice()
    {
        if (_redNoticePrefab != null) _redNoticePrefab.SetActive(true);
        if (_redExclamationmark != null) _redExclamationmark.SetActive(true);
    }
}
