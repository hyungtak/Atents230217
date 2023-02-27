using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 싱글톤 : 객체를 하나만 가지는 디자인 패턴
/// </summary>

public class Singleton : MonoBehaviour
{
    // static : "정적"으로 번역. 프로그램 실행 전에 메모리 주소가 결정되어있는 것에 붙임.
    //          맴버 변수에 붙이면 클래스의 모든 객체에서 공용으로 사용할 수 있는 변수가 된다.
    // dynamic : "동적"으로 번역. 프로그램 실행 중에 메모리 주소가 결정되는 것에 붙임.

    /// <summary>
    /// 싱글톤용 객체. 다른 곳에서 접근 못하게 private으로 설정.
    /// </summary>
    private static Singleton instance;

    /// <summary>
    /// 싱글톤 읽기 전용 프로퍼티. 이 프로퍼티로만 싱글톤에 접근 가능.
    /// </summary>
    public static Singleton Inst
    {
        get
        {
            if( instance == null )  // 접근한 시점에서 instance가 있는지 없는지 확인
            {
                // 없으면 만들어진적이 없다.

                Singleton obj = FindObjectOfType<Singleton>();  // 에디터에서 만들어진 것이 있는지 확인
                if( obj == null )                               // null이면 에디터에서 만들어진것도 없다.
                {
                    GameObject gameObj = new GameObject();      // 빈 오브젝트 생성
                    gameObj.name = "Singleton";                 // 이름 변경하고
                    obj = gameObj.AddComponent<Singleton>();          // 싱글톤을 컴포넌트로 추가
                }

                instance = obj; // 없어서 새로 만든 것이든 에디터가 만들어 놓았던 것이든 instance에 저장
                DontDestroyOnLoad(obj.gameObject);  // 씬이 닫히더라도 게임 오브젝트가 삭제되지 않게 설정
            }
            return instance;    // instance 리턴(없었으면 새로 만들었고 있었으면 있던 것, 그래서 무조건 null이 아닌 값이 리턴된다.))
        }
    }

    private void Awake()
    {
        if( instance == null )  
        {
            // instance가 null이면 처음 생성 완료된 싱글톤 게임 오브젝트이다.(씬에 배치되어 있는 게임 오브젝트)
            instance = this;                        // instance에 이 싱글톤 객체 기록
            DontDestroyOnLoad(instance.gameObject); // 씬이 닫히더라도 게임 오브젝트가 삭제되지 않게 설정
        }
        else
        {
            // instance가 null이 아니면 이미 만들어진 싱글톤 게임 오브젝트가 있는 상황
            if( instance != this )
            {
                // Awake되기 전에 다른 코드에서 프로퍼티를 통해서 접근 했고 그래서 생성이 된 상황
                Destroy(this.gameObject);           // 나중에 만들어진 자기 자신을 삭제하기
            }
        }
    }
}

/// <summary>
/// 일반 싱글톤 예제
/// </summary>
public class TestSingleton
{
    // static변수를 만들어서 객체를 만들지 않고 사용할 수 있게 만들기
    private static TestSingleton instance = null;

    // 다른 곳에서 instance를 수정하지 못하도록 읽기 전용 프로퍼티 만들기
    public static TestSingleton Instance
    {
        get
        {
            if (instance == null)   // 처음 접근했을 때 new하기.
            {
                instance = new TestSingleton();
            }
            return instance;        // 항상 return될 때 값은 존재한다.
        }
    }

    // 중복생성 방지 목적. private으로 생성자를 만들어 기본 public생성자가 생성되지 않게 막기
    private TestSingleton()     
    {
    }
}
