# Tsv2SO
## 의도
- Tsv를 자동으로 파싱 후 SO에 등록
## 방식
- EditorWindow, Reflection, UI Toolkit
- UI Toolkit과 EditorWindow를 사용하여, 사용자의 접근성 및 직관성을 유지하며, 사용에 용의할 수 있게 GUI 제작
- Reflection을 사용해 동적으로 경로 추적 및 Generic에 Class를 주입
## 사용법
1. 유니티 상단 Window/TSV2SO 클릭
2. 경로 찾기를 눌러 Assets 또는 Assets 하위에 있는 폴더 지정
2. TSV 형식으로 생성된 파일을 TSV 칸에 입력
3. ScriptableObject를 상속 받은 Script 생성
   1. Sample 폴더 내부에 SampleSO 참고
   2. `[SOInject(index)] public Type name`형식으로 선언
      - 즉, 3번째 열이 int인 경우 `[SOInject(0)] public int number`
      - enum도 호환 되며, enum은 미리 선언되어 있어야함.
4. 생성할 SO 칸에 입력
5. 시작 줄 칸에 실제 데이터가 시작되는 줄(Sample.tsv 기준 3) 입력
6. 마지막 줄은 선택 사항이며, 미 입력 시 자동으로 tsv의 마지막 줄 까지 생성됌
   - 빈 문자열로 인해 필요 이상에 SO가 생성 되는 것을 막기 위해 명시적으로 입력 추천

## 주의점
- 선언된 SO에 변수는 반드시 public이여야하며, property는 입력 받지 않습니다.
- 초기 버전이라 불안정합니다.
- 이미 생성되어 있는 경우 덮어씌우면서 부족한 경우에 추가로 생성됩니다.
- 기획 변경 등으로 table 행의 갯수가 줄어든 경우 자동으로 삭제합니다.