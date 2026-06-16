/*
 작성자 : 23M-RFT68
 작성일 : 26-06-16
 수정일 : 26-06-16

 역할 : 캡처 버튼이 눌렸을때 뒷배경인 메인 화면이 반짝하고 캡처가 되는 이펙트
 방식 : 코루틴과 유니티의 Color를 이용하여 뒷배경의 이미지에 흰색을 넣었다가 원래대로 돌아오는 방식
 */
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
