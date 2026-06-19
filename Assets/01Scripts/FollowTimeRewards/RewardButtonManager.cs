/*
 작성자 : cminhyeong1999
 작성일 : 26-06-15
 수정일 : 26-06-15

 역할 : 메인메뉴에 위치한 Reward 버튼창의 Inspector를 접근하고 관리하기 위한 매니저
*/
using UnityEngine;


public class RewardButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject _rewardIconYellow;
    [SerializeField] private GameObject _rewardIconRed;

    [SerializeField] private GameObject _rewardPopup;
    private bool _isPopupOpen;

    private void Awake()
    {
        _isPopupOpen = false;
        _rewardPopup.SetActive(false);
    }

    /// <summary>
    /// 받을 보상이 있을 경우 노란색 아이콘을 표시하기 위한 메서드
    /// </summary>
    /// <param name="active">단순 bool 값</param>
    public void SetRewardIconYellowActive(bool active) => _rewardIconYellow.SetActive(active);
    /// <summary>
    /// 받아야 할 보상이 꽉 찼을 경우 빨간색 아이콘을 표시하기 위한 메서드
    /// </summary>
    /// <param name="active">단순 bool 값</param>
    public void SetRewardIconRedActive(bool active) => _rewardIconRed.SetActive(active);

    /// <summary>
    /// 팝업창 On 메서드
    /// </summary>
    public void OnButtonClick()
    {
        _rewardPopup.SetActive(true);
    }
}
