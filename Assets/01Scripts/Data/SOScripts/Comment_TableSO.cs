/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 

 역할 : Comment 테이블의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Comment_TableSO", menuName = "Scriptable Objects/Comment_TableSO")]
public class Comment_TableSO : ScriptableObject, ISetSOData
{
    public int commentId;
    public int npcId;
    public string commentText;
    
    public void SetData(string[] datas)
    {
        datas[0].SetData(out commentId);
        datas[1].SetData(out npcId);
        commentText = datas[2];
    }
}
