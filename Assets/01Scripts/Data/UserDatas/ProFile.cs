/*
 작성자 : krokrai
 작성일 : 26-05-28

 역할 : FireStore 내부에 ProFile 하위에 있는 데이터를 script에 저장
 방식 : FireStore에서 정보를 불러온 후 ConvertTo<ProFile>()를 해주면, Google 쪽에서 자동으로 scrip에 저장
 */
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
