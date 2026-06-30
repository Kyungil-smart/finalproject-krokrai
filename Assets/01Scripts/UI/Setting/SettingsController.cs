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

public class SettingsController : MonoBehaviour
{
    [SerializeField] Slider _bgmSlider;
    [SerializeField] Slider _sfxSlider;
    [SerializeField] Toggle _bgmMuteToggle;
    [SerializeField] Toggle _sfxMuteToggle;

    private void Start()
    {
        _bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        _bgmMuteToggle.onValueChanged.AddListener(SetBGMMute);
        _sfxMuteToggle.onValueChanged.AddListener(SetSFXMute);
    }

    private void OnDestroy()
    {
        _bgmSlider.onValueChanged?.RemoveAllListeners();
        _sfxSlider.onValueChanged?.RemoveAllListeners();
        _bgmMuteToggle.onValueChanged?.RemoveAllListeners();
        _sfxMuteToggle.onValueChanged?.RemoveAllListeners();
    }

    public void SetSFXVolume(float value) => ServiceLocator.Get<IAudioManager>().SetSFXVolume(value);
    public void SetBGMVolume(float value) => ServiceLocator.Get<IAudioManager>().SetBGMVolume(value);

    public void SetSFXMute(bool state) => ServiceLocator.Get<IAudioManager>().MuteSFX(!state);
    public void SetBGMMute(bool state) => ServiceLocator.Get<IAudioManager>().MuteBGM(!state);
}
