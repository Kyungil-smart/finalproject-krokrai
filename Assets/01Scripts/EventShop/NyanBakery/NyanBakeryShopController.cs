/*
 작성자 : krokrai
 작성일 : 26-06-19

 역할 : 냥베이커리 상점 관리자
 방식 : Dictionary로 SO에 데이터를 사용하여 상점 최신화
 */
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NyanBakeryShopController : MonoBehaviour
{
    [SerializeField] private AutoSOGen_ContaineSO _bakeryTable;
    [SerializeField] private TextMeshProUGUI _remainTokenText;

    [SerializeField] private TextMeshProUGUI _special;
    [SerializeField] private Image _special_img;
    [SerializeField] private TextMeshProUGUI _pig;
    [SerializeField] private Image _pig_img;
    [SerializeField] private TextMeshProUGUI _clock;
    [SerializeField] private Image _clock_img;
    [SerializeField] private TextMeshProUGUI _gem;
    [SerializeField] private Image _gem_img;
    [SerializeField] private TextMeshProUGUI _energy;
    [SerializeField] private Image _energy_img;
    [SerializeField] private TextMeshProUGUI _magicHat;
    [SerializeField] private Image _magicHat_img;
    [SerializeField] private TextMeshProUGUI _scissors;
    [SerializeField] private Image _scissors_img;

    Dictionary<string, Bakery_ShopTableSO> _datas;
    private string[] _shopIndexs;

    void Awake()
    {
        int maxT = _bakeryTable.scriptableObjects.Length;
        _datas = new Dictionary<string, Bakery_ShopTableSO>(maxT);
        Bakery_ShopTableSO so;

        _shopIndexs = new string[maxT];

        for (int i = 0; i < maxT; i++)
        {
            if(_bakeryTable.scriptableObjects[i] is Bakery_ShopTableSO)
            {
                so = _bakeryTable.scriptableObjects[i] as Bakery_ShopTableSO;
                if (!_datas.ContainsKey(so.Shop_Index))
                {
                    _datas.Add(so.Shop_Index, so);
                    _shopIndexs[i] = so.Shop_Index;
                }
            }
        }
    }

    private void OnEnable()
    {
        RefreshToken();
        RefreshUI();
    }

    public void Buy(string Shop_Index)
    {
        var t = ServiceLocator.Get<IDataManager>().UserDatas.NyangBakery;

        if (_datas.ContainsKey(Shop_Index))
        {
            if ( (_datas[Shop_Index].Shop_buyLimit - t.BakeryShop[Shop_Index].Item_buycount ) - 1 < 0)
                return;
            int consumToken = _datas[Shop_Index].Shop_buyCost;
            if (t.bakerycoin - consumToken < 0)
                return;
            t.bakerycoin -= consumToken;
            t.BakeryShop[Shop_Index].Item_buycount += 1;
            RefreshToken();
            RefreshUI();
        }
        else
        {
            Log.Message($"없는 키 값입니다. {Shop_Index}");
        }
    }

    private void RefreshToken()
    {
        _remainTokenText.text = ServiceLocator.Get<IDataManager>().UserDatas.NyangBakery.bakerycoin.ToString();
    }

    private void RefreshUI()
    {
        var t = ServiceLocator.Get<IDataManager>().UserDatas.NyangBakery;
        Color TMProColor;
        Color ImageColor;
        foreach (var s in _datas )
        {
            if (t.bakerycoin < _datas[s.Key].Shop_buyCost)
            {
                TMProColor = Color.red;
                ImageColor = new Color(0.5f,0.5f,0.5f);
            } 
            else
            {
                TMProColor = Color.black;
                ImageColor = Color.white;
            }
                
            // 나중에 다른 방식으로 변경
            switch (s.Key)
            {
                case "Shop_sp_001":
                    _special.color = TMProColor;
                    _special_img.color = ImageColor;
                    _special.text = (_datas[s.Key].Shop_buyLimit - t.BakeryShop[s.Key].Item_buycount).ToString();
                    break;
                case "Shop_nor_001":
                    _pig.color = TMProColor;
                    _pig_img.color = ImageColor;
                    _pig.text = (_datas[s.Key].Shop_buyLimit - t.BakeryShop[s.Key].Item_buycount).ToString();
                    break;
                case "Shop_nor_002":
                    _clock.color = TMProColor;
                    _clock_img.color = ImageColor;
                    _clock.text = (_datas[s.Key].Shop_buyLimit - t.BakeryShop[s.Key].Item_buycount).ToString();
                    break;
                case "Shop_nor_003":
                    _gem.color = TMProColor;
                    _gem_img.color = ImageColor;
                    _gem.text = (_datas[s.Key].Shop_buyLimit - t.BakeryShop[s.Key].Item_buycount).ToString();
                    break;
                case "Shop_nor_004":
                    _energy.color = TMProColor;
                    _energy_img.color = ImageColor;
                    _energy.text = (_datas[s.Key].Shop_buyLimit - t.BakeryShop[s.Key].Item_buycount).ToString();
                    break;
                case "Shop_nor_005":
                    _magicHat.color = TMProColor;
                    _magicHat_img.color = ImageColor;
                    _magicHat.text = (_datas[s.Key].Shop_buyLimit - t.BakeryShop[s.Key].Item_buycount).ToString();
                    break;
                case "Shop_nor_006":
                    _scissors.color = TMProColor;
                    _scissors_img.color = ImageColor;
                    _scissors.text = (_datas[s.Key].Shop_buyLimit - t.BakeryShop[s.Key].Item_buycount).ToString();
                    break;
            }
        }
    }
}
