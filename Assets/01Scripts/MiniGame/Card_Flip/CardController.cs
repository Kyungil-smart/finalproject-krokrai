using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer _sr;
    //[SerializeField] private Ctrl;

    private CardGameController _root;

    private bool _isFlip = false;

    private int _index = -1;
    private int _cardNum = -1;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isFlip) return;
        _root.ClickedCard(_index,_cardNum);
        FlipCard();
    }

    public void SetCard(int index, int cardNum, CardGameController cgc)
    {
        _index = index;
        _root = cgc;
    }

    private void FlipCard()
    {
        if(_isFlip)
        {

        }
        else // 기본 상태
        {
            // 카드 뒤집기

            //
        }
    }
}
