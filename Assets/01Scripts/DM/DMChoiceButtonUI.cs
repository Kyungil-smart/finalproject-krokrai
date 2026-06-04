/*
작성자 : 이종현
작성일 : 26-06-01
수정일 : 26-06-04

역할 : 선택지 버튼 UI 담당
방식 : 선택지 텍스트 표시 및 클릭 이벤트 연결
*/

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DMChoiceButtonUI : MonoBehaviour
{
    [SerializeField] private TMP_Text choiceText;
    [SerializeField] private Button button;

    public void SetData(string text, Action onClick)
    {
        choiceText.text = text;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick?.Invoke());
    }
}