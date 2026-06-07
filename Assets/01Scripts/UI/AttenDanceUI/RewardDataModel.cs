using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardDataModel : MonoBehaviour
{
    [SerializeField] private AutoSOGen_ContaineSO rewardGroupSO;

    private Dictionary<int, List<Reward_Group_TableSO>> _rewardGroups = new();

    private void Awake()
    {
        for (int i = 0; i < rewardGroupSO.scriptableObjects.Length; i++)
        {
            if (rewardGroupSO.scriptableObjects[i] is Reward_Group_TableSO r)
            {
                if (!_rewardGroups.ContainsKey(r.Reward_Group_Id))
                {
                    _rewardGroups[r.Reward_Group_Id] = new List<Reward_Group_TableSO>();
                }

                _rewardGroups[r.Reward_Group_Id].Add(r);
            }
        }
    }

    /// <summary>
    /// Reward Table의 index(순서)를 기반으로 보상의 SO 데이터를 반환
    /// </summary>
    /// <param name="index">검색할 보상의 고유 index (ex: 44)</param>
    /// <returns>해당 index와 일치하는 보상 SO 데이터, 존재하지 않으면 null</returns>
    public List<Reward_Group_TableSO> GetRewardGroup(int index)
    {
        if (_rewardGroups.ContainsKey(index))
        {
            return _rewardGroups[index];
        }

        return null;
    }
}
