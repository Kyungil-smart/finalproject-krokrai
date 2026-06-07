/*
 작성자 : krokrai
 작성일 : 26-06-04
 
 역할 : 게시물 Image 및 선택된 경우 게시물 띄우기
 방식 : ProfileController에서 생성 시점에 SetPost를 호출하여, 초기 값 설정
 */
using UnityEngine;
using UnityEngine.UI;

public class ProfilePostController : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;

    private int _postNum;

    // post 내용도 출력 필요

    private void OnEnable()
    {
        _button.onClick.AddListener( OnClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

    public void SetPost(int postNum)
    {
        if (postNum == 0 || postNum < -1)
        {
            Log.Message($"UserPost 번호가 잘 못 입력되었습니다. [{postNum}]");
            return;
        }

        if(_button == null || _image == null)
        {
            _button = GetComponentInChildren <Button>();
            _image = GetComponentInChildren<Image>();
        }

        _postNum = postNum;
        Log.Message($"{_postNum} / {_image == null}");
        ServiceLocator.Get<IAddressableManager>().LoadImageSprite(_postNum.ToString(), _image);
    }

    private void OnClicked()
    {
        //ServiceLocator.Get<IUIManager>().UploadedPost(_postNum);
    }
}
