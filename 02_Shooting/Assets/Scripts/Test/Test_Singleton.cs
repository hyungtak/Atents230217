using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Singleton : Test_Base
{
    void Start()
    {
        //Player p = new Player();
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        Factory.Inst.GetBullet();
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        Factory.Inst.GetObject(PoolObjectType.Hit);
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        Factory.Inst.GetEnemy();
    }

    protected override void Test4(InputAction.CallbackContext _)
    {
        Factory.Inst.GetExplosionEffect();
    }

}
