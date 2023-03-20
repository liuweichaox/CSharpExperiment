using System;
using System.Collections.Generic;
using System.Text;

namespace AutofacDynamicProxyDemo
{
    public class Cat : ICat
    {
        public void Eat()
        {
            Console.WriteLine("猫在吃东西");
        }
    }
}
