/*
 작성자 : krokrai
 작성일 : 26-06-10

 역할 : upload 에서 냥스톤 부족 시 뜰 팝업
 방식 : SetPopUp 함수 호출 시 _baseWaitTime 초 만큼 대기 후 파괴됌
 */
using System.Collections;
using UnityEngine;

public class Uploadpopup : MonoBehaviour
{
    [SerializeField] float _baseWaitTime;
    [SerializeField] GameObject _popUp;

    private Coroutine _cr;

    public void SetPopUp()
    {
        if (_cr != null)
            return;
        _cr = StartCoroutine(PopUp());
    }

    IEnumerator PopUp()
    {
        _popUp.SetActive(true);
        yield return new WaitForSeconds(_baseWaitTime);
        _popUp.SetActive(false);
        _cr = null;
    }
}
