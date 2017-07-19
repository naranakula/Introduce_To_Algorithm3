using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 使用smtpClient发送邮件
    /// </summary>
    public static class MailHelper
    {
        /// <summary>
        /// MailHelper.Send126Mail("luchunminglu@126.com", "558276344Z", new List<string>() { "luchunminglu@163.com", "luchunminglu@gmail.com" }, "测试主题", "测试内容");
        /// </summary>
        /// <param name="from">luchunminglu@126.com</param>
        /// <param name="password">558276344Z</param>
        /// <param name="to">收件人列表</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static bool Send126Mail(string from,string password,IEnumerable<string> to,string subject,string body,Action<Exception> exceptionHandler=null)
        {
            return SendMailSafe(from, password, to, subject, body, "smtp.126.com", 25, exceptionHandler, null, null, true);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="from">发件人地址，不能为空</param>
        /// <param name="password">发件人密码，不能为空</param>
        /// <param name="to">收件人列表，不能为空</param>
        /// <param name="subject">邮件主题，不能为空</param>
        /// <param name="body">邮件正文，可以为空  正文可以使用html格式</param>
        /// <param name="host">smtp服务器地址</param>
        /// <param name="port">smtp端口号</param>
        /// <param name="bcc">密件抄送列表，可以为空或null</param>
        /// <param name="cc">普通抄送列表，可以为空或null</param>
        /// <param name="enableSsl">是否使用加密套接字连接</param>
        /// <param name="timeout">指定同步 Send 调用的超时时间(单位毫秒)</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <returns></returns>
        public static bool SendMailSafe(string from,string password,IEnumerable<string> to,string subject,string body,string host,int port,Action<Exception> exceptionHandler = null,IEnumerable < string> bcc=null,IEnumerable<string> cc=null,bool enableSsl = false,int timeout = 11000)
        {
            try
            {
                //邮件消息
                using (MailMessage mailMessage = new MailMessage())
                {
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

                    //设置邮件主题
                    mailMessage.Subject = subject == null ? "" : subject;
                    //设置邮件正文 邮件正文是Html格式
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = body == null ? "" : body;
                    //设置邮件的优先级
                    mailMessage.Priority = MailPriority.Normal;

                    //实例化邮件传输客户端
                    using (SmtpClient smtpClient = new SmtpClient())
                    {
                        smtpClient.Host = host;
                        smtpClient.Port = port;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(from, password);
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.EnableSsl = enableSsl;
                        //if (isAsync)
                        //{
                        //    //异步发送，将时钟信号传递给回调函数。注：实际上未使用
                        //    smtpClient.SendAsync(mailMessage, DateTime.Now.Ticks);
                        //}
                        //else
                        //{
                        //    //系统默认是100000ms
                        //    smtpClient.Timeout = timeout;
                        //    smtpClient.Send(mailMessage);
                        //}

                        //系统默认是100000ms
                        smtpClient.Timeout = timeout;
                        smtpClient.Send(mailMessage);
                    }
                }
                return true;
            }catch(Exception ex)
            {
                if(exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return false;
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
        public static SmtpConfiguration GmailSslSmtp = new SmtpConfiguration("smtp.gmail.com", 587, true);

        /// <summary>
        /// 163邮件服务器
        /// </summary>
        public static SmtpConfiguration M163SslSmtp = new SmtpConfiguration("smtp.163.com", 25, true);

        /// <summary>
        /// 126邮件服务器
        /// 建议使用这个
        /// </summary>
        public static SmtpConfiguration M126SslSmtp = new SmtpConfiguration("smtp.126.com", 25, true);

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
        public SmtpConfiguration(string host, int port, bool enableSsl)
        {
            Host = host;
            Port = port;
            EnableSsl = enableSsl;
        }

    }
}
