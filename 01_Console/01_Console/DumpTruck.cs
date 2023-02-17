using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Console
{
    // 접근 제한자(Access Modifier)
    // private      : 자기 자신만 접근할 수 있다.
    // protected    : 자신과 자신을 상속받은 대상만 접근할 수 있다.
    // public       : 모두가 접근 할 수 있다.

    // Car는 DumpTruck의 부모 클래스
    // DumpTruck은 Car의 자식 클래스(파생 클래스)
    public class DumpTruck : Car
    {
        void Cargo() 
        {
            oil = 100;
            passenger = 120;
        }
    }
}
