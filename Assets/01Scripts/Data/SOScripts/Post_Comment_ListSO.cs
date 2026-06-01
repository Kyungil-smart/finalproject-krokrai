/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 

 역할 : Post_Comment_List의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Post_Comment_ListSO", menuName = "Scriptable Objects/Post_Comment_ListSO")]
public class Post_Comment_ListSO : ScriptableObject, ISetSOData
{
    public int index;
    public int postId;
    public int commentId;
    public int displayOrder;
    
    public void SetData(string[] datas)
    {
        datas[0].SetData(out index);
        datas[1].SetData(out postId);
        datas[2].SetData(out commentId);
        datas[3].SetData(out displayOrder);
    }
}
