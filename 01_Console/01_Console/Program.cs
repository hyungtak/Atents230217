////Console.WriteLine("Hello, World!");
////int studyHour = 3;
////Console.WriteLine($"고병조는 {studyHour}시간 공부를 한다.");
////int exercise = 2;
////Console.WriteLine($"고병조는 {exercise}시간 운동을 한다.");

////float f = 10.5f;
////bool isAlive = true; // false
////string str = "a123456789";
////str = "b123456789";

////// 변수 : 변하는 숫자. 필요한 데이터를 기록하는 곳
////// 데이터 타입
//////  int - 32bit 크기의 정수형(소수점 없는 숫자) 데이터 타입. (-21억~21억)
//////  float - 32bit 크기의 실수형(소수점 있는 숫자) 데이터 타입. 구조적으로 오차가 있는 데이터 타입이다.
//////  bool - 1bit 크기의 bool형. true 아니면 false만 가진다.
//////  string - 문자열을 기록하는 데이터 타입. 크기가 정해져 있지 않다. 변경 불가능한 데이터 타입이다.

////str = Console.ReadLine();
////Console.WriteLine($"내가 방금 입력한 것은 : {str}");


////// 제어문(Control Statement)

////// 조건문
////isAlive = true;
////if (isAlive)     // () 사이가 true면 아래 코드 실행
////{
////    Console.WriteLine("살아있다."); // isAlive가 true면 실행된다.
////}

////if (!isAlive)    // bool 타입 변수에 !가 붙으면 true는 false, false는 true로 된다.
////{
////    Console.WriteLine("죽어있다.");
////}

////if (isAlive)
////{
////    // if의 조건이 true이면 이 코드 실행
////    Console.WriteLine("살아있다.");
////}
////else
////{
////    // if의 조건이 false이면 이 코드 실행
////    Console.WriteLine("죽어있다.");
////}

////// 내가 입력받은 숫자가 5보다 크면 "5보다 크다"고 출력하고 아니면 "5보다 작거나 같다."고 출력해보기
////int number = 0;
////int.TryParse(str, out number);  // 글자를 숫자로 바꾸기
////if (number > 5)
////{
////    Console.WriteLine("5보다 크다.");
////}
////else
////{
////    Console.WriteLine("5보다 작거나 같다.");
////}

////if (number > 90)
////{
////    Console.WriteLine("A");
////}
////else if (number > 80)
////{
////    Console.WriteLine("B");
////}
////else if (number > 70)
////{
////    Console.WriteLine("C");
////}
////else
////{
////    Console.WriteLine("F");
////}

////switch (number)
////{
////    case 1:
////        //lsakjdlaksdl
////        break;
////    case 2:
////        //asdasd
////        break;
////    case 100:
////        // asdasdaaa
////        break;
////    default:
////        break;
////}


//// 반복문
////Console.WriteLine("반복할 횟수를 입력하세요 : ");
//string str;
//int number = 0;
////for(int i = 0; i < number;i++)      // i++   i=i+1    i+=1      3종류는 같은 것
////{
////    Console.WriteLine("출력");
////}

//// 구구단 단수 입력 받아서 구구단 출력해보기
//Console.WriteLine("구구단 단수를 입력하세요 : ");
//str = Console.ReadLine();
//bool result = int.TryParse(str, out number);
//if(result)
//{
//    for(int i=1;i<10;i++)   // 이 for는 i가 1에서 시작해서 i가 9가 될 때까기 i가 1씩 증가하며 실행된다.
//    {
//        Console.WriteLine($"{number} * {i} = {number * i}");
//    }
//}

//int j = 0;
//while(j<100)  // while은 ()사이가 true면 계속 실행된다.
//{
//    Console.WriteLine($"j : {j}");
//    j++;
//    if (j > 5)
//        break;  // j가 5보다 커지면 while(j<100)를 깨고 다음 줄을 실행한다.
//}

////while(true)
////{
////    Console.WriteLine("true");
////}

////do
////{

////} while (); // while과 같은데 무조건 {}를 한번은 실행한다.

// 디버거
// 단축키
//  F5 : 디버그 시작. 브레이크 포인트에서 계속 진행하기
//  F9 : 브레이크 포인트 설정
//  F10 : 한 줄씩 진행하기
//  F11 : 한 줄씩 진행하기 + 함수면 함수 안으로 들어가기

// 함수
using _01_Console;
///
string str;
int number2 = 0;
Console.Write("구구단 단수를 입력하세요 : ");
str = Console.ReadLine();
bool result = int.TryParse(str, out number2);
if (result)
{
    GuGudan(number2);
}
Console.WriteLine("구구단 출력 끝");

void GuGudan(int number)
{
    for (int i = 1; i < 10; i++)
    {
        Console.WriteLine($"{number} X {i} = {number * i}");
    }
}

Car car = new Car();
car.passenger = 20;
