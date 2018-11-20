using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Readearth.Data;
using System.Data;
using System.Text;
using System.Collections;
using System.IO;
using Aspose.Cells;

public partial class HealthyWeather_PubUserSet : System.Web.UI.Page
{
    public static Database m_Database;
    public static string m_userName, m_alias, m_station;
    public static EmailHelper m_EmailHelper;
    public static Hashtable hstable = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        InitDB();
        hstable.Clear();
        string strSqls = " select * from  T_ShanghaiArea";
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
        if (Request.Cookies["User"] != null)
        {
            m_userName = Request.Cookies["User"]["name"].ToString();
            DataTable dt = m_Database.GetDataTable("SELECT POSTIONAREA,Alias FROM T_USER WHERE USERNAME='" + m_userName + "'");
            if (dt.Rows.Count > 0)
            {
                m_station = dt.Rows[0][0].ToString();
                m_alias = dt.Rows[0][1].ToString();
            }
        }
        else
            Response.Redirect("../Default.aspx", true);
    }
    private static void InitDB(){
        m_Database = new Database("DBCONFIGII");
    }

    private void  InitUser() {

        if (Request.Cookies["User"] != null)
        {
            m_userName = Request.Cookies["User"]["name"].ToString();
            DataTable dt = m_Database.GetDataTable("SELECT POSTIONAREA,Alias FROM T_USER WHERE USERNAME='" + m_userName + "'");
            if (dt.Rows.Count > 0)
            {
                m_station = dt.Rows[0][0].ToString();
                m_alias = dt.Rows[0][1].ToString();
            }
        }
    }

    [WebMethod]
    public static List<DataTable> GetHealthyInfo()
    {
        List<DataTable> myData = new List<DataTable>();
        string strSql = "SELECT ID AS DM,MC FROM D_HEALTYTYPE";
        myData.Add(m_Database.GetDataTable(strSql));
        strSql = "SELECT DM,MC FROM D_PublishLvl";
        myData.Add(m_Database.GetDataTable(strSql));
        strSql = "SELECT DM,MC FROM D_PublishPeriod";
        myData.Add(m_Database.GetDataTable(strSql));
        return myData;
    }
    
    [WebMethod]
    public static string DelGroup(string groupName)
    {
        groupName = HttpUtility.UrlDecode(groupName);
        string strSql = "DELETE FROM T_PubUser WHERE groupName='" + groupName + "'";
        try
        {
            m_Database.Execute(strSql).ToString();
            strSql = "DELETE FROM T_PubGroup WHERE groupName='" + groupName + "'";
            return m_Database.Execute(strSql).ToString();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [WebMethod]
    public static string EditGroup(string type, string newName, string groupName,string region, string healthyType, string Message_PubLvl, string Message_PubTime, string Email_PubLvl, string Email_PubTime)
    {
        string strSql;
        try
        {
            groupName = HttpUtility.UrlDecode(groupName);
            newName = HttpUtility.UrlDecode(newName);
            region = HttpUtility.UrlDecode(region).Replace("$","_");
            if (type == "new")
                strSql = "INSERT T_PubGroup(GroupName,Region,HealthyType, Message_PubLvl, Message_PubTime, Email_PubLvl, Email_PubTime) VALUES('"
                + groupName + "','" + region + "','" + healthyType + "','" + Message_PubLvl.Trim() + "','" + Message_PubTime + "','" + Email_PubLvl.Trim() + "','" + Email_PubTime + "')";
            else
                strSql = "UPDATE T_PubGroup SET GroupName='" + newName + "',HealthyType='" + healthyType + "',Region='" + region + "',Message_PubLvl='"
                    + Message_PubLvl.Trim() + "',Message_PubTime='" + Message_PubTime + "',Email_PubLvl='" + Email_PubLvl.Trim() + "',Email_PubTime='" + Email_PubTime
                    + "' WHERE GroupName='" + groupName + "'";
            m_Database.Execute(strSql);
            if (newName != groupName)
            {
                strSql = "UPDATE T_PUBUSER SET GROUPNAME='" + newName + "' WHERE GROUPNAME='" + groupName + "'";
                m_Database.Execute(strSql);
            }

            UpdateUser(newName.Trim(), groupName.Trim(), healthyType.Trim(), Message_PubLvl.Trim(), Message_PubTime.Trim(), Email_PubLvl.Trim(), Email_PubTime.Trim());
            return "1";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }


    //后加的，如果组中的信息更改则对应用户的信息也更新
    public static void UpdateUser(string newName, string groupName, string healthyType, string Message_PubLvl, string Message_PubTime, string Email_PubLvl, string Email_PubTime)
    {
        string update = "update t_pubuser set GroupName='" + newName + "',HealthyType='" + healthyType + "',Message_PubLvl='" + Message_PubLvl + "',Message_PubTime='" + Message_PubTime + "',"
        + "Email_PubLvl='" + Email_PubLvl + "',Email_PubTime='" + Email_PubTime + "' where GroupName='" + groupName + "'";
        m_Database.Execute(update);
    }



    [WebMethod]
    public static string DelUser(string userID)
    {
        string strSql = "DELETE FROM T_PubUser WHERE userID IN (" + userID + ")";
        try
        {
            return m_Database.Execute(strSql).ToString();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [WebMethod]
    public static string EditUser(string userID, string Name, string groupName,string phone,string email, string healthyType, string Message_PubLvl, string Message_PubTime, string Email_PubLvl,string Email_PubTime, string Remark)
    {
        string strSql;
        groupName = HttpUtility.UrlDecode(groupName);
        Name = HttpUtility.UrlDecode(Name);
        Remark = HttpUtility.UrlDecode(Remark);
        if (userID == "")
            strSql = "INSERT T_PubUser(Name,GroupName,Phone,Email,HealthyType, Message_PubLvl, Message_PubTime,CanMessage, Email_PubLvl,Email_PubTime,CanEmail,Remark) VALUES('" + Name + "','"
                + groupName + "','" + phone + "','" + email + "','" + healthyType + "','" + Message_PubLvl + "','" + Message_PubTime + "','" + (Message_PubLvl == "" ? 0 : 1) + "','" + Email_PubLvl + "','" + Email_PubTime + "','" + (Email_PubLvl == "" ? 0 : 1) + "','" + Remark + "')";
        else
            strSql = "UPDATE T_PubUser SET Name='" + Name + "',GroupName='" + groupName + "',phone='" + phone + "',email='" + email + "', HealthyType='" + healthyType + "', Message_PubLvl='"
                + Message_PubLvl + "', Message_PubTime='" + Message_PubTime + "', Email_PubLvl='" + Email_PubLvl + "', Email_PubTime='" + Email_PubTime + "', Remark='" + Remark + "', CanMessage=" 
                + (Message_PubLvl == "" ? 0 : 1) + ", CanEmail=" + (Email_PubLvl == "" ? 0 : 1) + " WHERE userID='" + userID + "'";
        try
        {
            return m_Database.Execute(strSql).ToString();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [WebMethod]
    public static DataTable QueryGroup(string healthyType)
    {
        string strWhere = healthyType == "" ? "" : "WHERE HealthyType like '%" + healthyType + "%'";
        string strSQl = @"SELECT GroupName,Region,HealthyType,Message_PubLvl,Message_PubTime,Email_PubLvl,Email_PubTime,COUNT(Name) AS USERCOUNT,
            STUFF(cast((select DISTINCT ','+Name from V_GROUPINFO t2 where t1.GroupName = t2.GroupName for xml path('')) as varchar(100)),1,1,'') as Name,
            STUFF(cast((select DISTINCT ','+Phone from V_GROUPINFO t2 where t1.GroupName = t2.GroupName for xml path('')) as varchar(100)),1,1,'') as Phone,
            STUFF(cast((select DISTINCT ','+Email from V_GROUPINFO t2 where t1.GroupName = t2.GroupName for xml path('')) as varchar(100)),1,1,'') as Email
            FROM V_GROUPINFO t1 " + strWhere + " group by GroupName,Region,HealthyType,Message_PubLvl,Message_PubTime,Email_PubLvl,Email_PubTime";
        return m_Database.GetDataTable(strSQl);
    }



    [WebMethod]
    public static DataTable QueryGroupII(string healthyType)
    {
        string strWhere = healthyType == "全部" ? "" : "WHERE Region like '%" + healthyType + "%'";
        string strSQl = @"SELECT GroupName,Region,HealthyType,Message_PubLvl,Message_PubTime,Email_PubLvl,Email_PubTime,COUNT(Name) AS USERCOUNT,
            STUFF(cast((select DISTINCT ','+Name from V_GROUPINFO t2 where t1.GroupName = t2.GroupName for xml path('')) as varchar(100)),1,1,'') as Name,
            STUFF(cast((select DISTINCT ','+Phone from V_GROUPINFO t2 where t1.GroupName = t2.GroupName for xml path('')) as varchar(100)),1,1,'') as Phone,
            STUFF(cast((select DISTINCT ','+Email from V_GROUPINFO t2 where t1.GroupName = t2.GroupName for xml path('')) as varchar(100)),1,1,'') as Email
            FROM V_GROUPINFO t1 " + strWhere + " group by GroupName,Region,HealthyType,Message_PubLvl,Message_PubTime,Email_PubLvl,Email_PubTime";
        return m_Database.GetDataTable(strSQl);
    }

    [WebMethod]
    public static string EmailCustom(string emailAddress, string msg, string title)
    {
        string type = "邮件";
        string[] emails = emailAddress.Split('&');
        int count = 0, successCount = 0;
        title = HttpUtility.UrlDecode(title);
        msg = HttpUtility.UrlDecode(msg);
        for (int i = 0; i < emails.Length; i++)
        {
            string[] info = emails[i].Split('\"');
            successCount++;
            if (EmailHelper.sendmail(title, msg, info[1], false))
            {
                WriteSendLog(info[0], type, 1);
                count++;
            }
            else WriteSendLog(info[0], type, 0);
        }
        EmailHelper.InsertLog("发送邮件至：" + emailAddress + "\t邮件内容为：" + msg + "\t邮件标题为：" + title + (count > successCount ? "\t其中" + (count - successCount) + "封发送失败。" : ""));
        if (count == 0) return "没有需要发送的邮件！";
        else if (count == successCount)
            return "总共" + count + "封邮件，全部发送成功！";
        else return "总共" + count + "封邮件，" + (count - successCount) + "封发送失败，可以联系平台维护人员来查取失败原因。";
    }


    [WebMethod]
    public static string EmailCustomII(string emailAddress, string msg, string title)
    {
        string type = "邮件";
        string[] emails = emailAddress.Split(',');
        int count = 0, successCount = 0;
        title = HttpUtility.UrlDecode(title);
        msg = HttpUtility.UrlDecode(msg);
        for (int i = 0; i < emails.Length; i++)
        {
            string info = emails[i];
            successCount++;
            if (EmailHelper.sendmail(title, msg, info, false))
            {
                WriteSendLog("-1", type, 1);
                count++;
            }
            else WriteSendLog("-1", type, 0);
        }
        EmailHelper.InsertLog("发送邮件至：" + emailAddress + "\t邮件内容为：" + msg + "\t邮件标题为：" + title + (count > successCount ? "\t其中" + (count - successCount) + "封发送失败。" : ""));
        if (count == 0) return "发送失败！";
        else if (count == successCount)
            return "总共" + count + "封邮件，全部发送成功！";
        else return "总共" + count + "封邮件，" + (count - successCount) + "封发送失败，可以联系平台维护人员来查取失败原因。";
    }

    [WebMethod]
    public static string EmailRegular(string UserIDS, string time, int isAll, string m_aliass,string products)
    {
        if (m_aliass == "") {
            m_aliass = m_alias;
        }

        string type = "邮件";
        DateTime dtNow = DateTime.Now;
        string[] userIDs = UserIDS.TrimEnd('&').Split('&');
        int count = 0, successCount = 0;
        string strSql = "", html = "", title = "";
        if (m_Database == null) InitDB();
        for (int i = 0; i < userIDs.Length; i++)
        {
            strSql = @"SELECT DISTINCT Email,T.EMAIL_PUBLVL,R.REGION,
                    STUFF(cast((select DISTINCT ','+CHEALTHYTYPE from V_USERINFO t2 where t.NAME = t2.NAME  for xml path('')) as varchar(100)),1,1,'') as [types],
                    STUFF(cast((select DISTINCT ','+OPERATE from V_USERINFO t2 where t.USERID = t2.USERID  for xml path('')) as varchar(100)),1,1,'') as OPERATE
                    FROM T_PUBUSER T INNER JOIN T_PUBGROUP R ON T.GROUPNAME=R.GROUPNAME INNER JOIN D_PUBLISHLVL D1 ON T.EMAIL_PUBLVL=D1.DM
                    LEFT OUTER JOIN D_HealtyType D ON T.HealthyType LIKE '%' + CAST(D.ID AS varchar(2)) + '%' LEFT OUTER JOIN D_PublishPeriod ON 
                    T.Email_PubTime LIKE '%' + D_PublishPeriod.DM + '%' WHERE USERID='" + userIDs[i] + "'";

            DataTable dt = m_Database.GetDataTable(strSql);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                html = "";
                products = products.Replace("2", "儿童感冒").Replace("3", "青年感冒").Replace("4", "老年感冒").
                                  Replace("5", "COPD").Replace("6", "儿童哮喘").Replace("7", "中暑").Replace("8", "重污染");
                string email = dr[0].ToString(), lvl = dr[1].ToString(), region = dr[2].ToString(), healType = products;

                strSql = "SELECT CODE,CODE2 FROM D_PUBLISHLVL WHERE DM>='" + lvl + "'";
                DataTable dt2 = m_Database.GetDataTable(strSql);
                lvl = "";
                foreach (DataRow dr2 in dt2.Rows)
                {
                    lvl += "'" + dr2[0].ToString() + "','" + dr2[1].ToString() + "',";
                }

                //03-27
                foreach (string st in region.Split('_'))
                {
                    strSql = "SELECT CONVERT(VARCHAR(10),LST,121),PERIOD,TYPE,[LEVEL] ,People,premunition FROM T_HEALTHYWEATHER WHERE DATEDIFF(DAY,GETDATE(),ForecastDate) = 0 AND STATION='" + st
                        + "' AND FORECASTER='" + m_aliass + "' AND [TYPE] IN ('" + healType.Replace(",", "','") + "') AND [LEVEL] IN (" + lvl.TrimEnd(',') + ") AND (";
                    
                    string timeOperateValue = "";
                    foreach (string tt in time.Split('_')) {
                        switch (tt) {
                            case "01": timeOperateValue += "10_0,"; break;
                            case "02": timeOperateValue += "10_1,"; break;
                            case "03": timeOperateValue += "17_1,"; break;
                            case "04": timeOperateValue += "17_2,"; break;
                        }
                    }
                    timeOperateValue = timeOperateValue.TrimEnd(',');
                    string[] timeOperates = timeOperateValue.Split(','); // 04-14 xuehui 
                    for (int k = 0; k < timeOperates.Length; k++)
                    {
                        string[] ops = timeOperates[k].Split('_');
                        strSql += (k == 0 ? "" : " OR ") + "(PERIOD='" + ops[0] + "' AND DATEDIFF(DAY,GETDATE(),LST)=" + ops[1] + ")";
                    }
                    dt = m_Database.GetDataTable(strSql + ") ORDER BY TYPE,LST,PERIOD DESC");
                    string oldType = "", oldTime = "";
                    List<string[]> infoes = new List<string[]>();
                    foreach (DataRow dr2 in dt.Rows)
                    {
                        if (oldType != dr2[2].ToString())
                        {
                            if (oldType != "")
                            {
                                title = SendHelper.GetTitle(dtNow.ToString("MM月dd日"), hstable[st].ToString(), oldType,st);
                                html = SendHelper.GetEmailHtml(type, userIDs[i], title, infoes);
                                if (infoes.Count > 0)
                                {
                                    count++;
                                    if (EmailHelper.sendmail(title, html, email, true))
                                    {
                                        if (isAll == 0) WriteSendLog(userIDs[i], type, oldType, 1, isAll);
                                        successCount++;
                                    }
                                    else if (isAll == 0) WriteSendLog(userIDs[i], type, oldType, 0, isAll);
                                }
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
                        title = SendHelper.GetTitle(dtNow.ToString("MM月dd日"), hstable[st].ToString(), oldType,st);
                        html = SendHelper.GetEmailHtml(type, userIDs[i], title, infoes);
                        if (html != "")
                        {
                            count++;
                            if (EmailHelper.sendmail(title, html, email, true))
                            {
                                if (isAll == 0) WriteSendLog(userIDs[i], type, oldType, 1, isAll);
                                successCount++;
                            }
                            else if (isAll == 0) WriteSendLog(userIDs[i], type, oldType, 0, isAll);
                        }
                    }
                }
            }
        }
        if (isAll == 0)
        {
            if (count == 0) return "没有需要发送的邮件！";

            else if (count == successCount)
                return "总共" + count + "封邮件，全部发送成功！";
            else return "总共" + count + "封邮件，"+(count-successCount)+"封发送失败，可以联系平台维护人员来查取失败原因。";
        }
        else
        {
            WriteSendLog("", type, "", 1, isAll);
            return "";
        }
    }

    [WebMethod]
    public static string MessageCustom(string phone, string msg)
    {
        string reMes = "";
        string[] phones = phone.Split('&');
        msg = HttpUtility.UrlDecode(msg);
        for (int i = 0; i < phones.Length; i++)
        {
            string[] info = phones[i].Split('\"');
            string content = info[1] + "\t" + msg + "\r\n";


            string direct = System.Web.HttpContext.Current.Server.MapPath("") + "\\tempMessage\\";
            if (!Directory.Exists(direct)) Directory.CreateDirectory(direct);
            string fileName = "SMS_3_YWK" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".txt";
            File.Create(direct + fileName).Close();
            StreamWriter sw = new StreamWriter(direct + fileName, false, Encoding.GetEncoding("GB2312"));
            sw.Write(content);
            sw.Close();
            try
            {
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
                EmailHelper.InsertLog("发送短信至：" + phone + "\t短信内容为：" + msg);
                WriteSendLog(info[0], "短信", 1);
                reMes= "1.1";

            }
            catch (Exception ex)
            {
                WriteSendLog(info[0], "短信", 0);
                reMes= ex.Message;
            }

        }
        return reMes;
        
    }

    [WebMethod]
    public static string MessageCustomII(string phone, string msg)
    {
        string reMes = "";
        string[] phones = phone.Split(',');
        msg = HttpUtility.UrlDecode(msg);
        for (int i = 0; i < phones.Length; i++)
        {
            string info = phones[i];
            string content = info + "\t" + msg + "\r\n";

            string direct = System.Web.HttpContext.Current.Server.MapPath("") + "\\tempMessage\\";
            if (!Directory.Exists(direct)) Directory.CreateDirectory(direct);
            string fileName = "SMS_3_YWK" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".txt";
            File.Create(direct + fileName).Close();
            StreamWriter sw = new StreamWriter(direct + fileName, false, Encoding.GetEncoding("GB2312"));
            sw.Write(content);
            sw.Close();
            try
            {
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
                EmailHelper.InsertLog("发送短信至：" + phone + "\t短信内容为：" + msg);
                WriteSendLog("-1", "短信", 1);
                reMes= "1.1";
            }
            catch (Exception ex)
            {
                WriteSendLog("-1", "短信", 0);
                reMes= ex.Message;
            }
        }
        return reMes;
       
    }


    //王斌  2017.5.11
    [WebMethod]
    public static string MessageRegular(string UserIDS, string time, int isAll, string products,string m_aliass )
    {
        if (m_aliass == "")
        {
            m_aliass = m_alias;
        }
        string type = "短信";
        string reMes = "";
        DateTime dtNow = DateTime.Now;
        string[] userIDs = UserIDS.TrimEnd('&').Split('&');
        string strSql = "", txtContent = "";
        for (int i = 0; i < userIDs.Length; i++)
        {
            strSql = @"SELECT DISTINCT PHONE,T.MESSAGE_PUBLVL,R.REGION,
                    STUFF(cast((select DISTINCT ','+CHEALTHYTYPE from V_USERINFO t2 where t.NAME = t2.NAME   for xml path('')) as varchar(100)),1,1,'') as [types],
                    STUFF(cast((select DISTINCT ','+OPERATE from V_USERINFO t2 where t.USERID = t2.USERID   for xml path('')) as varchar(100)),1,1,'') as OPERATE
                    FROM T_PUBUSER T INNER JOIN T_PUBGROUP R ON T.GROUPNAME=R.GROUPNAME INNER JOIN D_PUBLISHLVL D1 ON T.MESSAGE_PUBLVL=D1.DM
                    LEFT OUTER JOIN D_HealtyType D ON T.HealthyType LIKE '%' + CAST(D.ID AS varchar(2)) + '%' LEFT OUTER JOIN D_PublishPeriod ON 
                    T.Message_PubTime LIKE '%' + D_PublishPeriod.DM + '%' WHERE USERID='" + userIDs[i] + "'";

            DataTable dt = m_Database.GetDataTable(strSql);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                products = products.Replace("2", "儿童感冒").Replace("3", "青年感冒").Replace("4", "老年感冒").
                                  Replace("5", "COPD").Replace("6", "儿童哮喘").Replace("7", "中暑").Replace("8", "重污染");
                string phone = dr[0].ToString(), lvl = dr[1].ToString(), region = dr[2].ToString(), healType = products;

                strSql = "SELECT CODE,CODE2 FROM D_PUBLISHLVL WHERE DM>='" + lvl + "'";
                DataTable dt2 = m_Database.GetDataTable(strSql);
                lvl = "";
                foreach (DataRow dr2 in dt2.Rows)
                {
                    lvl += "'" + dr2[0].ToString() + "','" + dr2[1].ToString() + "',";
                }

                foreach (string st in region.Split('_'))
                {
                    txtContent = "";
                    strSql = "SELECT CONVERT(VARCHAR(10),LST,121),PERIOD,TYPE,[LEVEL],People,premunition  FROM T_HEALTHYWEATHER WHERE DATEDIFF(DAY,GETDATE(),ForecastDate) = 0 AND STATION='" + st
                        + "' AND FORECASTER='" + m_aliass + "' AND [TYPE] IN ('" + healType.Replace(",", "','") + "') AND [LEVEL] IN (" + lvl.TrimEnd(',') + ") AND (";

                    string timeOperateValue = "";
                    foreach (string tt in time.Split('_'))
                    {
                        switch (tt)
                        {
                            case "01": timeOperateValue += "10_0,"; break;
                            case "02": timeOperateValue += "10_1,"; break;
                            case "03": timeOperateValue += "17_1,"; break;
                            case "04": timeOperateValue += "17_2,"; break;
                        }
                    }
                    timeOperateValue = timeOperateValue.TrimEnd(',');
                    string[] timeOperates = timeOperateValue.Split(','); // 04-14 xuehui 
                    for (int k = 0; k < timeOperates.Length; k++)
                    {
                        string[] ops = timeOperates[k].Split('_');
                        strSql += (k == 0 ? "" : " OR ") + "(PERIOD='" + ops[0] + "' AND DATEDIFF(DAY,GETDATE(),LST)=" + ops[1] + ")";
                    }
                    dt = m_Database.GetDataTable(strSql + ") ORDER BY TYPE,LST,PERIOD DESC");
                    string oldType = "", oldTime = "", pubTime, message; DateTime theDt;
                    foreach (DataRow dr2 in dt.Rows)
                    {
                        if (oldTime != dr2[0].ToString())
                        {
                            theDt = DateTime.Parse(dr2[0].ToString());
                            pubTime = "0" + ((dr2[1].ToString() == "10" ? 1 : 2) + theDt.CompareTo(DateTime.Now.Date)).ToString();
                            oldType = dr2[2].ToString();
                            string lvls = dr2[3].ToString();
                            message = SendHelper.GetMessage(hstable[st].ToString(), theDt, oldType, lvls, dr2[5].ToString(),st);
                            txtContent += phone + "\t" + message + "\r\n";
                        }
                    }

                    //推送上去短信
                    #region 
                    string direct = System.Web.HttpContext.Current.Server.MapPath("") + "\\tempMessage\\";
                    if (!Directory.Exists(direct)) Directory.CreateDirectory(direct);
                    string fileName = "SMS_3_YWK" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".txt";
                    File.Create(direct + fileName).Close();
                    StreamWriter sw = new StreamWriter(direct + fileName, false, Encoding.GetEncoding("GB2312"));
                    sw.Write(txtContent);
                    sw.Close();
                    try
                    {
                        OpenFTP.FTP f = new OpenFTP.FTP();
                        f.Connect("172.21.107.24", "SmsRequest", "aa9dsMTr");
                        f.Files.Upload(fileName, direct + fileName);
                        while (!f.Files.UploadComplete)
                        {
                            Console.WriteLine("Uploading: TotalBytes: " + f.Files.TotalBytes.ToString() + ", : PercentComplete: " + f.Files.PercentComplete.ToString());
                        }
                        f.Disconnect();
                        f = null;
                        WriteSendLog(userIDs[i], type, oldType, 1, isAll);
                        //FtpHelper.Upload("SmsRequest", "aa9dsMTr", direct + fileName, "ftp://172.21.107.24/");
                        reMes= "1.1";
                    }
                    catch (Exception ex)
                    {
                        reMes= ex.Message;
                        WriteSendLog(userIDs[i], type, oldType, 0, isAll);
                    }
                    #endregion
                }
            }
        }
        return reMes;
    }

    private static void WriteSendLog(string userID, string type, int sendStatus)
    {
        string strSql = "INSERT T_SENDLOG(SendUser,ReceiverID,type,SendStatus,SendDate) VALUES('" + m_alias + "','" + userID + "','" + type + "'," + sendStatus + ",GETDATE())";
        m_Database.Execute(strSql);
    }

    private static void WriteSendLog(string userID, string type, string healthyType, int sendStatus, int isAll)
    {
        if (healthyType != null)
        {
            string strSql = "INSERT T_SENDLOG(SendUser,ReceiverID,type,HealthyType,SendStatus,SendDate,IsAll) VALUES('" + m_alias + "','" + userID
                + "','" + type + "','" + healthyType + "'," + sendStatus + ",GETDATE()," + (isAll == 0 ? "0" : "1") + ")";
            m_Database.Execute(strSql);
        }
    }


    public void Button1_Click(object sender, EventArgs e)
    {
    
        string [] u_header={"姓名","手机号码","邮件地址","是否发送短信","是否发送邮件","分组名称","所属区域","短信发送等级","短信时效","邮件发送等级","邮件时效","备注"};
        string[] g_header = {"分组","所属区域","疾病类型","短信发送等级","短信时效","邮件发送等级","邮件时效"};
        DataTable d_PubUser = new DataTable();
        DataTable d_PubGroup = new DataTable();
        string sql_User = "SELECT * FROM T_PubUser";
        string sql_Group = "SELECT * FROM T_PubGroup";
        d_PubUser = m_Database.GetDataTable(sql_User);
        d_PubGroup = m_Database.GetDataTable(sql_Group);
        Workbook workbook = new Workbook();
        Aspose.Cells.Style styleTitle=workbook.Styles[workbook.Styles.Add()];//新增样式
        styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        workbook.Worksheets.Clear();
        workbook.Worksheets.Add("A(用户)").AutoFitColumns();
        workbook.Worksheets.Add("B(分组)").AutoFitColumns();
        Cells u_cell = workbook.Worksheets["A(用户)"].Cells;
        Cells g_cell = workbook.Worksheets["B(分组)"].Cells;
        for (int i = 0; i < u_header.Length; i++) {
            u_cell[0, i].PutValue(u_header[i]);
        }
        for (int i = 0; i < g_header.Length; i++) {
            g_cell[0, i].PutValue(g_header[i]);
        }
        for (int i = 0; i < d_PubUser.Rows.Count;i++ )
        {
            DataRow dr=d_PubUser.Rows[i];
            for (int j = 0; j < u_header.Length; j++) {
                u_cell[(i + 1), j].PutValue(dr[j]);
            }
        }
        for (int i = 0; i < d_PubGroup.Rows.Count; i++) {
            DataRow dr = d_PubGroup.Rows[i];
            for (int j = 0; j < g_header.Length; j++) {
                g_cell[(i + 1), j].PutValue(dr[j]);
            }
        }
        string fileName = "用户导出" + DateTime.Now.ToString("yyyy年MM月dd日");
        string UserAgent = Request.ServerVariables["http_user_agent"].ToLower();
        Response.ContentType = "application/ms-excel";
        Response.AddHeader("Content-Disposition", "attachment:filement=" + fileName);
        Response.BinaryWrite(workbook.SaveToStream().ToArray());
        Response.Flush();
        Response.End();

    }


}