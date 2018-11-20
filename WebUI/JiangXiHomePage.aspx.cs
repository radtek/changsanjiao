using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.IO;
using System.Text;

public partial class JiangXiHomePage : System.Web.UI.Page
{
    public string logInfo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        logInfo = Encode(Request.Cookies["UserInfo"].Value,"transformer");
    }
    //加密方法
    public static string Encode(string str, string key)
    {
        DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
        provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
        provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        MemoryStream stream = new MemoryStream();
        CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
        stream2.Write(bytes, 0, bytes.Length);
        stream2.FlushFinalBlock();
        StringBuilder builder = new StringBuilder();
        foreach (byte num in stream.ToArray())
        {
            builder.AppendFormat("{0:X2}", num);
        }
        stream.Close();
        return builder.ToString();
    }

}