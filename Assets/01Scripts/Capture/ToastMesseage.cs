using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CanvasGroup = UnityEngine.CanvasGroup;

public class ToastMesseage : MonoBehaviour
{
    [SerializeField] private CanvasGroup _toastPanel;           // FadeOut용 CanvasGroup
    [SerializeField] private TextMeshProUGUI _messageText;      // 메시지 텍스트

    private Coroutine _toastCoroutine;

    private void Start()
    {
        _toastPanel.alpha = 0f;
        _toastPanel.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// 토스트 메시지 표시
    /// 외부에서 호출: _toastMessage.Show("메시지 내용");
    /// </summary>
    public void Show(string message)
    {
        // 이미 표시 중이면 즉시 제거 후 재시작
        if (_toastCoroutine != null)
            StopCoroutine(_toastCoroutine);
        
        _messageText.text = message;
        _toastCoroutine = StartCoroutine(ToastRoutine());
    }

    // 토스트 루틴
    // 총 2초 = 기본 출력 1초 + FadeOut 1초
    private IEnumerator ToastRoutine()
    {
        // 즉시 표시
        _toastPanel.gameObject.SetActive(true);
        _toastPanel.alpha = 1f;
        
        // 1초 유지
        yield return new WaitForSeconds(1f);
        
        // 1초 FadeOut
        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            _toastPanel.alpha = Mathf.Lerp(1f, 0f, elapsed / 1f);
            yield return null;
        }

        _toastPanel.alpha = 0f;
        _toastPanel.gameObject.SetActive(false);
        _toastCoroutine = null;
    }
}
