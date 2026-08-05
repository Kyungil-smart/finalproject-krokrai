using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButtonSound : MonoBehaviour
{
    private void Start()
    {
        ServiceLocator.Get<IAudioManager>().PlayBGM(BGMAudiosEnum.MAIN);
    }
    public void PlayButtonSFX1()
    {
        ServiceLocator.Get<IAudioManager>().PlaySFX(SFXAudiosEnum.BTN1);
    }
}
