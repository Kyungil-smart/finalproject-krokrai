/*
 작성자 : krokrai
 작성일 : 26-06-18

 역할 : 카드 게임 관리자
 방식 : 간격 설정으로 카드 위치 정렬 및 보상 UI 표기
 */

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardGameController : MonoBehaviour
{
    [SerializeField] int _playCount;
    [SerializeField] byte _row; // 4 열
    [SerializeField] byte _col; // 4 행

    [SerializeField] Vector2 _startPos;
    [SerializeField] Vector2 _offset;

    [SerializeField] AutoSOGen_ContaineSO _rewardTable;

    [SerializeField] AutoSOGen_ContaineSO _cardData;
    [SerializeField] GameObject _cardPrefab;

    [SerializeField] TextMeshProUGUI _count;
    [SerializeField] Sprite[] _cardImgs;

    [SerializeField] private PhysicsRaycaster _camRay;

    [SerializeField] private CardGameUIController _uiCtrl;

    CardController[] _cardCtrl;
    Transform[] _cardPoss;

    Vector2[] _presetCardPoss;

    Coroutine _coroutine;

    private bool _isOpenedCard;
    private bool _isFirst = true;

    private int _currentCardNum;
    private int _currentCardIndex;

    private int _hitCard;
    private int _maxOpenCardCount;

    private int[] _rndNums;

    private void Awake()
    {
        _cardCtrl = new CardController[_row * _col];
        _cardPoss = new Transform[_row * _col];
        _rndNums = new int[_row * _col];
        _presetCardPoss = new Vector2[_row * _col];
        _maxOpenCardCount = _playCount; // TODO : 나중에 SO로 교체
        _hitCard = 0;
        _count.text = $"남은 횟수 : {_maxOpenCardCount}회";

        int t = _rndNums.Length / 2;

        MiniGame_CardFlip_CardTable_TableSO so;

        for (int i = 0; i < t; i++)
        {
            if (!(_cardData.scriptableObjects[i] is MiniGame_CardFlip_CardTable_TableSO))
                return;
            so = _cardData.scriptableObjects[i] as MiniGame_CardFlip_CardTable_TableSO;

            _rndNums[i * 2] = so.card_ID;
            _rndNums[(i * 2) + 1] = so.card_ID;
        }

        _rndNums = Shuffle(_rndNums);

        t = 0;

        GameObject obj;

        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _col; j++)
            {
                obj = Instantiate(_cardPrefab, transform);
                _cardPoss[t] = obj.transform;
                _presetCardPoss[t] = new Vector2(_startPos.x + (_offset.x * j), _startPos.y - (_offset.y * i));
                _cardPoss[t].localPosition = _presetCardPoss[t];
                
                obj.name = $"Card_{t}";

                _cardCtrl[t] = obj.GetComponent<CardController>();

                _cardCtrl[t].SetCard(_rndNums[t], t, _cardImgs[_rndNums[t] - 1001], this);


                t++;
            }
        }
    }

    private void OnEnable()
    {
        if (_isFirst)
            return;
        ResetData();
    }

    private void Start()
    {
        _isFirst = false;
    }

    private T[] Shuffle<T>(T[] num) where T : struct
    {
        int c = num.Length;
        T temp;
        int index = 0;
        for (int i = c - 1; 0 < i; i--)
        {
            index = UnityEngine.Random.Range(0, c);

            temp = num[i];
            num[i] = num[index];
            num[index] = temp;
        }
        return num;
    }

    public void ResetData()
    {
        if (_isFirst) return;
        ServiceLocator.Get<IDataManager>().UserDatas.Minigame.Daily_Play_Count++;
        
        int t = 0;

        for (int i = 0; i < _cardCtrl.Length; i++)
        {
            _cardCtrl[i].ResetFlip();
            _presetCardPoss = Shuffle(_presetCardPoss);
        }
        
        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _col; j++)
            {
                _cardPoss[t].localPosition = _presetCardPoss[t];
                t++;
            }
        }

        _currentCardIndex = 0;
        _currentCardNum = 0;
        _isOpenedCard = false;
        _maxOpenCardCount = _playCount;
        _hitCard = 0;
        _count.text = $"남은 횟수 : {_maxOpenCardCount}회";
    }

    public void ClickedCard(int index, int cardNum)
    {
        _camRay.enabled = false;
        if (_isOpenedCard)
        {
            
            if (index == _currentCardIndex)
            {
                Log.Message("맞추기 성공");
                // 카드 뒤집힌 상태 고정
                _hitCard++;
                _camRay.enabled = true;
            }
            else
            {
                Log.Message("맞추기 실패");
                _coroutine = StartCoroutine(Waiter(cardNum));
            }
            _maxOpenCardCount--;
            _count.text = $"남은 횟수 : {_maxOpenCardCount}회";
            _isOpenedCard = false;
            if (_maxOpenCardCount < 1)
                EndGame();// 게임 오버
        }
        else
        {
            _currentCardIndex = index;
            _currentCardNum = cardNum;
            _isOpenedCard = true;
            _camRay.enabled = true;
        }
    }

    private void EndGame()
    {
        StopCoroutine(_coroutine);
        _camRay.enabled = true;
        Log.Message($"게임 종료 / {_hitCard}");

        int reward = -1;

        MiniGame_CardFlip_RewardTableSO so;

        for(int i = 0; i < _rewardTable.scriptableObjects.Length; i++)
        {
            if (_rewardTable.scriptableObjects[i] is MiniGame_CardFlip_RewardTableSO)
            {
                so = _rewardTable.scriptableObjects[i] as MiniGame_CardFlip_RewardTableSO;
                if (_hitCard == so.matchedPairs)
                {
                    reward = so.rewardAmount;
                    break;
                }
            }
        }

        if (reward < 0)
        {
            Log.Message("잘 못된 값이 입력되었습니다.");
            reward = 0;
        }
        ServiceLocator.Get<IDataManager>().UserDatas.Minigame.Token_Owned += reward;
        

        // 점수 판
        _uiCtrl.GameResult(reward);

        // 게임 재시작 여부
        //ResetData();
    }

    public void DeleteCards()
    {
        for(int i = 0; i < _cardCtrl.Length; i++)
        {
            _cardCtrl[i].SelfDestroy();
        }
        _cardCtrl = null;
    }

    IEnumerator Waiter(int num)
    {
        yield return new WaitForSeconds(0.5f);
        _cardCtrl[num].FlipCard(); // 카드 다시 뒤집기
        _cardCtrl[_currentCardNum].FlipCard();
        _camRay.enabled = true;
    }
}
