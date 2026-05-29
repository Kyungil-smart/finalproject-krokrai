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

    public void Init()
    {
        Event_490 = new()
        {
            ["Day_1"] = new()
            {
                ["49011"] = new(),
                ["49012"] = new(),
                ["49013"] = new(),
                ["49014"] = new(),
                ["49015"] = new()
            },
            ["Day_2"] = new()
            {
                ["49021"] = new(),
                ["49022"] = new(),
                ["49023"] = new(),
                ["49024"] = new(),
                ["49025"] = new()
            },
            ["Day_3"] = new()
            {
                ["49031"] = new(),
                ["49032"] = new(),
                ["49033"] = new(),
                ["49034"] = new(),
                ["49035"] = new()
            },
            ["Day_4"] = new()
            {
                ["49041"] = new(),
                ["49042"] = new(),
                ["49043"] = new(),
                ["49044"] = new(),
                ["49045"] = new()
            },
            ["Day_5"] = new()
            {
                ["49051"] = new(),
                ["49052"] = new(),
                ["49053"] = new(),
                ["49054"] = new(),
                ["49055"] = new()
            },
            ["Day_6"] = new()
            {
                ["49061"] = new(),
                ["49062"] = new(),
                ["49063"] = new(),
                ["49064"] = new(),
                ["49065"] = new()
            },
            ["Day_7"] = new()
            {
                ["49071"] = new(),
                ["49072"] = new(),
                ["49073"] = new(),
                ["49074"] = new(),
                ["49075"] = new()
            }
        };
    }
}
