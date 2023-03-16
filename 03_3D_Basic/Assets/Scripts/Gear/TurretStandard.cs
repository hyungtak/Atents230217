using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretStandard : Turret
{
    protected override void Start()
    {
        base.Start();
        StartCoroutine(fireCoroutine);
    }

    protected override void OnFire()
    {
        GameObject obj = Instantiate(bulletPrefab);
        obj.transform.position = fireTransform.position;
        obj.transform.rotation = fireTransform.rotation;
    }
}
