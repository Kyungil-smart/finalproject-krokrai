/*
 작성자 : krokrai
 작성일 : 26-06-04

 역할 : 게시물에 필요한 정보를 출력
 방식 : 댓글을 제외한 모든 데이터를 이 곳에서 처리 및 댓글은 객체로 분리
 */

using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostController : MonoBehaviour
{
    [Header("데이터 테이블")]
    [SerializeField] private AutoSOGen_ContaineSO _datas;
    [SerializeField] private AutoSOGen_ContaineSO _hashTag;

    [Header("유저 정보")]
    [SerializeField] TextMeshProUGUI _userName;

    [Header("게시물")]
    [SerializeField] Image _postImg;
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

    private int _postNum;

    bool _isLiked = false;

    private void Awake()
    {
        _userName.text = ServiceLocator.Get<IBackendManager>().Auth.CurrentUser.DisplayName;
    }

    private void OnEnable()
    {
        _user = ServiceLocator.Get<IDataManager>().UserDatas;
    }

    public void SetPost(int postNum)
    {
        if (postNum < 101000 || 190000 < postNum)
        {
            Log.Message($"잘 못된 숫자 입력 {postNum}");
            return;
        }

        // 좋아요 여부에 따른 활성화 체크 db UserPost 참조

        for (int i = 0; i < _datas.scriptableObjects.Length; i++)
        {
            if (_datas.scriptableObjects[i] is Post_TableSO && (_datas.scriptableObjects[i] as Post_TableSO).postID == postNum)
            {
                var so = (_datas.scriptableObjects[i] as Post_TableSO);
                // 게시물 좋아요 수
                _postLikeCount.text = so.likeCount.ToString();
                // 게시물 사진
                ServiceLocator.Get<IAddressableManager>().LoadImageSprite(so.postImage.ToString(), _postImg);

                // 해쉬 태크
                _hashTagText.text = _postListComp.GetHashTags(postNum);

                //본문
                _postContent.text = ServiceLocator.Get<IString_TableManager>().GetStringSO(postNum.ToString()).KR; // 언어 설정 어디서 함?

                // 댓글
                CommentManager();

                break;
            }
        }

        // 좋아요 여부
        _isLiked = false;
        _postLikeImg.SetActive(_isLiked);
        // 여기에 좋아요 표시한 게시물 UserPost에 접근해서 상태 전환
    }

    private void CommentManager()
    {
        if ( _comments == null)
        {
            _comments = new List<GameObject>(8);
        }
        
        int count = _postListComp.GetCommentCounts();
        List<PostComment> list = _postListComp.GetComments(_postNum);
        GameObject obj;

        for (int i = 0; i < count; i++)
        {
            obj = Instantiate(_postComment, _intantiateTarget);
            obj.name = $"comment_{i}";
            var temp = obj.GetComponent<PostCommentController>();
            temp.SetComment(list[i].comment, list[i].so.npcId.ToString(), list[i].so.npcImage); // TODO : 여기까지 완성 테스트 필요
        }
    }

    private void OnClickHeart()
    {
        _isLiked = !_isLiked;
        _postLikeImg.SetActive(_isLiked);
    }
}
