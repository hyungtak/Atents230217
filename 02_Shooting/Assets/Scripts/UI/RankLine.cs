using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankLine : MonoBehaviour
{
    TextMeshProUGUI nameText;
    TextMeshProUGUI recordText;

    private void Awake()
    {
        // 컴포넌트 찾기
        Transform child = transform.GetChild(1);
        nameText = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        recordText = child.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 데이터 세팅하는 함수
    /// </summary>
    /// <param name="rankerName">랭커의 이름</param>
    /// <param name="record">랭커의 점수기록</param>
    public void SetData(string rankerName, int record)
    {
        nameText.text = rankerName;
        recordText.text = record.ToString("N0");
    }
}
