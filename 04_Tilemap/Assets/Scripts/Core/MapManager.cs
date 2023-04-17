using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    string[] secneNames;

    /// <summary>
    /// 씬의 로딩 상태를 나타낼 Enum
    /// </summary>
    enum SceneLoadState : byte
    {
        Unload = 0,             //로딩 안되어있음
        PendingUnload,          //로딩 해제 진행중
        PendingLoad,            //로딩 진행중
        Load                    //로딩 완료됨
    }

    /// <summary>
    /// 모든 씬의 로딩 상태
    /// </summary>
    SceneLoadState[] sceneLoadState;

    /// <summary>
    /// 모든 씬이 언로드 되었음을 확인하기 위한 프로퍼티. 모든 씬이 언로드 되었으면 True, 아니면 False
    /// </summary>
    public bool IsUnloadAll
    {
        get
        {
            bool result = true;
            foreach( var state in sceneLoadState)
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


    public void PreInitialize()
    {

    }

    public void Initialize()
    {

    }


}
