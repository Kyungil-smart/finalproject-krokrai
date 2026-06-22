using Firebase.Firestore;
using System;

public class DMProgress
{
    private DateTime _sendTime = DateTime.Now;
    private int _progressState = 0;
    private int _selectedChoiceNum = 0;
    private int _dm_ID = 0;
    private int _dmType = 0;
    private int _questRewardState = 0;

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
    public int DM_ID
    {
        get
        {
            return _dm_ID;
        }
        set
        {
            _dm_ID = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }

    [FirestoreProperty]
    public int DMType
    {
        get
        {
            return _dmType;
        }
        set
        {
            _dmType = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }

    [FirestoreProperty]
    public int QuestRewardState
    {
        get
        {
            return _questRewardState;
        }
        set
        {
            _questRewardState = value;
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
