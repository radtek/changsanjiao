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
    public static Dictionary<string, float> standard;
    public static string globalPoll;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            m_Database = new Database("EleFore");
            standard = new Dictionary<string, float>();
            standard.Add("PM25", 75);
            standard.Add("NO2", 200);
            standard.Add("SO2", 500);
            standard.Add("O3", 200);
            standard.Add("PM10", 150);
            standard.Add("CO", 10);
        }
    }
    [WebMethod]
    public static string Getdate() {
        string maxTime = "",minTime="";
        string sql = "  SELECT MAX(lst) AS maxTime,MIN(lst) AS minTime FROM predicted_data_aqi WHERE forecastdate =( SELECT MAX(forecastdate) FROM predicted_data_aqi)";
        try
        {
            DataTable dt = m_Database.GetDataTableMySQL(sql);
            maxTime = dt.Rows[0][0].ToString();
            minTime = dt.Rows[0][1].ToString();
            maxTime = DateTime.Parse(maxTime).ToString("yyyy-MM-dd HH:00:00");
            minTime = DateTime.Parse(minTime).ToString("yyyy-MM-dd HH:00:00");
        } catch (Exception e) {
            maxTime = "error";
        }
        return maxTime+"&"+minTime;
    }
    [WebMethod]
    public static string GetChart(string pollName, string time, string sites, string endTime,string period)
    {
        //GetData("2018-08-23 06:00:00", "2018-08-26 19:00:00", "48");
        globalPoll = pollName;
        DataTable shiKDT = new DataTable(); 
        Database huankDB = new Database("HuanKe");
        string sqlSiteID = "select distinct(siteid) as siteid from predicted_data_aqi";
        DataTable dtSiteID = m_Database.GetDataTableMySQL(sqlSiteID);
        siteSelArr = dtSiteID.AsEnumerable().Select(t=>t.Field<string>("siteid")).ToArray();
        //siteSelArr = sites.Split(',');
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
            string sql = "SELECT " + field + " as val, lst,siteId from predicted_data_aqi WHERE forecastDate='"+time+"' and"+
                " lst between'" + time + "' and '"+endTime+"' and siteid in (" + siteids + ")";
            if (period != "0")
            {
                sql += " and pred_time='" + period + "'  order by lst asc;";
            }
            else {
                sql += "  order by lst asc;";
            }
            //string sql = "SELECT " + field + " as val, lst,siteId from predicted_data_aqi WHERE forecastTime ='" + time + " and siteid in (" + siteids + ")  order by lst asc;";  
            DataTable tempForeDT = m_Database.GetDataTableMySQL(sql);
            //站点取平均
            foreDT = CalDTAVG(tempForeDT);
            if (foreDT != null && foreDT.Rows.Count > 0)
            {
                //foreDT = m_Database.GetDataTableMySQL(sql);
                if (foreDT != null && foreDT.Rows.Count > 0)
                {
                    //DateTime maxTime, minTime;
                    // GetMaxMinTime(time, out maxTime, out minTime);
                    DateTime reStartTime = (DateTime)foreDT.Compute("Min(lst)", "true");
                    DateTime reEndTime =(DateTime)foreDT.Compute("Max(lst)", "true");
                    DateTime maxTime = DateTime.Parse(endTime);
                    DateTime minTime = DateTime.Parse(time);
                    string[] site = { "00000" };  //平均后的站点
                    txt = JoinJson(site, foreDT, reEndTime, reStartTime);
                   
                    txt = txt + "@" + GetShiK(reStartTime.ToString("yyyy-MM-dd HH:mm:ss"), pollName, siteSelArr, siteids, reEndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                // txt = json.ToString() + "&" + foreDT.Rows[0]["sitename"].ToString() + "#" + shiKjson.ToString();
            }
        }
        catch (Exception e) { txt = "error"; }
        //txt = json.ToString();
        return txt;
    }
    public static DataTable CalDTAVG(DataTable dt) { 
        DateTime maxTime = dt.AsEnumerable().Select(t => t.Field<DateTime>("lst")).Max();
        DateTime minTime = dt.AsEnumerable().Select(t => t.Field<DateTime>("lst")).Min();
        DataTable result = dt.Clone();
        for (DateTime time = minTime; time <= maxTime; time = time.AddHours(1)) {
            string filter = "lst='"+time+ "'";
            DataRow[] row = dt.Select(filter);
            float avg = 0;
            if (row.Length > 0)
            {
                avg = row.Select(t => t.Field<float>("val")).Average();
            }
            else {
                avg = 99999;
            }
            result.Rows.Add(avg, time,"00000");
        }
        return result;
    }
    public static string GetShiK(string time, string pollName, string[] siteSelArr,string siteids,string endTime)
    {
        Database huankDB = new Database("HuanKe");
        if (pollName == "O3") {
            pollName = "O31";
        }
        string txt = "";
        string shiKSQL = "SELECT cast({0} as float) as val,timePoint as lst,siteId as siteId FROM [dbo].[China_RT_CNEMC_Data] WHERE Area='上海市' AND siteid in (" + siteids + ") and Timepoint between '{1}' and '{2}' and {0} IS NOT NULL";
        //DateTime maxTime, minTime;
        //GetMaxMinTime(time, out maxTime, out minTime);
        DateTime maxTime = DateTime.Parse(endTime);
        DateTime minTime = DateTime.Parse(time);
        shiKSQL = string.Format(shiKSQL, pollName,minTime, maxTime);
        DataTable tempDT = huankDB.GetDataTable(shiKSQL);
        DataTable tem = tempDT.Clone();
        tem.Columns["val"].DataType = typeof(float);
        foreach (DataRow row in tempDT.Rows) {
            tem.Rows.Add(row.ItemArray);
        }
        DataView dv = new DataView(tem);
        DataTable  tempShiKDT = dv.ToTable(true);
        shiKDT = CalDTAVG(tempShiKDT);
        if (shiKDT != null && shiKDT.Rows.Count > 0)
        {
            string[] site = { "00000"};
            txt = JoinJson(site, shiKDT, maxTime, minTime);
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
                    val = val == "" ? "null" : (val=="99999"?"null":val);
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
        sb.Append("<td>站点</td><td>平均偏差</td><td>均方根偏差</td><td>TS评分</td><td>相关系数</td>");
        sb.Append("</tr></thead>");
        //表内容
        sb.Append("<tbody>");
        //GetTR(sb);
        #region  分站点的数据
        //foreach (string siteid in siteidArr)
        //{
        //    string con = "siteid='"+siteid+"'";
        //    GetTr(sb, siteid,con,con,foreDT,shiKDT);
        //}
        #endregion
        DataTable foreDTAvg = ForeDTSiteAvg(foreDT);
        DataTable shiKDTAvg = ForeDTSiteAvg(shiKDT);
        string allSiteCon = "siteid='00000'";
        GetTr(sb, "00000", allSiteCon, allSiteCon,foreDTAvg,shiKDTAvg);
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
            dr["siteid"] = "00000";  //表示求的所有站点的平均值
            result.Rows.Add(dr);
        }
        return result;
    }
    public static DataTable CalPerRelTS() {
        float foreAvg = foreDT.AsEnumerable().Where(x=>x.Field<float>("val")!=99999).Select(t => t.Field<float>("val")).Average();   //就一个平均过的站点，站点号为00000
        float shiKAvg = shiKDT.AsEnumerable().Where(x => x.Field<float>("val") != 99999).Select(t => t.Field<float>("val")).Average();
        DataTable shiKAvgDT = new DataTable();
        DataTable foreAvgDT = new DataTable();
        DataTable result = new DataTable();
        DataTable siteDT = new DataTable();
        siteDT.Columns.Add("siteid", typeof(string));
        shiKAvgDT.Columns.Add("avg", typeof(string));
        foreAvgDT.Columns.Add("avg", typeof(string));
        shiKAvgDT.Columns.Add("siteid", typeof(string));
        foreAvgDT.Columns.Add("siteid", typeof(string));
        result.Columns.Add("relXS", typeof(string));
        result.Columns.Add("ts", typeof(string));
        DataRow dr = result.NewRow();
        result.Rows.Add(dr);
        shiKAvgDT.Rows.Add(shiKAvg, "00000");
        foreAvgDT.Rows.Add(foreAvg, "00000");
        siteDT.Rows.Add("00000");
        DataTable shiKDTtemp = shiKDT.Copy();
        shiKDTtemp.Columns["lst"].ColumnName = "datetime";
        PearsonCorrelation(foreDT, foreAvgDT, shiKDTtemp, shiKAvgDT, result);
        CalTS(foreDT, shiKDTtemp, siteDT, result);
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
            DataTable result = CalPerRelTS();
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
                if (foreV == "" || shiKV == ""||foreV=="99999"|| shiKV == "99999")
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
            sb.Append("<td>"+result.Rows[0]["ts"]+"</td>");
            sb.Append("<td>" + result.Rows[0]["relXS"] + "</td>");
            // sb.Append("<td>" + RASE.ToString("f2") + "</td>");
        }
        sb.Append("</tr>");
    }
    public static string GetSite(string siteid) {
        string txt = "";
        switch (siteid) {
            case "1144A": txt = "徐汇上师大"; break;
            case "00000": txt = "全市"; break;
            case "1149A":txt = "浦东新区监测站";break;
        }
        return txt;
    }

    public static DataTable GetData(string startTime,string endTime,string period) {

        DataTable result = new DataTable();
        string [] colName = { "siteid","sitename","avgPiancha","jfcha","relXS","ts"};
        foreach (string str in colName) {
            result.Columns.Add(str, typeof(string));
        }
        Database m_Database2 = new Database("EleFore2");
        string sqlSiteID = "SELECT siteId,sitename FROM  predicted_data_vis_value GROUP BY sitename,siteid order by siteid desc";
        DataTable DTSite = m_Database.GetDataTableMySQL(sqlSiteID);
        string foreSql = "SELECT forecastdate,lst,(FLOOR(TIMESTAMPDIFF(HOUR,forecastdate,lst)/24)*24)+24 AS 'period',siteid,sitename,vishigh_value_predicted AS val FROM predicted_data_vis_value" +
            " WHERE forecastdate = '"+startTime+"' AND lst BETWEEN '"+startTime+"' AND '"+endTime+"'";
        string foreAVGSql = "SELECT AVG(a.vishigh_value_predicted) AS avg,siteid AS siteid,sitename FROM (SELECT *,(FLOOR(TIMESTAMPDIFF(HOUR,forecastdate,lst)/24)*24)+24 AS 'period' FROM predicted_data_vis_value" +
           " WHERE forecastdate = '" + startTime + "' ORDER BY siteid DESC ) AS a WHERE a.period = " + period + " GROUP BY a.siteid,a.sitename order by a.siteid desc";
        string realAvgSql = "", realSql = "";
        for (int i = 0; i < DTSite.Rows.Count; i++) {
            realSql += "(SELECT DATETIME,siteid,vis as val,sitename FROM "+DTSite.Rows[i]["siteId"] + "_real_data where DATETIME between '"+startTime+"' and '"+endTime+"') union all";
            realAvgSql += "(SELECT AVG(vis) AS 'avg','"+ DTSite.Rows[i]["siteId"] + "' as siteid FROM " + DTSite.Rows[i]["siteId"] + "_real_data where DATETIME between '" + startTime + "' and '" + endTime + "') union all";
        }
        realSql = realSql.TrimEnd("union all".ToArray());
        realAvgSql = realAvgSql.TrimEnd("union all".ToArray());
        DataTable DTReal = m_Database2.GetDataTableMySQL(realSql);
        DataTable DTRealAvg = m_Database2.GetDataTableMySQL(realAvgSql);
        DataTable DTForeTemp = m_Database.GetDataTableMySQL(foreSql);
        DataView dv = DTForeTemp.DefaultView;
        dv.RowFilter = "period='" + period + "'";
        DataTable DTFore = dv.ToTable();
        DataTable DTForeAvg = m_Database.GetDataTableMySQL(foreAVGSql);
        AvgPianCha(DTFore, DTForeAvg, result);
        CalRMSE(DTFore, DTReal, DTSite, result);
        PearsonCorrelation(DTFore, DTForeAvg, DTReal, DTRealAvg, result);
        CalTS(DTFore, DTReal, DTSite, result);
        return result;
    }
    public static void AvgPianCha(DataTable foreDT,DataTable foreAvgDT, DataTable result) {
        for (int i = 0; i < foreAvgDT.Rows.Count; i++) {
            float piancha = 0;
            for (int j = 0; j < foreDT.Rows.Count; j++) {
                DataRow[] foreRow = foreDT.Select("siteid='"+foreAvgDT.Rows[i]["siteid"] +"'");
                for (int t = 0; t < foreRow.Length; t++) {
                    piancha += Math.Abs(float.Parse(foreRow[t]["val"].ToString())-float.Parse(foreAvgDT.Rows[i]["avg"].ToString()));
                }
                piancha = (piancha / foreRow.Length);
            }
            result.Rows.Add(foreAvgDT.Rows[i]["siteid"], foreAvgDT.Rows[i]["sitename"],piancha.ToString("f2"),"-","-","-");
        }
    }
    //均方根误差
    public static void CalRMSE(DataTable foreDT,DataTable realDT,DataTable siteDT, DataTable result) {
        for (int i = 0; i < siteDT.Rows.Count; i++)
        {
            string siteid = siteDT.Rows[i]["siteId"].ToString();
            string filter = "siteid='" + siteid + "'";

            DataRow[] foreRow, realRow;
            ProForeRealRow(foreDT, realDT, siteid, filter, out foreRow, out realRow);
            if (realRow.Length > 0)
            {
                double v = 0;
                for (int j = 0; j < realRow.Length; j++)
                {
                    float realV = float.Parse(realRow[j]["val"].ToString());
                    v += Math.Pow(realV - float.Parse(foreRow[j]["val"].ToString()), 2);
                }
                v = v / realRow.Length;
                v = Math.Pow(v,0.5);
                result.Rows[i]["jfcha"] = v.ToString("f2");
            }
        }
    }

    private static void ProForeRealRow(DataTable foreDT, DataTable realDT, string siteid, string filter, out DataRow[] foreRow, out DataRow[] realRow)
    {
        foreRow = foreDT.Select(filter);
        DateTime maxTime = foreRow.AsEnumerable().Select(t => t.Field<DateTime>("lst")).Max();
        DateTime minTime = foreRow.AsEnumerable().Select(t => t.Field<DateTime>("lst")).Min();
        realRow = realDT.Select("siteid='" + siteid + "' and datetime>='" + minTime + "' and datetime<='" + maxTime + "'");
    }

    public static void PearsonCorrelation(DataTable foreDT,DataTable foreAvgDT,DataTable realDT,DataTable realAvgDT,DataTable result) {
        for (int i = 0; i < foreAvgDT.Rows.Count; i++) {
            string siteid = foreAvgDT.Rows[i]["siteid"].ToString();
            string filter = "siteid='"+siteid+"'";
            DataRow[] foreRow, realRow;
            ProForeRealRow(foreDT,realDT,siteid,filter, out foreRow,out realRow);
            double fenzi =0,fenm=0;
            double foreAvg = double.Parse(foreAvgDT.Select(filter)[0]["avg"].ToString());
            double realAvg = double.Parse(realAvgDT.Select(filter)[0]["avg"].ToString());
            if (realRow.Length > 0)
            {
                double vz = 0,vmx=0,vmy=0;
                for (int j = 0; j < realRow.Length; j++)
                {
                    double f = double.Parse(foreRow[j]["val"].ToString()) - foreAvg;
                    double r = double.Parse(realRow[j]["val"].ToString()) - double.Parse(realAvg.ToString());
                    vz += f*r;
                    vmx += Math.Pow(f, 2);
                    vmy += Math.Pow(r,2);
                }
                fenzi = vz;
                fenm = Math.Pow(vmx, 0.5)* Math.Pow(vmy, 0.5);
                double rj = fenzi / fenm;
                result.Rows[i]["relXS"] = Math.Round(rj, 2);
            }
        }
    }
    public static void CalTS(DataTable foreDT, DataTable realDT, DataTable siteDT,DataTable result)
    {
        //string[] grade = { "0", "1", "2", "3", "4" };
        int num = 0;
        float standartVal = standard[globalPoll];
        foreach (DataRow siteRow in siteDT.Rows)
        {
            float TS = 0;
            int NA = 0, NB = 0, NC = 0;
            //foreach (string g in grade)
            //{
                string siteid = siteRow["siteid"].ToString();
                DataRow[] foreRow, realRow;
                string filter = "siteid='" + siteid + "'";
                ProForeRealRow(foreDT, realDT, siteid, filter, out foreRow, out realRow);
            if (realRow.Length > 0 && foreRow.Length > 0)
            {
                for (int i = 0; i < realRow.Length; i++)
                {
                    float f = float.Parse(foreRow[i]["val"].ToString());
                    float r = float.Parse(realRow[i]["val"].ToString());
                    if (f > standartVal && r > standartVal)
                    {
                        NA++;
                    }
                    else if (r <= standartVal && f > standartVal)
                    {
                        NB++;
                    }
                    else if (r > standartVal && f <= standartVal)
                    {
                        NC++;
                    }
                }
                // }
                if (NA + NB + NC > 0)
                {
                    TS +=(float) NA / (NA + NB + NC);
                }
            }
            string strTS = (TS * 100).ToString("f2") + "%";
            if (TS == 0) {
                strTS = "0";
            }
            result.Rows[num]["ts"] = strTS;
            num++;
        }
    }

    private static void CalTs(ref int NA, ref int NC, string fGrade,string grade)
    {
        if (fGrade == grade)
        {
            NA++;
        }
        else if (fGrade != grade)
        {
            NC++;
        }
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
        else
        {
            txt = "0";
        }
        return txt;
    }
}