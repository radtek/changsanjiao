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
using System.Text;
using System.Collections;

public partial class HealthyWeather_PubView : System.Web.UI.Page
{
    public static Database m_Database;
    public static string m_station;
    public static string m_userName;
    public static string m_alias;
    public static string m_region;
    public static Hashtable hstable=new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        m_Database = new Database("DBCONFIGII");
        if (Request.Cookies["User"] != null)
        {
            //if (!IsPostBack)
           // {
                m_userName = Request.Cookies["User"]["name"].ToString();
                string strSql = "SELECT POSTIONAREA,Alias FROM T_USER WHERE USERNAME='" + m_userName + "'";
                DataTable dt = m_Database.GetDataTable(strSql);
                if (dt.Rows.Count > 0)
                {
                    m_station = dt.Rows[0][0].ToString();
                    m_alias = dt.Rows[0][1].ToString();
                    m_region = dt.Rows[0][1].ToString();
                }
              
          //  }
           hstable.Clear();
           string  strSqls = " select * from  T_ShanghaiArea";
           DataTable dts = m_Database.GetDataTable(strSqls);
            if (dts.Rows.Count > 0)
            {
                try
                {
                    foreach (DataRow row in dts.Rows)
                    {
                        hstable.Add(row["description"].ToString(),
                                    row["org"].ToString());
                    }
                }
                catch { }
            }
        }
        else
            Response.Redirect("../Default.aspx", true);
    }

    public static string GetTypeName(string healthyType)
    {

        string ht = "";
        switch (healthyType)
        {
            case "青年感冒": ht = "青少年和成年人感冒"; break;
            case "老年感冒": ht = "老年人感冒"; break;
            default: ht = healthyType; break;
        }
        return ht;
    }


    public static string GetTitles( string healthyType)
    {
        // return region + "气象台" + day + "发布" + healthyType + "气象风险预报（试行）";

        string ht = "青年感冒";
        switch (healthyType)
        {
            case "青年感冒": ht = "青少年和成年人感冒"; break;
            case "老年感冒": ht = "老年人感冒"; break;
            default: ht = healthyType; break;
           // case "COPD": ht = "COPD"; return region + "" + day + "发布" + ht + "气象环境风险预报（试行）";
            //case "儿童哮喘": ht = "儿童哮喘"; return region + "" + day + "发布" + ht + "气象环境风险预报（试行）";
        }

        return ht;
    }

    [WebMethod]
    public static string GetContents(string type,string selectSite)
    {
        string stations = "";
        if (m_station != "") {
            stations="(";
            foreach (string str in m_station.Split(',')) {
                stations += ("'" + str + "',"); 
            }
            stations=stations.TrimEnd(',');
            stations += ")";
        }


        string html = "<table cellspacing='1' cellpadding='5' border='0' bgcolor='#e9e9e9'>", title = "";
        DateTime dtNow = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
        string peirod = "17";
        string strSql = "SELECT CONVERT(VARCHAR(10),LST,121),PERIOD,TYPE,[LEVEL] ,People,premunition,Station FROM T_HEALTHYWEATHER WHERE DATEDIFF(DAY,GETDATE(),ForecastDate) = 0 and Station in " + stations + " AND PERIOD='" + peirod + "' AND FORECASTER='" + m_alias + "' ORDER BY TYPE,LST,PERIOD DESC";
        DataTable dts = m_Database.GetDataTable("SELECT CONVERT(VARCHAR(10),LST,121),PERIOD,TYPE,[LEVEL] ,People,premunition,Station FROM T_HEALTHYWEATHER WHERE  1<>1 ORDER BY TYPE,LST,PERIOD DESC");

        dts = m_Database.GetDataTable(strSql);
        if (dts != null && dts.Rows.Count > 0 && DateTime.Now.Hour > 12) // xuehui 0923
        {
            peirod = "17";
        }
        else
        {
            peirod = "10";
            try
            {
                dts = m_Database.GetDataTable("SELECT CONVERT(VARCHAR(10),LST,121),PERIOD,TYPE,[LEVEL] ,People,premunition,Station FROM T_HEALTHYWEATHER WHERE DATEDIFF(DAY,GETDATE(),ForecastDate) = 0 and Station in " + stations + " AND PERIOD='" + peirod + "' AND FORECASTER='" + m_alias + "' ORDER BY TYPE,LST,PERIOD DESC");
            }
            catch { }
        }


        //xuehui 2017-12-07 如果查不到就查本单位做的数据 
        if (dts == null || dts.Rows.Count <= 0)
        {
            #region
            //查询user的单位
            string sql_user = " select * from T_user where Alias='" + m_alias + "'";
            DataTable dt_user = m_Database.GetDataTable(sql_user);
            string companyName = "";
            if (dt_user != null && dt_user.Rows.Count > 0)
            {
                companyName = dt_user.Rows[0]["WindowsUser"].ToString();
                string sql_query = "  SELECT CONVERT(VARCHAR(10),t1.LST,121),t1.PERIOD,t1.TYPE,t1.[LEVEL] ,t1.People,t1.premunition,t1.Station FROM T_HEALTHYWEATHER " +
                                   "  t1 left join T_User t2  on t1.forecaster=t2.Alias " +
                                   "  WHERE DATEDIFF(DAY,GETDATE(),t1.ForecastDate) = 0  " +
                                   "  and t1.Station in " + stations + " AND t1.PERIOD='10' AND  " +
                                   "  t2.WindowsUser='" + companyName + "' ORDER BY t1.TYPE,t1.LST,t1.PERIOD DESC";
                dts = m_Database.GetDataTable(sql_query);
            }//如果查不到
            if (dts == null || dts.Rows.Count <= 0)
            {
                try
                {
                    companyName = dt_user.Rows[0]["WindowsUser"].ToString();
                    string sql_query = "  SELECT CONVERT(VARCHAR(10),t1.LST,121),t1.PERIOD,t1.TYPE,t1.[LEVEL] ,t1.People,t1.premunition,t1.Station FROM T_HEALTHYWEATHER " +
                                       "  t1 left join T_User t2  on t1.forecaster=t2.Alias " +
                                       "  WHERE DATEDIFF(DAY,GETDATE(),t1.ForecastDate) = 0  " +
                                       "  and t1.Station in " + stations + " AND t1.PERIOD='17' AND  " +
                                       "  t2.WindowsUser='" + companyName + "' ORDER BY t1.TYPE,t1.LST,t1.PERIOD DESC";
                    dts = m_Database.GetDataTable(sql_query);
                }
                catch { }
            }
            #endregion
        }

        foreach (string siteName in m_station.Split(','))
        {
            if (siteName != selectSite && selectSite!="全部")
                continue;

            //03-22薛辉加入排序
            DataTable dt_new = dts.Clone();
            string[] items = { "儿童感冒", "青年感冒", "老年感冒", "COPD", "儿童哮喘", "中暑", "重污染" };
            foreach (string str in items)
            {
                DataRow[] rows = dts.Select("TYPE= '" + str + "' and   Station='" + siteName + "'");
                if (rows != null && rows.Length > 0)
                {
                    for (int i = 0; i < rows.Length; i++)
                    {
                        dt_new.Rows.Add(rows[i].ItemArray);
                    }
                }
            }

            if (type == "email")
            {
                html += "<tr bgcolor='#1458d7' style='color:#fff'><td align='center' style='width:8%; display:none; '><strong>疾病</strong></td><td><strong>邮件内容</strong><strong style='margin-left:45%; '>" + siteName + "</strong></td><td width=\"70px\" align='center'><strong>发送人数</strong></td></tr>";
                string oldType = "", oldTime = "";
                List<string[]> infoes = new List<string[]>();
                foreach (DataRow dr2 in dt_new.Rows)
                {
                    if (oldType != dr2[2].ToString())
                    {
                        if (oldType != "")
                        {
                            DateTime theDt = DateTime.Parse(infoes[0][1].ToString());
                            string sendTime = "0" + ((peirod == "10" ? 1 : 2) + (theDt - dtNow).TotalDays).ToString();// 03-24 这里要修改下
                            theDt=DateTime.Parse(infoes[1][1].ToString());
                            string sendTime2 = "0" + ((peirod == "10" ? 1 : 2) + (theDt - dtNow).TotalDays).ToString();// 03-24 这里要修改下
                            string count = CountPeopleII(siteName, oldType, sendTime, type, infoes[0][2], infoes[1][2], sendTime2);


                            title = SendHelper.GetTitle(dtNow.ToString("MM月dd日"), hstable[siteName].ToString(), oldType,siteName);

                            html += "<tr bgcolor='#fafafa' style='color:#000'><td align='center' valign='center'style='display:none' >" + GetTitles(oldType) + "</td>" + "<td>" + SendHelper.GetHtml(title, infoes) + "</td>";
                            html += "<td align='center' valign='center'>" + count + "</td></tr>";
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
                    DateTime theDt = DateTime.Parse(infoes[0][1].ToString());
                    string sendTime = "0" + ((peirod == "10" ? 1 : 2) + (theDt - dtNow).TotalDays).ToString();// 03-24 这里要修改下
             
                    theDt = DateTime.Parse(infoes[1][1].ToString());
                    string sendTime2 = "0" + ((peirod == "10" ? 1 : 2) + (theDt - dtNow).TotalDays).ToString();// 03-24 这里要修改下
                    string count = CountPeopleII(siteName, oldType, sendTime, type, infoes[0][2], infoes[1][2], sendTime2);

                    title = SendHelper.GetTitle(dtNow.ToString("MM月dd日"), hstable[siteName].ToString(), oldType,siteName);
                    html += "<tr bgcolor='#fafafa' style='color:#000'><td align='center' valign='center' style='display:none'>" + oldType + "xxx</td>" + "<td>" + SendHelper.GetHtml(title, infoes) + "</td>";
                    html += "<td align='center' valign='center'>" + count + "</td></tr>";
                }
                if (dt_new.Rows.Count == 0) html = "<p>数据库内没有相关数据。</p>";

            }
            else if (type == "message")
            {
                html += "<tr bgcolor='#1458d7' style='color:#fff'><td style='width:13%'><strong>疾病</strong></td><td style='width:12%'><strong>预报日期</strong></td><td style='width:65%'><strong>短信</strong><strong style='margin-left:45%; '>" + siteName + "</strong></td><td style='width:5%'><strong >字数</strong></td><td style='width:5%'> <strong>人数</strong></td></tr>";
                foreach (DataRow dr2 in dt_new.Rows)
                {
                    DateTime theDt = DateTime.Parse(dr2[0].ToString());
                    string healType = dr2[2].ToString();
                    string content = SendHelper.GetMessage(hstable[siteName].ToString(), theDt, healType, dr2[3].ToString(), dr2[5].ToString(),siteName);

                    string sendTime = "0" + ((peirod == "10" ? 1 : 2) + (theDt - dtNow).TotalDays).ToString();// 03-24 这里要修改下

                    html += "<tr bgcolor='#fafafa' style='color:#000'><td>" + GetTypeName(healType) + "</td><td>" + theDt.ToString("yyyy年MM月dd日") + "</td><td>" + content + "</td><td>" + content.Length + "</td><td>" + CountPeople(siteName, healType, sendTime, type, dr2[3].ToString()) + "</td></tr>";
                }
                if (dt_new.Rows.Count == 0) html = "<p>数据库内没有相关数据。</p>";
                //else html += "</table>";
            }
            else if (type == "ftp")
            {
               // html += "<tr bgcolor='#1458d7' style='color:#fff'><td><strong>文件</strong></td><td><strong>内容</strong></td><td><strong>接收</strong></td></tr>";
               // foreach (DataRow dr2 in dt_new.Rows)
               // {
               //     DateTime theDt = DateTime.Parse(dr2[0].ToString());
               //     string healType = dr2[2].ToString();
               //     string content = SendHelper.GetMessage(m_station, theDt, healType, dr2[3].ToString(), dr2[5].ToString());
               //     html += "<tr bgcolor='#fafafa' style='color:#000'><td>" + healType + "</td><td>" + theDt.ToString("yyyy年MM月dd日") + "</td><td>" + content + "</td><td>" + content.Length + "</td><td>" + 0+ "</td></tr>";
               // }
               // if (dt_new.Rows.Count == 0) html = "<p>数据库内没有相关数据。</p>";
               //// else html += "</table>";

                html += "<tr bgcolor='#1458d7' style='color:#fff'><td style='width:13%; text-align:center'><strong>疾病</strong></td><td style='width:68%; word-break : break-all;'><strong>内容</strong><strong style='margin-left:45%; '>" + siteName + "</strong></td><td style='width:4%'><strong >字数</strong></td></tr>";
                foreach (DataRow dr2 in dt_new.Rows)
                {
                    DateTime theDt = DateTime.Parse(dr2[0].ToString());
                    string healType = dr2[2].ToString();
                    string content = SendHelper.GetMessageFTP(hstable[siteName].ToString(), theDt, healType, dr2[3].ToString(), dr2[5].ToString(), dr2["People"].ToString());

                    string sendTime = "0" + ((peirod == "10" ? 1 : 2) + (theDt - dtNow).TotalDays).ToString();// 03-24 这里要修改下
                    int length = content.Length;
                    if (healType == "中暑")
                    {
                        length = length - 15-1;
                    }
                    else {
                        length = length - 20-2;
                    }
                    html += "<tr bgcolor='#fafafa' style='color:#000'><td style='text-align:center'>" + GetTypeName(healType) + "</td><td style=' word-break : break-all;'>" + content + "</td><td>" + length + "</td></tr>";
                }
                if (dt_new.Rows.Count == 0) html = "<p>数据库内没有相关数据。</p>";
            }
        }
        return html;
    }

    [WebMethod]
    public static string GetLastTime()
    {

        //查询user的单位
        string sql_user = " select * from T_user where Alias='" + m_alias + "'";
        DataTable dt_user = m_Database.GetDataTable(sql_user);
        string companyName = "";
        if (dt_user != null && dt_user.Rows.Count > 0)
            companyName = dt_user.Rows[0]["WindowsUser"].ToString();
        
        //string strSql = "SELECT TOP(1) CONVERT(VARCHAR(19),SendDate,121),R.ALIAS FROM T_SENDLOG T INNER JOIN T_USER R ON T.SENDUSER=R.UserName WHERE IsAll=1  and SendUser='" + m_alias + "' ORDER BY SendDate DESC";
        string strSql = " select * from T_HealthyAuto where forecaster='" + m_alias + "' order by createTime desc ";// 04-01
        if (companyName != "") {
            strSql = " select t1.* from T_HealthyAuto t1 left join T_User t2  on t1.forecaster=t2.Alias where t2.WindowsUser='" + companyName + "' order by t1.createTime desc ";//  xuehui 2017-12-09
        }

        DataTable dt = m_Database.GetDataTable(strSql);
        if (dt.Rows.Count > 0)
            return "{time:'" + DateTime.Parse(dt.Rows[0]["createTime"].ToString()).ToString("yyyy-MM-dd HH:mm") + "',user:'" + dt.Rows[0]["forecaster"].ToString() + "'}";
        else return "";
    }


    [WebMethod]
    public static string GetLastWSTime()
    {
        //查询user的单位
        string sql_user = " select * from T_user where Alias='" + m_alias + "'";
        DataTable dt_user = m_Database.GetDataTable(sql_user);
        string companyName = "";
        if (dt_user != null && dt_user.Rows.Count > 0)
            companyName = dt_user.Rows[0]["WindowsUser"].ToString();

        //string strSql = "SELECT TOP(1) CONVERT(VARCHAR(19),SendDate,121),R.ALIAS FROM T_SENDLOG T INNER JOIN T_USER R ON T.SENDUSER=R.UserName WHERE IsAll=1  and SendUser='" + m_alias + "' ORDER BY SendDate DESC";
        string strSql = " select * from [T_HealthyAutoSingle] where forecaster='" + m_alias + "' order by createTime desc ";// 04-01

        if (companyName != "")
        {
            strSql = " select t1.* from T_HealthyAutoSingle t1 left join T_User t2  on t1.forecaster=t2.Alias where t2.WindowsUser='" + companyName + "' order by t1.createTime desc ";//  xuehui 2017-12-09
        }

        DataTable dt = m_Database.GetDataTable(strSql);
        if (dt.Rows.Count > 0)
            return "{time:'" + DateTime.Parse(dt.Rows[0]["createTime"].ToString()).ToString("yyyy-MM-dd HH:mm") + "',user:'" + dt.Rows[0]["forecaster"].ToString() + "'}";
        else return "";
    }
    
    [WebMethod]
    public static string GetEmailReceiver()
    {
        string sites=" 1=1 "; // 03-27
        string[] sit=m_station.Split(',');
        if (sit != null && sit.Length > 0) 
            sites = "";
        
        foreach (string str in sit) {
            sites += (" t2.REGION like '%" + str + "%' @ ");
        }
        sites = sites.TrimEnd(" @ ".ToCharArray());
        sites = sites.Replace(" @ ", " or ");

        string strSql = "SELECT DISTINCT STUFF(cast((select DISTINCT ','+  convert(varchar(2),USERID)  from V_USERINFO t2 WHERE "
            + sites + " and CanEmail=1 for xml path('')) as varchar(100)),1,1,'') FROM V_USERINFO ";
        return m_Database.GetFirstValue(strSql).Replace(",","&"); 
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
                message = SendHelper.GetMessage(m_station, theDt, healthyType, lvl, dr[5].ToString(), m_station);
                txtContent += GetMessageContent(m_station, healthyType, pubTime, lvl, message);
            }
            File.Create(direct + fileName).Close();
            StreamWriter sw = new StreamWriter(direct + fileName,false, Encoding.GetEncoding("GB2312"));
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

    [WebMethod]
    public static string SendAll() {

        string mes = "";
        // 查看下有没有数据需要发送
       string sql1=" select count(*) from T_HEALTHYWEATHER where DATEDIFF(DAY,GETDATE(),forecastDate)=0 "+
                   "and  period='17' and forecaster='" + m_alias + "'";
       string periods = "10";
       DataTable dt=m_Database.GetDataTable(sql1);
       if (dt != null && dt.Rows.Count > 0) {
           if(int.Parse(dt.Rows[0][0].ToString())>0 && DateTime.Now.Hour>12) // xuehui 0923
               periods = "17";
       }
       sql1 = " select * from T_HEALTHYWEATHER where DATEDIFF(DAY,GETDATE(),forecastDate)=0 " +
                   "and  period='" + periods + "' and forecaster='" + m_alias + "'";
       dt = m_Database.GetDataTable(sql1);
       if (dt == null || dt.Rows.Count <= 0)
       {
           mes = "没有需要发送的数据！";
           return mes;
       }
       //插入到发送表里面去
       sql1 = " select * from T_HealthyAuto where DATEDIFF(DAY,GETDATE(),createTime)=0 and forecaster='" +
           m_alias + "' and  period='" + periods + "' and status='0' ";
       dt = m_Database.GetDataTable(sql1);
       if (dt != null && dt.Rows.Count > 0)
       {
           string status = dt.Rows[0]["status"].ToString();
           if (status == "0")
           {
               mes = "已经在处理中，请稍后重试！";
               return mes;
           }
       
       }

       //插入到发送表
       try
       {
           sql1 = "insert into T_HealthyAuto(forecaster,period,createTime,type,status,isMessage,isFtp,isEmail,Region)  " +
                  "values('" + m_alias + "','" + periods + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','一键发送','0','0','0','0','" + m_station.Replace(",", "_") + "') ";
           m_Database.Execute(sql1);
           mes = "成功发送到后台处理，详细发送记录请查看发送日志！";
       }
       catch
       {
           mes = "发送数据库失败，请稍后重试！";
       }

       return mes;
       
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

    //0324薛辉注释这个方法
    //private static string CountPeople(string station,string type,string sendType,string level) 
    //{
    //    sendType = (sendType == "email" ? "Email_PubLvl" : "Message_PubLvl");
    //    string strSql = "SELECT COUNT(DISTINCT NAME) FROM V_USERINFO WHERE REGION='" + station + "' AND CHEALTHYTYPE='" + type + "' AND " + sendType + "!='' AND " + sendType
    //        + " <=(SELECT DM FROM D_PUBLISHLVL WHERE CODE='" + level + "' OR CODE2='" + level + "')";

    //    return m_Database.GetFirstValue(strSql);
    //}

    private static string CountPeople(string station, string type,string sendTime, string sendType, string level)
    {
        string canType = " CanMessage=1";
        if (sendType == "email")
        {
            sendType = "Email_PubLvl";
            canType = " CanEmail=1"; // xuehui 修改于 2017-04-13
        }

        sendType = (sendType == "Email_PubLvl" ? "Email_PubLvl" : "Message_PubLvl");
        string pubTime = (sendType == "Email_PubLvl" ? "EMAIL_PUBTIME" : "Message_PUBTIME");
        string strSql = "SELECT COUNT(DISTINCT NAME) FROM V_USERINFO WHERE REGION like '%" + station + "%' AND CHEALTHYTYPE='" + type + "' AND " + sendType + "!='' AND " + sendType
            + " <=(SELECT DM FROM D_PUBLISHLVL WHERE CODE='" + level + "' OR CODE2='" + level + "') AND " + pubTime + " LIKE '%" + sendTime + "%' and " + canType + "";
        return m_Database.GetFirstValue(strSql);

    }


    private static string CountPeopleII(string station, string type, string sendTime1, string sendType, string level1 ,string level2,string sendTime2)
    {
        string canType = " CanMessage=1";
        if (sendType == "email")
        {
            sendType = "Email_PubLvl";
            canType = " CanEmail=1"; // xuehui 修改于 2017-04-13
        }

        sendType = (sendType == "Email_PubLvl" ? "Email_PubLvl" : "Message_PubLvl");

        string pubTime = (sendType == "Email_PubLvl" ? "EMAIL_PUBTIME" : "Message_PUBTIME");



        string strSql = "SELECT COUNT(DISTINCT NAME) FROM V_USERINFO WHERE REGION like '%" + station + "%' AND CHEALTHYTYPE='" + type + "' AND " + sendType + "!='' AND (" + sendType
            + " <=(SELECT DM FROM D_PUBLISHLVL WHERE CODE='" + level1 + "' OR CODE2='" + level1 + "') or " + sendType
            + " <=(SELECT DM FROM D_PUBLISHLVL WHERE CODE='" + level2 + "' OR CODE2='" + level2 + "')) AND (" + pubTime + " LIKE '%" + sendTime1 + "%' or  " + pubTime + " LIKE '%" + sendTime2 + "%' )and " + canType + "";
        return m_Database.GetFirstValue(strSql);

    }

    [WebMethod]
    public static string UpdateSend(){
        string mes = "";
        string sql = " select count(*) from T_HEALTHYWEATHER where DATEDIFF(DAY,GETDATE(),forecastDate)=0 and  period='17' and forecaster='" + m_alias + "'";
        string periods = "10";
        DataTable dt= m_Database.GetDataTable(sql);
        if (dt != null && dt.Rows.Count > 0) {
            if (int.Parse(dt.Rows[0][0].ToString()) > 0 && DateTime.Now.Hour>12) // xuehui 0923 
            periods = "17";
        }
        sql = " select * from T_HEALTHYWEATHER where DATEDIFF(DAY,GETDATE(),forecastDate)=0 " +
                   "and  period='" + periods + "' and forecaster='" + m_alias + "'";
        dt = m_Database.GetDataTable(sql);
        if (dt == null || dt.Rows.Count <= 0)
        {
            mes = "没有需要更新的数据！";
            return mes;
        }
        sql = " select * from [T_HealthyAutoSingle] where DATEDIFF(DAY,GETDATE(),createTime)=0 and forecaster='" +
           m_alias + "' and  period='" + periods + "' ";
        dt = m_Database.GetDataTable(sql);
        if (dt != null && dt.Rows.Count > 0)
        {
            string status = dt.Rows[0]["status"].ToString();
            sql = "update  T_HealthyAutoSingle set createTime ='" +  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where  DATEDIFF(DAY,GETDATE(),createTime)=0 and forecaster='" +
            m_alias + "' and  period='" + periods + "' ";
            m_Database.Execute(sql);
            mes = "数据已更新成功！";
            return mes;
        }
        try
        {
            sql = "insert into T_HealthyAutoSingle(forecaster,period,createTime,type,status,Region)  " +
                   "values('" + m_alias + "','" + periods + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','一键发送','1','" + m_station.Replace(",", "_") + "') ";
            m_Database.Execute(sql);
            mes = "数据已更新成功！";
        }
        catch
        {
            mes = "数据更新失败，请稍后重试！";
        }

        return mes;
    }

    [WebMethod]
    public static string OnlySend(string type) {
        string mes = "";
        string message = "";
        string ftp = "";
        string email = "";
        string sql = " select count(*) from T_HEALTHYWEATHER where DATEDIFF(DAY,GETDATE(),forecastDate)=0 and  period='17' and forecaster='" + m_alias + "'";
        DataTable dt = m_Database.GetDataTable(sql);
        string periods = "10";
        if (dt.Rows.Count > 0 && dt != null) {
            if (int.Parse(dt.Rows[0][0].ToString()) > 0 && DateTime.Now.Hour>12) { // hour>12这个是新加的判断 0923 
                periods = "17";
            }
        }
        sql = " select * from T_HEALTHYWEATHER where DATEDIFF(DAY,GETDATE(),forecastDate)=0 and  period='" + periods + "' and forecaster='" + m_alias + "'";
        dt = m_Database.GetDataTable(sql);
        if (dt.Rows.Count < 0 || dt == null) {
            return mes = "没有需要发送的数据";
        }
        //检查[T_HealthyAuto]表中是否已经存在
        sql = " select * from [T_HealthyAuto] where DATEDIFF(DAY,GETDATE(),createTime)=0 and forecaster='"+m_alias + "' and  period='" + periods + "' ";
        dt = m_Database.GetDataTable(sql);
        if (dt != null && dt.Rows.Count > 0)
        {
            string status = dt.Rows[0]["status"].ToString();
            string date = dt.Rows[0]["createTime"].ToString();
            if (status == "1" || status == "0")
            {
                string fildSql = "";
                if (type == "邮件发送")
                {
                    fildSql = "isEmail='0'";
                }
                if (type == "短信发送")
                {
                    fildSql = "isMessage='0'";
                }
                if (type == "FTP发送")
                {
                    fildSql = "isFtp='0'";
                }
                mes = "成功发送到后台处理，详细发送记录请查看发送日志！";
                string update_sql = "update  T_HealthyAuto set status='0' , " + fildSql + " where " +
                      "forecaster='" + m_alias + "' and period='" + periods + "'  "; // 这里加一个更新操作
                m_Database.Execute(update_sql);
                return mes;
            }
        }
        else
        {
            try
            {
                if (type == "邮件发送")
                {
                    email = "0";
                    ftp = "1";
                    message = "1";
                }
                if (type == "短信发送")
                {
                    email = "1";
                    ftp = "1";
                    message = "0";
                }
                if (type == "FTP发送")
                {
                    email = "1";
                    ftp = "0";
                    message = "1";
                }
                sql = "insert into T_HealthyAuto(forecaster,period,createTime,type,status,isMessage,isFtp,isEmail,Region)  " +
                      "values('" + m_alias + "','" + periods + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','一键发送','0','" + message + "','" + ftp + "','" + email + "','" + m_station.Replace(",", "_") + "') ";
                m_Database.Execute(sql);
                mes = "成功发送到后台处理，详细发送记录请查看发送日志！";
            }
            catch
            {
                mes = "发送数据库失败，请稍后重试！";
            }
        }
        return mes;
    }
    /// <summary>
    /// 获取发送ftp、邮件、短信的数量以及统计总和
    /// </summary>
    /// <param name="sd">时段，是上午还是下午</param>
    /// <returns></returns>
    [WebMethod]
    public static string GetSendNum(string sd)
    {
        string userName = m_alias;
        DateTime dNow = DateTime.Now;
        string Date = dNow.ToString("yyyy-MM-dd");
        string startHour = " 00:00:00";
        string endHour = " 11:59:59";
        if (sd=="下午") {
            startHour = " 12:00:00";
            endHour = " 23:59:59";
        }

        string sql_user = " select * from T_user where Alias='" + userName + "'";
        DataTable dt_user = m_Database.GetDataTable(sql_user);
        string companyName = "";
        if (dt_user != null && dt_user.Rows.Count > 0)
        {
            companyName = dt_user.Rows[0]["WindowsUser"].ToString();
        }

        string sql = " select * from ( Select SENDUSER,RECEIVEUSER,TYPE,HealthyType,t1.EMAIL,PHONE,CASE SendStatus WHEN 1 THEN '成功'ELSE '失败'END AS SendStatus,"+
                     "CONVERT(VARCHAR(19),SENDDATE,121) as 'SENDDATE',CASE ISALL WHEN 1 THEN '是'ELSE '否'END AS ISALL,t2.WindowsUser FROM V_SENDLOG  	 t1 left join T_User t2  on t1.SendUser=t2.Alias where Type<>'FTP' " +

                     "union all Select SENDUSER,reciver,TYPE,HealthyType,t3.EMAIL,PHONE,CASE SendStatus WHEN 1 THEN '成功' ELSE '失败' END AS SendStatus, "+
                     "CONVERT(VARCHAR(19),SENDDATE,121) as 'SENDDATE',CASE ISALL WHEN 1 THEN '是' ELSE '否'END AS ISALL,t4.WindowsUser FROM V_SENDLOGII    t3 left join T_User t4  on t3.SendUser=t4.Alias where Type='FTP' ) T  " +
                    " WHERE T.SENDDATE BETWEEN '" + Date + startHour + "' and '" + Date + endHour + "' AND T.WindowsUser='" + companyName + "'";
                    //" WHERE T.SENDDATE BETWEEN '2016-10-01 00:00:00' and '2017-11-10 23:59:59' AND T.SendUser='" + userName + "'";


        DataTable dt = m_Database.GetDataTable(sql);
        string[] status = {"状态", "成功","失败","总数"};
        string[] e_status = { "title", "sucess", "failure", "total" };
        string[] type = { "FTP", "短信", "邮件" };
        StringBuilder json = new StringBuilder();
        int ftp = 0, mess = 0, email = 0;//各类型的总和
        for (int i = 0; i < status.Length; i++)
        {
            DataRow[] row = dt.Select("SendStatus='" + status[i] + "'");
            int ftp_count = 0, mess_count = 0, email_count = 0;
            for (int j = 0; j < row.Length; j++)
            {
                if (row[j]["TYPE"].ToString() == type[0])
                {
                    ftp_count++;
                }
                else if (row[j]["TYPE"].ToString() == type[1])
                {
                    mess_count++;
                }
                else if (row[j]["TYPE"].ToString() == type[2])
                {
                    email_count++;
                }
            }
            ftp += ftp_count;
            mess += mess_count;
            email += email_count;
            if (i == 0)
            {
                //第1行各列的值
                json.Append("vm.json." + e_status[i] + "[0].status='" + status[i] + "'*");
                json.Append("vm.json." + e_status[i] + "[1].status='" + type[0] + "'*");
                json.Append("vm.json." + e_status[i] + "[2].status='" + type[1] + "'*");
                json.Append("vm.json." + e_status[i] + "[3].status='" + type[2] + "'*");
            }else if(i==status.Length-1){     //最后一行计算总和
                json.Append("vm.json." + e_status[i] + "[0].status='" + status[i] + "'*");
                json.Append("vm.json." + e_status[i] + "[1].status='" + ftp + "'*");
                json.Append("vm.json." + e_status[i] + "[2].status='" + mess + "'*");
                json.Append("vm.json." + e_status[i] + "[3].status='" + email + "'*");
            }
            else
            {
                json.Append("vm.json." + e_status[i] + "[0].status='" + status[i] + "'*");
                json.Append("vm.json." + e_status[i] + "[1].status='" + ftp_count + "'*");
                json.Append("vm.json." + e_status[i] + "[2].status='" + mess_count + "'*");
                json.Append("vm.json." + e_status[i] + "[3].status='" + email_count + "'*");
            }
        }
        string txt = json.ToString();
        txt = txt.TrimEnd('*');
        return txt;
    }
}
