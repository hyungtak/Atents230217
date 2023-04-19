using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeTimeGauge : MonoBehaviour
{
    public float speed = 1.0f;

    Slider slider;

    float currentValue;
    float targetValue = 1.0f;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    private void Start()
    {
        Player player = GameManager.Inst.Player;
        player.onLifeTimeChange += OnLifeTimeChange;
        slider.value = 1;
        targetValue = 1.0f;
    }

    private void Update()
    {
        if(slider.value > targetValue) 
        {
            //slider.value가 줄어야 한다.
            slider.value -= Time.deltaTime * speed;
            if(slider.value < targetValue )
            {
                slider.value = MathF.Max(targetValue, 0);
            }
        }
        else
        {
            //slider.value가 늘어야 한다.
            slider.value += Time.deltaTime * speed;

            if (slider.value > targetValue)
            {
                slider.value = MathF.Max(targetValue, 1);
            }
        }
    }

    private void OnLifeTimeChange(float ratio)
    {
        targetValue= ratio;
    }
}

