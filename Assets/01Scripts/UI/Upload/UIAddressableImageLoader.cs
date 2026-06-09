using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.AddressableAssets;
//using UnityEngine.ResourceManagement.AsyncOperations;

public class UIAddressableImageLoader : MonoBehaviour
{
    [Header("연결할 UI 컴포넌트")]
    [SerializeField] private Image _targetImage;
    [SerializeField] private GameObject _alreadyImg;
    [SerializeField] private Button _button;

    private UploadController _controller;
    
    private bool _isPosted;
    private int _imgIndex;
    private int _postID;

    private void Awake()
    {
        _isPosted = false;
        _alreadyImg.SetActive(false);
    }

    private void OnEnable()
    {
        if (_isPosted)
        {
            // 잠금 및 포스트 출력 등록
            _button.onClick.AddListener(AlreadyPosted);
        }
        else
        {
            // 클릭 시 등록
            _button.onClick.AddListener(PostImgs);
        }
    }

    private void OnDisable()
    {
        if (_isPosted)
        {
            // 잠금 및 포스트 출력 등록
            _button.onClick.RemoveListener(AlreadyPosted);
        }
        else
        {
            // 클릭 시 등록
            _button.onClick.RemoveListener(PostImgs);
        }
    }

    /// <summary>
    /// 외부에서 문자열 주소를 인자값으로 넘겨주면 이미지를 로드하여 UI에 연결
    /// </summary>
    /// <param name="imageAddress">어드레서블 그룹창에 등록한 에셋 주소 (예: "Item_Icon_01")</param>
    public void ChangeImageByAddress(int imageAddress, UploadController ctrl)//int postId ,UploadController ctrl)
    {
        // 참고 사항 : AddressableManager가 존재합니다. 해당 Manager에 접근해서 Image를 교체하는 방식으로 진행하십시오.
        ServiceLocator.Get<IAddressableManager>().LoadImageSprite(imageAddress.ToString(), _targetImage);
        _imgIndex = imageAddress;
        _controller = ctrl;
        //_postID = postId;
        // 새로운 이미지를 로드하기 전에, 기존에 로드했던 이미지가 있다면 메모리에서 해제
        //ReleaseCurrentSprite();

        // 문자열 주소를 사용해 Sprite 타입으로 비동기 로드 시작
        //Addressables.LoadAssetAsync<Sprite>(imageAddress).Completed += OnSpriteLoaded;
    }

    // 포스트
    public void PostImgs()
    {
        _controller.SetPost(_imgIndex);
    }

    // 이미 포스트 된 경우
    public void AlreadyPosted()
    {
        //ServiceLocator.Get<UIManager>().tostUI();
        Log.Message("이미 등록 되어 있습니다.");
    }
    
    // 포스트 된 경우 전환
    public void PostedImg()
    {
        _isPosted = true;
        _alreadyImg.SetActive(true);
        _targetImage.color = _targetImage.color / 2;
        _button.onClick.RemoveListener(PostImgs);
        _button.onClick.AddListener(AlreadyPosted);
    }
}
