using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PowerUp : Test_Base
{
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        //player.Power = 1;
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        //player.Power = 2;
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        //player.Power = 3;
    }

    protected override void Test4(InputAction.CallbackContext _)
    {
        //player.Power = 4;
    }

    protected override void Test5(InputAction.CallbackContext _)
    {
        //player.Power = 0;
    }
}
