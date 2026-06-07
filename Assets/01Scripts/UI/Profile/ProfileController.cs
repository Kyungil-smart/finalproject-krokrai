/*
 작성자 : krokrai
 작성일 : 26-06-04
 
 역할 : Profile tab에 들어왔을 때 Profile에 필요한 Data를 등록
 방식 : DataManager에 등록된 Data를 갖고 와서 등록 및 게시물이 추가 등록 되었는 지 판정 및 생성
 */

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileController : MonoBehaviour
{
    [Header("자원")]
    [SerializeField] private AutoSOGen_ContaineSO _postTable;

    [Header("프로필")]
    [SerializeField] private TextMeshProUGUI _profileName;
    [SerializeField] private TextMeshProUGUI _postCount;
    [SerializeField] private TextMeshProUGUI _profileFollower;
    [SerializeField] private TextMeshProUGUI _profileFollowing;
    [SerializeField] private Image _profileImage;
    
    [Header("게시물")]
    [SerializeField] private Transform _scrollView;

    [Header("게시물 prefab")]
    [SerializeField] private GameObject _postPrefab;

    List<GameObject> _posts = new(8);

    Dictionary<int, Post_TableSO> _postTables = new();

    private int _currentPostNum;
    private int _postedCount;

    private void Awake()
    {
        Post_TableSO post;
        foreach (var t in _postTable.scriptableObjects)
        {
            if (t is Post_TableSO)
            {
                post = t as Post_TableSO;

                _postTables.Add(post.postID, post);
            }
        }
    }

    private void OnEnable()
    {
        //_postedCount = ServiceLocator.Get<IDataManager>().UserDatas.UserPost.Count;

        if (true)//_currentPostNum < _postedCount)
        {
            // ProfilePostController 선언 후 생성 요청
            GameObject obj;
            ProfilePostController post;

            var imgs = new UserDatas().UserPost;//ServiceLocator.Get<IDataManager>().UserDatas.UserPost;
            imgs.Add("102001", false);

            _postedCount = 1;

            List<int> keys = new List<int>(imgs.Count);
            int count = 0;

            foreach(var s in imgs)
            {
                if (!int.TryParse(s.Key, out int t))
                {
                    Log.Message($"UserPost에 잘 못된 키 값이 검출 되었습니다.{s.Key}");
                }
                keys.Add(t);
                count++;
            }

            for (int i = _currentPostNum; i < _postedCount; i++)
            {
                obj = Instantiate(_postPrefab,_scrollView);
                obj.name = $"Post_{i}";
                post = obj.GetComponent<ProfilePostController>();
                if (_postTables.ContainsKey(keys[i]))
                    post.SetPost( _postTables[keys[i]].postImage);
                else
                {
                    Log.Message("Table에 존재하지 않습니다.");
                    return;
                }
                _posts.Add(obj);
            }

            _currentPostNum = _postedCount;
            _postCount.text = _currentPostNum.ToString();
        }

        _profileFollower.text = "101"; //ServiceLocator.Get<IDataManager>().ProFile.followerCount.ToString();
        _profileFollowing.text = "1111"; // ServiceLocator.Get<IDataManager>().ProFile.followingCount.ToString();
        /*ServiceLocator.Get<IAddressableManager>().LoadImageSprite(
            ServiceLocator.Get<IDataManager>().ProFile.profileImage.ToString(),
            _profileImage);*/
    }

    private void Start()
    {
        //_profileName.text = ServiceLocator.Get<IBackendManager>().Auth.CurrentUser.UserId;
    }
}
