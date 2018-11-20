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
    public static DataTable shiKDT = new DataTable();
    public static DataTable foreDT = new DataTable();
    public static string[] siteidArr= { "1144A", "1149A" };
    public static string[] siteSelArr;
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
    public static string GetChart(string pollName,string time,string sites) {
        DataTable shiKDT = new DataTable(); 
         Database huankDB = new Database("HuanKe");
        siteSelArr = sites.Split(',');
        string siteids = "";
        foreach (var str in siteSelArr)
        {
            siteids += "'" + str + "',";
        }
        siteids = siteids.TrimEnd(',');
        string txt = "";
        try
        {
            string field = GetPollField(pollName);
            string sql = "SELECT " + field + " as val, lst,siteId from predicted_data_aqi WHERE forecastdate ='" + time + "' and siteid in (" + siteids + ")  order by lst asc;";  
            foreDT = m_Database.GetDataTableMySQL(sql);
            if (foreDT != null && foreDT.Rows.Count > 0)
            {
                foreDT = m_Database.GetDataTableMySQL(sql);
                if (foreDT != null && foreDT.Rows.Count > 0)
                {
                    DateTime maxTime, minTime;
                    GetMaxMinTime(time, out maxTime, out minTime);
                    txt = JoinJson(siteSelArr, foreDT, maxTime, minTime);
                    txt = txt + "@" + GetShiK(time, pollName, siteSelArr, siteids);
                }
                // txt = json.ToString() + "&" + foreDT.Rows[0]["sitename"].ToString() + "#" + shiKjson.ToString();
            }
        }
        catch (Exception e) { txt = "error"; }
        //txt = json.ToString();
        return txt;
    }
    public static string GetShiK(string time, string pollName, string[] siteSelArr,string siteids)
    {
        Database huankDB = new Database("HuanKe");
        if (pollName == "O3") {
            pollName = "O31";
        }
        string txt = "";
        string shiKSQL = "SELECT cast({0} as float) as val,siteId as siteId,timePoint as lst FROM [dbo].[China_RT_CNEMC_Data] WHERE Area='上海市' AND siteid in (" + siteids + ") and Timepoint between '{1}' and '{2}'";
        DateTime maxTime, minTime;
        GetMaxMinTime(time, out maxTime, out minTime);
        shiKSQL = string.Format(shiKSQL, pollName,minTime.AddHours(-8), maxTime.AddHours(-8));
        DataTable tempDT = huankDB.GetDataTable(shiKSQL);
        DataTable tem = tempDT.Clone();
        tem.Columns["val"].DataType = typeof(float);
        foreach (DataRow row in tempDT.Rows) {
            tem.Rows.Add(row.ItemArray);
        }
        DataView dv = new DataView(tem);
        shiKDT = dv.ToTable(true);
        if (shiKDT != null && shiKDT.Rows.Count > 0)
        {
            txt = JoinJson(siteSelArr, shiKDT, maxTime, minTime);
        }
        return txt;
    }
    private static string JoinJson(string[] siteSelArr, DataTable dt, DateTime maxTime, DateTime minTime)
    {
        string txt = "";
        StringBuilder json = new StringBuilder();
        json.Append("[");
        foreach (var str in siteSelArr)
        {

            for (DateTime time = minTime; time <= maxTime; time = time.AddHours(1))
            {
                DataRow[] row = dt.Select("siteId='" + str + "' and lst='" + time + "'");
                string milliseconds = GetMillSeconds(time);
                if (row.Length > 0)
                {
                    string val = row[0]["val"].ToString();
                    val = val == "" ? "null" : val;
                    json.Append("[" + milliseconds + "," + val + "],");
                }
                else
                {
                    json.Append("[" + milliseconds + ",null],");
                }
            }
            json.Remove(json.Length - 1, 1);
            json.Append("]&" + str + "#[");
            //txt += json.ToString() + "&" + str + "#";

        }
        json.Remove(json.Length - 1, 1);
        txt = json.ToString();
        txt = txt.TrimEnd('#');
        return txt;
    }
    private static void GetMaxMinTime(string time,out DateTime maxTime,out DateTime minTime)
    {
        string sqlTime = "SELECT max(lst) as maxTime,min(lst) as minTime from predicted_data_aqi WHERE forecastdate ='" + time + "';";
        DataTable dtTime = m_Database.GetDataTableMySQL(sqlTime);
        maxTime =DateTime.Parse(dtTime.Rows[0]["maxTime"].ToString());
        minTime = DateTime.Parse(dtTime.Rows[0]["minTime"].ToString());
    }

    private static string GetMillSeconds(DateTime lst)
    {
        DateTime t = new DateTime(1970, 1, 1, 8, 0, 0);
        string milliseconds = (lst - t).TotalMilliseconds.ToString();
        return milliseconds;
    }
   
    public static string GetPollField(string pollName) {
        string field = "";
        switch (pollName) {
            case "PM25":
                field = "pm25_predicted";
                break;
            case "PM10":
                field = "pm10_predicted";
                break;
            case "SO2":
                field = "so2_predicted";
                break;
            case "O3":
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
    [WebMethod]
    public static string GetTable()
    {
        StringBuilder sb = new StringBuilder();
        //表头
        sb.Append("<table style='width:100%;'><thead><tr>");
        sb.Append("<td>站点</td><td>平均偏差</td><td>均方根偏差</td>");
        sb.Append("</tr></thead>");
        //表内容
        sb.Append("<tbody>");
        //GetTR(sb);
        foreach (string siteid in siteidArr)
        {
            string con = "siteid='"+siteid+"'";
            GetTr(sb, siteid,con,con,foreDT,shiKDT);
        }
        DataTable foreDTAvg = ForeDTSiteAvg(foreDT);
        DataTable shiKDTAvg = ForeDTSiteAvg(shiKDT);
        string allSiteCon = "siteid='0000'";
        GetTr(sb, "0000", allSiteCon, allSiteCon,foreDTAvg,shiKDTAvg);
        sb.Append("</tbody></table>");
        return sb.ToString();
    }
    public static DataTable ForeDTSiteAvg(DataTable dt) {
        //dt.Columns["val"].DataType=
        DataTable result = HealthyWeather_AirEleFore.foreDT.Clone();
        DateTime minTime= foreDT.AsEnumerable().Select(t => t.Field<DateTime>("lst")).Min();
        DateTime maxTime = shiKDT.AsEnumerable().Select(t => t.Field<DateTime>("lst")).Max();
        for (DateTime lst = minTime; lst <= maxTime; lst = lst.AddHours(1)) {
            DataRow[] rows = dt.Select("lst='"+ lst + "'");
            bool flag = false;
            foreach (DataRow r in rows) {
                if (r.ItemArray[0].ToString() == "") {
                    flag = true;
                }
            }
            if (flag) {
                continue;
            }
            float val = rows.AsEnumerable().Select(t => t.Field<float>("val")).Average();
            DataRow dr = result.NewRow();
            dr["lst"] = lst;
            dr["val"] = val;
            dr["siteid"] = "0000";  //表示求的所有站点的平均值
            result.Rows.Add(dr);
        }
        return result;
    }
    private static void GetTr(StringBuilder sb, string siteid,string foreCondition,string shiKCondition,DataTable foreDT,DataTable shiKDT)
    {
        sb.Append("<tr>");
        DataRow[] foreRows = foreDT.Select(foreCondition);
        DataRow[] shiKRows = shiKDT.Select(shiKCondition);
        double RASE = 0, avgPiancha = 0;
        if (shiKRows.Length > 0)
        {
            double temp = 0, sum = 0, avg = 0, piancha = 0;
            foreach (DataRow foreRow in foreRows)
            {
                sum += float.Parse(foreRow["val"].ToString());
            }
            avg = sum / foreRows.Length;
            for (int i = 0; i < shiKRows.Length; i++)  //均方根偏差要有实况才能算
            {
                #region 计算均方根偏差
                string foreV = foreRows[i]["val"].ToString();
                string shiKV = shiKRows[i]["val"].ToString();
                if (foreV == "" || shiKV == "")
                {
                    continue;
                }
                temp += Math.Pow(float.Parse(foreV) - float.Parse(shiKV), 2);
                #endregion
                #region  计算平均偏差
                piancha += Math.Abs(float.Parse(foreV) - avg);
                #endregion
            }
            RASE = Math.Pow(temp / foreRows.Length, 0.5);
            avgPiancha = piancha / foreRows.Length;
            sb.Append("<td>" + GetSite(siteid) + "</td>");
            sb.Append("<td>" + avgPiancha.ToString("f2") + "</td>");
            sb.Append("<td>" + RASE.ToString("f2") + "</td>");
            // sb.Append("<td>" + RASE.ToString("f2") + "</td>");
        }
        sb.Append("</tr>");
    }
    public static string GetSite(string siteid) {
        string txt = "";
        switch (siteid) {
            case "1144A": txt = "徐汇上师大"; break;
            case "0000": txt = "全市"; break;
            case "1149A":txt = "浦东新区监测站";break;
        }
        return txt;
    }
}