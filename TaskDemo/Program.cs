/*
 Task是.NET4.0加入的，跟线程池ThreadPool的功能类似，用Task开启新任务时，会从线程池中调用线程，而Thread每次实例化都会创建一个新的线程。

使用基于 .NET 任务的异步模型可直接编写绑定 I/O 和 CPU 的异步代码。 
该模型由 Task 和 Task<T> 类型以及 C# 和 Visual Basic 中的 async 和 await 关键字公开。

任务和 Task<T>
任务是用于实现称之为并发 Promise 模型的构造。 简单地说，它们“承诺”，会在稍后完成工作，让你使用干净的 API 与 promise 协作。
    Task 表示不返回值的单个操作。
    Task<T> 表示返回 T 类型的值的单个操作。

请务必将任务理解为工作的异步抽象，而非在线程之上的抽象。 默认情况下，任务在当前线程上执行，且在适当时会将工作委托给操作系统。 
可选择性地通过 Task.Run API 显式请求任务在独立线程上运行。任务会公开一个 API 协议来监视、等候和访问任务的结果值（如 Task<T>）。
含有 await 关键字的语言集成可提供高级别抽象来使用任务。任务运行时，使用 await 在任务完成前将控制让步于其调用方，可让应用程序和服务执行有用工作。
任务完成后代码无需依靠回调或时间便可继续执行。 语言和任务 API 集成会为你完成此操作。 如果正在使用 Task<T>，任务完成时，await 关键字还将“打开”返回的值。
 */using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MyNetDemo
{
    class Program
    {

        static int book = 1;
        private static readonly object obj = new object();
        static void Main(string[] args)
        {

            Console.WriteLine("主线程启动");
            //var ts=new Program().GetFirstCharactersCountAsync();
            //Console.WriteLine(ts.Result); 
            //Task.Run启动一个线程
            //Task启动的是后台线程，要在主线程中等待后台线程执行完毕，可以调用Wait方法
            /*
             在.NET Framework 4.5开发者预览版中，微软引进了新的Task.Run方法。
             新方法不是为了替代旧的Task.Factory.StartNew方法，只是提供了一种使用Task.Factory.StartNew方法的更简洁的形式，而不需要去指定那一系列参数。
             这是一个捷径，事实上，Task.Run的内部实现逻辑跟Task.Factory.StartNew一样，只是传递了一些默认参数。
             */
            //Task task = Task.Factory.StartNew(() => { Thread.Sleep(1500); Console.WriteLine("task启动"); });
            //Task task = Task.Run(() =>{Thread.Sleep(1500);Console.WriteLine("task启动");});
            //Thread.Sleep(3000);
            //task.Wait();

            //ForSetProcuct_1000000();
            //ParallelForSetProcuct_1000000();

            Thread.Sleep(3000);
            Stopwatch swTask = new Stopwatch();
            swTask.Start();
            /*执行并行操作*/
            Parallel.Invoke(() => SetProcuct1_500(), () => SetProcuct2_500(), () => SetProcuct3_500(), () => SetProcuct4_500());
            swTask.Stop();
            Console.WriteLine("500条数据 并行编程所耗时间:" + swTask.ElapsedMilliseconds);

            Thread.Sleep(3000);/*防止并行操作 与 顺序操作冲突*/
            Stopwatch sw = new Stopwatch();
            sw.Start();
            SetProcuct1_500();
            SetProcuct2_500();
            SetProcuct3_500();
            SetProcuct4_500();
            sw.Stop();
            Console.WriteLine("500条数据  顺序编程所耗时间:" + sw.ElapsedMilliseconds);

            Thread.Sleep(3000);
            swTask.Restart();
            /*执行并行操作*/
            Parallel.Invoke(() => SetProcuct1_100000(), () => SetProcuct2_100000(), () => SetProcuct3_100000(), () => SetProcuct4_100000());
            swTask.Stop();
            Console.WriteLine("100000条数据 并行编程所耗时间:" + swTask.ElapsedMilliseconds);

            Thread.Sleep(3000);
            sw.Restart();
            SetProcuct1_100000();
            SetProcuct2_100000();
            SetProcuct3_100000();
            SetProcuct4_100000();
            sw.Stop();
            Console.WriteLine("100000条数据 顺序编程所耗时间:" + sw.ElapsedMilliseconds);

            Thread.Sleep(3000);
            swTask.Restart();
            var ts = Task.Run(() =>
               {
                   SetProcuct1_100000();
                   SetProcuct2_100000();
                   SetProcuct3_100000();
                   SetProcuct4_100000();
                   return "This is Task Result";
               }).ContinueWith(task =>
               {
                   var result = task.Result;
                   Console.WriteLine(result);
                   return "This is Task Continue Result";
               });
            ts.Wait();
            if (ts.IsCompletedSuccessfully)
            {
                swTask.Stop();
                Console.WriteLine("Task 100000条数据 并发编程所耗时间:" + swTask.ElapsedMilliseconds);
            }

            //Parallel.For(0, 2, item => { UpBook(); });
            for (int i = 0; i < 10; i++)
            {
                //ThreadPool.QueueUserWorkItem(state => { UpBook_Lock(); });
                //new System.Threading.Thread(UpBook_Lock).Start();
                //Task.Run(() => { UpBook_NotLock(); });
            }
            Console.WriteLine("主线程结束");
            Console.ReadKey();

        }
        public static void UpBook_NotLock()
        {
            if (book > 0)
            {
                Thread.Sleep(1000);
                book -= 1;
                Console.WriteLine($"售出一本图书，还剩余{book}本,当前线程-{Thread.CurrentThread.ManagedThreadId}");
            }
            else
            {
                Console.WriteLine($"图书已售完，当前线程-{Thread.CurrentThread.ManagedThreadId}");
            }
        }
        public static void UpBook_Lock()
        {
            lock (obj)
            {
                if (book > 0)
                {
                    Thread.Sleep(1000);
                    book -= 1;
                    Console.WriteLine($"售出一本图书，还剩余{book}本,当前线程-{Thread.CurrentThread.ManagedThreadId}");
                }
                else
                {
                    Console.WriteLine($"图书已售完，当前线程-{Thread.CurrentThread.ManagedThreadId}");
                }
            }
        }
        /*
        开启新任务的方法：Task.Run()或者Task.Factory.StartNew()，开启的是后台线程

        要在主线程中等待后台线程执行完毕，可以使用Wait方法(会以同步的方式来执行)。不用Wait则会以异步的方式来执行。

        比较一下Task和Thread：
         static void Main(string[] args)
            {
                for (int i = 0; i < 5; i++)
                {
                    new Thread(Run1).Start();
                }
                for (int i = 0; i < 5; i++)
                {
                    Task.Run(() => { Run2(); });
                }
            }
            static void Run1()
            {
                Console.WriteLine("Thread Id =" + Thread.CurrentThread.ManagedThreadId);
            }
            static void Run2()
            {
                Console.WriteLine("Task调用的Thread Id =" + Thread.CurrentThread.ManagedThreadId);
            }
            直接用Thread会开启5个线程，用Task(用了线程池)开启了3个
         */
        private static void SetProcuct1_500()
        {
            List<Product> ProductList = new List<Product>();
            for (int index = 1; index < 500; index++)
            {
                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                ProductList.Add(model);
            }
            Console.WriteLine("SetProcuct1 执行完成");
        }
        private static void SetProcuct2_500()
        {
            List<Product> ProductList = new List<Product>();
            for (int index = 500; index < 1000; index++)
            {
                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                ProductList.Add(model);
            }
            Console.WriteLine("SetProcuct2 执行完成");
        }
        private static void SetProcuct3_500()
        {
            List<Product> ProductList = new List<Product>();
            for (int index = 1000; index < 1500; index++)
            {
                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                ProductList.Add(model);
            }
            Console.WriteLine("SetProcuct3 执行完成");
        }
        private static void SetProcuct4_500()
        {
            List<Product> ProductList = new List<Product>();
            for (int index = 1500; index < 2000; index++)
            {
                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                ProductList.Add(model);
            }
            Console.WriteLine("SetProcuct4 执行完成");
        }
        private static void SetProcuct1_100000()
        {
            List<Product> ProductList = new List<Product>();
            for (int index = 1; index < 100000; index++)
            {
                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                ProductList.Add(model);
            }
            Console.WriteLine("SetProcuct1 执行完成");
        }
        private static void SetProcuct2_100000()
        {
            List<Product> ProductList = new List<Product>();
            for (int index = 100000; index < 200000; index++)
            {
                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                ProductList.Add(model);
            }
            Console.WriteLine("SetProcuct2 执行完成");
        }
        private static void SetProcuct3_100000()
        {
            List<Product> ProductList = new List<Product>();
            for (int index = 200000; index < 300000; index++)
            {
                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                ProductList.Add(model);
            }
            Console.WriteLine("SetProcuct3 执行完成");
        }
        private static void SetProcuct4_100000()
        {
            List<Product> ProductList = new List<Product>();
            for (int index = 300000; index < 4000000; index++)
            {
                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                ProductList.Add(model);
            }
            Console.WriteLine("SetProcuct4 执行完成");
        }
        private static void ForSetProcuct_1000000()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<Product> ProductList = new List<Product>();
            for (int index = 1; index < 1000000; index++)
            {
                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                ProductList.Add(model);
                Console.WriteLine("for SetProcuct index: {0}", index);
            }
            sw.Stop();
            Console.WriteLine("for SetProcuct 1000000 执行完成 耗时：{0}", sw.ElapsedMilliseconds);
        }
        private static void ParallelForSetProcuct_1000000()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<Product> ProductList = new List<Product>();
            Parallel.For(1, 1000000, index =>
            {
                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                ProductList.Add(model);
                Console.WriteLine("ForSetProcuct SetProcuct index: {0}", index);
            });
            sw.Stop();
            Console.WriteLine("ForSetProcuct SetProcuct 1000000 执行完成 耗时：{0}", sw.ElapsedMilliseconds);
        }
        public async Task<string> GetFirstCharactersCountAsync()
        {
            // Execution is synchronous here
            var client = new HttpClient();

            // Execution of GetFirstCharactersCountAsync() is yielded to the caller here
            // GetStringAsync returns a Task<string>, which is *awaited*
            var page = await client.GetStringAsync("http://www.dotnetfoundation.org");

            // Execution resumes when the client.GetStringAsync task completes,
            // becoming synchronous again.
            return page;
        }
    }
}
class Product
{
    public string Name { get; set; } = "name";

    public string Category { get; set; } = "category";

    public int SellPrice { get; set; } = 999;
}
class BookShop
{
    //剩余图书数量
    public int num = 1;
    public void Sale()
    {
        //使用lock关键字解决线程同步问题
        lock (this)
        {
            int tmp = num;
            if (tmp > 0)//判断是否有书，如果有就可以卖
            {
                Thread.Sleep(1000);
                num -= 1;
                Console.WriteLine("售出一本图书，还剩余{0}本", num);
            }
            else
            {
                Console.WriteLine("没有了");
            }
        }
    }
}