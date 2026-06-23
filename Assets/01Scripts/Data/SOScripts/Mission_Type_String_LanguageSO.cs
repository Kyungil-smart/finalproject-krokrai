/*
작성자 : 김영빈
작성일 : 26-05-29
수정일 : 26-06-02

역할 : 미선 분류, 텍스트 고유 키 + 로컬라이제이션을 위한 string이 담겨 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Mission_Type_String_LanguageSO", menuName = "Scriptable Objects/Mission_Type_String_LanguageSO")]
public class Mission_Type_String_LanguageSO : ScriptableObject
{
    [SOInject(0)] public string Mission_String_Id;
    [SOInject(1)] public int Mission_Type;
    [SOInject(2)] public string ko;
    [SOInject(3)] public string en;
    [SOInject(4)] public string jp;
    [SOInject(5)] public string ch;
}