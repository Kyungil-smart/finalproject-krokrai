using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotView : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI amountText;

    public void SetItem(Sprite icon, int amount)
    {
        if (itemIcon != null && icon != null)
        {
            itemIcon.sprite = icon;
        }

        if (amountText != null)
        {
            amountText.text = amount.ToString();
        }
    }
}
