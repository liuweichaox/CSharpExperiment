using System;
using System.Net;
using System.Net.Mail;

namespace MyNetDemo
{
    class Program
    {
        static void Main(string[] args)
        {           
          SendMail("vanfj@qq.com","刘大大","572590415@qq.com","发送邮件测试","Hello World", "smtp.qq.com",587, "bqwccwimxdvebfhj");
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="From">设置来自此邮件地址</param>
        /// <param name="FromName">设置来自此邮件地址人的姓名</param>
        /// <param name="To">设置收件人地址</param>
        /// <param name="Subject">邮件的主题</param>
        /// <param name="Body">邮件的正文</param>
        /// <param name="SmtpcClient">Smtp主机IP地址</param>
        /// <param name="Port">Smtp主机端口</param>
        /// <param name="PassWord">授权码</param>
        public static void SendMail(string From, string FromName, string To, string Subject, string Body, string SmtpcClient, int Port, string PassWord)
        {
            //实例化一个发送邮件类。
            MailMessage mailMessage = new MailMessage();
            //发件人邮箱地址，方法重载不同，可以根据需求自行选择。
            mailMessage.From = new MailAddress(From, FromName, System.Text.Encoding.UTF8);
            //收件人邮箱地址。
            mailMessage.To.Add(new MailAddress(To));
            //邮件标题。
            mailMessage.Subject = Subject;
            //设置标题UTF8编码
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            //邮件内容UTF8编码
            mailMessage.Body = Body;
            //设置正文
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            //实例化一个SmtpClient类。
            SmtpClient client = new SmtpClient();
            //在这里我使用的是qq邮箱，所以是smtp.qq.com，如果你使用的是126邮箱，那么就是smtp.126.com。
            client.Host = SmtpcClient;
            //Smtp主机端口
            client.Port = Port;
            //SSL加密
            client.EnableSsl = true;
            //使用安全加密连接。
            client.EnableSsl = true;
            //不和请求一块发送。
            client.UseDefaultCredentials = false;
            //验证发件人身份(发件人的邮箱，邮箱里的生成授权码);
            client.Credentials = new NetworkCredential(From, PassWord);
            //发送
            client.Send(mailMessage);
        }

    }
}
