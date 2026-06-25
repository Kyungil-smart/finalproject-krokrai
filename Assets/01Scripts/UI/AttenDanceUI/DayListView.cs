/*
작성자 : NekioEmilia
수정자 : 
 
작성일 : 26-06-04
수정일 : 26-06-15

역할 : 출석체크 UI의 1~7일차 UI에 MVP 패턴을 적용해 DayListPresenter와 통신
방식 : DayListPresenter에서 호출받아 UI를 갱신학고, 버튼 클릭시 EventManger를 통해 이벤트를 발생시킴
*/

using UnityEngine;
using UnityEngine.UI;

public class DayListView : MonoBehaviour
{
    [SerializeField] private GameObject[] _lockObjects;
    [SerializeField] private Button[] _dayBtns;
    [SerializeField] private Outline[] _outlines;

    private Outline _lastOutline = null; // 마지막으로 "눌린" Outline
    
    private void Awake()
    {
        for (int i = 0; i < _dayBtns.Length; i++)
        {
            int index = i;
            
            _dayBtns[i].onClick.AddListener(() =>
            {
                Log.Message($"일 차 눌림 (눌린 일차 {index + 1})");
                ServiceLocator.Get<IEventManager>().ClickDay(index + 1);
            });
        }
    }

    private void OnEnable()
    {
        var eventManager = ServiceLocator.Get<IEventManager>();
        if (eventManager != null)
        {
            eventManager.OnDayClicked += OnDayChangedExternally;
        }
        
        ToggleOutline(0);
    }
    
    private void OnDisable()
    {
        var eventManager = ServiceLocator.Get<IEventManager>();
        if (eventManager != null)
        {
            eventManager.OnDayClicked -= OnDayChangedExternally;
        }
    }

    /// <summary>
    /// 특정 일차 슬롯의 자물쇠 및 버튼 상호작용 상태를 갱신
    /// </summary>
    /// <param name="dayIndex">갱신할 일차의 인덱스 (0~6)</param>
    /// <param name="isUnlocked">해금 여부 (true 활성화 / false 비활성화)</param>
    public void DayListUI(int dayIndex, bool isUnlocked)
    {
        _lockObjects[dayIndex].SetActive(!isUnlocked);
        _dayBtns[dayIndex].interactable = isUnlocked;
        
    }

    // Outline 활성화/비활성화 해주는 메서드
    private void ToggleOutline(int index) 
    {
        if (index < 0 || index >= _outlines.Length)
        {
            return;
        }

        if (_lastOutline != null) _lastOutline.enabled = false;

        if (_outlines[index] != null)
        {
            _outlines[index].enabled = true;
            _lastOutline = _outlines[index];
        }
    }
    
    private void OnDayChangedExternally(int day)
    {
        int index = day - 1;
        ToggleOutline(index);
    }
}
