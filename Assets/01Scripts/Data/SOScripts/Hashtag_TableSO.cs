/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 

 역할 : Hashtag 테이블의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Hashtag_TableSO", menuName = "Scriptable Objects/Hashtag_TableSO")]
public class Hashtag_TableSO : ScriptableObject, ISetSOData
{
    public int hashtagId;
    public string hashtagText;
    
    public void SetData(string[] datas)
    {
        datas[0].SetData(out hashtagId);
        hashtagText = datas[1];
    }
}
