using Quartz.Impl;
using Quartz.Logging;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Quartz.net
{
    public class Program
    {
        private static void Main(string[] args)
        {
            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

            RunProgramRunExample().GetAwaiter().GetResult();

            Console.WriteLine("Press any key to close the application");
            Console.ReadKey();
        }

        private static async Task RunProgramRunExample()
        {
            try
            {
                //从工厂抓取调度器实例
                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                IScheduler scheduler = await factory.GetScheduler();

                // 开始任务
                await scheduler.Start();

                //定义工作并把它与我们的工作班联系起来
                IJobDetail job = JobBuilder.Create<HelloJob>()
                    .WithIdentity("job1", "group1")
                    .Build();

                // 触发作业运行，然后每隔1秒重复一次
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(1)                    
                    .RepeatForever())
                    .EndAt(new DateTimeOffset(DateTime.Now.AddMinutes(0.5)))
                    .Build();
       
                // 告诉工厂使用我们的触发器安排作业
                await scheduler.ScheduleJob(job, trigger);

                // 任务睡眠来显示正在发生的事情
                //await Task.Delay(TimeSpan.FromSeconds(30));

                // 最后在您关闭程序时关闭调度器
                //await scheduler.Shutdown();
            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
        }

        // 简单日志提供程序到控制台
        private class ConsoleLogProvider : ILogProvider
        {
            public Logger GetLogger(string name)
            {
                return (level, func, exception, parameters) =>
                {
                    if (level >= LogLevel.Info && func != null)
                    {
                        Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
                    }
                    return true;
                };
            }

            public IDisposable OpenNestedContext(string message)
            {
                throw new NotImplementedException();
            }

            public IDisposable OpenMappedContext(string key, string value)
            {
                throw new NotImplementedException();
            }
        }
    }

    /// <summary>
    ///继承 IJob 接口 实现 Execute方法
    /// </summary>
    public class HelloJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("DateTimeOffset Now\t" + DateTimeOffset.Now + "\nDateTimeOffset.UtcNow\t" + DateTimeOffset.UtcNow + "\nDateTime Now\t\t" + DateTime.Now + "\nDateTime UtcNow\t\t" + DateTime.UtcNow + "\n");
        }
    }
}
