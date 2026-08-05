/*
 작성자 : cminhyeong1999
 작성일 : 26-06-29
 수정일 : 26-06-29

 역할 : 특정 게시물의 댓글 작성자 이름칸에 현재 플레이어의 이름을 넣기 위함
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetCommentName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _commentNameText;
    private string _playerName;
    
    private void Start()
    {
        _playerName = ServiceLocator.Get<IDataManager>().UserName;
        _commentNameText.text = _playerName;
    }
}
