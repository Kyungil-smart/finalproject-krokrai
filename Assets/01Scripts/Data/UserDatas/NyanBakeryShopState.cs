/*
 작성자 : krokrai
 작성일 : 26-06-15

 역할 : FireStore 내부에 NyanBakeryShopState 하위에 있는 데이터를 script에 저장
 방식 : FireStore에서 정보를 불러온 후 ConvertTo<NyanBakeryShopState>()를 해주면, Google 쪽에서 자동으로 scrip에 저장
 */
using Firebase.Firestore;
using System;

[Serializable, FirestoreData]
public class NyanBakeryShopState
{
    private int _item_BuyCount = 0;

    [FirestoreProperty]
    public int Item_buycount
    {
        get
        {
            return _item_BuyCount;
        }
        set
        {
            _item_BuyCount = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }
}
