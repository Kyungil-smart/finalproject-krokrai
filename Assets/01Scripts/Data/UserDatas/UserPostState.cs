using Firebase.Firestore;
using System;

[FirestoreData,Serializable]
public class UserPostState
{
    private bool _isLiked;

    [FirestoreProperty]
    public bool isLiked
    {
        get
        {
            return _isLiked;
        }
        set
        {
            _isLiked = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }
}
