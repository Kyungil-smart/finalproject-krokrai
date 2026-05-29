/*
 작성자 : krokrai
 작성일 : 26-05-28

 역할 : FireStore 내부에 Attendance 하위에 있는 데이터를 script에 저장
 방식 : FireStore에서 정보를 불러온 후 ConvertTo<Attendance>()를 해주면, Google 쪽에서 자동으로 scrip에 저장
 */
using Firebase.Firestore;
using System;

[Serializable, FirestoreData]
public class Attendance
{
    [FirestoreProperty]
    public bool Final_Reward_Received { get; set; } = new();
    
    [FirestoreProperty]
    public DateTime Join_TimeStamp { get; set; } = DateTime.Now;

    [FirestoreProperty]
    public DateTime Last_Login_TimeStamp { get; set; } = new();

    [FirestoreProperty]
    public int Recent_Gauge_Step { get; set; } = new();

    [FirestoreProperty]
    public int Total_Festa_Point { get; set; } = new();

    [FirestoreProperty]
    public ulong User_Active_Day { get; set; } = new();
}
