using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService
{
    public partial class MyService : ServiceBase
    {
        public MyService()
        {
            InitializeComponent();
        }
        //定时器
        System.Timers.Timer tmBak = new System.Timers.Timer();

        /// <summary>
        ///服务器启动时写日志、开启定时器
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("D:\\log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Linq Service Start.");
            }
            //到时间的时候执行事件  1000毫秒/一秒
            tmBak.Interval = 10000;//一分钟执行一次
            tmBak.AutoReset = true;//执行一次 false，一直执行true 
            //是否执行System.Timers.Timer.Elapsed事件 
            tmBak.Enabled = true;
            tmBak.Start();
            //达到间隔时间时发生
            tmBak.Elapsed += new System.Timers.ElapsedEventHandler(SQLBak);

        }
        /// <summary>
        ///服务停止时写日志
        /// </summary>
        protected override void OnStop()
        {
            tmBak.Stop();
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("D:\\log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Linq Service Stop.");
            }
           
        }
        protected override void OnContinue()
        {         
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("D:\\log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Linq Service Continue.");
            }
        }
        protected override void OnPause()
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("D:\\log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Linq Service Pause.");
            }
        }
        protected override void OnShutdown()
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("D:\\log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Linq Service Shutdown.");
            }
        }    
        /// <summary>
        /// SQL数据库备份
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void SQLBak(object source, System.Timers.ElapsedEventArgs e)
        {
            //如果当前时间是10点30分
            if (DateTime.Now.Hour == 10 && DateTime.Now.Minute == 30)
            {
                //指定时间备份
            }
            string sql = string.Format(@"
                BACKUP DATABASE Linq 
                TO DISK = N'D:\DBBak\DATABASENAME {0}{1}{2}.bak'--目录一定要存在
                WITH INIT , NOUNLOAD , 
                NAME = N'DataBase Backup', --名字随便取
                NOSKIP , 
                STATS = 10, 
                NOFORMAT", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter("D:\\log.txt", true))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在备份Linq 数据库......");
                }
                using (SqlConnection conn = new SqlConnection("server=.;uid=sa;pwd=123;database=Linq"))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();                   
                }

            }
            catch (Exception ex)
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter("D:\\log.txt", true))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 备份Linq 数据库出现异常：" + ex.Message);
                    return;
                }
            }

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("D:\\log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 备份Linq 数据库成功！");
            }
        }
    }
}
