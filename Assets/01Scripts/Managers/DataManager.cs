/*
 작성자 : krokrai
 작성일 : 26-05-27

 역할 : Firebase Store
 */
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

public class DataManager : MonoBehaviour, IManagerBooter, IDataManager
{
    private UserDatas _userData;
    public UserDatas UserDatas => _userData;

    public ProFile ProFile => _userData.Profile;

    CollectionReference _profile;

    [ContextMenu("DataLoad")]
    public void GetCollection() => _profile = BackendManager.Firestore.Collection("User1");

    [ContextMenu("DataRead")]
    public void ReadData()
    {
        Log.Message(_profile.ToString());
        DocumentReference dr = _profile.Document("Profile");

        _profile.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Log.MessageColor($"읽기 중 실패 또는 취소 되었습니다.",Color.red);
                return;
            }

            QuerySnapshot snap = task.Result;

            bool isFind = false;

            foreach (var snapshot in snap)
            {
                if (snapshot.Id != "UID1") continue;
                Log.Message(snapshot.Id);
                try
                {
                    _userData = snapshot.ConvertTo<UserDatas>();
                    Log.Message($"{_userData.GetType()} 등록 완료");
                    isFind = true;
                }
                catch (System.Exception e)
                {
                    Log.Message(e.Message);
                }
            }

            if (!isFind)
            {
                Log.Message($"해당 UID를 찾을 수 없습니다.");
            }
        }
        );
    }

    public void Register() => ServiceLocator.Register<IDataManager>(this);
    public void UnRegister() => ServiceLocator.UnRegister<IDataManager>(this);
}
