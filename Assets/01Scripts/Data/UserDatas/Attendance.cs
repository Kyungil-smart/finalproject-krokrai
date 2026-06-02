/*
 작성자 : krokrai
 작성일 : 26-05-28
 수정일 : 26-05-29

 역할 : FireStore 내부에 Attendance 하위에 있는 데이터를 script에 저장
 방식 : FireStore에서 정보를 불러온 후 ConvertTo<Attendance>()를 해주면, Google 쪽에서 자동으로 scrip에 저장
 */
using Firebase.Firestore;
using System;

[Serializable, FirestoreData]
public class Attendance
{
    private bool _final_Reward_Received = false;
    private DateTime _join_TimeStamp = DateTime.Now;
    private DateTime _last_Login_TimeStamp = DateTime.Now;
    private int _recent_Gauge_Step = 0;
    private int _total_Festa_Point = 0;
    private ulong _user_Active_Day = 0;

    [FirestoreProperty]
    public bool Final_Reward_Received
    {
        get
        {
            return _final_Reward_Received;
        }
        set
        {
            _final_Reward_Received = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }

    [FirestoreProperty]
    public DateTime Join_TimeStamp
    {
        get
        {
            return _join_TimeStamp;
        }
        set
        {
            _join_TimeStamp = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }

    [FirestoreProperty]
    public DateTime Last_Login_TimeStamp
    {
        get
        {
            return _last_Login_TimeStamp;
        }
        set
        {
            _last_Login_TimeStamp = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }

    [FirestoreProperty]
    public int Recent_Gauge_Step
    {
        get
        {
            return _recent_Gauge_Step;
        }
        set
        {
            _recent_Gauge_Step = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }

    [FirestoreProperty]
    public int Total_Festa_Point
    {
        get
        {
            return _total_Festa_Point;
        }
        set
        {
            _total_Festa_Point = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }

    [FirestoreProperty]
    public ulong User_Active_Day
    {
        get
        {
            return _user_Active_Day;
        }
        set
        {
            _user_Active_Day = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }
}
