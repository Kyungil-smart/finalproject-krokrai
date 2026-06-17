using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReelsView : MonoBehaviour
{
    [SerializeField] Image[] _gameImgs;
    [SerializeField] Sprite[] _sprites;

    public int GetImgNums() => _gameImgs.Length;

    public void SetImgs(int[] num)
    {
        if (num.Length != _gameImgs.Length)
        {
            Log.Message("적용할 이미지들과 넘겨준 이미지 번호의 총 갯수가 다릅니다.");
            return;
        }
        
        for (int i = 0; i < num.Length; i++)
        {
            _gameImgs[i].sprite = _sprites[num[i]];
        }
    }
}
