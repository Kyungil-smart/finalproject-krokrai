/*
 작성자 : krokrai
 작성일 : 26-06-02

 역할 : FireStore 내부에 NPCPost 하위에 있는 데이터를 script에 저장
 */
using Firebase.Firestore;
using System;

[FirestoreData,Serializable]
public class NPCPostState
{
    private Timestamp _postTime;
    private bool _isLikedisLiked;

    [FirestoreProperty]
    public Timestamp postTime
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

    [FirestoreProperty]
    public bool isLikedisLiked
    {
        get
        {
            return _isLikedisLiked;
        }
        set
        {
            _isLikedisLiked = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }
}
