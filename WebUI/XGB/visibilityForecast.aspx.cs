using Readearth.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class XGB_visibilityForecast : System.Web.UI.Page
{
    public static Database s_Database;
    public static Database m_Database;
    public static Database m_Database2;

    protected void Page_Load(object sender, EventArgs e)
    {
        s_Database = new Database("siteInfo");
        m_Database = new Database("EleFore");
        m_Database2 = new Database("EleFore2");//EleFore2  王斌
    }

    [WebMethod]
    public static DataTable QuerySiteData()
    {
        string SQL = "select * from wea_site";
        DataTable dt = s_Database.GetDataTableMySQL(SQL);
        return dt;
    }
    /// <summary>
    /// 根据起始时间、时效，获取能见度等级的数据
    /// </summary>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <param name="codeId"></param>
    /// <param name="hours"></param>
    /// <returns></returns>
    [WebMethod]
    public static DataTable GetChartData(string startTime, string endTime, string codeId, string hours)
    {
        ////根据前台传过来的hours作为条件筛选起报日期到结束日期  之间的数据
        //var period = "";
        ////string a = DateTime.Parse(startTime).Hour.ToString();
        //if (hours == "24") {
        //    period = " a.period>=0&&a.period<=24 ";
        //   //// Convert.ToDateTime("");
        //   //DateTime dt = DateTime.ParseExact("","".)
        //}else if(hours=="48"){
        //    period = period = " a.period>24&&a.period<=48 ";
        //}else if(hours=="72"){
        //    period = period = " a.period>48&&a.period<=72 ";
        //}
        //if (startTime.Length >= 12) { } else {
        //    startTime = startTime + " 06:00:00";
        //}
        
        //string SQL = "select * from predicted_data_vis where forecastdate='2018-07-31 06:00:00' and siteID='58361' and lst BETWEEN '2018-07-31 06:00:00' and '2018-07-31 09:00:00'  order by lst asc";
        //string SQL = "select * from (select * from predicted_data_vis where forecastdate='" + startTime + "' and siteID='" + codeId + "' and lst BETWEEN '" + startTime + "' and '" + endTime + "'  order by lst asc) where lst BETWEEN '" + startTime + "' and '" + period + "'";
        string SQL = "SELECT * FROM (SELECT *,(FLOOR(TIMESTAMPDIFF(HOUR,forecastdate,lst)/24)*24)+24 AS 'period' FROM predicted_data_vis WHERE  siteId='" + codeId + "' AND lst BETWEEN '" + startTime + "' AND '" + endTime + "'  ) AS a WHERE  a.period=" + hours;
        DataTable dt = m_Database.GetDataTableMySQL(SQL);
        return dt;
    }

    /// <summary>
    /// 根据起始时间、时效，获取能见度值的数据
    /// </summary>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <param name="codeId"></param>
    /// <param name="hours"></param>
    /// <returns></returns>
    [WebMethod]
    public static DataTable GetVisValueData(string startTime, string endTime, string codeId, string hours)
    {
      
        string SQL = "SELECT * FROM (SELECT *,(FLOOR(TIMESTAMPDIFF(HOUR,forecastdate,lst)/24)*24)+24 AS 'period' FROM predicted_data_vis_value WHERE  siteId='" + codeId + "' AND lst BETWEEN '" + startTime + "' AND '" + endTime + "'  ) AS a WHERE  a.period=" + hours;
        DataTable dt = m_Database.GetDataTableMySQL(SQL);
        return dt;
    }

    /// <summary>
    /// 根据起始时间、时效，获取能见度实况值的数据
    /// </summary>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <param name="codeId"></param>
    /// <param name="hours"></param>
    /// <returns></returns>
    [WebMethod]
    public static DataTable GetVisRealValueData(string startTime, string endTime, string codeId, string hours)
    {

        string SQL = "SELECT `DATETIME`,siteid,sitename,vis FROM " + codeId + "_real_data WHERE `DATETIME` BETWEEN '" + startTime + "' AND '" + endTime+"'";
        DataTable dt = m_Database2.GetDataTableMySQL(SQL);
        return dt;
    }

    
    //能见度等级表中，获取最新的起报时间
    [WebMethod]
    public static DataTable GetLastForecastTime() {
        string SQL = "select DISTINCT forecastdate from predicted_data_vis  order by forecastdate desc LIMIT 0,1";
        DataTable dt = m_Database.GetDataTableMySQL(SQL);
        return dt;
    }
    //能见度等级表中，获取最新lst时间(距离起报时间72小时后的日期)
    [WebMethod]
    public static DataTable GetLastLstTime()
    {
        string SQL = "select DISTINCT lst from predicted_data_vis  order by lst desc LIMIT 0,1";
        DataTable dt = m_Database.GetDataTableMySQL(SQL);
        return dt;
    }

    //----------王斌-------------------
    //更新生成由能见度等级算出来的表
    [WebMethod]
    public static DataTable CreateTable(string startTime, string endTime, string period)
    {

        //DataTable result = new DataTable();
        //string[] colName = { "siteid", "sitename", "avgPiancha", "jfcha", "relXS", "ts" };
        //foreach (string str in colName)
        //{
        //    result.Columns.Add(str, typeof(string));
        //}
        Database m_Database2 = new Database("EleFore2");
        string sqlSiteID = "SELECT siteid,sitename,'-' AS avgPiancha,'-' AS jfcha,'-' AS relXS,'-' AS ts FROM  predicted_data_vis_value GROUP BY sitename,siteid ORDER BY siteid DESC";
        DataTable DTSite = m_Database.GetDataTableMySQL(sqlSiteID);
        DataTable result = DTSite;
        string foreSql = "SELECT forecastdate,lst,(FLOOR(TIMESTAMPDIFF(HOUR,forecastdate,lst)/24)*24)+24 AS 'period',siteid,sitename,vishigh_value_predicted AS val FROM predicted_data_vis_value" +
            " WHERE forecastdate = '" + startTime + "' AND lst BETWEEN '" + startTime + "' AND '" + endTime + "'";
        string foreAVGSql = "SELECT AVG(a.vishigh_value_predicted) AS avg,siteid AS siteid,sitename FROM (SELECT *,(FLOOR(TIMESTAMPDIFF(HOUR,forecastdate,lst)/24)*24)+24 AS 'period' FROM predicted_data_vis_value" +
           " WHERE forecastdate = '" + startTime + "' ORDER BY siteid DESC ) AS a WHERE a.period = " + period + " GROUP BY a.siteid,a.sitename order by a.siteid desc";
        string realAvgSql = "", realSql = "";
        for (int i = 0; i < DTSite.Rows.Count; i++)
        {
            realSql += "(SELECT DATETIME,siteid,vis as val,sitename FROM " + DTSite.Rows[i]["siteId"] + "_real_data where DATETIME between '" + startTime + "' and '" + endTime + "') union all";
            realAvgSql += "(SELECT AVG(vis) AS 'avg','" + DTSite.Rows[i]["siteId"] + "' as siteid FROM " + DTSite.Rows[i]["siteId"] + "_real_data where DATETIME between '" + startTime + "' and '" + endTime + "') union all";
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
        if (DTFore != null && DTFore.Rows.Count > 0)
        {
            AvgPianCha(DTFore, DTForeAvg, result);
            CalRMSE(DTFore, DTReal, DTSite, result);
            PearsonCorrelation(DTFore, DTForeAvg, DTReal, DTRealAvg, result);
            CalTS(DTFore, DTReal, DTSite, result);
        }
        return result;
    }
    public static void AvgPianCha(DataTable foreDT, DataTable foreAvgDT, DataTable result)
    {
        for (int i = 0; i < foreAvgDT.Rows.Count; i++)
        {
            float piancha = 0;
            for (int j = 0; j < foreDT.Rows.Count; j++)
            {
                DataRow[] foreRow = foreDT.Select("siteid='" + foreAvgDT.Rows[i]["siteid"] + "'");
                for (int t = 0; t < foreRow.Length; t++)
                {
                    piancha += Math.Abs(float.Parse(foreRow[t]["val"].ToString()) - float.Parse(foreAvgDT.Rows[i]["avg"].ToString()));
                }
                piancha = (piancha / foreRow.Length);
            }
            //result.Rows.Add(foreAvgDT.Rows[i]["siteid"], foreAvgDT.Rows[i]["sitename"], piancha.ToString("f2"), "-", "-", "-");
            result.Rows[i]["avgPiancha"] = piancha.ToString("f2");
        }
    }
    //均方根误差
    public static void CalRMSE(DataTable foreDT, DataTable realDT, DataTable siteDT, DataTable result)
    {
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

    public static void PearsonCorrelation(DataTable foreDT, DataTable foreAvgDT, DataTable realDT, DataTable realAvgDT, DataTable result)
    {
        for (int i = 0; i < foreAvgDT.Rows.Count; i++)
        {
            string siteid = foreAvgDT.Rows[i]["siteid"].ToString();
            string filter = "siteid='" + siteid + "'";
            DataRow[] foreRow, realRow;
            ProForeRealRow(foreDT, realDT, siteid, filter, out foreRow, out realRow);
            double fenzi = 0, fenm = 0;
            double foreAvg = double.Parse(foreAvgDT.Select(filter)[0]["avg"].ToString());
            double realAvg = double.Parse(realAvgDT.Select(filter)[0]["avg"].ToString());
            if (realRow.Length > 0)
            {
                double vz = 0, vmx = 0, vmy = 0;
                for (int j = 0; j < realRow.Length; j++)
                {
                    double f = double.Parse(foreRow[j]["val"].ToString()) - foreAvg;
                    double r = double.Parse(realRow[j]["val"].ToString()) - double.Parse(realAvg.ToString());
                    vz += f * r;
                    vmx += Math.Pow(f, 2);
                    vmy += Math.Pow(r, 2);
                }
                fenzi = vz;
                fenm = Math.Pow(vmx, 0.5) * Math.Pow(vmy, 0.5);
                double rj = fenzi / fenm;
                result.Rows[i]["relXS"] = Math.Round(rj, 2);
            }
        }
    }
    public static void CalTS(DataTable foreDT, DataTable realDT, DataTable siteDT, DataTable result)
    {
        //string[] grade = { "0", "1", "2", "3", "4" };
        int num = 0;
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
                    string fGrade = GetVisGrade(f);
                    string rGrade = GetVisGrade(r);
                    if (rGrade == "0")
                    {
                        CalTs(ref NA, ref NC, fGrade, "0");
                    }
                    else if (rGrade == "1")
                    {
                        CalTs(ref NA, ref NC, fGrade, "1");
                    }
                    else if (rGrade == "2")
                    {
                        CalTs(ref NA, ref NC, fGrade, "2");
                    }
                    else if (rGrade == "3")
                    {
                        CalTs(ref NA, ref NC, fGrade, "3");
                    }
                    else if (rGrade == "4")
                    {
                        CalTs(ref NA, ref NC, fGrade, "4");
                    }
                    else
                    {
                        NB++;
                    }
                }
                // }
                TS += NA / (NA + NB + NC);
            }
            string strTS = (TS * 100).ToString("f2") + "%";
            result.Rows[num]["ts"] = strTS;
            num++;
        }
    }

    private static void CalTs(ref int NA, ref int NC, string fGrade, string grade)
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


    //能见度值表中，获取最新的起报时间
    [WebMethod]
    public static DataTable GetLastForecastTimeVisValue()
    {
        string SQL = "select DISTINCT forecastdate from predicted_data_vis_value  order by forecastdate desc LIMIT 0,1";
        DataTable dt = m_Database.GetDataTableMySQL(SQL);
        return dt;
    }
    //能见度值表中，获取最新lst时间(距离起报时间72小时后的日期)
    [WebMethod]
    public static DataTable GetLastLstTimeVisValue()
    {
        string SQL = "select DISTINCT lst from predicted_data_vis_value  order by lst desc LIMIT 0,1";
        DataTable dt = m_Database.GetDataTableMySQL(SQL);
        return dt;
    }
}