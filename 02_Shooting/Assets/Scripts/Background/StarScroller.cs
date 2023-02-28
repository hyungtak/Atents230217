using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScroller : Scroller
{
    SpriteRenderer[] spriteRenderers;

    protected override void Awake()
    {
        base.Awake();   // Scroller의 Awake 실행하고

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();    // 자식으로 있는 SpriteRenderer 모두 찾기
    }

    protected override void MoveRightEnd(int index)
    {
        base.MoveRightEnd(index);   // Scroller의 MoveRightEnd 실행하고

        int rand = Random.Range(0, 4);  // 0(0b_00), 1(0b_01), 2(0b_10), 3(0b_11)

        spriteRenderers[index].flipX = ((rand & 0b_01) != 0);   // 첫번째 비트가 1이면 true, 아니면 false
        spriteRenderers[index].flipY = ((rand & 0b_10) != 0);   // 두번째 비트가 1이면 true, 아니면 false
    }
}
