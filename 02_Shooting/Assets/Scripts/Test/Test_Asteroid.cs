using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Asteroid : Test_Base
{
    protected override void Test1(InputAction.CallbackContext _)
    {
        Factory.Inst.GetObject(PoolObjectType.AsteroidSmall);
    }
}
