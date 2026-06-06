
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

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

    public void UpdateAllMissions(List<Mission_ListSO> missionSoData, List<EventState> dbStates)
    {
        for (int i = 0; i < missionSlots.Length; i++)
        {
            if (i >= missionSoData.Count || i >= dbStates.Count) return;

            string missionName = missionSoData[i].name;
            int goal = missionSoData[i].Goal_Value;

            int currentState = (int)dbStates[i].Mission_State;
            int flag = (int)dbStates[i].Mission_State_Flag;
            
            missionSlots[i].UpdateSlotUI(missionName, currentState, goal, flag);
        }
    }
}
