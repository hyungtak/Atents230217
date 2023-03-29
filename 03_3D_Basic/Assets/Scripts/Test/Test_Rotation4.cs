using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Rotation4 : MonoBehaviour
{
    float height = 5.0f;
    float timeCounter = 0.0f;
    Vector3 pos;
    Transform target;

    private void Start()
    {        
        target = transform.GetChild(0);
        timeCounter = 0.0f;
        pos = Vector3.zero;
    }

    private void Update()
    {
        timeCounter += Time.deltaTime;
        pos.x = Mathf.Cos(timeCounter) * height;
        pos.y = Mathf.Sin(timeCounter) * height;
        target.localPosition = pos;

        transform.Rotate(0, Time.deltaTime * 360.0f, 0);
    }
}
