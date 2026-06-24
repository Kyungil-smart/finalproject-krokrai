/*
 작성자 : krokrai
 작성일 : 26-06-18

 역할 : Reels 관리자
 방식 : 무한 스크롤을 위한 논리 계산 및 Game에 접근 상태 제어
 */
using System;
using System.Collections.Generic;
using UnityEngine;

public class ReelsController : MonoBehaviour
{
    [SerializeField] private AutoSOGen_ContaineSO _gameTable;
    [SerializeField] private ReelsView _view;
    [SerializeField] private ReelsDrag _drag;

    [SerializeField] private CardGameUIController _cardGame;

    [SerializeField] private ReelsNotReadyGame _notReadyGame;

    [SerializeField] private GameObject _networkErrorPopup;
    [SerializeField] private GameObject _adPopups;

    private List<Minigame_ID_ListSO> _imgs = new List<Minigame_ID_ListSO>(4);

    private int _currentGameIndex;

    private void Awake()
    {
        var data = ServiceLocator.Get<IDataManager>();
        if (data.Attendance.Join_TimeStamp.Day == DateTime.Now.Day || data.Attendance.Last_Login_TimeStamp.Day != DateTime.Now.Day)
        {
            data.UserDatas.Minigame.Daily_Play_Count = 0;
            data.UserGoods.Claw_ += 3;
        }

        //TODO : DB에 적용된 사항 추가
        _currentGameIndex = data.UserDatas.Minigame.ID_Play_Last;
        _currentGameIndex = 0;

        for (int i = 0; i < _gameTable.scriptableObjects.Length; i++)
        {
            if (_gameTable.scriptableObjects[i] is Minigame_ID_ListSO)
            {
                var t = _gameTable.scriptableObjects[i] as Minigame_ID_ListSO;

                _imgs.Add(t);
            }
        }
    }

    private void OnEnable()
    {
        _currentGameIndex = 0;//ServiceLocator.Get<IDataManager>().UserData

        int maxGameIndex = _imgs.Count - 1;

        int nextIndex = maxGameIndex < _currentGameIndex + 1 ? 0 : _currentGameIndex + 1;
        int previousIndex = _currentGameIndex - 1 < 0 ? maxGameIndex : _currentGameIndex - 1;

        SetView(nextIndex, _currentGameIndex, previousIndex);
    }

    public void ChangeGame(bool isUp)
    {
        int gameCount = _gameTable.scriptableObjects.Length - 1;

        if (isUp)
        {
            _currentGameIndex = gameCount < _currentGameIndex + 1  ? 0 : _currentGameIndex + 1;
        }
        else
        {
            _currentGameIndex = _currentGameIndex - 1 < 0 ? gameCount : _currentGameIndex - 1;
        }

        int nextIndex;
        int previousIndex;

        if (_imgs.Count <= _currentGameIndex + 1)
            nextIndex = 0;
        else
            nextIndex = _currentGameIndex + 1;
        if (_currentGameIndex - 1 < 0)
            previousIndex = _imgs.Count - 1;
        else
            previousIndex = _currentGameIndex - 1;

        SetView(nextIndex, _currentGameIndex, previousIndex);
    }

    private void SetView(int nextIndex, int currentIndex, int previousIndex)
    {
        if (nextIndex < 0 || currentIndex < 0 || previousIndex < 0)
        {
            Log.Message($"숫자가 0보다 작을 수 없습니다. {nextIndex} {currentIndex} {previousIndex}");
            return;
        }
        _view.SetImgs(new int[] {nextIndex, currentIndex, previousIndex});
    }

    public void OnPlayButtonClick()
    {
        if (!CheckCanPlay()) return;
        var data = ServiceLocator.Get<IDataManager>();

        // TODO : 임시코드 제거 필요
        if (_imgs[_currentGameIndex].Game_ID == 101)
        {
            data.UserDatas.Minigame.Daily_Play_Count++;
            data.UserGoods.Claw_--;
            data.UserDatas.Minigame.ID_Play_Last = _currentGameIndex;
            _cardGame.GameStart();
        }
        else
        {
            _notReadyGame.SetPopUp(objectType.NOTREADY);
        }
        // 게임 화면 출력
    }

    public bool CheckCanPlay()
    {
        //_currentGameIndex 기반으로 게임 시작 불러오기
        var data = ServiceLocator.Get<IDataManager>();
        if (9 < data.UserDatas.Minigame.Daily_Play_Count)
        {
            _notReadyGame.SetPopUp(objectType.OVERPLAY);
            return false; // Play 횟수
        }

        if (data.UserGoods.Claw_ < 1)
        {
            // 광고 팝업 팝업 띄우기
            if (Application.internetReachability == NetworkReachability.NotReachable)// || ADMob server에 ping)
            {
                _networkErrorPopup.SetActive(true);
                return false;
            }
            _adPopups.SetActive(true);
            return false;
        }
        return true;
    }

    public async void SeeAd()
    {
        if (true)//await ServiceLocator.Get<IADMobManager>().AutomatedAd())
        {
            _adPopups.SetActive(false);
            OnPlayButtonClick();
        }
    }
}