using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketController : MonoBehaviour
{
    // 오디오 변수 선언
    public AudioClip appleSE;
    public AudioClip bombSE;
    AudioSource aud;

    GameObject director; // 감독 오브젝트에 접근하기 위해 변수 선언

    // Start is called before the first frame update
    void Start()
    {
        this.director = GameObject.Find("GameDirector"); // 감독 오브젝트 검색해 대입
        this.aud = GetComponent<AudioSource>(); // 오디오 컴포넌트 적용
    }

    void OnTriggerEnter(Collider other) // 충돌 상대의 오브젝트로 적용된 콜라이더와 충돌할 경우
    {
        if (other.gameObject.tag == "Apple") // 사과를 받으면
        {
            this.director.GetComponent<GameDirector>().GetApple(); // 감독 스크립트의 GetApple 매서드 호출
            this.aud.PlayOneShot(this.appleSE); // 사과 사운드 실행
        }
        else // 폭탄을 받으면
        {
            this.director.GetComponent<GameDirector>().GetBomb(); // 감독 스크립트의 GetBomb 매서드 호출
            this.aud.PlayOneShot(this.bombSE); // 폭탄 사운드 실행
        }
        Destroy(other.gameObject); // 충돌한 상대 오브젝트를 소멸
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 화면을 탭 하면
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 탭한 위치를 월드 좌표(광선)로 변환
            RaycastHit hit; // 광선이 stage와 충돌하는 좌표
            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) // 광선이 stage 오브젝트와 충돌하였는지 확인
             // out : out에 계속 매서드 내 값을 채워 변수로 반환
             // Raycast 매서드 안에서 광선이 stage와 충돌한 좌표를 hit.point 변수에 반환
            {
                // RoundToInt : 반올림 매서드
                // 광선이 stage와 충돌한 좌표를 반올림하고 바구니 좌표에 대입
                float x = Mathf.RoundToInt(hit.point.x);
                float z = Mathf.RoundToInt(hit.point.z);
                transform.position = new Vector3(x, 0, z);
            }
        }
    }
}
