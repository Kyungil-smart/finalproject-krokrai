/*
작성자 : NekioEmilia
수정자 :

작성일 : 26-06-18
수정일 : 26-06-19

역할 : 베이커리 AccTime SO 데이터를 캐싱하고 반환하는 Model 스크립트
방식 : Presenter에서 데이터를 요청하면 즉시 반환함
*/

using System;
using UnityEngine;
using System.Collections.Generic;

public class BakeryAccTimeDataModel : MonoBehaviour
{
    [SerializeField] private AutoSOGen_ContaineSO _bakeryAccTimeSO;

    private List<Bakery_accTime_TableSO> _bakeryAccTimes = new();

    private void Awake()
    {
        if (_bakeryAccTimeSO == null || _bakeryAccTimeSO.scriptableObjects == null)
        {
            return;
        }

        for (int i = 0; i < _bakeryAccTimeSO.scriptableObjects.Length; i++)
        {
            if (_bakeryAccTimeSO.scriptableObjects[i] is Bakery_accTime_TableSO a)
            {
                _bakeryAccTimes.Add(a);
            }
        }
    }
    
    /// <summary>
    /// 캐싱해둔 베이커리 accTime 테이블 데이터를 배열 형태로 반환
    /// </summary>
    /// <returns>스트링 키 값, 코인 수, 시간이 배열 형태로 반환됨</returns>
    public Bakery_accTime_TableSO[] GetAllTableData()
    {
        return _bakeryAccTimes.ToArray();
    }
}