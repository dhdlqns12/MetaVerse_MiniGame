# MetaVerseMiniGame

## 유니티 입문 개인과제
<img width="1116" height="622" alt="Image" src="https://github.com/user-attachments/assets/5fa1eb7d-e30b-4195-b504-5775aa4ffc71" />

## 1. 프로젝트 소개
메타버스 게임 상에서 미니게임을 플레이하여 점수를 갱신하고 돈을 모아 꾸며보는 간단한 프로젝트입니다.

---

### **개발 환경**
- **Engine**: Unity 2022.3.62f2
- **Language**: C# 
- **IDE**: Visual Studio

## 2. 프로젝트 구현 내용
 # 필수 과제

### 1. 캐릭터 이동 및 탐색
- 캐릭터의 자유로운 이동 구현
- 맵 내 탐색 기능

### 2. 맵 설계 및 상호작용 영역
- 게임 맵 디자인
- 플레이어와 상호작용 가능한 영역 구현

### 3. 미니 게임 실행
- 다양한 미니 게임 구현
- 게임 실행 메커니즘

### 4. 점수 시스템
- 점수 획득 및 관리 시스템
- 점수 표시 UI

### 5. 게임 종료 및 복귀
- 게임 종료 기능
- 메인 화면 복귀 기능



 # 도전 과제

### 1. 추가 미니 게임
- 기본 미니 게임 외 추가 게임 콘텐츠

### 2. 커스텀 캐릭터
- 캐릭터 커스터마이징 기능

### 3. 리더보드 시스템
- 최고 점수 기록
- 순위 표시 시스템

### 4. NPC 대화 시스템
- NPC와의 상호작용
- 대화 인터페이스 구현

### 5. 탑승물 제작
- 이동 수단 구현
- 탑승물 활용 시스템

---
  
## 3. 사용 기술

### 전략 패턴 (Strategy Pattern)
- 플레이어 행동 모드 전환 (메타버스/미니게임 등)
- `IPlayerStrategy' 인터페이스로 전략 정의
- 런타임에 동적으로 전략 교체 가능

 <img width="587" height="347" alt="Image" src="https://github.com/user-attachments/assets/3aba0b3a-2b51-48f0-a668-1b65779e43b9" />

### 제네릭 싱글턴 패턴
- 게임 전역에서 접근 가능한 매니저 클래스 구현
- 코드 재사용성 및 유지보수성 향상

<img width="640" height="677" alt="Image" src="https://github.com/user-attachments/assets/afd2fad0-793a-465e-a81f-da112a4a4775" />

### Json을 통한 데이터 관리
- 게임 데이터의 직렬화 및 역직렬화
- 유연한 데이터 저장 및 로드 시스템

<img width="200" height="400" alt="Image" src="https://github.com/user-attachments/assets/6fbb1564-7421-4945-8f4d-7239df05b36f" />
<img width="400" height="400" alt="Image" src="https://github.com/user-attachments/assets/31b347c8-c668-4d4c-b7d2-1c66c973b3d4" />
<img width="200" height="186" alt="Image" src="https://github.com/user-attachments/assets/99990f17-35c3-40e6-beab-ed5fee4d2474" />

### InputSystem을 통한 입력 처리
- Unity New Input System 활용
- 여러개의 액션 맵 활용
<img width="800" height="600" alt="Image" src="https://github.com/user-attachments/assets/6518d3d9-6df9-4c9b-a61a-a1211ae843eb" />

### 커스터마이징 시스템
- 런타임 중 동적으로 스프라이트 및 애니메이션 교체
- 실시간 캐릭터 외형 변경

  <img width="661" height="395" alt="Image" src="https://github.com/user-attachments/assets/caceb217-83cc-4a03-93f1-8139dbbe0a52" />
  <img width="647" height="347" alt="Image" src="https://github.com/user-attachments/assets/0278ea35-c7fc-49bc-9ec0-aeb1c49f5f2c" />

---

## 4. 인게임 스크린샷
<img width="1000" height="600" alt="Image" src="https://github.com/user-attachments/assets/e6949597-ba65-4484-8f41-1135c3d8e602" />
<img width="1000" height="600" alt="Image" src="https://github.com/user-attachments/assets/ecf3ec9a-b8b3-4f89-85c4-443ab593e15e" />
<img width="1000" height="600" alt="Image" src="https://github.com/user-attachments/assets/0bdedeb7-7bdd-4579-9004-82bd671b0b43" />
<img width="1000" height="600" alt="Image" src="https://github.com/user-attachments/assets/806f1d5a-42d5-468b-9ede-72c172625f15" />
<img width="1000" height="600" alt="Image" src="https://github.com/user-attachments/assets/a12bb479-6a52-486e-87d5-49e6cc4ea7a3" />
<img width="1000" height="600" alt="Image" src="https://github.com/user-attachments/assets/6bd60f08-6a8e-43ec-987a-d1551f7e610a" />
<img width="1000" height="600" alt="Image" src="https://github.com/user-attachments/assets/2e950690-5451-48c9-9506-6e96aa84ab9a" />
<img width="1000" height="600" alt="Image" src="https://github.com/user-attachments/assets/56da57d9-70a6-4ce8-a6d7-1636600d44e5" />

---

## 5. 명시사항
\AppData\LocalLow\DefaultCompany\4week Project\SaveData 경로에 세이브 데이터들이 저장되니 테스트 후 삭제 필요
미니게임 씬 개별로 동작 불가 Main씬에서 시작 필요
