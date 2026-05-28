using Firebase.Firestore;
using System;

[Serializable, FirestoreData]
public class EventState
{
    [FirestoreProperty]
    public int Mission_State { get; set; }
    [FirestoreProperty]
    public int Mission_State_Flag { get; set; }
}
