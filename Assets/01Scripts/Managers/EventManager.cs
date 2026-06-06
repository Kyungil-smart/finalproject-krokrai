using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour, IEventManager
{
    public event Action<int> OnDayClicked;
    
    public void ClickDay(int day)
    {
        OnDayClicked?.Invoke(day);
    }
    
    public void Register() => ServiceLocator.Register<IEventManager>(this);

    public void UnRegister() => ServiceLocator.UnRegister<IEventManager>(this);
}
