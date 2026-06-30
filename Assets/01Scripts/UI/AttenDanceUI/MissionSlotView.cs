/*
작성자 : NekioEmilia
수정자 : NekioEmilia
 
작성일 : 26-06-08
수정일 : 26-06-29

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

    [Header("UI연결")] [SerializeField] private Slider _missionProgressSlider;
    [SerializeField] private TextMeshProUGUI _missionNameText;
    [SerializeField] private TextMeshProUGUI _missionProgressText;
    [SerializeField] private Button _rewardButton;
    [SerializeField] private GameObject _clearBackGround;

    [Header("Item 연결")] [SerializeField] private ItemSlotView[] _rewardItemSlots;

    [Header("Ui 소스 이미지 세팅")] 
    [SerializeField] private Image _targetImage;
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _changeSprite;
    
    [Header("MVP 연결")]
    [SerializeField] private ItemDataModel _itemModel;
    
    private int _slotIndex;

    /// <summary>
    /// 슬롯의 고유 인덱스를 부여하고 버튼 클릭 이벤트를 세팅하는 메서드
    /// </summary>
    public void InitSlot(int index)
    {
        _slotIndex = index;
        _rewardButton.onClick.AddListener(() =>
        {
            OnRewardClicked?.Invoke(_slotIndex);
        });
    }

    /// <summary>
    /// 전달받은 데이터로 미션 슬롯의 텍스트, 슬라이더, 버튼 상태, 보상 아이콘을 갱신하는 메서드
    /// </summary>
    public void UpdateSlotUI(string missionName, int currentState, int goalValue, int flag, List<Reward_Group_TableSO> rewardData)
    {
        // 현재 진행도가 목표치 못뚫게 막음
        _missionNameText.text = missionName;
        int displayState = Mathf.Min(currentState, goalValue);
        _missionProgressText.text = $"{displayState} / {goalValue}"; 
        
        float progress = goalValue > 0 ? (float)currentState / goalValue : 0f;
        _missionProgressSlider.value = Mathf.Clamp01(progress);

        if (flag == 0) // 클리어 X
        {
            _rewardButton.interactable = false;
            if (_targetImage != null && _defaultSprite != null) _targetImage.sprite = _defaultSprite;
            if (_clearBackGround != null) _clearBackGround.SetActive(false);
        }
        else if (flag == 1) // 수령 가능
        {
            _rewardButton.interactable = true;
            if (_targetImage != null && _changeSprite != null) _targetImage.sprite = _changeSprite;
            if (_clearBackGround != null) _clearBackGround.SetActive(false);
        }
        else if (flag == 2) // 이미 수령함
        {
            _rewardButton.interactable = false;
            if (_targetImage != null && _defaultSprite != null) _targetImage.sprite = _defaultSprite;
            if (_clearBackGround != null) _clearBackGround.SetActive(true);
        }
        
        for (int i = 0; i < _rewardItemSlots.Length; i++)
        {
            if (rewardData != null && i < rewardData.Count)
            {
                _rewardItemSlots[i].gameObject.SetActive(true);

                string addressableKey = "";

                if (_itemModel != null)
                {
                    var itemData = _itemModel.GetItemTableData(rewardData[i].Reward_Id);

                    if (itemData != null)
                    {
                        addressableKey = itemData.Item_string_Res_Id.Trim();
                    }
                }
                
                _rewardItemSlots[i].SetItem(addressableKey, rewardData[i].Amount);
            }
            else
            {
                _rewardItemSlots[i].gameObject.SetActive(false);
            }
        }
        
        
    }
}
