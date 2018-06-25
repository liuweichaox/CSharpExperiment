using System;
using System.Collections.Generic;
using System.Text;

namespace Autofac高级用法之动态代理
{
    public class Cat : ICat
    {
        public void Eat()
        {
            Console.WriteLine("猫在吃东西");
        }
    }
}
