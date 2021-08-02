using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public float dropSpeed = -0.03f; // 낙하 속도

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, this.dropSpeed, 0); // 낙하 속도 만큼 y축으로 이동
        if (transform.position.y < -1.0f) // stage 밑으로 안보일 정도로 내려가면
        {
            Destroy(gameObject); // 소멸
        }
    }
}
