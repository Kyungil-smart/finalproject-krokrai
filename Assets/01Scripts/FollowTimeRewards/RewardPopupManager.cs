/*
 작성자 : cminhyeong1999
 작성일 : 26-06-15
 수정일 : 26-06-15

 역할 : Reward 팝업창의 Inspector를 접근하고 관리하기 위한 매니저
 
 방식 : 메인 UI 7시 방향 버튼 클릭 시 나타나는 팝업창의 전반적인 Text Inspector를 접근 및 수정
*/
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardPopupManager : MonoBehaviour
{
    // 현재 레벨 칭호
    [SerializeField] private TMP_Text _tierNameText;
    // 최대 시간 도달까지 남은 시간을 표시
    [SerializeField] private TMP_Text _timeText;
    // 현재 팔로워 수를 표시
    [SerializeField] private TMP_Text _followText;
    // 수령 가능한 에너지 보상 표시
    [SerializeField] private TMP_Text _rewardEnergyText;
    // 수령 가능한 코인 보상 표시
    [SerializeField] private TMP_Text _rewardCoinText;

    // 현재 레벨 칭호와 다음 레벨업 까지 필요한 팔로워 수를 표시하는 팝업창
    [SerializeField] private GameObject _nextTierPopup;
    // 보상 받기 버튼
    [SerializeField] private Button _rewardButton;
    
    // 최대 보상 누적 가능 시간 , 12시간
    private double _maxRewardSeconds = 43200;
    
    /// <summary>
    /// 보상 받기 버튼 클릭시 전달하는 액션
    /// </summary>
    public event Action OnButtonClicked;
    
    private void Awake()
    {
        // 보상 받기 버튼 클릭시 실행
        _rewardButton.onClick.AddListener(OnRewardButtonClick);
        _nextTierPopup.SetActive(false);
    }

    /// <summary>
    /// 현재 레벨에 해당하는 칭호를 표시하는 메서드
    /// </summary>
    /// <param name="tierName">현재 레벨 칭호 텍스트</param>
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
    
    /// <summary>
    /// 보상 받기 버튼의 활성화 여부를 설정하는 메서드
    /// </summary>
    /// <param name="_isCanGetReward">받을 보상이 있으면 true, 아니면  false</param>
    public void SetRewardButton(bool _isCanGetReward) => _rewardButton.interactable = _isCanGetReward;

    /// <summary>
    /// 팝업창에서 X버튼 클릭시 창 off 메서드
    /// </summary>
    public void OnXButtonClicked()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 현재 레벨 칭호 쪽 클릭 시 나타나는 팝업창
    /// </summary>
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
