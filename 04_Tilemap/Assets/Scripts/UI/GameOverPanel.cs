using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public float alphaChangeSpeed = 1.0f;

    CanvasGroup canvasGroup;

    TextMeshProUGUI playTime;
    TextMeshProUGUI killCount;

    Button restart;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Transform childe = transform.GetChild(1);
        playTime = childe.GetComponent<TextMeshProUGUI>();
        childe = transform.GetChild(2);
        killCount = childe.GetComponent<TextMeshProUGUI>();
        restart = GetComponentInChildren<Button>();

        // 버튼에 함수 등록
        restart.onClick.AddListener(OnRestartClick);
    }

    private void Start()
    {
        Player player = GameManager.Inst.Player;
        player.onDie += OnPlayerDie;
    }

    private void OnPlayerDie(float totalPlayTime, int totalKillCount)
    {
        playTime.text = $"Total Play Time \n\r< {totalPlayTime:F1} Sec >";
        killCount.text = $"Total Kill Count\n\r< {totalKillCount:F1} kill>";

        StartCoroutine(StartAlphaChange());
    }

    IEnumerator StartAlphaChange()
    {
        while(canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
    }

    /// <summary>
    /// 버튼이 클릭되었을 때 실행될 함수
    /// </summary>
    private void OnRestartClick()
    {
        StartCoroutine(WaitUnloadAll());
    }
    /// <summary>
    /// 씬 전부 로딩 해제한 후. 로딩 씬으로 돌아가는 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitUnloadAll()
    {
        MapManager mapManager = GameManager.Inst.MapManager;
        while (!mapManager.IsUnloadAll) // 로드되었던 모든씬이 해제될때까지 대기
        {
            yield return null;
        }
        SceneManager.LoadScene("LoadingScene"); // 로드해제가 끝나면 로딩씬으로 돌아가기
    }
}
