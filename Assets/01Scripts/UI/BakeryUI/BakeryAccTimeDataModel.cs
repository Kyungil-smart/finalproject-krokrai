/*
작성자 : NekioEmilia
수정자 : 

작성일 : 26-06-18
수정일 : 

역할 : 
방식 : 
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
        
        Debug.Log($"<color=green><b>[BakeryAccTimeDataModel] SO 데이터 {_bakeryAccTimes.Count}개 로드 및 정렬 완료!</b></color>");
    }

    public Bakery_accTime_TableSO[] GetAllTableData()
    {
        return _bakeryAccTimes.ToArray();
    }
}
