/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 

 역할 : Image 테이블의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Image_TableSO", menuName = "Scriptable Objects/Image_TableSO")]
public class Image_TableSO : ScriptableObject, ISetSOData
{
    public int ImgId;
    public string ImgPath;
    public Image_TableEnum ImgType;
    public int imgFolder;
    
    public void SetData(string[] datas)
    {
        datas[0].SetData(out ImgId);
        ImgPath = datas[1];
        datas[2].SetData<Image_TableEnum>(out ImgType);
        datas[3].SetData(out imgFolder);
    }
}
