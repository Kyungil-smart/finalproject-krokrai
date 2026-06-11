using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelsController : MonoBehaviour
{
    [SerializeField] private AutoSOGen_ContaineSO _gameTable;
    [SerializeField] private ReelsView _view;
    [SerializeField] private ReelsDrag _drag;

    private int _currentGameIndex;

    private void Awake()
    {
        //TODO : DB에 적용된 사항 추가
        //_currentGameIndex = ServiceLocator.Get<IDataManager>().UserDatas.;
        
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

        for(int i = 0; i < gameCount; i++)
        {
            if (true) // TODO : _gameTable.scriptableObjects[i] is (여기에 Minigame ID List 추가) )
            {
                // TODO : var t = _gameTable.scriptableObjects[i] as () ;

            }
        }

        //_view.SetImgs()
    }
}