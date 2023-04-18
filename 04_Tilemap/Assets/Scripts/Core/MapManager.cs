using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
            loadWork.RemoveAll((x) => x == index);  // 중복으로 추가된 것을 모두 삭제
        }
        loadWorkComplete.Clear();
        foreach (var index in loadWork)             // 남아있는 loadWork를 로딩 시도
        {
            AsyncSceneLoad(index);
        }

        foreach (var index in unloadWorkComplete)  // 완료된 것은 unloadWork에서 제거
        {
            unloadWork.RemoveAll((x) => x == index);    // 중복으로 추가된 것을 모두 삭제
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

        Player player = GameManager.Inst.Player;
        if(player != null)
        {
            player.onMapMoved += (gridPos) =>
            {
                RefreshScenes(gridPos.x, gridPos.y);  // 맵 변경될 때마다 주변 로딩 요청
            };
            Vector2Int grid = WorldToGreed(player.transform.position);
            RequestAsyncSceneLoad(grid.x, grid.y);                                  // 플레이어 존재 맵을 최우선으로 로딩
            RefreshScenes(grid.x, grid.y);                                          // 주변 위치 로딩 요청
        }
    }

    int GetIndex(int x, int y)
    {
        return x + WidthCount * y;
    }

    /// <summary>
    /// 씬을 비동기로 로딩할 것을 요청하는 함수
    /// </summary>
    /// <param name="x">x좌표(맵의 위치)</param>
    /// <param name="y">y좌표(맵의 위치)</param>
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
        if (sceneLoadStates[index] == SceneLoadState.Loaded)         // 로딩 완료된 상태일 때만
        {
            sceneLoadStates[index] = SceneLoadState.PendingUnload;   // 언로드 진행중이라고 표시
            // 비동기로 언로드 시작
            AsyncOperation async = SceneManager.UnloadSceneAsync(sceneNames[index]);
            // 언로드가 끝나면 Unload 상태로 변경하는 람다함수를 델리게이트에 추가
            async.completed += (_) =>
            {
                sceneLoadStates[index] = SceneLoadState.Unload;      // 상태를 Unload로 변경
                unloadWorkComplete.Add(index);                      // 로딩 해제 완료 목록에 추가
            };
        }
    }

    /// <summary>
    /// 월드 좌표가 어떤 그리드에 있는지 계산하는 함수
    /// </summary>
    /// <param name="worldPos">확일할 월드 좌표</param>
    /// <returns>그리드 좌표(맵 기준)</returns>
    public Vector2Int WorldToGreed(Vector3 worldPos)
    {
        Vector2 offset = (Vector2)worldPos - totalOrigin;
        return new Vector2Int((int)(offset.x / mapWidthLenght), (int)(offset.y / mapHeightLength));
    }

    /// <summary>
    /// 지정된 그리드 위치(맵) 주변은 로딩을 하고, 그 외에는 전부 로딩을 해제하는 함수
    /// </summary>
    /// <param name="gridX"></param>
    /// <param name="gridY"></param>
    void RefreshScenes(int gridX, int gridY)
    {
        int startX = Mathf.Max(0, gridX - 1);           // 최소값은 0
        int endX = Mathf.Min(WidthCount, gridX + 2);    // 최대값은 WidthCount
        int startY = Mathf.Max(0, gridY - 1);           // 최소값은 0
        int endY = Mathf.Min(HeightCount, gridY + 2);   // 최대값은 HeightCount

        List<Vector2Int> open = new List<Vector2Int>(WidthCount * HeightCount);
        for (int y = startY; y < endY; y++)
        {
            for (int x = startX; x < endX; x++)
            {
                RequestAsyncSceneLoad(x, y);    // 적정 범위 안에 있는 것을 로드요청하고
                open.Add(new(x, y));            // 열린 것 목록에 추가
            }
        }

        // 그 외는 모두 로딩이 해제되어야 한다.
        Vector2Int target = new Vector2Int();   // 매번 new하는 것을 피하는 것이 목적
        for (int y = 0; y < HeightCount; y++)
        {
            for (int x = 0; x < WidthCount; x++)
            {
                target.x = x;
                target.y = y;
                if (!open.Exists((x) => x == target))   // open에 없으면
                {
                    RequestAsyncSceneUnload(x, y);      // 로드 해제 요청
                }
            }
        }
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
