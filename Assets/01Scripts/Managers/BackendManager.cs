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
    public FirebaseApp App => _app;

    private FirebaseAuth _auth;
    public FirebaseAuth Auth => _auth;

    private FirebaseDatabase _database;
    public FirebaseDatabase Database => _database;

    private FirebaseFirestore _firestore;
    public FirebaseFirestore Firestore => _firestore;

    private static readonly TaskCompletionSource<bool> _readyTcs = new();
    public Task<bool> ReadyTask => _readyTcs.Task;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            bool isAvailable = task.Result == DependencyStatus.Available; // 자동 로그인을 위해 변경 되었음.

            if (isAvailable)
            {
                _app = FirebaseApp.DefaultInstance;
                _auth = FirebaseAuth.DefaultInstance;
                _database = FirebaseDatabase.DefaultInstance;
                _firestore = FirebaseFirestore.DefaultInstance;

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
    }

    public void Register()
    {
        ServiceLocator.Register<IBackendManager>(this);
    }

    public void UnRegister()
    {
        ServiceLocator.UnRegister<IBackendManager>(this);
    }
}