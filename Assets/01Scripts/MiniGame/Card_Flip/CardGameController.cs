using System.Collections.Generic;
using UnityEngine;

public class CardGameController : MonoBehaviour
{
    [SerializeField] byte _row; // 4 열
    [SerializeField] byte _col; // 4 행

    [SerializeField] GameObject _cardPrefab;

    CardController[] _cardCtrl;

    private bool _isOpenedCard;

    private int _currentCardNum;
    private int _currentCardIndex;

    private int _hitCard;

    private int[] _rndNums;

    private void Awake()
    {
        _cardCtrl = new CardController[_row * _col];
        _rndNums = new int[] // new int[_row * _col];
        {
            101,101,102,102
        }; // TODO : Table에서 이미지 번호 받아오기

        Shuffle();

        GameObject obj;

        for (int i = 0; i < _cardCtrl.Length; i++)
        {
            obj = Instantiate(_cardPrefab, transform);
            obj.name = $"Card_{i}";

            _cardCtrl[i] = obj.GetComponent<CardController>();
            _cardCtrl[i].SetCard(_rndNums[i],i,this);
        }
    }

    private void Shuffle()
    {
        int c = _rndNums.Length;
        int temp = 0;
        int index = 0;
        for (int i = c - 1; 0 < i; i--)
        {
            index = Random.Range(0, c);

            temp = _rndNums[i];
            _rndNums[i] = _rndNums[index];
            _rndNums[index] = temp;
        }
    }

    public void ResetData()
    {

    }

    public void ClickedCard(int index, int cardNum)
    {
        if (_isOpenedCard)
        {
            if(index == _currentCardIndex)
            {
                Log.Message("맞추기 성공");
                // 카드 뒤집힌 상태 고정
                _hitCard++;
            }
            else
            {
                Log.Message("맞추기 실패");
                _cardCtrl[cardNum].FlipCard(); // 카드 다시 뒤집기
                _cardCtrl[_currentCardNum].FlipCard();
            }

            _isOpenedCard = false;
        }
        else
        {
            _currentCardIndex = index;
            _currentCardNum = cardNum;
            _isOpenedCard = true;
        }
    }
}
