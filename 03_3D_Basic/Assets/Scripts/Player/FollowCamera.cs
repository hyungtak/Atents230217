using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float speed = 3.0f;
    Vector3 offset;
    float lenght;

    private void Start()
    {
        if(target == null)
        {
            Player player = FindObjectOfType<Player>();
            target = player.transform.GetChild(7);  // cameraRoot가 바라보는 대상
        }

        offset = transform.position - target.position;  // 카메라가 target에서 떨어져 있는 정도
        lenght = offset.magnitude;                      // 카메라가 target에서 떨어진 거리
    }

    //private void LateUpdate()
    private void FixedUpdate()
    {
        transform.position = Vector3.Slerp(     // 호를 그리며 움직이게 만들기
            transform.position,                 // 현재 위치에서 
            target.position + Quaternion.LookRotation(target.forward) * offset, // offset만큼 떨어진 위치로(회전 적용됨)
            Time.fixedDeltaTime * speed);       // Time.fixedDeltaTime * speed만큼 보간

        transform.LookAt(target);               // 카메라가 목표지점 바라보기

        // target에서 카메라로 나가는 레이
        Ray ray = new Ray(target.position, transform.position - target.position);   
        if( Physics.Raycast(ray, out RaycastHit hit, lenght) )  // 충돌 체크
        {
            transform.position = hit.point;                     // 충돌하면 충돌한 위치로 카메라 옮김
        }
    }
}
