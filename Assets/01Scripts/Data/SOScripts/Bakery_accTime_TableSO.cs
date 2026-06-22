/*
작성자 : NekioEmilia
수정자 : 
작성일 : 26-06-16
수정일 : 

역할 : 베이커리 accTime의 테이블 값을 가지고 있는 SO 스크립트
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Bakery_accTime_TableSO", menuName = "Scriptable Objects/Bakery_accTime_TableSO")]
public class Bakery_accTime_TableSO : ScriptableObject
{
    [SOInject(0)] public string Reward_ID;
    [SOInject(1)] public int Target_Time;
    [SOInject(2)] public int Reward_Count;
}
