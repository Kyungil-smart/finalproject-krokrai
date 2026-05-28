using Firebase.Firestore;
using System;

[Serializable, FirestoreData]
public class Attendance
{
    [FirestoreProperty]
    public bool Final_Reward_Received { get; set; }
    
    [FirestoreProperty]
    public DateTime Join_TimeStamp { get; set; }
    
    [FirestoreProperty]
    public DateTime Last_Login_TimeStamp { get; set; }
    
    [FirestoreProperty]
    public int Recent_Gauge_Step { get; set; }
    
    [FirestoreProperty]
    public int Total_Festa_Point { get; set; }

    [FirestoreProperty]
    public ulong User_Active_Day { get; set; }
}
