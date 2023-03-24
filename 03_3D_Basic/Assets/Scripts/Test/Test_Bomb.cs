using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Bomb : Test_Base
{
    public float explosionPower = 100.0f;
    public float radius = 2.0f;
    public float upward = -1.0f;

    protected override void Test1(InputAction.CallbackContext _)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        Rigidbody[] rigids = new Rigidbody[colliders.Length];

        for(int i = 0; i<colliders.Length; i++)
        {
            rigids[i] = colliders[i].GetComponent<Rigidbody>();            
        }

        foreach(Rigidbody rigid in rigids) 
        {
            if(rigid != null)
            {
                rigid.AddExplosionForce(explosionPower, transform.position, radius, upward, ForceMode.Impulse);
            }
        }
    }
}
