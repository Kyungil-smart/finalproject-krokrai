/*
작성자 : NekioEmilia
수정자 : 

작성일 : 26-06-08
수정일 : 26-06-15

역할 : 미션 올클리어 시 등장하는 스토리 팝업 및 메인 화면의 스토리 텍스트 UI를 관리하는 View 스크립트
방식 : Presenter의 명령을 받아 팝업을 띄우거나(최초 클리어), 메인 화면의 텍스트 상태(미클리어/기클리어)를 갱신함
*/

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryPopupView : MonoBehaviour
{
    [Header("스토리 팝업 UI")] 
    [SerializeField] private GameObject _storyPopupPanel;    // Story_Popup
    [SerializeField] private TextMeshProUGUI _titleText;     // Story_Popup_Title
    [SerializeField] private TextMeshProUGUI _contentText;   // Story_Popup_Text
    [SerializeField] private Button _closeButton;            // Btn_Popup_Close

    [Header("팝업 창 꺼진 뒤 남는 글자")] 
    [SerializeField] private TextMeshProUGUI _mainTitle;     // Story_Main_Title
    [SerializeField] private TextMeshProUGUI _mainText;      // Story_Main_Text

    private void Start()
    {
        if (_closeButton != null)
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
        }
    }

    /// <summary>
    /// 해당 일차 미션을 최초로 올클리어 했을 때 호출되어 스토리 팝업창을 띄어주는 메서드
    /// </summary>
    /// <param name="title">팝업 및 메인 화면에 표시할 스토리 제목</param>
    /// <param name="content">팝업 맞 메인 화면에 표시할 스토리 본문 내용</param>
    public void OpenStoryPopup(string title, string content)
    {
        string realContent = content.Replace("\\n", "\n");

        _titleText.text = title;
        _contentText.text = realContent;
        _mainTitle.text = title;
        _mainText.text = realContent;
        
        _storyPopupPanel.SetActive(true);
        _mainText.gameObject.SetActive(false);
        _mainTitle.gameObject.SetActive(true);
    }

    private void OnCloseButtonClicked()
    {
        _storyPopupPanel.SetActive(false);
        _mainText.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// 미션을 아직 다 깨지 않은 일차의 탭을 눌렀을 때, 호출되는 메서드
    ///
    /// </summary>
    /// <param name="newTitle">메인 화면에 표시할 스토리 제목</param>
    public void ResetMainStoryText(string newTitle)
    {
        if (_mainTitle != null)
        {
            _mainTitle.text = newTitle; 
            _mainTitle.gameObject.SetActive(true);
        }

        if (_mainText != null)
        {
            _mainText.gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// 이미 올클리어한 일차의 탭을 다시 눌렀을 때 호출되는 메서드
    /// </summary>
    /// <param name="title">메인 화면에 표시할 스토리 제목</param>
    /// <param name="content">메인 화면에 표시할 스토리 본문 내용</param>
    public void ShowClearedStoryText(string title, string content)
    {
        string realContent = content.Replace("\\n", "\n");
        
        if (_mainTitle != null)
        {
            _mainTitle.text = title; 
            _mainTitle.gameObject.SetActive(true);
        }

        if (_mainText != null)
        {
            _mainText.text = realContent;
            _mainText.gameObject.SetActive(true);
        }
    }
}
