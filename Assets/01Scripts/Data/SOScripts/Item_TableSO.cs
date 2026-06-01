/*
작성자 : 김영빈
작성일 : 26-05-29
수정일 : 26-06-01 (menuName에 언더바 추가)

역할 : 아이템에 대한 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Item_TableSO", menuName = "Scriptable Objects/Item_TableSO")]
public class Item_TableSO : ScriptableObject, ISetSOData
{
    public int Item_Id;
    public string Item_String_Name_Id;
    public string Item_string_Res_Id;
    
    public void SetData(string[] datas)
    {
        datas[0].SetData(out Item_Id);
        Item_String_Name_Id = datas[1];
        Item_string_Res_Id = datas[2];
    }
}