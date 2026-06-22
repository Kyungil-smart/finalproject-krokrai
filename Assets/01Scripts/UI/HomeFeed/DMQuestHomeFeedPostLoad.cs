/*
 작성자 : cminhyeong1999
 작성일 : 26-06-22
 수정일 : 26-06-22

 역할 : DM 퀘스트 진행 중 홈피드에 포스트 게시 요청시 해당 게시글을 홈피드에 게시
 방식 : GetPost에 게시해야할 포스트 ID를 전달하면 해당 포스트를 홈피드에 게시
 특이사항 : 이미지 스프라이트는 아직 코드로 제어하지 않았음, 관련 작업 추후 예정
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMQuestHomeFeedPostLoad : MonoBehaviour
{
    
    //[Header("테스트")]
    //[SerializeField] private int _testID;
    
    [Header("DM 포스트 목록")]
    [SerializeField] private GameObject[] _postLists;
    
    [Header("DM 포스트 ID")]
    [SerializeField] private List<int> _dmPostIds;
    
    [Header("포스트를 표시할 위치")]
    [SerializeField] private Transform _postPoint;
    
    // 포스트ID와 해당 포스트를 매칭한 Dictionary
    private Dictionary<int, GameObject> _postDict;
    
    // 실제로 홈 피드에 게시할 게시물 목록
    private Queue<GameObject> _postQueue;

    private void Awake()
    {
        _postDict = new();
        _postQueue = new();
        
        int index = 0;
        
        foreach (var post in _postLists)
        {
            _postDict.Add(_dmPostIds[index], post);
            index++;
        }
    }
    
    /// <summary>
    /// 게시해야할 포스트의 ID를 전달하면 홈피드에 게시
    /// </summary>
    /// <param name="postID">홈피드에 게시할 포스트 ID (DB에 postId칸에 있음)</param>
    public void GetPost(int postID)
    {
        if (_postDict.TryGetValue(postID, out var post))
        {
            // 게시해야할 포스트 복사본 생성후 enqueue
            var createTemp = CreatePost(post);
            _postQueue.Enqueue(createTemp);
            
            // enqueue후 게시해야 할 포스트가 4개 이상이면 Dequeue
            if (_postQueue.Count > 3)
            {
                var oldTemp = _postQueue.Dequeue();
                DeletePost(oldTemp);
            }
        }
    }
    
    private GameObject CreatePost(GameObject post)
    {
        var instTemp = Instantiate(post);
        instTemp.transform.localPosition = Vector3.zero;
        instTemp.transform.localScale = Vector3.one;
        instTemp.transform.SetParent(_postPoint);
        return instTemp;
    }

    private void DeletePost(GameObject post)
    {
        if (post != null)
        {
            Destroy(post);
        }
    }

    /*
    [ContextMenu("Load")]
    private void Test()
    {
        GetPost(_testID);
    }
    */
}
