/*
 작성자 : krokrai
 작성일 : 26-06-18

 역할 : 미완성 게임 접근 제한
 방식 : 접근 요청 시 해당 script로 넘겨주어 Popup UI 생성
 */
using System.Collections;
using TMPro;
using UnityEngine;

public enum objectType : byte
{
    NOTREADY, OVERPLAY,
}

public class ReelsNotReadyGame : MonoBehaviour
{
    [SerializeField] private GameObject _obj;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _fadeTime;

    private const string _notReady = "아직 준비되지 않은 게임입니다.";
    private const string _overPlay = "일일 미니게임 입장 횟수를 다 사용하셨습니다.";

    private bool _isActive;

    public void SetPopUp(objectType t)
    {
        if (_isActive) return;
        StartCoroutine(PopUp(t));
    }

    IEnumerator PopUp(objectType t)
    {
        _isActive = true;
        _obj.SetActive(true);
        _text.text = t switch
        {
            objectType.NOTREADY => _notReady,
            objectType.OVERPLAY => _overPlay,
            _ => throw new System.ArgumentException("범위를 초과했습니다.")
        };
        yield return new WaitForSeconds(_fadeTime);
        _obj.SetActive(false);
        _isActive = false;
    }
}
