using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CanvasGroup = UnityEngine.CanvasGroup;

public class ToastMessage : MonoBehaviour
{
    private Coroutine _toastCoroutine;

    private void Start()
    {
        // 시작 시 숨겨두기
        gameObject.SetActive(false);
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
        // 텍스트 완전히 보이게
        gameObject.SetActive(true);
        
        // 1초 유지
        yield return new WaitForSeconds(1f);
        
        // 1초 FadeOut
        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();
        
        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, elapsed / 1f);
            yield return null;
        }

        cg.alpha = 1f;
        gameObject.SetActive(false);
        _toastCoroutine = null;
    }
    
}
