/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 

 역할 : Post_Hashtag_List의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Post_Hashtag_ListSO", menuName = "Scriptable Objects/Post_Hashtag_ListSO")]
public class Post_Hashtag_ListSO : ScriptableObject
{
    [SOInject(0)] public int index;
    [SOInject(1)] public int postId;
    [SOInject(2)] public int hashtagId;
    [SOInject(3)] public int displayOrder;
}