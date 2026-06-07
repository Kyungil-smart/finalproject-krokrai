using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardPopupView : MonoBehaviour
{
    [SerializeField] private ItemSlotView[] popupItemSlots;
    [SerializeField] private Button confirmButton;

    private void Awake()
    {
        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(() => gameObject.SetActive(false));
        }
    }

    private void OnDisable()
    {
        gameObject.SetActive(false);
    }

    public void OpenPopup(List<Reward_Group_TableSO> rewardDatas)
    {
        gameObject.SetActive(true);

        for (int i = 0; i < popupItemSlots.Length; i++)
        {
            if (rewardDatas != null && i < rewardDatas.Count)
            {
                popupItemSlots[i].gameObject.SetActive(true);
                // 스프라이트가 없어서 일단 null로 처리, 후에 생기면 바로 교체
                popupItemSlots[i].SetItem(null, rewardDatas[i].Amount);
            }
            else
            {
                popupItemSlots[i].gameObject.SetActive(false);
            }
        }
    }
}
