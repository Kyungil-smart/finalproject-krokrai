/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 26-06-02

 역할 : Choice 테이블의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Choice_TableSO", menuName = "Scriptable Objects/Choice_TableSO")]
public class Choice_TableSO : ScriptableObject
{
    [SOInject(0)] public int choiceGroupId;
    [SOInject(1)] public int choiceNum;
    [SOInject(2)] public string choiceText;
    [SOInject(3)] public int nextDialogId;
}
