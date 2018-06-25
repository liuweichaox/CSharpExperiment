/*
 线程池

     在实际开发中使用的线程往往是大量的和更为复杂的，这时，每次都创建线程、启动线程。
     从性能上来讲，这样做并不理想（因为每使用一个线程就要创建一个，需要占用系统开销）；
     从操作上来讲，每次都要启动，比较麻烦。为此引入的线程池的概念。

 好处：

      1.减少在创建和销毁线程上所花的时间以及系统资源的开销 
      2.如不使用线程池，有可能造成系统创建大量线程而导致消耗完系统内存以及”过度切换”。

在什么情况下使用线程池？ 

    1.单个任务处理的时间比较短 
    2.需要处理的任务的数量大 

线程池最多管理线程数量=“处理器数 * 250”。也就是说，如果您的机器为2个2核CPU，那么CLR线程池的容量默认上限便是1000

通过线程池创建的线程默认为后台线程，优先级默认为Normal。
 */
using System;
using System.Collections.Generic;
using System.Threading;

namespace MyNetDemo
{
    class Program
    {


        public static void Main()
        {
            //声明一个Action委托的List，添加一些委托测试用
            List<Action> actions = new List<Action>
            {
                ()=>{Console.WriteLine("A-1");},
                ()=>{Console.WriteLine("A-2");},
                ()=>{Console.WriteLine("A-3");},
                ()=>{Console.WriteLine("A-4");}
            };

            Console.WriteLine("Main thread does some work, then sleeps.");

            ThreadPool.QueueUserWorkItem(state => ThreadProc("object", "jon"), null);

            //遍历输出结果
            foreach (var action in actions)
            {

                ThreadPool.QueueUserWorkItem(x => action(), null);
            }

            Console.WriteLine("Main thread exits.");

            Console.ReadKey();
        }

        static void ThreadProc(object stateInfo, string name)
        {
            Console.WriteLine(stateInfo);
            Console.WriteLine(name);
            // No state object was passed to QueueUserWorkItem,   
            // so stateInfo is null.              
            Console.WriteLine("Hello from the thread pool.");
        }
    }
}
