using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _profileName;
    [SerializeField] private TextMeshProUGUI _profileFollower;
    [SerializeField] private TextMeshProUGUI _profileFollowing;
    [SerializeField] private Image _profileImage;

    private void OnEnable()
    {
        _profileName.text = ServiceLocator.Get<IDataManager>().ProFile.NickName;
        _profileName.text = ServiceLocator.Get<IDataManager>().ProFile.followerCount.ToString();
        _profileName.text = ServiceLocator.Get<IDataManager>().ProFile.followingCount.ToString();
        ServiceLocator.Get<IAddressableManager>().LoadImageSprite(
            ServiceLocator.Get<IDataManager>().ProFile.profileImage.ToString(),
            _profileImage);
    }
}
