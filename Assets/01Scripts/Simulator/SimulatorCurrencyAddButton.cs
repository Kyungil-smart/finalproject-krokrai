/*
작성자 : 이종현
수정자 : NekioEmilia
작성일 : 26-05-26
수정일 : 26-06-29

역할 : 드롭다운 값을 읽어 재화 증가 요청
방식 : Dropdown 선택값을 숫자로 변환 후 Controller에 전달

설명 :
k / m 단위 문자열도 숫자로 변환 가능
ex) 1k -> 1000
ex) 1m -> 1000000
*/

using System;
using TMPro;
using UnityEngine;

public class SimulatorCurrencyAddButton : MonoBehaviour
{
    [SerializeField] private SimulatorCurrencyController controller;
    [SerializeField] private CurrencyType currencyType;
    [SerializeField] private TMP_Dropdown amountDropdown;

    /// <summary>
    /// 버튼 클릭 시 재화 증가
    /// </summary>
    public void OnClickAdd()
    {
        int amount = GetDropdownAmount();

        
        // 버튼이 눌렸을 때 실제로 Firebase에 반영
        UserGoods goods = ServiceLocator.Get<IDataManager>().UserGoods;
        ProFile followers = ServiceLocator.Get<IDataManager>().ProFile;

        switch (currencyType)
        {
            case CurrencyType.Energy:
                goods.Energy_ += amount;
                break;
            case CurrencyType.Coin:
                goods.Coin_ += amount;
                break;
            case CurrencyType.Gem:
                goods.Gem_ += amount;
                break;
            case CurrencyType.Stone:
                goods.Stone_ += amount;
                break;
            case CurrencyType.FurDoll:
                goods.FurDoll_ += amount;
                break;
            case CurrencyType.Claw:
                goods.Claw_ += amount;
                break;
            case CurrencyType.Follower:
                followers.followerCount += amount;
                break;
        }
        ServiceLocator.Get<IEventManager>().CurrencyAdded(currencyType, amount);
        
        controller.RefreshUI();
    }

    /// <summary>
    /// Dropdown 값 숫자 변환
    /// </summary>
    private int GetDropdownAmount()
    {
        string text = amountDropdown.options[amountDropdown.value].text
            .ToLower()
            .Replace(",", "")
            .Trim();

        if (text.EndsWith("k"))
        {
            text = text.Replace("k", "");
            return int.Parse(text) * 1000;
        }

        if (text.EndsWith("m"))
        {
            text = text.Replace("m", "");
            return int.Parse(text) * 1000000;
        }

        return int.Parse(text);
    }

}