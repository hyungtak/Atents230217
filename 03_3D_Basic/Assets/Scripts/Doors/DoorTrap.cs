using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrap : DoorManual
{
    ParticleSystem ps;

    protected override void Awake()
    {
        base.Awake();

        Transform child = transform.GetChild(2);
        ps = child.GetComponent<ParticleSystem>();
    }

    protected override void OnOpen()
    {
        ps.Play();
    }

    protected override void OnClose()
    {
        ps.Stop();
    }
}
