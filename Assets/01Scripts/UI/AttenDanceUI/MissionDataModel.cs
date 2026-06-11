/*
작성자 : NekioEmilia
수정자 : 

작성일 : 26-06-08
수정일 : 

역할 : 미션 SO 데이터를 캐싱하고 반환하는 Model 스크립트
방식 : Presenter에서 미션 ID를 넘겨주면, 해당 ID의 전체 SO 데이터를 반환함
*/

using System.Collections.Generic;
using UnityEngine;

public class MissionDataModel : MonoBehaviour
{
    [SerializeField] private AutoSOGen_ContaineSO missionListSO;
    
    private Dictionary<int, Mission_ListSO> _missionLists = new();

    private void Awake()
    {
        if (missionListSO == null || missionListSO.scriptableObjects == null)
        {
            Log.Message($"<color=green> MissionListSO 자체가 비어있습니다</color>");
            return;
        }

        int addCount = 0;
        for (int i = 0; i < missionListSO.scriptableObjects.Length; i++)
        {
            // is as 문법 최적화
            if (missionListSO.scriptableObjects[i] is Mission_ListSO m)
            {
                if (!_missionLists.ContainsKey(m.Mission_Id))
                {
                    _missionLists.Add(m.Mission_Id , m);
                    addCount++;
                }
                else
                {
                    Log.Message($"<color=cyan> 중복된 미션 ID가 있음 : {m.Mission_Id}</color>");
                }
            }
        }
        
        Log.Message($"<color=yellow> 딕셔너리 세팅 완료 (총 {missionListSO.scriptableObjects.Length}개 중 {addCount}개 추가됨</color>");
    }

    /// <summary>
    /// missionId를 기반으로 미션의 SO 데이터를 반환
    /// </summary>
    /// <param name="missionId">검색할 미션의 고유 ID (ex: 49011)</param>
    /// <returns>해당 Id와 일치하는 미션 SO데이터, 존재하지 않으면 null</returns>
    public Mission_ListSO GetMissionListData(int missionId)
    {
        if (_missionLists.ContainsKey(missionId))
        {
            Log.Message($"<color=blue>missionId: {missionId}에 맞게 MissionList가 반환됨</color>");
            return _missionLists[missionId];
        }

        Log.Message($"<color=red><b>missionId: {missionId}와 일치하는 값이 없어서 null 반환함</b></color>");
        return null;
    }
}
