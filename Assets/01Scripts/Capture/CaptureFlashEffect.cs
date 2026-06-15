using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureFlashEffect : MonoBehaviour
{
    [SerializeField] private Image _flashPanel;     // 흰색 전체화면 패널

    private void Start()
    {
        // 시작 시 완전 투명
        SetAlpha(0f);
        _flashPanel.gameObject.SetActive(false);
    }
    
    // 반짝 연출 실행
    // 0.5초 = 밝아지기 0.25초 + 복구 0.25초
    public IEnumerator PlayFlash()
    {
        _flashPanel.gameObject.SetActive(true);
        
        // 0.25초 동안 밝아지기
        yield return StartCoroutine(FadeAlpha(0f, 1f, 0.25f));
        
        // 0.25초 동안 복구
        yield return StartCoroutine(FadeAlpha(1f, 0f, 0.25f));
        
        _flashPanel.gameObject.SetActive(false);
    }

    private IEnumerator FadeAlpha(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            SetAlpha(Mathf.Lerp(from, to, elapsed / duration));
            yield return null;
        }

        SetAlpha(to);
    }

    private void SetAlpha(float alpha)
    {
        Color color = _flashPanel.color;
        color.a = alpha;
        _flashPanel.color = color;
    }
}
