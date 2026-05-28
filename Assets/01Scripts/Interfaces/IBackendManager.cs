/*
 작성자 : krokrai
 작성일 : 26-05-28

 역할 : Backend Manager를 Service locator에 등록
*/ 

using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Firestore;
using System.Threading.Tasks;

public interface IBackendManager
{

    public FirebaseApp App { get; }
    public FirebaseAuth Auth { get; }
    public FirebaseDatabase Database { get; }
    public FirebaseFirestore Firestore { get; }
    public Task<bool> ReadyTask { get; }
}
