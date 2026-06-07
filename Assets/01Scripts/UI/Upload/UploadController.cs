using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UploadController : MonoBehaviour
{
    [SerializeField] AutoSOGen_ContaineSO _image_Table;
    [SerializeField] AutoSOGen_ContaineSO _folder_Table;
    [SerializeField] Image _postImg;
    [SerializeField] TMP_Dropdown _folderDropDown;

    [SerializeField] Transform _spawnPoint;
    [SerializeField] GameObject _postPrefab;

    [SerializeField] Button _upLoadButton;

    int _currentPosts = 0;
    int _currentPost;
    

    private Dictionary<int, UIAddressableImageLoader> _upLoadImgs = new();
    private Dictionary<int, GameObject> _upLoadobject = new();
    private Dictionary<int, int> _folders = new();

    private void Awake()
    {
        Image_TableSO _imgTableSO;

        foreach (var t in _image_Table.scriptableObjects)
        {
            if (t is Image_TableSO)
            {
                _imgTableSO = t as Image_TableSO;
                if (_imgTableSO.ImgId < 502000)
                    continue;
                _folders.Add(_imgTableSO.ImgId, _imgTableSO.imgFolder);
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
                temp.ChangeImageByAddress(key,this);

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
                    if (_folders.ContainsKey(t.Key))
                    {
                        Log.Message(i);
                        if (_folders[t.Key] == i)
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
        Log.Message(_currentPost);
        if (_currentPost == 0 || !(100000 < _currentPost && _currentPost < 200000))
            return;

        ServiceLocator.Get<IDataManager>().UserDatas.UserPost.Add(_currentPost.ToString(), false);

        _upLoadImgs[_currentPost].PostedImg();
        _postImg.sprite = null;

        Log.Message($"등록 됌 : {_currentPost}");
    }

    public void SetPost(int PostID)
    {
        _currentPost = PostID;
        ServiceLocator.Get<IAddressableManager>().LoadImageSprite(_currentPost.ToString(),_postImg);
    }
}
