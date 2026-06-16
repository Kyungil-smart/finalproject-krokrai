/*
 작성자 : krokrai
 작성일 : 26-06-15

 역할 : FireStore 내부에 MiniGameDatas 하위에 있는 데이터를 script에 저장
 방식 : FireStore에서 정보를 불러온 후 ConvertTo<MiniGameDatas>()를 해주면, Google 쪽에서 자동으로 scrip에 저장
 */

using Firebase.Firestore;
using System;

[Serializable, FirestoreData]
public class MiniGameDatas
{
    private int _daily_Play_Count = 0;
    private int _id_Play_Last = 0;
    private int _token_Owned = 0;

    [FirestoreProperty]
    public int Daily_Play_Count
    {
        get
        {
            return _daily_Play_Count;
        }
        set
        {
            _daily_Play_Count = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }

    [FirestoreProperty]
    public int ID_Play_Last
    {
        get
        {
            return _id_Play_Last;
        }
        set
        {
            _id_Play_Last = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }

    [FirestoreProperty]
    public int Token_Owned
    {
        get
        {
            return _token_Owned;
        }
        set
        {
            _token_Owned = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }
}
