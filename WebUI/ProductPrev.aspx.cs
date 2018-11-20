using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public partial class ProductPrev : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        loginResult.Value = Request.Cookies["UserInfo"].Value;
        
        //string strAlias = Request["logPara"];
        //string loginInfo = Decode(strAlias, "transformer");
        //HttpCookie userCookie = new HttpCookie("User");
        //userCookie.Values.Add("indexUser", "JX");

        //HttpCookie newCookie = new HttpCookie("UserInfo", loginInfo);
        //newCookie.Expires = DateTime.Now.AddDays(1);
        //Response.Cookies.Add(newCookie);
        //Response.Cookies.Add(userCookie);
    }

    public string Decode(string str, string key)
    {
        DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
        provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
        provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));
        byte[] buffer = new byte[str.Length / 2];
        for (int i = 0; i < (str.Length / 2); i++)
        {
            int num2 = Convert.ToInt32(str.Substring(i * 2, 2), 0x10);
            buffer[i] = (byte)num2;
        }
        MemoryStream stream = new MemoryStream();
        CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
        stream2.Write(buffer, 0, buffer.Length);
        stream2.FlushFinalBlock();
        stream.Close();
        return Encoding.Default.GetString(stream.ToArray());
    }
}