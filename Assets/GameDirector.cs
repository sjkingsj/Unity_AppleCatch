using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    GameObject timerText; // Time UI를 대입할 수 있도록 변수 선언
    GameObject pointText; // Point UI를 대입할 수 있도록 변수 선언
    float time = 30.0f; // 남은 시간 time 변수를 30초로 초기화
    int point = 0; // 점수 point 변수 초기화
    GameObject generator; // ItemGenerator 스크립트에 접근하기 위해 변수 선언

    public void GetApple() // 사과를 받았을 때 매서드
    {
        this.point += 100;
    }

    public void GetBomb() // 폭탄을 받았을 때 매서드
    {
        this.point /= 2;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.timerText = GameObject.Find("Time"); // Time UI에 대입
        this.pointText = GameObject.Find("Point"); // Point UI에 대입
        this.generator = GameObject.Find("ItemGenerator"); // ItemGenerator 스크립트 호출
    }

    // Update is called once per frame
    void Update()
    {
        this.time -= Time.deltaTime; // 각 프레임 사시의 시간 차를 time 변수에서 빼기

        // 시간에 따른 난이도 설정
        if (this.time < 0)
        {
            this.time = 0;
            this.generator.GetComponent<ItemGenerator>().SetParameter(10000.0f, 0, 0);
            // 게임을 마친 후에는 아이템 생성을 중단시키기 위해 생성 시간에 큰 값 설정
        }
        else if (0 <= this.time && this.time < 5)
        {
            this.generator.GetComponent<ItemGenerator>().SetParameter(0.7f, -0.04f, 3);
        }
        else if (5 <= this.time && this.time < 12)
        {
            this.generator.GetComponent<ItemGenerator>().SetParameter(0.5f, -0.05f, 6);
        }
        else if (12 <= this.time && this.time < 23)
        {
            this.generator.GetComponent<ItemGenerator>().SetParameter(0.8f, -0.04f, 4);
        }
        else if (23 <= this.time && this.time < 30)
        {
            this.generator.GetComponent<ItemGenerator>().SetParameter(1.0f, -0.03f, 2);
        }

        this.timerText.GetComponent<Text>().text = this.time.ToString("F1"); // 나머지 시간을 문자열로 변환 해 UI Text에 대입
                                                                             // 소수점 아래 첫째 자리까지 표현할 수 있도록 "F1" 서식 지정자 지정
        this.pointText.GetComponent<Text>().text = this.point.ToString() + " point"; // 점수를 문자열로 변환 해 Point UI의 Text에 대입
    }
}
