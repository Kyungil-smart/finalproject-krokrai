/*
작성자 : NekioEmilia
수정자 : NekioEmilia

작성일 : 26-06-08
수정일 : 26-06-25

역할 : 미션 UI와 데이터사이를 제어하는 Presenter 스크립트
방식 : Model(기획 SO 데이터)을 기준으로 DB(유저 진행도) 데이터를 동기화, 이후 탭 전환 및 보상 수령 이벤트를 감지하여 데이터를 가공한 뒤, 각 View(미션 목록, 보상 팝업, 스토리 팝업)에 UI 갱신함.
*/

using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionPresenter : MonoBehaviour
{
    [Header("View 및 Model 연결")] 
    [SerializeField] private MissionView _missionView;
    [SerializeField] private RewardPopupView _rewardPopupView;
    [SerializeField] private MissionDataModel _missionModel;
    [SerializeField] private RewardDataModel _rewardModel;

    [Header("스토리 데이터")] 
    [SerializeField] private List<Story_TableSO> _storyDataList = new();
    [SerializeField] private StoryPopupView _storyPopupView;
    [SerializeField] private GameObject _prologuePopup;

    private List<Mission_ListSO> _currentMissions = new();
    private string _currentDayKey;

    private void Awake()
    {
        var eventManager = ServiceLocator.Get<IEventManager>();

        if (eventManager != null)
        {
            eventManager.OnCurrencyAdded += HandleCurrencyAdded;
            eventManager.OnDayClicked += OnDayTabChangedMission;
        }
    }

    private void Start()
    {
        ServiceLocator.Get<IEventManager>().ClickDay(1);
    }

    private void OnDestroy()
    {
        var eventManager = ServiceLocator.Get<IEventManager>();

        if (eventManager != null)
        {
            eventManager.OnCurrencyAdded -= HandleCurrencyAdded;
            eventManager.OnDayClicked -= OnDayTabChangedMission;
        }
    }

    private void OnEnable()
    {
        if (_missionView != null)
        {
            _missionView.OnSlotRewardRequested += OnRewardClaimed;
        }

        int hasSeenPrologue = PlayerPrefs.GetInt("HasSeenPrologue", 0); // HasSeenPrologue 

        if (hasSeenPrologue == 0)
        {
            _prologuePopup.SetActive(true);
        }
        else
        {
            InitAttendanceUI();
        }
    }

    private void OnDisable()
    {
        if (_missionView != null)
        {
            _missionView.OnSlotRewardRequested -= OnRewardClaimed;
        }
    }

    private void InitAttendanceUI()
    {
        if (!string.IsNullOrEmpty(_currentDayKey))
        {
            if (int.TryParse(_currentDayKey.Replace("Day_", ""), out int currentDay))
            {
                ServiceLocator.Get<IEventManager>().ClickDay(currentDay);
            }
        }
        else
        {
            int activeDay = (int)ServiceLocator.Get<IDataManager>().Attendance.User_Active_Day;
            if (activeDay <= 0) activeDay = 1;
            ServiceLocator.Get<IEventManager>().ClickDay(activeDay);
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

        List<Mission_ListSO> todayMissions = _missionModel.GetMissionsByDay(day);
        Dictionary<string, EventState> getDailyDB = null;
        var dataManager = ServiceLocator.Get<IDataManager>();

        if (dataManager != null && dataManager.UserDatas != null && dataManager.UserDatas.Event_Mission != null)
        {
            if (!dataManager.UserDatas.Event_Mission.Event_490.ContainsKey(_currentDayKey))
            {
                dataManager.UserDatas.Event_Mission.Event_490[_currentDayKey] = new Dictionary<string, EventState>();
            }

            getDailyDB = dataManager.UserDatas.Event_Mission.Event_490[_currentDayKey];
        }

        List<EventState> dbStateList = new();
        List<List<Reward_Group_TableSO>> allRewardGroupList = new List<List<Reward_Group_TableSO>>(); // 5개의 미션 보상 리스트 

        if (_storyPopupView != null)
        {
            string targetStoryId = $"STORY_DAY_{day}";
            Story_TableSO targetStory = _storyDataList.Find(x => x.Story_Id == targetStoryId);

            if (targetStory != null)
            {
                _storyPopupView.ResetMainStoryText(targetStory.ko_Title);
            }
        }

        foreach (var so in todayMissions)
        {
            _currentMissions.Add(so);
            string idStr = so.Mission_Id.ToString();

            EventState currentState;

            if (getDailyDB != null && getDailyDB.ContainsKey(idStr))
            {
                var originalState = getDailyDB[idStr];
                currentState = new EventState
                {
                    Mission_State = originalState.Mission_State,
                    Mission_State_Flag = originalState.Mission_State_Flag
                };
            }
            else
            {
                currentState = new EventState { Mission_State = 0, Mission_State_Flag = 0 };
                getDailyDB[idStr] = currentState;
            }

            if (so.Check_Desc == false)
            {
                currentState.Mission_State = so.Goal_Value;

                if (currentState.Mission_State_Flag != 2)
                {
                    currentState.Mission_State_Flag = 1;
                }
            }

            dbStateList.Add(currentState);

            int rewardGroupId = so.Reward_Daliy_Id;
            var rewardList = _rewardModel.GetRewardGroup(rewardGroupId);
            allRewardGroupList.Add(rewardList);
        }

        _missionView.UpdateAllMissions(_currentMissions, dbStateList, allRewardGroupList);
        CheckAndOpenStoryPopup(getDailyDB, false);
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

        var rewardList = _rewardModel.GetRewardGroup(targetMission.Reward_Daliy_Id);
        
        
        if (rewardList != null && _rewardPopupView != null)
        {
            //  _rewardPopupView.OpenRewardPopup(rewardList);

            var userGoods = ServiceLocator.Get<IDataManager>().UserGoods;
            
            if (userGoods != null)
            {
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
        }

        ServiceLocator.Get<IEventManager>().GaugeIncrease(targetMission.Festa_Point);

        if (dailyDB != null && dailyDB.ContainsKey(idString))
        {
            EventState displayState = new EventState
            {
                Mission_State = dailyDB[idString].Mission_State,
                Mission_State_Flag = dailyDB[idString].Mission_State_Flag
            };

            if (targetMission.Check_Desc == false)
            {
                displayState.Mission_State = targetMission.Goal_Value;
            }

            _missionView.UpdateSingleSlot(slotIndex, targetMission, displayState, rewardList);
        }

        bool isAllCleared = CheckIfAllMissionsCleared(dailyDB);

        if (isAllCleared)
        {
            int currentDayNum = int.Parse(_currentDayKey.Replace("Day_", ""));
            string targetStoryId = $"STORY_DAY_{currentDayNum}";
            Story_TableSO targetStory = _storyDataList.Find(x => x.Story_Id == targetStoryId);

            if (targetStory != null && _storyPopupView != null)
            {
                _storyPopupView.gameObject.SetActive(true);
                
                _storyPopupView.OpenStoryPopup(targetStory.ko_Title, targetStory.ko_Text, () => 
                {
                    if (rewardList != null && _rewardPopupView != null)
                    {
                        _rewardPopupView.OpenRewardPopup(rewardList);
                    }
                });
            }
        }
        else
        {
            if (rewardList != null && _rewardPopupView != null)
            {
                _rewardPopupView.OpenRewardPopup(rewardList);
            }
        }
        
        // CheckAndOpenStoryPopup(dailyDB, true);
    }

    private bool CheckIfAllMissionsCleared(Dictionary<string, EventState> dailyDB)
    {
        if (dailyDB == null) return false;

        foreach (var mission in _currentMissions)
        {
            string id = mission.Mission_Id.ToString();
            if (!dailyDB.ContainsKey(id) || dailyDB[id].Mission_State_Flag != 2)
            {
                return false;
            }
        }
        return true;
    }

    private void CheckAndOpenStoryPopup(Dictionary<string, EventState> dailyDB, bool showPopup)
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
                if (showPopup)
                {
                    Log.Message(
                        $"<color=yellow><b>{currentDayNum}일차 미션 올클리어 '{targetStory.ko_Title}' 스토리 팝업 띄움</b></color>");
                    _storyPopupView.gameObject.SetActive(true);
                    _storyPopupView.OpenStoryPopup(targetStory.ko_Title, targetStory.ko_Text);
                }
                else
                {
                    _storyPopupView.ShowClearedStoryText(targetStory.ko_Title, targetStory.ko_Text);
                }
            }
        }
    }

    private void HandleCurrencyAdded(CurrencyType type, int amount)
    {
        var dataManager = ServiceLocator.Get<IDataManager>();
        if (dataManager == null || dataManager.UserDatas == null) return;

        int currentUnlockedDay = (int)dataManager.Attendance.User_Active_Day;
        var getEventDB = dataManager.UserDatas.Event_Mission.Event_490;

        for (int day = 1; day <= currentUnlockedDay; day++)
        {
            List<Mission_ListSO> dayMissions = _missionModel.GetMissionsByDay(day);

            foreach (var mission in dayMissions)
            {
                if (IsTargetMatch(mission.Target_Id, type))
                {
                    string dayKey = $"Day_{day}";
                    string idStr = mission.Mission_Id.ToString();

                    if (getEventDB.ContainsKey(dayKey) && getEventDB[dayKey].ContainsKey(idStr))
                    {
                        var currentState = getEventDB[dayKey][idStr];

                        if (currentState.Mission_State_Flag == 2 || currentState.Mission_State >= mission.Goal_Value)
                            continue;

                        currentState.Mission_State += amount;


                        if (currentState.Mission_State >= mission.Goal_Value)
                        {
                            currentState.Mission_State = mission.Goal_Value;
                            currentState.Mission_State_Flag = 1;
                        }

                        getEventDB[dayKey][idStr] = currentState;
                    }
                }
            }
        }

        if (gameObject.activeInHierarchy && !string.IsNullOrEmpty(_currentDayKey))
        {
            int currentDay = int.Parse(_currentDayKey.Replace("Day_", ""));
            OnDayTabChangedMission(currentDay);
        }
    }

    public void OnClickPrologueConfirmButton()
    {
        PlayerPrefs.SetInt("HasSeenPrologue", 1);
        PlayerPrefs.Save();
        
        _prologuePopup.SetActive(false);

        InitAttendanceUI();
    }

    private bool IsTargetMatch(int targetId, CurrencyType currencyType)
    {
        switch (currencyType)
        {
            case CurrencyType.Coin: return targetId == 100001;
            case CurrencyType.Gem: return targetId == 100002;
            case CurrencyType.Stone: return targetId == 100003;
            case CurrencyType.FurDoll: return targetId == 100004;
            case CurrencyType.Claw: return targetId == 100005;
            case CurrencyType.Energy: return targetId == 100006;
            default:
            {
                return false;
            }
        }
    }
}