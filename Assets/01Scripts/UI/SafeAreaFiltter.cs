using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaFiltter : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Rect _lastSafeArea = new Rect(0, 0, 0, 0);
    private ScreenOrientation _lastOrientation = ScreenOrientation.Unknown;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void Update()
    {
        // 화면 방향이나 Safe Area 값이 변했을 때만 갱신
        if (_lastSafeArea != Screen.safeArea || _lastOrientation != Screen.orientation)
        {
            ApplySafeArea();
        }
    }

    private void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;
        
        // DivideByZeroException 방지
        float screenWidth = Screen.width > 0 ? Screen.width : 1;
        float screenHeight = Screen.height > 0 ? Screen.height : 1;

        // Safe Area의 좌표를 Anchor 비율(0.0 ~ 1.0)로 변환
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= screenWidth;
        anchorMin.y /= screenHeight;
        anchorMax.x /= screenWidth;
        anchorMax.y /= screenHeight;

        // RectTransform에 Anchor 값 적용
        _rectTransform.anchorMin = anchorMin;
        _rectTransform.anchorMax = anchorMax;

        // 다음 프레임 비교를 위해 현재 상태 저장
        _lastSafeArea = safeArea;
        _lastOrientation = Screen.orientation;
    }
}