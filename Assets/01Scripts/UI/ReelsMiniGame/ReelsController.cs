using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelsController : MonoBehaviour
{
    [SerializeField] private AutoSOGen_ContaineSO _gameTable;
    [SerializeField] private ReelsView _view;
    [SerializeField] private ReelsDrag _drag;

    [SerializeField] private GameObject _whiteBG;
    private List<Minigame_ID_ListSO> _imgs = new List<Minigame_ID_ListSO>(4);

    private int _currentGameIndex;

    private void Awake()
    {
        //TODO : DB에 적용된 사항 추가
        //_currentGameIndex = ServiceLocator.Get<IDataManager>().UserDatas.;
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

        int maxGameIndex = _gameTable.scriptableObjects.Length - 1;

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
        _view.SetImgs(new string[] { _imgs[nextIndex].Image_ID, _imgs[currentIndex].Image_ID, _imgs[previousIndex].Image_ID });
    }

    public void OnPlayButtonClick()
    {
        //_currentGameIndex 기반으로 게임 시작 불러오기
        var data = ServiceLocator.Get<IDataManager>();
        //if (10 < data.UserDatas. ) return // Play 횟수

        if (data.UserGoods.Claw_ < 1)
        {
            // 광고 팝업 팝업 띄우기
            return;
        }

        _whiteBG.SetActive(false);

        // 게임 화면 출력
    }
}