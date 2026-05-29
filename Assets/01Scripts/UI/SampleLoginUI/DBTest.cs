/*
 작성자 : cminhyeong1999
 작성일 : 26-05-29

 역할 : Realtime Database가 실제로 Read/Write가 가능한지를 검증하기 위한 Test UI script
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DBTest : MonoBehaviour
{
    private IBackendManager _backendManager;
    private FirebaseDatabase _db;
    private RealTimeUserDatas _realTimeUserDatas;
    
    [SerializeField] private Button _readButton;
    [SerializeField] private Button _writeButton;
    [SerializeField] private TextMeshProUGUI _readText;
    [SerializeField] private TextMeshProUGUI _writeText;
    
    [SerializeField] private TMP_InputField _energyInputField;
    [SerializeField] private TMP_InputField _coinInputField;
    [SerializeField] private TMP_InputField _gemInputField;
    [SerializeField] private TMP_InputField _stoneInputField;
    [SerializeField] private TMP_InputField _furDollInputField;
    [SerializeField] private TMP_InputField _ClawInputField;

    private async void Start() => await Init();
    private void OnEnable() => BindButtonEvents();
    private void OnDisable() => UnbindButtonEvents();

    private async Task Init()
    {
        try
        {
            _backendManager = ServiceLocator.Get<IBackendManager>();
            bool isBackendReady = await _backendManager.ReadyTask;

            if (isBackendReady)
            {
                Debug.Log("Firebase 준비 완료");
                _db = _backendManager.Database;
            }
            else
            {
                Debug.Log("Firebase 준비 실패");
            }
        }
        catch (Exception e)
        {
            Debug.Log($"서비스 로케이터 에러 또는 초기화 실패: {e.Message}");
            throw;
        }
    }
    
    private void BindButtonEvents()
    {
        _readButton.onClick.AddListener(OnReadClicked);
        _writeButton.onClick.AddListener(OnWriteClicked);
    }

    private void UnbindButtonEvents()
    {
        _readButton.onClick.RemoveListener(OnReadClicked);
        _writeButton.onClick.RemoveListener(OnWriteClicked);
    }

    private void OnReadClicked()
    {
        ReadData();
    }
    
    ///<summary>
    ///각 Input field에 값을 넣은 후 Write 버튼을 클릭 시 Realtime Database에 데이터를 Write 시도
    ///</summary>
    private void OnWriteClicked()
    {
        RealTimeUserDatas temp = new();
        temp.Energy = Int32.Parse(_energyInputField.text);
        temp.Coin = Int32.Parse(_coinInputField.text);
        temp.Gem = Int32.Parse(_gemInputField.text);
        temp.Stone = Int32.Parse(_stoneInputField.text);
        temp.FurDoll = Int32.Parse(_furDollInputField.text);
        temp.Claw = Int32.Parse(_ClawInputField.text);
        
        WriteData(temp);
    }

    private async void WriteData(RealTimeUserDatas data)
    {
        DatabaseReference dbRef = _db.GetReference("user/user2");
        
        try
        {
            // Write할 데이터를 Json화
            string jsonText = JsonUtility.ToJson(data);
            // 실제로 데이터를 Write
            await dbRef.SetRawJsonValueAsync(jsonText);

            Log.Message("DB 데이터 저장 성공!");
            // 어떤 데이터를 Write 했는지 확인하기 위한 코드
            _readText.text = data.ToString();
        }
        catch (System.Exception e)
        {
            Log.MessageColor($"쓰기 중 실패 또는 취소 되었습니다. {e.Message}",Color.red);
        }
    }
    
    public void ReadData()
    {
        DatabaseReference dbRef = _db.GetReference("user/user2");
        
        dbRef.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    Log.MessageColor($"읽기 중 실패 또는 취소 되었습니다.",Color.red);
                    return;
                }

                // Read할 데이터
                DataSnapshot snapshot = task.Result;

                bool isFind = false;

                Log.Message($"식별된 UID : {snapshot.Key}");
                try
                {
                    // Read할 데이터 Json화
                    string jsonText = snapshot.GetRawJsonValue();
                    // Json화한 데이터를 클라이언트에 저장
                    _realTimeUserDatas = JsonUtility.FromJson<RealTimeUserDatas>(jsonText);
                    Log.Message($"{snapshot.Key} 등록 완료");
                    isFind = true;
                    // 어떤 데이터를 Read 했는지 확인용 코드
                    _readText.text = _realTimeUserDatas.ToString();
                }
                catch (System.Exception e)
                {
                    Log.Message(e.Message);
                    _readText.text = "Fail... " + e.Message;
                }

                if (!isFind)
                {
                    Log.Message($"해당 UID를 찾을 수 없습니다.");
                }
            }
        );
    }
}
