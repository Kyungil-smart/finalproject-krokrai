using System;
using UnityEngine;

[Serializable]
public class UserGoods
{
    [SerializeField] private long Energy;
    [SerializeField] private long Coin;
    [SerializeField] private long Gem;
    [SerializeField] private long Stone;
    [SerializeField] private long FurDoll;
    [SerializeField] private long Claw;

    public long Energy_
    {
        get
        {
            return Energy;
        }
        set
        {
            Energy = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestRTDBSave();
        }
    }
    public long Coin_
    {
        get
        {
            return Coin;
        }
        set
        {
            Coin = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestRTDBSave();
        }
    }
    public long Gem_
    {
        get
        {
            return Gem;
        }
        set
        {
            Gem = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestRTDBSave();
        }
    }
    public long Stone_
    {
        get
        {
            return Stone;
        }
        set
        {
            Stone = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestRTDBSave();
        }
    }

    public long FurDoll_
    {
        get
        {
            return FurDoll;
        }
        set
        {
            FurDoll = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestRTDBSave();
        }
    }
    public long Claw_
    {
        get
        {
            return Claw;
        }
        set
        {
            Claw = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestRTDBSave();
        }
    }

    public UserGoods() { }
}
