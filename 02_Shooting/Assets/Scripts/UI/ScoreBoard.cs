using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    /// <summary>
    /// 점수가 올라가는 속도
    /// </summary>
    public float minScoreUpSpeed = 50.0f;

    /// <summary>
    /// 현재 UI에서 보이는 점수
    /// </summary>
    float currentScore = 0;

    /// <summary>
    /// 실제 플레이어가 가지고 있는 점수
    /// </summary>
    int targetScore = 0;

    /// <summary>
    /// 점수를 출력할 UI text
    /// </summary>
    TextMeshProUGUI score;

    private void Awake()
    {
        Transform child = transform.GetChild(1);        // 두번째 자식 가져오기
        score = child.GetComponent<TextMeshProUGUI>();  // 두번째 자식의 컴포넌트 가져오기
    }

    private void Start()
    {
        Player player = FindObjectOfType<Player>();     // 임시로 플레이어 가져오기
        // player의 onScoreChange 델리게이트가 실행될 때 RefreshScore를 실행해라
        player.onScoreChange += ScoreUpdate;

        score.text = currentScore.ToString();   // 초기값 설정
    }

    private void Update()
    {
        if( currentScore < targetScore )        // currentScore가 targetScore보다 작으면
        {
            // currentScore를 점수 차이에 비례해서 증가시킨다.(최저 minScoreUpSpeed)
            float speed = Mathf.Max((targetScore - currentScore) * 5.0f, minScoreUpSpeed);            
            currentScore += Time.deltaTime * speed;

            currentScore = Mathf.Min(currentScore, targetScore);  // currentScore의 최대치는 targetScore
            score.text = $"{currentScore:f0}";  // UI에 출력할 때 소수점은 0개만 출력한다.(소수점 출력안함)
        }
    }

    /// <summary>
    /// 목표 점수를 새 점수로 설정
    /// </summary>
    /// <param name="newScore">새 점수</param>
    void ScoreUpdate(int newScore)
    {
        targetScore = newScore;
    }

}
