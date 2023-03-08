using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_SpecialEnemy : Test_Base
{
    protected override void Test1(InputAction.CallbackContext _)
    {
        var test = Factory.Inst.GetPowerUp();
        test.transform.position = Vector3.zero;
    }
}
