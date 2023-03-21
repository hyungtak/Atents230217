using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour
{
    public DoorBase target;
    Transform keyModel;

    private void Awake()
    {
        keyModel = transform.GetChild(0);
    }

    private void Update()
    {
        
    }
}
