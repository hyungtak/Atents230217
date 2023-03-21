using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManual : DoorBase, IUseableObject
{
    public void Used()
    {
        Debug.Log("사용됨");
    }
}
