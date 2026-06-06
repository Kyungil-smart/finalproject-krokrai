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

    public Mission_ListSO GetMissionListData(int missionId)
    {
        if (_missionLists.ContainsKey(missionId))
        {
            return _missionLists[missionId];
        }

        return null;
    }
}
