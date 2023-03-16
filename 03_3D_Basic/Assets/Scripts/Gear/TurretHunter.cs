using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHunter : Turret
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //Vector3 dir = other.transform.position - transform.position;
            //barrelBodyTransform.forward = dir;
            barrelBodyTransform.LookAt(other.transform.position);
        }
    }
}
