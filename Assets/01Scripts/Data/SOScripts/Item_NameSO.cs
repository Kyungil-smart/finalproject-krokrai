/*

 
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Item_NameSO", menuName = "Scriptable Objects/Item_NameSO")]
public class Item_NameSO : ScriptableObject, ISetSOData
{
    public string Item_String_Name_Id;
    public string ko;
    public string en;
    public string jp;
    public string ch;
    
    public void SetData(string[] datas)
    {
        Item_String_Name_Id = datas[0];
        ko = datas[1];
        en = datas[2];
        jp = datas[3];
        ch = datas[4];
    }
}