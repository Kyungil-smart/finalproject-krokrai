/*
 작성자 : krokrai
 작성일 : 26-06-18

 역할 : 미완성 게임 접근 제한
 방식 : 접근 요청 시 해당 script로 넘겨주어 Popup UI 생성
 */
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
