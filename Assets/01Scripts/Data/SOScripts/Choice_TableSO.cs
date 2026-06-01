/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 

 역할 : Choice 테이블의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Choice_TableSO", menuName = "Scriptable Objects/Choice_TableSO")]
public class Choice_TableSO : ScriptableObject, ISetSOData
{
    public int choiceGroupId;
    public int choiceNum;
    public string choiceText;
    public int nextDialogId;
    
    public void SetData(string[] datas)
    {
        datas[0].SetData(out choiceGroupId);
        datas[1].SetData(out choiceNum);
        choiceText = datas[2];
        datas[3].SetData(out nextDialogId);
    }
}
