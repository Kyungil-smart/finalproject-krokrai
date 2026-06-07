/*
작성자 : NekioEmilia
수정자 : 
 
작성일 : 26-06-04
수정일 : 

역할 : 출석체크 UI의 1~7일차 UI에 MVP 패턴을 적용해 db에서 값을 받아와서 DayListView와 통신
방식 : AttendanceUICanvas 하위에 Day_List에 DayListView와 함께 부착
*/


using System;
using UnityEngine;

[RequireComponent(typeof(DayListView))]
public class DayListPresent : MonoBehaviour
{
    [SerializeField] private DayListView _view;

    private DateTime _joinTimeStamp;
    private DateTime _lastLoginTime;
    private DateTime _currentTime;

    private void OnEnable()
    {
        _currentTime = DateTime.Now;
        SetTimestamp();
        SetUserActiveDay();
        RefreshUI();
    }

    /*
    private void OnDisable()
    {
        
    }
    */

    void SetTimestamp()
    {
        _joinTimeStamp = ServiceLocator.Get<IDataManager>().Attendance.Join_TimeStamp;
        // _joinTimeStamp = new DateTime(2026, 06, 01, 0,0,0);
        _lastLoginTime = ServiceLocator.Get<IDataManager>().Attendance.Last_Login_TimeStamp; 
        // _lastLoginTime = new DateTime(2026, 06, 03, 0,0,0);;
    }

    
    void SetUserActiveDay()
    {
        if (_lastLoginTime.Date != _currentTime.Date)
        {
            // 연-월-일만 비교해서 다르면 +1
            ServiceLocator.Get<IDataManager>().Attendance.User_Active_Day += 1;
            
            ServiceLocator.Get<IDataManager>().Attendance.Last_Login_TimeStamp = _currentTime.Date; // Last_Login 업데이트
        }
    }

    private void RefreshUI()
    {
        int activeDay = (int)Math.Min(ServiceLocator.Get<IDataManager>().Attendance.User_Active_Day, 7); 
        
        for (int i = 0; i < 7; i++)
        {
            bool isUnlocked = (i < activeDay);
            
            _view.DayListUI(i, isUnlocked);
        }
    }
}
