using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRandomize : MonoBehaviour
{
    public bool check = true;

    private void OnValidate()
    {
        //Debug.Log($"Check is {check}");
        if (check)
        {
            transform.Rotate(0, Random.Range(0f, 360.0f), 0);
            transform.localScale = new Vector3(
                1 + Random.Range(-0.15f, 0.15f),
                1 + Random.Range(-0.15f, 0.15f),
                1 + Random.Range(-0.15f, 0.15f));
            check = false;
        }
    }
}
