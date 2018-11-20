using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using Readearth.Data;

public partial class HealthyWeather_SendLog : System.Web.UI.Page
{
    public static Database m_Database;
    public static string m_userName, m_alias, m_station;
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
    public static DataTable GetHealthyType()
    {
        List<DataTable> myData = new List<DataTable>();
        string strSql = "SELECT MC FROM D_HEALTYTYPE";
        return m_Database.GetDataTable(strSql);
    }
}