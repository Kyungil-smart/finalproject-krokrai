/*
작성자 : NekioEmilia
수정자 :

작성일 : 26-06-08
수정일 : 26-06-15

역할 : 미션 UI의 전체 목록을 관리하고 갱신하는 View 스크립트
방식 : Presenter로부터 데이터를 받아 각각의 MissionSlotView에 데이터를 분배함
*/

using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

public class MissionView : MonoBehaviour
{
    public event Action<int> OnSlotRewardRequested; // 보상 버튼 클릭 Action // < 오류 원인

    [SerializeField] private MissionSlotView[] _missionSlots;

    [Header("메인 화면 스토리 UI 연결")] [SerializeField] private GameObject _mainStoryGroup;

    private void Awake()
    {
        for (int i = 0; i < _missionSlots.Length; i++)
        {
            _missionSlots[i].InitSlot(i);

            _missionSlots[i].OnRewardClicked += (idx) => { OnSlotRewardRequested?.Invoke(idx); };
        }
    }

    /// <summary>
    /// Presenter에게 전달받은 데이터로 전체 미션 슬롯의 UI를 갱신
    /// </summary>
    /// <param name="missionSoData">MissionList SO 데이터 리스트</param>
    /// <param name="dbStates">미션 진행도 리스트</param>
    /// <param name="rewardDataList">각 미션별 보상 리스트</param>
    public void UpdateAllMissions(List<Mission_ListSO> missionSoData, List<EventState> dbStates, List<List<Reward_Group_TableSO>> rewardDataList)
    {
        foreach (var slot in _missionSlots)
        {
            if (slot != null) slot.gameObject.SetActive(false);
        }
        
        if (missionSoData == null || dbStates == null)
        {
            return;
        }

        for (int i = 0; i < _missionSlots.Length; i++)
        {
            if (i >= missionSoData.Count || i >= dbStates.Count)
            {
                continue;
            }
            
            _missionSlots[i].gameObject.SetActive(true);

            // 슬롯이 받아야 할 보상 리스트 하나만 넘김
            List<Reward_Group_TableSO> slotReward = null;
            if (rewardDataList != null && i < rewardDataList.Count)
            {
                slotReward = rewardDataList[i];
            }

            UpdateSingleSlot(i, missionSoData[i], dbStates[i], slotReward);
        }
    }

    /// <summary>
    /// 지정한 인덱스의 미션 슬롯 1개만 단일 갱신
    /// </summary>
    /// <param name="index">갱신할 슬롯의 배열 인덱스</param>
    /// <param name="soData">해당 슬롯의 원본 데이터</param>
    /// <param name="dbState">해당 슬롯의 진행도 데이터</param>
    /// <param name="rewardData"></param>
    public void UpdateSingleSlot(int index, Mission_ListSO soData, EventState dbState, List<Reward_Group_TableSO> rewardData)
    {
        string missionDesc = soData.Mission_Desc;
        int goal = soData.Goal_Value;
        int currentState = (int)dbState.Mission_State;
        int flag = (int)dbState.Mission_State_Flag;

        /*
        if (!soData.Check_Desc)
        {
            currentState = goal;
        }
        */

        _missionSlots[index].UpdateSlotUI(missionDesc, currentState, goal, flag, rewardData);
    }
}