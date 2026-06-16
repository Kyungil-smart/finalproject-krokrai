/*
 작성자 : cminhyeong1999
 작성일 : 26-06-15
 수정일 : 26-06-15

 역할 : Reward 팝업창의 Inspector를 접근하고 관리하기 위한 매니저
*/
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardPopupManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _tierNameText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _followText;
    [SerializeField] private TMP_Text _rewardEnergyText;
    [SerializeField] private TMP_Text _rewardCoinText;

    [SerializeField] private GameObject _nextTierPopup;
    [SerializeField] private Button _rewardButton;
    
    private double _maxRewardSeconds = 43200;
    
    public event Action OnButtonClicked;
    
    private void Awake()
    {
        // 보상 받기 버튼 클릭시 실행
        _rewardButton.onClick.AddListener(OnRewardButtonClick);
        _nextTierPopup.SetActive(false);
    }

    public void SetTierName(string tierName) => _tierNameText.SetText(tierName);
    
    /// <summary>
    /// 최대까지 남은 시간을 표시하는 text 메서드, 12시간을 기준
    /// </summary>
    /// <param name="seconds">초를 기준으로 계산</param>
    public void SetTimeText(double seconds)
    {
        // 만약 시간이 0보다 작을 경우 0으로 고정
        double showSeconds = Math.Max(0, _maxRewardSeconds - seconds);
        TimeSpan timeSpan = TimeSpan.FromSeconds(showSeconds);
        
        _timeText.SetText("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    /// <summary>
    /// 현재까지 누적된 보상들을 표시하는 메서드
    /// </summary>
    /// <param name="rewardEnergy">표시할 에너지</param>
    /// <param name="rewardCoin">표시할 코인</param>
    public void SetRewardText(int rewardEnergy, int rewardCoin)
    {
        _rewardEnergyText.SetText("{0}", rewardEnergy);
        _rewardCoinText.SetText("{0}", rewardCoin);
    }

    /// <summary>
    /// 팔로워 숫자를 표시하는 메서드
    /// </summary>
    /// <param name="follow">팔로워 숫자</param>
    public void SetFollowText(int follow) => _followText.SetText("{0}", follow);
    
    
    public void SetRewardButton(bool _isCanGetReward) => _rewardButton.interactable = _isCanGetReward;

    /// <summary>
    /// 팝업창에서 X버튼 클릭시 창 off 메서드
    /// </summary>
    public void OnXButtonClicked()
    {
        gameObject.SetActive(false);
    }

    public void OnNextTierPopupButtonClicked()
    {
        _nextTierPopup.SetActive(true);
    }

    // TimeRewardsManager에서 보상받기 버튼 클릭을 인식하기 위함
    private void OnRewardButtonClick()
    {
        // 버튼이 클릭 가능한 상태일 때만 이벤트를 발생시킴
        if (_rewardButton != null && _rewardButton.interactable)
        {
            OnButtonClicked?.Invoke();
        }
    }
}
