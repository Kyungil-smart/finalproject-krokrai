using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public void SetSFXVolume(float value) => ServiceLocator.Get<AudioManager>().SetSFXVolume(value);
    public void SetBGMVolume(float value) => ServiceLocator.Get<AudioManager>().SetBGMVolume(value);

    public void SetSFXMute() => ServiceLocator.Get<AudioManager>().MuteSFX();
    public void SetBGMMute() => ServiceLocator.Get<AudioManager>().MuteBGM();
}
