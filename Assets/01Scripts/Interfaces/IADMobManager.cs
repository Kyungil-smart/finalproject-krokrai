using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface  IADMobManager
{
    public bool CanShowAd();
    public Task<bool> AutomatedAd();
    public void LoadAd();
    public void ShowAd();
    public void DestoryAd();
}
