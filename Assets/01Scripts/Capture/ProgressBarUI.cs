/*
 작성자 : 23M-RFT68
 작성일 : 26-06-16
 수정일 : 26-06-16

 역할 : 가구 오브젝트(이미지)를 꾹 눌렀을 때 프로그래스바 UI가 보이게 하는 역할
 방식 : 5개의 스프라이트 이미지로 된 프리팹 형태의 프로그래스바를 기획서에서 지정해준 1초동안 차오르는 형식으로 0.2초마다
        해당 스프라이트를 활성화 하여 눈에 보이게하는 방식
 */
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
    
    /// <summary>
    /// 프로그래스바 초기화 (숨기기)
    /// </summary>
    public void ResetBar()
    {
        _progressBarPanel.SetActive(false);
        
        for (int i = 0; i < _progressSteps.Length; i++)
            _progressSteps[i].gameObject.SetActive(false);
    }
}
