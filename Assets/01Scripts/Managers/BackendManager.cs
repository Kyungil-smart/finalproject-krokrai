using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BackendManager : MonoBehaviour
{
    public static BackendManager Instance { get; private set; }

    private FirebaseApp _app;
    public static FirebaseApp App => Instance._app;

    private FirebaseAuth _auth;
    public static FirebaseAuth Auth => Instance._auth;

    private FirebaseDatabase _database;
    public static FirebaseDatabase Database => Instance._database;

    private FirebaseFirestore _firestore;
    public static FirebaseFirestore Firestore => Instance._firestore;

    private static readonly TaskCompletionSource<bool> _readyTcs = new();
    public static Task<bool> ReadyTask => _readyTcs.Task;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

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
}