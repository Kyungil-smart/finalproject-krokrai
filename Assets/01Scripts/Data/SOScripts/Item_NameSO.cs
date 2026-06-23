/*
작성자 : 김영빈
작성일 : 26-05-29
수정일 : 26-06-02

역할 : 아이템 이름 스트링 id와 로컬라이제이션을 위한 string이 담겨져 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Item_NameSO", menuName = "Scriptable Objects/Item_NameSO")]
public class Item_NameSO : ScriptableObject
{
    [SOInject(0)] public string Item_String_Name_Id;
    [SOInject(1)] public string ko;
    [SOInject(2)] public string en;
    [SOInject(3)] public string jp;
    [SOInject(4)] public string ch;
}