/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 26-06-02

 역할 : Comment 테이블의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Comment_TableSO", menuName = "Scriptable Objects/Comment_TableSO")]
public class Comment_TableSO : ScriptableObject
{
    [SOInject(0)] public int commentId;
    [SOInject(1)] public int npcId;
    [SOInject(2)] public string commentText;
}
