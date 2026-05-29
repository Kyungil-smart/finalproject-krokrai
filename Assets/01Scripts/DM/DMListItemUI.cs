using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DMListItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text previewText;
    [SerializeField] private Button button;

    public void SetData(string senderName, string preview, Action onClick)
    {
        nameText.text = senderName;
        previewText.text = preview;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick?.Invoke());
    }
}