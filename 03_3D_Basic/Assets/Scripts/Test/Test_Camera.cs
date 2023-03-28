using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Camera : Test_Base
{
    public Camera[] cameras;

    protected override void Test1(InputAction.CallbackContext _)
    {
        cameras[0].enabled = true;
        cameras[1].enabled = false;
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        cameras[0].enabled = false;
        cameras[1].enabled = true;
    }
}
