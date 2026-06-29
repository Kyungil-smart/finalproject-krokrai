/*
작성자 : NekioEmilia
수정자 : NekioEmilia

작성일 : 26-06-04
수정일 : 26-06-29

역할 : 출석체크 UI의 1~7일차 UI에 MVP 패턴을 적용해 db에서 값을 받아와서 DayListView와 통신
방식 : AttendanceUICanvas 하위에 Day_List에 DayListView와 함께 부착
*/

using System;
using UnityEngine;

[RequireComponent(typeof(DayListView))]
public class DayListPresent : MonoBehaviour
{
    [SerializeField] private DayListView _view;

    private void OnEnable()
    {
        RefreshUI();
    }

    /// <summary>
    /// 누적 출석 일수에 맞춰 1~7일차 UI 슬롯의 잠금을 해제    
    /// </summary>
    private void RefreshUI()
    {
        var attendance = ServiceLocator.Get<IDataManager>().Attendance;

        DateTime joinTime = attendance.Join_TimeStamp;
        DateTime lastLoginTime = attendance.Last_Login_TimeStamp;
        
        int calculatedDay = (lastLoginTime.Date - joinTime.Date).Days + 1;
        
        if (calculatedDay <= 0)
        {
            calculatedDay = 1;
        }

        if (attendance.User_Active_Day > (ulong)calculatedDay)
        {
            calculatedDay = (int)attendance.User_Active_Day;
        }
        else
        {
            attendance.User_Active_Day = (ulong)calculatedDay;
        }
        
        int displayActiveDay = Math.Min(calculatedDay, 7);


        Log.Message($"<color=cyan>가입: {joinTime.Date:MM/dd} / 최근접속: {lastLoginTime.Date:MM/dd} -> 현재 {calculatedDay}일차</color>");
        
        for (int i = 0; i < 7; i++)
        {
            bool isUnlocked = (i < displayActiveDay);
            _view.DayListUI(i, isUnlocked);
        }
        
        _view.InitOutline(calculatedDay - 1);
    }
}