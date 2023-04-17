using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    /// <summary>
    /// 서브맵의 세로 개수
    /// </summary>
    const int HeightCount = 3;

    /// <summary>
    /// 서브맵의 가로 개수
    /// </summary>
    const int WidthCount = 3;

    /// <summary>
    /// 서브맵 하나의 세로 길이
    /// </summary>
    const float mapHeightLength = 20.0f;
    /// <summary>
    /// 서브맵 하나의 가로 길이
    /// </summary>
    const float mapWidthLenght = 20.0f;

    /// <summary>
    /// 전체 맵의 왼쪽 아래 끝지점
    /// </summary>
    readonly Vector2 totalOrigin = new Vector2(-mapWidthLenght * WidthCount * 0.5f, -mapHeightLength * HeightCount * 0.5f);

    /// <summary>
    /// 씬 이름 조합용 기본이름
    /// </summary>
    const string SceneNameBase = "Seemless";
    /// <summary>
    /// 조합이 완료된 모든 씬의 이름 배열
    /// </summary>
    string[] sceneNames;

    /// <summary>
    /// 씬의 로딩 상태를 나타낼 Enum
    /// </summary>
    enum SceneLoadState : byte
    {
        Unload = 0,             //로딩 안되어있음
        PendingUnload,          //로딩 해제 진행중
        PendingLoad,            //로딩 진행중
        Loaded                    //로딩 완료됨
    }

    /// <summary>
    /// 모든 씬의 로딩 상태
    /// </summary>
    SceneLoadState[] sceneLoadStates;

    /// <summary>
    /// 모든 씬이 언로드 되었음을 확인하기 위한 프로퍼티. 모든 씬이 언로드 되었으면 True, 아니면 False
    /// </summary>
    public bool IsUnloadAll
    {
        get
        {
            bool result = true;
            foreach( var state in sceneLoadStates)
            {
                if(state != SceneLoadState.Unload)
                {
                    result = false;
                    break;

                }
            }
            return result;
        }
    }

    List<int> loadWork = new List<int>();
    List<int> loadWorkComplete = new List<int>();
    List<int> unloadWork = new List<int>();
    List<int> unloadWorkComplete = new List<int>();

    private void Update()
    {
        foreach (var index in loadWorkComplete)      // 완료된 것은 loadWork에서 제거
        {
            loadWork.Remove(index);
        }
        loadWorkComplete.Clear();
        foreach (var index in loadWork)             // 남아있는 loadWork를 로딩 시도
        {
            AsyncSceneLoad(index);
        }

        foreach (var index in unloadWorkComplete)  // 완료된 것은 unloadWork에서 제거
        {
            unloadWork.Remove(index);
        }
        unloadWorkComplete.Clear();
        foreach (var index in unloadWork)          // 남아있는 unloadWork를 로딩 해제 시도
        {
            AsyncSceneUnload(index);
        }
    }

    public void PreInitialize()
    {
        sceneNames = new string[HeightCount * WidthCount];
        sceneLoadStates = new SceneLoadState[HeightCount * WidthCount];
        for (int y=0; y < HeightCount; y++) 
        {
            for (int x = 0; x <WidthCount;x++)
            {
                int index = GetIndex(x, y);

                sceneNames[index] = $"{SceneNameBase}_{x}_{y}";
                sceneLoadStates[index] = SceneLoadState.Unload;
            }
        }
    }

    public void Initialize()
    {
        for (int i =0; i < sceneLoadStates.Length; i++)
        {
            sceneLoadStates[i] = SceneLoadState.Unload;
        }
        
        //플레이어 기준으로 플레이어 주변의 맵만 로딩하기
    }

    int GetIndex(int x, int y)
    {
        return x + WidthCount * y;
    }

    void RequestAsyncSceneLoad(int x, int y)
    {
        int index = GetIndex(x, y);
        if (sceneLoadStates[index] == SceneLoadState.Unload)
        {
            loadWork.Add(index);    // 로딩이 안된 맵이면 loadWork에 추가
        }
    }

    void AsyncSceneLoad(int index)
    {
        if (sceneLoadStates[index] == SceneLoadState.Unload)        //언로드 상태일때만 로딩 시도
        {
            sceneLoadStates[index] = SceneLoadState.PendingLoad;                  //로딩 중이라고 표시

            AsyncOperation async = SceneManager.LoadSceneAsync(sceneNames[index], LoadSceneMode.Additive);      //비동기로 로딩 시작

            async.completed += (_) =>
            {
                sceneLoadStates[index] = SceneLoadState.Loaded;                             //로딩이 끝나면 load상태로 변경하는 함수를 델리게이트에 추가.
                loadWorkComplete.Add(index);
            };
        }
    }

    void RequestAsyncSceneUnload(int x, int y)
    {
        int index = GetIndex(x, y);
        if (sceneLoadStates[index] == SceneLoadState.Loaded)
        {
            unloadWork.Add(index);  // 로딩이 되어있는 맵이면 unloadWork에 추가
        }

        // 슬라임을 풀로 되돌려서 삭제 되지 않게 만들기
        Scene scene = SceneManager.GetSceneByName(sceneNames[index]);   // 해당 씬 가져오기
        if (scene.isLoaded) // 로드 되어 있으면
        {
            GameObject[] sceneObjs = scene.GetRootGameObjects();    // 씬의 루트 오브젝트 전부 가져오기
            if (sceneObjs != null && sceneObjs.Length > 0)          // 오브젝트가 하나라도 있으면
            {
                Slime[] slimes = sceneObjs[0].GetComponentsInChildren<Slime>(); // 그 안에서 슬라임 전부 찾기
                foreach (var slime in slimes)
                {
                    slime.ReturnToPool();                           // 슬라임을 모두 풀로 되돌리기
                }
            }
        }
    }

    void AsyncSceneUnload(int index)
    {
        if (sceneLoadStates[index] == SceneLoadState.Loaded)
        {
            sceneLoadStates[index] = SceneLoadState.PendingUnload;
            //비동기로 언로드 시작
            AsyncOperation async = SceneManager.UnloadSceneAsync(sceneNames[index]);
            // 언로드가 끝나면 Unload상태로 변경하는 람다 함수를 델리게이트에 추가.
            async.completed += (_) =>
            {
                sceneLoadStates[index] = SceneLoadState.Unload;      // 상태를 Unload로 변경
                unloadWorkComplete.Add(index);                      // 로딩 해제 완료 목록에 추가
            };
        }
    }

    void WorldToGreed(Vector2 pos)
    {
        pos -= totalOrigin;
        float greedX = pos.x / mapWidthLenght;          //맵의 x좌표
        float greedY = pos.y / mapHeightLength;         //맵의 y좌표
        int index = GetIndex((int)greedX, (int)greedY);     //맵의 인덱스
        float mapGreedX = pos.x % mapWidthLenght;       //맵 내 x좌표
        float mapGreedY = pos.y % mapHeightLength;      //맵 내 y좌표
    }   

#if UNITY_EDITOR
    public void Test_LoadScene(int x, int y)
    {
        RequestAsyncSceneLoad(x, y);
    }

    public void Test_UnloadScene(int x, int y)
    {
        RequestAsyncSceneUnload(x, y);
    }

#endif
}
