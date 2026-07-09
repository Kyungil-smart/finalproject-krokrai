using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SNSExitButton : MonoBehaviour
{
    [SerializeField] GameObject _snsCanvas;
    public void OnExitButtonClicked()
    {
        ServiceLocator.Get<IAudioManager>().PlayBGM(BGMAudiosEnum.MAIN);
        _snsCanvas.SetActive(false);
    }
}
