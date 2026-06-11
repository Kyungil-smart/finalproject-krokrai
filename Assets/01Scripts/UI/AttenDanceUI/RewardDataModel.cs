/*
작성자 : NekioEmilia
수정자 : 
 
작성일 : 26-06-08
수정일 : 

역할 : Reward_Group_Table의 SO를 반환해주는 스크립트
방식 : Presenter에서 그룹 ID를 넘겨주면 해당 그룹의 보상 리스트를 반환함
*/

using System.Collections.Generic;
using UnityEngine;

public class RewardDataModel : MonoBehaviour
{
    [SerializeField] private AutoSOGen_ContaineSO rewardGroupSO;

    private Dictionary<int, List<Reward_Group_TableSO>> _rewardGroups = new();

    private void Awake()
    {
        if (rewardGroupSO == null || rewardGroupSO.scriptableObjects == null)
        {
            Log.Message($"<color=green> rewardGroupSO 자체가 비어있습니다</color>");
            return;
        }

        int groupCount = 0; // 만들어진 그룹의 개수
        int totalItemCount = 0; // 들어간 보상 아이템의 총 개수
        
        for (int i = 0; i < rewardGroupSO.scriptableObjects.Length; i++)
        {
            if (rewardGroupSO.scriptableObjects[i] is Reward_Group_TableSO r)
            {
                if (!_rewardGroups.ContainsKey(r.Reward_Group_Id))
                {
                    _rewardGroups[r.Reward_Group_Id] = new List<Reward_Group_TableSO>();
                    groupCount++;
                }
                
                _rewardGroups[r.Reward_Group_Id].Add(r);
                totalItemCount++;
            }
        }
        
        Log.Message($"<color=yellow>보상 세팅 완료 (총 {groupCount}개 그룹, {totalItemCount}개 아이템)</color>");
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
            Log.Message($"<color=blue>Reward_Group_Id: {index}에 맞게 Reward_Group이 반환됨</color>");
            return _rewardGroups[index];
        }
        
        Log.Message($"<color=red><b>Reward_Group_Id: {index}와 일치하는 값이 없어서 null 반환함</b></color>");
        return null;
    }
}
