/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 26-06-02

 역할 : Npc 테이블의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Npc_TableSO", menuName = "Scriptable Objects/Npc_TableSO")]
public class Npc_TableSO : ScriptableObject
{
    [SOInject(0)] public int npcId;
    [SOInject(1)] public string npcAccountName;
    [SOInject(2)] public bool hasMark;
    [SOInject(3)] public string npcDisplayName;
    [SOInject(4)] public string npcBio;
    [SOInject(5)] public int npcImage;
    [SOInject(6)] public int npcFollowCount;
    [SOInject(7)] public int npcFollowingCount;
}
