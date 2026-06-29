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
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour, IAudioManager, IManagerBooter
{
    // 버튼 소리 
    [SerializeField] private AudioSource _sfx;
    [SerializeField] private AudioSource _bgm;
    [SerializeField] private SoundsSO _audios;

    private Dictionary<string, AudioClip> _clips;
    private AudioClip _currentClip;

    private bool _isSFXMute;
    private bool _isBGMMute;

    private void Awake()
    {
        _isSFXMute = false;
        _isBGMMute = false;

        _sfx.mute = false;
        _bgm.mute = false;
        _bgm.loop = true;

        for (int i = 0; i < _audios.sfx.Length; i++)
        {
            if (_clips.TryAdd(_audios.sfx[i].name, _audios.sfx[i]))
                continue;
            else
            {
                Log.Message($"Dictionary에 추가 실패 : {_audios.sfx[i]} / {i} 해당 clip이 중복 등록인지 확인해주세요.");
            }
        }
        Log.Message("AudioManager 작동 준비 완료");
    }

    public void MuteSFX()
    {
        _isSFXMute = !_isSFXMute;
        _sfx.mute = _isSFXMute;
    }

    public void MuteBGM()
    {
        _isBGMMute = !_isBGMMute;
        _bgm.mute = _isBGMMute;
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
        _sfx.volume = ProcessedVolume(volume);
    }

    public void SetBGMVolume(float volume)
    {
        _bgm.volume = ProcessedVolume(volume);
    }

    private float ProcessedVolume(float value) => Mathf.Log10( Mathf.Clamp(value, 0.001f, 1f) ) * 20f + (value * 20f);

    public void Register() => ServiceLocator.Register<IAudioManager>(this);

    public void UnRegister() => ServiceLocator.UnRegister<IAudioManager>(this);
}
