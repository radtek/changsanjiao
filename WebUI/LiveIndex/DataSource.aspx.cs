using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Readearth.Data;

public partial class LiveIndex_DataSource : System.Web.UI.Page
{
    public static Database m_database;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            m_database = new Database("DBCONFIG");
        }
    }
    public static string DataTableToJson(string jsonName, System.Data.DataTable dt)
    {
        StringBuilder Json = new StringBuilder();
        Json.Append("{\"" + jsonName + "\":[");
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Json.Append("[");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Json.Append("\"" + dt.Rows[i][j].ToString() + "\"");
                    if (j < dt.Columns.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
                Json.Append("]");
                if (i < dt.Rows.Count - 1)
                {
                    Json.Append(",");
                }
            }
        }
        Json.Append("]}");
        return Json.ToString();
    }
    [WebMethod]
    public static DataTable GetSite()
    {
        string sql = "select StationCo,name,sort from  dbo.Weather_Station order by sort asc";
        return m_database.GetDataTable(sql);
    }

    [WebMethod]
    public static string GetInfo(string type)
    {
        string sql = "select Dept,srcpath,lastTime,srcIp,describe from Weather_Source where srcInfo='" + type + "'";
        DataTable dt = m_database.GetDataTable(sql);
        return DataTableToJson("data",dt);
    }
    [WebMethod]
    public static DataTable GetModuleName() {
        string sql = "select SrcInfo from dbo.Weather_Source where type=1 order by SrcIndex asc ";
        DataTable dt = m_database.GetDataTable(sql);
        return dt;
    }
}