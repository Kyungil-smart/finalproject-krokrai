using System;
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

    private void OnEnable()
    {
        // TODO : 나중에 여기서 마지막 접근한 게임에 코드 받아오기
        //_currentGameNum = ServiceLocator.Get<IDataManager>().UserDatas
    }

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
    }

    private void CheckPos()
    {
        float y = transform.localPosition.y;
        Log.Message(y);

        if (_aniCoroutine != null)
        {
            StopCoroutine(_aniCoroutine);
            _aniCoroutine = null;
        }
        if (2490 > y && y > 830) // 830 
        {
            Log.Message($"enter restore");
            _aniCoroutine = StartCoroutine(RestoreAni());
        }
        else if (y < 830)
        {
            Log.Message($"up");
            _aniCoroutine = StartCoroutine(SnapAni(true));
        }
        else
        {
            Log.Message($"down");
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

        yield return new WaitForSeconds(0.5f);

        Log.Message("백터 조정 완료");
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

        Log.Message("백터 조정 완료");
        transform.localPosition = new Vector3(0,1660,0);

        // play 버튼 활성화
    }
}
