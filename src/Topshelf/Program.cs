using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Topshelf
{
    #region Topshelf教程
    /*
    所有操作需要管理员权限运行！！！
    Install-Package Topshelf
    Install-Package Topshelf.Log4Net
    安装：TopshelfDemo.exe install
    启动：TopshelfDemo.exe start
    卸载：TopshelfDemo.exe uninstall
     */
    #endregion
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<TownCrier>(s =>
                {
                    s.ConstructUsing(name => new TownCrier());
                    s.WhenStarted(tc => tc.Start());                   
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.SetDescription("Sample Topshelf Host");
                x.SetDisplayName("Stuff");
                x.SetServiceName("Stuff");
            });
        }
    }
    public class TownCrier
    {
        readonly System.Timers.Timer timer;
        public TownCrier()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 10000;
            timer.AutoReset = true;
            timer.Elapsed += (sender, eventArgs) => Console.WriteLine("It is {0} and all is well", DateTime.Now);
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start() { timer.Start(); }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop() { timer.Stop(); }
    }
}
