/*
 작성자 : NekioEmilia
 수정자 : krokrai
 작성일 : 26-06-01
 수정일 : 26-06-05

 역할 : Post_notification_List의 정보를 가지고 있는 SO
*/

using UnityEngine;


[CreateAssetMenu(fileName = "Post_Notification_ListSO", menuName = "Scriptable Objects/Post_Notification_ListSO")]
public class Post_Notification_ListSO : ScriptableObject
{
    [SOInject(0)] public int index;
    [SOInject(1)] public int postId;
    [SOInject(2)] public int notiTemplate;
    [SOInject(3)] public int displayOrder;
    [SOInject(5)] public string commentText;
    [SOInject(4)] public int referencedNpcId;
}
