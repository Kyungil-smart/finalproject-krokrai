# Addressables Manager R&D

## 개요

Unity Addressables 시스템을 활용하여 리소스를 주소 기반으로 관리하고,  
프로젝트 내에서 공통적으로 사용할 수 있는 AddressableManager 구조를 설계하고 구현.

Addressables API를 여러 곳에서 직접 호출하지 않고 Manager에서 공통 관리하도록 구성하여  
리소스 로드 및 메모리 관리 흐름을 통합하는 것을 목표로 진행.

---

# Addressables 도입 목적

기존 Resources.Load 방식은 리소스 참조 및 메모리 관리 측면에서 한계가 존재함.

Addressables는 다음과 같은 장점이 있어 프로젝트 구조 개선을 위해 도입.

- Address(Key) 기반 리소스 관리
- 비동기 로드 지원
- 메모리 Release 가능
- 리소스 의존성 관리 가능
- 프로젝트 규모 증가 시 유지보수에 유리

---

# 구조 설계

Addressables 관련 코드를 프로젝트 전체에 분산시키지 않고  
AddressableManager에서 공통 관리하도록 설계.

사용 예시:

```csharp
addressableManager.InstantiatePrefab("TilePrefab", Vector3.zero);
```

---

# 주요 기능

## Prefab Load & Instantiate

```csharp
public void InstantiatePrefab(string key, Vector3 position)
{
    Addressables.InstantiateAsync(key, position, Quaternion.identity);
}
```

Address(Key)를 기반으로 Prefab을 비동기 로드 후 생성.

---

## Sprite Load

```csharp
public void LoadSprite(string key)
{
    Addressables.LoadAssetAsync<Sprite>(key);
}
```

UI Sprite 등의 리소스를 Address(Key) 기반으로 로드.

---

## Resource Release

```csharp
public void ReleaseInstance(GameObject obj)
{
    Addressables.ReleaseInstance(obj);
}
```

생성된 Addressable Instance를 해제하여 메모리 관리 가능하도록 구성.

---

# 학습 및 적용 내용

- Addressables 패키지 구조 및 사용 방식 학습
- Addressable 등록 및 Key 기반 리소스 관리
- AsyncOperationHandle 기반 비동기 로드 방식 이해
- InstantiateAsync를 활용한 Prefab 생성 구조 확인
- Release를 통한 메모리 관리 방식 학습
- Manager 구조를 통한 Addressables 코드 공통화

---

# 기대 효과

- Addressables 관련 코드 중앙 관리 가능
- 리소스 로드 방식 통일
- 중복 코드 감소
- 유지보수성 향상
- 메모리 관리 흐름 통합 가능

---

# 참고 문서

- [Unity Addressables 공식 문서](https://docs.unity3d.com/Packages/com.unity.addressables@latest/)