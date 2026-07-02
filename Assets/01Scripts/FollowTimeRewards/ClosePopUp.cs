/*
 작성자 : cminhyeong1999
 작성일 : 26-07-02
 수정일 : 26-07-02

 역할 : SNS 팔로워 방치 보상 팝업창 외부 클릭시 비활성화
 
 방식 : TimeRewards_PopUp 의 Dummy 클릭 시 실행
       보상 팝업 구간 클릭 시 -> 미니 팝업만 종료 
       보상 팝업 외부 클릭 시 -> 보상 팝업+미니 팝업 함께 종료
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePopUp : MonoBehaviour
{
    [SerializeField] private GameObject _timeRewardPopUp;
    [SerializeField] private GameObject _nextTierPopUp;
    
    /// <summary>
    /// 팝업영역 외부 클릭시 실행할 메서드
    /// </summary>
    public void OnClickClosePopUp()
    {
        if (_timeRewardPopUp != null && _timeRewardPopUp.activeSelf)
        {
            if (_nextTierPopUp != null && _nextTierPopUp.activeSelf)
            {
                _nextTierPopUp.SetActive(false);
            }
            _timeRewardPopUp.SetActive(false);
        }
    }
}
