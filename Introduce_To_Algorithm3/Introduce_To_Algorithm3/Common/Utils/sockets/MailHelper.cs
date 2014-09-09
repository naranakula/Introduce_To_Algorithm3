using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Net.Mail;

namespace Com.Utility.Commons
{
    public class MailHelper
    {
    	  /*
        /// <summary>
        /// 发送一个mail
        /// </summary>
        /// <param name="from">sender address</param>
        /// <param name="password">发件人密码 </param>
        /// <param name="to">recipient address</param>
        /// <param name="bcc">密件抄送人，可以为空，</param>
        /// <param name="cc">普通抄送，可以为空</param>
        /// <param name="subject">标题</param>
        /// <param name="body">邮件体</param>
        public static void SendMailMessage(string from,string password,string to,string bcc,string cc,string subject,string body)
        {
            //MailHelper.SendMailMessage("luchunminglu@gmail.com","558276355,"cmlu@iflytek.com",null,null,"test","Hello 你好");
            List<string> list = new List<string>();
            list.Add(to);
            try
            {
                SendMailMessage(from,password,list,null,null,"测试126","tick 126",SmtpConfiguration.M126SslSmtp.Host,SmtpConfiguration.M126SslSmtp.Port,SmtpConfiguration.M126SslSmtp.EnableSsl,false,100000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }*/

        /// <summary>
        /// 发送一个mail
        /// </summary>
        /// <param name="from">发件人地址，不能为空</param>
        /// <param name="password">发件人密码，不能为空</param>
        /// <param name="to">收件人列表，不能为空</param>
        /// <param name="bcc">密件抄送列表，可以为空或null</param>
        /// <param name="cc">普通抄送列表，可以为空或null</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件正文</param>
        /// <param name="host">smtp服务器</param>
        /// <param name="port">smtp端口号</param>
        /// <param name="enableSsl">是否使用加密套接字连接</param>
        /// <param name="isAsync">是否异步发送邮件</param>
        /// <param name="timeout"> 获取或设置一个值，该值指定同步 Send 调用的超时时间(单位毫秒)。</param>
        public static void SendMailMessage(string from,string password,IEnumerable<string> to,IEnumerable<string> bcc,IEnumerable<string> cc,string subject,string body,string host,int port,bool enableSsl,bool isAsync,int timeout)
        {
            MailMessage mailMessage = new MailMessage();
            //设置发件人地址
            mailMessage.From = new MailAddress(from);
            //设置收件人地址，可以设置多个收件人
            foreach (string item in to)
            {
                mailMessage.To.Add(new MailAddress(item));
            }
            
            //设置密件人
            if (bcc != null)
            {
                foreach (string item in bcc)
                {
                    mailMessage.Bcc.Add(new MailAddress(item));
                }
            }
            //设置普通抄送
            if (cc != null)
            {
                foreach (string item in cc)
                {
                    mailMessage.CC.Add(new MailAddress(item));
                }
            }

            //设置主题
            mailMessage.Subject = subject;
            //设置邮件的正文
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body;
            //设置邮件的优先级
            mailMessage.Priority = MailPriority.Normal;

            //实例化邮件传输客户端
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = host;
            smtpClient.Port = port;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(from, password);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = enableSsl;
            
            if (isAsync)
            {
                //异步发送，将时钟信号传递给回调函数。注：实际上未使用
                smtpClient.SendAsync(mailMessage, DateTime.Now.Ticks);
            }
            else
            {
                //系统默认是100000ms
                smtpClient.Timeout = timeout;
                smtpClient.Send(mailMessage);
            }
        }
    }

    /// <summary>
    /// Smtp的配置类
    /// </summary>
    public class SmtpConfiguration
    {
        /// <summary>
        /// 加密连接的谷歌邮件服务器
        /// </summary>
        public static SmtpConfiguration GmailSslSmtp = new SmtpConfiguration("smtp.gmail.com",587,true);

        /// <summary>
        /// 163邮件服务器
        /// </summary>
        public static SmtpConfiguration M163SslSmtp = new SmtpConfiguration("smtp.163.com",25,true);

        /// <summary>
        /// 126邮件服务器
        /// </summary>
        public static SmtpConfiguration M126SslSmtp = new SmtpConfiguration("smtp.126.com",25,true);

        /// <summary>
        /// 获取或设置用于 SMTP 事务的主机的名称或 IP 地址。
        /// </summary>
        public string Host;
        /// <summary>
        /// 获取或设置用于 SMTP 事务的端口。
        /// </summary>
        public int Port;
        /// <summary>
        /// 指定 SmtpClient 是否使用安全套接字层 (SSL) 加密连接
        /// </summary>
        public bool EnableSsl;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host">smtp服务器</param>
        /// <param name="port">smtp端口号</param>
        /// <param name="enableSsl">是否加密连接</param>
        public SmtpConfiguration(string host,int port,bool enableSsl)
        {
            Host = host;
            Port = port;
            EnableSsl = enableSsl;
        }

    }
}
