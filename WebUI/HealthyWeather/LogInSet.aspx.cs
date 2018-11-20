using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Readearth.Data;
using System.Data;
using System.Text;
using System.Collections;
using System.IO;

public partial class HealthyWeather_LogInSet : System.Web.UI.Page
{
    public static Database m_Database;
    public static string m_userName, m_alias, m_station;
    public static EmailHelper m_EmailHelper;
    protected void Page_Load(object sender, EventArgs e)
    {
        m_Database = new Database("DBCONFIGII");
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


    [WebMethod]
    public static List<DataTable> GetStationInfo()
    {
        List<DataTable> myData = new List<DataTable>();
        string strSql = "SELECT ID AS DM,Description AS MC FROM T_Classes";
        myData.Add(m_Database.GetDataTable(strSql));
        strSql = "SELECT ID AS DM,description AS MC FROM T_ShanghaiArea";
        myData.Add(m_Database.GetDataTable(strSql));
        return myData;
    }


    [WebMethod]
    public static string DelUser(string userName)
    {
        string strSql = "DELETE FROM T_User WHERE UserName='" + userName +"'";  
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
    public static string EditUser(string type, string UserName, string SN, int BZ, string Alias, string EMail, string POSTIONAREA, string WindowsUser)
    {
        string strSql;
        UserName = HttpUtility.UrlDecode(UserName);
        SN = HttpUtility.UrlDecode(SN);
        Alias = HttpUtility.UrlDecode(Alias);
        POSTIONAREA = HttpUtility.UrlDecode(POSTIONAREA);
        WindowsUser = HttpUtility.UrlDecode(WindowsUser);

        string DateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:00");
        strSql = "SELECT * FROM T_User WHERE Alias='" + Alias+"'";
        DataTable dt = m_Database.GetDataTable(strSql);
        if (type == "创建")
        {

            if (dt.Rows.Count == 0)
            {
                strSql = "INSERT T_User(UserName,SN,BZ,Alias,EMail,DateTime,POSTIONAREA,WindowsUser) VALUES('" + UserName + "','" + SN + "','" + BZ + "','" + Alias + "','" + EMail + "','" + DateTime + "','" + POSTIONAREA + "','" + WindowsUser + "')";
            }
            else { return "存在重名用户！"; }
        }
        else
        {
            if (dt.Rows.Count == 0 || (dt.Rows.Count == 1 && dt.Rows[0][0].ToString() == UserName))
            {
                strSql = "UPDATE T_User SET BZ='" + BZ + "',Alias='" + Alias + "',EMail='" + EMail + "', POSTIONAREA='" + POSTIONAREA + "', WindowsUser='" + WindowsUser + "' WHERE UserName='" + UserName + "'";
            }
            else { return "存在重名用户！"; }
        }
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
    public static DataTable GetCompanys()
    {
        string strSql = "select * from [dbo].[T_ShanghaiArea]";
        try
        {
            return m_Database.GetDataTable(strSql);
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}