using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReelsView : MonoBehaviour
{
    [SerializeField] Image[] _gameImgs;

    public int GetImgNums() => _gameImgs.Length;

    public void SetImgs(string[] num)
    {
        if (num.Length != _gameImgs.Length)
        {
            Log.Message("적용할 이미지들과 넘겨준 이미지 번호의 총 갯수가 다릅니다.");
            return;
        }

        var t = ServiceLocator.Get<IAddressableManager>();
        
        for (int i = 0; i < num.Length; i++)
        {
            t.LoadImageSprite(num[i], _gameImgs[i]);
        }
    }
}
