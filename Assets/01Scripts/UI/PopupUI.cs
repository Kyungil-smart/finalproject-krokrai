/*
작성자 : 이종현
작성일 : 26-06-04
수정일 : 26-06-04

역할 : 팝업 활성화 / 비활성화 담당
방식 : 버튼 클릭 시 지정된 팝업 Canvas를 활성화하거나 비활성화
*/

using UnityEngine;

public class PopupUI : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] private GameObject targetPopup;

    public void OpenPopup()
    {
        if (targetPopup == null)
        {
            Log.Message("Target Popup이 연결되지 않았습니다.");
            return;
        }

        targetPopup.SetActive(true);
    }

    public void ClosePopup()
    {
        if (targetPopup == null)
        {
            Log.Message("Target Popup이 연결되지 않았습니다.");
            return;
        }
        Log.Message("ClosePopup 호출");
        targetPopup.SetActive(false);
    }
}