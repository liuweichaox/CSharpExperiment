using System;

namespace SingletonDemo
{
    class Program
    {

        static void Main(string[] args)
        {
            Singleton singleton2 = Singleton.GetInstance();

            Singleton singleton3 = Singleton.GetInstance();


            if (singleton2 == singleton3)

            {

                Console.WriteLine("实例singleton2与实例singleton3相同！");

            }

            Console.ReadKey();
        }
    }
}
