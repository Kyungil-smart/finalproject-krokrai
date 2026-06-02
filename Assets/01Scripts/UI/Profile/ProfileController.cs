using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _profileName;
    [SerializeField] private TextMeshProUGUI _postCount;
    [SerializeField] private TextMeshProUGUI _profileFollower;
    [SerializeField] private TextMeshProUGUI _profileFollowing;
    [SerializeField] private Image _profileImage;

    

    private int _currentPostNum;
    private int _postedCount;

    private void OnEnable()
    {
        _postedCount = ServiceLocator.Get<IDataManager>().UserDatas.UserPost.Count;
        if (_currentPostNum != _postedCount)
        {
            // ProfilePostController 선언 후 생성 요청
            _currentPostNum = _postedCount;
            _postCount.text = _currentPostNum.ToString();
        }
        
        _profileName.text = ServiceLocator.Get<IDataManager>().ProFile.followerCount.ToString();
        _profileName.text = ServiceLocator.Get<IDataManager>().ProFile.followingCount.ToString();
        ServiceLocator.Get<IAddressableManager>().LoadImageSprite(
            ServiceLocator.Get<IDataManager>().ProFile.profileImage.ToString(),
            _profileImage);
    }

    private void Start()
    {
        _profileName.text = ServiceLocator.Get<IBackendManager>().Auth.CurrentUser.UserId;
    }
}
