/*
작성자 : NekioEmilia
수정자 : NekioEmilia

작성일 : 26-06-16
수정일 : 26-06-19

역할 : 냥냥 베이커리 이벤트의 누적 플레이 타임을 전역적으로 관리, 추적하는 스크립트
방식 : 싱글턴 패턴과 DontDestroyOnLoad를 이용해 냥냥베이커리 화면을 끄고 있어도 자동적으로 시간 업데이트
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

        if (dataManager != null && dataManager.UserDatas != null)
        {
            var bakeryDB = dataManager.UserDatas.NyangBakery;
            DateTime lastLoginTime = dataManager.Attendance.Last_Login_TimeStamp;

            bool isFirstTimeEver = (lastLoginTime == default(DateTime));

            if (isFirstTimeEver)
            {
                if (bakeryDB != null && bakeryDB.accTime == default(DateTime))
                {
                    bakeryDB.accTime = DateTime.Today;
                }
            }
        }
        
        _lastCheckedTime = DateTime.Now;
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

            // 자정이 지났을 때
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