using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Shader : Test_Base
{
    // 0번 : outline
    // 1,2번 : phase
    // 3,4번 : dissolve

    public SpriteRenderer[] spriteRenderers;
    Material[] materials;    

    protected override void Awake()
    {
        base.Awake();
        materials = new Material[spriteRenderers.Length];
        for(int i=0;i<spriteRenderers.Length;i++)
        {
            materials[i] = spriteRenderers[i].material;
        }
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        materials[0].SetFloat("_Thickness", 0.0f);
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        materials[0].SetFloat("_Thickness", 0.005f);
    }

    private void Update()
    {
        // 숫자를 0~1사이로 계속 왕복하게 만들기
        // phase와 dissolve가 계속 핑퐁되도록 만들기

        // 1,2번 : phase
        // 3,4번 : dissolve
    }
}
