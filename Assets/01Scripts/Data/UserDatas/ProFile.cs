using Firebase.Firestore;
using System;

[Serializable, FirestoreData]
public class ProFile
{
    [FirestoreProperty]
    public string NickName { get; set; }
    [FirestoreProperty]
    public long followerCount { get; set; }
    [FirestoreProperty]
    public long followingCount { get; set; }
    [FirestoreProperty]
    public long profileImage { get; set; }
}
