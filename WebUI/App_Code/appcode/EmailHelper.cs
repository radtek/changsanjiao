using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Web;
using System.IO;

public class EmailHelper
{
    public static string m_mailAddresss, m_mailPassword, m_smtpHost;
    public static void InitConfig()
    {
        m_mailAddresss = ConfigurationManager.AppSettings["mailAddresss"].ToString();
        m_mailPassword = ConfigurationManager.AppSettings["mailPassword"].ToString();
        m_smtpHost = ConfigurationManager.AppSettings["smtpHost"].ToString();
    }
      
    ///<summary>
    /// 发送邮件
    ///<summary>
    ///<param name="subject"> 邮件标题</param>
    /// <param name="body">邮件正文</param>
    /// <param name="to">收件人</param>
    /// <param name="Ishtml">是否为html格式</param>
    //public static bool sendmail(string subject, string body, string to, bool Ishtml)
    //{
    //    try
    //    {
    //        MailMessage mm = new MailMessage();
    //        if (m_mailAddresss == null) InitConfig();
    //        MailAddress Fromma = new MailAddress(m_mailAddresss);
    //        MailAddress Toma = new MailAddress(to, null);
    //        mm.From = Fromma;
    //        //收件人
    //        mm.To.Add(to);
    //        //邮箱标题
    //        mm.Subject = subject;
    //        mm.IsBodyHtml = Ishtml;
    //        //邮件内容
    //        mm.Body = body;
    //        //内容的编码格式
    //        mm.BodyEncoding = System.Text.Encoding.UTF8;
    //        //mm.ReplyTo = Toma;
    //        //mm.Sender =Fromma;
    //        //mm.IsBodyHtml = false;
    //        mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
    //        //mm.CC.Add(Toma);//设置抄送
    //        SmtpClient sc = new SmtpClient();
    //        NetworkCredential nc = new NetworkCredential();
    //        nc.UserName = m_mailAddresss;//你的邮箱地址
    //        nc.Password = m_mailPassword;//你的邮箱密码,这里的密码是xxxxx@qq.com邮箱的密码，特别说明下~
    //        sc.UseDefaultCredentials = true;
    //        sc.DeliveryMethod = SmtpDeliveryMethod.Network;
    //        sc.Credentials = nc;
    //        //sc.EnableSsl = true;
    //        //如果这里报mail from address must be same as authorization user这个错误，是你的QQ邮箱没有开启SMTP，
    //        //到你自己的邮箱设置一下就可以啦！在帐户下面,如果是163邮箱的话，下面该成smtp.163.com
    //        if (to.Contains("163.com"))
    //            sc.Host = "smtp.163.com";
    //        else
    //            sc.Host = "smtp.smb.gov.cn";
    //        sc.Send(mm);
    //        return true;
    //    }
    //    catch(Exception ex)
    //    {
    //        InsertLog("发送邮件至" + to + "失败，原因如下：" + ex.Message);
    //        return false;
    //    }

    //}


    public static bool sendmail(string subject, string body, string to, bool Ishtml)
    {
        try
        {
            //从WEB.CONFIG文件中读取邮件发送者的信息
            if (m_mailAddresss == null) InitConfig();
            string mailAddressSender = m_mailAddresss;
            //邮件发送者显示名字
            string mailDisplayNameSender = "健康气象";
            //邮箱密码
            string mailPasswordSender = m_mailPassword;
            //smtp协议使用126发送邮件
            string SMTP_Server = m_smtpHost;
            //smtp端口号
            int SMTP_Port = 25;
            MailMessage MailMessage = new MailMessage();
            //设置基础编码信息
            MailMessage.SubjectEncoding = Encoding.UTF8;
            MailMessage.BodyEncoding = Encoding.UTF8;
            MailMessage.IsBodyHtml = true;
            //生成邮件发送者
            MailAddress MailAddress_Sender = new MailAddress(mailAddressSender, mailDisplayNameSender);
            MailMessage.From = MailAddress_Sender;
            //生成邮件接收者
            //配置文件中读取邮件接收地址
            //将邮件用;进行分割，生成数组
            MailMessage.To.Add(to);
            //赋值邮件主题
            MailMessage.Subject = subject;
            //赋值邮件内容
            MailMessage.Body = body;
            //发送邮件内容
            SmtpClient client = new SmtpClient(SMTP_Server, SMTP_Port);
            client.Credentials = new NetworkCredential(mailAddressSender, mailPasswordSender);
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.Send(MailMessage);
            return true; // 发送成功
        }
        catch (Exception ex)
        {
            InsertLog("发送邮件至" + to + "失败，原因如下：" + ex.StackTrace);
            return false;
        }

    }

    public static void InsertLog(string errors)
    {

        string direct = System.Web.HttpContext.Current.Server.MapPath("") + "\\SendLog";
        if (!Directory.Exists(direct)) Directory.CreateDirectory(direct);
        string logName = DateTime.Now.ToString("yyyyMMdd") + ".log";
        if (!File.Exists(direct + "\\" + logName)) File.Create(direct + "\\" + logName).Close();
        StreamWriter sr = new StreamWriter(direct + "\\" + logName, true, Encoding.UTF8);
        sr.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " " + errors);
        sr.Close();
    }
}


