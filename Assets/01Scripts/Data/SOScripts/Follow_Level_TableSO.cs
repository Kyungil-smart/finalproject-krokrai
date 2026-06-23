/*
 작성자 : cminhyeong1999
 작성일 : 26-06-15
 수정일 : 26-06-15

 역할 : Follow 레벨 테이블의 정보를 가지고 있는 SO
*/
using UnityEngine;

[CreateAssetMenu(fileName = "Follow_Level_TableSO", menuName = "Scriptable Objects/Follow_Level_TableSO")]
public class Follow_Level_TableSO : ScriptableObject
{
    [SOInject(0)] public int level;
    [SOInject(1)] public int requiredFollowers;
    [SOInject(2)] public string tierName;
    [SOInject(3)] public int maxEnergy;
    [SOInject(4)] public int maxCoin;
}
