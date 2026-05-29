/*


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