/*
 작성자 : krokrai
 작성일 : 26-05-28
 수정일 : 26-05-29

 역할 : Firebase에 접근을 하기 위한 manager
 방식 : Firebase에서 지원하는 의존성 주입을 사용하여, 미리 선언된 class 들에 의존성 주입
 */
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Threading.Tasks;
using UnityEngine;

public class BackendManager : MonoBehaviour, IManagerBooter, IBackendManager
{

    private FirebaseApp _app;
    /// <summary>
    /// Firebase App에 접근
    /// </summary>
    public FirebaseApp App => _app;

    private FirebaseAuth _auth;
    /// <summary>
    /// Firebase Auth에 접근
    /// </summary>
    public FirebaseAuth Auth => _auth;

    private FirebaseDatabase _database;
    /// <summary>
    /// Firebase DB에 접근
    /// </summary>
    public FirebaseDatabase Database => _database;

    private FirebaseFirestore _firestore;
    /// <summary>
    /// Firebase store에 접근
    /// </summary>
    public FirebaseFirestore Firestore => _firestore;

    private static readonly TaskCompletionSource<bool> _readyTcs = new(false);
    /// <summary>
    /// 초기화 후 접근이 필요한 경우를 위한 비동기 변수
    /// </summary>
    public Task<bool> ReadyTask => _readyTcs.Task;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            bool isAvailable = task.Result == DependencyStatus.Available; // 자동 로그인을 위해 변경 되었음.

            if (isAvailable) // 첫 로그인 시 백엔드가 대기 없이 진행 되는 것이 문제 인듯 "재형님 말씀".
            {
                _app = FirebaseApp.DefaultInstance;
                _auth = FirebaseAuth.DefaultInstance;
                _database = FirebaseDatabase.DefaultInstance;
                _firestore = FirebaseFirestore.DefaultInstance;

                _firestore.Settings.PersistenceEnabled = false;

                Log.Message("Firebase 의존성 주입 완료");
            }
            else
            {
                Log.Message($"Firebase 의존성 주입 실패, 사유 : {task.Result}");
                _app = null;
                _auth = null;
                _database = null;
                _firestore = null;
            }
            _readyTcs.TrySetResult(isAvailable);
        });
        ServiceLocator.Register<IBackendManager>(this);
    }

    public void Register()
    {
        ServiceLocator.Register<IBackendManager>(this);
    }

    public void UnRegister() => ServiceLocator.UnRegister<IBackendManager>(this);
}