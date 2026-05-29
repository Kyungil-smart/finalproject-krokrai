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
    /// <summary>
    /// 최상위 Script, 대부분의 경우 사용 X
    /// </summary>
    public UserDatas UserDatas => _userData;
    /// <summary>
    /// 프로필 데이터
    /// </summary>
    public ProFile ProFile => _userData.Profile;
    /// <summary>
    /// 접속 데이터
    /// </summary>
    public Attendance Attendance => _userData.Attendance;
    /// <summary>
    /// 이벤트 데이터
    /// </summary>
    public Event_Missions Event_Missions => _userData.Event_Mission;

    
    public void ReadData(string uid)
    {
        BackendManager.Firestore.Collection("User1").Document(uid).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Log.MessageColor($"읽기 중 실패 또는 취소 되었습니다.",Color.red);
                return;
            }

            DocumentSnapshot snapshot = task.Result;

            bool isFind = false;

            Log.Message($"식별된 UID : {snapshot.Id}");
            try
            {
                _userData = snapshot.ConvertTo<UserDatas>();
                Log.Message($"{_userData.ToString()} 등록 완료");
                isFind = true;
            }
            catch (System.Exception e)
            {
                Log.Message(e.Message);
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
