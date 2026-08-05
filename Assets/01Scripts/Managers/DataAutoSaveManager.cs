/*
 작성자 : krokrai
 수정자 : 이종현, cminhyeong1999
 작성일 : 26-05-29
 수정일 : 26-06-30

 역할 : firebase에 저장될 정보를 자동으로 기달렸다가 저장함(firebase에 과도한 요청으로 인한 데이터 유실 방지)
 방식 : 비동기 방식으로 1.5초 대기 후 추가 요청 사항이 없는 경우 DataManager의 SaveData 호출
 */

using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class DataAutoSaveManager : MonoBehaviour, IManagerBooter, IDataAutoSaveManager
{
    private CancellationTokenSource _cts;
    private CancellationTokenSource _rtdbCts;
    private MainCurrencyController _mainCurrencyController;
    private TimeRewardsManager _timeRewardsManager;

#if UNITY_EDITOR
    private bool _testMode = false;

    public void SetTestMode() => _testMode = true;
#endif

    public void SetMainCurrencyController(MainCurrencyController mainCurrencyController)
    {
        _mainCurrencyController = mainCurrencyController;
    }

    public void SetTimeRewardsManager(TimeRewardsManager timeRewardsManager)
    {
        _timeRewardsManager = timeRewardsManager;
    }
    public void RequestSave()
    {
#if UNITY_EDITOR
        if (_testMode) return;
#endif
        if (!ServiceLocator.Get<IDataManager>().CanSave) return;
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        _ = DelaySaveAsync(_cts.Token);
        // SNS Follow 기반 방치 보상 시스템에서 follow 수 및 UI 갱신
        _timeRewardsManager?.RefreshFollowCount();
        _timeRewardsManager?.RefreshUI();
    }

    public void RequestRTDBSave()
    {
#if UNITY_EDITOR
        _mainCurrencyController.RefreshUI();
        if (_testMode) return;
#endif

        if (!ServiceLocator.Get<IDataManager>().CanSave) return;

        _mainCurrencyController?.RefreshUI();

        _rtdbCts?.Cancel();
        _rtdbCts = new CancellationTokenSource();

        _ = DelayRTDBSaveAsync(_rtdbCts.Token);
    }

    private async Task DelaySaveAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(1000, token);

            if (!token.IsCancellationRequested)
            {
                ServiceLocator.Get<IDataManager>().SaveData();
                Log.Message("Store Data 저장 완료");
            }
        }
        catch (OperationCanceledException e) { }
        catch (Exception e)
        {
            Log.Message($"Store Data 저장 대기 중 문제 발생 {e}");
        }
    }

    private async Task DelayRTDBSaveAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(500, token);

            if (!token.IsCancellationRequested)
            {
                ServiceLocator.Get<IDataManager>().SaveRTDBData();
                Log.Message("RTDB Data 저장 완료");
            }
        }
        catch (OperationCanceledException e) { }
        catch (Exception e)
        {
            Log.Message($"RTDB Data 저장 대기 중 문제 발생 {e}");
        }
    }

    public void Register() => ServiceLocator.Register<IDataAutoSaveManager>(this);
    public void UnRegister() => ServiceLocator.UnRegister<IDataAutoSaveManager>(this);
}
