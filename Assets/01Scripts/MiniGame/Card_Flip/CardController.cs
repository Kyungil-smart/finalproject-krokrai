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
        //Log.Message();
        FlipCard();
    }

    public void SetCard(int index, int cardNum, CardGameController cgc)
    {
        _sr.color = Color.yellow;

        _index = index;
        _cardNum = cardNum;
        _root = cgc;
    }

    public void FlipCard()
    {
        if(_isFlip)
        {
            _sr.color = Color.yellow;

            _isFlip = false;
        }
        else // 기본 상태
        {
            _sr.color = Color.green;
            // 카드 뒤집기

            //
            _isFlip = true;
        }
    }
}
