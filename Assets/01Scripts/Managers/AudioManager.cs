/*
 작성자 : krokrai
 수정자 : 
 작성일 : 26-05-25
 수정일 : 26-05-25

 역할 : 오디오를 한 곳에서 관리하기 위한 Manager
 방식 : 추후 기획에 따라 추가 작성할 예정.
 */

using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IAudioManager, IManagerBooter
{
    // 버튼 소리 
    [SerializeField] private AudioSource _sfx;
    [SerializeField] private AudioSource _bgm;
    [SerializeField] private SoundsSO _audios;

    private Dictionary<string, AudioClip> _clips = new Dictionary<string, AudioClip>(8);
    private AudioClip _currentClip;

    private void Awake()
    {
        _sfx.mute = false;
        _bgm.mute = false;
        _bgm.loop = true;

        _sfx.volume = 0.5f;
        _bgm.volume = 0.5f;

        for (int i = 0; i < _audios.sfx.Length; i++)
        {
            if (_clips.TryAdd(_audios.sfx[i].name, _audios.sfx[i]))
                continue;
            else
            {
                Log.Message($"Dictionary에 추가 실패 : {_audios.sfx[i]} / {i} 해당 clip이 중복 등록인지 확인해주세요.");
            }
        }

        for (int i = 0; i < _audios.bgm.Length; i++)
        {
            if (_clips.TryAdd(_audios.bgm[i].name, _audios.bgm[i]))
                continue;
            else
            {
                Log.Message($"Dictionary에 추가 실패 : {_audios.bgm[i]} / {i} 해당 clip이 중복 등록인지 확인해주세요.");
            }
        }

        Log.Message("AudioManager 작동 준비 완료");
    }

    public void MuteSFX(bool state) => _sfx.mute = state;

    public void MuteBGM(bool state) => _bgm.mute = state;

    public void PlaySFX(SFXAudiosEnum sfx)
    {
        PlaySFX(sfx switch
        {
            SFXAudiosEnum.BTN1 => "sfx_btn1",
            SFXAudiosEnum.BTN2 => "sfx_btn2",
            SFXAudiosEnum.CAPTURE => "sfx_purchased",
            SFXAudiosEnum.PURCHASED => "sfx_reward",
            SFXAudiosEnum.REWARD => "sfx_capture",
            _ => ""
        });
    }
    public void PlayBGM(BGMAudiosEnum sfx)
    {
        PlayBGM(sfx switch
        {
            BGMAudiosEnum.MAIN => "bgm_main",
            BGMAudiosEnum.SNS => "bgm_sns",
            BGMAudiosEnum.MINIGAME_CARDFLIPPING => "bgm_minigame_cardflipping",
            _ => ""
        });
    }

    public void PlaySFX(string clipName)
    {
        if (!_clips.TryGetValue(clipName, out _currentClip))
        {
            Log.Message($"clip을 찾지 못했습니다. {clipName}");
            return;
        }
        _sfx.PlayOneShot(_currentClip);
    }

    public void PlayBGM(string clipName)
    {
        if (!_clips.TryGetValue(clipName, out _currentClip))
        {
            Log.Message($"clip을 찾지 못했습니다. {clipName}");
            return;
        }

        _bgm?.Stop();
        _bgm.clip = _currentClip;
        _bgm.Play();
    }

    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        _sfx.volume = volume * volume;
    }

    public void SetBGMVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        _bgm.volume = volume * volume;
    }

    public void Register() => ServiceLocator.Register<IAudioManager>(this);

    public void UnRegister() => ServiceLocator.UnRegister<IAudioManager>(this);
}
