/*
 작성자 : 23M-RFT68
 작성일 : 26-06-16
 수정일 : 26-06-17

 역할 : 캡처를 눌렀을때 이미 획득한 이미지라면 해당 스크립트가 이미 획득했다는 텍스트를 띄워줌
 방식 : 캡처 시스템에서 초회 획득인지 확인 후 초회가 아니라면 토스트 메시지가 활성화 되었다가 페이드아웃하는 방식
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CanvasGroup = UnityEngine.CanvasGroup;

public class ToastMessage : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private Coroutine _toastCoroutine;

    private void Awake()
    {
        // 캔버스 그룹 자동 추가 또는 가져오기
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null) 
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        
        // 오브젝트는 켜둔 채로 투명하게 만들기
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;    // 투명할때 클릭 방지


    }
    
    /// <summary>
    /// 토스트 메시지 표시
    /// 총 2초 = 1초 유지 + 1초 FadeOut
    /// </summary>
    public void Show()
    {
        // 이미 표시 중이면 즉시 제거 후 재시작
        if (_toastCoroutine != null)
            StopCoroutine(_toastCoroutine);
        
        _toastCoroutine = StartCoroutine(ToastRoutine());
    }

    // 토스트 루틴
    private IEnumerator ToastRoutine()
    {
        // 즉시 표시
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        
        // 1초 유지
        yield return new WaitForSeconds(1f);
        
        // 1초 FadeOut
        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / 1f);
            yield return null;
        }

        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _toastCoroutine = null;
    }
    
}
