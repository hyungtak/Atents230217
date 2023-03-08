using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enum(이넘) 타입
public enum PoolObjectType
{
    Bullet = 0,
    Hit,
    Enemy,
    SpecialFighter,
    Asteroid,
    AsteroidSmall,
    Explosion,
    PowerUp
}

/// <summary>
/// 오브젝트 생성해주는 클래스
/// </summary>
public class Factory : Singleton<Factory>
{
    // 생성할 오브젝트의 풀들
    BulletPool bulletPool;
    EnemyPool enemyPool;
    SpecialFighterPool specialFighterPool;
    ExplosionEffectPool explosionPool;
    HitEffectPool hitPool;
    AsteroidPool asteroidPool;
    AsteroidSmallPool asteroidSmallPool;
    PowerUpPool powerUpPool;

    /// <summary>
    /// 이 싱글톤이 만들어질 때 처음 한번만 호출될 함수
    /// </summary>
    protected override void PreInitialize()
    {
        // 자식으로 붙어있는 풀들 다 찾아놓기
        bulletPool = GetComponentInChildren<BulletPool>();
        enemyPool = GetComponentInChildren<EnemyPool>();
        specialFighterPool = GetComponentInChildren<SpecialFighterPool>();
        explosionPool = GetComponentInChildren<ExplosionEffectPool>();
        hitPool = GetComponentInChildren<HitEffectPool>();
        asteroidPool = GetComponentInChildren<AsteroidPool>();
        asteroidSmallPool = GetComponentInChildren<AsteroidSmallPool>();
        powerUpPool = GetComponentInChildren<PowerUpPool>();
    }

    /// <summary>
    /// 씬이 로드될 때마다 호출되는 초기화 함수
    /// </summary>
    protected override void Initialize()
    {
        bulletPool?.Initialize();       // ?.은 null이 아니면 실행, null이면 아무것도 하지 않는다.
        enemyPool?.Initialize();
        specialFighterPool?.Initialize();
        explosionPool?.Initialize();
        hitPool?.Initialize();
        asteroidPool?.Initialize();
        asteroidSmallPool?.Initialize();
        powerUpPool?.Initialize();
    }

    /// <summary>
    /// 지정된 오브젝트를 풀에서 꺼내주는 함수
    /// </summary>
    /// <param name="type">꺼낼 오브젝트의 종류</param>
    /// <returns>꺼낸 오브젝트의 게임 오브젝트</returns>
    public GameObject GetObject(PoolObjectType type)
    {
        GameObject result = null;
        switch (type)       // type에 맞게 꺼내서 result에 저장
        {
            case PoolObjectType.Bullet:
                result = GetBullet().gameObject;
                break;
            case PoolObjectType.Hit:
                result = GetHitEffect().gameObject;
                break;
            case PoolObjectType.Enemy:
                result = GetEnemy().gameObject;
                break;
            case PoolObjectType.SpecialFighter:
                result = GetSpecialFigher().gameObject;
                break;
            case PoolObjectType.Explosion:
                result = GetExplosionEffect().gameObject;
                break;
            case PoolObjectType.Asteroid:
                result = GetAsteroid().gameObject;
                break;
            case PoolObjectType.AsteroidSmall:
                result = GetAsteroidSmall().gameObject;
                break;
            case PoolObjectType.PowerUp: 
                result = GetPowerUp().gameObject;
                break;
        }
        return result;      // result를 리턴. 타입이 없는 타입이면 null
    }

    /// <summary>
    /// Bullet풀에서 Bullet하나 꺼내는 함수
    /// </summary>
    /// <returns></returns>
    public Bullet GetBullet() => bulletPool?.GetObject();

    /// <summary>
    /// HitEffect풀에서 HitEffect하나 꺼내는 함수
    /// </summary>
    /// <returns></returns>
    public Effect GetHitEffect() => hitPool?.GetObject();

    /// <summary>
    /// Enemy풀에서 Enemy하나 꺼내는 함수
    /// </summary>
    /// <returns></returns>
    public Fighter GetEnemy() => enemyPool?.GetObject();

    /// <summary>
    /// specialFighterPool에서 특수적 하나 꺼내는 함수
    /// </summary>
    /// <returns></returns>
    public SpecialFighter GetSpecialFigher() => specialFighterPool?.GetObject();

    /// <summary>
    /// ExplosionEffect풀에서 ExplosionEffect하나 꺼내는 함수
    /// </summary>
    /// <returns></returns>
    public Effect GetExplosionEffect() => explosionPool?.GetObject();

    /// <summary>
    /// AsteroidPool풀에서 운석 하나 꺼내는 함수
    /// </summary>
    /// <returns></returns>
    public Asteroid GetAsteroid() => asteroidPool?.GetObject();

    /// <summary>
    /// AsteroidSmallPool풀에서 작은 운석 하나 꺼내는 함수
    /// </summary>
    /// <returns></returns>
    public AsteroidBase GetAsteroidSmall() => asteroidSmallPool?.GetObject();

    /// <summary>
    /// PowerUpPool에서 파워업 하나 꺼내는 함수
    /// </summary>
    /// <returns></returns>
    public PowerUp GetPowerUp() => powerUpPool?.GetObject();

}
