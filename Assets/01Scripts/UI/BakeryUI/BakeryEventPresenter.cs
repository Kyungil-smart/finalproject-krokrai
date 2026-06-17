using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeryEventPresenter : MonoBehaviour
{
    [SerializeField] private BakeryEventView _bakeryView;

    private readonly int[] _targetTimes = { 15, 30, 45, 60 };
    
    void Start()
    {
        var eventManager = ServiceLocator.Get<IEventManager>();

        if (eventManager != null)
        {
            eventManager.OnPlayTimeUpdated += HandlePlayTimeUpdated;
            eventManager.OnMidnightReset += HandleMidnightReset;
            eventManager.OnBakeryCoinChanged += HandleCoinChanged;
        }
        
        if (_bakeryView != null)
        {
            _bakeryView.OnClaimButtonClicked += HandleClaimReward;
        }

        InitUI();
    }

    private void OnDestroy()
    {
        var eventManager = ServiceLocator.Get<IEventManager>();

        if (eventManager != null)
        {
            eventManager.OnPlayTimeUpdated -= HandlePlayTimeUpdated;
            eventManager.OnMidnightReset -= HandleMidnightReset;
            eventManager.OnBakeryCoinChanged -= HandleCoinChanged;
        }
        
        if (_bakeryView != null)
        {
            _bakeryView.OnClaimButtonClicked -= HandleClaimReward;
        }
    }

    private void InitUI()
    {
        var dataManager = ServiceLocator.Get<IDataManager>();
        if (dataManager == null || dataManager.UserDatas == null) return;

        var bakeryDB = dataManager.UserDatas.NyangBakery;
        int currentMinutes = bakeryDB.accTime.Minute;
        
        _bakeryView.UpdatePlayTimeUI(currentMinutes);
        _bakeryView.UpdateCoinUI(bakeryDB.bakerycoin);

        RefreshButtonState(currentMinutes);
    }
    
    
    private void HandlePlayTimeUpdated(int currentMinutes)
    {
        _bakeryView.UpdatePlayTimeUI(currentMinutes);
        RefreshButtonState(currentMinutes);
    }
    
    private void HandleMidnightReset()
    {
        _bakeryView.UpdatePlayTimeUI(0);
        RefreshButtonState(0);
    }
    
    private void HandleCoinChanged(int currentCoin)
    {
        _bakeryView.UpdateCoinUI(currentCoin);
    }
    
    private void RefreshButtonState(int currentMinutes)
    {
        var bakeryDB = ServiceLocator.Get<IDataManager>().UserDatas.NyangBakery;

        for (int i = 0; i < _targetTimes.Length; i++)
        {
            int target = _targetTimes[i];
            string key = $"rewards_{target}min";
            var rewardState = bakeryDB.rewardHistory[key];
            
            if (rewardState.RewardTime)
            {
                _bakeryView.SetButtonState(i, "Claimed");
            }
            else if (currentMinutes >= target)
            {
                _bakeryView.SetButtonState(i, "Ready");
            }
            else
            {
                _bakeryView.SetButtonState(i, "Locked");
            }
        }
    }
    
    private void HandleClaimReward(int index)
    {
        if (index < 0 || index >= _targetTimes.Length) return;
        
        var dataManager = ServiceLocator.Get<IDataManager>();
        var bakeryDB = dataManager.UserDatas.NyangBakery;
        
        int target = _targetTimes[index];
        string key = $"rewards_{target}min";
        var rewardState = bakeryDB.rewardHistory[key];

        if (rewardState.RewardTime || bakeryDB.accTime.Minute < target) return;

        // 롤백용 데이터 백업
        int originalCoin = bakeryDB.bakerycoin;
        bool originalClaimed = rewardState.RewardTime;
        DateTime originalTime = rewardState.claimedAt;

        try
        {
            rewardState.RewardTime = true;
            rewardState.claimedAt = DateTime.Now;
            bakeryDB.bakerycoin += 10;

            _bakeryView.UpdateCoinUI(bakeryDB.bakerycoin);
            _bakeryView.SetButtonState(index, "Claimed");

            Log.Message($"[{key}] 보상 수령 성공, 현재 코인: {bakeryDB.bakerycoin}");
        }
        catch (Exception e)
        {
            // 롤백
            rewardState.RewardTime = originalClaimed;
            rewardState.claimedAt = originalTime;
            bakeryDB.bakerycoin = originalCoin;
            
            Debug.LogError($"저장 실패함. 에러: {e.Message}");
        }
    }
}
