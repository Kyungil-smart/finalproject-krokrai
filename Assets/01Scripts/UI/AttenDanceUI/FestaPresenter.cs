/*
작성자 : NekioEmilia
수정자 : NekioEmilia
 
작성일 : 26-06-05
수정일 : 26-06-25

역할 : 출석체크 UI의 Festa Slider와 보물상자에 접근한 Presenter 스크립트 FestaView와 통신
방식 : Presenter를 UI에 적용시키면 View도 같이 따라옴
*/

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FestaView))]
public class FestaPresenter : MonoBehaviour
{
    private const int MAX_FESTA_CHEST_COUNT = 7;
    private const int GRACE_DAYS = 3;
    
    private int _totalFestaPoint;
    private int _recentGaugeStep;
    private int _finalRewardReceived = 0;

    [Header("MVP 스크립트")]
    [SerializeField] private GaugeDataModel _gaugeModel;
    [SerializeField] private FestaView _view;
    [SerializeField] private RewardDataModel _rewardModel;
    [SerializeField] private RewardPopupView _rewardPopupView;

    [Header("특별 감사 팝업 UI")] 
    [SerializeField] private GameObject _specialThanksPopupPanel;
    [SerializeField] private Button _specialThanksConfirmButton;

    [Header("출석체크 종료 UI")] 
    [SerializeField] private GameObject _eventEndNotificationPanel;
    [SerializeField] private Button _eventEndConfirmButton;
    
    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("Festa_Event_Totally_Finished", 0) == 1)
        {
            Log.Message("<color=red><b>완전히 종료된 이벤트. UI를 강제 종료함.</b></color>");
            transform.root.gameObject.SetActive(false);
            return;
        }
        
        var eventManager = ServiceLocator.Get<IEventManager>();

        if (eventManager != null)
        {
            eventManager.OnGaugeIncrease += HandleGaugeIncrease;
        }
        
        if (_specialThanksConfirmButton != null)
        {
            _specialThanksConfirmButton.onClick.RemoveAllListeners();
            _specialThanksConfirmButton.onClick.AddListener(HandleThanksPopupConfirm);
        }
        
        if (_eventEndConfirmButton != null)
        {
            _eventEndConfirmButton.onClick.RemoveAllListeners();
            _eventEndConfirmButton.onClick.AddListener(HandleEventEndConfirm);
        }

        if (_view != null)
        {
            _view.OnChestClicked += HandleChestClick;
        }
        
        SetPoint();
        ReFreshUI();
        CheckSpecialPopups();
    }

    private void OnDisable()
    {
        var eventManager = ServiceLocator.Get<IEventManager>();

        if (eventManager != null)
        {
            eventManager.OnGaugeIncrease -= HandleGaugeIncrease;
        }

        if (_view != null)
        {
            _view.OnChestClicked -= HandleChestClick;
        }
    }
    
    /// <summary>
    /// 미션의 Festa 포인트를 매개변수로 받아서 Total_Festa_Point에 대입
    /// </summary>
    /// <param name="point"></param>
    private void HandleGaugeIncrease(int point)
    {
        _totalFestaPoint += point; 
        ServiceLocator.Get<IDataManager>().Attendance.Total_Festa_Point = _totalFestaPoint;
        
        ReFreshUI();
    }

    private void HandleChestClick(int chestIndex)
    {
        var gaugeSO = _gaugeModel.GetGaugeSetting(chestIndex + 1);

        if (gaugeSO == null) return;

        int rewardGroupId = gaugeSO.Reward_Accrue_Id;
        var rewardList = _rewardModel.GetRewardGroup(rewardGroupId);

        if (rewardList != null && _rewardPopupView != null)
        {
            _rewardPopupView.OpenRewardPopup(rewardList);
            
            var userGoods = ServiceLocator.Get<IDataManager>().UserGoods;
            
            foreach (var reward in rewardList)
            {
                switch (reward.Reward_Group_Id)
                {
                    case 1: userGoods.Coin_ += reward.Amount; break;
                    case 2: userGoods.Gem_ += reward.Amount; break;
                    case 3: userGoods.Energy_ += reward.Amount; break;
                    case 4: userGoods.Claw_ += reward.Amount; break;
                    case 5: userGoods.FurDoll_ += reward.Amount; break;
                    case 6: userGoods.Stone_ += reward.Amount; break;
                }
            }
        }

        _recentGaugeStep = chestIndex + 1;
        ServiceLocator.Get<IDataManager>().Attendance.Recent_Gauge_Step = _recentGaugeStep;
        
        if (_recentGaugeStep >= MAX_FESTA_CHEST_COUNT && _finalRewardReceived == 0)
        {
            _finalRewardReceived = 1;
            ServiceLocator.Get<IDataManager>().Attendance.Final_Reward_Received = 1;

            OpenSpecialThanksPopup();
        }
        
        ReFreshUI();
    }

    void SetPoint()
    {
        _totalFestaPoint = ServiceLocator.Get<IDataManager>().Attendance.Total_Festa_Point;
        _recentGaugeStep = ServiceLocator.Get<IDataManager>().Attendance.Recent_Gauge_Step;
        _finalRewardReceived = ServiceLocator.Get<IDataManager>().Attendance.Final_Reward_Received;
    }

    private void ReFreshUI()
    {
        _view.UpdateFestaUI(_totalFestaPoint, _recentGaugeStep); 
    }

    private void OpenSpecialThanksPopup()
    {
        if (_specialThanksPopupPanel == null) return;
        
        _specialThanksPopupPanel.SetActive(true);
    }

    private void HandleThanksPopupConfirm()
    {
        _specialThanksPopupPanel.SetActive(false);

        _finalRewardReceived = 2;
        
        var attendanceDB = ServiceLocator.Get<IDataManager>().Attendance;

        if (attendanceDB != null)
        {
            attendanceDB.Final_Reward_Received = 2;
        }
        
        DateTime simulatedTime = ServiceLocator.Get<IDataManager>().Attendance.Last_Login_TimeStamp;
        string todayStr = simulatedTime.ToString("yyyy-MM-dd");

        PlayerPrefs.SetString("Festa_Final_Claim_Date", todayStr);
        PlayerPrefs.Save();
        
        transform.root.gameObject.SetActive(false);
    }

    private void HandleEventEndConfirm()
    {
        if (_eventEndNotificationPanel != null)
        {
            _eventEndNotificationPanel.SetActive(false);
        }
        
        PlayerPrefs.SetInt("Festa_Event_Totally_Finished", 1);
        PlayerPrefs.Save();
        
        transform.root.gameObject.SetActive(false);
    }

    private void CheckSpecialPopups()
    {
        var attendanceDB = ServiceLocator.Get<IDataManager>().Attendance;
        
        if (attendanceDB == null) return;

        if (_finalRewardReceived == 1)
        {
            OpenSpecialThanksPopup();
            return;
        }

        if (_finalRewardReceived == 2)
        {
            string savedDateStr = PlayerPrefs.GetString("Festa_Final_Claim_Date", "");
            
            if (!string.IsNullOrEmpty(savedDateStr) && DateTime.TryParse(savedDateStr, out DateTime savedDate))
            {
                DateTime simulatedTime = ServiceLocator.Get<IDataManager>().Attendance.Last_Login_TimeStamp;
                
                // 오늘 날짜와 로컬에 저장된 날짜의 차이 계산
                int elapsedDays = (simulatedTime - savedDate).Days;

                if (elapsedDays >= GRACE_DAYS) // GRACE_DAYS = 3
                {
                    Log.Message($"<color=red>보상 수령 후 {elapsedDays}일 경과 최종 종료 팝업을 띄웁니다.</color>");
                    
                    if (_eventEndNotificationPanel != null)
                    {
                        _eventEndNotificationPanel.SetActive(true);
                    }
                }
            }
            else
            {
                PlayerPrefs.SetString("Festa_Final_Claim_Date", DateTime.Today.ToString("yyyy-MM-dd"));
                PlayerPrefs.Save();
            }
        }
    }
}
