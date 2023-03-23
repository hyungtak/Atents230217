using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKeyUnlock : DoorKey
{
    Action onConsume;

    private void Start()
    {
        ResetTarget();
    }

    private void OnValidate()
    {
        ResetTarget();
    }

    void ResetTarget()
    {
        if (target != null)
        {
            DoorAutoLock lockDoor = target as DoorAutoLock;
            if (lockDoor != null)
            {
                onConsume = lockDoor.Unlock;
            }
            else
            {
                target = null;
            }
        }
    }

    protected override void OnConsume()
    {        
        onConsume?.Invoke();
        Destroy(gameObject);
    }
}
