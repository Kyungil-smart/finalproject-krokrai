using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniGame_CardFlip_CardTable_TableSO", menuName = "Scriptable Objects/MiniGame_CardFlip_CardTable_TableSO")]
public class MiniGame_CardFlip_RewardTableSO : ScriptableObject
{
    [SOInject(0)] public int matchedPairs;
    [SOInject(1)] public MiniGame_Reward_TypeEnum rewardType;
    [SOInject(2)] public int rewardAmount;
}
