![Typing SVG](https://readme-typing-svg.demolab.com?font=Fira+Code&size=50&pause=1000&width=600&height=70&lines=SUNSET+SURVIVAL+🎮)

# 🛠️ Description
- **프로젝트 소개** <br>
Sunset Survival은 **Unity 6**와 **FishNet** 네트워킹 프레임워크를 기반으로 개발한 **비대칭 멀티플레이어 서바이벌 게임**입니다. 플레이어는 **생존자** 또는 **살인마** 역할을 맡아 긴장감 넘치는 심리전과 전략적 게임플레이를 경험합니다. <br>
**직접 제작한 사운드**를 통해 높은 몰입감을 구현하였으며, **아이템을 파밍하고 제작**하여 **퀘스트**를 클리어하는 구조로 게임이 진행됩니다.

- **개발 기간** : 2025.04.22 ~ 2025.05.28
- **개발 인원** : 3인 개발
- **사용 기술** <br>

  | 언어 | 엔진 | 네트워킹 프레임워크 | 데이터 연동 라이브러리 |
  |----|----|----|----|
  | C# | Unity 6 | FishNet | Unity Google Sheet | 

<br><br>


---
# 🎮 시연 영상 (이미지 클릭시 유튜브 링크로 이동합니다)

<a href="https://youtu.be/7PB0w4Xyckg" target="_blank">
  <img src="https://github.com/user-attachments/assets/0ef01b59-40c5-4e87-8fb4-7c4bd0e09097" 
       alt="YouTube Video" 
       width="700"
</a>


<br><br>
<br><br>
<br><br>

---


# 🤝 팀원(역할 분담)

| 염기용 | 박관우 | 김영송 |
|----|----|----|
| 플레이어 로직 | 사운드 제작 및 적용 | 개발 프레임워크 |
| 네트워크 | 채팅 UI | UI |

<br><br>
<br><br>

---
# 📁 프로젝트 구조

```
Assets/
├── 0. External              - 외부 에셋 및 라이브러리
├── 1. Scene                 - 게임 씬 파일
├── 2. AddressableAssets     - 어드레서블 에셋
├── 3. Font                  - 게임 폰트
├── 4. Animation             - 애니메이션 및 컨트롤러
├── 5. Datas                 - SO 에셋 인스턴스 및 각종 데이터
├── 6. Prefab                - 프리팹 객체
├── Scripts                  - 게임 스크립트
│   ├── 1. Entity            - 게임 엔티티 관련
│   ├── 2. Controller        - 컨트롤러
│   ├── 3. Handler           - 핸들러 클래스
│   ├── 4. Manager           - 매니저 클래스
│   ├── 5. UI                - UI 스크립트
│   ├── 6. Scene             - 씬 관련 스크립트
│   ├── 7. Data              - 데이터 스크립트
│   ├── Editor               - 에디터 스크립트
│   ├── Steamworks.NET       - Steam 통합
│   └── Utils                - 유틸리티 클래스
├── Resources/               - 리소스 파일
└── UGS.Generated/           - Unity Gaming Services 생성 파일
```

<br><br>
<br><br>

---

# 🎮핵심 구현 요소
### 인벤토리
<img src="https://github.com/user-attachments/assets/3a070e9d-059a-417d-ac94-51bcb63199a4" alt="Inventory_퀄리티중" width="600">

- **핵심 기능** <br>
  아이템 드래그 & 드롭 <br>
  슬롯 합치기 <br>
  아이템 버리기 <br>
  아이템 타입별로 보기 <br>
  
- 인벤토리 데이터와 UI를 나누어서 설계했습니다. Event 기반으로 인벤토리 데이터가 변경되면 UI로 출력되도록 했습니다.

<br><br>

---

### 상호작용 시스템
<img src="https://github.com/user-attachments/assets/78aec45b-09d9-42bd-b825-3d26fd4b564c" alt="Inventory_퀄리티중" width="600">

- **핵심 기능** <br>
  근처의 상호작용이 가능한 물건을 탐지 <br>
  상호작용이 가능한 상태의 물건에 외각선 표사 <br>

- 상호작용 키를 누르면 상호작용 애니메이션이 실행되고 상호작용이 되도록 설계 했습니다. <br>
- 또한 셰이더를 통해 외각선이 보이게하여 직관적으로 접근할 수 있도록 했습니다. <br>
- 근처의 오브젝트 탐지시 Unity의 OnTriggerEnter와 OnTriggerExit 함수를 활용했으며, <br>
  HashSet을 통해 오브젝트를 관리하여 중복 감지를 예방 했습니다. <br>
- 아이템 줍기의 경우 네트워크상에서 한명만 습득이 가능하도록 했습니다.


- **소스 코드** <br>
  ```csharp
  public class InteractionHandler : MonoBehaviour
  {
      private HashSet<Interactable> interactables = new HashSet<Interactable>();
      
      public Interactable GetInteractObject(bool Remove = false)
      {
          // 거리 기반 우선순위 정렬
          // 인벤토리 공간 체크
          // 아이템 타입별 상호작용 가능 여부 확인
      }
  }
  ```

</details>

<details>
<summary>상호작용 가능 객체</summary>

  | 객체 타입 | 설명 | 네트워크 동기화 |
  |----------|------|-----------------|
  | `StorageBox` | 공유 저장소 | ✅ 실시간 동기화 |
  | `CraftingTable` | 제작대 | ✅ 제작 과정 동기화 |
  | `QuestStorageBox` | 퀘스트 아이템 보관함 | ✅ 진행도 공유 |
  | `ItemObject` | 드롭된 아이템 | ✅ 픽업 즉시 동기화 |

</details>

<details>
<summary>상호작용 흐름도</summary>

  ```mermaid
  sequenceDiagram
      participant P as Player
      participant IH as InteractionHandler
      participant I as Interactable
      participant N as Network
      
      P->>IH: 상호작용 입력
      IH->>IH: 범위 내 객체 검색
      IH->>I: Interact() 호출
      I->>N: ServerRpc 전송
      N->>N: 모든 클라이언트 동기화
      N->>I: ObserversRpc 실행
  ```

</details>

<br><br>

---

### 퀘스트 시스템
![퀘스트](https://github.com/user-attachments/assets/0d8a7c93-c93d-4b95-bfa5-59d5bf2b15d4)
![image](https://github.com/user-attachments/assets/85aca123-2ecc-48ab-bb04-0a8f72452aeb)
![image](https://github.com/user-attachments/assets/e4e8dde2-0dfb-45c4-9e26-9b7d3a6db86e)

- 미션 위주의 게임을 설계하기 위해 퀘스트 시스템을 만들었습니다. <br>
  데이터 테이블을 활용하여 퀘스트를 작성하였고 퀘스트와 퀘스트 작업을 나누어서 관리했습니다. <br>
  퀘스트가 여러개의 작업을 가질 수 있도록 구현했습니다. <br>
  또한 작업을 완료하면 연결된 다른 작업이 생성되는 기능을 linkedTask를 통해 설정할 수 있게 했습니다. <br>

- 퀘스트의 진행도를 올리는 설계 방법은 퀘스트의 행동이 발생하는 곳에서 <br>
  해당 행동의 카테고리와 행동의 주체가 되는 Object의 ID값을 통해 진행도를 업데이트 합니다.



<details>
<summary>📋 퀘스트 구조 코드</summary>

```csharp
public class Quest
{
    public QuestData QuestData { get; private set; }
    public List<Task> Tasks { get; private set; }
    public event Action<Quest> onCompleted;
}
```

#### 진행 추적 시스템
```csharp
public void ReceiveReport(ETaskCategory category, int targetId, int successCount = 1)
{
    foreach (var quest in activeQuests.ToArray())
        quest.RecieveReport(category, targetId, successCount);
}
```

</details>

<br><br>

---

### 제작 시스템
<img src="https://github.com/user-attachments/assets/7f2519ca-5774-483a-841e-c780fbab125c" alt="Inventory_퀄리티중" width="600">

![image](https://github.com/user-attachments/assets/7c14ca8f-8c75-4336-afd4-91b06a8ff588)

- 아이템을 제작하는 기능을 만들었습니다. <br>
  데이터 테이블을 통해 어떤 아이템을 어떤 재료로 만들 수 있는지 읽어드렸습니다. <br>
  인벤토리를 실시간으로 탐색하여 아이템을 만들 수 있는지, 또 몇개를 만들 수 있는지 검사하고, UI에 표시합니다 <br>
  인벤토리 탐색 속도를 개선하기 위해 아이템 ID를 Key로 해당 아이템이 있는 아이템 슬롯을 Value로 하여 Dictionary에 넣고 탐색하는 방식을 사용했습니다. <br>

  
<br><br>

---

### 보관함
<img src="https://github.com/user-attachments/assets/1d1afedc-491d-4a7c-87e2-8382ef60eef5" width="600">

- 아이템을 보관함을 통해 유저들과 공유할 수 있게 했습니다. <br>
  플레이어가 아이템을 옮겨서 보관함에 변화가 생기면 해당 정보를 모든 유저에게 전달하는 방식으로 구현했습니다.
  
<br><br>

---


### 퀘스트 보관함
<img src="https://github.com/user-attachments/assets/0252d63a-5a22-4388-8a8c-131587700d7b" width="600">

![image](https://github.com/user-attachments/assets/966ec0ea-2c9e-4ac5-9922-adbd502c5e73)

- 보관함에 아이템을 채우는 퀘스트를 만들었습니다. <br>
  데이터테이블을 통해 어떤 아이템을 채워넣어야하는지 설정할 수 있도록 설계했습니다. <br>
  보관함을 채우는 퀘스트의 경우 보관함 채우는 카테고리와 해당 보관함의 ID 값을 통해 퀘스트 진행도를 업데이트 합니다 <br>
  퀘스트 보관함에 아이템을 넣을 때, 네트워크상의 모든 유저에게 해당 정보를 공유하도록 하였습니다.
  
<br><br>

---

### 아이템 슬롯
<img src="https://github.com/user-attachments/assets/c030a94d-66b8-4040-be5f-477f09988965" width="600"> 
<br>

<img src="https://github.com/user-attachments/assets/b8e63de1-8e3d-4891-98aa-299503803606" width="600">
<br>

- 인벤토리, 퀵슬롯, 보관함, 제작대, 퀘스트 보관함 모두 같은 아이템 슬롯을 활용해서 구현했습니다. <br>
  슬롯에 담고 있는 아이템에 따라 배경색이 달라지도록 구현했으며, <br>
  마우스 호버시 아이템의 정보가 출력되도록 했습니다. 아이템에 효과가 있다면 해당 정보도 함께 출력됩니다. <br>
  무기의 경우 내구도가 표시되도록 했습니다. <br>

<br><br>

<div align="center"> 감사합니다! </div>
