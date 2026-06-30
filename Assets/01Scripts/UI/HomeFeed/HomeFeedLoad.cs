/*
 작성자 : cminhyeong1999
 수정자 : krokrai
 작성일 : 26-06-22
 수정일 : 26-06-29

 역할 : 게시물에 필요한 정보를 홈피드에 맞게끔 출력
 방식 : 댓글을 제외한 모든 데이터를 이 곳에서 처리 및 댓글은 객체로 분리
 특이사항 : 현재 유니티 안에서 코드제어 없이 인스펙터만으로 홈피드 게시물을 만들었음. 추후 코드로 제어할 예정
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeFeedLoad : MonoBehaviour
{
    [Header("SO Root")]
    [SerializeField] private AutoSOGen_ContaineSO _npcTableRoot;
    
    [Header("PostListCompression")]
    [SerializeField] private PostListCompression _postListComp;

    [Header("출력할 포스트 ID")]
    [SerializeField] private int _postID;
    [SerializeField] private int _postImgID;
    
    [Header("포스트 인스펙터")]
    [SerializeField] private Image _postImg;
    [SerializeField] private TextMeshProUGUI _poster;
    [SerializeField] Button _postLike;
    [SerializeField] GameObject _postLikeImg;
    [SerializeField] Button _postShare;
    [SerializeField] TextMeshProUGUI _postLikeCount;
    [SerializeField] TextMeshProUGUI _hashTagText;
    [SerializeField] TextMeshProUGUI _postContent;
    
    [Header("댓글")]
    [SerializeField] GameObject _postComment;
    [SerializeField] Transform _intantiateTarget;
    
    private List<GameObject> _comments;
    private UserDatas _user;
    
    bool _isLiked = false;

    private void Start()
    {
        LoadPost();
    }
    
    public void LoadPost()
    {
        // 좋아요 여부에 따른 활성화 체크 db UserPost 참조
        
        for (int i = 0; i < _npcTableRoot.scriptableObjects.Length; i++)
        {
            if (_npcTableRoot.scriptableObjects[i] is Post_NPC_TableSO)// && (_datas.scriptableObjects[i] as Post_TableSO).postID == postNum)
            {
                if (!((_npcTableRoot.scriptableObjects[i] as Post_NPC_TableSO).postImage == _postImgID))
                    continue;

                var so = (_npcTableRoot.scriptableObjects[i] as Post_NPC_TableSO);
                
                // 게시자
                //_poster.text = ServiceLocator.Get<IString_TableManager>().GetStringSO(so.postProFile).KR;

                // 게시물 좋아요 수
                _postLikeCount.text = so.likeCount.ToString();

                // 게시물 사진
                //Debug.Log($"{so.postImage.ToString()} / {so.postImage} / {_postID} / {_postImgID == null}");
                //ServiceLocator.Get<IAddressableManager>().LoadImageSprite(so.postImage.ToString(), _postImg);

                // 해쉬 태크
                _hashTagText.text = _postListComp.GetHashTags(_postID);

                //본문
                _postContent.text = ServiceLocator.Get<IString_TableManager>().GetStringSO(so.captionText).KR;

                // 댓글
                CommentManager();

                break;
            }
        }

        // 좋아요 여부
        //_isLiked = _user.UserPost[_postID.ToString()].isLiked;
        _postLikeImg.SetActive(_isLiked);
    }
    
    private void CommentManager()
    {
        if ( _comments == null)
        {
            _comments = new List<GameObject>(8);
        }
        
        int count = _postListComp.GetCommentCounts(_postID);
        List<PostComment> list = _postListComp.GetComments(_postID);
        GameObject obj;

        for (int i = 0; i < count; i++)
        {
            obj = Instantiate(_postComment, _intantiateTarget);
            obj.name = $"comment_{i}";
            var temp = obj.GetComponent<PostCommentController>();
            temp.SetComment(list[i].comment, list[i].so.npcId.ToString(), list[i].so.npcImage);
        }
    }
}
