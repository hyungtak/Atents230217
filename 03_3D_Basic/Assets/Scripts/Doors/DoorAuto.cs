using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAuto : DoorBase
{
    private void OnTriggerEnter(Collider other)
    {
        OnOpen();
    }

    private void OnTriggerExit(Collider other)
    {
        OnClose();
    }
}
