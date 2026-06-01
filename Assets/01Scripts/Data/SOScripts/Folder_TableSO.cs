/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 

 역할 : Folder 테이블의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Folder_TableSO", menuName = "Scriptable Objects/Folder_TableSO")]
public class Folder_TableSO : ScriptableObject, ISetSOData
{
    public int folderId;
    public string folderName;
    
    public void SetData(string[] datas)
    {
        datas[0].SetData(out folderId);
        folderName = datas[1];
    }
}
