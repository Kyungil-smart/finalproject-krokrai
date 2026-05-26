/*
작성자 : 이종현
작성일 : 26-05-26

역할 : AddressableManager ServiceLocator 호출용 Interface
방식 : ServiceLocator.Get<IAddressableManager>() 로 호출
*/

using UnityEngine;
using UnityEngine.UI;

public interface IAddressableManager
{
    /// <summary>
    /// UI Image용 Sprite 로드
    /// </summary>
    void LoadImageSprite(string key, Image targetImage);

    /// <summary>
    /// SpriteRenderer용 Sprite 로드
    /// </summary>
    void LoadRendererSprite(string key, SpriteRenderer renderer);

    /// <summary>
    /// 특정 Sprite 리소스 해제
    /// </summary>
    void ReleaseSprite(string key);

    /// <summary>
    /// 전체 Sprite 리소스 해제
    /// </summary>
    void ReleaseAll();
}