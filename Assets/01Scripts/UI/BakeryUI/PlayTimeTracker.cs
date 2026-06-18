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
    private float _realTimer = 0f;
    private float _dbCheckTimer = 0f;
    private DateTime _lastCheckedTime;
    
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        _lastCheckedTime = DateTime.Now;
        
        var dataManager = ServiceLocator.Get<IDataManager>();

        if (dataManager != null && dataManager.UserDatas != null)
        {
            var bakeryDB = dataManager.UserDatas.NyangBakery;
            if (bakeryDB != null)
            {
                // DateTime.Today => 00H:00M:00S로 초기화
                bakeryDB.accTime = DateTime.Today;
            }
        }
    }

    private void Update()
    {
        // unscaledDeltaTime = 게임을 일시 정지(Time.timeScale = 0)하거나 배속을 적용해도 영향을 받지 않음
        _realTimer += Time.unscaledDeltaTime;
        _dbCheckTimer += Time.unscaledDeltaTime;
            
        // unscaledDeltaTime을 받아 1분마다 업데이트
        if (_realTimer >= 60f)
        {
            _realTimer = 0f;
            AddMinuteToDB(1);
        }
        
        // 최적화를 위해 1초마다 검사
        if (_dbCheckTimer >= 1f)
        {
            _dbCheckTimer = 0f;
            CheckSimulatorJump();
        }
    }
    
    /// <summary>
    /// 실제 시간(DateTime.Now)와 비교하여 시간이 크게 튀었거나 1분 이상 경과했는지 체크하는 메서드
    /// </summary>
    private void CheckSimulatorJump()
    {
        DateTime currentRealTime = DateTime.Now; 
        // TimeSpan = 현재 컴퓨터 시간 - 마지막으로 검사했던 컴퓨터 시간
        TimeSpan timeDiff = currentRealTime - _lastCheckedTime;

        // 실제 현실 시간이 1분 이상 흘렀다면
        if (timeDiff.TotalMinutes >= 1)
        {
            int minutesToAdd = (int)timeDiff.TotalMinutes;
            AddMinuteToDB(minutesToAdd);
            
            if (currentRealTime.Day != _lastCheckedTime.Day)
            {
                var eventManager = ServiceLocator.Get<IEventManager>();
                if (eventManager != null)
                {
                    eventManager.MidnightReset();
                }
            }
            
            // 자정이 지나면 0으로 초기화
            var bakeryDB = ServiceLocator.Get<IDataManager>().UserDatas.NyangBakery;
            if (bakeryDB != null) bakeryDB.accTime = DateTime.Today;
            
            _lastCheckedTime = currentRealTime; 
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

        var eventManager = ServiceLocator.Get<IEventManager>();
        if (eventManager != null)
        {
            eventManager.PlayTimeUpdated(totalAccumulatedMinutes);
        }
    }
}
