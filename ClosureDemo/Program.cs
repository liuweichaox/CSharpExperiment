using System;

namespace ClosureDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<Func<int>> act = () =>
            {
                var count = 0;
                Func<int> func = () =>
                {
                    count++;
                    return count;
                };
                return func;
            };

            var func = act();
            Console.WriteLine(func());
            Console.WriteLine(func());
            Console.WriteLine(func());
        }
    }
}
