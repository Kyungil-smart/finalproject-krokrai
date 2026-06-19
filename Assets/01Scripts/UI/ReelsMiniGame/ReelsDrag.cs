/*
 작성자 : krokrai
 작성일 : 26-06-18

 역할 : 드래그하여 게임을 전환하기 위한 제어자
 방식 : IPointer를 사용해 EventSystem 기반으로 제어 및 마지막에 놓인 위치에 따라 위 또는 아래를 전달 및 위치 변경
 */
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReelsDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] private float _lerpSpeed;
    [SerializeField] private float _snapSpeed;
    [SerializeField] private ReelsController _reelsCtrl;

    Coroutine _aniCoroutine;

    float _moveYPos;
    bool _isFirst;

    public void OnPointerDown(PointerEventData eventData)
    {
        _moveYPos = eventData.pressPosition.y;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isFirst = false;
        CheckPos();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        // TODO  : 여기서 Play button 비활성화
        if(!_isFirst)
        {
            _isFirst = true;
        }
        else
        {
            transform.localPosition += new Vector3(0, eventData.position.y - _moveYPos, 0);
        }
        _moveYPos = eventData.position.y;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isFirst = false;
        CheckPos();
    }

    private void CheckPos()
    {
        float y = transform.localPosition.y;

        if (_aniCoroutine != null)
        {
            StopCoroutine(_aniCoroutine);
            _aniCoroutine = null;
        }
        if (2490 > y && y > 830) // 830 
        {
            _aniCoroutine = StartCoroutine(RestoreAni());
        }
        else if (y < 830)
        {
            _aniCoroutine = StartCoroutine(SnapAni(true));
        }
        else
        {
            _aniCoroutine = StartCoroutine(SnapAni(false));
        }
    }

    IEnumerator SnapAni(bool isUp)
    {
        if (isUp)
        {
            while (5 < transform.localPosition.y)
            {
                transform.localPosition -= new Vector3(0, _snapSpeed, 0);
                yield return null;
            }
        }
        else
        {
            while (transform.localPosition.y < 3315)
            {
                transform.localPosition += new Vector3(0, _snapSpeed, 0);
                yield return null;
            }
        }

        //yield return new WaitForSeconds(0.5f);

        transform.localPosition = new Vector3(0, 1660, 0);
        
        // 객체 위치 재조정 및 비활성화
        _reelsCtrl.ChangeGame(isUp);
    }

    IEnumerator RestoreAni()
    {
        while (1640 < transform.localPosition.y && transform.localPosition.y < 1680)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0,1660,0), _lerpSpeed);
            yield return null;
        }

        transform.localPosition = new Vector3(0,1660,0);
    }
}
