using System;
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

    private void Start()
    {
        if (btnAddGauge != null)
        {
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
            btnClearMission.onClick.AddListener(() =>
            {
                var dataManager = ServiceLocator.Get<IDataManager>();
                if (dataManager != null && dataManager.UserDatas.Event_Mission?.Event_490 != null)
                {
                    string dayKey = $"Day_{_day}";
                    if (dataManager.UserDatas.Event_Mission.Event_490.ContainsKey(dayKey))
                    {
                        var dailyDB = dataManager.UserDatas.Event_Mission.Event_490[dayKey];
                        var keys = new List<string>(dailyDB.Keys);
                        
                        foreach (var key in keys)
                        {
                            var state = dailyDB[key];
                            state.Mission_State_Flag = 1;
                            dailyDB[key] = state;
                        }
          
                        var eventManager = ServiceLocator.Get<IEventManager>();
                        if (eventManager != null)
                        {
                            eventManager.ClickDay(_day); 
                        }
                    }
                }
            });
        }

        if (btnNextDay != null)
        {
            btnNextDay.onClick.AddListener(() =>
            {
                
                if (dayListView == null) return;

                _day++;
                if (_day > 7) _day = 1;

                // 1. 진짜 DB의 '현재 출석 일수'를 갱신
                var dataManager = ServiceLocator.Get<IDataManager>();
                if (dataManager != null)
                {
                    dataManager.Attendance.User_Active_Day = (ulong)_day;
                }

                // 2. DayListView(출석부 1~7일차) 껍데기 강제 갱신 -> 자물쇠 까맣게 풀리게 만들기!
                for (int i = 0; i < 7; i++)
                {
                    // 인덱스 i가 현재 일차(_day)보다 작으면 자물쇠 해제!
                    bool isUnlocked = (i < _day); 
                    dayListView.DayListUI(i, isUnlocked);
                }

                // 3. 미션 프레젠터한테 "야! 날짜 바꼈으니까 N일차 DB 다시 불러와!" 라고 이벤트 쏘기
                var eventManager = ServiceLocator.Get<IEventManager>();
                if (eventManager != null)
                {
                    eventManager.ClickDay(_day); 
                }
            });
        }
    }
}