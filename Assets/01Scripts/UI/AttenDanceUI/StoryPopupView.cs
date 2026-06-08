using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryPopupView : MonoBehaviour
{
    [Header("스토리 팝업 UI")] 
    [SerializeField] private GameObject storyPopupPanel;    // Story_Popup
    [SerializeField] private TextMeshProUGUI titleText;     // Story_Popup_Title
    [SerializeField] private TextMeshProUGUI contentText;   // Story_Popup_Text
    [SerializeField] private Button closeButton;            // Btn_Popup_Close

    [Header("팝업 창 꺼진 뒤 남는 글자")] 
    [SerializeField] private GameObject mainStoryPanel;     // Story_Main
    [SerializeField] private TextMeshProUGUI mainTitle;     // Story_Main_Title
    [SerializeField] private TextMeshProUGUI mainText;      // Story_Main_Text

    private void Start()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(OnCloseButtonClicked);
        }
    }

    public void OpenPopup(string title, string content)
    {
        Log.Message("OpenPopup 메서드 실행됨");
        
        string realContent = content.Replace("\\n", "\n");

        titleText.text = title;
        contentText.text = realContent;
        mainTitle.text = title;
        mainText.text = realContent;
        
        storyPopupPanel.SetActive(true);
        mainStoryPanel.SetActive(false);
    }

    private void OnCloseButtonClicked()
    {
        storyPopupPanel.SetActive(false);
        mainStoryPanel.SetActive(true);
    }
}
