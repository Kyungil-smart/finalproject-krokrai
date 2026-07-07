/*
 작성자 : 23M-RFT68
 작성일 : 26-06-16
 수정일 : 26-07-07

 역할 : 캡처 버튼 눌렀을때 초회 획득 이미지라면 팝업을 띄우는 시스템
 방식 : 캡처 시스템을 통해서 초회인지 확인하고 확인후 맞다면 팝업을 띄우고 팝업에 해당 획득 사진을 넣고 업로드 버튼까지 연결
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapturePreviewPopup : MonoBehaviour
{
    [SerializeField] private GameObject _semiCanvas;            // 스마트폰의 캔버스
    [SerializeField] private RowbtnController _rowbtnController;// 하단바 컨트롤러
    
    [Header("팝업 UI")]
    [SerializeField] private Image _captureImg;                 // 획득 이미지
    [SerializeField] private Button _postBtn;                   // 게시 버튼
    [SerializeField] private Button _outsideBtn;                // 투명 전체화면 버튼(외부 클릭시 나가지게)
    [SerializeField] private UploadController _uploadController;
    
    private ObjectEditMode _currentEditMode;                    // 현재 활성화 된 편집모드 저장

    private int _currentGetImg;  // 현재 표시 중인 이미지 ID
    private int _currentPostId;  // 포스트 ID

    private void Start()
    {
        gameObject.SetActive(false);   
        _postBtn.onClick.AddListener(OnPostButtonClicked);
        _outsideBtn.onClick.AddListener(OnOutsideClicked);
    }
    
    
    /// <summary>
    /// 미리보기 팝업 표시
    /// </summary>
    public void Show(int getImg, int postId, ObjectEditMode editMode)
    {
        _currentGetImg = getImg;
        _currentPostId = postId;
        _currentEditMode = editMode;  // 호출한 가구의 편집모드 저장
        
        gameObject.SetActive(true);
        // 이전 이미지 잔상 지우기
        _captureImg.sprite = null;

        ServiceLocator.Get<IAddressableManager>().LoadImageSprite(getImg.ToString(), _captureImg);
    }

    // 게시 버튼 클릭
    // -> 팝업 닫기 -> 편집 모드 종료 -> 업로드 화면으로 이동
    private void OnPostButtonClicked()
    {
        // 버튼 사운드
        ServiceLocator.Get<IAudioManager>().PlaySFX(SFXAudiosEnum.BTN1);
        
        StartCoroutine(CloseWithAnimation(() =>
        {
            // 호출한 가구의 편집 모드 종료
            _currentEditMode?.ExitEditMode();
            
            // 스마트폰 캔버스 활성화\
            if (_semiCanvas != null)
                _semiCanvas.SetActive(true);

            // 업로드 화면으로 이동
             _rowbtnController.OnTabClicked(2);
            Log.Message("업로드 화면으로 이동");
        }));
    }
    
    /// <summary>
    /// 팝업 외부 클릭 -> 팝업 닫기
    /// </summary>
    public void OnOutsideClicked()
    {
        StartCoroutine(CloseWithAnimation(null));
    }

    // 팝업 닫기 애니메이션
    // 중앙으로 빠르게 축소되면 사라짐
    private IEnumerator CloseWithAnimation(System.Action onComplete)
    {
        Vector3 originalScale = transform.localScale;
        float duration = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            yield return null;
        }

        // 스캐일 복구 후 비활성화
        transform.localScale = originalScale;
        gameObject.SetActive(false);
        
        onComplete?.Invoke();
    }
}
