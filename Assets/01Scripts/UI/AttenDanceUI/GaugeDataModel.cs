/*
작성자 : NekioEmilia
수정자 : 

작성일 : 26-06-08
수정일 : 

역할 : 누적 보상(Festa게이지) SO 데이터를 캐싱하고 반환하는 스크립트
방식 : FestaPresenter에서 게이지 단계를 넘겨주면, 해당 단계의 목표 점수 및 보상 그룹 ID가 담긴 SO를 반환
*/

using System.Collections.Generic;
using UnityEngine;

public class GaugeDataModel : MonoBehaviour
{
    [SerializeField] private AutoSOGen_ContaineSO gaugeGroupSO;

    private Dictionary<int, Mission_Gauge_SettingSO> _gaugeData = new();

    private void Awake()
    {
        for (int i = 0; i < gaugeGroupSO.scriptableObjects.Length; i++)
        {
            if (gaugeGroupSO.scriptableObjects[i] is Mission_Gauge_SettingSO g)
            {
                if (!_gaugeData.ContainsKey(g.Gauge_Step))
                {
                    _gaugeData.Add(g.Gauge_Step, g);
                }
            }
        }
    }

    /// <summary>
    /// Festa 게이지에 따른 보상의 SO 데이터를 반환
    /// </summary>
    /// <param name="gaugeRewardId">검색할 게이지의 고유 ID (ex: 5201)</param>
    /// <returns>해당 Id와 일치하는 게이지 SO데이터, 존재하지 않으면 null</returns>
    public Mission_Gauge_SettingSO GetGaugeSetting(int gaugeRewardId)
    {
        if (_gaugeData.ContainsKey(gaugeRewardId))
        {
            return _gaugeData[gaugeRewardId];
        }

        return null;
    }
}