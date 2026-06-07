
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class MissionSlotView : MonoBehaviour
{
    public event Action<int> OnRewardClicked;

    [Header("UI연결")] [SerializeField] private Slider missionProgressSlider;
    [SerializeField] private TextMeshProUGUI missionNameText;
    [SerializeField] private TextMeshProUGUI missionProgressText;
    [SerializeField] private Button rewardButton;
    [SerializeField] private GameObject clearBackGround;

    [Header("Item 연결")] [SerializeField] private ItemSlotView[] rewardItemSlots;

    [Header("Ui 소스 이미지 세팅")] 
    [SerializeField] private Image targetImage;

    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite changeSprite;
    
    private int _slotIndex;

    public void InitSlot(int index)
    {
        _slotIndex = index;
        rewardButton.onClick.AddListener(() => OnRewardClicked?.Invoke(_slotIndex));
    }

    public void UpdateSlotUI(string missionName, int currentState, int goalValue, int flag, List<Reward_Group_TableSO> rewardData)
    {
        missionNameText.text = missionName;
        missionProgressText.text = $"{currentState} / {goalValue}"; 
        missionProgressSlider.value = (float)currentState / goalValue; // 정규화

        if (flag == 0) // 클리어 X
        {
            rewardButton.interactable = false;
            if (targetImage != null && defaultSprite != null) targetImage.sprite = defaultSprite;
        }
        else if (flag == 1) // 수령 가능
        {
            rewardButton.interactable = true;
            if (targetImage != null && changeSprite != null) targetImage.sprite = changeSprite;
        }
        else if (flag == 2) // 이미 수령함
        {
            rewardButton.interactable = false;
            if (targetImage != null && defaultSprite != null) targetImage.sprite = defaultSprite;
        }

        for (int i = 0; i < rewardItemSlots.Length; i++)
        {
            if (rewardData != null && i < rewardData.Count)
            {
                rewardItemSlots[i].gameObject.SetActive(true);
                
                // 일단 아이콘이 없어서 null 넘겨주는데, 스프라이트 생기는 즉시 교체
                rewardItemSlots[i].SetItem(null, rewardData[i].Amount);
            }
            else
            {
                rewardItemSlots[i].gameObject.SetActive(false);
            }
        }
    }
}
