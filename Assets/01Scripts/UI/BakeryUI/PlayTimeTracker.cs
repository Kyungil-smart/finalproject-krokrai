
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
        var dataManager = ServiceLocator.Get<IDataManager>();
        _lastCheckedTime = dataManager.UserDatas.Attendance.Last_Login_TimeStamp;
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
    /// Last_Login_TimeStamp를 받아 TimeSpan으로 계산해 자정이 지났는지 여부를 체크하는 메서드
    /// </summary>
    private void CheckSimulatorJump()
    {
        var dataManager = ServiceLocator.Get<IDataManager>();
        DateTime currentDbTime = dataManager.UserDatas.Attendance.Last_Login_TimeStamp;
        
        // TimeSpan timeDiff = 현재시간 - 예전시간
        TimeSpan timeDiff = currentDbTime - _lastCheckedTime;

        // TotalMinutes를 쓰면 시간의 총길이를 전부 분으로 환산
        if (timeDiff.TotalMinutes >= 1)
        {
            AddMinuteToDB((int)timeDiff.TotalMinutes);

            // 자정이 지났으면 초기화
            if (currentDbTime.Day != _lastCheckedTime.Day)
            {
                ServiceLocator.Get<IEventManager>().MidnightReset();
            }
            
            _lastCheckedTime = currentDbTime;
        }
    }

    /// <summary>
    /// accTime에 시간을 더하고 PlayTimeUpdated까지 Invoke 시키는 메서드
    /// </summary>
    /// <param name="minutesToAdd"></param>
    private void AddMinuteToDB(int minutesToAdd)
    {
        var dataManager = ServiceLocator.Get<IDataManager>();
        if (dataManager == null || dataManager.UserDatas == null) return;

        var bakeryDB = dataManager.UserDatas.NyangBakery;
        
        // accTime에 딱 minutesToAdd만큼 더해줌 (AddMinutes(더할 분))
        bakeryDB.accTime = bakeryDB.accTime.AddMinutes(minutesToAdd);
        
        int currentMinutes = bakeryDB.accTime.Minute;
        ServiceLocator.Get<IEventManager>().PlayTimeUpdated(currentMinutes);
    }
}
