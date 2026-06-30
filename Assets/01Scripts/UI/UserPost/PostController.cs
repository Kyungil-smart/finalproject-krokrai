/*
 작성자 : krokrai
 작성일 : 26-06-04
 수정일 : 26-06-29

 역할 : 게시물에 필요한 정보를 출력
 방식 : 댓글을 제외한 모든 데이터를 이 곳에서 처리 및 댓글은 객체로 분리
 */

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostController : MonoBehaviour
{
    [Header("실제 정보")]
    [SerializeField] private GameObject _posts;
    [Header("데이터 테이블")]
    [SerializeField] private AutoSOGen_ContaineSO _datas;
    [SerializeField] private AutoSOGen_ContaineSO _hashTag;

    [Header("유저 정보")]
    [SerializeField] TextMeshProUGUI _userName;

    [Header("게시물")]
    [SerializeField] Image _postImgUI;
    [SerializeField] Button _postLike;
    [SerializeField] GameObject _postLikeImg;
    [SerializeField] Button _postShare;
    [SerializeField] TextMeshProUGUI _postLikeCount;
    [SerializeField] TextMeshProUGUI _hashTagText;
    [SerializeField] TextMeshProUGUI _postContent;

    [Header("댓글")]
    [SerializeField] GameObject _postComment;
    [SerializeField] Transform _intantiateTarget;

    [SerializeField] PostListCompression _postListComp;

    private List<GameObject> _comments;
    private UserDatas _user;

    private int _postImgNum;
    private int _postId;

    bool _isLiked = false;

    private void Awake()
    {
        _userName.text = ServiceLocator.Get<IDataManager>().UserName;
    }

    private void OnEnable()
    {
        _user = ServiceLocator.Get<IDataManager>().UserDatas;
        _postLike.onClick.AddListener(OnClickHeart);
    }

    private void OnDisable()
    {
        _postLike.onClick.RemoveListener(OnClickHeart);
    }

    public void SetPost(int postImg, int postId)
    {
        _posts.SetActive(true);
        _postImgNum = postImg;
        _postId = postId;

        // 좋아요 여부에 따른 활성화 체크 db UserPost 참조
        
        for (int i = 0; i < _datas.scriptableObjects.Length; i++)
        {
            if (_datas.scriptableObjects[i] is Post_TableSO)// && (_datas.scriptableObjects[i] as Post_TableSO).postID == postNum)
            {
                if (!((_datas.scriptableObjects[i] as Post_TableSO).postImage == postImg))
                    continue;

                var so = (_datas.scriptableObjects[i] as Post_TableSO);

                // 게시물 좋아요 수
                _postLikeCount.text = so.likeCount.ToString();

                // 게시물 사진
                ServiceLocator.Get<IAddressableManager>().LoadImageSprite(so.postImage.ToString(), _postImgUI);

                // 해쉬 태크
                _hashTagText.text = _postListComp.GetHashTags(_postId);

                //본문
                _postContent.text = ServiceLocator.Get<IString_TableManager>().GetStringSO(so.captionText).KR;

                // 댓글
                CommentManager();

                break;
            }
        }

        // 좋아요 여부
        _isLiked = _user.UserPost[_postId.ToString()].isLiked;
        _postLikeImg.SetActive(_isLiked);
    }

    private void DeletComment()
    {
        for (int i = 0; i < _comments.Count; i++)
        {
            Destroy(_comments[i]);
        }
        _comments.Clear();
    }

    private void CommentManager()
    {
        if (_comments == null) _comments = new List<GameObject>(8);
        else if (0 < _comments.Count) DeletComment();

        int count = _postListComp.GetCommentCounts(_postId);
        List<PostComment> list = _postListComp.GetComments(_postId);
        GameObject obj;

        for (int i = 0; i < list.Count; i++)
        {
            obj = Instantiate(_postComment, _intantiateTarget);
            obj.name = $"comment_{i}";
            var temp = obj.GetComponent<PostCommentController>();
            temp.SetComment(list[i].comment, list[i].so.npcId.ToString(), list[i].so.npcImage);
            _comments.Add(obj);
        }
    }

    private void OnClickHeart()
    {
        _isLiked = !_isLiked;
        _user.UserPost[_postImgNum.ToString()].isLiked = _isLiked;
        _postLikeImg.SetActive(_isLiked);
    }
}
