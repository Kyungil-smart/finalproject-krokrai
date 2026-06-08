/*
작성자 : 이종현
작성일 : 26-06-04
수정일 : 26-06-05

역할 : 팝업 활성화 / 비활성화 담당
방식 : 버튼 클릭 시 지정된 팝업을 활성화하거나 비활성화하고 Dropdown 잔여 오브젝트를 정리
*/

using UnityEngine;

public class PopupUI : MonoBehaviour
{
    [SerializeField] private GameObject _targetPopup;

    public void OpenPopup()
    {
        ClearDropdownObjects();

        if (_targetPopup == null)
        {
            Log.Message("TargetPopup이 연결되지 않았습니다.");
            return;
        }

        _targetPopup.SetActive(true);
    }

    public void ClosePopup()
    {
        ClearDropdownObjects();

        if (_targetPopup == null)
        {
            Log.Message("TargetPopup이 연결되지 않았습니다.");
            return;
        }

        _targetPopup.SetActive(false);
    }

    private void ClearDropdownObjects()
    {
        GameObject dropdownList = GameObject.Find("Dropdown List");
        if (dropdownList != null)
            Destroy(dropdownList);

        GameObject blocker = GameObject.Find("Blocker");
        if (blocker != null)
            Destroy(blocker);
    }
}