/*
 작성자 : krokrai
 작성일 : 26-06-18

 역할 : 구글 광고 접근을 위한 관리자 interface
 */
using System.Threading.Tasks;

public interface  IADMobManager
{
    public bool CanShowAd();
    public Task<bool> AutomatedAd();
    public void LoadAd();
    public void ShowAd();
    public void DestoryAd();
}
