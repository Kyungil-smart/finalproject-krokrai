using System.Collections;
using UnityEngine;

public class ReelsNotReadyGame : MonoBehaviour
{
    [SerializeField] private GameObject _obj;
    [SerializeField] private float _fadeTime;

    private bool _isActive;

    public void SetPopUp()
    {
        if (_isActive) return;
        StartCoroutine(PopUp());
    }

    IEnumerator PopUp()
    {
        _isActive = true;
        _obj.SetActive(true);
        yield return new WaitForSeconds(_fadeTime);
        _obj.SetActive(false);
        _isActive = false;
    }
}
