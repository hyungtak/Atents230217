using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    /// <summary>
    /// UI에서 표시하는 랭킹 한줄들을 모아둔 배열(0번째가 1등, 4번째가 5등)
    /// </summary>
    RankLine[] rankLines = null;

    /// <summary>
    /// 랭킹별 최고점(0번째가 1등, 4번째가 5등)
    /// </summary>
    int[] highScores = null;

    /// <summary>
    /// 랭킹에 들어간 사람 이름(0번째가 1등, 4번째가 5등)
    /// </summary>
    string[] rankerNames = null;

    private void Awake()
    {        
        rankLines = GetComponentsInChildren<RankLine>();    // 모든 랭킹 라인 다 가져오기

        int size = rankLines.Length;
        highScores = new int[size];     // 배열 확보
        rankerNames = new string[size];

        LoadRankingData();              // 데이터 읽기(파일 없으면 디폴트)
    }

    void SaveRankingData()
    {
        //PlayerPrefs.SetInt("Score", 10);              // 컴퓨터에 Score라는 이름으로 10을 저장

        //SaveData saveData = new SaveData();
        SaveData saveData = new();  // 윗줄과 같은 코드(타입을 알 수 있기 때문에 생략한 것)
        saveData.rankerNames = rankerNames;             // 생성한 인스턴스에 데이터 기록
        saveData.highScores = highScores;

        string json = JsonUtility.ToJson(saveData);     // saveData에 있는 내용을 json 양식으로 설정된 string으로 변경
        
        string path = $"{Application.dataPath}/Save/";  // 저장될 경로 구하기(에디터에서는 Assets 폴더)
        if (!Directory.Exists(path))                    // path에 저장된 폴더가 있는지 확인
        {
            Directory.CreateDirectory(path);            // 폴더가 없으면 그 폴더를 만든다.
        }
        
        string fullPath = $"{path}Save.json";           // 전체 경로 = 폴더 + 파일이름 + 파일확장자
        File.WriteAllText(fullPath, json);              // fullPath에 json내용 파일로 기록하기        
    }

    bool LoadRankingData()
    {
        bool result = false;

        string path = $"{Application.dataPath}/Save/";              // 경로
        string fullPath = $"{path}Save.json";                       // 전체 경로

        result = Directory.Exists(path) && File.Exists(fullPath);   // 폴더와 파일이 있는지 확인

        if (result) 
        {
            // 폴더와 파일이 있으면 읽기
            string json = File.ReadAllText(fullPath);                   // 텍스트 파일 읽기
            SaveData loadData = JsonUtility.FromJson<SaveData>(json);   // json문자열을 파싱해서 SaveData에 넣기
            highScores = loadData.highScores;       // 실제로 최고 점수 넣기
            rankerNames= loadData.rankerNames;      // 이름 넣기
        }
        else
        {
            // 파일에서 못읽었으면 디폴트 값 주기
            int size = rankLines.Length;
            for (int i = 0; i < size; i++)
            {
                int resultScore = 1;
                for (int j = size - i; j > 0; j--)
                {
                    resultScore *= 10;
                }
                highScores[i] = resultScore; // 10만, 만, 천, 백, 십

                char temp = 'A';
                temp = (char)((byte)temp + i);
                rankerNames[i] = $"{temp}{temp}{temp}";     // AAA,BBB,CCC,DDD,EEE
            }
        }
        RefreshRankLines();     // 로딩이 되었으니 RankLines 갱신
        return result;
    }

    void RefreshRankLines()
    {
        for (int i = 0; i < rankLines.Length; i++)
        {
            rankLines[i].SetData(rankerNames[i], highScores[i]);
        }
    }

}
