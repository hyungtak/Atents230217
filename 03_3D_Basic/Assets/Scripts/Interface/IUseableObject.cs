using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인터페이스는 기본적으로 public
// 인터페이스는 클래스에 여러개의 인터페이스를 상속시킬 수 있다.
// 인터페이스에는 구현이 포함되어있지 않아야 한다.
// 인터페이스를 상속받은 클래스는 반드시 인터페이스의 맴버 함수들을 구현해야 한다.
public interface IUseableObject
{
    void Used();
}
