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
        score.text = "점수점수";
    }
}
