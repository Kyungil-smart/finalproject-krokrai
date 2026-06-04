/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 26-06-02

 역할 : DM 테이블의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "DM_TableSO", menuName = "Scriptable Objects/DM_TableSO")]
public class DM_TableSO : ScriptableObject
{
    [SOInject(0)] public int messageId;
    [SOInject(1)] public int senderName;
}
