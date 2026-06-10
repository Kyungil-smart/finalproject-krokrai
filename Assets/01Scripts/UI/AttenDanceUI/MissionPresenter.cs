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
    [Header("View 및 Model 연결")]
    [SerializeField] private MissionView missionView;
    [SerializeField] private RewardPopupView rewardPopupView;
    [SerializeField] private MissionDataModel missionModel;
    [SerializeField] private RewardDataModel rewardModel;

    [Header("스토리 데이터")] 
    [SerializeField] private List<Story_TableSO> storyDataList = new();
    [SerializeField] private StoryPopupView storyPopupView;
    
    private List<Mission_ListSO> _currentMissions = new();
    private string _currentDayKey;

    private void Start()
    {
        var eventManager = ServiceLocator.Get<IEventManager>();
        if (eventManager != null)
        {
            eventManager.OnDayClicked += OnDayTabChangedMission;
        }

        if (missionView != null)
        {
            missionView.OnSlotRewardRequested += OnRewardClaimed;
        }
    }

    private void OnDisable()
    {
        var eventManager = ServiceLocator.Get<IEventManager>();
        if (eventManager != null)
        {
            eventManager.OnDayClicked -= OnDayTabChangedMission; // 오류
        }

        if (missionView != null)
        {
            missionView.OnSlotRewardRequested -= OnRewardClaimed;
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

        /*
        if (getDailyDB == null)
        {
            getDailyDB = new Dictionary<string, EventState>();
            int startId = 49000 + (day * 10) + 1;

            for (int i = 0; i < 5; i++)
            {
                getDailyDB.Add((startId + i).ToString(), new EventState { Mission_State = 99, Mission_State_Flag = 1});
            }
        }
        */
        
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
        
        missionView.UpdateAllMissions(_currentMissions, dbStateList, allRewardGroupList);
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
            // 오류
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
        var dataManager =  ServiceLocator.Get<IDataManager>();

        if (dataManager != null && dataManager.UserDatas != null)
        {
            if (dataManager.UserDatas.Event_Mission.Event_490.ContainsKey(_currentDayKey))
            {
                dailyDB = dataManager.UserDatas.Event_Mission.Event_490[_currentDayKey];
                if (dailyDB != null && dailyDB.ContainsKey(idString))
                {
                    dailyDB[idString].Mission_State_Flag = 2;
                    Log.Message($"[데이터 갱신] 미션 {idString} 플래그 2로 변경 성공");
                }
            }
        }
        else
        {
            Log.Message("[주의] 유저 DB에서 Event_490 데이터를 찾을 수 없습니다");
        }

        if (rewardModel == null)
        {
            Log.Message("rewardModel이 null입니다. 매니저 초기 화 필요");
        }
        
        var rewardList = rewardModel.GetRewardGroup(targetMission.Reward_Daliy_Id);

        if (rewardList != null && rewardPopupView != null)
        {
            rewardPopupView.OpenPopup(rewardList);
        }
        else
        {
            Log.Message("[주의] 보상 리스트가 없거나 팝업 뷰가 연결되지 않았습니다");
        }
        ServiceLocator.Get<IEventManager>().GaugeIncrease(targetMission.Festa_Point);

        if (dailyDB != null && dailyDB.ContainsKey(idString))
        {
            missionView.UpdateSingleSlot(slotIndex, targetMission, dailyDB[idString], rewardList);
        }
        else
        {
            // DB가 null일 때 UI 강제 갱신
            // EventState mockState = new EventState { Mission_State = 99, Mission_State_Flag = 2 };
            // missionView.UpdateSingleSlot(slotIndex, targetMission, mockState, rewardList);
        }

        CheckAndOpenStoryPopup(dailyDB);
    }

    private void CheckAndOpenStoryPopup(Dictionary<string, EventState> dailyDB)
    {
        bool isAllCleared = true;

        foreach (var mission in _currentMissions)
        {
            string id = mission.Mission_Id.ToString();

            if (dailyDB != null && dailyDB.ContainsKey(id))
            {
                if (dailyDB[id].Mission_State_Flag != 2)
                {
                    isAllCleared = false;
                    break;
                }
            }
            else
            {
                isAllCleared = false;
                break;
            }
        }

        if (isAllCleared)
        {
            int currentDayNum = int.Parse(_currentDayKey.Replace("Day_", ""));
            string targetStoryId = $"STORY_DAY_{currentDayNum}";
            
            Story_TableSO targetStory = storyDataList.Find(x => x.Story_Id == targetStoryId);

            if (targetStory != null)
            {
                if (storyPopupView != null)
                {
                    storyPopupView.OpenPopup(targetStory.ko_Title, targetStory.ko_Text);
                }
                else
                {
                    Log.Message($"리스트에서 {targetStoryId}를 찾을 수 없습니다");
                }
            }
        }
    }
}
