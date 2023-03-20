using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Rotation2 : Test_Base
{
    public Transform objBase;
    public Transform objChild1;
    public Transform objChild2;

    Quaternion to = Quaternion.LookRotation(Vector3.right, Vector3.up);

    protected override void Test1(InputAction.CallbackContext _)
    {
        Quaternion q = Quaternion.identity;
        objBase.rotation = q;
        objChild1.rotation = q;
        // objChild2.rotation = Quaternion.Euler(0, 90, 0);// y축으로 90도(degree)만큼 회전하는 쿼터니언 만들기

        // 위의 Euler와 같은 기능을 하는 코드 작성하기
        objChild2.rotation = Quaternion.AngleAxis(90, Vector3.up);
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        // 파라메터로 받은 회전과 Inverse함수의 결과를 곱하면 identity가 나온다
        Quaternion q = Quaternion.Inverse(objChild2.rotation);
        objChild2.rotation *= q;
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        //Quaternion.Lerp;    // 선형보간

        Quaternion a = Quaternion.LookRotation(Vector3.forward);
        Quaternion b = Quaternion.LookRotation(Vector3.right);

        // a회전에서 b회전으로 진행된다고 했을 때 절반만큼 진행되었을 때의 회전 구하기
        objBase.rotation = Quaternion.Slerp(a, b, 0.5f);   // 곡면용 보간
    }

    private void Update()
    {
        // from회전에서 to회전으로 maxDegreeDetal만큼 회전시키기. 일정한 속도로 회전한다.
        //objBase.rotation = Quaternion.RotateTowards(objBase.rotation, to, Time.deltaTime * 90.0f);

        // from회전에서 to회전으로 점점 속도가 줄어들면서 회전한다.
        objBase.rotation = Quaternion.Slerp(objBase.rotation, to, Time.deltaTime * 0.5f);
    }
}
