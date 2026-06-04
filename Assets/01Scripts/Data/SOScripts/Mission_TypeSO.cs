/*
작성자 : 김영빈
작성일 : 26-05-29
수정일 : 26-06-02

역할 : 미션 분류 & 텍스트 고유 키를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Mission_TypeSO", menuName = "Scriptable Objects/Mission_TypeSO")]
public class Mission_TypeSO : ScriptableObject
{
    [SOInject(0)] public int Mission_Type;
    [SOInject(1)] public string Mission_String_Id;
}