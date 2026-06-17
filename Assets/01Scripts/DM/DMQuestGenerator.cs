/*
작성자 : 이종현
작성일 : 26-06-16
수정일 : 26-06-16

역할 : 로컬 DM 퀘스트 생성 요청 담당
방식 : DB 저장 없이 DMListUI_LocalOnly의 로컬 Quest DM 생성 함수를 호출하여 테스트용 Quest DM을 생성
*/

using UnityEngine;

public class DMQuestGenerator : MonoBehaviour
{
    [SerializeField] private DMListUI dmListUI;
    [SerializeField] private bool generateOnStart;

    private void Start()
    {
        if (generateOnStart)
            TryGenerateQuestDM();
    }

    ///<summary>
    /// 로컬 Quest DM 생성을 요청합니다.
    ///</summary>
    public void TryGenerateQuestDM()
    {
        if (dmListUI == null)
        {
            dmListUI = FindObjectOfType<DMListUI>();
        }

        if (dmListUI == null)
        {
            Log.Message("DMListUI를 찾을 수 없습니다.");
            return;
        }

        dmListUI.TryGenerateQuestDMLocal();
    }

    ///<summary>
    /// 버튼 테스트용 Quest DM 생성 함수입니다.
    ///</summary>
    public void TestGenerateQuestDM()
    {
        TryGenerateQuestDM();
    }
}