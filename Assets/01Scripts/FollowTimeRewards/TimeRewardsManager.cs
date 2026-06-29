/*
 작성자 : cminhyeong1999
 작성일 : 26-06-15
 수정일 : 26-06-15

 역할 : 전반적인 방치 관련 시스템을 관리하기 위한 매니저
        접속중인 유저의 현재 팔로워, 마지막 로그인 정보, 코인 및 에너지 지급은 여기서 받아서 관련 처리 함
        
 방식 : 처음 접속 시 마지막 로그인 시간과 팔로워 수를 받아 (현재 접속시간 - 마지막 로그인 시간) 만큼의 오프라인 보상을 누적
       Coroutine을 1초마다 실행하여 온라인 접속 시 초당 보상을 누적, 최대 12시간 까지 보상을 누적
       "보상 받기"를 눌러 보상을 수령, 수령 시 RealtimeDatabase에 에너지와 코인을 추가
*/
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeRewardsManager : MonoBehaviour
{
    [Header("RBM = RewardButtonManager / RPM = RewardPopupManager / FLM = FollowLevelSOManager")]
    [SerializeField] private RewardButtonManager _rBM;
    [SerializeField] private RewardPopupManager _rPM;
    [SerializeField] private FollowLevelSOManager _fLM;
    [SerializeField] private NextTierPopUpManager _nTPM;
    
    private TimeSpan _onlineTotalTime;
    private TimeSpan _offlineTotalTime;
    
    private DateTime _lastLoginTime;
    private DateTime _startLoginTime;
    
    private readonly float _maxRewardSeconds = 43200f;

    // 현재 방치하여 얻은 에너지, 코인의 양
    // int, float 두 값으로 있는 이유는 보상으로 주는 코인은 정수로 지급하고, 나머지 소수부분을 남기기 위해서
    private int _rewardEnergyIntValue;
    private float _rewardEnergyFloatValue;
    private int _rewardCoinIntValue;
    private float _rewardCoinFloatValue;
    
    // 팔로우 레벨에 비례해서 최대로 얻을 수 있는 에너지, 코인의 최대 수치
    private float _maxRewardEnergyFloatValue;
    private float _maxRewardCoinFloatValue;

    // 현재 접속한 유저의 팔로우 값
    private int _follow;
    
    // 1초를 기다리기 위함
    private readonly WaitForSeconds _oneSecondDelay = new WaitForSeconds(1.0f);

    public int TESTFOLLOW;

    private void Awake()
    {
        _startLoginTime = DateTime.Now;
    }
    
    private void Start()
    {
        GetLastLoginTime();
        // 초마다 실행할 코루틴
        StartCoroutine(UpdateForSeconds());
    }

    // RewardPopupManager에 있는 보상 받기 버튼을 인식하기 위함
    private void OnEnable()
    {
        _rPM.OnButtonClicked += OnRewardButtonClicked;
    }

    private void OnDisable()
    {
        _rPM.OnButtonClicked -= OnRewardButtonClicked;
    }

    private void GetLastLoginTime()
    {
        _lastLoginTime = ServiceLocator.Get<IDataManager>().Attendance.Last_Login_TimeStamp;
        _follow = (int)ServiceLocator.Get<IDataManager>().ProFile.followerCount;
        
        
        _offlineTotalTime = DateTime.Now - _lastLoginTime;
        
        // 오프라인으로 유지된 시간을 계산 후에 그만큼 보상을 추가
        // 만약 12시간 넘어서 오프라인을 유지하다 접속하였을 경우, 12시간만큼만 보상을 주도록 제한
        double offlineSeconds = Math.Min(_offlineTotalTime.TotalSeconds, _maxRewardSeconds);

        // 최초 로그인 시 _maxRewardEnergyFloatValue, _maxRewardCoinFloatValue가 어떤 값인지 설정
        CheckMaxReward(_follow);
        
        // 오프라인에서 경과한 시간만큼 얻을 방치형 보상
        _rewardEnergyFloatValue += _maxRewardEnergyFloatValue * (float)(offlineSeconds / _maxRewardSeconds);
        _rewardCoinFloatValue += _maxRewardCoinFloatValue * (float)(offlineSeconds / _maxRewardSeconds);
        
        // 보상 버튼 활성화 여부
        CanActiveRewardButton();
        
        // 온라인 토탈 시간에 오프라인 경과시간 추가
        _onlineTotalTime += _offlineTotalTime;
    }
    
    private IEnumerator UpdateForSeconds()
    {
        while (true)
        {
            // (최대 시간 - 흘러간 전체 시간) 으로 남은시간 UI 갱신
            double totalSeconds = Math.Min(_onlineTotalTime.TotalSeconds, _maxRewardSeconds);
            // 시간 표시
            _rPM.SetTimeText(totalSeconds);

            // 팔로워에 비례한 최대 보상 수치 체크 및 설정
            CheckMaxReward(_follow);
            // 보상 받기
            GetReward();
            // 메인메뉴에 있는 리워드 버튼의 노란색, 빨간색 아이콘 설정
            SetRewardIconUpdate();
            // NextTierPopUp에 있는 현재 레벨 칭호, 다음 레벨 도달에 필요한 팔로워 수 표시
            SetNextTierPopUpText();
            // 에너지, 코인 둘 중 하나라도 받을 보상이 있어야만 버튼이 활성화 되게끔
            CanActiveRewardButton();
            
            // 팝업창에 표시할 현재 레벨 칭호
            _rPM.SetTierName(_fLM.GetFollowTierName(_follow));
            // 팝업창에 표시할 현재 수령 가능한 에너지, 코인 수량 표시
            _rPM.SetRewardText(_rewardEnergyIntValue, _rewardCoinIntValue);
            // 팝업창에 표시할 현재 팔로워 수
            _rPM.SetFollowText(_follow);
            
            // 온라인 접속시 1초 누적
            _onlineTotalTime += TimeSpan.FromSeconds(1);
            // 1초 마다 실행
            yield return _oneSecondDelay;
        }
    }

    // 1초마다 보상을 누적하는 방식
    private void GetReward()
    {
        // 초당 얻는 에너지, 코인 양
        float energyPerSecond = _maxRewardEnergyFloatValue / _maxRewardSeconds;
        float coinPerSecond = _maxRewardCoinFloatValue / _maxRewardSeconds;
        
        // 최대치를 넘지 않았을 때만 1초치 보상을 누적 (+=)
        if (_rewardEnergyFloatValue < _maxRewardEnergyFloatValue)
        {
            _rewardEnergyFloatValue = Mathf.Min(_rewardEnergyFloatValue + energyPerSecond, _maxRewardEnergyFloatValue);
        }
        
        if (_rewardCoinFloatValue < _maxRewardCoinFloatValue)
        {
            _rewardCoinFloatValue = Mathf.Min(_rewardCoinFloatValue + coinPerSecond, _maxRewardCoinFloatValue);
        }
        
        // 각 보상들의 정수 부분만 따로 가져옴
        _rewardEnergyIntValue = Mathf.FloorToInt(_rewardEnergyFloatValue);
        _rewardCoinIntValue = Mathf.FloorToInt(_rewardCoinFloatValue);
    }

    // RewardPopupManager에 있는 보상받기 버튼 클릭시 실제로 실행되는 메서드
    private void OnRewardButtonClicked()
    {
        // Todo : 이 부분에서 정수 부분 보상만큼을 RealtimeDatabase에 전달
        ServiceLocator.Get<IDataManager>().UserGoods.Energy_ += _rewardEnergyIntValue;
        ServiceLocator.Get<IDataManager>().UserGoods.Coin_ += _rewardCoinIntValue;
        // 소수 부분만 남김
        _rewardEnergyFloatValue -= _rewardEnergyIntValue;
        _rewardCoinFloatValue -= _rewardCoinIntValue;
        // 정수 부분 초기화
        _rewardEnergyIntValue = 0;
        _rewardCoinIntValue = 0;
        // 누적된 시간 초기화
        _onlineTotalTime = TimeSpan.Zero;
        // 텍스트 갱신
        _rPM.SetRewardText(_rewardEnergyIntValue, _rewardCoinIntValue);
    }

    private void SetRewardIconUpdate()
    {
        // 에너지나 코인 둘 다 받을 보상이 없을 경우
        if (_rewardEnergyIntValue == 0 && _rewardCoinIntValue == 0)
        {
            _rBM.SetRewardIconYellowActive(false);
            _rBM.SetRewardIconRedActive(false);
        } // 에너지 또는 코인 둘 중 하나가 받을 보상이 있을 경우
        else if ((_rewardEnergyIntValue > 0 || _rewardCoinIntValue > 0) 
            && _rewardEnergyIntValue < _maxRewardEnergyFloatValue && _rewardCoinIntValue < _maxRewardCoinFloatValue)
        {
            _rBM.SetRewardIconYellowActive(true);
            _rBM.SetRewardIconRedActive(false);
        } // 에너지, 코인 둘 다 최대치까지 보상이 쌓여 있을 경우
        else if (_rewardEnergyIntValue >= _maxRewardEnergyFloatValue &&
                  _rewardCoinIntValue >= _maxRewardCoinFloatValue)
        {
            _rBM.SetRewardIconYellowActive(false);
            _rBM.SetRewardIconRedActive(true);
        }
    }

    // 현재 팔로워 수에 따른 최대 보상치 체크
    private void CheckMaxReward(int follow)
    {
        _maxRewardEnergyFloatValue = _fLM.GetEnergyLevel(follow);
        _maxRewardCoinFloatValue = _fLM.GetCoinLevel(follow);
    }

    // 에너지, 코인 둘 중 하나라도 받을 보상이 있어야만 버튼이 활성화 되게끔
    private void CanActiveRewardButton()
    {
        if(_rewardEnergyIntValue <= 0 && _rewardCoinIntValue <= 0) 
            _rPM.SetRewardButton(false);
        else 
            _rPM.SetRewardButton(true);
    }

    // 현재 레벨 이름과 다음 레벨까지 필요한 팔로워수를 알려주는 팝업창
    private void SetNextTierPopUpText()
    {
        _nTPM.SetTierName(_fLM.GetFollowTierName(_follow));
        _nTPM.SetNextTierFollowValueText(_fLM.GetNextTierFollowValue(_follow).ToString());
    }
}
