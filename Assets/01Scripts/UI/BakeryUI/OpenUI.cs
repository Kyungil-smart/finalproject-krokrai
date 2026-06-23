
using System;
using UnityEngine;
using UnityEngine.UI;

public class OpenUI : MonoBehaviour
{
    [SerializeField] private Button _openButton;
    [SerializeField] private GameObject _targetUI;

    private void Awake()
    {
        if (_openButton != null)
        {
            _openButton.onClick.AddListener(() => _targetUI.SetActive(true));
        }
    }
}
