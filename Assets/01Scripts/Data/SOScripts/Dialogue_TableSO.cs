/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 

 역할 : Dialogue 테이블의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue_TableSO", menuName = "Scriptable Objects/Dialogue_TableSO")]
public class Dialogue_TableSO : ScriptableObject, ISetSOData
{
    public int dialogId;
    public int messageId;
    public Dialogue_TableEnum senderType;
    public string dialogText;
    public int nextDialogId;
    public int choiceGroupId;
    public bool isEnd;
    
    public void SetData(string[] datas)
    {
        datas[0].SetData(out dialogId);
        datas[1].SetData(out messageId);
        datas[2].SetData<Dialogue_TableEnum>(out senderType);
        dialogText = datas[3];
        datas[4].SetData(out nextDialogId);
        datas[5].SetData(out choiceGroupId);
        datas[6].SetData(out isEnd);
    }
}
