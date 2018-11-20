using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Readearth.Data;

public partial class LiveIndex_log : System.Web.UI.Page
{
    public static Database m_Database;
    protected void Page_Load(object sender, EventArgs e)
    {
        m_Database = new Database("DBCONFIGII");
    }

    [WebMethod]
    public static DataTable GetPeople(){
        string sql = "select alias from t_user order by alias asc";
        DataTable dt = m_Database.GetDataTable(sql);
        return dt;
    }
    [WebMethod]
    public static string GetLogTxt(string sendDate,string option) {
        string txt ="";
        string ftpUploadTxt = ConfigurationManager.AppSettings["ftpUploadTxt"];
        if (option != "首席上传" && option != "短信上传")
        {
            ftpUploadTxt = ConfigurationManager.AppSettings["FtpUploadTxtForecaster"];
        }
        string localDirect = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ftpUploadTxt);
        string fileName = GetFileName(sendDate, option);
        string[] name = fileName.Split('#');
        for (int i = 0; i < name.Length; i++) {   //早上首席发布了两个文件
            string strName = name[i];
            if (option.IndexOf("短信") > -1)
            {
                DirectoryInfo dir = new DirectoryInfo(localDirect);
                FileInfo [] fileDir =dir.GetFiles("*"+name[i]+"*");
                strName = fileDir[0].Name;
                
            }
            string path = localDirect + "\\" + strName;
            using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding("GB2312")))
            {
                try
                {
                    txt += sr.ReadToEnd()+"$"+strName+"#";
                    sr.Close();
                }
                catch (IOException e)
                {
                    txt += "error" + e.Message+"#";
                }
            }
        }
        txt = txt.TrimEnd('#');
        return txt;
    }

    public static string GetFileName(string sendDate, string fileName)
    {
        DateTime Time = DateTime.Parse(sendDate);
        if (fileName.IndexOf('：') > -1)
        {     //预报员上传
            fileName = fileName.Split('：')[1];
            if (fileName.IndexOf("HX") > -1 && Time.Hour >= 12)    //火险指数的文件名称下午的要加1天，其他上传的不变
            {
                fileName = fileName.Replace("yyyymmdd", Time.AddDays(1).ToString("yyyyMMdd"));
            }
            else
            {
                fileName = fileName.Replace("yyyymmdd", Time.ToString("yyyyMMdd"));
            }
        }
        else if (fileName.IndexOf("短信") > -1)
        {    //短信上传
            fileName = "SMS-20012-" + Time.ToString("yyyyMMdd");   //短信有md5码
        }
        else if (fileName.IndexOf("首席") > -1)
        {
            string period = "";
            int hour = Time.Hour;
            fileName = "";
            if (hour < 10)
            {
                period = "07";
            }
            else if (hour < 15)
            {
                period = "11";
            }
            else if (hour >= 15)
            {
                period = "16";
            }
            if (period == "16")
            {
                fileName = "index_" + Time.ToString("yyyyMMdd") + period + "24.txt";
            }
            else
            {
                fileName = "index_" + Time.ToString("yyyyMMdd") + period + "24.txt" + "#" + "index_" + Time.ToString("yyyyMMdd") + period + "48.txt";
            }
        }
        return fileName;
        
    }
}