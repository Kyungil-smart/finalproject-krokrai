/*
작성자 : 김영빈
작성일 : 26-05-29
수정일 : 26-06-02

역할 : 아이템에 대한 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Item_TableSO", menuName = "Scriptable Objects/Item_TableSO")]
public class Item_TableSO : ScriptableObject
{
    [SOInject(0)] public int Item_Id;
    [SOInject(1)] public string DB_Name;
    [SOInject(2)] public string Item_String_Name_Id;
    [SOInject(3)] public string Item_string_Res_Id;

}