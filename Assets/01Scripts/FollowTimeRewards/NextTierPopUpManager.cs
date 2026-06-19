/*
 작성자 : cminhyeong1999
 작성일 : 26-06-15
 수정일 : 26-06-15

 역할 : NextTierPopUp 의 Inspector를 접근하고 관리하기 위한 매니저
*/
using UnityEngine;
using TMPro;

public class NextTierPopUpManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _tierName;
    [SerializeField] private TMP_Text _nextTierFollowValueText;

    /// <summary>
    /// 현재 레벨 이름 표시하는 메서드
    /// </summary>
    /// <param name="tierName">현재 레벨 이름</param>
    public void SetTierName(string tierName)
    {
        _tierName.text = tierName;
    }

    /// <summary>
    /// 다음 레벨까지 필요한 팔로워 수를 표시하는 메서드
    /// </summary>
    /// <param name="tierFollowValueText">다음 레벨까지 필요한 팔로워 수</param>
    public void SetNextTierFollowValueText(string tierFollowValueText)
    {
        _nextTierFollowValueText.text = tierFollowValueText;
    }

    /// <summary>
    /// 팝업창 외부 클릭 시 창을 닫기 위한 메서드
    /// </summary>
    public void OnDummyClicked()
    {
        gameObject.SetActive(false);
    }
}
