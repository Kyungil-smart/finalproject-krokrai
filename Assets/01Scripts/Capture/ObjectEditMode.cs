/*
 작성자 : 23M-RFT68
 작성일 : 26-06-18
 수정일 : 26-06-18

 역할 : 오브젝트를 꾹 눌러서 프로그래스바가 완전히 찬 후, 편집모드로 들어가지도록 하는 역할
 방식 : 프로그래스바 스크립트와 연결해서 프로그래스바를 활용하고, 편집모드에 들어가는 버튼들을 직접 설정하여, 캡처 시스템의
        주요기능인 캡처 버튼을 만들어주고, 코루틴을 사용하여, 유저가 편집모드 진입중 다른곳으로 드래그하면 편집모드 진입을 실패하게 하는 방식
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ObjectEditMode : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("편집 모드 UI")]
    [SerializeField] private ProgressBarUI _progressBarUI;
    [SerializeField] private GameObject _editMenu;            // edit_menu 오브젝트
    [SerializeField] private GameObject _loadingIcon;         // 로딩 아이콘

    [Header("버튼 연결")] 
    [SerializeField] private Button _btnBack;                 // 뒤로가기
    [SerializeField] private Button _btnX;                    // 취소/종료
    [SerializeField] private Button _btnCapture;              // 캡처 버튼
    [SerializeField] private Button _btnRotate;               // 회전 버튼
    
    [Header("캡처 시스템")]
    [SerializeField] private CaptureSystem _captureSystem;       // 캡처 시스템
    [SerializeField] private int objectImg;                         // 소파(300001), 의자(300002), 침대(300003) 지정해주기
    
    [Header("홀딩 시스템")]
    [SerializeField] private float _holdThreshold = 0.5f;           // 편집 모드 진입 홀딩 시간

    [Header("가구 이미지")] 
    [SerializeField] private Transform _objImg;                     // 가구의 오브젝트
    
    private bool _isHolding = false;                // 현재 홀딩 중인지
    private bool _isDragging = false;               // 드래그 중인지
    private bool _isEditMode = false;               // 편집 모드 상태
    private Coroutine _holdCoroutine;               // 홀딩 코루틴

    private Vector2 _pressPosition;                 // 터치 시작 위치 (드래그 판별용)
    private const float _drag_ThresHold = 10f;      // 드래그 판별 임계값 (픽셀)
    private const float _hold_Time = 0.5f;          // 홀드 타임

    private void Start()
    {
        // 버튼 이벤트 연결
        _btnBack.onClick.AddListener(OnBackClicked);
        _btnX.onClick.AddListener(OnXclicked);
        _btnRotate.onClick.AddListener(OnRotateClicked);
        _btnCapture.onClick.AddListener(OnCaptureClicked);
        
        // 초기 상태: 편집 UI 전부 비활성화
        _editMenu.SetActive(false);
        _loadingIcon.SetActive(false);
        _progressBarUI.ResetBar();
    }
    
    /// <summary>
    /// 터치 시작
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isEditMode) return;  // 편집 모드 중엔 홀딩 감지 안함
        
        _isHolding = true;
        _isDragging = false;
        _pressPosition = eventData.position;
        
        // 홀딩 코루틴 시작
        _holdCoroutine = StartCoroutine(HoldRoutine());
    }
    
    /// <summary>
    /// 터치 종료
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_isHolding) return;
        
        _isHolding = false;
        
        // 프로그래스바 완료 전 손 뗌 -> 실패
        if (_holdCoroutine != null)
        {
            StopCoroutine(_holdCoroutine);
            _holdCoroutine = null;
        }

        _progressBarUI.ResetBar(); // 프로그래스바 초기화
    }
    
    // 홀딩 코루틴
    // 0.5초 이상 홀딩 + 드래그 없으면 편집 모드 진입
    private IEnumerator HoldRoutine()
    {
        float elapsed = 0f;

        while (elapsed < _hold_Time)
        {
            elapsed += Time.deltaTime;
            
            // 드래그 감지 -> 실패
            if (IsDragging())
            {
                _progressBarUI.ResetBar();
                yield break;
            }
            
            // 0.1초마다 프로그래스바 1칸 진행
            _progressBarUI.UpdateBar(elapsed / _hold_Time);
            yield return null;
        }
        
        // 홀딩 성공 -> 편집 모드 진입
        StartCoroutine(EnterEditMode());
    }

    //  드래그 판별
    private bool IsDragging()
    {
#if Unity_Editor
        // 에디터는 마우스로 테스트
        if (Mouse.current != null)
        {
            Vector2 currentPos = Mouse.current.position.ReadValue();
            returnn Vector2.Distance(_pressPosition, currentPos) > _drag_ThresHold;
        }
#endif
        // 실제 기기에서 터치
        if (Touchscreen.current != null)
        {
            Vector2 currentPos = Touchscreen.current.primaryTouch.position.ReadValue();
            return Vector2.Distance(_pressPosition, currentPos) > _drag_ThresHold;
        }

        return false;
    }
    
    // 편집 모드 진입
    // 오브젝트를 화면 중앙으로 슬라이드 이동 후 버튼 표시
    private IEnumerator EnterEditMode()
    {
        // 화면 중앙으로 이동 (슬라이드 연출)
        yield return StartCoroutine(MoveToCenter());
        
        // 편집 모드 UI 활성화
        _isEditMode = true;
        _editMenu.SetActive(true);
        _progressBarUI.ResetBar();
        
        Log.Message("편집 모드 진입");
    }
    
    // 화면 중앙으로 슬라이드 이동
    private IEnumerator MoveToCenter()
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 startPos = rect.anchoredPosition;
        Vector2 centerPos = Vector2.zero;
        float duration = 0.3f;
        float elapsed = 0f;
        
        if (Vector2.Distance(startPos, centerPos) > 1f)
            yield break;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            rect.anchoredPosition = Vector2.Lerp(startPos, centerPos, elapsed / duration);
            yield return null;
        }
        
        rect.anchoredPosition = centerPos;
    }

    // 버튼 이벤트
    
    // back버튼 : 뒤로가기 -> 편집 모드 종료
    private void OnBackClicked()
    {
        ExitEditMode();
    }

    // X버튼: 취소 -> 편집 모드 종료
    private void OnXclicked()
    {
        ExitEditMode();
    }
    
    // rotate버튼: 좌우반전
    private void OnRotateClicked()
    {
        Vector3 scale = _objImg.localScale;
        scale.x *= -1;
        _objImg.localScale = scale;
        Log.Message("방향전환");
    }

    private void OnCaptureClicked()
    {
        _captureSystem.OnCaptureClick(objectImg);
    }
    
    
    /// <summary>
    /// 편집 모드 종료 (X버튼 or 뒤로가기)
    /// </summary>
    public void ExitEditMode()
    {
        _isEditMode = false;
        _editMenu.SetActive(false);
        Log.Message("편집 모드 종료");
    }
    
    /// <summary>
    /// 로딩 아이콘 제어 (CaptureSystem에서 호출)
    /// </summary>
    public void ShowLoadingIcon(bool show)
    {
        _loadingIcon.SetActive(show);
    }
}
