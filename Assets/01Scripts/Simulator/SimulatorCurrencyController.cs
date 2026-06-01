/*
작성자 : 이종현
작성일 : 26-05-26
수정일 : 26-05-26

역할 : 시뮬레이터 재화 데이터 및 UI 관리
방식 : 버튼 입력을 통해 로컬 재화 데이터를 증가시키고 UI를 갱신

설명 :
현재는 로컬 데이터 기반으로만 동작하며,
추후 DataManager 및 Firebase 연동 예정
*/

using TMPro;
using UnityEngine;

public class SimulatorCurrencyController : MonoBehaviour
{
    [Header("총 재화 UI")]
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text gemText;
    [SerializeField] private TMP_Text stoneText;
    [SerializeField] private TMP_Text furDollText;
    [SerializeField] private TMP_Text clawText;
    [SerializeField] private TMP_Text followerText;
    
    // TODO:
    // 현재는 로컬 데이터만 관리
    // 추후 DataManager, Firebase 연동 시
    // 저장 및 로드 로직 연결 필요
    
    private int energy;
    private int coin;
    private int gem;
    private int stone;
    private int furDoll;
    private int claw;
    private int follower;

    /// <summary>
    /// 재화 추가 함수
    /// </summary>
    public void AddCurrency(CurrencyType type, int amount)
    {
        switch (type)
        {
            case CurrencyType.Energy:
                energy += amount;
                break;

            case CurrencyType.Coin:
                coin += amount;
                break;

            case CurrencyType.Gem:
                gem += amount;
                break;

            case CurrencyType.Stone:
                stone += amount;
                break;

            case CurrencyType.FurDoll:
                furDoll += amount;
                break;

            case CurrencyType.Claw:
                claw += amount;
                break;

            case CurrencyType.Follower:
                follower += amount;
                break;
        }
        
        RefreshUI();
    }

    /// <summary>
    /// 현재 재화 UI 갱신
    /// </summary>
    private void RefreshUI()
    {
        energyText.text = $"{energy:N0}";
        coinText.text = $"{coin:N0}";
        gemText.text = $"{gem:N0}";
        stoneText.text = $"{stone:N0}";
        furDollText.text = $"{furDoll:N0}";
        clawText.text = $"{claw:N0}";
        followerText.text = $"{follower:N0}";
    }
}