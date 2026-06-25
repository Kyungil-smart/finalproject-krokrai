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

    private void OnEnable()
    {
        ServiceLocator.Get<IDataManager>().OnUserDataReseted += RefreshUI;
        RefreshUI();
    }

    private void OnDisable()
    {
        ServiceLocator.Get<IDataManager>().OnUserDataReseted -= RefreshUI;
    }

    public void ResetButton()
    {
        ServiceLocator.Get<IDataManager>().ResetUserData();
    }

    /// <summary>
    /// 현재 재화 UI 갱신
    /// </summary>
    public void RefreshUI()
    {
        IDataManager dataManager = ServiceLocator.Get<IDataManager>();

        if (dataManager == null)
        {
            Log.Message("IDataManager가 등록되지 않았습니다.");
            return;
        }

        if (dataManager.UserGoods == null)
        {
            Log.Message("UserGoods 데이터가 없습니다.");
            return;
        }

        if (dataManager.ProFile == null)
        {
            Log.Message("ProFile 데이터가 없습니다.");
            return;
        }

        UserGoods goods = dataManager.UserGoods;
        ProFile followers = dataManager.ProFile;

        energyText.text = $"{goods.Energy_:N0}";
        coinText.text = $"{goods.Coin_:N0}";
        gemText.text = $"{goods.Gem_:N0}";
        stoneText.text = $"{goods.Stone_:N0}";
        furDollText.text = $"{goods.FurDoll_:N0}";
        clawText.text = $"{goods.Claw_:N0}";
        followerText.text = $"{followers.followerCount:N0}";
    }
}