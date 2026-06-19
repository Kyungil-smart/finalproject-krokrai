/*
작성자 : NekioEmilia
수정자 : 
작성일 : 26-06-17
수정일 : 

역할 : 냥냥 베이커리 이벤트 UI와 유저 입력을 담당하는 View
방식 : Presenter의 명령을 받아 UI를 갱신하며, 유저의 버튼 클릭 시 Presenter에 이벤트를 전달함
*/


using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BakeryEventView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nyangCoin; // 코인 TMP
    [SerializeField] private TextMeshProUGUI _bakeryTime; // 시간 TMP
    [SerializeField] private Slider _timeGauge; // 게이지 슬라이더
    [SerializeField] private Button[] _nyangCoinButtons; // 냥냥코인 버튼
    [SerializeField] private GameObject[] _backGrounds;
    [SerializeField] private GameObject[] _claimedBackGrounds;

    public event Action<int> OnClaimButtonClicked;

    private void Awake()
    {
        _timeGauge.minValue = 0;
        _timeGauge.maxValue = 60;

        for (int i = 0; i < _nyangCoinButtons.Length; i++)
        {
            int index = i;

            _nyangCoinButtons[i].onClick.AddListener(() => OnClaimButtonClicked?.Invoke(index));
        }
    }

    /// <summary>
    /// 플레이 타임 게이지바와 타이머 텍스트를 실시간으로 갱신
    /// </summary>
    /// <param name="currentMinutes"></param>
    public void UpdatePlayTimeUI(int currentMinutes)
    {
        _timeGauge.value = currentMinutes;

        int hours = currentMinutes / 60;
        int mins = currentMinutes % 60;

        _bakeryTime.text = $"{hours:D2}:{mins:D2}";
    }

    /// <summary>
    /// 보유중인 냥냥 식빵 코인 텍스트 갱신
    /// </summary>
    /// <param name="coinCount">현재 코인의 개수</param>
    public void UpdateCoinUI(int coinCount)
    {
        _nyangCoin.text = coinCount.ToString();
    }

    /// <summary>
    /// UI 버튼 갱신해주는 메서드
    /// </summary>
    /// <param name="index">유저가 클릭한 버튼 인덱스</param>
    /// <param name="state">버튼의 상태</param>
    public void SetButtonState(int index, string state)
    {
        // 이상한 인덱스 들어오면 컷
        if (index < 0 || index >= _nyangCoinButtons.Length) return;

        var targetBtn = _nyangCoinButtons[index];

        switch (state)
        {
            case "Locked": // 목표 미달성 (기본 상태)
                targetBtn.interactable = false;
                
                // 둘 다 끄면 기본 버튼 이미지만 남음 (이게 Locked 연출!)
                _backGrounds[index].SetActive(false);
                _claimedBackGrounds[index].SetActive(false);
                break;

            case "Ready": // 목표 달성, 수령 가능
                targetBtn.interactable = true;
                
                // Ready 배경만 켬!
                _backGrounds[index].SetActive(true);
                _claimedBackGrounds[index].SetActive(false);
                break;

            case "Claimed": // 수령 완료
                targetBtn.interactable = false;
                
                // Claimed 배경만 켬!
                _backGrounds[index].SetActive(false);
                _claimedBackGrounds[index].SetActive(true);
                break;
        }
    }
}