using System;
using System.Drawing;

namespace 委托
{
    class Program
    {
        public delegate int MethodDelegate(int x, int y);
        private static MethodDelegate method;
        static void Main(string[] args)
        {
            method = new MethodDelegate(Add);
            //method = delegate(int a,int b) { return a + b; };
            //method = (a, b) => a + b;
              
            Console.WriteLine(method(10, 20));

            Test<string>(Action, "Hello World!");
            Test<int>(Action, 1000);
            Test<string>(p => { Console.WriteLine("{0}", p); }, "Hello World");//使用Lambda表达式定义委托

            Console.WriteLine(Test<int, int>(Fun, 100, 200));

            Point[] points = {
                new Point(100, 200),
                new Point(150, 250),
                new Point(250, 375),
                new Point(275, 395),
                new Point(295, 450)
            };
            Point first = Array.Find(points, ProductGT10);
            Console.WriteLine("Found: X = {0}, Y = {1}", first.X, first.Y);

            Console.ReadKey();
        }
        private static int Add(int x, int y)
        {
            return x + y;
        }

        public static void Test<T>(Action<T> action, T p)
        {
            action(p);
        }
        private static void Action(string s)
        {
            Console.WriteLine(s);
        }
        private static void Action(int s)
        {
            Console.WriteLine(s);
        }

        public static int Test<T1, T2>(Func<T1, T2, int> func, T1 a, T2 b)
        {
            return func(a, b);
        }
        private static int Fun(int a, int b)
        {
            return a + b;
        }
        private static bool ProductGT10(Point p)
        {
            if (p.X * p.Y > 100000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
