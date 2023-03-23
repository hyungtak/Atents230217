using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAuto : DoorBase
{
    private void OnTriggerEnter(Collider other)
    {
        Open();
    }

    private void OnTriggerExit(Collider other)
    {
        Close();
    }
}
