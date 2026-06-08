/*
작성자 : NekioEmilia
수정자 : 

작성일 : 26-06-08 (생성일 기준)
수정일 :

역할 : 미션 UI와 데이터사이를 제어하는 Presenter 스크립트
방식 : Day 탭을 누르면 DB에서 진행도를 읽어와 View에 뿌리고, 보상 수령 시 DB 갱신 후 페스타 게이지를 올림
*/

using System.Collections.Generic;
using UnityEngine;

public class MissionPresenter : MonoBehaviour
{
    [SerializeField] private MissionView view;
    [SerializeField] private RewardPopupView rewardPopupView;
    [SerializeField] private MissionDataModel missionModel;
    [SerializeField] private RewardDataModel rewardModel;
    
    private List<Mission_ListSO> _currentMissions = new();
    private string _currentDayKey;

    private void Start()
    {
        // Awake -> Start (유니티 이벤트 매니저가 문제인가?)
        ServiceLocator.Get<IEventManager>().OnDayClicked += OnDayTabChangedMission;
        view.OnSlotRewardRequested += OnRewardClaimed;
    }

    private void OnDestroy()
    {
        ServiceLocator.Get<IEventManager>().OnDayClicked -= OnDayTabChangedMission; 
        if (view != null) view.OnSlotRewardRequested -= OnRewardClaimed;
    }

    /// <summary>
    /// EventManager를 통해 N일차 탭이 눌렸을 때 호출되어 해당 일차의 미션 목록을 갱신해주는 메서드
    /// </summary>
    /// <param name="day"></param>
    private void OnDayTabChangedMission(int day)
    {
        _currentDayKey = $"Day_{day}";
        _currentMissions.Clear();

        var getDailyDB = ServiceLocator.Get<IDataManager>().Event_Missions.Event_490[_currentDayKey];
        
        // List<Mission_ListSO> soDataList = new();
        List<EventState> dbStateList = new();
        List<List<Reward_Group_TableSO>> allRewardGroupList = new List<List<Reward_Group_TableSO>>(); // 5개의 미션 보상 리스트 

        foreach (var item in getDailyDB)
        {
            int missionId = int.Parse(item.Key);
            var missionListSO = missionModel.GetMissionListData(missionId);

            if (missionListSO == null) return;
            
            _currentMissions.Add(missionListSO);
            dbStateList.Add(item.Value);

            int rewardGroupId = missionListSO.Reward_Daliy_Id;
            var rewardList = rewardModel.GetRewardGroup(rewardGroupId);
            
            allRewardGroupList.Add(rewardList);
        }
        
        view.UpdateAllMissions(_currentMissions, dbStateList, allRewardGroupList);
    }
    
    /// <summary>
    /// 보상 받기 버튼 눌렀을 때 호출되는 메서드
    /// </summary>
    /// <param name="slotIndex"></param>
    private void OnRewardClaimed(int slotIndex)
    {
        Mission_ListSO targetMission = _currentMissions[slotIndex];
        string idString = targetMission.Mission_Id.ToString();

        ServiceLocator.Get<IDataManager>().Event_Missions.Event_490[_currentDayKey][idString].Mission_State_Flag = 2;

        var rewardList = rewardModel.GetRewardGroup(targetMission.Reward_Daliy_Id);

        if (rewardList != null)
        {
            rewardPopupView.OpenPopup(rewardList);
        }
        
        int gaugeAmount = targetMission.Festa_Point;
        ServiceLocator.Get<IEventManager>().GaugeIncrease(gaugeAmount);
    }
}
