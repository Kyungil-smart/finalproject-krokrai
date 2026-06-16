/*
작성자 : NekioEmilia
수정자 : NekioEmilia
 
작성일 : 26-06-08
수정일 : 26-06-15

역할 : 이벤트 관련 전역 이벤트를 관리하는 매니저
방식 : ServiceLocator를 통해 전역에서 접근 가능하고, 특장 상황 발생 시 Action을 통해서 Presenter에게 발송함
*/

using System;
using UnityEngine;

public class EventManager : MonoBehaviour, IEventManager, IManagerBooter
{
    
    public event Action<int> OnDayClicked; // Day 탭이 클릭되었을 때 발생하는 이벤트
    public event Action<int> OnGaugeIncrease; // Festa 게이지 점수를 획득했을 때 발생하는 이벤트
    public event Action<CurrencyType, int> OnCurrencyAdded;

    /// <summary>
    /// 특정 Day 탭을 클릭했을 때 호출되어 OnDayClicked를 Invoke 시킴
    /// </summary>
    /// <param name="day">선택된 일자</param>
    public void ClickDay(int day)
    {
        OnDayClicked?.Invoke(day);
    }

    /// <summary>
    /// 미션 보상 등을 통해 페스타 게이지 포인트를 얻었을 때 호출되어 OnGaugeIncrease를 Invoke 시킴
    /// </summary>
    /// <param name="amount">증가할 페스타 게이지 점수</param>
    public void GaugeIncrease(int amount)
    {
        OnGaugeIncrease?.Invoke(amount);
    }

    /// <summary>
    /// 자원이 추가되면 OnCurrencyAdded를 Invoke 시킴
    /// </summary>
    /// <param name="type">추가된 아이템 타입</param>
    /// <param name="amount">추가된 아이템 수량</param>
    public void CurrencyAdded(CurrencyType type, int amount)
    {
        OnCurrencyAdded?.Invoke(type, amount);
    }

    public void Register() => ServiceLocator.Register<IEventManager>(this);

    public void UnRegister() => ServiceLocator.UnRegister<IEventManager>(this);
}
