/*
작성자 : NekioEmilia
수정자 : NekioEmilia
 
작성일 : 26-06-05
수정일 : 26-06-15

역할 : 출석체크 UI의 Festa Slider와 보물상자에 접근한 Presenter 스크립트 FestaView와 통신
방식 : Presenter를 UI에 적용시키면 View도 같이 따라옴
*/

using System;
using UnityEngine;

[RequireComponent(typeof(FestaView))]
public class FestaPresenter : MonoBehaviour
{
    private const int MAX_FESTA_CHEST_COUNT = 7;
    
    private int _totalFestaPoint;
    private int _recentGaugeStep;
    private bool _finalRewardReceived;

    [SerializeField] private GaugeDataModel _gaugeModel;
    [SerializeField] private FestaView _view;
    [SerializeField] private RewardDataModel _rewardModel;
    [SerializeField] private RewardPopupView _rewardPopupView;
    
    private void OnEnable() // Start -> OnEnable
    {
        var eventManager = ServiceLocator.Get<IEventManager>();

        if (eventManager != null)
        {
            eventManager.OnGaugeIncrease += HandleGaugeIncrease;
        }

        if (_view != null)
        {
            _view.OnChestClicked += HandleChestClick;
        }
        
        SetPoint();
        ReFreshUI();
    }

    private void OnDisable()
    {
        var eventManager = ServiceLocator.Get<IEventManager>();

        if (eventManager != null)
        {
            eventManager.OnGaugeIncrease -= HandleGaugeIncrease;
        }

        if (_view != null)
        {
            _view.OnChestClicked -= HandleChestClick;
        }
    }

    private void HandleGaugeIncrease(int amount)
    {
        _totalFestaPoint += amount; 
        
        ServiceLocator.Get<IDataManager>().Attendance.Total_Festa_Point = _totalFestaPoint;
        
        ReFreshUI();
    }

    private void HandleChestClick(int chestIndex)
    {
        var gaugeSO = _gaugeModel.GetGaugeSetting(chestIndex + 1);

        if (gaugeSO == null) return;

        int rewardGroupId = gaugeSO.Reward_Accrue_Id;
        var rewardList = _rewardModel.GetRewardGroup(rewardGroupId);

        if (rewardList != null && _rewardPopupView != null)
        {
            _rewardPopupView.OpenRewardPopup(rewardList);
            
            var userGoods = ServiceLocator.Get<IDataManager>().UserGoods;
            
            foreach (var reward in rewardList)
            {
                switch (reward.Reward_Group_Id)
                {
                    case 1: userGoods.Coin_ += reward.Amount; break;
                    case 2: userGoods.Gem_ += reward.Amount; break;
                    case 3: userGoods.Energy_ += reward.Amount; break;
                    case 4: userGoods.Claw_ += reward.Amount; break;
                    case 5: userGoods.FurDoll_ += reward.Amount; break;
                    case 6: userGoods.Stone_ += reward.Amount; break;
                }

                Log.Message($"<color=yellow><b>아이템 ID: {reward.Reward_Id}, 수량: {reward.Amount} 지급 </b></color>");
            }
        }

        _recentGaugeStep = chestIndex + 1;
        ServiceLocator.Get<IDataManager>().Attendance.Recent_Gauge_Step = _recentGaugeStep;
        
        if (_recentGaugeStep >= MAX_FESTA_CHEST_COUNT)
        {
            _finalRewardReceived = true;
            ServiceLocator.Get<IDataManager>().Attendance.Final_Reward_Received = true;
        }
        
        ReFreshUI();
    }

    void SetPoint()
    {
        _totalFestaPoint = ServiceLocator.Get<IDataManager>().Attendance.Total_Festa_Point;
        _recentGaugeStep = ServiceLocator.Get<IDataManager>().Attendance.Recent_Gauge_Step;
        _finalRewardReceived = ServiceLocator.Get<IDataManager>().Attendance.Final_Reward_Received;
    }

    private void ReFreshUI()
    {
        _view.UpdateFestaUI(_totalFestaPoint, _recentGaugeStep); 
    }
}
