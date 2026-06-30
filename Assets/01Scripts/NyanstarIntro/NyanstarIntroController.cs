/*
 작성자 : krokrai
 작성일 : 26-06-30

 역할 : Intro animation 종료 시 인트로 UI 비활성화
 */
using UnityEngine;

public class NyanstarIntroController : MonoBehaviour
{
    [SerializeField] private GameObject _snsIntro;

    public void EndIntro()
    {
        _snsIntro.SetActive(false);
    }
}
