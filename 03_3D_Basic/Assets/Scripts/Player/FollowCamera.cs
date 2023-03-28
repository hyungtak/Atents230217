using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float speed = 3.0f;
    Vector3 offset;

    private void Start()
    {
        if(target == null)
        {
            Player player = FindObjectOfType<Player>();
            target = player.transform;
        }

        offset = transform.position - target.position; 
    }

    //private void LateUpdate()
    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            target.position + offset,
            Time.fixedDeltaTime * speed);
    }
}
