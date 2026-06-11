/*
작성자 : NekioEmilia
수정자 :

작성일 : 26-06-08
수정일 :

역할 : 미션 UI와 데이터사이를 제어하는 Presenter 스크립트
*/

using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionPresenter : MonoBehaviour
{
    [Header("View 및 Model 연결")] [SerializeField]
    private MissionView _missionView;

    [SerializeField] private RewardPopupView _rewardPopupView;
    [SerializeField] private MissionDataModel _missionModel;
    [SerializeField] private RewardDataModel _rewardModel;

    [Header("스토리 데이터")] [SerializeField] private List<Story_TableSO> _storyDataList = new();
    [SerializeField] private StoryPopupView _storyPopupView;

    private List<Mission_ListSO> _currentMissions = new();
    private string _currentDayKey;

    private void OnEnable() // Start -> OnEnable
    {
        var eventManager = ServiceLocator.Get<IEventManager>();
        if (eventManager != null)
        {
            eventManager.OnDayClicked += OnDayTabChangedMission;
        }

        if (_missionView != null)
        {
            _missionView.OnSlotRewardRequested += OnRewardClaimed;
        }
    }

    private void OnDisable()
    {
        var eventManager = ServiceLocator.Get<IEventManager>();
        if (eventManager != null)
        {
            eventManager.OnDayClicked -= OnDayTabChangedMission; // 오류
        }

        if (_missionView != null)
        {
            _missionView.OnSlotRewardRequested -= OnRewardClaimed;
        }
    }

    /// <summary>
    /// EventManager를 통해 N일차 탭이 눌렸을 때 호출되어 해당 일차의 미션 목록을 갱신해주는 메서드
    /// </summary>
    /// <param name="day"></param>
    private void OnDayTabChangedMission(int day)
    {
        _currentDayKey = $"Day_{day}";
        _currentMissions.Clear();

        Dictionary<string, EventState> getDailyDB = null;

        var dataManager = ServiceLocator.Get<IDataManager>();

        if (dataManager != null && dataManager.UserDatas != null && dataManager.UserDatas.Event_Mission != null)
        {
            if (dataManager.UserDatas.Event_Mission.Event_490.ContainsKey(_currentDayKey))
            {
                getDailyDB = dataManager.UserDatas.Event_Mission.Event_490[_currentDayKey];
            }
        }

        if (getDailyDB == null || getDailyDB.Count == 0)
        {
            _missionView.UpdateAllMissions(new List<Mission_ListSO>(), new List<EventState>(),
                new List<List<Reward_Group_TableSO>>());
            return;
        }

        List<EventState> dbStateList = new();
        List<List<Reward_Group_TableSO>> allRewardGroupList = new List<List<Reward_Group_TableSO>>(); // 5개의 미션 보상 리스트 

        foreach (var item in getDailyDB)
        {
            int missionId = int.Parse(item.Key);
            var missionListSO = _missionModel.GetMissionListData(missionId);

            if (missionListSO == null)
            {
                continue;
            }

            _currentMissions.Add(missionListSO);
            dbStateList.Add(item.Value);

            int rewardGroupId = missionListSO.Reward_Daliy_Id;
            var rewardList = _rewardModel.GetRewardGroup(rewardGroupId);
            allRewardGroupList.Add(rewardList);
        }
        _missionView.UpdateAllMissions(_currentMissions, dbStateList, allRewardGroupList);
    }

    /// <summary>
    /// 보상 받기 버튼 눌렀을 때 호출되는 메서드
    /// </summary>
    /// <param name="slotIndex"></param>
    private void OnRewardClaimed(int slotIndex)
    {
        if (_currentMissions == null || slotIndex < 0 || slotIndex >= _currentMissions.Count)
        {
            Log.Message($"[데이터 불일치] 빈 슬롯을 클릭했습니다 (요청 인덱스: {slotIndex}, 현재 리스트 개수: {_currentMissions?.Count}");
            return;
        }

        Mission_ListSO targetMission = _currentMissions[slotIndex];

        if (targetMission == null)
        {
            Log.Message($"null 에러 {_currentMissions.Count}개 리스트 중 {slotIndex}번째 슬롯에 SO 데이터가 없습니다.");
            return;
        }

        string idString = targetMission.Mission_Id.ToString();
        Dictionary<string, EventState> dailyDB = null;
        var dataManager = ServiceLocator.Get<IDataManager>();

        if (dataManager != null && dataManager.UserDatas != null)
        {
            if (dataManager.UserDatas.Event_Mission.Event_490.ContainsKey(_currentDayKey))
            {
                dailyDB = dataManager.UserDatas.Event_Mission.Event_490[_currentDayKey];
                if (dailyDB != null && dailyDB.ContainsKey(idString))
                {
                    var currentState = dailyDB[idString];
                    currentState.Mission_State_Flag = 2;
                    dailyDB[idString] = currentState;
                }
            }
        }

        if (_rewardModel == null)
        {
            Log.Message("rewardModel이 null입니다. 매니저 초기화 필요");
        }

        var rewardList = _rewardModel.GetRewardGroup(targetMission.Reward_Daliy_Id);

        if (rewardList != null && _rewardPopupView != null)
        {
            _rewardPopupView.OpenRewardPopup(rewardList);
        }
        
        ServiceLocator.Get<IEventManager>().GaugeIncrease(targetMission.Festa_Point);
        

        if (dailyDB != null && dailyDB.ContainsKey(idString))
        {
            _missionView.UpdateSingleSlot(slotIndex, targetMission, dailyDB[idString], rewardList);
        }

        // 5개 미션 다 채우면 스토리 팝업
        CheckAndOpenStoryPopup(dailyDB);
    }

    private void CheckAndOpenStoryPopup(Dictionary<string, EventState> dailyDB)
    {
        if (dailyDB == null)
        {
            Log.Message("dailyDB가 null입니다");
            return;
        }

        bool isAllCleared = true;

        foreach (var mission in _currentMissions)
        {
            string id = mission.Mission_Id.ToString();

            if (dailyDB.ContainsKey(id))
            {
                int flag = (int)dailyDB[id].Mission_State_Flag;

                if (flag != 2)
                {
                    isAllCleared = false;
                    
                }
            }
            else
            {
                isAllCleared = false;
            }
        }

        if (isAllCleared)
        {
            int currentDayNum = int.Parse(_currentDayKey.Replace("Day_", ""));
            string targetStoryId = $"STORY_DAY_{currentDayNum}";
            Story_TableSO targetStory = _storyDataList.Find(x => x.Story_Id == targetStoryId);

            if (targetStory != null)
            {
                if (_storyPopupView != null)
                {
                    Log.Message($"<color=yellow><b>{currentDayNum}일차 미션 올클리어 '{targetStory.ko_Title}' 스토리 팝업 띄움</b></color>");
                    _storyPopupView.gameObject.SetActive(true);
                    _storyPopupView.OpenStoryPopup(targetStory.ko_Title, targetStory.ko_Text);
                }
            }
        }
    }
}