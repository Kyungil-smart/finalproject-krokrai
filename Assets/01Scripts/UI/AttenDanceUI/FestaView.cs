/*
작성자 : NekioEmilia
수정자 : NekioEmilia
 
작성일 : 26-06-05
수정일 : 26-06-07

역할 : 출석체크 UI의 Festa Slider와 보물상자에 접근한 View 스크립트 FestaPresenter와 통신
방식 : Presenter에서 UpdateFestaUI를 불러와서 Festa (보물상자 UI)를 갱신
*/

using System;
using UnityEngine;
using UnityEngine.UI;

public class FestaView : MonoBehaviour
{
    public event Action<int> OnChestClicked;
    
    [SerializeField] private Slider festaSlider;
    [SerializeField] private GameObject[] festaRewardOpenObjects;
    [SerializeField] private GameObject[] festaRewardCloseObjects;
    [SerializeField] private Button[] chestButtons;
    
    private void Awake()
    {
        for (int i = 0; i < chestButtons.Length; i++)
        {
            int index = i;
            
            chestButtons[i].onClick.AddListener(() =>
            {
                OnChestClicked?.Invoke(index);
            });
        }
    }

    /// <summary>
    /// Slider Value를 0~1로 정규화시키고
    /// targetPoint보다 currentPoint가 높으면 보물상자 open
    /// </summary>
    /// <param name="currentPoint">DB 컬럼으로는 Total_Festa_Point를 받아오면 됨</param>
    /// <param name="maxPoint">선택적 매개변수를 사용해 페스타 게이지의 최고치</param>
    public void UpdateFestaUI(int currentPoint, int maxPoint = 700)
    {
        festaSlider.value = (float)currentPoint / maxPoint;

        for (int i = 0; i < festaRewardCloseObjects.Length; i++)
        {
            int targetPoint = (i + 1) * 100;

            if (currentPoint >= targetPoint)
            {
                festaRewardOpenObjects[i].SetActive(true);
                festaRewardCloseObjects[i].SetActive(false);
            }
            else
            {
                festaRewardOpenObjects[i].SetActive(false);
                festaRewardCloseObjects[i].SetActive(true);
            }
        }
    }
}
 