/*
작성자 : NekioEmilia
수정자 : 
 
작성일 : 26-06-08
수정일 : 

역할 : 단일 미션 슬롯의 시각적 요소(게이지, 텍스트, 보상 아이콘 등)를 담당하는 View 스크립트
방식 : MissionView의 명령을 받아 자신(1개 슬롯)의 UI 상태만 갱신함
*/

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

    /// <summary>
    /// 슬롯의 고유 인덱스를 부여하고 버튼 클릭 이벤트를 세팅하는 메서드
    /// </summary>
    public void InitSlot(int index)
    {
        _slotIndex = index;
        rewardButton.onClick.AddListener(() => OnRewardClicked?.Invoke(_slotIndex));
    }

    /// <summary>
    /// 전달받은 데이터로 미션 슬롯의 텍스트, 슬라이더, 버튼 상태, 보상 아이콘을 갱신하는 메서드
    /// </summary>
    public void UpdateSlotUI(string missionName, int currentState, int goalValue, int flag, List<Reward_Group_TableSO> rewardData)
    {
        missionNameText.text = missionName;
        
        // 현재 진행도가 목표치 못뚫게 막음
        int displayState = Mathf.Min(currentState, goalValue);
        missionProgressText.text = $"{displayState} / {goalValue}"; 
        
        missionProgressSlider.value = goalValue > 0 ? (float)currentState / goalValue : 0f; // 정규화

        if (flag == 0) // 클리어 X
        {
            rewardButton.interactable = false;
            if (targetImage != null && defaultSprite != null) targetImage.sprite = defaultSprite;
            if (clearBackGround != null) clearBackGround.SetActive(false);
        }
        else if (flag == 1) // 수령 가능
        {
            rewardButton.interactable = true;
            if (targetImage != null && changeSprite != null) targetImage.sprite = changeSprite;
            if (clearBackGround != null) clearBackGround.SetActive(false);
        }
        else if (flag == 2) // 이미 수령함
        {
            rewardButton.interactable = false;
            if (targetImage != null && defaultSprite != null) targetImage.sprite = defaultSprite;
            if (clearBackGround != null) clearBackGround.SetActive(true);
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
