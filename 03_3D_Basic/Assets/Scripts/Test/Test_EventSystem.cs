using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test_EventSystem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
    IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Start");
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drag End");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit");
    }
}
