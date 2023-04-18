using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class LifeTimeText : MonoBehaviour
{
    // 시간 변화에 따라 남아있는 플레이어의 수명 출력하기(소수점 둘째자리까지 출력)

    TextMeshProUGUI textUI;
    float maxLifeTime;

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
    }

    private void OnLifeTimeChange(float ratio)
    {
        textUI.text = $"{(maxLifeTime * ratio):f2} Sec";
    }
}
