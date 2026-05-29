/*
작성자 : NekioEmilia
수정자 : 
작성일 : 26-05-29
수정일 : 26-05-29

역할 : 미션의 대한 정보를 담고 있는 SO
*/
using UnityEngine;


[CreateAssetMenu(fileName = "Mission_ListSO", menuName = "Scriptable Objects/Mission_ListSO")]
public class Mission_ListSO : ScriptableObject, ISetSOData
{
    public int Mission_Id;
    public int Day_Index;
    public int Mission_Order;
    public string Mission_Desc;
    public bool Check_Desc;
    public int Mission_Type;
    public int Target_Id;
    public int Goal_Value;
    public int Festa_Point;
    public int Reward_Daliy_Id;
    public string Mission_String_Id;
    
    public void SetData(string[] datas)
    {
        datas[0].SetData(out Mission_Id);
        datas[1].SetData(out Day_Index);
        datas[2].SetData(out Mission_Order);
        Mission_Desc = datas[3];
        datas[4].SetData(out Check_Desc);
        datas[5].SetData(out Mission_Type);
        datas[6].SetData(out Target_Id);
        datas[7].SetData(out Goal_Value);
        datas[8].SetData(out Festa_Point);
        datas[9].SetData(out Reward_Daliy_Id);
        Mission_String_Id = datas[10];
    }
}
