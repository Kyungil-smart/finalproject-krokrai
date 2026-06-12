using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelsController : MonoBehaviour
{
    [SerializeField] private AutoSOGen_ContaineSO _gameTable;
    [SerializeField] private ReelsView _view;
    [SerializeField] private ReelsDrag _drag;

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

    public void ChangeGame(bool isUp)
    {
        int gameCount = _gameTable.scriptableObjects.Length;

        if (isUp)
        {
            _currentGameIndex = gameCount < _currentGameIndex + 1  ? 0 : _currentGameIndex + 1;
        }
        else
        {
            _currentGameIndex = _currentGameIndex - 1 < 0 ? gameCount : _currentGameIndex - 1;
        }

        //string[] 
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

        Log.Message($"{previousIndex} / {_currentGameIndex} / {nextIndex}");

        _view.SetImgs(new string[] { _imgs[previousIndex].Image_ID, _imgs[_currentGameIndex].Image_ID, _imgs[nextIndex].Image_ID});
    }
}