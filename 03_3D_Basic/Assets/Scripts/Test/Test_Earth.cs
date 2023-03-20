using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Earth : MonoBehaviour
{
    public Transform sun;

    private void Update()
    {
        //transform.LookAt(sun);    // 특정 방향을 바라보게 만들기
        //transform.rotation = Quaternion.LookRotation(sun.position - transform.position);

        // 특정 지점에 하나의 축을 세우고 그 축을 기준으로 회전시키기
        transform.RotateAround(sun.position, sun.up, Time.deltaTime * 360.0f);
    }
}
