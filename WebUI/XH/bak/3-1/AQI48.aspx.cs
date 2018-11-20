using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public partial class Comforecast_AQI48 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static List<string> GetDatas(string date)
    {
        //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://10.228.177.99/PEMFCShare/Product/FtpTemp/AQIPeriod/20170101161407_Text_AQI_SH_" + date + "1700.txt");//创建请求
        //request.Method = "GET";//设置访问方式
        //HttpWebResponse result = request.GetResponse() as HttpWebResponse;
        //StreamReader sr = new StreamReader(result.GetResponseStream(), Encoding.Default);
        string strResult = GetFtp48AQITXT();
        if (strResult == "") throw new Exception("");
        List<string> strReturn = new List<string>();
        Regex regex1 = new Regex("\r\n");
        Regex regex2 = new Regex("\\s+");
        string[] sult = regex1.Split(strResult);
        DateTime theDt = DateTime.Parse(regex2.Split(sult[1])[1]);
        for (int i = 3; i < 8; i++)
        {
            string[] infoes = sult[i].Split(new char[2] { '（', '）' });
            string time;
            if (i == 3) time = theDt.ToString("MM-dd");
            else if (i < 7) time = theDt.AddDays(1).ToString("MM-dd");
            else time = theDt.AddDays(2).ToString("MM-dd");
            string[] dd = regex2.Split(infoes[2]);
            strReturn.Add(infoes[0] + "&" + time + ":" + (int.Parse(dd[1].Split('-')[1]) - 10) + ":" + dd[3]);
        }
        return strReturn;
    }
    public static string GetFtp48AQITXT()
        {
            string txt = "";
            DirectoryInfo info = new DirectoryInfo(@"F:\EMFCDataBase\FtpTemp\AQIPeriod");
            if (info != null)
            {
                string filter = DateTime.Now.ToString("yyyyMMdd1700");
                txt=getTXT(info, filter);
                if (string.IsNullOrEmpty(txt))
                {
                    filter = DateTime.Now.AddDays(-1).ToString("yyyyMMdd1700");
                    txt = getTXT(info, filter);
                }
            }
            return txt;
        }

    public static string getTXT(DirectoryInfo INFO,string filter) {
            string txt = "";
            foreach (FileInfo infos in INFO.GetFiles())
            {
                if (infos.FullName.IndexOf("Text_AQI_SH_" + filter + ".txt") >= 0)
                {
                    try
                    {
                        FileStream fs = infos.OpenRead();
                        StreamReader sr = new StreamReader(fs);
                        txt = sr.ReadToEnd();
                        sr.Close();
                        break;
                    }
                    catch { }
                }
            }
            return txt;
        }
    
}