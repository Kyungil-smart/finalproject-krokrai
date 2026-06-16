using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniGame_CardFlip_CardTable_TableSO", menuName = "Scriptable Objects/MiniGame_CardFlip_CardTable_TableSO")]
public class MiniGame_CardFlip_CardTable_TableSO : ScriptableObject
{
    [SOInject(0)] public int card_ID;
    [SOInject(1)] public string car_Name;
}
