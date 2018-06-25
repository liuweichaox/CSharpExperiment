using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace CoreWebDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // NLog：首先设置记录器以捕获所有错误
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("初始化 Main");
                BuildWebHost(args).Run();
            }
            catch (Exception exception)
            {
                // NLog：catch安装错误 
                logger.Error(exception, "由于异常停止程序");
                throw;
            }
            finally
            {
                //确保在退出应用程序之前刷新并停止内部定时器/线程（避免Linux上的分段错误）
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog()   // NLog：setup NLog用于依赖注入 
                .Build();
    }
}
