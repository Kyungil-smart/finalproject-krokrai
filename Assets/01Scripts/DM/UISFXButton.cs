/*
작성자 : 이종현
작성일 : 26-06-26
수정일 : 26-06-26

역할 : UI 버튼 클릭 사운드 재생
방식 : 버튼 OnClick 이벤트에서 호출되어 AudioManager의 SFX를 재생
*/

using UnityEngine;

public class UISFXButton : MonoBehaviour
{
    ///<summary>
    /// 기본 버튼 클릭 사운드를 재생합니다.
    ///</summary>
    public void PlayButtonSFX()
    {
        ServiceLocator.Get<IAudioManager>().PlaySFX(SFXAudiosEnum.BTN2);
    }
}