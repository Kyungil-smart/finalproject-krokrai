using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }
    }

}
