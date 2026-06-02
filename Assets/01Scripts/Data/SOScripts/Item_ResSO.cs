/*
작성자 : 김영빈
작성일 : 26-05-29
수정일 : 26-06-02

역할 : 아이템의 리소스를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Item_ResSO", menuName = "Scriptable Objects/Item_ResSO")]
public class Item_ResSO : ScriptableObject
{
    [SOInject(0)] public string Item_String_Res_Id;
    [SOInject(1)] public string Item_res;

}