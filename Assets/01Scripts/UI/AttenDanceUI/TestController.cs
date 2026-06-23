/*
작성자 : NekioEmilia
수정자 : NekioEmilia
 
작성일 : 26-06-15
수정일 : 

역할 : 테스트를 위한 테스트 스크립트
방식 : AddGauge 버튼 => 게이지 점수 + 100, ClearMission 버튼 => 일차 별 미션 하나씩 완료, NextDay 버튼 => 클릭 시 다음 일차로 넘어감
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestController : MonoBehaviour
{
    [Header("UI View 연결")]
    [SerializeField] private FestaView festaView;
    [SerializeField] private MissionSlotView[] missionSlots;
    [SerializeField] private DayListView dayListView;

    [Header("버튼 연결")]
    [SerializeField] private Button btnAddGauge;
    [SerializeField] private Button btnClearMission;
    [SerializeField] private Button btnNextDay;
    
    private int _gauge = 0;
    private int _day = 1;

    private void OnEnable()
    {
        var eventManager = ServiceLocator.Get<IEventManager>();
        if (eventManager != null)
        {
            eventManager.OnDayClicked += SyncDay;
        }
    }

    private void OnDisable()
    {
        var eventManager = ServiceLocator.Get<IEventManager>();
        if (eventManager != null)
        {
            eventManager.OnDayClicked -= SyncDay;
        }
    }
    
    private void SyncDay(int day)
    {
        _day = day;
        Debug.Log($"<color=white>[TestController] 타겟 일차가 {_day}일 차로 동기화됨!</color>");
    }
    
    private void Start()
    {
        if (btnAddGauge != null)
        {
            btnAddGauge.onClick.RemoveAllListeners();
            btnAddGauge.onClick.AddListener(() =>
            {
                var eventManager = ServiceLocator.Get<IEventManager>();
                if (eventManager != null)
                {
                    eventManager.GaugeIncrease(100); 
                }
            });
        }

        if (btnClearMission != null)
        {
            btnClearMission.onClick.RemoveAllListeners();
            btnClearMission.onClick.AddListener(() =>
            {
                var dataManager = ServiceLocator.Get<IDataManager>();
                
                if (dataManager == null || dataManager.UserDatas?.Event_Mission == null)
                {
                    Debug.LogError("[Test] Event_Mission DB가 아예 없습니다! (게임 첫 시작 로직 안 탐)");
                    return;
                }

                if (dataManager.UserDatas.Event_Mission.Event_490 == null)
                {
                    dataManager.UserDatas.Event_Mission.Event_490 = new Dictionary<string, Dictionary<string, EventState>>();
                }

                string dayKey = $"Day_{_day}";
                
                if (!dataManager.UserDatas.Event_Mission.Event_490.ContainsKey(dayKey))
                {
                    Debug.LogWarning($"<color=orange>[Test] {dayKey} DB가 아직 없습니다! (프레젠터가 화면을 한 번 그려야 생성됨)</color>");
                    return;
                }

                var dailyDB = dataManager.UserDatas.Event_Mission.Event_490[dayKey];
                var keys = new List<string>(dailyDB.Keys);
                
                foreach (var key in keys)
                {
                    var state = dailyDB[key];

                    if (state.Mission_State_Flag == 0)
                    {
                        state.Mission_State = 999;
                        state.Mission_State_Flag = 1;
                        dailyDB[key] = state;
                        
                        break;
                    }
                }
                
                var eventManager = ServiceLocator.Get<IEventManager>();
                if (eventManager != null)
                {
                    eventManager.ClickDay(_day); // 화면 다시 그리기
                }
            });
        }

        if (btnNextDay != null)
        {
            btnNextDay.onClick.RemoveAllListeners();
            btnNextDay.onClick.AddListener(() =>
            {
                if (dayListView == null) return;

                _day++;
                if (_day > 7) _day = 1;

                var dataManager = ServiceLocator.Get<IDataManager>();
                if (dataManager != null && dataManager.Attendance != null)
                {
                    dataManager.Attendance.User_Active_Day = (ulong)_day;
                }

                for (int i = 0; i < 7; i++)
                {
                    bool isUnlocked = (i < _day); 
                    dayListView.DayListUI(i, isUnlocked);
                }
                
                var eventManager = ServiceLocator.Get<IEventManager>();
                if (eventManager != null)
                {
                    eventManager.ClickDay(_day); 
                }
            });
        }
    }
}