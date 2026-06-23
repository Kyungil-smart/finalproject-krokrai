/*
작성자 : NekioEmilia
수정자 : 

작성일 : 26-06-08
수정일 : 26-06-15

역할 : 미션 SO 데이터를 캐싱하고 반환하는 Model 스크립트
방식 : Presenter에서 미션 ID를 넘겨주면, 해당 ID의 전체 SO 데이터를 반환함
*/

using System.Collections.Generic;
using UnityEngine;

public class MissionDataModel : MonoBehaviour
{
    [SerializeField] private AutoSOGen_ContaineSO _missionListSO;
    
    private Dictionary<int, Mission_ListSO> _missionLists = new();

    private void Awake()
    {
        if (_missionListSO == null || _missionListSO.scriptableObjects == null)
        {
            return;
        }
        
        for (int i = 0; i < _missionListSO.scriptableObjects.Length; i++)
        {
            // is as 문법 최적화
            if (_missionListSO.scriptableObjects[i] is Mission_ListSO m)
            {
                if (!_missionLists.ContainsKey(m.Mission_Id))
                {
                    _missionLists.Add(m.Mission_Id , m);
                }
            }
        }
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
            return _missionLists[missionId];
        }

        Log.Message($"<color=red><b>missionId: {missionId}와 일치하는 값이 없어서 null 반환함</b></color>");
        return null;
    }

    /// <summary>
    /// 특정 일차에 해당하는 미션 SO 데이터들만 필터링해서 반환하는 메서드
    /// </summary>
    /// <param name="day">검색할 일 차</param>
    /// <returns>해당 일차와 매칭되는 미션 SO</returns>
    public List<Mission_ListSO> GetMissionsByDay(int day)
    {
        List<Mission_ListSO> result = new List<Mission_ListSO>();

        foreach (var m in _missionLists.Values)
        {
            if (m.Day_Index == day)
            {
                result.Add(m);
            }
        }

        return result;
    }
}


