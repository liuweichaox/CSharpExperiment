/*
线程的和进程的关系以及优缺点:
     windows系统是一个多线程的操作系统。一个程序至少有一个进程,一个进程至少有一个线程。
     进程是线程的容器，一个C#客户端程序开始于一个单独的线程，CLR(公共语言运行库)为该进程创建了一个线程，该线程称为主线程。
     例如当我们创建一个C#控制台程序，程序的入口是Main()函数，Main()函数是始于一个主线程的。它的功能主要 是产生新的线程和执行程序。

C#是一门支持多线程的编程语言，通过Thread类创建子线程，引入using System.Threading命名空间。 

多线程的优点：
1、 多线程可以提高CPU的利用率，因为当一个线程处于等待状态的时候，CPU会去执行另外的线程
2、 提高了CPU的利用率，就可以直接提高程序的整体执行速度
多线程的缺点：
1、线程开的越多，内存占用越大
2、协调和管理代码的难度加大，需要CPU时间跟踪线程
3、线程之间对资源的共享可能会产生可不遇知的问题

C# 多线程
线程被定义为程序的执行路径。每个线程都定义了一个独特的控制流。如果您的应用程序涉及到复杂的和耗时的操作，那么设置不同的线程执行路径往往是有益的，每个线程执行特定的工作。
线程是轻量级进程。一个使用线程的常见实例是现代操作系统中并行编程的实现。使用线程节省了 CPU 周期的浪费，同时提高了应用程序的效率。
到目前为止我们编写的程序是一个单线程作为应用程序的运行实例的单一的过程运行的。但是，这样子应用程序同时只能执行一个任务。为了同时执行多个任务，它可以被划分为更小的线程。

线程生命周期
线程生命周期开始于 System.Threading.Thread 类的对象被创建时，结束于线程被终止或完成执行时。
下面列出了线程生命周期中的各种状态：
未启动状态：当线程实例被创建但 Start 方法未被调用时的状况。
就绪状态：当线程准备好运行并等待 CPU 周期时的状况。
不可运行状态：下面的几种情况下线程是不可运行的：
              已经调用 Sleep 方法
              已经调用 Wait 方法
              通过 I/O 操作阻塞
死亡状态：当线程已完成执行或已中止时的状况。

主线程
在 C# 中，System.Threading.Thread 类用于线程的工作。它允许创建并访问多线程应用程序中的单个线程。进程中第一个被执行的线程称为主线程。

当 C# 程序开始执行时，主线程自动创建。使用 Thread 类创建的线程被主线程的子线程调用。您可以使用 Thread 类的 CurrentThread 属性访问线程。

Thread 类常用的属性和方法
下表列出了 Thread 类的一些常用的 属性：
属性	                 描述
CurrentContext	         获取线程正在其中执行的当前上下文。
CurrentCulture	         获取或设置当前线程的区域性。
CurrentPrinciple         获取或设置线程的当前负责人（对基于角色的安全性而言）。
CurrentThread	         获取当前正在运行的线程。
CurrentUICulture	     获取或设置资源管理器使用的当前区域性以便在运行时查找区域性特定的资源。
ExecutionContext	     获取一个 ExecutionContext 对象，该对象包含有关当前线程的各种上下文的信息。
IsAlive	                 获取一个值，该值指示当前线程的执行状态。
IsBackground	         获取或设置一个值，该值指示某个线程是否为后台线程。
IsThreadPoolThread	     获取一个值，该值指示线程是否属于托管线程池。
ManagedThreadId	         获取当前托管线程的唯一标识符。
Name	                 获取或设置线程的名称。
Priority	             获取或设置一个值，该值指示线程的调度优先级。
ThreadState	             获取一个值，该值包含当前线程的状态。

下面列出了 Thread 类的一些常用的 方法：

序号       方法名 & 描述
1   public void Abort()
    在调用此方法的线程上引发 ThreadAbortException，以开始终止此线程的过程。调用此方法通常会终止线程。
2   public static LocalDataStoreSlot AllocateDataSlot()
    在所有的线程上分配未命名的数据槽。为了获得更好的性能，请改用以 ThreadStaticAttribute 属性标记的字段。
3	public static LocalDataStoreSlot AllocateNamedDataSlot( string name)
    在所有线程上分配已命名的数据槽。为了获得更好的性能，请改用以 ThreadStaticAttribute 属性标记的字段。
4	public static void BeginCriticalRegion()
    通知主机执行将要进入一个代码区域，在该代码区域内线程中止或未经处理的异常的影响可能会危害应用程序域中的其他任务。
5	public static void BeginThreadAffinity()
    通知主机托管代码将要执行依赖于当前物理操作系统线程的标识的指令。
6	public static void EndCriticalRegion()
    通知主机执行将要进入一个代码区域，在该代码区域内线程中止或未经处理的异常仅影响当前任务。
7	public static void EndThreadAffinity()
    通知主机托管代码已执行完依赖于当前物理操作系统线程的标识的指令。
8	public static void FreeNamedDataSlot(string name)
    为进程中的所有线程消除名称与槽之间的关联。为了获得更好的性能，请改用以 ThreadStaticAttribute 属性标记的字段。
9	public static Object GetData( LocalDataStoreSlot slot )
    在当前线程的当前域中从当前线程上指定的槽中检索值。为了获得更好的性能，请改用以 ThreadStaticAttribute 属性标记的字段。
10	public static AppDomain GetDomain()
    返回当前线程正在其中运行的当前域。
11	public static AppDomain GetDomainID()
    返回唯一的应用程序域标识符。
12	public static LocalDataStoreSlot GetNamedDataSlot( string name )
    查找已命名的数据槽。为了获得更好的性能，请改用以 ThreadStaticAttribute 属性标记的字段。
13	public void Interrupt()
    中断处于 WaitSleepJoin 线程状态的线程。
14	public void Join()
    在继续执行标准的 COM 和 SendMessage 消息泵处理期间，阻塞调用线程，直到某个线程终止为止。此方法有不同的重载形式。
15	public static void MemoryBarrier()
    按如下方式同步内存存取：执行当前线程的处理器在对指令重新排序时，不能采用先执行 MemoryBarrier 调用之后的内存存取，再执行 MemoryBarrier 调用之前的内存存取的方式。
16	public static void ResetAbort()
    取消为当前线程请求的 Abort。
17	public static void SetData( LocalDataStoreSlot slot, Object data )
    在当前正在运行的线程上为此线程的当前域在指定槽中设置数据。为了获得更好的性能，请改用以 ThreadStaticAttribute 属性标记的字段。
18	public void Start()
    开始一个线程。
19	public static void Sleep( int millisecondsTimeout )
    让线程暂停一段时间。
20	public static void SpinWait( int iterations )
    导致线程等待由 iterations 参数定义的时间量。
21	public static byte VolatileRead( ref byte address )
    public static double VolatileRead( ref double address )
    public static int VolatileRead( ref int address )
    public static Object VolatileRead( ref Object address ) 
    读取字段值。无论处理器的数目或处理器缓存的状态如何，该值都是由计算机的任何处理器写入的最新值。此方法有不同的重载形式。这里只给出了一些形式。
22	public static void VolatileWrite( ref byte address, byte value )
    public static void VolatileWrite( ref double address, double value )
    public static void VolatileWrite( ref int address, int value )
    public static void VolatileWrite( ref Object address, Object value ) 
    立即向字段写入一个值，以使该值对计算机中的所有处理器都可见。此方法有不同的重载形式。这里只给出了一些形式。
23	public static bool Yield()导致调用线程执行准备好在当前处理器上运行的另一个线程。由操作系统选择要执行的线程。
 */using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MyNetDemo
{
    class Program
    {
        static void Main(string[] args)
        {
           
            Console.WriteLine("主线程开始 ManagedThreadId " + Thread.CurrentThread.ManagedThreadId);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //ThreadStart这个委托定义为void ThreadStart(),也就是说,所执行的方法不能有参数。
            Thread t1 = new Thread(new ThreadStart(TestMethod));

            //ParameterThreadStart的定义为void ParameterizedThreadStart(object state)，使用这个这个委托定义的线程的启动函数可以接受一个输入参数
            Thread t2 = new Thread(new ParameterizedThreadStart(TestMethod));

            //匿名函数无参数实现
            Thread t3 = new Thread(delegate ()
            {
                Console.WriteLine("不带参数的线程函数 ManagedThreadId " + Thread.CurrentThread.ManagedThreadId);
            });

            //Lambda无参数实现
            Thread t4 = new Thread(() =>
            {
                Console.WriteLine("不带参数的线程函数 ManagedThreadId " + Thread.CurrentThread.ManagedThreadId);
            });

            //匿名函数带参数实现
            Thread t5 = new Thread(delegate (object data)
            {
                Console.WriteLine("带参数的线程函数 ManagedThreadId " + Thread.CurrentThread.ManagedThreadId + " 参数：" + data);
            });

            //Lambda带参数实现
            Thread t6 = new Thread((data) =>
            {
                Console.WriteLine("带参数的线程函数 ManagedThreadId " + Thread.CurrentThread.ManagedThreadId + " 参数：" + data);
            });

            //C#中的线程分为前台线程和后台线程，线程创建时不做设置默认是前台线程。即线程属性IsBackground=false。

            //t1.IsBackground = true;
            //t2.IsBackground = true;
            //t3.IsBackground = true;
            //t4.IsBackground = true;
            //t5.IsBackground = true;
            //t6.IsBackground = true;
            /*
            区别以及如何使用：

            这两者的区别就是：应用程序必须运行完所有的前台线程才可以退出；

            而对于后台线程，应用程序则可以不考虑其是否已经运行完毕而直接退出，所有的后台线程在应用程序退出时都会自动结束。

            一般后台线程用于处理时间较短的任务，如在一个Web服务器中可以利用后台线程来处理客户端发过来的请求信息。

            而前台线程一般用于处理需要长时间等待的任务，如在Web服务器中的监听客户端请求的程序。

            * 线程是寄托在进程上的，进程都结束了，线程也就不复存在了！

            * 只要有一个前台线程未退出，进程就不会终止！即说的就是程序不会关闭！（即在资源管理器中可以看到进程未结束。）*/
            t1.Start();
            t2.Start("TestMethod");
            t3.Start();
            t4.Start();
            t5.Start("delegate");
            t6.Start("lambda");
            Console.WriteLine("主线程结束");
            stopwatch.Stop();
            Console.WriteLine("耗时 " + stopwatch.Elapsed.TotalSeconds);
            Console.ReadKey();

        }

        public static void TestMethod()
        {
            Console.WriteLine("不带参数的线程函数 ManagedThreadId " + Thread.CurrentThread.ManagedThreadId);
        }
        public static void TestMethod(object data)
        {

            string datastr = data as string;
            Console.WriteLine("带参数的线程函数 ManagedThreadId " + Thread.CurrentThread.ManagedThreadId + " 参数：" + data);
        }

    }
}
