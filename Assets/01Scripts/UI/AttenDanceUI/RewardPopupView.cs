/*
작성자 : NekioEmilia
수정자 : 

작성일 : 26-06-08
수정일 : 26-06-15

역할 : 미션 및 게이지 보상 획득 시 화면에 나타나는 보상 팝업 스크립트
방식 : Presenter에서 획득한 보상 리스트를 넘겨주면 슬롯 UI 를 갱신하고 팝업을 활성화함
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardPopupView : MonoBehaviour
{
    [SerializeField] private ItemSlotView[] _popupItemSlots;
    [SerializeField] private Button _confirmButton;

    private void Awake()
    {
        if (_confirmButton != null)
        {
            _confirmButton.onClick.AddListener(() =>
            {
                Log.Message("버튼 클릭됨");
                gameObject.SetActive(false);
            });
        }
        else
        {
            Log.Message("confirmButton is null");
        }
    }

    /// <summary>
    /// 획득한 보상 리스트를 받아와서 팝업창의 슬롯 아이콘과 수량을 갱신하고 팝업을 띄우는 메서드
    /// </summary>
    /// <param name="rewardDatas">획득한 아이템 데이터 리스트</param>
    public void OpenRewardPopup(List<Reward_Group_TableSO> rewardDatas)
    {
        gameObject.SetActive(true);

        for (int i = 0; i < _popupItemSlots.Length; i++)
        {
            if (rewardDatas != null && i < rewardDatas.Count)
            {
                _popupItemSlots[i].gameObject.SetActive(true);
                // 스프라이트가 없어서 일단 null로 처리, 후에 생기면 바로 교체
                _popupItemSlots[i].SetItem(null, rewardDatas[i].Amount);
            }
            else
            {
                _popupItemSlots[i].gameObject.SetActive(false);
            }
        }
    }
}
