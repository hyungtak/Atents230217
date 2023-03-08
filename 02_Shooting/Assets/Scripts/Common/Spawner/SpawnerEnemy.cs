using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : Spawner
{    
    protected override void OnSpawn(EnemyBase enemy)
    {
        Fighter fighter = enemy as Fighter;
        if (fighter != null)
        {
            float r = Random.Range(minY, maxY);             // 랜덤하게 적용할 기준 높이 구하고
            fighter.BaseY = transform.position.y + r;         // 기준 높이 적용
        }
        else
        {
            Debug.LogError("SpawnerEnemy : 적 비행기가 아닌데 스폰하려고 함");
        }
    }
}
