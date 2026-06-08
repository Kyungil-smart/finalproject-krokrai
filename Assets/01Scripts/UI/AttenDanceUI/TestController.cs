using System;
using UnityEngine;
using UnityEngine.UI;

public class TestController : MonoBehaviour
{
    [Header("UI View 연결")]
    [SerializeField] private FestaView festaView;
    [SerializeField] private MissionSlotView[] missionSlots;
    [SerializeField] private DayListView dayListView;

    [Header("버튼 연결")]
    [SerializeField] private Button btnAddGauge;
    [SerializeField] private Button btnClearMission;
    [SerializeField] private Button btnNextDay;
    
    private int _gauge = 0;
    private int _day = 1;

    private void Start()
    {
        if (btnAddGauge != null)
        {
            btnAddGauge.onClick.AddListener(() =>
            {
                _gauge += 100;
                if (festaView != null) festaView.UpdateFestaUI(_gauge);
            });
        }

        if (btnClearMission != null)
        {
            btnClearMission.onClick.AddListener(() =>
            {
                if (missionSlots == null) return;

                foreach (var slot in missionSlots)
                {
                    slot.UpdateSlotUI("122", 100, 100, 1);
                }
            });
        }

        if (btnNextDay != null)
        {
            btnNextDay.onClick.AddListener(() =>
            {
                if (dayListView == null) return;

                _day++;
                if (_day > 7) _day = 1;

                for (int i = 0; i < 7; i++)
                {
                    bool isUnlocked = (1 < _day);
                    dayListView.DayListUI(i, isUnlocked);
                }
            });
        }
    }
}