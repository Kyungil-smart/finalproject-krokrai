/*
작성자 : NekioEmilia
수정자 : 
 
작성일 : 26-06-08
수정일 : 

역할 : 버튼 클릭하면 UI SetActive 해주는 스크립트
*/

using UnityEngine;
using UnityEngine.UI;

public class CloseUI : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private GameObject _targetObject;

    private void Awake()
    {
        if (_closeButton != null)
        {
            _closeButton.onClick.AddListener(() => _targetObject.SetActive(false));
        }
    }
}
