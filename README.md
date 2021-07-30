# WEEK6_Unity

## Ch.8 Level Design

### AppleCatch

#### 8.1 게임 설계하기

##### 8.1.1 게임 기획하기

- 떨어지는 사과를 바구니로 받는 게임
- 사과를 받으면 점수가 늘어나고, 폭탄을 받으면 점수가 줄어든다.



##### 8.1.2 게임 리소스 생각하기

1. 화면에 놓일 오브젝트 모두 나열
   - 사과, 폭탄, 바구니, 무대, UI
2. 컨트롤러 스크립트
   - 사과 컨트롤러, 폭탄 컨트롤러, 바구니 컨트롤러
3. 제너레이터 스크립트
   - 아이템(사과, 폭탄) 제너레이터
4. 감독 스크립트
   - 득점과 제한 시간 UI
5. 스크립트 흐름
   - (사과, 폭탄 컨트롤러)
     - 사과와 폭탄을 화면 위에서 아래로 떨어뜨린다. 보이지 않으면 소멸시킨다.
   - (바구니 컨트롤러)
     - 탭한 곳으로 바구니를 이동시킨다.
   - (아이템 제너레이터)
     - 사과와 폭탄을 화면 윗부분에 생성.
     - 게임 진행 상황에 맞춰 생성 속도와 폭탄 비율을 변화시킨다.
   - (게임 씬 감독)
     - 사과를 잡으면 +100, 폭탄을 잡으면 얻은 점수를 절반으로 줄임.
     - 제한 시간은 60초부터 카운트다운하고 UI에 표시





#### 8.2 프로젝트와 씬 만들기

##### 8.2.1 프로젝트 만들기

- 템플릿 3D로 (AppleCatch)
- 프로젝트에 리소스 추가
- VSync 체크



##### 8.2.2 스마트폰용으로 설정

- Platform을 iOS / Android 설정
- 화면 크기 Landscape로 설정



##### 8.2.3 씬 저장하기

- (GameScene)으로 저장



##### 8.2.4 Lighting 설정

- Window -> Rendering -> Lighting
- Scene 탭의 Generate Lighting 클릭, Auto Generate 체크 해제



#### 8.3 바구니 움직이기

##### 8.3.1 무대 배치하기

- 무대 원점에 배치 : Scene 탭 -> (stage)를 드래그&드롭, POS (0, 0, 0)
- X축이 오른쪽 방향이 되도록 시점을 회전



##### 8.3.2 카메라의 위치와 각도 조절하기

- 무대를 내려다 볼 수 있도록 카메라의 위치와 각도 조절
  - (Main Camera) Pos(0, 3.8, -1.6) Rotation (60, 0, 0)



##### 8.3.3 라이트를 설정해 그림자 붙이기

- 떨어지는 위치에 그림자를 넣어 아이템이 어디에 떨어질지 나타낸다 -> 라이트 설정

- | 라이트 종류와 역할 |                                                |
  | ------------------ | ---------------------------------------------- |
  | Directional Light  | 태양광처럼 직선으로 한 곳을 빛을 비추는 라이트 |
  | Point Light        | 전체 방향으로 동등하게 빛을 비추는 라이트      |
  | Spotlight          | 특정 방향에서 방사상으로 빛을 비추는 라이트    |
  | Area Light         | 직사각의 평면에서 전체 방위로 투사되는 라이트  |
  - 베이크 : 라이트에 의한 빛과 그림자 정보를 미리 계산해 두고 텍스쳐에 새겨 두는 처리



- 그림자로 아이템의 낙하 지점을 표시하기 때문에 라이트 방향을 조절해야 한다.
  - (Directional Light) -> Rotation (90, 0, 0)
- 라이트 빛이 너무 강하기 때문에 라이트 빛을 약하게 줄인다.
  - (Directional Light) -> Light 항목의 Intensity (0.7)



##### 8.3.4 바구니 배치하기

1. 바구니 오브젝트 배치하기
   - (basket) -> 드래그&드롭 -> Pos(0, 0, 0)
   - 바구니 그림자 조절 : Edit -> Project Settings -> Quality -> Shadow Distance(30)



##### 8.3.5 바구니를 움직이는 스크립트 작성하기

2. 오브젝트 움직이는 스크립트 작성
   - 탭한 곳으로 바구니를 이동시키는 스크립트. 무대는 3x3 구역으로 나뉘며, 탭한 구역 중심으로 바구니를 배치할 것
   -   무대는 한 변의 길이가 3인 정방형 -> 탭 한 좌표가 (-1.5<X<-0.5) 일 때 X=-1.0

```c#
// Create -> C# Script -> (BasketController)
public class BasketController : MonoBehaviour
{
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
  			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
  			// 탭한 위치의 좌표를 ScreenPointToRay 로 월드 좌표로 변환
  			RaycastHit hit;  // 월드 좌표 광선이 stage와 충돌한 좌표
  			if (Physics.Raycast(ray, out hit, Mathf.Infinity)
  			// 광선이 stage와 충돌했는지 확인, out : 매서드 내 값을 채워 변수로 변환
  			// Raycast 매서드 안에서 광선이 stage와 충돌한 좌표를 hit.point 변수에 반환
  			{
  				// stage에 충돌한 좌표를 반올림하고 바구니 좌표에 대입
  				float x = Mathf.RoundToInt(hit.point.x);
  				float z = Mathf.RoundToInt(hit.point.z);
  				transform.position = new Vector3(x, 0, z);
  			}
 		}
	}
}
```

- Mathf.RoundToInt : 반올림 매서드

  

##### 8.3.6 바구니에 스크립트 적용하기

- (BasketController)를 (basket)에 적용



- 탭하여도 바구니가 이동하지 않는 버그
  - 무대에 콜라이더가 적용되어 있지 않기 때문. 카메라에서 나온 Ray가 무대에 닿지 않고 빠져나갔기 때문
  - (stage)에 콜라이더 컴포넌트를 적용한다
    - (stage) -> Add Component -> Physics -> Box Collider
    - Box Collider 항목의 Size (3, 0.1, 3)

- 바구니가 제대로 동작하지 않을 경우
  - Main Camera의 Tag가 Untagged로 설정된 경우
    -> (Main Camera) -> Tag 칸의 (Main Camera)를 선택





#### 8.4 아이템 떨어뜨리기

##### 8.4.1 아이템 배치하기

- (apple) Pos(-1, 3, 0)
- (bomb) Pos(1, 3, 0)



##### 8.4.2 아이템을 떨어뜨리는 스크립트 작성하기

- 낙하 속도 등을 미세하게 조정하고자 Physics를 사용하지 않고 작성

  ```c#
  // (ItemController)
  public class ItemController : Monobehaviour
  {
  	public float dropSpeed = -0.03f;  // 낙하 속도
  	
      void Update()
  	{
   		transform.Translate(0, this.dropSpeed, 0);  // y축으로 낙하 속도 만큼 이동
   		if (transform.position.y < -1.0f)  // 무대 아래로 내려가 보이지 않으면
   		{
              Destroy(gameObject); // 소멸
          }
  	}
  }
  ```

  

##### 8.4.3 아이템에 스크립트 적용하기

- (ItemController) -> (apple) & (bomb)에 적용





#### 8.5 아이템 받기

##### 8.5.1 바구니와 아이템 충돌 판정하기

- Physics를 이용하여 오브젝트들이 충돌했을 때 OnTriggerEnter 매서드를 호출
- 바구니와 아이템 모두 Collider 컴포넌트를 적용해야 하고, 바구니에 Rigidbody 컴포넌트를 적용해야 함
  - (apple) -> Add Componenet -> Physics -> Sphere Collider
  - (apple) -> Sphere Collider -> Center(0, 0.25, 0), Radius(0.25)
  - (bomb) -> Add Component -> Physics -> Sphere Collider
  - (bomb) -> Sphere Collider -> Center(0, 0.25, 0), Radius(0.25)
  - (basket) -> Add Component -> Physics -> Rigidbody
    - 바구니에 물리 연산을 적용하지 않음 -> Is Kinematic 체크
  - (basket) -> Add Component -> Physics -> Box Collider
    - 바구니와 아이템끼리 충돌 반응 필요하지 않음 -> Is Trigger 체크
  - (basket) -> Box Collider -> Center(0, 0.5, 0), Size(0.5, 0.1, 0.5)



##### 8.5.2 충돌 상황을 스크립트에서 감지하기

- OnTriggerEnter 매서드 추가

  ```c#
  // (BasketContriller 수정)
  public class Basketcontroller : Monobehaviour
  {
  	void OnTriggerEnter(Collider other)  // 충돌할 때
  	{
   		Debug.Log("잡았다!");
   		Destroy(other.gameObject);
          // 충돌 상대는 OnTriggerEnter 매서드의 매개변수로 전달되어 충돌 상대의 게임 오브젝트로 적용된 콜라이더를 소멸
  	}
  ...
  }
  ```

  

| 충돌할 때 호출되는 매서드 | 2D               | 3D             |
| ------------------------- | ---------------- | -------------- |
| 충돌 시작 시점            | OnTriggerEnter2D | OnTriggerEnter |
| 충돌 중                   | OnTriggerStay2D  | OnTriggerStay  |
| 충돌 종료 시점            | OnTriggerExit2D  | OnTriggerExit  |



##### 8.5.3 Tag를 사용해 아이템 종류 판별하기

- Tag를 사용하면 오브젝트에 특정 이름을 달 수 있어 스크립트에서도 태그로 오브젝트를 판별할 수 있다.
  - Edit -> Project Settings -> Tags and Layers
  - Tags -> + -> New Tag Name (Apple) -> Save
  - Tags -> + -> New Tag Name (Bomb) -> Save
  - (apple) -> Tag -> Apple 선택
  - (bomb) -> Tag -> Bomb 선택

- 사과인지 폭탄인지 판별하기 위해 수정

  ```c#
  // (BasketController 수정)
  ...
  {
  	void OnTriggerEnter(Collider other)
  	{
   		if (other.gameObject.tag == "Apple")
   		{
              Debug.Log("Tag=Apple");
          }
  	 	else
   		{
              Debug.Log("Tag=Bomb");
          }
  	}
  	...
  }
  ```

  

##### 8.5.4 아이템을 받을 때 효과음 내기

- 바구니에 AudioSource 컴포넌트 적용
  - (basket) -> Add Component -> Audio -> Audio Source

- 효과음을 내는 시점을 스크립트에서 지정

  - AudioSource 컴포넌트에 등록할 수 있는 음원은 한 종류이기 때문에 사과와 폭탄을 받을 때의 음을 분리해야 하므로 스크립트를 사용해 음원을 지정

  ```c#
  // (BasketController 수정)
  public class BaksetController : MonoBehaviour
  {
  	public AudioClip appleSE;
  	public AudioClip bombSE;
  	AudioSource aud;
  
      void Start()
  	{ 
         this.aud = GetComponent<AudioSource>();
      }
  
      void OnTriggerEnter(Collider other)
  	{
   		if (other.gameObject.tag == "Apple")
   		{
              this.aud.PlayOneShot(this.appleSE);
          }
  	 	else
   		{
              this.aud.PlayOneShot(this.bombSE);
          }
   	...
      }
  }
  ```

  

- 스크립트 변수에 음성 파일 대입
  - 아웃렛 접속을 사용.
    - (basket) -> (Apple SE 에 get_se), (Bomb SE 에 damage_se) 

