/*
 작성자 : krokrai
 작성일 : 26-05-28
 수정일 : 26-05-29

 역할 : FireStore 내부에 ProFile 하위에 있는 데이터를 script에 저장
 방식 : FireStore에서 정보를 불러온 후 ConvertTo<ProFile>()를 해주면, Google 쪽에서 자동으로 scrip에 저장
 */
using Firebase.Firestore;
using System;

[Serializable, FirestoreData]
public class ProFile
{
    private long _followerCount = 100;
    private long _followingCount = 10;
    private long _profileImage = 60003;
    private bool _isActive = false;

    [FirestoreProperty]
    public long followerCount
    {
        get
        {
            return _followerCount;
        }
        set
        {
            _followerCount = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }

    [FirestoreProperty]
    public long followingCount
    {
        get
        {
            return _followingCount;
        }
        set
        {
            _followingCount = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }

    [FirestoreProperty]
    public long profileImage
    {
        get
        {
            return _profileImage;
        }
        set
        {
            _profileImage = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }
    
    [FirestoreProperty]
    public bool isActive
    {
        get
        {
            return _isActive;
        }
        set
        {
            _isActive = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }
}
