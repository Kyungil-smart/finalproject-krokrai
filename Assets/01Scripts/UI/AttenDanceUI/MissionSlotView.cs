
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MissionSlotView : MonoBehaviour
{
    public event Action<int> OnRewardClicked;
    
    [SerializeField] private Slider missionProgressSlider;
    [SerializeField] private TextMeshProUGUI missionNameText;
    [SerializeField] private TextMeshProUGUI missionProgressText;
    [SerializeField] private Button rewardButton;
    [SerializeField] private GameObject clearBackGround;

    private int _slotIndex;

    public void InitSlot(int index)
    {
        _slotIndex = index;
        
        rewardButton.onClick.AddListener(() => OnRewardClicked?.Invoke(_slotIndex));
    }

    public void UpdateSlotUI(string missionName, int currentState, int goalValue, int flag)
    {
        missionNameText.text = missionName;
        missionProgressText.text = $"{currentState} / {goalValue}"; 
        missionProgressSlider.value = (float)currentState / goalValue; // 정규화

        rewardButton.interactable = (flag == 1);
        clearBackGround.SetActive(flag == 2);
    }
}
