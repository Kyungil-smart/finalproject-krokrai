/*


*/

using UnityEngine;

[CreateAssetMenu(fileName = "Item_TableSO", menuName = "Scriptable Objects/Item TableSO")]
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