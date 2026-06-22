using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game_3_Table", menuName = "Scriptable Objects/Game_3_Table")]
public class Game_3_TableSO : ScriptableObject
{
    [SOInject(0)] public int index;
    [SOInject(1)] public int Game_ID;
    [SOInject(2)] public int Catch_Score;
    [SOInject(3)] public int Get_Token;
}
