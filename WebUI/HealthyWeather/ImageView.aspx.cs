using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using Readearth.Data;
using System.IO;

public partial class ImageView : System.Web.UI.Page
{
    public static Database m_Database;
    public static string m_station;
    public static string m_userName;
    public static string m_alias;
    protected void Page_Load(object sender, EventArgs e)
    {
        m_Database = new Database("DBCONFIGII");
        //if (Request.Cookies["User"] != null)
        //{
        m_userName = "readearth"; //Request.Cookies["User"]["name"].ToString();
        string strSql = "SELECT POSTIONAREA,Alias FROM T_USER WHERE USERNAME='" + m_userName + "'";
        DataTable dt = m_Database.GetDataTable(strSql);
        if (dt.Rows.Count > 0)
        {
            m_station = dt.Rows[0][0].ToString();
            m_alias = dt.Rows[0][1].ToString();
        }
        //}
    }

    [WebMethod]
    public static string GetContents(string type)
    {
        string html = "<table cellspacing='1' cellpadding='5' border='0' bgcolor='#e9e9e9'>", title = "";
        DateTime dtNow = DateTime.Now.Date;
        string peirod = "17";
        string strSql = "SELECT CONVERT(VARCHAR(10),LST,121),PERIOD,TYPE,[LEVEL] ,People,premunition FROM T_HEALTHYWEATHER WHERE DATEDIFF(DAY,GETDATE(),ForecastDate) = 0 and Station='" + m_station + "' AND PERIOD='" + peirod + "' AND FORECASTER='"+m_alias+"' ORDER BY TYPE,LST,PERIOD DESC";
        DataTable dt = m_Database.GetDataTable(strSql);
        if (dt.Rows.Count == 0)
        {
            peirod = "10";
            dt = m_Database.GetDataTable("SELECT CONVERT(VARCHAR(10),LST,121),PERIOD,TYPE,[LEVEL] ,People,premunition FROM T_HEALTHYWEATHER WHERE DATEDIFF(DAY,GETDATE(),ForecastDate) = 0 and Station='" + m_station + "' AND PERIOD='" + peirod + "' AND FORECASTER='" + m_alias + "' ORDER BY TYPE,LST,PERIOD DESC");
        }
        if (type == "email") {
            html += "<tr bgcolor='#1458d7' style='color:#fff'><td align='center'><strong>疾病</strong></td><td><strong>邮件内容</strong></td><td width=\"70px\" align='center'><strong>发送人数</strong></td></tr>";
            string oldType = "", oldTime = "";
            List<string[]> infoes = new List<string[]>();
            foreach (DataRow dr2 in dt.Rows)
            {
                if (oldType != dr2[2].ToString())
                {
                    if (oldType != "")
                    {
                        title = "";//SendHelper.GetTitle(dtNow.ToString("MM月dd日"), m_station, oldType);
                        html += "<tr bgcolor='#fafafa' style='color:#000'><td align='center' valign='center'>" + oldType + "</td>" + "<td>" + SendHelper.GetHtml(title, infoes) + "</td>";
                        html += "<td align='center' valign='center'>" + CountPeople(m_station, oldType, type, infoes[0][2]) + "</td></tr>";
                        infoes = new List<string[]>();
                    }
                    oldType = dr2[2].ToString();
                    oldTime = dr2[0].ToString();
                    string[] a = { oldType, oldTime, dr2[3].ToString(), dr2[4].ToString(), dr2[5].ToString() };
                    infoes.Add(a);
                }
                else
                {
                    if (oldTime != dr2[0].ToString())
                    {
                        oldTime = dr2[0].ToString();
                        string[] a = { oldType, oldTime, dr2[3].ToString(), dr2[4].ToString(), dr2[5].ToString() };
                        infoes.Add(a);
                    }
                }
            }
            if (infoes.Count > 0)
            {
                title ="";// SendHelper.GetTitle(dtNow.ToString("MM月dd日"), m_station, oldType);
                html += "<tr bgcolor='#fafafa' style='color:#000'><td align='center' valign='center'>" + oldType + "</td>" + "<td>" + SendHelper.GetHtml(title, infoes) + "</td>";
                html += "<td align='center' valign='center'>" + CountPeople(m_station, oldType, type, infoes[0][2]) + "</td></tr>";
            }
            if (dt.Rows.Count == 0) html = "<p>数据库内没有相关数据。</p>";
        }
        else if (type == "message") {
            html += "<tr bgcolor='#1458d7' style='color:#fff'><td><strong>疾病</strong></td><td><strong>预报日期</strong></td><td><strong>短信</strong></td><td><strong>字数</strong></td><td><strong>人数</strong></td></tr>";
            foreach (DataRow dr2 in dt.Rows)
            {
                DateTime theDt= DateTime.Parse(dr2[0].ToString());
                string healType=dr2[2].ToString();
                string content ="";// SendHelper.GetMessage(m_station, theDt, healType, dr2[3].ToString(), dr2[5].ToString());
                string sendTime = "0" + ((peirod == "10" ? 1 : 2) + theDt.CompareTo(dtNow)).ToString();
                html += "<tr bgcolor='#fafafa' style='color:#000'><td>" + healType + "</td><td>" + theDt.ToString("yyyy年MM月dd日") + "</td><td>" + content + "</td><td>" + content.Length + "</td><td>" + CountPeople(m_station, healType, sendTime, type, dr2[3].ToString()) + "</td></tr>";
            }
            if (dt.Rows.Count == 0) html = "<p>数据库内没有相关数据。</p>";
            else html += "</table>";
        }
        else if (type == "ftp")
        {	
            html += "<tr bgcolor='#1458d7' style='color:#fff'><td><strong>文件</strong></td><td><strong>内容</strong></td><td><strong>接收</strong></td></tr>";
            //foreach (DataRow dr2 in dt.Rows)
            //{
            //    DateTime theDt = DateTime.Parse(dr2[0].ToString());
            //    string healType = dr2[2].ToString();
            //    string content = "";//SendHelper.GetMessage(m_station, theDt, healType, dr2[3].ToString(), dr2[5].ToString());
            //    html += "<tr bgcolor='#fafafa' style='color:#000'><td>" + healType + "</td><td>" + theDt.ToString("yyyy年MM月dd日") + "</td><td>" + content + "</td><td>" + content.Length + "</td><td>" + CountPeople(m_station, healType, type, dr2[3].ToString()) + "</td></tr>";
            //}
            if (dt.Rows.Count == 0) html = "<p>数据库内没有相关数据。</p>";
            else html += "</table>";
        }
        return html;
    }

    [WebMethod]
    public static string GetLastTime()
    {
        string strSql = "SELECT TOP(1) CONVERT(VARCHAR(19),SendDate,121),R.ALIAS FROM T_SENDLOG T INNER JOIN T_USER R ON T.SENDUSER=R.UserName WHERE IsAll=1 ORDER BY SendDate DESC";
        DataTable dt = m_Database.GetDataTable(strSql);
        if (dt.Rows.Count > 0)
            return "{time:'" + dt.Rows[0][0].ToString() + "',user:'" + dt.Rows[0][1].ToString() + "'}";
        else return "";
    }
    
    [WebMethod]
    public static string GetEmailReceiver()
    {
        string strSql = "SELECT DISTINCT STUFF(cast((select DISTINCT ','+  convert(varchar(2),USERID)  from V_USERINFO t2 WHERE t2.REGION='" 
            + m_station + "' for xml path('')) as varchar(100)),1,1,'') FROM V_USERINFO";
        return m_Database.GetFirstValue(strSql).Replace(",","&"); ;
    }

    [WebMethod]
    public static void SendMessage()
    {
        string direct = System.Web.HttpContext.Current.Server.MapPath("")+ "//tempMessage";
        if (!Directory.Exists(direct)) Directory.CreateDirectory(direct);
        string fileName = "SMS_3_YWK"+DateTime.Now.ToString("yyyyMMddhhmmss")+".txt";
        string strSql = "SELECT CONVERT(VARCHAR(10),LST,121),PERIOD,TYPE,[LEVEL],People,premunition FROM T_HEALTHYWEATHER WHERE DATEDIFF(DAY,GETDATE(),ForecastDate) = 0 and Station='" + m_station + "' AND PERIOD='17' AND FORECASTER='" + m_alias + "' ORDER BY TYPE,LST,PERIOD DESC";
        DataTable dt = m_Database.GetDataTable(strSql);
        if (dt.Rows.Count == 0) dt = m_Database.GetDataTable(strSql.Replace("PERIOD='17'", "PERIOD='10'"));
        if (dt.Rows.Count > 0)
        {
            string healthyType, pubTime, lvl, message, txtContent = ""; DateTime theDt;
            foreach (DataRow dr in dt.Rows)
            {
                theDt = DateTime.Parse(dr[0].ToString());
                pubTime = "0" + ((dr[1].ToString() == "10" ? 1 : 2) + theDt.CompareTo(DateTime.Now.Date)).ToString();
                healthyType = dr[2].ToString();
                lvl = dr[3].ToString();
                message = "";//SendHelper.GetMessage(m_station, theDt, healthyType, lvl, dr[5].ToString());
                txtContent += GetMessageContent(m_station, healthyType, pubTime, lvl, message);
            }
            File.Create(direct + fileName).Close();
            StreamWriter sw = new StreamWriter(direct + fileName);
            sw.Write(txtContent);
            sw.Close();

            OpenFTP.FTP f = new OpenFTP.FTP();
            f.Connect("172.21.107.24", "SmsRequest", "aa9dsMTr");
            f.Files.Upload(fileName, direct + fileName);
            while (!f.Files.UploadComplete)
            {
                Console.WriteLine("Uploading: TotalBytes: " + f.Files.TotalBytes.ToString() + ", : PercentComplete: " + f.Files.PercentComplete.ToString());
            }
            f.Disconnect();
            f = null;
            //FtpHelper.Upload("SmsRequest", "aa9dsMTr", direct + fileName, "ftp://172.21.107.24/");
        }
    }

    private static string GetMessageContent(string station, string type, string sendTime, string level,string message)
    {
        string content = "", strSql = "SELECT DISTINCT PHONE FROM V_USERINFO WHERE REGION='" + station + "' AND CHEALTHYTYPE='" + type + "'AND MESSAGE_PubLvl!='' AND MESSAGE_PubLvl <="
            + "(SELECT DM FROM D_PUBLISHLVL WHERE CODE='" + level + "' OR CODE2='" + level + "') AND MESSAGE_PUBTIME LIKE '%"+sendTime+"%'";
        DataTable dt = m_Database.GetDataTable(strSql);
        foreach (DataRow dr in dt.Rows) {
            content += dr[0].ToString() + "\t" + message+"\r\n";
        }
        return content;
    }

    private static string CountPeople(string station,string type,string sendType,string level) 
    {
        sendType = (sendType == "email" ? "Email_PubLvl" : "Message_PubLvl");
        string strSql = "SELECT COUNT(DISTINCT NAME) FROM V_USERINFO WHERE REGION='" + station + "' AND CHEALTHYTYPE='" + type + "' AND " + sendType + "!='' AND " + sendType
            + " <=(SELECT DM FROM D_PUBLISHLVL WHERE CODE='" + level + "' OR CODE2='" + level + "')";
        return m_Database.GetFirstValue(strSql);
    }

    private static string CountPeople(string station, string type,string sendTime, string sendType, string level)
    {
        sendType = (sendType == "email" ? "Email_PubLvl" : "Message_PubLvl");
        string strSql = "SELECT COUNT(DISTINCT NAME) FROM V_USERINFO WHERE REGION='" + station + "' AND CHEALTHYTYPE='" + type + "' AND " + sendType + "!='' AND " + sendType
            + " <=(SELECT DM FROM D_PUBLISHLVL WHERE CODE='" + level + "' OR CODE2='" + level + "') AND EMAIL_PUBTIME LIKE '%" + sendTime + "%'";
        return m_Database.GetFirstValue(strSql);
    }

}