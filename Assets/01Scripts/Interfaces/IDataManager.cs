/*
 작성자 : krokrai
 작성일 : 26-05-29

 역할 : Service Locator 연결용 interface
 */

public interface IDataManager
{
    public UserDatas UserDatas { get; }
    public ProFile ProFile { get; }
    public Attendance Attendance { get; }
    public Event_Missions Event_Missions { get; }

    public UserGoods UserGoods { get; }

    public bool CanSave { get; }

    public void SaveData();

    public void SaveRTDBData();
}
