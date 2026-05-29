/*
작성자 : NekioEmilia
수정자 : 
작성일 : 26-05-29
수정일 : 26-05-29

역할 : 미션의 누적 세팅을 담고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Mission_Gauge_SettingSO", menuName = "Scriptable Objects/Mission_Gauge_SettingSO")]
public class Mission_Gauge_SettingSO : ScriptableObject, ISetSOData
{
    public int Gauge_Reward_Id;
    public int Gauge_Step;
    public int Goal_Point;
    public int Reward_Accrue_Id;
    
    public void SetData(string[] datas)
    {
        datas[0].SetData(out Gauge_Reward_Id);
        datas[1].SetData(out Gauge_Step);
        datas[2].SetData(out Goal_Point);
        datas[3].SetData(out Reward_Accrue_Id);
    }
}
// TODO: 해당 SO, DataBaker 제작 후 tsv bake 하기 // 3, 4, 5, 6, 7, 8 다 만들고 SO, DataBaker 제작 후 tsv bake 하기