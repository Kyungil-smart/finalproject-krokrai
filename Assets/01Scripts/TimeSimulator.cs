/*
작성자 : 23M-RFT68
작성일 : 26-05-27
수정일 : 26-05-27

역할 : 인게임 시간을 실제 디바이스의 시간으로 설정 및 변경
방식 : DateTime을 이용하여 실제 디바이스의 시간 반영
*/
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeSimulator : MonoBehaviour
{
    [Header("UI연결")]
    [SerializeField] private TMP_Dropdown timeDropdown;
    [SerializeField] private Button advanceButton;
    [SerializeField] private TextMeshProUGUI currentTimeText;
    
    private DateTime currentGameTime;

    private void Awake()
    {
        currentGameTime = DateTime.Now;
    }

    private void Start()
    {
        advanceButton.onClick.AddListener(OnAdvanceButtonClicked);

        UpdateTimeText();

        Log.Message($"현재 시간 초기화 완료 : {currentGameTime}");
    }

    private void OnAdvanceButtonClicked()
    {
        // 현재 드롭다운에 선택된 인덱스 가져오기
        int selectedIndex = timeDropdown.value;
        
        
    }

}
