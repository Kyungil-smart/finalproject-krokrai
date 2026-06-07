

using System;

public interface IEventManager
{
    public event Action<int> OnDayClicked;
    public event Action<int> OnGaugeIncrease;

    public void ClickDay(int day);
    
    public void GaugeIncrease(int amount);
}
