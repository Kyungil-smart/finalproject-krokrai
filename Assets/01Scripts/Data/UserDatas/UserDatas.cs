using Firebase.Firestore;
using System;

[Serializable, FirestoreData]
public class UserDatas
{
    [FirestoreProperty]
    public Attendance Attendance { get; set; } = new();

    [FirestoreProperty]
    public Event_Missions Event_Mission { get; set; } = new();

    [FirestoreProperty]
    public ProFile Profile { get; set; } = new();
}
