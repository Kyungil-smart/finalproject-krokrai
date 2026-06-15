/*
 작성자 : krokrai
 작성일 : 26-06-15

 역할 : FireStore 내부에 DMQuest 하위에 있는 데이터를 script에 저장
 방식 : FireStore에서 정보를 불러온 후 ConvertTo<DMQuest>()를 해주면, Google 쪽에서 자동으로 scrip에 저장
 */
using Firebase.Firestore;
using System;

[Serializable, FirestoreData]
public class DMQuest
{
    private DateTime _dm_Generation_timestamp = DateTime.Now;

    [FirestoreProperty]
    public DateTime DM_generation_timestamp
    {
        get
        {
            return _dm_Generation_timestamp;
        }
        set
        {
            _dm_Generation_timestamp = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }
}
