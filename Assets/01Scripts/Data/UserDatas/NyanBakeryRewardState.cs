/*
 작성자 : krokrai
 작성일 : 26-05-28
 수정일 : 26-05-29

 역할 : FireStore 내부에 NyanBakeryRewardState 하위에 있는 데이터를 script에 저장
 방식 : FireStore에서 정보를 불러온 후 ConvertTo<NyanBakeryRewardState>()를 해주면, Google 쪽에서 자동으로 scrip에 저장
 */
using Firebase.Firestore;
using System;

[Serializable, FirestoreData]
public class NyanBakeryRewardState
{
    private bool _isClaimed = false;
    private DateTime _claimedAt = DateTime.Now;

    [FirestoreProperty]
    public bool RewardTime
    {
        get
        {
            return _isClaimed;
        }
        set
        {
            _isClaimed = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }
    [FirestoreProperty]
    public DateTime claimedAt
    {
        get
        {
            return _claimedAt;
        }
        set
        {
            _claimedAt = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }
}
