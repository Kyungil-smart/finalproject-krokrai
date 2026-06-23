/*
작성자 : NekioEmilia
수정자 : 
 
작성일 : 26-06-08
수정일 : 26-06-15

역할 : Reward_Group_Table의 SO를 반환해주는 스크립트
방식 : Presenter에서 그룹 ID를 넘겨주면 해당 그룹의 보상 리스트를 반환함
*/

using System.Collections.Generic;
using UnityEngine;

public class RewardDataModel : MonoBehaviour
{
    [SerializeField] private AutoSOGen_ContaineSO _rewardGroupSO;

    private Dictionary<int, List<Reward_Group_TableSO>> _rewardGroups = new();

    private void Awake()
    {
        if (_rewardGroupSO == null || _rewardGroupSO.scriptableObjects == null)
        {
            return;
        }
        
        for (int i = 0; i < _rewardGroupSO.scriptableObjects.Length; i++)
        {
            if (_rewardGroupSO.scriptableObjects[i] is Reward_Group_TableSO r)
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
        
        Log.Message($"<color=red><b>Reward_Group_Id: {index}와 일치하는 값이 없어서 null 반환함</b></color>");
        return null;
    }
}
