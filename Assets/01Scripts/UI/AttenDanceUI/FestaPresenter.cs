/*
작성자 : NekioEmilia
수정자 : NekioEmilia
 
작성일 : 26-06-05
수정일 : 26-06-08

역할 : 출석체크 UI의 Festa Slider와 보물상자에 접근한 Presenter 스크립트 FestaView와 통신
방식 : Presenter를 UI에 적용시키면 View도 같이 따라옴
*/

using System;
using UnityEngine;

[RequireComponent(typeof(FestaView))]
public class FestaPresenter : MonoBehaviour
{
    private int _totalFestaPoint;
    private int _recentGaugeStep;
    private bool _finalRewardReceived;

    [SerializeField] private GaugeDataModel gaugeModel;
    [SerializeField] private FestaView view;
    [SerializeField] private RewardDataModel rewardModel;
    [SerializeField] private RewardPopupView rewardPopupView;
    
    private void Start()
    {
        var eventManager = ServiceLocator.Get<IEventManager>();

        if (eventManager != null)
        {
            eventManager.OnGaugeIncrease += HandleGaugeIncrease;
        }

        if (view != null)
        {
            view.OnChestClicked += HandleChestClick;
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

        if (view != null)
        {
            view.OnChestClicked -= HandleChestClick;
        }
    }

    private void HandleGaugeIncrease(int amount)
    {
        _totalFestaPoint += amount; 
        
        // ServiceLocator.Get<IDataManager>().Attendance.Total_Festa_Point = _totalFestaPoint;
        
        ReFreshUI();
    }

    private void HandleChestClick(int chestIndex)
    {
        var gaugeSO = gaugeModel.GetGaugeSetting(chestIndex + 1); // 

        if (gaugeSO == null) return;

        int rewardGroupId = gaugeSO.Reward_Accrue_Id;

        var rewardList = rewardModel.GetRewardGroup(rewardGroupId);

        if (rewardList != null && rewardPopupView != null)
        {
            rewardPopupView.OpenPopup(rewardList);
        }
    }

    void SetPoint()
    {
        // _totalFestaPoint = ServiceLocator.Get<IDataManager>().Attendance.Total_Festa_Point;
        // _recentGaugeStep = ServiceLocator.Get<IDataManager>().Attendance.Recent_Gauge_Step;
        // _finalRewardReceived = ServiceLocator.Get<IDataManager>().Attendance.Final_Reward_Received;
        
        _totalFestaPoint = 0;
        _recentGaugeStep = 0;
        _finalRewardReceived = false;
    }

    private void ReFreshUI()
    {
        view.UpdateFestaUI(_totalFestaPoint); 
    }
}
