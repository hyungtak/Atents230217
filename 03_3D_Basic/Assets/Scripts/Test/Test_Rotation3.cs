using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Rotation3 : MonoBehaviour
{
    public Transform objBase;
    public Transform objChild1;
    public Transform objChild2;

    private void Update()
    {
        // y축 기준으로 초당 180도
        //objBase.Rotate(0, Time.deltaTime * 180.0f, 0);
        //objBase.Rotate(new Vector3(0, Time.deltaTime * 180.0f, 0));
        //objBase.Rotate(transform.up, Time.deltaTime * 180.0f);  //자신의 y축 기준으로 초당 180도
        //objBase.Rotate(Vector3.up, Time.deltaTime * 180.0f);    // 월드의 y축 기준으로 초당 180도
        //objBase.Rotate(Vector3.up, Time.deltaTime * 180.0f, Space.Self); // 자신의 y축 기준으로 초당 180도
        //objBase.Rotate(Vector3.up, Time.deltaTime * 180.0f, Space.World);// 월드의 y축 기준으로 초당 180도

        //objBase.RotateAround(new Vector3(-2,0,0), Vector3.up, Time.deltaTime * 180.0f);
        objBase.RotateAround(new Vector3(0,0,5), Vector3.right, Time.deltaTime * 180.0f);
    }
}
