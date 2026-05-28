using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataManager
{
    public UserDatas UserDatas { get; }
    public ProFile ProFile { get; }
    public Attendance Attendance { get; }
    public Event_Missions Event_Missions { get; }
    public void ReadData(string uid);
}
