using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 클래스는 직렬화가 가능하다고 표시해 놓은 attribute
[Serializable]
public class SaveData
{
    public string[] rankerNames;    // 랭커들 이름
    public int[] highScores;        // 랭커들 점수
}
