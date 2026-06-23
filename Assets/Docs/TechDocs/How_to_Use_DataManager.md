# firebase store 사용법
- Data를 직접 제어하는 곳
  - ServiceLocator (실제로는 Bootstraper가 Service Locator에 대신 등록 사유 : 초기화 순서가 중요해서)
    - ㄴBackendManager
    - ㄴDataManager
      - 테스트만 진행하는 경우 DataManager의 inspector에 Test Mode 체크

- 자동으로 처리해주는 곳 (참고만)
  - GoogleSignInService
  - LoginUI
  - DataAutoSaveManager

- Data 구조
  - UserDatas / Store
    - ProFile
    - Event_Missions
      - EventState
    - Attendance
  - UserGoods / RTDB

## 의도
- firebase에 접근 간소화를 위한 Data Manager 생성

## 방식
- **DataManager의 임의의 SaveData(), SaveRTDBData() 호출 금지.**
- Service Locator를 이용하여 Backend Manager와 Data Manager에 접근
- Backend Manager는 Firebase에서 정보를 갖고와서 보유
- Data Manager는 Backend Manager가 갖고온 정보 중 Store와 RTDB에 접근하여 유저 정보 찾기, 생성, 초기화 진행
- Firestore에 저장된 유저 정보들은 UserDatas에 순수 C# class로 저장되며, Data Manager에서 ConvertTo<UserDatas>() 방식으로 Firebase 쪽에서 직접 주입(정보가 있는 상태일 경우)
  - 없는 경우 초기 상태 생성 및 Firestore에 등록
  - RTDB도 유사한 방식으로 작동
- Event_Missions의 경우 많은 양의 데이터가 뭉쳐 있어 Dictionary<string,Dictionary<string,EventState>> Event_490(이벤트 이름) 형식
  - Event_490의 내부에 init()의 경우 초기 값으로 만들기 때문에 호출 금지.
- DataAutoSaveManager가 첫 Save 요청이 들어온 경우 1초 내에 추가적인 Save 요청이 없는 경우 그 때 DataManager의 Save를 요청함.
  - RTDB는 0.5초

## 사용법
- 호출 가능한 함수
```csharp
UserDatas // 가장 최상위 Root로 대부분에서는 필요 X

Attendance // Attendance Data에 접근
Event_Missions  // Event_Missions에 접근하여 Event_490 2중첩 Dictionary에 접근 가능
ProFile // ProFile Data에 접근 가능

UserGoods // 재화에 바로 접근
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

    void SetCoin()
    {
      // ' = ' 연산자 사용 시 덮어씌어 지게 되니 대입 연산자 금지
      SErviceLocator.Get<IDataManager>().UserGoods.Coin_ += 100; 
    }

    void SetCoin()
    {
      // ' = ' 연산자 사용 시 덮어씌어 지게 되니 대입 연산자 금지
      SErviceLocator.Get<IDataManager>().UserGoods.Coin_ -= 100;
    }

    // 수동 저장이 필요한 경우; 대부분의 상황에서 사용 X
    void ManualSave()
    {
      ServiceLocator.Get<IDataAutoSaveManager>.RequestSave(); // store save
      ServiceLocator.Get<IDataAutoSaveManager>.RequestRTDBSave(); // RTDB save
    }
}
```

## 주의점
- **DataManager의 SaveData()와 SaveRTDB()를 임의로 호출하지 마세요.** 대부분의 상황에서 즉각적인 강제 저장이 필요하지 않기 때문입니다.
- UserGoods쪽 변수에 대입 연산자 사용 시 값이 덮어씌어 질 수 있으니 필요한 경우가 아니면 사용하지 마세요.