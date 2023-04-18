using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillCountText : MonoBehaviour
{
    // 시간 변화에 따라 남아있는 플레이어의 수명 출력하기(소수점 둘째자리까지 출력)

    TextMeshProUGUI textUI;

    private void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = GameManager.Inst.Player;
        player.onKillCountChange += OnKillCountChange;
        textUI.text = $"Kill Count: {player.KillCount}";

    }

    private void OnKillCountChange(int ratio)
    {
        textUI.text = $"Kill Count: {ratio}";
    }
}
