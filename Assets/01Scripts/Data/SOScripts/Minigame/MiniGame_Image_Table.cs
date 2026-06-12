using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniGame_Image_TableSO", menuName = "Scriptable Objects/MiniGame_Image_TableSO")]
public class MiniGame_Image_TableSO : ScriptableObject
{
    [SOInject(0)] public string imgID;
}
