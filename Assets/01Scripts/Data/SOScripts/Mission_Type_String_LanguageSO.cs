using UnityEngine;

[CreateAssetMenu(fileName = "Mission_Type_String_LanguageSO", menuName = "Scriptable Objects/Mission_Type_String_LanguageSO")]
public class Mission_Type_String_LanguageSO : ScriptableObject, ISetSOData
{
    public string Mission_String_Id;
    public int Mission_Type;
    public string ko;
    public string en;
    public string jp;
    public string ch;
    
    public void SetData(string[] datas)
    {
        Mission_String_Id = datas[0];
        datas[1].SetData(out Mission_Type);
        ko = datas[2];
        en = datas[3];
        jp = datas[4];
        ch = datas[5];
    }
}