using System;
using System.Collections.Generic;
using System.Text;

namespace AutofacDynamicProxyDemo
{
    /// <summary>
    /// 通过静态代理实现AOP
    /// </summary>
    public class CatProxy : ICat
    {
        private readonly ICat _cat;
        public CatProxy(ICat cat)
        {
            _cat = cat;
        }
        public void Eat()
        {
            Console.WriteLine("猫吃东西之前");
            _cat.Eat();
            Console.WriteLine("猫吃东西之后");
        }
    }
}
