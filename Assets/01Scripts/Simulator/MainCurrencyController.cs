/*
작성자 : 이종현
작성일 : 26-06-05
수정일 : 26-06-05

역할 : 메인 화면 재화 UI 관리
방식 : DataManager의 UserGoods 데이터를 읽어 메인 화면 재화 텍스트에 반영
*/

using TMPro;
using UnityEngine;

public class MainCurrencyController : MonoBehaviour
{
    [Header("메인 화면 재화 UI")]
    [SerializeField] private TMP_Text _energyText;
    [SerializeField] private TMP_Text _coinText;
    [SerializeField] private TMP_Text _gemText;
    [SerializeField] private TMP_Text _stoneText;
    [SerializeField] private TMP_Text _furDollText;
    [SerializeField] private TMP_Text _clawText;

    private void Awake()
    {
        ServiceLocator.Get<IDataAutoSaveManager>().SetMainCurrencyController(this);
    }

    private void OnEnable()
    {
        ServiceLocator.Get<IDataManager>().OnUserDataReseted += RefreshUI;
        RefreshUI();
    }

    private void OnDisable()
    {
        ServiceLocator.Get<IDataManager>().OnUserDataReseted -= RefreshUI;
    }
    
    /// <summary>
    /// 메인 화면 재화 UI 갱신
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

        UserGoods goods = dataManager.UserGoods;

        _energyText.text = $"{goods.Energy_:N0}";
        _coinText.text = $"{goods.Coin_:N0}";
        _gemText.text = $"{goods.Gem_:N0}";
        _stoneText.text = $"{goods.Stone_:N0}";
        _furDollText.text = $"{goods.FurDoll_:N0}";
        _clawText.text = $"{goods.Claw_:N0}";
    }
}