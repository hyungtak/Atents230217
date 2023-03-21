using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBase : MonoBehaviour
{
    protected Animator anim;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
    }

    protected virtual void OnOpen()
    {
        anim.SetBool("IsOpen", true);
    }

    protected virtual void OnClose()
    {
        anim.SetBool("IsOpen", false);
    }
}
