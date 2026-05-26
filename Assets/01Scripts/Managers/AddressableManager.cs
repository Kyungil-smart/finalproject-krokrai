using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : MonoBehaviour, IAddressableManager, IManagerBooter
{
    private Dictionary<string, AsyncOperationHandle<Sprite>> spriteHandle
    = new Dictionary<string, AsyncOperationHandle<Sprite>>();

    private void Awake()
    {
        Register();
    }
    
    // Image용 Sprite 로드
    // 사용법: AddressableManager.Instance.LoadImageSprite("스프라이트의 어드레스Key", 타겟이미지);
    public void LoadImageSprite(string key, Image targetImage)
    {
        // 로드된 이미지는 재사용 하는 코드
        if (spriteHandle.ContainsKey(key))
        {
            targetImage.sprite = spriteHandle[key].Result;
            return;
        }

        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(key);
        
        handle.Completed += h =>
        {
            if (h.Status == AsyncOperationStatus.Succeeded)
            {
                spriteHandle[key] = h;
                targetImage.sprite = h.Result;
                Log.Message($"로드 성공 : {key}");
            }
            else
            {
                Log.Message($"로드 실패 : {key}");
            }
        };
    }
    
    // Sprite Renderer용 Sprite 로드
    // 사용법: AddressableManager.Instance.LoadRendererSprite("스프라이트의 어드레스Key", 렌더러)
    public void LoadRendererSprite(string key, SpriteRenderer renderer)
    {
        if (spriteHandle.ContainsKey(key))
        {
            renderer.sprite = spriteHandle[key].Result;
            return;
        }

        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(key);
        
        handle.Completed += h =>
        {
            if (h.Status == AsyncOperationStatus.Succeeded)
            {
                spriteHandle[key] = h;
                renderer.sprite = h.Result;
                Log.Message($"로드 성공 : {key}");
            }
            else
            {
                Log.Message($"로드 실패 : {key}");
            }
        };
    }
    
    // Release - 사용한 스프라이트 해제
    // 사용법: AddressableManager.Instance.ReleaseSprite("해제할 스프라이트의 어드레스key")
    //        AddressableManager.Instance.ReleaseAll(); -> 씬 전환시 이전 씬의 모든 스프라이드 해제
    public void ReleaseSprite(string key)
    {
        if (!spriteHandle.ContainsKey(key))
        {
            Log.Message($"해제할 스프라이트 없음 : {key}");
        }

        Addressables.Release(spriteHandle[key]);
        spriteHandle.Remove(key);
        Log.Message("로드 해제");
    }
    
    public void ReleaseAll()
    {
        foreach (var handle in spriteHandle.Values)
        {
            Addressables.Release(handle);
        }
        
        spriteHandle.Clear();
        Log.Message("모든 스프라이트 로드 해제");
    }

    // 앱 종료 또는 오브젝트 파괴 시 자동 해제
    private void OnDestroy()
    {
        ReleaseAll();
        UnRegister();
    }
    
    public void Register() => ServiceLocator.Register<IAddressableManager>(this);

    public void UnRegister() => ServiceLocator.UnRegister<IAddressableManager>(this);
}