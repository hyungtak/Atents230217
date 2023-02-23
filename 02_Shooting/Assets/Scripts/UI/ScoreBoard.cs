using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    TextMeshProUGUI score;

    private void Awake()
    {
        Transform child = transform.GetChild(1);        // 두번째 자식 가져오기
        score = child.GetComponent<TextMeshProUGUI>();  // 두번째 자식의 컴포넌트 가져오기
    }

    private void Start()
    {
        //score.text = "점수점수";
        Player player = FindObjectOfType<Player>();

        // player의 onScoreChange 델리게이트가 실행될 때 RefreshScore를 실행해라
        player.onScoreChange += RefreshScore;       
    }

    /// <summary>
    /// 새 점수로 UI 변경
    /// </summary>
    /// <param name="newScore">새 점수</param>
    void RefreshScore(int newScore)
    {
        score.text = newScore.ToString();
    }

}
