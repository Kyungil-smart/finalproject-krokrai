using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game_1_Table", menuName = "Scriptable Objects/Game_1_Table")]
public class Game_1_TableSO : ScriptableObject
{
    [SOInject(0)] public int index;
    [SOInject(1)] public int Game_ID;
    [SOInject(2)] public int Flip_Count;
    [SOInject(3)] public int Get_Token;
}
