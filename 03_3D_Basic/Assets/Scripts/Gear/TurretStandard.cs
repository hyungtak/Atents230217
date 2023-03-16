using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretStandard : Turret
{
    protected override void Start()
    {
        base.Start();
        StartCoroutine(fireCoroutine);
        //Time.timeScale = 0.1f;    // 1/10의 속도로 게임이 돌아감
    }

    protected override void OnFire()
    {
        Factory.Inst.GetBullet(fireTransform);        
    }
}
