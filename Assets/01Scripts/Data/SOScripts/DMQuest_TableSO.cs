/*
 작성자 : 이종현
 작성일 : 26-06-16
 수정일 : 26-06-16

 역할 : DM 퀘스트 메인 테이블 정보를 가지고 있는 SO
 방식 : DM 퀘스트 ID, NPC, DM, 타입, 보상 정보를 데이터 테이블에서 주입받아 저장
*/

using UnityEngine;

[CreateAssetMenu(fileName = "DMQuest_TableSO", menuName = "Scriptable Objects/DMQuest_TableSO")]
public class DMQuest_TableSO : ScriptableObject
{
    [SOInject(0)] public int dmQuestId;
    [SOInject(1)] public int npcId;
    [SOInject(2)] public int dmId;
    [SOInject(3)] public DMQuestTypeEnum questType;
    [SOInject(4)] public int followerReward;
}