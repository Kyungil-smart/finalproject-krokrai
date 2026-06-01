/*
 작성자 : NekioEmilia
 작성일 : 26-06-01
 수정일 : 

 역할 : Npc 테이블의 정보를 가지고 있는 SO
*/

using UnityEngine;

[CreateAssetMenu(fileName = "Npc_TableSO", menuName = "Scriptable Objects/Npc_TableSO")]
public class Npc_TableSO : ScriptableObject, ISetSOData
{
    public int npcId;
    public string npcAccountName;
    public bool hasMark;
    public string npcDisplayName;
    public string npcBio;
    public int npcImage;
    public int npcFollowCount;
    public int npcFollowingCount;
    
    public void SetData(string[] datas)
    {
        datas[0].SetData(out npcId);
        npcAccountName = datas[1];
        datas[2].SetData(out hasMark);
        npcDisplayName = datas[3];
        npcBio = datas[4];
        datas[5].SetData(out npcImage);
        datas[6].SetData(out npcFollowCount);
        datas[7].SetData(out npcFollowingCount);
    }
}
