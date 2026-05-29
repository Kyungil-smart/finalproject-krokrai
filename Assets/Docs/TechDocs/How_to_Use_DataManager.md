# firebase store 사용법
- Data를 직접 제어하는 곳
  - ServiceLocator (실제로는 Bootstraper가 Service Locator에 대신 등록 사유 : 초기화 순서가 중요해서)
    - ㄴBackendManager
    - ㄴDataManager

- 자동으로 처리해주는 곳 (참고만)
  - GoogleSignInService
  - LoginUI
  - DataAutoSaveManager

- Data 구조
  - UserDatas
    - ProFile
    - Event_Missions
      - EventState
    - Attendance

## 의도
- firebase에 접근 간소화를 위한 Data Manager 생성

## 방식
- **DataManager의 임의의 SaveData() 호출 금지.**
- Service Locator를 이용하여 Backend Manager와 Data Manager에 접근
- Backend Manager는 Firebase에서 정보를 갖고와서 보유
- Data Manager는 Backend Manager가 갖고온 정보 중 Store와 RTDB에 접근하여 유저 정보 찾기, 생성, 초기화 진행
- Firestore에 저장된 유저 정보들은 UserDatas에 순수 C# class로 저장되며, Data Manager에서 ConvertTo<UserDatas>() 방식으로 Firebase 쪽에서 직접 주입(정보가 있는 상태일 경우)
  - 없는 경우 초기 상태 생성 및 Firestore에 등록
- Event_Missions의 경우 많은 양의 데이터가 뭉쳐 있어 Dictionary<string,Dictionary<string,EventState>> Event_490(이벤트 이름) 형식
  - Event_490의 내부에 init()의 경우 초기 값으로 만들기 때문에 호출 금지.
- DataAutoSaveManager가 첫 Save 요청이 들어온 경우 1.5초 내에 추가적인 Save 요청이 없는 경우 그 때 DataManager의 Save를 요청함.

## 사용법
- 호출 가능한 함수
```csharp
UserDatas // 가장 최상위 Root로 대부분에서는 필요 X

Attendance // Attendance Data에 접근
Event_Missions  // Event_Missions에 접근하여 Event_490 2중첩 Dictionary에 접근 가능
ProFile // ProFile Data에 접근 가능

```

- 출석 이벤트 작성인 경우
```csharp

class Test : MonoBehaviour
{
    void SetMissionState()
    {
        ServiceLocator.Get<IDataManager>().Event_Missions.Event490["Day_1"]["49011"].Mission_State = 1;
    }

    void SetName()
    {
        ServiceLocator.Get<IDataManager>().ProFile.NickName = "Hi"
    }

    // 수동 저장이 필요한 경우;
    void ManualSave()
    {
        ServiceLocator.Get<IDataAutoSaveManager>.RequestSave();
    }
}
```

## 주의점
- **DataManager의 SaveData()를 임의로 호출하지 마세요. 절대로**