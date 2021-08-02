using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    // 사과와 폭탄의 인스턴스를 만들기 위해 Prefab 변수 선언
    public GameObject applePrefab;
    public GameObject bombPrefab;
   
    float span = 1.0f; // 아이템이 생성 될 주기 변수
    float delta = 0; // 프레임마다 카운트 할 변수
    int ratio = 2; // 폭탄이 생성될 확률 변수
    float speed = -0.03f; //아이템의 낙하 속도 변수

    public void SetParameter(float span, float speed, int ratio) // 매개변수를 일괄적으로 설정할 수 있는 매서드 정의
    {
        this.span = span; // 아이템의 생성 간격
        this.speed = speed; // 아이템의 낙하 속도
        this.ratio = ratio; // 사과와 폭탄의 생성 비율
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.delta += Time.deltaTime; // 프레임마다 카운트
        if (this.delta > this.span) // 아이템을 생성할 주기(1초)가 흐르면
        {
            this.delta = 0; // 카운트 초기화

            GameObject item; // 아이템 오브젝트 변수 선언
            int dice = Random.Range(1, 11); // 10 번의 확률 중
            if (dice <= this.ratio) // 폭탄이 생성될 확률에 걸리면
            {
                item = Instantiate(bombPrefab) as GameObject; // 폭탄 프리팹 생성
            }
            else // 사과가 생성될 확률에 걸리면
            {
                item = Instantiate(applePrefab) as GameObject; // 사과 프리팹 생성
            }
            // 아이템이 생성될 좌표를 -1, 0, 1 중 랜덤으로 선택되도록
            float x = Random.Range(-1, 2);
            float z = Random.Range(-1, 2);
            // 아이템을 (x, 4, z) 위치로 이동
            item.transform.position = new Vector3(x, 4, z);
            // 아이템의 낙하 속도 변수를 ItemController 내 정의한 변수에 대입
            item.GetComponent<ItemController>().dropSpeed = this.speed;
        }
    }
}
