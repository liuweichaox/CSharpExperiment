using System;

namespace 闭包
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<int, Func<int, int>> func = a => b => a + b;
            Func<int, int> func1 = func(2);
            Console.WriteLine(func1(5));
            Console.WriteLine(func1(7));      
      
           
        }
    }
}
