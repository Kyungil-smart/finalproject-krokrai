using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventManager
{
    // private은 아니고.. protected? public? 흠
    public event Action<int> OnDayClicked;

    void ClickDay(int day);
}
