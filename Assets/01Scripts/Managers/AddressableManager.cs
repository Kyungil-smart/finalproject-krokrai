using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : MonoBehaviour
{
    private AsyncOperationHandle<Sprite> spriteHandle;
    
    public void LoadImageSprite(string key, Image targetImage)
    {
        spriteHandle = Addressables.LoadAssetAsync<Sprite>(key);

        spriteHandle.Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                targetImage.sprite = handle.Result;

                Log.Message("로드 성공");
            }
        };
    }
    
    public void LoadRendererSprite(string key, SpriteRenderer renderer)
    {
        spriteHandle = Addressables.LoadAssetAsync<Sprite>(key);

        spriteHandle.Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                renderer.sprite = handle.Result;

                Log.Message("로드 성공");
            }
        };
    }
    
    public void ReleaseImageSprite(Image targetImage)
    {
        targetImage.sprite = null;
        
        Addressables.Release(spriteHandle);
        
        Log.Message("로드 해제");
    }
    
    public void ReleaseRendererSprite(SpriteRenderer renderer)
    {
        renderer.sprite = null;
        
        Addressables.Release(spriteHandle);
        
        Log.Message("로드 해제");
    }
}