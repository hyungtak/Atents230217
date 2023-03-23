using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorKey : MonoBehaviour
{
    public DoorBase target;
    public float rotateSpeed = 360.0f;

    Transform keyModel;

    private void Awake()
    {
        keyModel = transform.GetChild(0);
    }

    private void Update()
    {
        keyModel.Rotate(Time.deltaTime * rotateSpeed * Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnConsume();        
    }

    private void OnValidate()
    {
        //Debug.Log("OnValidate");
        if(target != null)
        {
            // target이 자동문이어야 한다.
            // DoorAuto이면 그대로
            // DoorAuto가 아니면 target은 null.
            
            target = target as DoorAuto;       
            
            //target = target.GetComponent<DoorAuto>(); // 위와 같은 기능의 코드

            //bool isDoorAuto = target is DoorAuto;     // 위와 같은 기능의 코드
            //if(!isDoorAuto)
            //{
            //    target = null;
            //}
        }
    }

    protected virtual void OnConsume()
    {
        target.Open();
        Destroy(this.gameObject);
    }
}
