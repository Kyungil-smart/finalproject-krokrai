/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 

 역할 : Notification 테이블의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Notification_TableSO", menuName = "Scriptable Objects/Notification_TableSO")]
public class Notification_TableSO : ScriptableObject, ISetSOData
{
    public int notiTemplateId;
    public Notification_TableEnum notiType;
    public string notiText;
    
    public void SetData(string[] datas)
    {
        datas[0].SetData(out notiTemplateId);
        datas[1].SetData<Notification_TableEnum>(out notiType);
        notiText = datas[2];
    }
}
