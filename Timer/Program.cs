using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Timer
{
    class Program
    {

        static void Main(string[] args)
        {
            using (StreamWriter sw = new StreamWriter("D:\\Sleep.txt", true))
            {
                sw.WriteLine($"{DateTime.Now}");
            }
            //while (true)
            //{
            //    if (DateTime.Now.Hour==18&&DateTime.Now.Minute==0)
            //    {
            //        using (StreamWriter sw = new StreamWriter("D:\\Sleep.txt", true))
            //        {
            //            sw.WriteLine($"{DateTime.Now}");
            //        }
            //        break;
            //    }
            //}

            #region Timer定时任务
            /*           
            使用windows自带的命令sc
            使用sc create 方法创建。
            sc create CaptureScreen binpath= F:\zwmei-project\decklink-learning\OutputBitmap\Debug\OutputBitmap.exe
            type= own 
            start= auto 
            displayname= Screen_Capture
            CaptureScreen为服务名，可以在系统服务中找到，（通过在命令行运行services.msc打开系统服务）。
            binpath 为你的应用程序所在的路径。 
            displayname 为服务显示的名称,这个在注册表中可以找到，（通过在命令行中输入regedit打开注册表，在HKEY_LOCAL_MACHINE -- SYSTEM -- CurrentControlSet 下找到你的服务显示名）
            注意：在sc命令中，=号前面不能有空格，而=号后面必须有一个空格，切记。另外要以管理员的身份打开命令行。
            这种方法不一定能成功，如果你的exe不符合服务的规范，可能会启动失败。
                        System.Timers.Timer timer = new System.Timers.Timer();
                        //True 一直执行， False 只执行一次
                        timer.Interval = 1000;
                        timer.AutoReset = false;

                        if (DateTime.Now.Hour == 00 && DateTime.Now.Minute == 00)
                        {
                        //定时任务
                        using (StreamWriter sw = new StreamWriter("D:\\Sleep.txt", true))
                        {
                         sw.WriteLine($"{DateTime.Now}");
                        }
                        }
                        timer.Enabled = true;
                        timer.Start();
                        timer.Elapsed += (s, e) =>
                        {
                        using (StreamWriter sw = new StreamWriter("D:\\Sleep.txt", true))
                        {
                         sw.WriteLine($"{DateTime.Now}");
                        }
     
            };*/
            #endregion

        }

    }
}
