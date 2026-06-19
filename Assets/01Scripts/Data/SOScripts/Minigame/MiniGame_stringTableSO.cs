using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniGame_stringTableSO", menuName = "Scriptable Objects/MiniGame_stringTableSO")]
public class MiniGame_stringTableSO : ScriptableObject
{
    [SOInject(0)] public string stringId;
    [SOInject(1)] public string KR;
    [SOInject(2)] public string EN;
}
