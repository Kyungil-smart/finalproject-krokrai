# 기업 협약 프로젝트 플머 규칙 및 시간표
## 시간표
- 09 : 20 ~ 10 : 00 / 오전 기획자 회의 참석(없는 경우 플머끼리 간단 회의)
- 10 : 00 ~ 13 : 10 / 개인 분담 역할 실시
- 13 : 10 ~ 14 : 10 / 점심
- 14 : 10 ~ 15 : 30 / 플머 중간 점검(기획쪽 의견에 따라 변경될 수 있음)
- 15 : 30 ~ 16 : 00 / 개인 분담 역할 실시
- 16 : 00 ~ 16 : 30
- 16 : 30 ~ 17 : 30 / 개인 작업 마무리
- 17 : 30 ~ 18 : 00 / 마무리 회의

## 플머 규칙
### 개인 단위 규칙
1. 기획쪽 전달 사항은 반드시 이슈/질문에 등록
2. 깃 및 네이밍 컨벤션 반드시 준수
   - 미준수 branch merge 거부
   - 팀장의 허가 사항인 경우 예외
3. 도움이 필요한 상황에서 반드시 플머 팀장에게 우선 보고
4. **절대 남에 작업물 허락 없이 건들지 말 것**
5. 모든 Asset 및 Package, .png, .wave 등 Project에 필요한 file들은 **전부 팀장에게 전달 및 팀장이 배포**
   - TextMeshPro 같은 것도 절대 **스스로 받아서 사용 금지**
   - Git에 올라가는 위험성(**유료 Asset의 경우 특히 위험**) 및 각자가 받을 경우 **UID가 달라져 Missing으로 인한 일 2번**하게 될 수 있음
6. Live Scene에 올라가야 되는 **모든 객체들은 Prefab으로 생성** 및 Prefab 폴더에 넣기
7. Imports 내부를 제외한 모든 곳에서 folder 생성 필요 시 반드시 모든 팀원에게 전파 후 생성
   - folder 생성 후 하위 folder는 자유롭게 생성, 단, Git에 올라간 후에 생성인 경우 다른 팀원이 작업 중인지 확인 후 생성
8. 기술 문서 script 단위로 작성, **script 작성 -> 검증 -> 기술 문서 작성 -> 팀장 검토 -> merge**
   - 단, interface, enum은 작성 X
   - ScriptableObject은 단순 저장용이면 작성하지 않으나, 함수를 포함할 시 주석 혹은 기술 문서 작성
     - 주석만 작성하는 경우 : 복사자 혹은 생성자를 이용한 데이터 초기화 / 데이터를 함수 단위로 받아와 저장하는 경우

### 기본 폴더 구조
- Scene을 제외한 모든 file들은 folder 생성 후 folder 내부에서 file 생성

- Assets
  - ㄴ01Scripts
    - ㄴBaseScripts
    - ㄴData
      - ㄴSO
      - ㄴSOScripts
    - ㄴInterfaces
    - ㄴManagers
    - ㄴUI
  - ㄴ02Scenes
    - ㄴLive / **절대 내부에 있는 거 건들지 말 것**
    - ㄴPersonalScene / 개인 단위 Scene은 여기서 생성 및 사용, **이 외에 곳에 Scene 만들기 금지**
      - ㄴ필요시 폴더 생성 후 사용
  - ㄴ03Prefab
    - ㄴUI
  - ㄴdocs
    - ㄴRule / 규칙 관련 문서
    - ㄴTechDocs / 여기에 기술 문서 업로드
      - ㄴManager
        - ㄴEventManager.md / **실제 문서는 상위**에 두고
        - ㄴEventManager / **image 등**이 첨부된 경우에는 **folder를 만든 후 내부에 저장**
      - ㄴPlayer
  - ㄴImports
    - ㄴTempFile / 단순 테스트용 파일(.png, .wave 등)들, 필요에 따라서 **각자 만들고 사용**

### 깃
#### branch
- 모든 branch는 **소문자로 작성**
  - feature/name : 기능 추가 branch이며, name은 어떤 기능을 추가할 건지
  - fix/name : 기능 수정 branch이며, name은 어떤 기능을 수정할 것인지
  - refactor/name : 성능 개선 혹은 구조 개선 branch 이며, name은 어떤 기능을 개선할 것인지

#### commit
  - 최대한 작은 단위로 남기는 것이 좋으나, 어려운 것도 알고 있으니, 놓친 경우 **body에 추가 작성**
  - header
    - type : 제목
    - type
      - fix : 버그 등의 문제로 작동이 안되어 수정한 경우
      - feat : 새로운 기능 추가
      - style : 변수, 함수 등의 이름을 바꾼 경우 (기능에 변화가 없는 경우)
      - docs : project 내부에 문서를 수정한 경우
      - refac : 기존 구조를 개선한 경우
      - chore : 외 사항
      - test : merge 전 test 완료 commite
      - release : 출시를 위한 build
      - ! : public 함수 등의 경우 단순 이름 변경 사항으로도 문제 발생 여지가 있으니, **기존 생성된 script의 public 함수 변경 시** `type ! :` 형식으로 **반드시 명시**
    - 예시 / **영어가 어려운 경우 한글**로 추가
      -  feat : playermovement 추가
      -  fix : playermovement의 이동 불가 해결
      -  style : 변수 playermovement/movespeed -> playermovement/_movespeed
      -  docs : player 기술 문서에 player attack 추가
      -  test : playermovement 검증 완료
      -  chore : asset/playermovement -> 01script/player/playermovement
      -  chore : playermove -> playermovement
- body
  - 생략 가능하며, **필요한 경우에만 사용**
  - 여러 수정 사항이 있는 경우 header와 같은 양식으로 남기며, **issues에 보고된 사항인 경우 마지막 줄에 fix : #번호, #번호로 남깁니다.**
  - 혹은 단순 header만으로 설명이 어려운 사항인 경우 추가로 작성합니다. 특히 **refac, test의 경우에는 바뀐 내용 및 어떤 것을 검증했는 지 명시**합니다.
![예시1](ProjectRole/commitEx1.PNG)
![예시2](ProjectRole/commitEx2.PNG)
![예시2](ProjectRole/commitEx3.PNG)

### 코드
#### 변수
- private : _camalCase
- protected : _camalCase
- internal : _camalCase
- public : camalCase

- 예시
  - _isMove
  - healthPoint

#### 함수
- private : PascalCase
- protected : PascalCase
- internal : PascalCase
- public : PascalCase
- property : PascalCase

- 예시
  - Move();
  - TakeDamage(int dmg);

#### class, interface 등
- interface : I + PascalCase
- enum : PascalCase + Enum
  - enum 내부에 선언된 문자열은 모두 대문자로 표기
- ScriptableObject : PascalCase + SO
- abstract : Base + PascalCase

- struct : PascalCase
- class : PascalCase

- 예시
  - IDamageable
  - TeamNumEnum
  - EnemySO
    - SLIME, OCK
  - BaseWeapone

#### 주석
- **코드 주석은 반드시** `//`로만 주석
- **public 함수는 반드시** `///<summary> </summary>` 사용
  - 내부에는 사용법 및 어떤 인자를 어떻게 넘겨 주어야 하는 지 명시 및 작성 시 짧고 간결한 말투로 작성
  - 예시
```csharp
///<summary>
///dmg에는 피해량을 넣어준 후 team에는 공격 주체 기입
///</summary>
public void Attack(int dmg, TeamNumEnum team)
{
  // 공격
}
```
- script 최 상위에 반드시 `/* */`를 사용해 아래 양식에 맞게 작성
  - 작성자 : 이름, 깃 닉네임 중 1 택
  - 수정자 : 이름, 깃 닉네임 중 1 택
    - 작성자가 아닌 다른 사람이 해당 코드를 수정한 경우에만 추가 작성하며, 마지막 수정자(작성자가 아닌 사람)만 기록.
  - 최초 작성일 (yy-mm-dd 형식)
  - 수정 날자 (yy-mm-dd 형식) / 첫 작성인 경우 빈칸 혹은 작성일과 동일하게
  - 해당 script의 역할 및 방식(혹은 의도)
    - SO나 enum의 경우 방식까지는 작성할 필요가 없다고 판단하여 선택 사항으로 변경(26-05-21)
  - (선택사항) script의 자세한 설명 작성

- 예시
```csharp
/*
작성자 : krokrai
수정자 : 
작성일 : 26-05-17
수정일 : 26-05-17

역할 : 플레이어의 움직임을 담당
방식 : Rigidboy를 이용한 물리 기반 움직임
*/
```