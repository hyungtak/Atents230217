using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualButton : MonoBehaviour, IPointerClickHandler
{
    public Action onClick;
    Image coolDown;

    void Awake()
    {
        coolDown = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        coolDown.fillAmount = 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }

    /// <summary>
    /// 쿨다운 이미지의 fill 정도를 갱신하는 함수
    /// </summary>
    /// <param name="ratio">새 비율</param>
    public void RefreshCoolTime(float ratio)
    {
        coolDown.fillAmount = ratio;
    }
}
