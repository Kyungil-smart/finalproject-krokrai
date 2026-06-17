using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardGameUIController : MonoBehaviour
{
    [SerializeField] GameObject _reels;

    [SerializeField] GameObject _cardFlipGame;
    
    [SerializeField] GameObject _endUI;
    [SerializeField] TextMeshProUGUI _rewardText;
    [SerializeField] GameObject _rePlayGameUI;

    [SerializeField] private ReelsController _reelsCtrl;

    public void GameStart()
    {
        _reels.SetActive(false);
        _endUI.SetActive(false);
        _cardFlipGame.SetActive(true);
    }

    public void GameResult(int hitedCards)
    {
        _endUI.SetActive(true);
        _rewardText.text = $"x{hitedCards.ToString()}";
        //ServiceLocator.Get<IDataManager>().UserDatas. // TODO : need develop merge Into this
    }

    public void GameEnd() // 리플레이 조건 검사 확인
    {
        _cardFlipGame.SetActive(false);
        _reels.SetActive(true);
    }

    public void Restart()
    {
        _rePlayGameUI.SetActive(false);
        if (!_reelsCtrl.CheckCanPlay())
        {
            _reels.SetActive(true);
            return;
        }
        _cardFlipGame.GetComponent<CardGameController>().ResetData();
    }

    public void Complite()
    {
        _endUI.SetActive(false);
        _rePlayGameUI.SetActive(true);
    }

    public void DeleteGameData()
    {
        _cardFlipGame.GetComponent<CardGameController>().DeleteCards();
    }
}
