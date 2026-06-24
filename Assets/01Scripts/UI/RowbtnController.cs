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
    
    [SerializeField] private TabInfo[] tabs;
    [SerializeField] private int defaultTab = 0;   // 시작 탭 (0 = 홈피드)
    
    private int _currentTab = -1;   //  현재 활성화 된 탭

    private void Start()
    {
        // 버튼 이벤트 연결
        for (int i = 0; i < tabs.Length; i++)
        {
            int index = i;  // 클로저 문제 방지
            tabs[i].button.onClick.AddListener(() => OnTabClicked(index));
        }
        
        // 시작 탭 활성화
        OnTabClicked(defaultTab);
    }

    /// <summary>
    /// 탭 클릭 시 호출
    /// </summary>
    public void OnTabClicked(int index)
    {
        // 예외처리 (업로드)
        if (index == 2)
        {
            // 1. 업로드 화면 키기
            if (tabs[index].page != null)
                tabs[index].page.SetActive(true);
            
            // 2. 하단 바 비활성화
            this.gameObject.SetActive(false);
            
            Log.Message("업로드 진입 -> 하단바 비활성화");
            return;     // _currentTab을 2로 갱신하지 않고 그냥 패싱해서 직전 활성화 탭을 메모리에 보존
        }
        
        if (_currentTab == index) return;    // 이미 활성화 된 탭 클릭 시 무시
        
        // 모든 탭 비활성화
        for (int i = 0; i < tabs.Length; i++)
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
        // 하단바 다시 키기
        this.gameObject.SetActive(true);
        
        // 직전 탭 상태를 재현하기위해 탭상태 변경 사용
        if (_currentTab != -1)
        {
            for (int i = 0; i < tabs.Length; i++)
                SetTabState(i, i ==  _currentTab);
        }
    }

    // 탭 상태 변경
    private void SetTabState(int index, bool isActive)
    {
        TabInfo tab = tabs[index];
        
        if (tab.page != null)
            tab.page.SetActive(isActive);
        
        if (tab.normalIcon != null)
            tab.normalIcon.SetActive(!isActive);    // 비활성화 시 보라 아아콘
        
        if (tab.activeBg != null)
            tab.activeBg.SetActive(isActive);       // 활성화 시 보라 배경 + 흰 아이콘
    }
}
