/*
 작성자 : krokrai
 수정자 : 
 작성일 : 26-06-29
 수정일 : 

 역할 : 음량 조절 및 음소거 설정용 제어자
 방식 : Slider 및 CheckBox에서 받아온 정보를 기반으로 IAudioManager에 전달.
*/
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] Slider _bgmSlider;
    [SerializeField] Slider _sfxSlider;

    private void Start()
    {
        _bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        ServiceLocator.Get<IAudioManager>().SetSFXVolume(0.5f);
        ServiceLocator.Get<IAudioManager>().SetBGMVolume(0.5f);
    }

    private void OnDestroy()
    {
        _bgmSlider.onValueChanged?.RemoveAllListeners();
        _sfxSlider.onValueChanged?.RemoveAllListeners();
    }

    public void SetSFXVolume(float value) => ServiceLocator.Get<IAudioManager>().SetSFXVolume(value);
    public void SetBGMVolume(float value) => ServiceLocator.Get<IAudioManager>().SetBGMVolume(value);

    public void SetSFXMute() => ServiceLocator.Get<IAudioManager>().MuteSFX();
    public void SetBGMMute() => ServiceLocator.Get<IAudioManager>().MuteBGM();
}
