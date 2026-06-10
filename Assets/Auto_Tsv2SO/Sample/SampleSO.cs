using UnityEngine;

[CreateAssetMenu(fileName = "SampleSO", menuName = "Scriptable Objects/SampleSO")]
public class SampleSO : ScriptableObject
{
    [SOInject(0)] public string s;
    [SOInject(1)] public int i;
    [SOInject(2)] public float f;
    [SOInject(3)] public ennn e;
    [SOInject(4)] public bool b;

}

public enum ennn
{
    TYPE,ENTER
}