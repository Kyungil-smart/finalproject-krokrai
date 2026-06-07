
using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionPersenter : MonoBehaviour
{
    [SerializeField] private MissionView view;
    [SerializeField] private RewardPopupView rewardPopupView;
    [SerializeField] private MissionDataModel missionModel;
    [SerializeField] private RewardDataModel rewardModel;
    
    private List<Mission_ListSO> _currentMissions = new();
    private string _currentDayKey;

    private void Awake()
    {
        ServiceLocator.Get<IEventManager>().OnDayClicked += OnDayTabChangedMission;
        view.OnSlotRewardRequested += OnRewardClaimed;
    }

    private void OnDestroy()
    {
        ServiceLocator.Get<IEventManager>().OnDayClicked -= OnDayTabChangedMission;
        view.OnSlotRewardRequested -= OnRewardClaimed;
    }

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
            
            _currentMissions.Add(missionListSO);
            dbStateList.Add(item.Value);

            int rewardGroupId = missionListSO.Reward_Daliy_Id;
            var rewardList = rewardModel.GetRewardGroup(rewardGroupId);
            
            allRewardGroupList.Add(rewardList);
        }
        
        view.UpdateAllMissions(_currentMissions, dbStateList, allRewardGroupList);
    }
    
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
