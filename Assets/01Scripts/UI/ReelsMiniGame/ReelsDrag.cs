using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReelsDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerMoveHandler
{
    float _moveYPos;
    bool _isFirst;

    public void OnPointerDown(PointerEventData eventData)
    {
        Log.Message(eventData.pressPosition);
        _moveYPos = eventData.pressPosition.y;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Log.Message(eventData.pressPosition);
        _isFirst = false;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if(!_isFirst)
        {
            _isFirst = true;
        }
        transform.localPosition = new Vector3(0, eventData.position.y - _moveYPos,0);
        _moveYPos = eventData.position.y;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Log.Message(eventData.pressPosition);
    }
}
