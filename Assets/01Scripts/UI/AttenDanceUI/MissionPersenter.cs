
using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionPersenter : MonoBehaviour
{
    //TODO MissionPresenter 완성 및 View 작성, UI 데이터테이블 받아와 교체
    [SerializeField] private MissionView view;
    [SerializeField] private MissionDataModel model;

    private int missionState;
    private int missionStateFlag;

    private void Awake()
    {
        ServiceLocator.Get<IEventManager>().OnDayClicked += OnDayTabChangedMission;
    }

    private void OnDestroy()
    {
        ServiceLocator.Get<IEventManager>().OnDayClicked -= OnDayTabChangedMission;
    }

    private void OnDayTabChangedMission(int day)
    {
        string dayKey = $"Day_{day}";

        var getDB = ServiceLocator.Get<IDataManager>().Event_Missions.Event_490[dayKey];
        Debug.Log(getDB);

        List<Mission_ListSO> soDataList = new();
        List<EventState> dbStateList = new();

        foreach (var item in getDB)
        {
            int missionId = int.Parse(item.Key);

            var missionListSO = model.GetMissionListData(missionId);
            
            soDataList.Add(missionListSO);
            dbStateList.Add(item.Value);
        }
        
        view.UpdateAllMissions(soDataList, dbStateList);
    }
}
