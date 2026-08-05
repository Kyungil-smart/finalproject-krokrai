/*
작성자 : NekioEmilia
수정자 : 
 
작성일 : 26-06-08
수정일 : 26-06-25

역할 : 미션의 ItemSlot의 View 부분을 담당하는 스크립트
방식 : Presenter에서 SetItem을 호출해서 아이콘과 수량을 갱신
*/

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotView : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI amountText;

    /// <summary>
    /// 아이템 슬롯의 UI(아이콘 이밎, 수량 텍스트)를 갱신
    /// </summary>
    /// <param name="iconKey">Addressable에서 사용할 이미지 Key (ex: Item_Res_100001)</param>
    /// <param name="amount">표시할 아이템의 수량</param>
    public void SetItem(string iconKey, int amount)
    {
        if (itemIcon == null || amountText == null)
        {
            return; 
        }
        
        amountText.text = amount.ToString();

        if (!string.IsNullOrEmpty(iconKey))
        {
            var addressableManager = ServiceLocator.Get<IAddressableManager>();
            
            if (addressableManager != null)
            {
                addressableManager.LoadImageSprite(iconKey, itemIcon);
            }
        }
        else
        {
            itemIcon.sprite = null;
        }
    }
}
