using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer _sr;
    //[SerializeField] private Ctrl;

    public void OnPointerClick(PointerEventData eventData)
    {
        _sr.color = Color.black;
    }

    public void SetCard()
    {

    }
}
