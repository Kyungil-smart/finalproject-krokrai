/*
 작성자 : 23M-RFT68
 작성일 : 26-06-16
 수정일 : 26-06-16
 수정자 : 23M-RFT68

 역할 : 가구 오브젝트의 정보를 담고 있는 SO
 */
using UnityEngine;

[CreateAssetMenu(fileName = "Object_TableSO", menuName = "Scriptable Objects/Object_TableSO")]
public class Object_TableSO : ScriptableObject
{
    [SOInject(0)] public int objectId;
    [SOInject(1)] public int getImg;
    [SOInject(2)] public int objectImg;

}

