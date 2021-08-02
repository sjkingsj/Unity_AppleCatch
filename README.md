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





#### 8.6 사과와 폭탄 공장 만들기

##### 8.6.1 프리팹 만들기

- 사과와 폭탄을 일정 시간 간격으로 무작위 위치에 생성한다.
- 사과와 폭탄의 프리팹 만들기
  - (apple)을 Project 창으로 드래그&드롭 -> Original Prefab -> (applePrefab)
  - Hierarchy 창의 (apple)은 제거
  - (bomb)도 마찬가지로 (bombPrefab)의 프리팹 만들고 기존 오브젝트 제거



##### 8.6.2 제너레이터 스크립트 작성하기

- 사과가 1초 간격으로 떨어지는 것으로 구현

  ```c#
  // (ItemGenerator)
  public class ItemGenerator : MonoBehaviour
  {
  	public GameObject applePrefab;
  	public GameObject bombPrefab;
      
  	float span = 1.0f;  // 1초마다로 지정
  	float delta = 0;  // 카운트 변수
  
      void Update()
  	{
   		this.delta += Time.deltaTime; // 프레임마다 흘러가는 시간
   		if (this.delta > this.span)  // 프레임이 1초가 지나면
   		{
   			this.delta = 0;  // 카운트 초기화
    			Instantiate(applePrefab);  // 사과 프리팹 만들기
   		}
  	}
  }
  ```

  

##### 8.6.3 빈 오브젝트에 제너레이터 스크립트 적용하기

- Hierarchy 창 -> + -> Create Empty -> (ItemGenerator)
- (ItemGenerator) 스크립트를 적용하기



##### 8.6.4 제너레이터 스크립트로 프리팹 전달하기

- (ItemGenerator)의 Inspector 창에서 (applePrefab), (bombPrefab) 적용



##### 8.6.5 공장 업그레이드하기

- 아이템이 떨어지는 위치를 무작위로 변경

- 아홉 개로 분할한 구역 어딘가로 아이템을 떨어뜨릴 것이며 중앙은 원점, 좌우상하 구역의 중심은 +-1 이다.

  ```c#
  // (ItemGenerator 수정)
  public class ItemGenerator : MonoBehaviour
  {
  	...
  	void Update()
  	{
   		...
   		if (this.delta > this.span)
   		{
    			...
    			// 사과 프리팹 생성
    			GameObject item = Instantiate(applePrefab) as GameObject;
    			float x = Random.Range(-1, 2);
    			float z = Random.Range(-1, 2); // 무작위의 x, z 좌표 생성
    			item.transform.position = new Vector3(x, 4, z);  // 사과 프리팹 이동
  		}
      }
  }
  ```

  

- 랜덤 값
  - x = Random.Range(a, b);  // a 이상, b 미만의 무작위의 정수 생성

- 출현할 아이템을 무작위로 변경

  - 사과 대신 일정 확률로 폭탄이 생성되도록 수정

  ```c#
  // (ItemGenerator 수정)
  public class ItemGenerator : MonoBehaviour
  {
  	...
  	int ratio = 2; // 폭탄이 생성될 확률 변수
  
      void Update()
  	{
  		...
  		if (this.delta > this.span)
   		{
    			...
    			GameObject item;
    			int dice = Random.Range(1, 11); // 1 ~ 10의 확률 중
    			if (dice <= this.ratio)  // 폭탄이 나올 확률이 걸리면
    			{
                  item = Instantiate(bombPrefab) as GameObject; // 폭탄 프리팹 생성
              }  
    			else  // 그 외에는
    			{
          	    item = Instantiate(applePrefab) as GameObject; // 사과 프리팹 생성
          	}  
    			...
  		}
      }
  }
  ```

  

- 매개변수를 외부에서 조절할 수 있도록 설정

  - 생성 위치, 생성 속도, 아이템 종류 등을 변경하여 게임 난이도를 조절하기 위해 매개변수를 조절해 긴장감이 떨어지지 않도록 게임을 연출

  ```c#
  // (ItemGenerator 수정)
  public class ItemGenerator : MonoBehaviour
  {
  	...
  	float speed = -0.03f; // 아이템 낙하 속도 변수
  	
      // 매개변수를 일괄적으로 설정할 수 있는 매서드 정의
      public void SetParameter(float span, float speed, int ratio)
  	{
   		this.span = span; // 아이템의 생성 간격
   		this.speed = speed; // 아이템의 낙하 속도
   		this.ratio = ratio; // 사과와 폭탄의 생성 비율
  	}
      
  	void Update()
  	{
   		...
   		if (this.delta > this.span)
   		{
    			...
    			item.GetComponent<ItemGenerator>().dropSpeed = this.speed;
    			// 아이템 낙하 속도 변수를 ItemController 내 정의한 변수에 대입
  		}
      }
  }
  ```



- 소멸될 오브젝트에 효과음을 넣고 싶을 경우
  - 사과나 폭탄에 AudioSource 컴포넌트를 적용하면 충돌한 순간 아이템이 소멸되므로 효과음을 내기 전에 아이템에 적용 된 AudioSource 컴포넌트 역시 소멸된다.
  - 따라서 소멸되는 오브젝트에 적용된 스크립트에서 효과음을 내고 싶다면 
  - AudioSource.PlayClipAtPoint(AudioClip clip, Vector3 pos) 매서드를 사용해야한다.
  - 이 매서드는 음원과 소리를 내고 싶은 좌표를 지정하면 지정한 장소에서 새로운 게임 오브젝트를 생성하고 그곳에서 효과음을 재생한다.



- 난수
  - 컴퓨터로 다루는 난수는 진짜 난수가 아닌 의사 난수이기 때문에 패턴을 해석할 수 있다. 이러한 의사 난수라는 수열 패턴만 알면 다음에 나올 숫자를 알 수 있다. 이러한 생성을 막으려면 몇 번째 난수부터 사용할지 매번 바뀌도록 수열 패턴을 변경해야 한다. 이러한 값을 '난수의 시드'라고 한다.





#### 8.7 UI 만들기

##### 8.7.1 UI 배치하기

- 제한 시간과 득점 표시 두 가지를 UI로 준비한다.
  - 제한 시간 UI : 남아 있는 게임 시간을 카운트다운해 표시
  - 득점 UI : 플레이어가 얻은 점수를 표시
- 감독 스크립트 작성 : 제한 시간 UI
  - Hierarchy 창 -> + -> UI -> Text -> (Time)
  - (Time) -> 앵커 포인트 (오른쪽 위), Rect Transform의 Pos(-170, -70, 0), Width & Height (250, 100), Text(60), Font Size(84), Alignment(가로 오른쪽 정렬, 세로 중앙 정렬)
- 감독 스크립트 작성 : 득점 UI
  - 똑같이 (Point) 만들기
  - (Point) -> 앵커 포인트 (오른쪽 위), Rect Transform의 Pos(-270, -180, 0), Width & Height (450, 100), Text(0 point), Font Size(84), Alignment(가로 오른쪽 정렬, 세로 중앙 정렬)



##### 8.7.2 UI를 갱신하는 감독 만들기

- 제한 시간과 득점을 관리하고 두 값이 갱신될 때 UI에 반영

  1. 감독 스크립트 작성

     - 제한 시간은 60초부터 카운트다운을 시작해 0초에 정지

        -> 현재 시간에서 Time.deltaTime을 빼면 카운트다운을 구현할 수 있다.

     ```c#
     // (GameDirector)
     using UnityEngine.UI;
     
     public class GameDirector : MonoBehaviour
     {
     	GameObject timerText; // Time을 대입할 수 있도록 변수 선언
     	float time = 60.0f; // 남은 시간 time 변수를 60초로 초기화
     
         void Start()
     	{
             this.timerText = GameObject.Find("Time");
         } // Time UI 대입
         
         void Update()
     	{
      		this.time -= Time.deltaTime; // 각 프레임 사이의 시간 차를 time 변수에서 빼기
      		this.timerText.GetComponent<Text>().text = this.time.ToString("F1");
      		// 나머지 시간을 문자열로 변환해 UI Text에 대입. 소수점 아래 첫째 자리까지 표시할 수 있도록 "F1" 서식 지정자를 지정
     	}
     }
     ```

     

2. 빈 오브젝트 생성
   - \+ -> Create Empty -> (GameDirector)

3. 감독 스크립트 적용
   - (GameDirector)에 스크립트 적용



##### 8.7.3 감독에게 득점 관리시키기

- 아이템이 바구니와 충돌할 때 득점 증감을 알리고 UI를 갱신

  1. 감독으로 UI 갱신

  ```c#
  // (GameDirector 수정)
  public class GameDirector : MonoBehaviour
  {
  	...
  	GameObject pointText; // Point UI 대입할 수 있도록 pointText 변수 선언
  	int point = 0; // 점수 point 변수 초기화
  
      public void GetApple() // 사과를 받았을 때 매서드
  	{
          this.point += 100;
      }
      
  	public void GetBomb() // 폭탄을 받았을 때 매서드
  	{
          this.point /= 2;
      }
      
  	void Start()
  	{
          ...
   		this.pointText = GameObject.Find("Point"); // Point UI 대입
      }
  
      void Update()
  	{
          ...
   		this.pointText.GetComponent<Text>().text = this.point.ToString() + " point";
          // 점수를 UI에 문자열로 변환해 표시
  	}
  }
  ```

  

2. 바구니 컨트롤러에서 감독으로 득점 전달

   ```c#
   // (BasketController 수정)
   public class BasketController : MonoBehaviour
   {
   	...
   	GameObject director;
   
       void Start()
   	{
           ...
    		this.director = GameObject.Find("GameDirector"); // 감독 오브젝트 검색
   	}
       
   	void OnTriggerEnter(Collider other)
   	{
    		if (other.gameObject.tag == "Apple")
    		{
               ...
     			this.director.GetComponent<GameDirector>().GetApple();
           } // 감독 오브젝트의 GetApple 매서드 호출
    		else
    		{
               ...
     			this.director.GetComponent<GameDirector>().GetBomb();
           } // 감독 오브젝트의 GetBomb 매서드 호출
   		...
   	}
   }
   ```

   

**[ 자신 이외의 오브젝트 컴포넌트에 접근하는 방법 ]**

1. Find 매서드로 오브젝트를 찾는다.
2. GetComponent 매서드로 오브젝트의 컴포넌트를 구한다.
3. 컴포넌트를 가진 데이터에 접근한다.





#### 8.8 레벨 디자인하기

##### 8.8.1 게임 플레이하기

- 제한 시간이 너무 길어 싫증 난다.
- 게임이 단조로워서 단순 작업이 되기 쉽다.



##### 8.8.2 제한 시간 조절하기

- 제한 시간이 너무 짧으면 즐거움보다 압박감이 심해져 플레이해도 기분이 좋지 않다.

  -> 30초 정도로 변경하기

  ```c#
  // (GameDirector 수정)
  float time = 30.0f;
  ```

- 게임 진행에 따라 난이도에 변화를 주자



##### 8.8.3 레벨 디자인

- 난이도를 레벨이라고 하기도 하고 게임의 무대인 맵을 레벨이라고 하기도 한다.
- 플레이어 능력과 도전 내용의 난이도가 균형을 이루고 있을 때 플로 상태 (몰입 상태). 이러한 상태로 향하는 행동을 재미있게 느껴진다.
- 따라서 최적의 난이도로 설정하는 것이 중요하다.
  - 난이도 곡선 : 시작할 때 도전할 내용을 이해하는 준비 운동 시간으로서 너무 어렵지 않도록 난이도를 낮게 설정하고, 중반에는 게임에 익숙해질 무렵이므로 난이도를 서서히 올린다. 이 단계의 끝에 오면 가장 어렵게 느끼도록 만들고, 후반에는 난이도를 조금 떨어뜨린다. 이것은 게임을 기분 좋게 마치도록 하는 배려이며, 끝까지 어려우면 게임을 마치고 났을 때 재미는 사라지고 피로만 남기 때문이다.



##### 8.8.4 레벨 디자인 도전하기

**난이도에 직결될 것 같은 매개변수**

- 아이템 생성 속도

- 아이템 낙하 속도

- 사과와 폭탄 비율

  - 난이도 곡선에 맞춰 게임 진행을 축으로 각 매개변수를 설정한다.

  | 남은 시간 | 생성 속도 | 낙하 속도 | 폭탄 비율 |
  | :-------: | :-------: | :-------: | :-------: |
  |   30~20   |    1초    |   -0.03   |    20%    |
  |   20~10   |   0.7초   |   -0.04   |    40%    |
  |   10~5    |   0.4초   |   -0.06   |    60%    |
  |    5~0    |   0.9초   |   -0.04   |    30%    |

  ```c#
  // (GameDirector 수정)
  public class GameDirector : MonoBehaviour
  {
  	...
  	GameObject generator;
  	...
  	void Start()
  	{
   		this.generator = GameObject.Find("ItemGenerator");
   		...
  	}
  
      void Update()
  	{
   		...
   		if (this.time < 0)
          {
    			this.time = 0;
    			this.generator.GetComponent<ItemGenerator>().SetParameter(10000.0f, 0, 0);
   		} // 게임을 끝낸 후에는 아이템 생성을 중단시키기 위해 생성 시간에 큰 값 설정
   		else if (0 <= this.time && this.time < 5)
   		{
              this.generator.GetComponent<ItemGenerator>().SetParameter(0.9f, -0.04f, 3);
          }
   		else if (5 <= this.time && this.time < 10)
   		{
              this.generator.GetComponent<ItemGenerator>().SetParameter(0.4f, -0.06f, 6);
          }
   		else if (10 <= this.time && this.time < 20)
   		{
              this.generator.GetComponent<ItemGenerator>().SetParameter(0.7f, -0.04f, 4);
          }
   		else if (20 <= this.time && this.time < 30)
   		{
              this.generator.GetComponent<ItemGenerator>().SetParameter(1.0f, -0.03f, 2); 
          }
   		...
  	}
  }
  ```

  

##### 8.8.5 매개변수 조절하기

- 직접 게임을 체험해보며 적절한 난이도로 매개변수를 조절해나간다.

  | 남은 시간 | 생성 속도 | 낙하 속도 | 폭탄 비율 |
  | :-------: | :-------: | :-------: | :-------: |
  |   30~23   |    1초    |   -0.03   |    20%    |
  |   23~12   |   0.8초   |   -0.04   |    40%    |
  |   12~5    |   0.5초   |   -0.05   |    60%    |
  |    5~0    |   0.7초   |   -0.04   |    30%    |
