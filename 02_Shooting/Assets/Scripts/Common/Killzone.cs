using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PoolObject comp = collision.GetComponent<PoolObject>();
        if(comp!= null)
        {
            //Debug.Log("Killzone");
            comp.gameObject.SetActive(false);
        }
    }
}
