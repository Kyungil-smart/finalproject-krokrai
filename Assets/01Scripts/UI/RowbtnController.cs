/*
 작성자 : 23M-RFT68
 수정자 : 이종현
 작성일 : 26-06-24
 수정일 : 26-07-07
 
 역할  :  하단바 제어 + 홈 이동(돌아가기 버튼) + 알림의 경우 알림 뱃지 해제 추가
 방식  : RowbtnBar 오브젝트에 붙인 후 배열을 5개 추가 해주고 각각 버튼과 기본 버튼, 눌렸을때 버튼, 페이지를 연결해준 뒤
        디폴트 값으로 0번 페이지(홈피드)가 나오도록 defaultTab엔 0을 고정으로 기입 해준다.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RowbtnController : MonoBehaviour
{
    [System.Serializable]
    public class TabInfo
    {
        public Button button;           // 버튼
        public GameObject page;         // 해당 탭의 페이지
        public GameObject normalIcon;   // 기본 보라색 아이콘
        public GameObject activeBg;     // 활성화 보라색 배경 + 흰색 아이콘
    }
    
    [SerializeField] private TabInfo[] _tabs;
    [SerializeField] private int _defaultTab = 0;            // 시작 탭 (0 = 홈피드)
    [SerializeField] private GameObject _notiAlertIcon;      // 알림이 왔을때 켜지는 아이콘 뱃지
    [SerializeField] private GameObject _dmList;             // DM 목록
    [SerializeField] private CanvasGroup _rowBarCanvas;      // 하단바 캔버스
    [SerializeField] private Button _homeXButton;            // 하단 X 버튼
    [SerializeField] private GameObject _canvasSemi;         // 캔버스 세미
    
    private int _currentTab = -1;   //  현재 활성화 된 탭
    private int _tabBeforeDM = 1;   //  직전탭 기억용

    private void Start()
    {
        // 버튼 이벤트 연결
        for (int i = 0; i < _tabs.Length; i++)
        {
            int index = i;  // 클로저 문제 방지
            _tabs[i].button.onClick.AddListener(() => OnTabClicked(index));
        }
        
        // 시작 탭 활성화
        OnTabClicked(_defaultTab);
    }

    /// <summary>
    /// 탭 클릭 시 호출
    /// </summary>
    public void OnTabClicked(int index)
    {
        // 버튼 사운드
        ServiceLocator.Get<IAudioManager>().PlaySFX(SFXAudiosEnum.BTN2);
        
        // 예외처리 (업로드)
        if (index == 2)
        {
            // 업로드 진입 시 DM 목록 닫기
            if (_dmList != null && _dmList.activeSelf)
            {
                _dmList.SetActive(false);
                Log.Message("업로드 진입 -> DM 목록 닫힘");
            }
            
            // 1. 업로드 화면 키기
            if (_currentTab != -1 && _tabs[_currentTab].page != null)
                _tabs[_currentTab].page.SetActive(false);
            
            if (_tabs[index].page != null)
                _tabs[index].page.SetActive(true);
            
            // 2. 하단 바 비활성화
            this.gameObject.SetActive(false);
            
            Log.Message("업로드 진입 -> 하단바 비활성화");
            return;     // _currentTab을 2로 갱신하지 않고 그냥 패싱해서 직전 활성화 탭을 메모리에 보존
        }
        
        if (_currentTab == index) return;    // 이미 활성화 된 탭 클릭 시 무시
        
        // 하단바 클릭 시 DM 목록 페이지 자동으로 닫기
        if (_dmList != null && _dmList.activeSelf)
        {
            _dmList.SetActive(false);
            Log.Message("DM 목록 닫힘");
        }

        // [알림 뱃지 자동 삭제] 알림 탭 누르면 알림 아이콘 끄기
        if (index == 3 && _notiAlertIcon != null)
        {
            _notiAlertIcon.SetActive(false);

            try
            {
                ServiceLocator.Get<IDataManager>().UserDatas.Profile.isActive = false;
                Log.Message("알림 확인 완료: isActive = false 저장");
            }
            catch (Exception e)
            {
                Log.Message($"알림 상태 저장 실패: {e.Message}");
            }
        }

        // 모든 탭 비활성화
        for (int i = 0; i < _tabs.Length; i++)
            SetTabState(i, false);
        
        // 클릭한 탭만 활성화
        SetTabState(index, true);
        _currentTab = index;
    }

    /// <summary>
    /// 3. 뒤로가기 복원 함수
    /// </summary>
    public void CloseUploadAndRestore()
    {
        // 버튼 사운드
        ServiceLocator.Get<IAudioManager>().PlaySFX(SFXAudiosEnum.BTN2);
        
        // 업로드 페이지 닫기
        if (_tabs[2].page != null)
            _tabs[2].page.SetActive(false);
        
        // 하단바 다시 키기
        this.gameObject.SetActive(true);
        
        // 직전 탭 상태를 재현하기위해 탭상태 변경 사용
        if (_currentTab != -1)
        {
            for (int i = 0; i < _tabs.Length; i++)
                SetTabState(i, i == _currentTab);
        }
    }

    /// <summary>
    /// 알림 / 릴스 등에서 뒤로가기 버튼 누르면 홈피드로 강제 복귀!
    /// </summary>
    public void MoveToHomeFeed()
    {
        // 버튼 사운드
        ServiceLocator.Get<IAudioManager>().PlaySFX(SFXAudiosEnum.BTN2);
        
        Log.Message("뒤로가기 클릭 -> 홈피드 화면으로 이동");
        OnTabClicked(0);
    }

    /// <summary>
    /// DM 열렸을때 하단바 비활성화
    /// </summary>
    public void OpenDM()
    {
        // 버튼 사운드
        ServiceLocator.Get<IAudioManager>().PlaySFX(SFXAudiosEnum.BTN2);
        
        // 현재 페이지 비활성화
        _tabBeforeDM = _currentTab;
        Log.Message($"DM 목록 진입 -> 직전 위치 백업: {_tabBeforeDM}");
        
        // 현재 활성화되어있던 탭 페이지를 화면에서 비활성화
        if (_currentTab != -1 && _tabs[_currentTab].page != null)
        {
            _tabs[_currentTab].page.SetActive(false);
            Log.Message($"{_currentTab}번 탭 비활성화");
        }

        // DM 목록 오브젝트를 화면에 활성화
        if (_dmList != null)
        {
            _dmList.SetActive(true);
        }
    }

    /// <summary>
    /// DM 닫았을때 하단바 복원
    /// </summary>
    public void CloseDM()
    {
        // 버튼 사운드
        ServiceLocator.Get<IAudioManager>().PlaySFX(SFXAudiosEnum.BTN2);
        
        // DM 목록 비활성화
        if (_dmList != null)
        {
            _dmList.SetActive(false);
        }

        // 백업한 직전탭으로 복원
        int targetTab = (_tabBeforeDM != -1) ? _tabBeforeDM : _defaultTab;
        Log.Message($"DM 목록 종료 -> {targetTab}번 탭 복원");
        
        // 모든 탭의 상태를 갱신
        for (int i = 0; i < _tabs.Length; i++)
        {
            SetTabState(i, i == targetTab);
        }

        // 현재 활성화된 탭 정보를 복원된 탭으로 동기화
        _currentTab = targetTab;

        Log.Message("DM 목록 종료 -> 이전 페이지 복원");
    }

    // 탭 상태 변경
    private void SetTabState(int index, bool isActive)
    {
        TabInfo tab = _tabs[index];
        
        if (tab.page != null)
            tab.page.SetActive(isActive);
        
        if (tab.normalIcon != null)
            tab.normalIcon.SetActive(!isActive);    // 비활성화 시 보라 아아콘
        
        if (tab.activeBg != null)
            tab.activeBg.SetActive(isActive);       // 활성화 시 보라 배경 + 흰 아이콘
    }

    /// <summary>
    /// 홈버튼(X버튼) 누르면 끄기
    /// </summary>
    public void OnHomeButtonClicked()
    {
        // 버튼 사운드
        ServiceLocator.Get<IAudioManager>().PlaySFX(SFXAudiosEnum.BTN2);
        
        if (_canvasSemi != null)
        {
            _canvasSemi.SetActive(false);
        }
    }

    /// <summary>
    /// 미니게임 중에 하단바, 스마트폰 끄는키 비활성화 (미니게임 시작되는 부분에 해당 함수 호출하기)
    /// </summary>
    public void LockEverythingForMiniGame()
    {
        // 1. 하단바 비활성화
        if (_rowBarCanvas != null)
        {
            _rowBarCanvas.interactable = false;
            _rowBarCanvas.blocksRaycasts = false;
        }

        // 2. 스마트폰 하단 X 버튼 차단
        if (_homeXButton != null)
        {
            _homeXButton.interactable = false;
        }

        Log.Message("릴스 미니게임 시작 -> 하단바, X버튼 비활성화");
    }

    /// <summary>
    /// 게임 종료시 터치 다시 허용
    /// </summary>
    public void UnlockEverythingOnMiniGameExit()
    {
        // 1. 하단바 복원
        if (_rowBarCanvas != null)
        {
            _rowBarCanvas.interactable = true;
            _rowBarCanvas.blocksRaycasts = true;
        }
        
        // 2. 스마트폰 하단 X버튼 터치 복원
        if (_homeXButton != null)
        {
            _homeXButton.interactable = true;
        }

        Log.Message("미니게임 종료 -> 버튼 다시 활성화");
    }
}
