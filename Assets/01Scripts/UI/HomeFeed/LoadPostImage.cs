/*
 작성자 : cminhyeong1999
 작성일 : 26-06-22
 수정일 : 26-06-22

 역할 : 각 게시물에 출력해야 할 이미지 변환용 클래스
 */
using UnityEngine;
using UnityEngine.UI;

public class LoadPostImage : MonoBehaviour
{
    [Header("수정할 이미지 공간")]
    [SerializeField] private Image _postImage;

    /// <summary>
    /// 표시할 이미지 스프라이트를 교체
    /// </summary>
    /// <param name="changeImg">표시할 이미지 스프라이트</param>
    public void SetImage(Sprite changeImg)
    {
        _postImage.sprite = changeImg;
    }
}
