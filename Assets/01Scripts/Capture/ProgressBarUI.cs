using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject _progressBarPanel;      // 프로그래스바 패널
    [SerializeField] private Image[] _progressSteps;            // 화살표 5개 배열

    private void Start()
    {
        ResetBar();
    }
    
    /// <summary>
    /// 프로그래스바 업데이트 (0f ~ 1f)
    /// 0.2 = 1칸 0.4 = 2칸, 0.6 = 3칸, 0.8 = 4칸, 1.0 = 5칸
    /// </summary>
    public void UpdateBar(float progress)
    {
        // 처음 시작 시 패널 활성화
        if (!_progressBarPanel.activeSelf)
            _progressBarPanel.SetActive(true);
        
        // 현재 progress에 맞게 켜야 할 칸 수 계산
        // ex) progress = 0.45 -> 2칸 켜기
        int activeCount = Mathf.FloorToInt(progress * _progressSteps.Length);

        for (int i = 0; i < _progressSteps.Length; i++)
        {
            _progressSteps[i].gameObject.SetActive(i < activeCount);
        }
    }
    
    // 프로그래스바 초기화 (숨기기)
    public void ResetBar()
    {
        _progressBarPanel.SetActive(false);
        
        for (int i = 0; i < _progressSteps.Length; i++)
            _progressSteps[i].gameObject.SetActive(false);
    }
}
