using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    Animator anim;
    Button button;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(OnRestart);      // 버튼 클릭 이벤트에 함수 등록
    }

    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        player.onDie += (_) => ShowPanel();      // 플레이어의 onDie 델리게이트에 함수 등록
        //gameObject.SetActive(false);    // 게임 오브젝트 비활성화
    }

    private void OnRestart()
    {
        SceneManager.LoadScene(0);  // 0번씬 불러오기
    }

    private void ShowPanel()
    {
        //transform.GetChild(0).gameObject.SetActive(true);   // 글자가 비활성화 되어있던 것을 활성화
        //gameObject.SetActive(true);     // 게임 오버 패널 전체 보이기

        anim.SetTrigger("GameOverStart");
    }
}
