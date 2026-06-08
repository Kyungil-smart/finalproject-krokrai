/*
 작성자 : krokrai
 작성일 : 26-06-08

 역할 : prefab화 되어, 댓글 하나를 당담
 방식 : prefab화 된 상태로 PostConttroller에서 정보를 주입 받아온 후 해당 형식에 맞게 Textmeshpro에 전달
 */
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PostCommentController : MonoBehaviour
{
    [SerializeField] private Image _commentImg;
    [SerializeField] private TextMeshProUGUI _commentID;
    [SerializeField] private TextMeshProUGUI _commentComment;

    public void SetComment(string comment, string npcID,int imgNum)
    {
        ServiceLocator.Get<IAddressableManager>().LoadImageSprite(imgNum.ToString(), _commentImg);

        _commentID.text = npcID;
        _commentComment.text = comment;
    }
}
