/*
 작성자 : krokrai
 작성일 : 26-05-29
 수정일 : 26-06-29

 역할 : Service Locator 연결용 interface
 */

using Firebase.Firestore;
using System;

public interface IDataManager
{
    public event Action OnUserDataReseted;
    public void ResetUserData();


    public string UserName { get; }
    public UserDatas UserDatas { get; }
    public ProFile ProFile { get; }
    public Attendance Attendance { get; }
    public Event_Missions Event_Missions { get; }

    public UserGoods UserGoods { get; }

    public DateTime _simulationCurrentTime { get; set; }

    public bool CanSave { get; }

    public void SaveData();

    public void SaveRTDBData();
}
