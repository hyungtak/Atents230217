using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAuto : Platform
{
    bool isMoving = true;

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Move();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isMoving = true;
        }
    }

    protected override void OnArrived()
    {
        base.OnArrived();
        isMoving = false;
    }
}
