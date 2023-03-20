using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutofacDynamicProxyDemo
{
 
    /// <summary>
    /// 定义一个拦截器
    /// </summary>
    public class CatInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("猫吃东西之前");
            invocation.Proceed();
            Console.WriteLine("猫吃东西之后");     
        }
    }
}
