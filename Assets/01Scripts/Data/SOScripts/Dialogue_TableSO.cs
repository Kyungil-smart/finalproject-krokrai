/*
 작성자 : NekioEmilia
 수정자 : 이종현
 작성일 : 26-06-01
 수정일 : 26-06-16

 역할 : Dialogue 테이블의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue_TableSO", menuName = "Scriptable Objects/Dialogue_TableSO")]
public class Dialogue_TableSO : ScriptableObject
{
    [SOInject(0)] public int dialogId;
    [SOInject(1)]public int messageId;
    [SOInject(2)] public Dialogue_TableEnum senderType;
    [SOInject(3)] public string dialogText;
    [SOInject(4)] public int nextDialogId;
    [SOInject(5)] public int choiceGroupId;
    [SOInject(6)] public bool isEnd;
    [SOInject(7)] public bool isRequest;
    [SOInject(8)] public int feedPostId;
    [SOInject(9)] public int requestId;
    [SOInject(10)] public int rewardFollower;
}
