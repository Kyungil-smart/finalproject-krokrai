using Firebase.Firestore;
using System;

public class DMProgress
{
    private DateTime _sendTime = DateTime.Now;
    private int _progressState = 0;
    private int _selectedChoiceNum = 0;

    [FirestoreProperty]
    public DateTime SentTime
    {
        get
        {
            return _sendTime;
        }
        set
        {
            _sendTime = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }

    [FirestoreProperty]
    public int ProgressState
    {
        get
        {
            return _progressState;
        }
        set
        {
            _progressState = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }

    [FirestoreProperty]
    public int SelectedChoiceNum
    {
        get
        {
            return _selectedChoiceNum;
        }
        set
        {
            _selectedChoiceNum = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }
}
