using UnityEngine;

public class CardGameController : MonoBehaviour
{
    [SerializeField] byte _row; // 4 열
    [SerializeField] byte _col; // 4 행

    [SerializeField] GameObject _cardPrefab;

    CardController[] _cardCtrl;

    private void Awake()
    {
        _cardCtrl = new CardController[_row * _col];

        GameObject obj;

        for(int i = 0; i < _cardCtrl.Length; i++)
        {
            obj = Instantiate(_cardPrefab, transform);
            obj.name = $"Card_{i}";

            _cardCtrl[i] = obj.GetComponent<CardController>();
            _cardCtrl[i].SetCard();
        }
    }

    public void ResetData()
    {

    }
}
