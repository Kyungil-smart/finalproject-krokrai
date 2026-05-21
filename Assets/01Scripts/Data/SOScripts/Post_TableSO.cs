/*
 작성자 : krokrai
 작성일 : 26-05-21
 수정일 : 26-05-21

 역할 : 게시물의 정보를 담고 있는 SO
 */
using UnityEngine;

[CreateAssetMenu(fileName = "Post_TableSO", menuName = "Scriptable Objects/Post_TableSO")]
public class Post_TableSO : ScriptableObject, ISetSOData
{
    public int postID;
    public ProfileTypeEnum profileType;
    public int postProFile;
    public int postImage;
    public int likeCount;
    public string captionText;
    public Route getRoute;

    public void SetData(string[] datas)
    {
        datas[0].SetData(out postID);
        datas[1].SetData<ProfileTypeEnum>(out profileType);
        datas[2].SetData(out postProFile);
        datas[3].SetData(out postImage);
        datas[4].SetData(out likeCount);
        captionText = datas[5];
        datas[6].SetData<Route>(out getRoute);
    }
}
