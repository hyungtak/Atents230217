using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeTimeGauge : MonoBehaviour
{
    /// <summary>
    /// 수명 표시할 슬라이더
    /// </summary>
    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        // 시작할 때 플레이어 찾아서 델리게이트에 함수 등록
        Player player = FindObjectOfType<Player>();
        player.onLifeTimeChange += Refresh;
    }

    /// <summary>
    /// UI 표시 갱신해야 할 때 실행되는 함수
    /// </summary>
    /// <param name="ratio">현재수명/최대수명</param>
    private void Refresh(float ratio)
    {
        slider.value = ratio;   // 비율에 맞게 표시
    }
}
