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

public partial class HealthyWeather_AirEleFore : System.Web.UI.Page
{
    public static Database m_Database;
    protected void Page_Load(object sender, EventArgs e)
    {
        m_Database = new Database("EleFore");
    }
    [WebMethod]
    public static string Getdate() {
        string maxTime = "";
        string sql = "SELECT MAX(forecastdate) from predicted_data_aqi;";
        try
        {
            DataTable dt = m_Database.GetDataTableMySQL(sql);
            maxTime = dt.Rows[0][0].ToString();
            maxTime = DateTime.Parse(maxTime).ToString("yyyy-MM-dd HH:00:00");
        } catch (Exception e) {
            maxTime = "error";
        }
        return maxTime;
    }
    [WebMethod]
    public static string GetChart(string pollName,string time) {
        string txt = "";
        try
        {
            string field = GetPollField(pollName);
            string sql = "SELECT UNIX_TIMESTAMP(lst)*1000 as lst,sitename," + field + " from predicted_data_aqi WHERE forecastdate ='" + time + "';";
            DataTable dt = m_Database.GetDataTableMySQL(sql);
            StringBuilder json = new StringBuilder();
            
            if (dt != null && dt.Rows.Count > 0)
            {
                string lst = "", val = "";
                json.Append("[");
                foreach (DataRow row in dt.Rows)
                {
                    //float f = float.Parse(row["lst"].ToString());
                    lst =row["lst"].ToString().Split('.')[0];
                    val = row[field].ToString();
                    json.Append("["+lst+","+val+"],");
                }
                json = json.Remove(json.Length-1,1);
                json.Append("]");
                txt = json.ToString()+"&"+dt.Rows[0]["sitename"].ToString();
            }
        }
        catch(Exception e) { txt = "error"; }
        //txt = json.ToString();
        return txt;
    }
    public static string GetPollField(string pollName) {
        string field = "";
        switch (pollName) {
            case "PM2.5":
                field = "pm25_predicted";
                break;
            case "PM10":
                field = "pm10_predicted";
                break;
            case "SO2":
                field = "so2_predicted";
                break;
            case "03":
                field = "o3_predicted";
                break;
            case "NO2":
                field = "no2_predicted";
                break;
            case "CO":
                field = "co_predicted";
                break;
        }
        return field;
    }
}