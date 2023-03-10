using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_GameOver : Test_Base
{
    // 1번을 누르면 "/Assets/Save/TestSave.json"에 현재 점수 저장하기
    // 2번을 누르면 "/Assets/Save/TestSave.json"을 읽어서 Debug로 출력하기

    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        player.AddScore(700);
    }

    protected override void Test4(InputAction.CallbackContext _)
    {
        string path = $"{Application.dataPath}/Save/";      // 경로
        string fullPath = $"{path}TestSave.json";           // 전체 경로

        TestSaveData data = new TestSaveData();
        data.score = player.Score;
        string json = JsonUtility.ToJson(data);
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        File.WriteAllText(fullPath, json);
        Debug.Log("세이브 완료");
    }

    protected override void Test5(InputAction.CallbackContext _)
    {
        string path = $"{Application.dataPath}/Save/";      // 경로
        string fullPath = $"{path}TestSave.json";           // 전체 경로

        if(Directory.Exists(path) && File.Exists(fullPath))
        {
            string dataStr = File.ReadAllText(fullPath);
            TestSaveData data = JsonUtility.FromJson<TestSaveData>(dataStr);
            Debug.Log($"읽은 데이터 : {data.score}");
            player.AddScore(data.score);
        }
    }
}
