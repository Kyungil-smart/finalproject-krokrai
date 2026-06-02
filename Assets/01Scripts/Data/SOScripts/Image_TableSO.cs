/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 26-06-02

 역할 : Image 테이블의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Image_TableSO", menuName = "Scriptable Objects/Image_TableSO")]
public class Image_TableSO : ScriptableObject
{
    [SOInject(0)] public int ImgId;
    [SOInject(1)] public string ImgPath;
    [SOInject(2)] public Image_TableEnum ImgType;
    [SOInject(3)] public int imgFolder;

}
