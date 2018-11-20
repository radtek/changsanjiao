using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using NetDimension.Weibo;
using QWeiboSDK;
using System.Collections.Specialized;
using System.Net;
using System.IO;
namespace WeiBo
{
    public class SendWeiBo
    {
        string m_sinakey;
        string m_sinascrect;
        string m_sinauser;
        string m_sinapsd;
        string m_tokenkey;
        string m_tokenscrect;
        string m_appkey;
        string m_appscreat;
        private NetDimension.Weibo.OAuth oauth;
        private NetDimension.Weibo.Client Sina;
        protected static readonly log4net.ILog m_Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public SendWeiBo()
        {
            try
            {

                //获取连接数据库的参数
                ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["Sina"];
                string constring = settings.ConnectionString;
                string[] parts = constring.Split(new char[] { ';', '=' }, StringSplitOptions.None);
                m_sinakey = parts[3];
                m_sinascrect=parts[5];
                m_sinauser=parts[7];
                m_sinapsd = parts[9];
                settings = ConfigurationManager.ConnectionStrings["Tencent"];
                constring = settings.ConnectionString;
                parts = constring.Split(new char[] { ';', '=' }, StringSplitOptions.None);
                m_appkey = parts[3];
                m_appscreat = parts[5];
                m_tokenkey=parts[7];
                m_tokenscrect=parts[9];
               


               
            }
            catch (Exception ex)
            {
                m_Log.Error("SendWeiBo", ex);

            }
        }
        public string  SendSina(string content)
        {
            oauth = new OAuth(m_sinakey, m_sinascrect, "www.readearth.com.cn");
            oauth.ClientLogin(m_sinauser, m_sinapsd);
            NetDimension.Weibo.TokenResult tr = oauth.VerifierAccessToken();
            if (tr != NetDimension.Weibo.TokenResult.Success)
            {
                return tr.ToString();
            }
            try
            {
                Sina = new NetDimension.Weibo.Client(oauth);
                Sina.API.Statuses.Update(content, 0, 0, null);
                return "成功";

            }
            catch (Exception ex)
            {
                return ex.ToString();
                m_Log.Error("SendWeiBo.SendSina", ex);
            }
        }
        public string HttpPost(string url, NameValueCollection nvc)
        {
            HttpWebRequest requestScore = (HttpWebRequest)WebRequest.Create(url);
            ASCIIEncoding encoding = new ASCIIEncoding();

            Encoding myEncoding = Encoding.GetEncoding("UTF-8");
            UTF8Encoding utf8 = new UTF8Encoding();
            string postData = "";
            foreach (string key in nvc.Keys)
            {
                if (postData == "")
                    postData += (key + "=" + nvc[key]);
                else
                    postData += ("&" + key + "=" + nvc[key]);
                ;
            }
            byte[] data = myEncoding.GetBytes(postData);
            requestScore.Accept = "Accept:textml,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            requestScore.Headers["Accept-Language"] = "zh-CN,zh;q=0.";
            requestScore.Headers["Accept-Charset"] = "GBK,utf-8;q=0.7,*;q=0.3";
            requestScore.Method = "Post";
            requestScore.ContentType = "application/x-www-form-urlencoded";
            requestScore.ContentLength = data.Length;
            requestScore.KeepAlive = true;

            //使用cookies
            //requestScore.CookieContainer = ...;
            Stream stream = requestScore.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            HttpWebResponse responseSorce = (HttpWebResponse)requestScore.GetResponse();
            StreamReader reader = new StreamReader(responseSorce.GetResponseStream(), Encoding.UTF8);
            return reader.ReadToEnd();
        }

        public string SendTencent(string content)
        {
            try
            {
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("content", content);
                nvc.Add("clientip", "127.0.0.1");
                nvc.Add("format", "xml");
                nvc.Add("oauth_consumer_key", m_appkey);
                nvc.Add("access_token", m_tokenkey);
                nvc.Add("openid", m_tokenscrect);
                nvc.Add("oauth_version", "2.a");
                string result = HttpPost("https://open.t.qq.com/api/t/add", nvc);
                if (result.Contains("<msg>ok</msg>"))
                    return "成功";
                else
                    return "失败";
            }
            catch(Exception ex)
            {
                return ex.ToString();
                m_Log.Error("SendWeiBo.SendTencent", ex);
            }
        }
    }
}
