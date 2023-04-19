using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class LifeTimeText : MonoBehaviour
{
    public float speed = 1.0f;

    TextMeshProUGUI textUI;
    float maxLifeTime;

    float targetValue;
    float currentValue;

    private void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        Player player = GameManager.Inst.Player;
        player.onLifeTimeChange += OnLifeTimeChange;
        maxLifeTime = player.maxLifeTime;
        textUI.text = $"{maxLifeTime:f2} Sec";

        targetValue = maxLifeTime;
        currentValue = maxLifeTime;
    }

    //private void Update()
    //{
    //    if (currentValue > targetValue)
    //    {
    //        //slider.value가 줄어야 한다.
    //        currentValue -= Time.deltaTime * speed;
    //        if (currentValue < targetValue)
    //        {
    //            currentValue = MathF.Max(targetValue, 0);
    //        }
    //    }
    //    else
    //    {
    //        //slider.value가 늘어야 한다.
    //        currentValue += Time.deltaTime * speed;

    //        if (currentValue > targetValue)
    //        {
    //            currentValue = MathF.Max(targetValue, 1);
    //        }
    //    }
    //    textUI.text = $"{(currentValue):f2} Sec";

    //}

    private void OnLifeTimeChange(float ratio)
    {
        textUI.text = $"{(maxLifeTime * ratio):f2} Sec";

        //targetValue = maxLifeTime * ratio;
    }
}