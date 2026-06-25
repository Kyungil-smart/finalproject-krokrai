/*
작성자 : 23M-RFT68
수정자 : 이종현

작성일 : 26-05-27
수정일 : 26-06-24

역할 : 인게임 시간을 실제 디바이스의 시간으로 설정 및 변경
방식 : DateTime을 이용하여 실제 디바이스의 시간 반영
*/
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeSimulator : MonoBehaviour
{
    [Header("UI연결")] [SerializeField] private TMP_Dropdown _timeDropdown;
    [SerializeField] private Button _advanceButton;
    [SerializeField] private TextMeshProUGUI _currentTimeText;

    private DateTime _currentGameTime;

    // 드롭다운에 들어갈 추가 시간(분), 추가 할 시간이 더있다면 배열에 추가하시면 됩니당
    private readonly DateTime[] _timeOptions =
    {
        DateTime.MinValue.AddMinutes(10),
        DateTime.MinValue.AddHours(1),
        DateTime.MinValue.AddDays(1),
        DateTime.MinValue.AddDays(7),
    };

    private void Awake()
    {
        // 게임시간 현재시간으로 초기화
        _currentGameTime = DateTime.Now;
    }

    private void OnEnable()
    {
        ServiceLocator.Get<IDataManager>().OnUserDataReseted += ResetButton;
    }

    private void OnDisable()
    {
        ServiceLocator.Get<IDataManager>().OnUserDataReseted -= ResetButton;
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

        // 선택된 인덱스에 해당하는 경과시간 가져오기
        DateTime selected = _timeOptions[selectedIndex];

        // 게임 시간 경과 버튼눌렀을때 경과하기
        _currentGameTime = _currentGameTime
            .AddDays(selected.Day - 1) // 기본적으로 1일이 추가되어서 1을 빼줘야함
            .AddHours(selected.Hour)
            .AddMinutes(selected.Minute);

        // 현재시간 텍스트 갱신
        UpdateTimeText();

        ServiceLocator.Get<IDataManager>().Attendance.Last_Login_TimeStamp = _currentGameTime;
        ServiceLocator.Get<IPlayTimeTracker>().OnTimeAdvanced(_currentGameTime);

        Log.Message($" 적용 완료! -> {_currentGameTime}");
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

    public void ResetButton()
    {
        _currentGameTime = DateTime.Now;
        UpdateTimeText();
    }
}