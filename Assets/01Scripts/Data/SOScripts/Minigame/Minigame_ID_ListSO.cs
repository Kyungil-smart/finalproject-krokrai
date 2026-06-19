using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Minigame_ID_ListSO",menuName = "Scriptable Objects/Minigame_ID_ListSO")]
public class Minigame_ID_ListSO : ScriptableObject
{
    [SOInject(0)] public int Game_ID;
    [SOInject(1)] public string Name;
    [SOInject(2)] public int Sort_Order;
    [SOInject(3)] public string Image_ID;
}
