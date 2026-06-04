/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 26-06-02

 역할 : Notification 테이블의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Notification_TableSO", menuName = "Scriptable Objects/Notification_TableSO")]
public class Notification_TableSO : ScriptableObject
{
    [SOInject(0)] public int notiTemplateId;
    [SOInject(1)] public Notification_TableEnum notiType;
    [SOInject(2)] public string notiText;

}
