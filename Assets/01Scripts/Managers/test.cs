using UnityEngine;
using UnityEngine.UI;

public class AddressableTest : MonoBehaviour
{
    public AddressableManager manager;

    [SerializeField] private Image testImage1;
    [SerializeField] private Image testImage2;
    [SerializeField] private SpriteRenderer renderer1;
    [SerializeField] private SpriteRenderer renderer2;

    public void LoadTwoImages()
    {
        manager.LoadImageSprite("TestSprite", testImage1);
        manager.LoadImageSprite("TestSprite1", testImage2);
    }

    public void ReleaseImage1()
    {
        manager.ReleaseSprite("TestSprite");
        manager.ReleaseSprite("TestSprite1");
    }

    public void LoadAllSprites()
    {
        manager.LoadImageSprite("TestSprite", testImage1);
        manager.LoadImageSprite("TestSprite1", testImage2);
        manager.LoadRendererSprite("TestSprite", renderer1);
        manager.LoadRendererSprite("TestSprite1", renderer2);
    }

    public void ReleaseImage2AndRenderer1()
    {
        manager.ReleaseAll();
    }
}