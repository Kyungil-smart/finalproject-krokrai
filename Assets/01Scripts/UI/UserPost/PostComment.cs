using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Drawing.Text;

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
