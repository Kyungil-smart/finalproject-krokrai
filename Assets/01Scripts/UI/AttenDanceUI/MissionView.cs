
using UnityEngine;
using System;
using System.Collections.Generic;

public class MissionView : MonoBehaviour
{
    public event Action<int> OnSlotRewardRequested; // 보상 Action

    [SerializeField] private MissionSlotView[] missionSlots;

    private void Awake()
    {
        for (int i = 0; i < missionSlots.Length; i++)
        {
            missionSlots[i].InitSlot(i);

            missionSlots[i].OnRewardClicked += (idx) =>
            {
                OnSlotRewardRequested?.Invoke(idx);
            };
        }
    }

    /// <summary>
    /// Presenter에게 전달받은 데이터로 전체 미션 슬롯의 UI를 갱신
    /// </summary>
    /// <param name="missionSoData">MissionList SO 데이터 리스트</param>
    /// <param name="dbStates">미션 진행도 리스트</param>
    /// <param name="rewardDataList"></param>
    public void UpdateAllMissions(List<Mission_ListSO> missionSoData, List<EventState> dbStates, List<List<Reward_Group_TableSO>> rewardDataList)
    {
        for (int i = 0; i < missionSlots.Length; i++)
        {
            if (i >= missionSoData.Count || i >= dbStates.Count) return;

            // 슬롯이 받아야 할 보상 리스트 하나만 넘김
            List<Reward_Group_TableSO> slotReward;

            if (rewardDataList != null && i < rewardDataList.Count)
            {
                slotReward = rewardDataList[i];
            }
            else
            {
                slotReward = null;
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
        
        missionSlots[index].UpdateSlotUI(missionDesc, goal, currentState, flag, rewardData);
    }
}
