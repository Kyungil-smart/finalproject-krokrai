/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 

 역할 : String 테이블의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "String_TableSO", menuName = "Scriptable Objects/String_TableSO")]
public class String_TableSO : ScriptableObject, ISetSOData
{
    public string stringId;
    public string KR;
    public string EN;
    
    public void SetData(string[] datas)
    {
        stringId = datas[0];
        KR = datas[1];
        EN = datas[2];
    }
}
