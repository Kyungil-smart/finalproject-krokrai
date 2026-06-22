/*
 작성자 : krokrai
 작성일 : 26-06-18

 역할 : 구글 광고 접근을 위한 관리자
 방식 : AdMob sdk를 받아서 형식에 맞춰 접근, AutoMated 함수로 자동 생성 및 광고 팝업
 */
using GoogleMobileAds.Api;
using System.Threading.Tasks;
using UnityEngine;

public class ADMobManager : MonoBehaviour, IADMobManager, IManagerBooter
{
    // 실제 수익을 발생하지 않을 예정이므로 공개되어 있는 테스트 코드 기입
    private const string AD_UNIT_ID = "ca-app-pub-3940256099942544/5224354917";

    TaskCompletionSource<bool> _canAdReward;
    bool _hasReward;

    RewardedAd _rewardedAd;

    void Start()
    {
        if (_rewardedAd != null)
            DestoryAd();
        MobileAds.Initialize((InitializationStatus initstatus) =>
        {
            if (initstatus == null)
            {
                Log.Message("Google Mobile Ads 초기화 실패.");
                return;
            }

            Log.Message("Google Mobile Ads 초기화 완료");
        });
    }

    /// <summary>
    /// 현재 광고가 시청 가능하게 준비되어 있는 지 확인용
    /// </summary>
    /// <returns></returns>
    public bool CanShowAd() => _rewardedAd != null && _rewardedAd.CanShowAd();

    /// <summary>
    /// 자동으로 광고 준비 및 시청 및 비동기 방식으로 시청 완료 후 상태 전환 가능
    /// </summary>
    /// <returns></returns>
    public async Task<bool> AutomatedAd()
    {
        if (_rewardedAd == null)
            LoadAd();
        else if (!_rewardedAd.CanShowAd())
        {
            DestoryAd();
            LoadAd();
        }

        _canAdReward?.TrySetCanceled();
        _canAdReward = new TaskCompletionSource<bool>(false);

        ShowAd();
        Log.Message("광고 보기 성공!");
        return await _canAdReward.Task;
    }

    /// <summary>
    /// 광고를 불러옵니다. 만약 첫 호출이 아닌 경우 DestoryAd를 먼저 호출하세요.
    /// </summary>
    public void LoadAd()
    {
        if (_rewardedAd != null && !_rewardedAd.CanShowAd())
        {
            Log.Message("광고가 시청 가능하거나 초기화 되어 있지 않습니다.");
            return;
        }
        var adRequest = new AdRequest(); // load 됌

        // TODO : AD_Unit_ID를 Test ID로 교체
        RewardedAd.Load(AD_UNIT_ID, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Log.Message("광고 수신 실패");
                return;
            }
            Log.Message($"광고 수신을 위한 응답 상태 :{ad.GetResponseInfo()}");
            
            _rewardedAd = ad;

            RegisterEventHandler(ad);
        });
    }

    /// <summary>
    /// 준비된 광고를 시청합니다. 시청 완료 후 DestoryAd()를 호출하세요.
    /// </summary>
    public void ShowAd()
    {
        if(_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            Log.Message("광고 시청.");
            _rewardedAd.Show(RewardAd);
        }
        else
        {
            Log.Message("광고를 초기화 해주세요.");
        }
    }

    private void RewardAd(Reward reward)
    {
        ServiceLocator.Get<IDataManager>().UserGoods.Claw_ += 1;
        Log.Message($"보상 추가! {ServiceLocator.Get<IDataManager>().UserGoods.Claw_}");
        _hasReward = true;
    }

    private void RegisterEventHandler(RewardedAd ad)
    {
        ad.OnAdFullScreenContentFailed += OnAdFailed;
        ad.OnAdFullScreenContentClosed += OnAdClosed;
    }

    private void UnRegisterEventHandler(RewardedAd ad)
    {
        ad.OnAdFullScreenContentFailed -= OnAdFailed;
        ad.OnAdFullScreenContentClosed -= OnAdClosed;
    }

    private void OnAdClosed()
    {
        if (_hasReward)
            _canAdReward.TrySetResult(true);
        else
            _canAdReward.TrySetResult(false);
        _hasReward = false;
    }

    private void OnAdFailed(AdError error)
    {
        // 광고 열기 중 실패한 경우
        Debug.LogError("광고 열기를 실패 했습니다. 사유 : "
                + error);
    }

    // 광고는 1 회성, 반드시 파괴 후 다시 생성 해야 됌.
    /// <summary>
    /// 시청이 완료된 광고는 재활용이 불가 하기 때문에 파괴후 LoadAd()를 통해 다시 불러오세요.
    /// </summary>
    public void DestoryAd()
    {
        if(_rewardedAd != null)
        {
            Log.Message("광고 제거됌");
            UnRegisterEventHandler(_rewardedAd);
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
        else
        {
            Log.Message("이미 참조가 제거된 상태입니다.");
        }
    }

    public void Register() => ServiceLocator.Register<IADMobManager>(this);

    public void UnRegister() => ServiceLocator.UnRegister<IADMobManager>(this);
}
