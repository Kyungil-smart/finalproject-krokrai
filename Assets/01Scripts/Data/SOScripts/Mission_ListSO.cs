/*
작성자 : NekioEmilia
수정자 : NekioEmilia
작성일 : 26-05-29
수정일 : 26-06-02

역할 : 미션의 대한 정보를 담고 있는 SO
*/
using UnityEngine;


[CreateAssetMenu(fileName = "Mission_ListSO", menuName = "Scriptable Objects/Mission_ListSO")]
public class Mission_ListSO : ScriptableObject
{
    [SOInject(0)] public int Mission_Id;
    [SOInject(1)] public int Day_Index;
    [SOInject(2)] public int Mission_Order;
    [SOInject(3)] public string Mission_Desc;
    [SOInject(4)] public bool Check_Desc;
    [SOInject(5)] public int Mission_Type;
    [SOInject(6)] public int Target_Id;
    [SOInject(7)] public int Goal_Value;
    [SOInject(8)] public int Festa_Point;
    [SOInject(9)] public int Reward_Daliy_Id;
    [SOInject(10)] public string Mission_String_Id;
}
