/*
작성자 : NekioEmilia
수정자 : NekioEmilia
 
작성일 : 26-06-05
수정일 : 26-06-15

역할 : 출석체크 UI의 Festa Slider와 보물상자에 접근한 View 스크립트 FestaPresenter와 통신
방식 : Presenter에서 UpdateFestaUI를 불러와서 Festa (보물상자 UI)를 갱신
*/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FestaView : MonoBehaviour
{
    public event Action<int> OnChestClicked;
    
    [Header("UI 컴포넌트 연결")]
    [SerializeField] private Slider _festaSlider;
    [SerializeField] private Button[] _chestButtons;
    
    [Header("상자 이미지 교체용 세팅")]
    [SerializeField] private Image[] _chestBackGroundImages;    // 보라색/노란색으로 바뀔 슬롯 배경 이미지
    [SerializeField] private Image[] _chestIconImages;          // 열림/닫힘 상자 그림이 들어갈 이미지들
    [SerializeField] private Sprite _closeChestSprite;           // 닫힌 상자 원본 스프라이트
    [SerializeField] private Sprite _openChestSprite;            // 열린 상자 원본 스프라이트

    [Header("노란색, 보라색 색상 세팅")] 
    [SerializeField] private Color _lockedColor;
    [SerializeField] private Color _unlockedColor;
    
    private void Awake()
    {
        for (int i = 0; i < _chestButtons.Length; i++)
        {
            int index = i;
            
            _chestButtons[index].onClick.AddListener(() =>
            {
                Log.Message($"{index + 1}번 상자 클릭됨");
                OnChestClicked?.Invoke(index);
                ServiceLocator.Get<IAudioManager>().PlaySFX(SFXAudiosEnum.PURCHASED);
            });
        }
    }

    /// <summary>
    /// Slider Value를 0~1로 정규화시키고
    /// targetPoint보다 currentPoint가 높으면 보물상자 open
    /// </summary>
    /// <param name="currentPoint">DB 컬럼으로는 Total_Festa_Point를 받아오면 됨</param>
    /// <param name="recentGaugeStep">유저가 마지막으로 수령한 게이지 단계(DB값)</param>
    /// <param name="maxPoint">페스타 게이지의 최고치</param>
    public void UpdateFestaUI(int currentPoint, int recentGaugeStep, int maxPoint = 700)
    {
        _festaSlider.value = (float)currentPoint / maxPoint;

        for (int i = 0; i < _chestButtons.Length; i++)
        {
            int targetPoint = (i + 1) * 100;
            
            // 이미 수령한 단계인지 검사
            // recentGaugeStep이 2면, 인덱스 0, 1은 수령 완료
            bool isClaimed = (i < recentGaugeStep);
            // 점수는 채웠는데 아직 수령 안 한 상태인지 검사
            bool isClaimable = (!isClaimed && currentPoint >= targetPoint && i == recentGaugeStep);

            if (isClaimed)
            {
                Log.Message($"수령 완료함, 버튼 터치 불가, 아이콘 바꿈");
                // 수령 완료 : 배경  노란색 / 열린 상자 / 터치 불가
                _chestBackGroundImages[i].color = _unlockedColor;
                _chestIconImages[i].sprite = _openChestSprite;
                _chestButtons[i].interactable = false;
            }
            else if (isClaimable)
            {
                // 수령 가능 : 배경 노란색 / 닫힌 상자 / 터치 가능
                _chestBackGroundImages[i].color = _unlockedColor;
                _chestIconImages[i].sprite = _closeChestSprite; 
                _chestButtons[i].interactable = true;
            }
            else
            {
                // 잠금/미달성 : 배경 보라색 / 닫힌 상자 / 터치 불가
                _chestBackGroundImages[i].color = _lockedColor;
                _chestIconImages[i].sprite = _closeChestSprite;
                _chestButtons[i].interactable = false;
            }
        }
    }
}