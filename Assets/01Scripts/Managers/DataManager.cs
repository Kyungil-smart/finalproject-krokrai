/*
 작성자 : krokrai
 작성일 : 26-05-27
 수정일 : 26-06-02

 역할 : Firebase Store 및 RTDB와 연동으로 데이터 읽기 및 쓰기
 방식 : Firestore에는 최상위 경로에서 User만 찾은 후 Script에 밀어 넣는 방식
 */
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Firestore;
using System;
using UnityEngine;

public class DataManager : MonoBehaviour, IManagerBooter, IDataManager // 현재 업데이트 마다 데이터 추가 생성은 미구현
{
    public event Action OnUserDataReseted;

    private bool _readyToSave;
    public bool CanSave => _readyToSave;

    private string _userID;

    public DateTime _simulationCurrentTime { get; set; }

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

    // rtdb 연동할 위치
    private UserGoods _userGoods;
    public UserGoods UserGoods=> _userGoods;

    private DatabaseReference _rtdb;

    private void ReadRTDBData()
    {
        // RTDB
        _rtdb = ServiceLocator.Get<IBackendManager>()
            .Database
            .RootReference
            .Child("Users")
            .Child(_userID);

        _rtdb.GetValueAsync()
            .ContinueWithOnMainThread(async task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Log.MessageColor($"Store의 정보를 읽기 중 실패 또는 취소 되었습니다. {task.Exception.GetBaseException().Message}", Color.red);
                    return;
                }
                DataSnapshot snapShot = task.Result;

                if (!snapShot.Exists)
                {
                    Log.Message("신규 유저 감지 됌");
                    _userGoods = new UserGoods();
                    NewRTDBDataSave();
                    return;
                }

                try
                {
                    string json = snapShot.GetRawJsonValue();
                    _userGoods = JsonUtility.FromJson<UserGoods>(json);
                    Log.Message("RTDB 유저 데이터 불러오기 성공");
                }
                catch (Exception e)
                {
                    throw e;
                }
            });
    }
    public void ResetUserData()
    {
        _userGoods = new UserGoods();
        _userData = new UserDatas();
        ImageState s = new ImageState();
        s.getTime = DateTime.Now;
        _userData.ImgList.Add("502004", s); // TODO : 빌드 후 삭제
        _userData.UserPost.Add("102001",new());
        _userData.Event_Mission.Init();
        SaveRTDBData();
        OnUserDataReseted?.Invoke();
    }

    private void NewRTDBDataSave()
    {
        string json = JsonUtility.ToJson(_userGoods);
        _rtdb.SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    Log.Message("신규 유저 재화 등록 완료");
                }
                else
                {
                    Log.Message("신규 유저 재화 등록 실패");
                }
            });
    }

    public async void SaveRTDBData()
    {
        try
        {
            string json = JsonUtility.ToJson(_userGoods);
            await _rtdb.SetRawJsonValueAsync(json);
            Log.Message("재화 업데이트 완료");
        }
        catch
        {
            throw;
        }
        finally
        {
            Log.Message("유저 데이터 저장 완료");
        }
    }

    private void ReadData()
    {
        if (_userID == null || _userID == "")
        {
            Log.Message("user의 번호가 빈 값 또는 null 일 수 없습니다.");
            return;
        }
        ServiceLocator.Get<IBackendManager>()
            .Firestore
            .Collection("Users")
            .Document(_userID)
            .GetSnapshotAsync()
            .ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Log.MessageColor($"Store의 정보를 읽기 중 실패 또는 취소 되었습니다. {task.Exception.GetBaseException().Message}",Color.red);
                return;
            }

            DocumentSnapshot snapshot = task.Result;

            if (!snapshot.Exists)
            {
                Log.Message("신규 유저 감지됌. Firestore에 정보 생성");
                _userData = new();
                _userData.Event_Mission.Init();
                ImageState s = new ImageState();
                s.getTime = DateTime.Now;
                _userData.ImgList.Add("502004", s); // TODO : 빌드 후 삭제
                _userData.UserPost.Add("102001", new());
                _readyToSave = true;
                SaveData();
            }
            else
            {
                bool isFind = false;

                try
                {
                    _userData = snapshot.ConvertTo<UserDatas>();
                    Log.Message($"{_userData.ToString()} 등록 완료");
                    _readyToSave = true;
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
            Attendance.Last_Login_TimeStamp = DateTime.Now;
        }
        );
    }

    /// <summary>
    /// store에 저장이 필요한 경우 호출
    /// </summary>
    public void SaveData()
    {
        if (!_readyToSave) return;
        if (_userID == null || _userID == "")
        {
            Log.Message("user의 번호가 빈 값 또는 null 일 수 없습니다.");
            return;
        }

        ServiceLocator.Get<IBackendManager>()
            .Firestore.Collection("Users")
            .Document(_userID)
            .SetAsync(_userData)
            .ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Log.Message($"문서 저장 실패 : {task.Exception.GetBaseException().Message}");
                return;
            }
            Log.Message("문서 저장 완료");
        }
        );
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private async void ReadUserID()
    {
        try
        {
            if (await ServiceLocator.Get<IBackendManager>().ReadyTask)
            {
                _userID = ServiceLocator.Get<IBackendManager>().Auth.CurrentUser.UserId;
                ReadData();
                ReadRTDBData();
                Log.Message("User ID 성공적으로 입력됌");
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void Register()
    {
        ServiceLocator.Register<IDataManager>(this);
        _readyToSave = false;
        ReadUserID();
    }
    public void UnRegister() => ServiceLocator.UnRegister<IDataManager>(this);
}
