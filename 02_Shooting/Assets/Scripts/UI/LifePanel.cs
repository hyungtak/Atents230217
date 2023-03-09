using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LifePanel : MonoBehaviour
{
    TextMeshProUGUI lifeText;

    private void Awake()
    {
        Transform textTransform = transform.GetChild(2);
        lifeText = textTransform.GetComponent<TextMeshProUGUI>();   // 텍스트매시 프로 찾아놓기
    }

    private void Start()
    {
        //Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Player player = FindObjectOfType<Player>(); // 플레이어 찾아서
        player.onLifeChange += Refresh;             // 델리게이트에 함수 등록
    }

    private void Refresh(int life)
    {
        lifeText.text = life.ToString();            // 파라메터 그대로 글자 찍기
    }
}
