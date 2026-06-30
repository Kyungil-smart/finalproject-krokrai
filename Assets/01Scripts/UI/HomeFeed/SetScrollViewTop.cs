/*
 작성자 : cminhyeong1999
 작성일 : 26-06-15
 수정일 : 26-06-15

 역할 : 홈피드 진입 시 맨 나중에 추가된 게시물이 맨 처음으로 보이게 하기 위함
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetScrollViewTop : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    // Start is called before the first frame update
    private void OnEnable()
    {
        _scrollRect.verticalNormalizedPosition = 1f;
    }
}
