/*
작성자 : NekioEmilia
수정자 : 
 
작성일 : 26-06-04
수정일 : 26-06-08

역할 : 출석체크 UI의 1~7일차 UI에 MVP 패턴을 적용해 DayListPresenter와 통신
방식 : DayListPresenter에서 호출받아 UI를 갱신학고, 버튼 클릭시 EventManger를 통해 이벤트를 발생시킴
*/


using System;
using UnityEngine;
using UnityEngine.UI;

public class DayListView : MonoBehaviour
{
    // public event Action<int> OnDayClicked;
    
    [SerializeField] private GameObject[] lockObjects;
    [SerializeField] private Button[] dayBtns;
    [SerializeField] private Outline[] outlines;

    private Outline _lastOutline = null;
    
    private void Awake()
    {
        for (int i = 0; i < dayBtns.Length; i++)
        {
            int index = i;
            
            dayBtns[i].onClick.AddListener(() =>
            {
                ToggleOutline(index);

                ServiceLocator.Get<IEventManager>().ClickDay(index + 1);
            });
        }
    }

    private void OnEnable()
    {
        ToggleOutline(0);
    }

    /// <summary>
    /// 특정 일차 슬롯의 자물쇠 및 버튼 상호작용 상태를 갱신
    /// </summary>
    /// <param name="a">일 차</param>
    /// <param name="active">자물쇠, 상호작용 활성화 변수</param>
    public void DayListUI(int a, bool active)
    {
        lockObjects[a].SetActive(!active);
        dayBtns[a].interactable = active;
    }

    // Outline 활성화/비활성화 해주는 메서드
    private void ToggleOutline(int index)
    {
        if (index < 0 || index >= outlines.Length) return;

        if (_lastOutline != null) _lastOutline.enabled = false;

        if (outlines[index] != null)
        {
            outlines[index].enabled = true;
            _lastOutline = outlines[index];
        }
    }
}
