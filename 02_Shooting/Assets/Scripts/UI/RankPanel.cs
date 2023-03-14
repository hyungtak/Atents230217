using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    /// <summary>
    /// 최대 랭킹 표시 수
    /// </summary>
    int rankCount = 5;

    /// <summary>
    /// 랭킹이 업데이트 되지 않았음을 표시하는 상수
    /// </summary>
    const int NotUpdated = -1;

    /// <summary>
    /// 현재 업데이트 된 랭킹의 인덱스
    /// </summary>
    int updatedIndex = NotUpdated;

    /// <summary>
    /// 인풋 필드 컴포넌트
    /// </summary>
    TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();  // 컴포넌트 찾고
        inputField.onEndEdit.AddListener(OnNameInputEnd);       // 입력이 끝났을 때 실행될 함수 등록

        rankLines = GetComponentsInChildren<RankLine>();        // 모든 랭킹 라인 다 가져오기
        rankCount = rankLines.Length;           // UI 갯수에 맞게 최대 랭킹 갯수 설정
        highScores = new int[rankCount];        // 배열 확보
        rankerNames = new string[rankCount];
    }

    private void Start()
    {
        inputField.gameObject.SetActive(false);     // 시작할 때 인풋 필드 안보이게 만들기
        Player player = FindObjectOfType<Player>();
        player.onDie += RankUpdate;         // 플레이어가 죽었을 때 랭크 업데이트 시도
        LoadRankingData();                  // 데이터 읽기(파일 없으면 디폴트)
    }

    /// <summary>
    /// 이름 입력이 완료되었을 때 실행되는 함수
    /// </summary>
    /// <param name="text">입력받은 이름</param>
    private void OnNameInputEnd(string text)
    {
        rankerNames[updatedIndex] = text;       // 입력받은 텍스트를 해당 랭커의 이름으로 지정
        inputField.gameObject.SetActive(false); // 입력 완료되었으니 다시 안보이게 만들기
        SaveRankingData();  // 새로 저장하고 
        RefreshRankLines(); // UI 갱신
    }

    /// <summary>
    /// 랭킹 업데이트 하는 함수
    /// </summary>
    /// <param name="player">점수를 가지고 있는 플레이어</param>
    private void RankUpdate(Player player)
    {
        int newScore = player.Score;    // 새 점수
        for(int i=0;i<rankCount; i++)   // 랭킹 1등부터 5등까지 확인
        {
            if(highScores[i] < newScore)    // 새 점수가 현재 랭킹보다 높으면
            {
                for(int j = rankCount-1; j>i;j--)           // 현재 랭킹부터 한칸씩 아래로 밀기
                {
                    highScores[j] = highScores[j - 1];
                    rankerNames[j] = rankerNames[j - 1];
                }
                highScores[i] = newScore;   // 새 점수를 현재 랭킹에 끼워 넣기
                rankerNames[i] = "";
                updatedIndex = i;           // 랭킹 업데이트된 인덱스 기억

                Vector3 newPos = inputField.transform.position;
                newPos.y = rankLines[i].transform.position.y;
                inputField.transform.position = newPos;     // 인풋 필드의 위치를 랭크라인과 같게 만들기
                //inputField.ActivateInputField();
                inputField.gameObject.SetActive(true);      // 인풋 필드 활성화
                break;  // 랭킹 삽입 될 곳을 찾았으니 중지
            }
        }
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

    /// <summary>
    /// 랭크 라인 화면 업데이트용 함수
    /// </summary>
    void RefreshRankLines()
    {
        for (int i = 0; i < rankLines.Length; i++)
        {
            rankLines[i].SetData(rankerNames[i], highScores[i]);
        }
    }

}
