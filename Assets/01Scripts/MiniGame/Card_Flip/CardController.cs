using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Sprite _defaultCard;

    private CardGameController _root;

    private bool _isFlip = false;

    private int _index = -1;
    private int _cardNum = -1;

    private Sprite _cardImg;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isFlip) return;
        _root.ClickedCard(_index,_cardNum);
        //Log.Message();
        FlipCard();
    }

    public void SetCard(int index, int cardNum, Sprite cardImg,CardGameController cgc)
    {
        _index = index;
        _cardNum = cardNum;
        _root = cgc;
        _cardImg = cardImg;
    }

    public void FlipCard()
    {
        if (_isFlip)
        {
            _sr.sprite = _defaultCard;
            _isFlip = false;
        }
        else // 기본 상태
        {
            // 카드 뒤집기
            _sr.sprite = _cardImg;

            _isFlip = true;
        }
    }

    public void ResetFlip()
    {
        _sr.sprite = _defaultCard;
        _isFlip = false;
    }
}
