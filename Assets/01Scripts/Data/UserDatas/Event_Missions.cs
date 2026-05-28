/*
 작성자 : krokrai
 작성일 : 26-05-28

 역할 : FireStore 내부에 Event_Missions 하위에 있는 데이터를 script에 저장
 방식 : FireStore에서 정보를 불러온 후 ConvertTo<Event_Missions>()를 해주면, Google 쪽에서 자동으로 scrip에 저장
 */
using Firebase.Firestore;
using System;
using System.Collections.Generic;

[Serializable, FirestoreData]
public class Event_Missions
{
    [FirestoreProperty]
    public Dictionary<string, Dictionary<string, EventState>> Event_490 { get; set; } = new();
    // 추가 이벤트 시 여기에 위에 줄과 같이 작성 단, firestore의 이름과 변수 명이 정확히 같아야함.
}
