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

    private int _postImg;
    private int _postId;

    //임시
    GameObject _post;
    // post 내용도 출력 필요

    private void OnEnable()
    {
        _button.onClick.AddListener( OnClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

    public void SetPost(int postImg, int postId,GameObject post)
    {
        if (postImg == 0 || postImg < 0)
        {
            Log.Message($"UserPost 번호가 잘 못 입력되었습니다. [{postImg}]");
            return;
        }

        _post = post;

        if(_button == null || _image == null)
        {
            _button = GetComponentInChildren <Button>();
            _image = GetComponentInChildren<Image>();
        }

        _postId = postId;
        _postImg = postImg;

        ServiceLocator.Get<IAddressableManager>().LoadImageSprite(_postImg.ToString(), _image);
        
    }

    private void OnClicked()
    {
        Log.Message("버튼 클릭");
        _post.SetActive(true);
        _post.GetComponent<PostController>().SetPost(_postImg, _postId);
        //ServiceLocator.Get<IUIManager>().UploadedPost(_postNum);
    }
}
