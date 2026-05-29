/*
작성자 : 김영빈
작성일 : 26-05-29

역할 : 아이템의 리소스를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Item_ResSO", menuName = "Scriptable Objects/Item_ResSO")]
public class Item_ResSO : ScriptableObject, ISetSOData
{
    public string Item_String_Res_Id;
    public string Item_res;
    
    public void SetData(string[] datas)
    {
        Item_String_Res_Id = datas[0];
        Item_res = datas[1];
    }
}