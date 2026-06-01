/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 

 역할 : Post_notification_List의 정보를 가지고 있는 SO
*/

using UnityEngine;


[CreateAssetMenu(fileName = "Post_Notification_ListSO", menuName = "Scriptable Objects/Post_Notification_ListSO")]
public class Post_Notification_ListSO : ScriptableObject, ISetSOData
{
    public int index;
    public int postId;
    public int notiTemplate;
    public int displayOrder;
    public int commentId;
    public int referencedNpcId;
    
    public void SetData(string[] datas)
    {
        datas[0].SetData(out index);
        datas[1].SetData(out postId);
        datas[2].SetData(out notiTemplate);
        datas[3].SetData(out displayOrder);
        datas[4].SetData(out commentId);
        datas[5].SetData(out referencedNpcId);
    }
}
