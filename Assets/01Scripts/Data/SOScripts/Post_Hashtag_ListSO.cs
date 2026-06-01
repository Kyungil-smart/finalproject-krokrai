/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 

 역할 : Post_Hashtag_List의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Post_Hashtag_ListSO", menuName = "Scriptable Objects/Post_Hashtag_ListSO")]
public class Post_Hashtag_ListSO : ScriptableObject, ISetSOData
{
    public int index;
    public int postId;
    public int hashtagId;
    public int displayOrder;
    
    public void SetData(string[] datas)
    {
        datas[0].SetData(out index);
        datas[1].SetData(out postId);
        datas[2].SetData(out hashtagId);
        datas[3].SetData(out displayOrder);
    }
}
