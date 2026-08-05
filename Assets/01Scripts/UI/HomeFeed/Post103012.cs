/*
 작성자 : cminhyeong1999
 작성일 : 26-06-29
 수정일 : 26-06-29

 역할 : 103012 post의 본문 내용 중 현재 플레이어 닉네임을 참조하기 위함
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Post103012 : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _postText;
    private string _playerName;

    private void Start()
    {
        _playerName = ServiceLocator.Get<IDataManager>().UserName;
        _postText.text = $"{_playerName}님이 보내주신 방울 낚시대! 저희 고양이는 유독 방울 낚시대만 좋아하는 것 같아요*^^*";
    }
}
