/*
작성자 : NekioEmilia
수정자 : NekioEmilia

작성일 : 26-06-18
수정일 : 26-06-19

역할 : 냥냥 베이커리 이벤트 UI와 데이터를 연결하고 제어하는 Presenter
방식 : View에 직접 데이터를 그리지 않고, DB의 상태를 확인해 View에게 UI를 갱신함
*/

using System;
using System.Collections.Generic;
using UnityEngine;

public class BakeryEventPresenter : MonoBehaviour
{
    [SerializeField] private BakeryEventView _bakeryView;
    [SerializeField] private BakeryAccTimeDataModel _accTimeModel;

    private Bakery_accTime_TableSO[] _rewardTables;
    
    private void Start()
    {
        var eventManager = ServiceLocator.Get<IEventManager>();

        if (eventManager != null)
        {
            eventManager.OnPlayTimeUpdated += HandlePlayTimeUpdated;
            eventManager.OnMidnightReset += HandleMidnightReset;
            eventManager.OnBakeryCoinChanged += HandleCoinChanged;
        }
        
        if (_bakeryView != null)
        {
            _bakeryView.OnClaimButtonClicked += HandleClaimReward;
        }

        if (_accTimeModel != null)
        {
            _rewardTables = _accTimeModel.GetAllTableData();
        }
        else
        {
            Log.Message($"<color=red><b>BakeryAccTimeDataModel이 인스펙터에 연결되지 않았습니다.</b></color>");
            return;
        }

        InitUI();
    }

    private void OnDestroy()
    {
        var eventManager = ServiceLocator.Get<IEventManager>();

        if (eventManager != null)
        {
            eventManager.OnPlayTimeUpdated -= HandlePlayTimeUpdated;
            eventManager.OnMidnightReset -= HandleMidnightReset;
            eventManager.OnBakeryCoinChanged -= HandleCoinChanged;
        }
        
        if (_bakeryView != null)
        {
            _bakeryView.OnClaimButtonClicked -= HandleClaimReward;
        }
    }

    /// <summary>
    /// 처음 팝업이 켜질 때 DB에서 데이터 받아와서 UI를 초기 세팅하는 메서드
    /// </summary>
    private void InitUI()
    {
        var dataManager = ServiceLocator.Get<IDataManager>();
        if (dataManager == null || dataManager.UserDatas == null) return;

        var bakeryDB = dataManager.UserDatas.NyangBakery;
        
        // 시간이 1시간을 넘어갈 경우 총 누적 분으로 계산 보정
        int currentMinutes = (bakeryDB.accTime.Hour * 60) + bakeryDB.accTime.Minute;
        
        _bakeryView.UpdatePlayTimeUI(currentMinutes);
        _bakeryView.UpdateCoinUI(bakeryDB.bakerycoin);

        if (bakeryDB.rewardHistory == null)
        {
            bakeryDB.Init();
        }
        
        RefreshButtonState(currentMinutes);
    }
    
    /// <summary>
    /// PlayTimeTracker에서 분 단위가 변동될 때 수신하여, 플레이 타임 게이지를 조절해주는 메서드
    /// </summary>
    /// <param name="currentMinutes">현재 갱신된 누적 접속 시간</param>
    private void HandlePlayTimeUpdated(int currentMinutes)
    {
        Debug.Log($"<color=yellow><b>이벤트 수신 완료 UI 갱신 요청 들어옴 (현재: {currentMinutes}분)</b></color>");
        
        _bakeryView.UpdatePlayTimeUI(currentMinutes);
        RefreshButtonState(currentMinutes);
    }
    
    /// <summary>
    /// PlayTimeTracker에서 자정이 지났을 때 수신하여, 시간과 버튼을 초기화해주는 메서드
    /// </summary>
    private void HandleMidnightReset()
    {
        var dataManager = ServiceLocator.Get<IDataManager>();
        if (dataManager == null || dataManager.UserDatas == null) return;
        
        var bakeryDB = dataManager.UserDatas.NyangBakery;

        int currentMinutes = (bakeryDB.accTime.Hour * 60) + bakeryDB.accTime.Minute;
        int autoClaimedCoins = 0;

        for (int i = 0; i < _rewardTables.Length; i++)
        {
            var targetData = _rewardTables[i];

            if (targetData == null) continue;

            int target = targetData.Target_Time;
            string key = targetData.Reward_ID;

            if (bakeryDB.rewardHistory.TryGetValue(key, out var rewardState))
            {
                if (currentMinutes >= target && !rewardState.RewardTime)
                {
                    autoClaimedCoins += targetData.Reward_Count;
                    Log.Message($"자정 자동 수령 {key} 보상 획득 + {targetData.Reward_Count} 코인");
                }

                rewardState.RewardTime = false;
                rewardState.claimedAt = default;
                
                bakeryDB.rewardHistory[key] = rewardState;
            }
        }

        if (autoClaimedCoins > 0)
        {
            bakeryDB.bakerycoin += autoClaimedCoins;
            _bakeryView.UpdateCoinUI(bakeryDB.bakerycoin);
            
            Debug.Log($"<color=magenta><b>[자정 정산 완료] 총 {autoClaimedCoins} 코인이 자동 지급되었습니다. (현재: {bakeryDB.bakerycoin})</b></color>");
        }
        
        _bakeryView.UpdatePlayTimeUI(0);
        RefreshButtonState(0);
    }
    
    /// <summary>
    /// 코인 개수가 변경되면 호출되어 코인을 갱신해주는 메서드 
    /// </summary>
    /// <param name="currentCoin">현재 보유한 식빵 코인 총 개수</param>
    private void HandleCoinChanged(int currentCoin)
    {
        _bakeryView.UpdateCoinUI(currentCoin);
    }
    
    /// <summary>
    /// 4개의 버튼을 DB 정보와 비교해서 한 번에 상태(잠금/수령가능/완료)를 변화시켜주는 메서드
    /// </summary>
    /// <param name="currentMinutes">달성한 플레이 시간</param>
    private void RefreshButtonState(int currentMinutes)
    {
        var bakeryDB = ServiceLocator.Get<IDataManager>().UserDatas.NyangBakery;

        if (bakeryDB == null)
        {
            Log.Message($"<color><b>DB의 bakeryDB가 null입니다.</b></color>");
            return;
        }

        for (int i = 0; i < _rewardTables.Length; i++)
        {
            var targetData = _rewardTables[i];
            if (targetData == null) continue;
            
            string key = targetData.Reward_ID;
            int target = targetData.Target_Time;

            bool isClaimed = false;
            if (bakeryDB.rewardHistory.TryGetValue(key, out var rewardState))
            {
                isClaimed = rewardState.RewardTime;
            }
            
            if (isClaimed) // 이미 획득함
            {
                _bakeryView.SetButtonState(i, "Claimed");
            }
            else if (currentMinutes >= target) // 수령 가능함
            {
                _bakeryView.SetButtonState(i, "Ready");
            }
            else // 아직 시간이 안됨
            {
                _bakeryView.SetButtonState(i, "Locked");
            }
        }
    }
    
    /// <summary>
    /// 유저가 N번째 버튼을 눌렀을 때 실행되는 수령 로직, 실패 시 롤백하는 트랜잭션 방어가 포함되어 있는 메서드
    /// </summary>
    /// <param name="index">유저가 클릭한 버튼의 배열 인덱스 (0~3)</param>
    private void HandleClaimReward(int index)
    {
        if (index < 0 || index >= _rewardTables.Length) return;
        
        var dataManager = ServiceLocator.Get<IDataManager>();
        var bakeryDB = dataManager.UserDatas.NyangBakery;
        
        var targetData = _rewardTables[index];
        int target = targetData.Target_Time;
        string key = targetData.Reward_ID;
        int rewardCoin = targetData.Reward_Count;
        
        var rewardState = bakeryDB.rewardHistory[key];

        // 보상을 받았거나, 시간이 안됐다면 return
        int currentMinutes = (bakeryDB.accTime.Hour * 60) + bakeryDB.accTime.Minute;
        
        if (rewardState.RewardTime || currentMinutes < target) return;

        // 롤백용 데이터 백업
        int originalCoin = bakeryDB.bakerycoin;
        bool originalClaimed = rewardState.RewardTime;
        DateTime originalTime = rewardState.claimedAt;

        try
        {
            // 수령 처리 및 시간 기록, 코인 추가
            rewardState.RewardTime = true;
            rewardState.claimedAt = DateTime.Now;
            bakeryDB.rewardHistory[key] = rewardState;
            bakeryDB.bakerycoin += rewardCoin;
            
            // UI 갱신
            _bakeryView.UpdateCoinUI(bakeryDB.bakerycoin);
            _bakeryView.SetButtonState(index, "Claimed");

            Log.Message($"[{key}] 보상 수령 성공, 현재 코인: {bakeryDB.bakerycoin}");
        }
        catch (Exception e)
        {
            // 롤백
            rewardState.RewardTime = originalClaimed;
            rewardState.claimedAt = originalTime;
            bakeryDB.rewardHistory[key] = rewardState;
            bakeryDB.bakerycoin = originalCoin;
            
            Debug.LogError($"저장 실패함. 에러: {e.Message}");
        }
    }
}
