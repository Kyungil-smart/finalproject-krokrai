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

        _bakeryTime.tag = $"{hours:D2}:{mins:D2}";
    }

    /// <summary>
    /// 보유중인 냥냥 식빵 코인 텍스트 갱신
    /// </summary>
    /// <param name="coinCount">현재 코인의 개수</param>
    public void UpdateCoinUI(int coinCount)
    {
        _nyangCoin.text = coinCount.ToString();
    }

    public void SetButtonState(int index, string state)
    {
        if (index < 0 || index >= _nyangCoinButtons.Length) return;

        var targetBtn = _nyangCoinButtons[index];
        var image = targetBtn.GetComponent<Image>();
        var text = targetBtn.GetComponentInChildren<TextMeshProUGUI>();

        switch (state)
        {
            case "Locked": // 목표 미달성
                targetBtn.interactable = false;
                if (image != null) image.color = new Color(0.7f, 0.7f, 0.7f, 1f);
                if (text != null) text.text = "보상 받기";
                break;

            case "Ready": // 목표 달성, 수령 완료
                targetBtn.interactable = true;
                if (image != null) image.color = Color.white; // 색깔 다시 확인
                if (text != null) text.text = "수령 가능";
                break;

            case "Claimed": // 수령 완료
                targetBtn.interactable = false;
                if (image != null) image.color = new Color(0.3f, 0.3f, 0.3f, 1f);
                if (text != null) text.text = "수령 완료";
                break;
        }
    }
}