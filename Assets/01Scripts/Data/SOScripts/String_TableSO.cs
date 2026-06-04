/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 26-06-02

 역할 : String 테이블의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "String_TableSO", menuName = "Scriptable Objects/String_TableSO")]
public class String_TableSO : ScriptableObject
{
    [SOInject(0)] public string stringId;
    [SOInject(1)] public string KR;
    [SOInject(2)] public string EN;
}
