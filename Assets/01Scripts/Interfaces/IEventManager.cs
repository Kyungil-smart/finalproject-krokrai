/*
작성자 : NekioEmilia
수정자 : 
 
작성일 : 26-06-08
수정일 : 

역할 : 전역적으로 사용할 Action을 모아두는 인터페이스
방식 : EventManage에서 상속받아 ServiceLocator를 통해 Action을 실행
*/

using System;

public interface IEventManager
{
    public event Action<int> OnDayClicked;
    public event Action<int> OnGaugeIncrease;
    public event Action<CurrencyType, int> OnCurrencyAdded;

    public void ClickDay(int day);
    
    public void GaugeIncrease(int amount);

    public void CurrencyAdded(CurrencyType type, int amount);
}
