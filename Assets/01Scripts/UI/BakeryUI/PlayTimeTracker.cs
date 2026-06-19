/*
작성자 : NekioEmilia
수정자 :
작성일 : 26-06-16
수정일 :

역할 : 베이커리 accTime의 테이블 값을 가지고 있는 SO 스크립트
*/


using System;
using UnityEngine;

public class PlayTimeTracker : MonoBehaviour
{
    public static PlayTimeTracker Instance { get; private set; }
    
    private DateTime _lastCheckedTime;
    private float _realTimer = 0f;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        var dataManager = ServiceLocator.Get<IDataManager>();

        if (dataManager != null)
        {
            _lastCheckedTime = dataManager.Attendance.Last_Login_TimeStamp;

            if (dataManager.UserDatas != null)
            {
                var bakerDB = dataManager.UserDatas.NyangBakery;
                if (bakerDB != null)
                {
                    // 게임 켜질 때 00:00으로 초기화
                    bakerDB.accTime = DateTime.Today;
                }
            }
        }
        else
        {
            _lastCheckedTime = DateTime.Now;
        }
    }

    private void Update()
    {
        _realTimer += Time.unscaledDeltaTime;

        if (_realTimer >= 60f)
        {
            _realTimer -= 60f;
            AddMinuteToDB(1);
            
            _lastCheckedTime = _lastCheckedTime.AddMinutes(1);
        }
    }

    /// <summary>
    /// 시뮬레이터가 시간을 움직였을 때 호출해주는 메서드
    /// </summary>
    public void OnTimeAdvanced(DateTime advancedTime)
    {
        TimeSpan timeDiff = advancedTime - _lastCheckedTime;

        if (timeDiff.TotalMinutes >= 1)
        {
            int minutesToAdd = (int)timeDiff.TotalMinutes;
            AddMinuteToDB(minutesToAdd);

            // 저장이 지났을 때
            if (advancedTime.Day != _lastCheckedTime.Day)
            {
                var eventManager = ServiceLocator.Get<IEventManager>();

                if (eventManager != null)
                {
                    eventManager.MidnightReset();
                }
                
                var bakeryDB = ServiceLocator.Get<IDataManager>().UserDatas.NyangBakery;

                if (bakeryDB != null)
                {
                    bakeryDB.accTime = DateTime.Today;
                }
            }
            
            _lastCheckedTime = advancedTime;
        }
    }

    private void AddMinuteToDB(int minutesToAdd)
    {
        var dataManager = ServiceLocator.Get<IDataManager>();
        if (dataManager == null || dataManager.UserDatas == null) return;

        var bakeryDB = dataManager.UserDatas.NyangBakery;
        if (bakeryDB == null) return;

        bakeryDB.accTime = bakeryDB.accTime.AddMinutes(minutesToAdd);

        // 시간 보정 (Hour * 60) + Minute
        int totalAccumulatedMinutes = (bakeryDB.accTime.Hour * 60) + bakeryDB.accTime.Minute;

        Debug.Log($"<color=cyan><b>DB에 {minutesToAdd}분 추가됨 총 누적 시간: {totalAccumulatedMinutes}분</b></color>");

        var eventManager = ServiceLocator.Get<IEventManager>();
        if (eventManager != null)
        {
            eventManager.PlayTimeUpdated(totalAccumulatedMinutes);
        }
    }
}