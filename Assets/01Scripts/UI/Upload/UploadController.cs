/*
 작성자 : krokrai
 작성일 : 26-06-08
 수정일 : 26-06-10

 역할 : Upload system 관리 및 Image 등록
 방식 : prefab화 된 객체를 생성 후 Image를 밀어 넣어 객체를 완성 및 자신을 주입하여 반환 받을 수 있음
 */
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UploadController : MonoBehaviour
{
    [SerializeField] AutoSOGen_ContaineSO _image_Table;
    [SerializeField] AutoSOGen_ContaineSO _folder_Table;
    [SerializeField] AutoSOGen_ContaineSO _post_Table;
    [SerializeField] Image _postImg;
    [SerializeField] TMP_Dropdown _folderDropDown;

    [SerializeField] Transform _spawnPoint;
    [SerializeField] GameObject _postPrefab;

    [SerializeField] Button _upLoadButton;

    public event Action<int> OnUpload;
    [SerializeField] Uploadpopup _popup;

    int _currentPosts = 0;
    int _currentPost;
    int _currentPostImg;
    

    private Dictionary<int, UIAddressableImageLoader> _upLoadImgs = new();
    private Dictionary<int, GameObject> _upLoadobject = new();
    private Dictionary<int, Post_TableSO> _posts = new();
    private Dictionary<int, int> _folder = new();

    private void Awake()
    {
        Post_TableSO _postTableSO;
        Image_TableSO _folderSO;

        foreach (var t in _post_Table.scriptableObjects)
        {
            if (t is Post_TableSO)
            {
                _postTableSO = t as Post_TableSO;
                if (_postTableSO.postImage == 0)
                    continue;
                _posts.Add(_postTableSO.postImage, _postTableSO);
            }
        }

        foreach (var t in _folder_Table.scriptableObjects)
        {
            if (t is Image_TableSO)
            {
                _folderSO = t as Image_TableSO;
                if (_folderSO.imgFolder < 502000)
                    continue;
                _folder.Add(_folderSO.ImgId, _folderSO.imgFolder);
            }
        }
    }

    private void OnEnable()
    {
        _currentPost = 0;
        _folderDropDown.value = 0;

        if (_currentPosts < ServiceLocator.Get<IDataManager>().UserDatas.ImgList.Count)
        {
            var userDatas = ServiceLocator.Get<IDataManager>().UserDatas;
            GameObject obj;
            UIAddressableImageLoader temp;

            foreach(var data in userDatas.ImgList)
            {
                if (!int.TryParse(data.Key, out int key))
                    continue;
                if (_upLoadImgs.ContainsKey(key))
                    continue;
                // 이미지 저장용 한 개  생성
                obj = Instantiate(_postPrefab, _spawnPoint);
                obj.name = $"Post_{key}";
                // 지정하기 위해 컴포넌트 갖고 오기 및 주입
                temp = obj.GetComponent<UIAddressableImageLoader>();
                temp.ChangeImageByAddress(key, _posts[key].postID,this);

                // 관리를 위해 등록
                _upLoadImgs.Add(key, temp);
                _upLoadobject.Add(key, obj);
                _currentPosts++;
            }
        }

        _upLoadButton.onClick.AddListener(OnUpLoadClick);
        _folderDropDown.onValueChanged.AddListener(OnChangeDropDownMenu);
    }

    private void OnDisable()
    {
        _upLoadButton.onClick.RemoveListener(OnUpLoadClick);
        _folderDropDown.onValueChanged.RemoveListener(OnChangeDropDownMenu);
    }

    private void OnChangeDropDownMenu(int value)
    {
        Log.Message(value);
        switch(value)
        {
            case 0:
                foreach(var t in _upLoadobject) // 오브젝트를 갖고옴
                {
                    t.Value.SetActive(true);
                }
                break;
            case 1: case 2: case 3: case 4:
                int i;
                if (_folder_Table.scriptableObjects[value] is Folder_TableSO)
                    i = (_folder_Table.scriptableObjects[value] as Folder_TableSO).folderId;
                else
                    break;
                foreach (var t in _upLoadobject) // 오브젝트를 갖고옴
                {
                    if (_folder.ContainsKey(t.Key))
                    {
                        Log.Message(i);
                        if (_folder[t.Key] == i)
                        {
                            _upLoadobject[t.Key].SetActive(true);
                        }
                        else
                            _upLoadobject[t.Key].SetActive(false);
                    }
                }
                break;
            default:
                Log.Message("등록 되지 않은 폴더 입니다.");
                break;
        }
    }

    private void OnUpLoadClick()
    {
        if ( !(100000 < _currentPost && _currentPost < 200000) )
            return;

        if (ServiceLocator.Get<IDataManager>().UserGoods.Stone_ < 1)
        {
            _popup.SetPopUp();
            return;
        }

        var t = ServiceLocator.Get<IDataManager>();

        t.UserDatas.UserPost.Add(_currentPost.ToString(), new UserPostState());
        t.UserDatas.ImgList[_currentPostImg.ToString()].isUploaded = true;
        
        _upLoadImgs[_currentPostImg].PostedImg();
        _postImg.sprite = null;

        t.ProFile.followerCount += _posts[_currentPostImg].getFollower;

        _currentPost = 0;
        _currentPostImg = 0;

        OnUpload?.Invoke(_currentPost);

        Log.Message($"등록 됌 : {_currentPost}");
    }

    public void SetPost(int imgID,int PostID)
    {
        _currentPost = PostID;
        _currentPostImg = imgID;
        ServiceLocator.Get<IAddressableManager>().LoadImageSprite(_currentPostImg.ToString(),_postImg);
    }
}
