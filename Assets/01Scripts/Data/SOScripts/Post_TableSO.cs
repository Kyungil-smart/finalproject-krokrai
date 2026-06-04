/*
 작성자 : krokrai
 작성일 : 26-05-21
 수정일 : 26-06-02
 수정자 : NekioEmilia

 역할 : 게시물의 정보를 담고 있는 SO
 */
using UnityEngine;

[CreateAssetMenu(fileName = "Post_TableSO", menuName = "Scriptable Objects/Post_TableSO")]
public class Post_TableSO : ScriptableObject
{
    [SOInject(1)] public ProfileTypeEnum profileType;
    [SOInject(2)] public int postProFile;
    [SOInject(3)] public int postImage;
    [SOInject(4)] public int likeCount;
    [SOInject(5)] public string captionText;
    [SOInject(6)] public Route getRoute;
    [SOInject(0)] public int postID;
}
