using Readearth.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HealthyWeather_VisFore : System.Web.UI.Page
{
    public static Database m_Database;
    protected void Page_Load(object sender, EventArgs e)
    {
        m_Database = new Database("EleFore");
    }
    [WebMethod]
    public static string Getdate()
    {
        string maxTime = "";
        string sql = "select MAX(forecastdate) FROM predicted_data_vis;";
        try
        {
            DataTable dt = m_Database.GetDataTableMySQL(sql);
            maxTime = dt.Rows[0][0].ToString();
            maxTime = DateTime.Parse(maxTime).ToString("yyyy-MM-dd HH:00:00");
        }
        catch (Exception e)
        {
            maxTime = "error";
        }
        return maxTime;
    }
    [WebMethod]
    public static DataTable GetSelVal()
    {
        string sql = "SELECT siteid,sitename from predicted_data_vis GROUP BY sitename,siteid  ORDER BY siteid asc;";
        DataTable dt = new DataTable();
        try
        {
            dt = m_Database.GetDataTableMySQL(sql);
        }
        catch (Exception e)
        {
        }
        return dt;
    }
    [WebMethod]
    public static string GetChart(string sites,string time) {
        string[] siteidArr = sites.Split(',');
        string siteids = "";
        foreach (var str in siteidArr) {
            siteids += "'"+str+"',";
        }
        siteids = siteids.TrimEnd(',');
        StringBuilder json = new StringBuilder();
        string txt = "";
        string sql = "SELECT UNIX_TIMESTAMP(lst)*1000 as lst,siteid,sitename,vis_scale_predicted FROM predicted_data_vis where forecastdate='"+time+"' and siteid in ("+ siteids + ");";
        try
        {
            DataTable dt = m_Database.GetDataTableMySQL(sql);
            if (dt != null && dt.Rows.Count > 0) {
                foreach (var str in siteidArr) {
                    DataRow[] row = dt.Select("siteid='" + str + "'");
                    if (row.Length > 0) {
                        json.Clear();
                        json.Append("[");
                        foreach (DataRow dr in row) {
                            string lst = dr["lst"].ToString().Split('.')[0];
                            string val = dr["vis_scale_predicted"].ToString();
                            json.Append("["+lst+","+val+"],");
                        }
                        json.Remove(json.Length-1,1);
                        json.Append("]");
                        txt += json.ToString() + "&" + row[0]["sitename"]+"#";
                    }
                }
                txt = txt.TrimEnd('#');
            }
        }
        catch (Exception e) { txt = "error"; }
        return txt;
    }
}