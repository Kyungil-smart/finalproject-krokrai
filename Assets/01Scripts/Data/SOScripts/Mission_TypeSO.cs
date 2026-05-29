/*


*/

using UnityEngine;

[CreateAssetMenu(fileName = "Mission_TypeSO", menuName = "Scriptable Objects/Mission_TypeSO")]
public class Mission_TypeSO : ScriptableObject, ISetSOData
{
    public int Mission_Type;
    public string Mission_String_Id;
    
    public void SetData(string[] datas)
    {
        datas[0].SetData(out Mission_Type);
        Mission_String_Id = datas[1];
    }
}