/*
 작성자 : krokrai
 수정자 : 
 작성일 : 26-05-25
 수정일 : 26-06-29

 역할 : Service locator에 등록하기 위한 audio manager interface
 */

using UnityEngine;

public interface IAudioManager
{
    public void MuteSFX(bool state);
    public void MuteBGM(bool state);
    public void PlaySFX(SFXAudiosEnum sfx);
    public void PlayBGM(BGMAudiosEnum sfx);
    public void PlaySFX(string clipName);
    public void PlayBGM(string clipName);
    public void SetSFXVolume(float volume);
    public void SetBGMVolume(float volume);
}
