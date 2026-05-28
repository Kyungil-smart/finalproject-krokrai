using Firebase.Firestore;
using System;
using System.Collections.Generic;

[Serializable, FirestoreData]
public class Event_Missions
{
    [FirestoreProperty]
    public Dictionary<string, Dictionary<string, Dictionary<string, EventState>>> Event_490 { get; set; } = new();
}
