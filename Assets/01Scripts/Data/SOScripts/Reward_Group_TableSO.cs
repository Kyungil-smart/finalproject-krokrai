/*


*/

using UnityEngine;

[CreateAssetMenu(fileName = "Reward_Group_TableSO", menuName = "Scriptable Objects/Reward_Group_TableSO")]
public class Reward_Group_TableSO : ScriptableObject, ISetSOData
{
    public int Index;
    public int Reward_Group_Id;
    public int Reward_Order;
    public string Reward_Name;
    public int Reward_Id;
    public int Amount;
    
    public void SetData(string[] datas)
    {
        datas[0].SetData(out Index);
        datas[1].SetData(out Reward_Group_Id);
        datas[2].SetData(out Reward_Order);
        Reward_Name = datas[3];
        datas[4].SetData(out Reward_Id);
        datas[5].SetData(out Amount);
    }
}
