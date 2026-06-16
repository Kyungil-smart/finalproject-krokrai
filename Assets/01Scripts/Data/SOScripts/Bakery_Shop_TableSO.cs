/*
작성자 : NekioEmilia
수정자 : 
작성일 : 26-06-16
수정일 : 

역할 : 베이커리 Shop 테이블 값을 가지고 있는 SO 스크립트
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Bakery_Shop_TableSO", menuName = "Scriptable Objects/Bakery_Shop_TableSO")]
public class Bakery_Shop_TableSO : ScriptableObject
{
    [SOInject(0)] public string Shop_Index;
    [SOInject(1)] public Bakery_Shop_TableEnum Shop_Type;
    [SOInject(2)] public int Shop_number;
    [SOInject(3)] public int Item_Id;
    [SOInject(4)] public int Shop_buyCost;
    [SOInject(5)] public int Shop_buyLimit;
}
