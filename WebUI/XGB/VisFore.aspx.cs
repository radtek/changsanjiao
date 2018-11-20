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
    public static DataTable siteDT = new DataTable();
    public static DataTable shiKDT = new DataTable();
    public static DataTable foreDT = new DataTable();
    public static string[] siteidArr;

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
        siteDT = new DataTable();
        try
        {
            siteDT = m_Database.GetDataTableMySQL(sql);
        }
        catch (Exception e)
        {
        }
        return siteDT;
    }
    [WebMethod]
    public static string GetChart(string sites,string time) {
        siteidArr = sites.Split(',');
        string siteids = "";
        foreach (var str in siteidArr) {
            siteids += "'"+str+"',";
        }
        siteids = siteids.TrimEnd(',');
        string txt = "";
        string sql = "SELECT lst,siteid,sitename,vis_scale_predicted as vis FROM predicted_data_vis where forecastdate='" + time+"' and siteid in ("+ siteids + ") order by lst asc;";
        try
        {
            foreDT = m_Database.GetDataTableMySQL(sql);
            if (foreDT != null && foreDT.Rows.Count > 0)
            {
                DateTime maxTime, minTime;
                GetMaxMinTime(time, siteids,out maxTime,out minTime);
                txt = JoinJson(siteidArr, foreDT,maxTime,minTime,"fore");
                txt = txt +"@" + GetShiK(time, siteids, siteidArr);
            }
        }
        catch (Exception e) { txt = "error"; }
        return txt;
    }

    private static string JoinJson(string[] siteidArr, DataTable dt,DateTime maxTime,DateTime minTime,string flag)
    {
        string txt = "";
       StringBuilder json = new StringBuilder();
        json.Append("[");
        foreach (var str in siteidArr)
        {
            
            for (DateTime time = minTime; time <= maxTime; time = time.AddHours(1)) {
                DataRow[] row = dt.Select("siteid='" + str + "' and lst='" + time + "'");
                string milliseconds = GetMillSeconds(time);
                if (row.Length > 0)
                {
                    string val = row[0]["vis"].ToString();
                    if (flag == "shiK" && val!="") {
                        val = GetVisGrade(float.Parse(val));
                    }
                    val = val == "" ? "null" : val;
                    json.Append("[" + milliseconds + "," + val + "],");
                }
                else {
                    json.Append("[" + milliseconds + ",null],");
                }
            }
            json.Remove(json.Length - 1, 1);
            json.Append("]&" + GetSite(str) + "#[");
            //txt += json.ToString() + "&" + str + "#";

        }
        json.Remove(json.Length - 1, 1);
        txt = json.ToString();
        txt = txt.TrimEnd('#');
        return txt;
    }

    private static string GetMillSeconds(DateTime lst)
    {
        DateTime t = new DateTime(1970, 1, 1, 8, 0, 0);
        string milliseconds = (lst - t).TotalMilliseconds.ToString();
        return milliseconds;
    }

    public static string GetShiK(string time,string siteids,string[] siteidArr)
    {
        Database db = new Database("CIMSS");
        string txt = "";
        string shiKSQL = "  SELECT vis,Station_Id_C as siteid,collect_time AS lst, Station_Name AS sitename FROM T_CIMISS_SURF_CHN_MUL_HOR WHERE collect_time between'{0}' and '{1}' AND Station_Id_C in ({2}) order by lst asc";
        DateTime maxTime, minTime;
        GetMaxMinTime(time, siteids, out maxTime, out minTime);
        shiKSQL = string.Format(shiKSQL, minTime, maxTime, siteids);
        DataTable tempDT = db.GetDataTable(shiKSQL);
        DataView dv = new DataView(tempDT);
        shiKDT = dv.ToTable(true);
        if (shiKDT != null && shiKDT.Rows.Count > 0)
        {
            txt = JoinJson(siteidArr, shiKDT,maxTime,minTime,"shiK");
        }
        return txt;
    }

    private static void GetMaxMinTime(string time, string siteids, out DateTime maxTime, out DateTime minTime)
    {
        string sqlTime = "SELECT max(lst) as maxLST,min(lst) as minLST from predicted_data_vis where forecastdate='" + time + "' and siteid in (" + siteids + ");";
        DataTable dtLST = m_Database.GetDataTableMySQL(sqlTime);
        maxTime =DateTime.Parse( dtLST.Rows[0][0].ToString());
        minTime = DateTime.Parse(dtLST.Rows[0][1].ToString());
    }

    [WebMethod]
    public static string GetTable() {
        StringBuilder sb = new StringBuilder();
        //表头
        sb.Append("<table style='width:100%;'><thead><tr>");
        sb.Append("<td>站点</td><td>平均偏差</td><td>均方根偏差</td>");
        sb.Append("</tr></thead>");
        //表内容
        sb.Append("<tbody>");
        GetTR(sb);
        sb.Append("</tbody></table>");
        return sb.ToString();
    }
    public static StringBuilder GetTR(StringBuilder sb) {
        foreach (string siteid in siteidArr)
        {
            sb.Append("<tr>");
            DataRow[] foreRows = foreDT.Select("siteid='" + siteid + "'");
            DataRow[] shiKRows = shiKDT.Select("siteid='" + siteid + "'");
            double RASE = 0,avgPiancha=0;
            if (shiKRows.Length > 0)
            {
                double temp = 0,sum=0,avg=0,piancha=0;
                foreach (DataRow foreRow in foreRows) {
                    sum+= float.Parse(foreRow["vis"].ToString());
                }
                avg = sum / foreRows.Length;
                for (int i = 0; i < shiKRows.Length; i++)  //均方根偏差要有实况才能算
                {
                    #region 计算均方根偏差
                    string foreV = foreRows[i]["vis"].ToString();
                    string shiKV = shiKRows[i]["vis"].ToString();
                    if (foreV == "" || shiKV == "")
                    {
                        continue;
                    }
                    shiKV = GetVisGrade(float.Parse(shiKV));
                    temp += Math.Pow(float.Parse(foreV) - float.Parse(shiKV), 2);
                    #endregion
                    #region  计算平均偏差
                    piancha +=Math.Abs(float.Parse(foreV) - avg);
                    #endregion
                }
                RASE = Math.Pow(temp /foreRows.Length,0.5);
                avgPiancha = piancha / foreRows.Length;
                sb.Append("<td>" +GetSite(siteid) + "</td>");
                sb.Append("<td>"+avgPiancha.ToString("f2") + "</td>");
                sb.Append("<td>" + RASE.ToString("f2") + "</td>");
                //sb.Append("<td>" + RASE.ToString("f2") + "</td>");
            }
            sb.Append("</tr>");
        }
        return sb;
    }
    public static string GetSite(string siteid) {
        string txt = "";
        DataRow [] dr = siteDT.Select("siteid='"+siteid+"'");
        txt = dr[0]["sitename"].ToString();
        return txt;
    }
    public static string GetVisGrade(float val)
    {
        string txt = "";
        if (val < 500)
        {
            txt = "4";
        }
        else if (val < 1000)
        {
            txt = "3";
        }
        else if (val < 3000)
        {
            txt = "2";
        }
        else if (val < 5000)
        {
            txt = "1";
        }
        else {
            txt = "0";
        }
        return txt;
    }
}