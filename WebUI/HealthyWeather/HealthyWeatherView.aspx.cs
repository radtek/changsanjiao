using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Readearth.Data;
using System.Data;
using System.IO;
using System.Collections;
using System.Text;
using System.Net;

public partial class HealthyWeather_HealthyWeatherView : System.Web.UI.Page//2017.8.1 孙明宇
{

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    /// <summary>
    /// 返回HTML表格 
    /// </summary>
    /// <param name="selectSite"></param>
    /// <returns></returns>
    [WebMethod]
    public static string GetContents(string selectSite)
    {
        string oldType = "", oldTime = "";
        List<string[]> infoes = new List<string[]>();
        string html = "<table cellspacing='0' cellpadding='5' border='0' bgcolor='#e9e9e9' width='100%'>", title = "";
        DateTime dtNow = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
        string forecastTime="";
        WebReference.Publish web = new WebReference.Publish();
        DataSet ds = web.GetCrows("shjkqxyb");
        DataTable dts = ds.Tables[0];
        string houtian = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd 0:00:00");
        bool flag = dts.Select("Date='" + houtian + "'").Count() > 0;//判断预报时间
        if(flag==true)
        {
            forecastTime="17";
        }
        else
        {
            forecastTime="10";//2017-8-3 孙明宇
        }
        dts.DefaultView.Sort = "ID";
        string siteName = "中心城区";
        DataTable dt_new = dts.Clone();
        string[] items = { "儿童感冒", "青少年和成年人感冒", "老年人感冒", "儿童哮喘","COPD患者", "中暑", "重污染" };
        foreach (string str in items)
        {
            DataRow[] rows = dts.Select("Crow= '" + str + "'");
            if (rows != null && rows.Length > 0)
            {
                for (int i = 0; i < rows.Length; i++)
                {
                    dt_new.Rows.Add(rows[i].ItemArray);
                }
            }
        }
        
        foreach (DataRow dr in dt_new.Rows)
        {
            if (oldType != dr[1].ToString())
            {
                if (oldType != "")
                {
                    DateTime theDt = DateTime.Parse(infoes[0][1].ToString());
                    theDt = DateTime.Parse(infoes[1][1].ToString());
                    title = SendHelperHW.GetTitle(dtNow.ToString("MM月dd日"), oldType, siteName,forecastTime);
                    if (infoes[0][0].IndexOf("儿童感冒") >= 0 || infoes[0][0].IndexOf("COPD") >= 0 || infoes[0][0].IndexOf("哮喘") >= 0)
                    {
                        html+="<tr style='color:#000'><td>";
                    }
                    html += SendHelperHW.GetHtml(title, infoes);
                    if (infoes[0][0].IndexOf("老年人感冒") >= 0 || infoes[0][0].IndexOf("COPD") >= 0 || infoes[0][0].IndexOf("哮喘") >= 0)
                    {
                        html += "</td></tr>";
                    }
                    infoes = new List<string[]>();
                }
                oldType = dr[1].ToString();
                oldTime = Convert.ToDateTime(dr[2]).ToString("yyyy-MM-dd");//2017-8-3 孙明宇
                string[] a = { oldType, oldTime, dr[4].ToString(), dr[5].ToString(), dr[6].ToString() };
                infoes.Add(a);
            }
            else
            {
                if (oldTime != dr[2].ToString())
                {
                    oldTime = Convert.ToDateTime(dr[2]).ToString("yyyy-MM-dd");//2017-8-3 孙明宇
                    string[] a = { oldType, oldTime, dr[4].ToString(), dr[5].ToString(), dr[6].ToString() };
                    infoes.Add(a);
                }
            }
        }
        if (infoes.Count > 0)
        {
            DateTime theDt = DateTime.Parse(infoes[0][1].ToString());
            theDt = DateTime.Parse(infoes[1][1].ToString());
            title = SendHelperHW.GetTitle(dtNow.ToString("MM月dd日"), oldType, siteName,forecastTime);
            html += "<tr  style='color:#000'><td>" + SendHelperHW.GetHtml(title, infoes) + "</td></tr>";
            if (dt_new.Rows.Count == 0) html = "<p>数据库内没有相关数据。</p>";
        }
        html += "<tr  style='color:#000'><td>" + getFTPTxt() + "</td></tr><tr><td align='right' style='font-family:微软雅黑'><strong>上海市气象与健康重点实验室发布</strong> </td></tr></table>";//2017-8-3 孙明宇
        return html;
    }

    /// <summary>
    /// 读取FTP文件
    /// </summary>
    /// <param name="ftp"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static string[] readerFtpFile(string ftp, string username, string password)
    {
        StringBuilder result = new StringBuilder();
        FtpWebRequest reqFTP;
        string filename = "";
        string ftpserver = "ftp://" + username + ":" + password + "@" + ftp+"/";
        reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpserver));
        reqFTP.UsePassive = true;
        reqFTP.UseBinary = true;
        reqFTP.Credentials = new NetworkCredential(username, password);
        reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
        WebResponse responseNew = reqFTP.GetResponse();
        StreamReader readerNew = new StreamReader(responseNew.GetResponseStream());
        string line = readerNew.ReadLine();
        DateTime newest = new DateTime();
        while (line != null)
        {
            try
            {
                line=line.Substring(6, 10);
                line = line.Insert(4, "-").Insert(7, "-").Insert(10, " ").Insert(line.Length+3,":00:00");
                DateTime dt = Convert.ToDateTime(line);
                
                 if (dt > newest)
                {
                    newest = dt;
                }
            }
            catch
            {
                line = readerNew.ReadLine();
                continue; 
            }
            line = readerNew.ReadLine();
        }
        
        string day = "";
        if (Convert.ToInt32(newest.ToString("HH")) == 07)
        {
            filename = "index_" + newest.ToString("yyyyMMddHH") + "24.txt";
            day = newest.ToString("dd日");
        }
        else if (Convert.ToInt32(newest.ToString("HH")) == 11)
        {
            filename = "index_" + newest.ToString("yyyyMMddHH") + "48.txt";
            day = newest.AddDays(1).ToString("dd日");
        }
        else 
        {
            filename = "index_" + newest.ToString("yyyyMMddHH") + "24.txt";
            if (Convert.ToInt32(DateTime.Now.ToString("HH")) < 7)
            {
                day = newest.ToString("dd日"); ;
            }
            else { day = newest.AddDays(1).ToString("dd日"); }
        }
        string time = newest.ToString("MM月dd日HH时");
        responseNew.Close();



        string ftpserverDLD = "ftp://" + username + ":" + password + "@" + ftp + "/" + filename;
        FtpWebRequest FTPDLD = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpserverDLD));
        FTPDLD.UsePassive = true;
        FTPDLD.UseBinary = true;
        FTPDLD.Credentials = new NetworkCredential(username, password);
        FTPDLD.Method = WebRequestMethods.Ftp.DownloadFile;
        FtpWebResponse response = (FtpWebResponse)FTPDLD.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312"));
        //string line = reader.ReadLine();
        string[] lines = new string[20];
        int i = 0;
        while (i < 10)
        {
            lines[i] = reader.ReadLine();
            i++;
        }
        lines[10] = time;
        lines[11] = day;
        reader.Close();
        response.Close();
        return lines;
    }

    /// <summary>
    /// 返回中暑表格
    /// </summary>
    /// <returns></returns>
    public static string getFTPTxt()
    {
        DateTime dateTime = DateTime.Now;
        string[] zhongshu = new string[10];
        int hour = Convert.ToInt32(dateTime.ToString("HH"));
        zhongshu=readerFtpFile("172.21.3.242", "zsyb", "zsyb1");

        string title = zhongshu[10] + "发布上海市" + zhongshu[11] + "中暑指数预报";//2017-8-3 孙明宇
        string html = "<div class='littletitle' style='margin-top:10px'><h4 class='headtext'>" + title + "</h4></div><div class='radius'><table class='uptable' style='background-color:#F0F5FB;text-align:center' cellspacing='1' cellpadding='5' border='0' bgcolor='#e9e9e9' width='100%'>";
        string[] str = new string[10];
        for (int j = 8; j <= 9; j++)
        {
            str = zhongshu[j].Split(' ');
            html += "<tr>";
            if (j == 8)
            {
                html += "<td class='zstd'>上午</td>";
            }
            else
            {
                html += "<td class='zstd'>下午</td>";
 
            }
            for (int i = 5; i < 9; i++)
            {
                if (i < 8 && i!=7)
                {
                    html += "<td class='zstd'>" + str[i] + "</td>";
                }
                else if(i==8)
                    html += "<td class='zsinfo'>" + str[i] + "</td>";

            }
            html += "</tr>";
        }
        html += "</table></div>";
       
        return html;
    }

}