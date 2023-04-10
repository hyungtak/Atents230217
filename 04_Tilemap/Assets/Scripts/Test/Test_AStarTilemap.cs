using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Test_AStarTilemap : Test_Base
{
    public Tilemap background;
    public Tilemap obstacle;
    public Transform start;
    public Transform end;

    GridMap map;

    private void Start()
    {
        map = new GridMap(background, obstacle);
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        Vector2Int startGrid = map.WorldToGrid(start.position);
        Debug.Log($"Start : {startGrid}");
        Vector2Int endGrid = map.WorldToGrid(end.position);
        Debug.Log($"End : {endGrid}");
    }

    private void Test_GetTileMapInfos()
    {
        // 타일맵의 크기
        Debug.Log($"Background : {background.size}");
        Debug.Log($"Obstacle : {obstacle.size}");

        // 타일맵의 원점(그리드좌표)
        Debug.Log($"Background origin : {background.origin}");
        Debug.Log($"Obstacle origin : {obstacle.origin}");

        // 타일맵 전체 순회하기
        for (int y = background.cellBounds.yMin; y < background.cellBounds.yMax; y++)
        {
            for (int x = background.cellBounds.xMin; x < background.cellBounds.xMax; x++)
            {
                TileBase tile = obstacle.GetTile(new(x, y));    // 특정 위치의 타일 가져오기
                if (tile != null)
                {
                    Debug.Log($"Obstacle Pos : ({x},{y})");
                }
            }
        }
    }
}