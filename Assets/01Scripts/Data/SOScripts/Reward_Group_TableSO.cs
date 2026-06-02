/*
작성자 : 김영빈
작성일 : 26-05-29
수정일 : 26-06-02

역할 : 미션 보상에 대한 것을 담고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Reward_Group_TableSO", menuName = "Scriptable Objects/Reward_Group_TableSO")]
public class Reward_Group_TableSO : ScriptableObject
{
    [SOInject(0)] public int Index;
    [SOInject(1)] public int Reward_Group_Id;
    [SOInject(2)] public int Reward_Order;
    [SOInject(3)] public string Reward_Name;
    [SOInject(4)] public int Reward_Id;
    [SOInject(5)] public int Amount;
}
