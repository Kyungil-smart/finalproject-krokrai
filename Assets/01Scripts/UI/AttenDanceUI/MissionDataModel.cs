using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionDataModel : MonoBehaviour
{
    [SerializeField] private AutoSOGen_ContaineSO missionListSO;

    private Dictionary<int, Mission_ListSO> _missionLists = new();

    private void Awake()
    {
        Mission_ListSO m;
        
        for (int i = 0; i < missionListSO.scriptableObjects.Length; i++)
        {
            if (missionListSO.scriptableObjects[i] is Mission_ListSO)
            {
                m = (missionListSO.scriptableObjects[i] as Mission_ListSO);
                _missionLists.Add(m.Mission_Id , m);
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

        return null;
    }
}
