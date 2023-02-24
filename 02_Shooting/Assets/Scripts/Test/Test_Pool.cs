using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Pool : Test_Base
{
    public BulletPool pool;

    protected override void Test1(InputAction.CallbackContext _)
    {
        Bullet bullet = pool.GetObject();
    }
}
