/*
 작성자 : krokrai
 작성일 : 26-05-29

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

    public void RequestSave()
    {
        if (!ServiceLocator.Get<IDataManager>().CanSave) return;
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        _ = DelaySaveAsync(_cts.Token);
    }

    private async Task DelaySaveAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(1500, token);

            if (!token.IsCancellationRequested)
            {
                ServiceLocator.Get<IDataManager>().SaveData();
                Log.Message("저장 완료");
            }
        }
        catch (OperationCanceledException e) { }
        catch (Exception e)
        {
            Log.Message($"저장 대기 중 문제 발생 {e}");
        }
    }

    public void Register() => ServiceLocator.Register<IDataAutoSaveManager>(this);
    public void UnRegister() => ServiceLocator.UnRegister<IDataAutoSaveManager>(this);
}
