/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 

 역할 : DM 테이블의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "DM_TableSO", menuName = "Scriptable Objects/DM_TableSO")]
public class DM_TableSO : ScriptableObject, ISetSOData
{
    public int messageId;
    public int senderName;
    
    public void SetData(string[] datas)
    {
        datas[0].SetData(out messageId);
        datas[1].SetData(out senderName);
    }
}
