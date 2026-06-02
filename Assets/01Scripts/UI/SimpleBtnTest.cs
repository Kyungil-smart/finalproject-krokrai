using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleBtnTest : MonoBehaviour
{
    // 버튼을 누르면 저거 SetActive true 아니면 Alpha값

    public Button btn;
    public GameObject attendancePrefab;
    
    private void Start()
    {
        btn.onClick.AddListener(SetTestMethod);
    }

    private void SetTestMethod()
    {
        attendancePrefab.SetActive(true);
    }
}
