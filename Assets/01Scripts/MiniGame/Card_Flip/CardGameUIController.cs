using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardGameUIController : MonoBehaviour
{
    [SerializeField] GameObject _cardFlipGame;
    
    [SerializeField] GameObject _endUI;
    [SerializeField] TextMeshProUGUI _rewardText;

    public void GameStart()
    {
        _endUI.SetActive(false);
        _cardFlipGame.SetActive(true);
    }

    public void GameEnd(int hitedCards)
    {
        _endUI.SetActive(true);
        _rewardText.text = hitedCards.ToString();
        //ServiceLocator.Get<IDataManager>().UserDatas. // TODO : need develop merge Into this
    }

    public void Complite()
    {
        _endUI.SetActive(false);
        _cardFlipGame.SetActive(false);
    }

    public void DeleteGameData()
    {
        _cardFlipGame.GetComponent<CardGameController>().DeleteCards();
    }
}
