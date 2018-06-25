using Autofac;
using Autofac.Extras.DynamicProxy;
using System;

namespace Autofac高级用法之动态代理
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("静态代理实现AOP：");

            ICat icat = new Cat();

            var catProxy = new CatProxy(icat);

            catProxy.Eat();

            Console.WriteLine("DynamicProxy实现AOP：");

            var builder = new ContainerBuilder();
            builder.RegisterType<CatInterceptor>();//注册拦截器
            builder.RegisterType<Cat>().As<ICat>().InterceptedBy(typeof(CatInterceptor)).EnableInterfaceInterceptors();//注册Cat并为其添加拦截器
            var container = builder.Build();
            var cat = container.Resolve<ICat>();
            cat.Eat();

            Console.Read();


        }
    }
}
