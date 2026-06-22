/*
작성자 : 이종현
작성일 : 26-06-16

역할 : DM 요청 퀘스트 데이터를 담는 SO
방식 : Request_Table TSV에서 필요한 아이템과 보상 아이템 정보를 주입받아 사용
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Request_TableSO", menuName = "Scriptable Objects/Request_TableSO")]
public class Request_TableSO : ScriptableObject
{
    [SOInject(0)] public int requestId;
    [SOInject(1)] public string reqItemId;
    [SOInject(2)] public int reqItemCount;
    [SOInject(3)] public int reqImgId;
    [SOInject(4)] public string rewardItemId;
    [SOInject(5)] public int rewardItemCount;
    [SOInject(6)] public int rewardImgId;
}