using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniGame_ConfigSO", menuName = "Scriptable Objects/MiniGame_ConfigSO")]
public class MiniGame_ConfigSO : ScriptableObject
{
    [SOInject(0)] public int index;
    [SOInject(1)] public string Config_Key;
    [SOInject(2)] public int Value;
}
