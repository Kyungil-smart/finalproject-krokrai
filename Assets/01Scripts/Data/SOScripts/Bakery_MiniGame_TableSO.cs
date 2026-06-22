/*
작성자 : NekioEmilia
수정자 : 
작성일 : 26-06-16
수정일 : 

역할 : 베이커리 MiniGame 테이블 값을 가지고 있는 SO 스크립트
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Bakery_MiniGame_TableSO", menuName = "Scriptable Objects/Bakery_MiniGame_TableSO")]
public class Bakery_MiniGame_TableSO : ScriptableObject
{
    [SOInject(0)] public int Round_Id;
    [SOInject(1)] public int success_Count;
    [SOInject(2)] public int all_Cookie_Count;
    [SOInject(3)] public bool isRoundend;
    [SOInject(4)] public int success_reward_amount;
    [SOInject(5)] public int fail_reward_amount;
}
