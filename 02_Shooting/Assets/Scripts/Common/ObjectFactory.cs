using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enum(이넘) 타입
public enum PoolType
{
    Bullet = 0,
    Hit,
    Enemy,
    Explosion
}

public class ObjectFactory : Singleton<ObjectFactory>
{
    BulletPool bulletPool;
    EnemyPool enemyPool;
    ExplosionEffectPool explosionPool;
    HitEffectPool hitPool;

    protected override void PreInitialize()
    {
        bulletPool = GetComponentInChildren<BulletPool>();
        enemyPool = GetComponentInChildren<EnemyPool>();
        explosionPool = GetComponentInChildren<ExplosionEffectPool>();
        hitPool = GetComponentInChildren<HitEffectPool>();
    }

    protected override void Initialize()
    {
        bulletPool?.Initialize();       // ?.은 null이 아니면 실행, null이면 아무것도 하지 않는다.
        enemyPool?.Initialize();
        explosionPool?.Initialize();
        hitPool?.Initialize();
    }

    public PoolObject GetObject(PoolType type)
    {
        PoolObject result = null;
        switch (type)
        {
            case PoolType.Bullet:
                result = GetBullet();
                break;
            case PoolType.Hit:
                result = GetHitEffect();
                break;
            case PoolType.Enemy:
                result = GetEnemy();
                break;
            case PoolType.Explosion:
                result = GetExplosionEffect();
                break;         
        }
        return result;
    }

    public Bullet GetBullet() => bulletPool?.GetObject();
    public Effect GetHitEffect() => hitPool?.GetObject();
    public Enemy GetEnemy() => enemyPool?.GetObject();
    public Effect GetExplosionEffect() => explosionPool?.GetObject();

}
