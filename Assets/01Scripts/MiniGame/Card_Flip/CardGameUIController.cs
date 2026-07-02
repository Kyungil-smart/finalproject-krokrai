/*
 작성자 : krokrai
 작성일 : 26-06-18

 역할 : 카드 뒤집기 게임에 필요한 UI 제어
 방식 : 화면 구성을 위해 GameObject의 활성화를 변경하여, 화면 구성
 */
using System.Collections;
using TMPro;
using UnityEngine;

public class CardGameUIController : MonoBehaviour
{
    [SerializeField] GameObject _reels;

    [SerializeField] GameObject _cardFlipGame;
    [SerializeField] GameObject _mainUI;
    [SerializeField] GameObject _whiteBG;

    [SerializeField] GameObject _endUI;
    [SerializeField] TextMeshProUGUI _rewardText;
    [SerializeField] GameObject _rePlayGameUI;

    [SerializeField] private ReelsController _reelsCtrl;

    public void GameStart()
    {
        _reels.SetActive(false);
        _endUI.SetActive(false);
        _mainUI.SetActive(false);
        _whiteBG.SetActive(false);
        ServiceLocator.Get<IAudioManager>().PlayBGM(BGMAudiosEnum.MINIGAME_CARDFLIPPING);
        _cardFlipGame.SetActive(true);
        _cardFlipGame.GetComponent<CardGameController>().ResetData();
    }   

    public void GameResult(int hitedCards)
    {
        StartCoroutine(RewardPopUp(hitedCards));
    }

    public void GameEnd() // 리플레이 조건 검사 확인
    {
        _cardFlipGame.SetActive(false);
        _reels.SetActive(true);
        _mainUI.SetActive(true);
        _whiteBG.SetActive(true);
    }

    public void Restart()
    {
        _rePlayGameUI.SetActive(false);
        if (!_reelsCtrl.CheckCanPlay())
        {
            _reels.SetActive(true);
            _cardFlipGame.SetActive(false);
            _endUI.SetActive(false);
            ServiceLocator.Get<IAudioManager>().PlayBGM(BGMAudiosEnum.MINIGAME_CARDFLIPPING);
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

    IEnumerator RewardPopUp(int hitedCards)
    {
        _endUI.SetActive(true);
        _rewardText.text = $"x{hitedCards.ToString()}";
        yield return new WaitForSeconds(1f);
        _endUI.SetActive(false);
        GameEnd();
    }
}
