/*
 작성자 : krokrai
 작성일 : 26-05-27
 수정일 : 26-05-29

 역할 : Firebase Store 및 RTDB와 연동으로 데이터 읽기 및 쓰기
 방식 : Firestore에는 최상위 경로에서 User만 찾은 후 Script에 밀어 넣는 방식
 */
using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class DataManager : MonoBehaviour, IManagerBooter, IDataManager
{
    private string _userID;
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

    private void ReadData()
    {
        if (_userID == null || _userID == "")
        {
            Log.Message("user의 번호가 빈 값 또는 null 일 수 없습니다.");
            return;
        }
        ServiceLocator.Get<IBackendManager>()
            .Firestore
            .Collection("User1")
            .Document(_userID)
            .GetSnapshotAsync()
            .ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Log.MessageColor($"읽기 중 실패 또는 취소 되었습니다. {task.Exception.GetBaseException().Message}",Color.red);
                return;
            }

            DocumentSnapshot snapshot = task.Result;

            if (!snapshot.Exists)
            {
                Log.Message("신규 유저 감지됌. Firestore에 정보 생성");
                _userData = new();
                _userData.Event_Mission.Init();
                // TODO : 유저 닉네임 기획에 따라 여기 추가 필요
                SaveData();
            }
            else
            {
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
        }
        );
    }
    

    /// <summary>
    /// store에 저장이 필요한 경우 호출
    /// 후에 자동 저장 구현 예정 현재은 수동 저장 필요
    /// </summary>
    public void SaveData()
    {
        if (_userID == null || _userID == "")
        {
            Log.Message("user의 번호가 빈 값 또는 null 일 수 없습니다.");
            return;
        }

        ServiceLocator.Get<IBackendManager>()
            .Firestore.Collection("User1")
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

    private async void ReadUserID()
    {
        try
        {
            if (await ServiceLocator.Get<IBackendManager>().ReadyTask)
            {
                _userID = ServiceLocator.Get<IBackendManager>().Auth.CurrentUser.UserId;
                ReadData();
                Log.Message("User ID 성공적으로 입력됌");
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void OnDestroy()
    {
        SaveData();
    }
    public void Register()
    {
        ServiceLocator.Register<IDataManager>(this);
        ReadUserID();
    }
    public void UnRegister() => ServiceLocator.UnRegister<IDataManager>(this);
}
