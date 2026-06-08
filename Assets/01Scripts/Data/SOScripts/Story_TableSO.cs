
/*
 작성자 : NekioEmilia
 작성일 : 26-06-08
 수정일 : 

 역할 : Story_Table의 정보를 가지고 있는 SO
*/
using UnityEngine;

[CreateAssetMenu(fileName = "Story_TableSO", menuName = "Scriptable Objects/Story_TableSO")]
public class Story_TableSO : ScriptableObject
{
    [SOInject(0)] public string Story_Id;
    [SOInject(1)] public string ko_Title;
    [SOInject(2)] public string en_Title;
    [SOInject(3)] public string jp_Title;
    [SOInject(4)] public string ch_Title;
    [SOInject(5)] public string ko_Text;
    [SOInject(6)] public string en_Text;
    [SOInject(7)] public string jp_Text;
    [SOInject(8)] public string ch_Text;
}
