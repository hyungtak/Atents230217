using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectPool : ObjectPool<Effect>
{
    private void Start()
    {
        Initialize();
    }
}
