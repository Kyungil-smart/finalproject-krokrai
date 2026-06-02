/*
작성자 : NekioEmilia
수정자 : NekioEmilia
작성일 : 26-05-29
수정일 : 26-06-02

역할 : 미션의 누적 세팅을 담고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Mission_Gauge_SettingSO", menuName = "Scriptable Objects/Mission_Gauge_SettingSO")]
public class Mission_Gauge_SettingSO : ScriptableObject
{
    [SOInject(0)] public int Gauge_Reward_Id;
    [SOInject(1)] public int Gauge_Step;
    [SOInject(2)] public int Goal_Point;
    [SOInject(3)] public int Reward_Accrue_Id;
}