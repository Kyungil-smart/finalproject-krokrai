/*
 작성자 : krokrai
 작성일 : 26-06-18

 역할 : 릴스의 표시된 게임 이미지를 바꾸기 위한 제어자
 방식 : MVP 패턴에서의 V에 해당 되며, int 배열로 받아 내부에서 직접 처리
 */
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
