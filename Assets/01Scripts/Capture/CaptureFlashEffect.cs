using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureFlashEffect : MonoBehaviour
{
    [SerializeField] private Image _bgImage;     // 메인 백그라운드 이미지

    private Color _originalColor;
    
    private void Start()
    {
        _originalColor = _bgImage.color;
    }
    
    /// <summary>
    /// 반짝 연출 실행
    /// 0.5초 = 밝아지기 0.25초 + 복구 0.25초
    /// </summary>
    public IEnumerator PlayFlash()
    {
        // 0.25초 동안 밝아지기
        yield return StartCoroutine(FadeBrightness(1f, 0.25f));
        
        // 0.25초 동안 복구
        yield return StartCoroutine(FadeBrightness(0f, 0.25f));
    }

    // brightness: 0f = 원래 색상, 1f = 흰색
    private IEnumerator FadeBrightness(float targetBrightness, float duration)
    {
        Color startColor = _bgImage.color;
        Color targetColor = Color.Lerp(_originalColor, Color.white, targetBrightness);
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _bgImage.color = Color.Lerp(startColor, targetColor, elapsed / duration);
            yield return null;
        }

        _bgImage.color = targetColor;
    }
}
