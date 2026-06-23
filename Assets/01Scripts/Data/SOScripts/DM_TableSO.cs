/*
 작성자 : NekioEmilia
 수정자 : 이종현
 작성일 : 26-06-01
 수정일 : 26-06-16

 역할 : DM 테이블의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "DM_TableSO", menuName = "Scriptable Objects/DM_TableSO")]
public class DM_TableSO : ScriptableObject
{
    [SOInject(0)] public int messageId;
    [SOInject(1)] public int senderName;
    [SOInject(2)] public DMQuestTypeEnum dmQuestType;
}
