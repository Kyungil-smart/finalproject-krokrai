/*
 작성자 : krokrai
 수정자 : NekioEmilia
 
 작성일 : 26-06-15
 수정일 : 26-06-19

 역할 : FireStore 내부에 NyangBakeryDatas 하위에 있는 데이터를 script에 저장
 방식 : FireStore에서 정보를 불러온 후 ConvertTo<NyangBakeryDatas>()를 해주면, Google 쪽에서 자동으로 scrip에 저장
 */
using Firebase.Firestore;
using System;
using System.Collections.Generic;

[Serializable, FirestoreData]
public class NyangBakeryDatas
{
    private DateTime _accTime;
    private int _bakeryCoin = 0;
    private Dictionary<string, NyanBakeryRewardState> _rewardHistory;
    private Dictionary<string, NyanBakeryShopState> _bakeryShop;

    [FirestoreProperty]
    public DateTime accTime
    {
        get
        {
            return _accTime;
        }
        set
        {
            _accTime = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }

    [FirestoreProperty]
    public int bakerycoin
    {
        get
        {
            return _bakeryCoin;
        }
        set
        {
            _bakeryCoin = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }

    [FirestoreProperty]
    public Dictionary<string, NyanBakeryRewardState> rewardHistory
    {
        get
        {
            return _rewardHistory;
        }
        set
        {
            _rewardHistory = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }
    

    [FirestoreProperty]
    public Dictionary<string, NyanBakeryShopState> BakeryShop
    {
        get
        {
            return _bakeryShop;
        }
        set
        {
            _bakeryShop = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }

    public void Init()
    {
        _rewardHistory = new Dictionary<string, NyanBakeryRewardState>()
        {
            {"rewards_15min",new()},
            {"rewards_30min",new()},
            {"rewards_45min",new()},
            {"rewards_60min",new()}
        };
        _bakeryShop = new Dictionary<string, NyanBakeryShopState>()
        {
            {"Shop_sp_001",new()},
            {"Shop_nor_001",new()},
            {"Shop_nor_002",new()},
            {"Shop_nor_003",new()},
            {"Shop_nor_004",new()},
            {"Shop_nor_005",new()},
            {"Shop_nor_006",new()},
        };
    }
}
