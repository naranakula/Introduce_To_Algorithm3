using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils
{
    /// <summary>
    /// 网址：https://github.com/jstedfast/MailKit
    /// 构建在MimeKit之上的跨平台右键客户端
    /// </summary>
    public static class MailKitHelper
    {

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
        public static bool SendMailSafe(string from, string password, IEnumerable<string> to, string subject, string body, string host, int port, Action<Exception> exceptionHandler = null, IEnumerable<string> bcc = null, IEnumerable<string> cc = null, bool enableSsl = false, int timeout = 11000)
        {
            try
            {
                //邮件消息
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(from));
                foreach(string item in to)
                {
                    message.To.Add(new MailboxAddress(item));
                }

                //设置邮件主题
                message.Subject = subject == null ? "" : subject;
                //设置邮件内容
                message.Body = new TextPart(TextFormat.Plain) { Text = body };

                //MailKit的client
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    //连接邮件服务器
                    client.Connect(host, port, enableSsl);
                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(from, password);

                    client.Send(message);
                    client.Disconnect(true);
                }


                return true;
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return false;
            }
        }
    }
}
