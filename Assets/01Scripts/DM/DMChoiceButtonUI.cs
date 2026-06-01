using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DMChoiceButtonUI : MonoBehaviour
{
    [SerializeField] private TMP_Text choiceText;
    [SerializeField] private Button button;

    public void SetData(string text, Action onClick)
    {
        choiceText.text = text;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick?.Invoke());
    }
}