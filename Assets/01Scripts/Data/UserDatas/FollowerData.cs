/*
 작성자 : krokrai
 작성일 : 26-06-15

 역할 : FireStore 내부에 FollowerData 하위에 있는 데이터를 script에 저장
 방식 : FireStore에서 정보를 불러온 후 ConvertTo<FollowerData>()를 해주면, Google 쪽에서 자동으로 scrip에 저장
 */
using Firebase.Firestore;
using System;

[Serializable, FirestoreData]
public class FollowerData
{
    private DateTime _rewardTime = DateTime.Now;
    private int _remained = 0;

    [FirestoreProperty]
    public DateTime RewardTime
    {
        get
        {
            return _rewardTime;
        }
        set
        {
            _rewardTime = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }

    [FirestoreProperty]
    public int Remained
    {
        get
        {
            return _remained;
        }
        set
        {
            _remained = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }
}
