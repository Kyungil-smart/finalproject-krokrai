/*
 작성자 : cminhyeong1999
 작성일 : 26-06-25
 수정일 : 26-06-25

 역할 : SNS 팔로워 시스템 관련 String 테이블의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "SNS_Follow_TierSO", menuName = "Scriptable Objects/SNS_Follow_TierSO")]
public class SNS_Follow_TierSO : ScriptableObject
{
    [SOInject(0)] public string stringId;
    [SOInject(1)] public string KR;
    [SOInject(2)] public string EN;

    
}
