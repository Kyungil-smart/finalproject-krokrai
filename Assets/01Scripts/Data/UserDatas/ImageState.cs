/*
 작성자 : krokrai
 작성일 : 26-06-02

 역할 : FireStore 내부에 ImgList 하위에 있는 데이터를 script에 저장
 */
using Firebase.Firestore;
using System;

[FirestoreData, Serializable]
public class ImageState
{
    private DateTime _getTime;
    private bool _isUploaded;
    private DateTime _postTime;

    [FirestoreProperty]
    public DateTime getTime
    {
        get
        {
            return _getTime;
        }
        set
        {
            _getTime = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }

    [FirestoreProperty]
    public bool isUploaded
    {
        get
        {
            return _isUploaded;
        }
        set
        {
            _isUploaded = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }

    [FirestoreProperty]
    public DateTime postTime
    {
        get
        {
            return _postTime;
        }
        set
        {
            _postTime = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }
}
