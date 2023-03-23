using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAuto : DoorBase
{
    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player")) // ItemUseChecker와 겹쳤을 때 실행되는 것 방지
        {
            Open();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Close();
    }
}
