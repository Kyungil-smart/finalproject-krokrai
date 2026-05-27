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
    [SerializeField] private TMP_Dropdown _timeDropdown;
    [SerializeField] private Button _advanceButton;
    [SerializeField] private TextMeshProUGUI _currentTimeText;
    
    private DateTime _currentGameTime;

    // 드롭다운에 들어갈 추가 시간(분), 추가 할 시간이 더있다면 배열에 추가하시면 됩니당
    private readonly int[] _timeOptions = { 10, 30, 60, 360, 1440 };
    
    private void Awake()
    {
        // 게임시간 현재시간으로 초기화
        _currentGameTime = DateTime.Now;
    }

    private void Start()
    {
        _advanceButton.onClick.AddListener(OnAdvanceButtonClicked);

        UpdateTimeText();

        Log.Message($"현재 시간 초기화 완료 : {_currentGameTime}");
    }

    private void OnAdvanceButtonClicked()
    {
        // 현재 드롭다운에 선택된 인덱스 가져오기
        int selectedIndex = _timeDropdown.value;
        
        // 선택된 인덱스에 해당하는 분(min) 가져오기
        int minutesToAdd = _timeOptions[selectedIndex];
        
        // 게임 시간 경과 버튼눌렀을때 경과하기
        _currentGameTime = _currentGameTime.AddMinutes(minutesToAdd);
        
        // 현재시간 텍스트 갱신
        UpdateTimeText();
        
        Log.Message($" {minutesToAdd}분 경과 -> {_currentGameTime}");
    }

    private void UpdateTimeText()
    {
        // 형식 : 월월/일일  시시:분분
        _currentTimeText.text = _currentGameTime.ToString("MM/dd   HH:mm");
    }

    // 외부에서 현재 게임 시간 조회(참조)할 때 사용
    public DateTime GetCurrentTime()
    {
        return _currentGameTime;
    }
    
    // 시간 초기화
    public void ResetTime()
    {
        _currentGameTime = DateTime.Now;
        UpdateTimeText();
        
        Log.Message($"시간 초기화 : {_currentGameTime}");
    }
}
