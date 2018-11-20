
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Readearth.Data;
using Readearth.Data.Entity;
using MMShareBLL.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ChinaAQI;
using Lucas.AQI2012;
using MASlib;
using Lucas;
using System.Net;
using Readearth.Common;
using WeiBo;
using Aspose.Cells;
using System.IO;
using System.Text.RegularExpressions;
using MMShareBLL.DAL;
using Aspose.Words;
using Aspose.Words.Tables;
using System.Web;
using System.Drawing;
using System.Security.Cryptography;
using Svg;
using Svg.Transforms;

namespace MMShareBLL.DAL
{
    public class HealthyWeatherGW
    {

        //用于记录系统错误日志
        protected static readonly log4net.ILog m_Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Database m_Database;
        private int m_BackDays;
        DataSetForcast.T_HealthyWeatherDataTable t_healthyweather = new DataSetForcast.T_HealthyWeatherDataTable();
        #region sqls
        string insert_sql = "INSERT INTO [T_HealthyWeather] " +
                               "([ForecastDate]" +
                               ",[Period]" +
                               ",[Station]" +
                               ",[LST]" +
                               ",[Level]" +
                               ",[People]" +
                               ",[premunition]" +
                               ",[Type],[forecaster]) VALUES" +
                               "('{0}'" +
                               ",'{1}'" +
                               ",'{2}'" +
                               ",'{3}'" +
                               ",'{4}'" +
                               ",'{5}'" +
                               ",'{6}'" +
                               ",'{7}','{8}')";
        string del_sql = "delete from T_HealthyWeather where Period='{0}' and  Type in ('{1}','{2}','{3}') and CONVERT(varchar(10), ForecastDate, 120)='{4}' and forecaster='{5}'  ";
        string del_sqlNew = "delete from T_HealthyWeather where Period='{0}' and  Type in ('{1}') and CONVERT(varchar(10), ForecastDate, 120)='{2}' and forecaster='{3}'  ";
        string del_sqlII = "delete from T_HealthyWeather where Period='{0}' and  Type in ('{1}') and CONVERT(varchar(10), ForecastDate, 120)='{2}' and forecaster='{3}' and  CONVERT(varchar(10), LST, 120)='{4}'  ";
        string query_sql = "select * from T_HealthyWeather where Period='{0}' and  Type in ('{1}') and CONVERT(varchar(10), ForecastDate, 120)='{2}' and forecaster='{3}' order by lst asc ";
        string query_forecast_sql = "SELECT * FROM (SELECT T1.ForecastDate,T1.station,T1.PERIOD,T1.TGrade,T1.Type,T2.GuideLines1,T2.GuideLines2" +
                                    " FROM T_HealthyGrade T1 LEFT join " +
                                    " T_HealthyForecast T2 on T1.ForecastDate=T2.ForecastDate " +
                                    " AND T1.station=T2.station and T1.Type=T2.Type and  T1.PERIOD=T2.PERIOD  " +
                                    " where T1.ForecastDate='{0}' AND t1.Type='{1}' ) X " +
                                    " LEFT join T_AffectGroup T4 on X.Type=Replace(Replace(T4.Type,'青少年和成年人感冒','成人感冒'),'老年人感冒','老人感冒') and  X.TGrade=T4.Grade";
        #endregion

        public HealthyWeatherGW()
        {
            //string connection = "Data Source=172.21.107.31;Initial Catalog=ACIS;Persist Security Info=True;User ID=sa;Password=Passw0rd";
            m_Database = new Database("DBCONFIGGW");
            m_BackDays = int.Parse(ConfigurationManager.AppSettings["BackDays"]);
        }

        public string QueryHealthyWeather(string priod, string type, string forecastTime, string user)
        {
            Database m_DatabaseII = new Database("DBCONFIGII");
            StringBuilder strBuilder = new StringBuilder();
            try
            {
                string sql = string.Format(query_sql, priod, type,
                                                      DateTime.Parse(forecastTime).
                                                      ToString("yyyy-MM-dd"), user);
                DataTable dt = m_DatabaseII.GetDataTable(sql);

                //xuehui 2017-12-06 如果查不到就查本单位做的数据 
                if (dt == null || dt.Rows.Count <= 0) {
                    //查询user的单位
                    string sql_user=" select * from T_user where Alias='"+user+"'";
                    DataTable dt_user= m_DatabaseII.GetDataTable(sql_user);
                    string companyName="";
                    if (dt_user != null && dt_user.Rows.Count > 0) {
                        companyName = dt_user.Rows[0]["WindowsUser"].ToString();
                        string sql_query = " select t1.* from T_HealthyWeather t1 left join T_User t2  on t1.forecaster=t2.Alias " +
                                           " where  t1.Period='" + priod + "' and  t1.Type in ('"+type+"') and CONVERT(varchar(10), " +
                                           " t1.ForecastDate, 120)='" + DateTime.Parse(forecastTime).ToString("yyyy-MM-dd") 
                                           + "' and t2.WindowsUser='" + companyName + "' order by lst asc";
                        dt = m_DatabaseII.GetDataTable(sql_query);
                    }
                }


                if (dt != null && dt.Rows.Count > 0)
                {
                    string lv = "";
                    string ple = "";
                    string pre = "";
                    string lst = "";
                    string sites = "";
                    string disTime = dt.Rows[0]["createTime"].ToString();// xuehui 2017-12-06 
                    foreach (DataRow row in dt.Rows)
                    {
                      
                        string lenvel = row["level"].ToString();
                        lv += (lenvel + ",");
                        string people = row["people"].ToString().Replace(",", "，");
                        ple += (people + ",");
                        string premunition = row["premunition"].ToString().Replace(",", "，"); ;
                        pre += (premunition + ",");
                        string lsts = DateTime.Parse(row["LST"].ToString()).ToString("MM月dd日"); //*****************这个前台改了，这个也要改
                        lst += (lsts + ",");
                        string site = row["Station"].ToString();
                        sites += (site + ",");

                    }
                    strBuilder.Append(lv + "*" + ple + "*" + pre + "*" + lst + "*" + sites + "@" + disTime);
                }
            }
            catch { }
            return strBuilder.ToString();
        }

        private string GetSiteName(string site)
        {
            string siteName = "";
            switch (site)
            {
                case "58367": siteName = "中心城区"; break;
                case "58370": siteName = "浦东新区"; break;
                case "58361": siteName = "闵行区"; break;
                case "58362": siteName = "宝山区"; break;
                case "58462": siteName = "松江区"; break;
                case "58460": siteName = "金山区"; break;
                case "58461": siteName = "青浦区"; break;
                case "58463": siteName = "奉贤区"; break;
                case "58365": siteName = "嘉定区"; break;
                case "58366": siteName = "崇明"; break;

            }
            return siteName;
        }

        private string GetLevelName(string level)
        {
            string levelName = "";
            switch (level)
            {
                case "1": levelName = "低"; break;
                case "2": levelName = "轻微"; break;
                case "3": levelName = "中等"; break;
                case "4": levelName = "较高"; break;
                case "5": levelName = "高"; break;

            }
            return levelName;
        }

        private string GetLevelNameII(string level)
        {
            string levelName = "";
            switch (level)
            {
                case "0": levelName = "不易中暑"; break;
                case "1": levelName = "不易中暑"; break;
                case "2": levelName = "可能中暑"; break;
                case "3": levelName = "较易中暑"; break;
                case "4": levelName = "容易中暑"; break;
                case "5": levelName = "极易中暑"; break;

            }
            return levelName;
        }

        /// <summary>
        /// 扩展保存
        /// </summary>
        /// <param name="texts"></param>
        /// <param name="priod"></param>
        /// <param name="type"></param>
        /// <param name="forecastTime"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public string SaveHealthyWeatherII(string texts, string priod, string type, string forecastTime, string user)
        {
            Database m_DatabaseII = new Database("DBCONFIGII");
            string status = "false";
            string[] vs = texts.Split('@');
            int count = (vs.Length - 1) / 40;
            DataSet ds = new DataSet();
            for (int i = 0; i < count; i++)
            {
                DataTable dt = t_healthyweather.Clone();
                dt.TableName = i.ToString();
                for (int j = 0; j < 10; j++)
                {
                    DataRow row = dt.NewRow();
                    row["area"] = vs[(i * 10) + j];
                    row["level"] = vs[(i * 10) + 20 + j];
                    row["people"] = vs[(i * 10) + 40 + j];
                    row["premunition"] = vs[(i * 10) + 60 + j];
                    #region
                    if (i % 2 == 1)
                    {
                        if (priod == "10")
                            row["LST"] = DateTime.Parse(forecastTime).AddDays(1).ToString("yyyy-MM-dd");
                        else
                            row["LST"] = DateTime.Parse(forecastTime).AddDays(2).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        if (priod == "17")
                            row["LST"] = DateTime.Parse(forecastTime).AddDays(1).ToString("yyyy-MM-dd");
                        else
                            row["LST"] = DateTime.Parse(forecastTime).AddDays(0).ToString("yyyy-MM-dd");
                    }
                    #endregion
                    row["type"] = type;
                    dt.Rows.Add(row);
                }
                ds.Tables.Add(dt.Copy());
            }
            //先删除
            m_DatabaseII.Execute(string.Format(del_sqlNew, priod, type,
                                              DateTime.Parse(forecastTime).ToString("yyyy-MM-dd"), user));
            try
            {
                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string area = row["area"].ToString();
                        string level = row["level"].ToString();
                        string people = row["people"].ToString();
                        string premunition = row["premunition"].ToString();
                        string lst = row["LST"].ToString();
                        string types = row["type"].ToString();
                        string sql = string.Format(insert_sql, forecastTime, priod, area, lst, level, people, premunition, types, user);
                        m_DatabaseII.Execute(sql);
                    }
                }
                return "true";
            }
            catch { }
            return status;

        }

        private string GetNAME(string py)
        {
            string Name = "";
            switch (py.Replace("I", ""))
            {
                case "PuDong": Name = "浦东新区"; break;
                case "XuHui": Name = "中心城区"; break;
                case "MinHang": Name = "闵行区"; break;
                case "BaoShanArea": Name = "宝山区"; break;
                case "SongJiang": Name = "松江区"; break;
                case "JinShan": Name = "金山区"; break;
                case "QingPu": Name = "青浦区"; break;
                case "FengXian": Name = "奉贤区"; break;
                case "JiaDing": Name = "嘉定区"; break;
                case "ChongMing": Name = "崇明县"; break;
            }
            return Name;
        }

        public string ConvertDateTimeInt(System.DateTime time)
        {

            //double intResult = 0;
            //System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0));
            //intResult = (time - startTime).TotalSeconds;
            //return intResult.ToString();


            double intResult = 0;
            System.DateTime startTime = new System.DateTime(1970, 1, 1, 0, 0, 0);
            intResult = (time - startTime).TotalSeconds;
            return intResult.ToString();

        }

        public string GetChartElements(string fromDate, string toDate, string eName, string type, string period, string duration)
        {
            string strReturn = "";
            string x = "";
            string y = "";
            string val = "DustMassCon";
            string from = DateTime.Parse(fromDate).ToString("yyyy-MM-dd 00:00:00");
            string to = DateTime.Parse(toDate).ToString("yyyy-MM-dd 23:59:59");

  
                #region
                string e1 = "";
                string maxValue = ""+val+"<=500";
            if (eName == "PM25")
            {
                e1 = "2";
            }
            else if (eName == "PM10")
            {
                e1 = "3";
            }
            else if (eName == "PM1")
            {
                e1 = "4";
            }
            #endregion

            string where = " where (DateTime between '" + from + "' and '" + to + "' ) " +
                         " and stationID in ('58367','58362','58460','58366','58370','58363','99114','99116','99115','99119','99118','99110','99989','58365','58463')";
            string strSQL = " SELECT DATEDIFF(S,'1970-01-01 00:00:00',CONVERT(CHAR(16),[DateTIME], 120)) AS [END], " +
                          " "+val+" as 'value',[DateTime],stationID as 'Site' from tbDustS ";

            if (eName == "O3")   //o3查询表tbO3
            {
                val = "o3";
                maxValue = ""+val+">=0 and "+val+"<=250";
                where += "and " + maxValue + "";
                strSQL = " SELECT DATEDIFF(S,'1970-01-01 00:00:00',CONVERT(CHAR(16),[DateTIME], 120)) AS [END], "+val+" as 'value',[DateTime],stationID as 'Site' FROM dbo.tbO3" + where + "  ORDER BY datetime asc";
            }
            else {
                where += " and " + maxValue + " and DeviceType='" + e1 + "'";
                strSQL = strSQL + where + "  ORDER BY datetime asc";
            }
            string[] SitesArray = { "58367", "58362", "58460", "58366", "58370",
                                    "58363", "99114", "99116", "99115", "99119", "99118", "99110", "99989" ,"58365","58463"};
            //先全部查询出来
            Database m_Databases = new Database("DBCONFIGGW");
            DataTable dst = m_Databases.GetDataTable(strSQL);

            string strReturns = "";
            try
            {
                #region
                for (int i = 0; i < SitesArray.Length; i++){
                    DataTable dtElementNew = dst.Clone();
                    DataRow[] rows;
                    rows = dst.Select("Site='" + SitesArray[i] + "'");
                    if (SitesArray[i]== "58363" && eName == "PM25") { //东滩pm25取另一张表
                        DataTable dt_d_pm25 = GettbDust(from,to);
                        rows = dt_d_pm25.Select("Site='" + SitesArray[i] + "'");
                    }
                    #region
                    if (rows != null && rows.Length > 0)
                    {
                        foreach (DataRow row in rows)
                        {
                            dtElementNew.Rows.Add(row.ItemArray);
                        }
                    }
                    #endregion
                    x = ""; y = "";
                    //这里补数据
                    if (dtElementNew == null || dtElementNew.Rows.Count <= 0)
                    {
                        strReturns += "{}&";
                        continue;
                    }

                    DateTime bt = DateTime.Parse(dtElementNew.Rows[0][2].ToString());
                    DateTime endt = DateTime.Parse(dtElementNew.Rows
                        [dtElementNew.Rows.Count - 1][2].ToString());
                    #region
                    for (DateTime dt = bt; dt <= endt; dt = dt.AddMinutes(1))
                    {
                        bool bl = false;
                        foreach (DataRow dr in dtElementNew.Rows)
                        {
                            if (DateTime.Parse(dr[2].ToString()).
                                ToString("yyyy-MM-dd HH:mm:00")
                                == dt.ToString("yyyy-MM-dd HH:mm:00"))
                            {
                                x = x + "|" + dr[0].ToString();
                                y = y + "|" + dr[1].ToString();
                                bl = true;
                                break;
                            }
                        }
                        if (!bl)
                        {
                            x = x + "|" + ConvertDateTimeInt(dt);
                            y = y + "|" + "NULL";
                        }
                    }
                    #endregion
                    strReturn = strReturn + ",'0':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "'";
                    if (strReturn != ",")
                        strReturns += "{" + strReturn.TrimStart(',') + "}&";
                }
                #endregion
            
                if (strReturns != "&")
                    strReturns = strReturns.TrimEnd('&');
                return strReturns;
            }
            catch (Exception ex)
            {
                m_Log.Error("GetAQIChart", ex);
                return ex.ToString();
            }
        }

        public string GetWarnLevel(string Level)
        {
            string warnLevel = Level;
            switch (Level)
            {
                case "低": warnLevel = "1"; break;
                case "轻微": warnLevel = "2"; break;
                case "中等": warnLevel = "3"; break;
                case "较高": warnLevel = "4"; break;
                case "高": warnLevel = "5"; break;
                case "不易中暑": warnLevel = "1"; break;
                case "可能中暑": warnLevel = "2"; break;
                case "较易中暑": warnLevel = "3"; break;
                case "容易中暑": warnLevel = "4"; break;
                case "极易中暑": warnLevel = "5"; break;
            }
            return warnLevel;
        }
        public DataTable GettbDust(string form,string to) {
            Database m_Databases = new Database("DBCONFIGGW");
            string sql = "SELECT DATEDIFF(S,'1970-01-01 00:00:00',CONVERT(CHAR(16),[DateTIME], 120)) AS [END],  PM2_5 as 'value',[DateTime],stationID as 'Site' from tbDust "+
                  "where(DateTime between '"+form+"' and '"+to+"') AND PM2_5<= 500"+
                  "AND stationID ='58363' ORDER BY datetime asc; ";
            DataTable dt = m_Databases.GetDataTable(sql);
            return dt;
        }
        /// <summary>
        /// 查询等级
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string GetChartLevels(string fromDate, string toDate, string type,string forecaster)
        {
            string strSQL = "";
            string strReturn = "";
            string x = "";
            string y = "";

            string from = DateTime.Parse(fromDate).ToString("yyyy-MM-dd 00:00:00");
            string to = DateTime.Parse(toDate).ToString("yyyy-MM-dd 23:59:59");

            strSQL += "select DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),T.ForecastDate, 120)) AS [END], T.TGrade as 'value',T.ForecastDate,T.Station as 'Site' from " +
                     " T_HealthyGrade T  " +
                       "  where (T.ForecastDate between '" + from + "' and '" + to + "' ) and substring(CONVERT(CHAR(19),T.ForecastDate, 120),12,14)='20:00:00'" +
                       " and T.Station  in ('58367', '58370', '58361', '58362', '58462', " +
                       "'58460', '58461', '58463', '58365', '58366')  and Period='0' and T.Type='" + type + "'" +
                       "order by T.ForecastDate asc; ";//实况


            //strSQL += "select DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),T.LST, 120)) AS [END], T.TGrade as 'value',T.lst,T.Station as 'Site' from " +
            //          "( SELECT *,dateadd(HOUR,PERIOD,ForecastDate) as 'LST' from T_HealthyGrade ) T  " +
            //            "  where (T.LST between '" + from + "' and '" + to + "' ) and substring(CONVERT(CHAR(19),T.LST, 120),12,14)='20:00:00'" +
            //            " and T.Station  in ('58367', '58370', '58361', '58362', '58462', " +
            //            "'58460', '58461', '58463', '58365', '58366')   and T.Type='" + type + "'" +
            //            "order by T.LST asc";


            //如果包含今天的
            if (DateTime.Now <= DateTime.Parse(to))
            {

                //加上今天的内容, 要判断有没有更新发布
                //string sql = "select count (*) from [T_HealthyAutoSingle] where " +
                //          "   forecaster='" + forecaster + "' and DATEDIFF(day,createTime,getdate())=0 and period='17' ";
                //这里还是去掉forecaster,以后别的区要用这里统一要改 2017-12-06
                string sql = "select count (*) from [T_HealthyAutoSingle] where  DATEDIFF(day,createTime,getdate())=0 and period='17' ";

                Database m_Databases = new Database("DBCONFIG");
                DataSet dsts = m_Databases.GetDataset(sql);
                if (dsts != null && dsts.Tables.Count > 0 && dsts.Tables[0].Rows.Count > 0)
                {
                    int count = int.Parse(dsts.Tables[0].Rows[0][0].ToString());
                    if (count > 0)
                    {
                        strSQL += " SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),LST, 120)) AS [END], Level as 'value',LST,Station as 'Site' from [dbo].[T_HealthyWeather] where Station in ('中心城区','浦东新区','闵行区','宝山区','松江区','金山区','青浦区','奉贤区','嘉定区','崇明')  " +
                               "  and Period='17' and ForecastDate between '" + from + "' and '" + DateTime.Now.AddDays(0).ToString("yyyy-MM-dd 23:59:59") + "' " +
                               "  and  DateDiff(HOUR,ForecastDate,LST)='24' and Type='" + type + "' order by ForecastDate asc ";
                    }
                    else
                    {
                        strSQL += " SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),LST, 120)) AS [END], Level as 'value',LST,Station as 'Site' from [dbo].[T_HealthyWeather] where Station in ('中心城区','浦东新区','闵行区','宝山区','松江区','金山区','青浦区','奉贤区','嘉定区','崇明')  " +
                        "  and Period='17' and ForecastDate between '" + from + "' and '" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 23:59:59") + "' " +
                        "  and  DateDiff(HOUR,ForecastDate,LST)='24' and Type='" + type + "' order by LST  asc ";
                    }
                }
            }
            else
            {
                //这里要改成已经发布的人工预报
                strSQL += " SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),LST, 120)) AS [END], Level as 'value',LST,Station as 'Site' from [dbo].[T_HealthyWeather] where Station in ('中心城区','浦东新区','闵行区','宝山区','松江区','金山区','青浦区','奉贤区','嘉定区','崇明') " +
                            "  and Period='17' and ForecastDate between '" + from + "' and '" + to + "'  " +
                            "  and  DateDiff(HOUR,ForecastDate,LST)='24' and Type='" + type + "' order by LST asc";
            }

            


            string[] SitesArray = { "58367", "58370", "58361", "58362", "58462", "58460", "58461", "58463", "58365", "58366" };
            string[] SitesArrayName = { "中心城区", "浦东新区", "闵行区", "宝山区", "松江区", "金山区", "青浦区", "奉贤区", "嘉定区", "崇明" };

            //先全部查询出来
            Database m_Databasess = new Database("DBCONFIGIII");
            DataSet dst = m_Databasess.GetDataset(strSQL);

            string strReturns = "";
            try
            {
                for (int i = 0; i < SitesArray.Length; i++)
                {
                    for (int index = 0; index < dst.Tables.Count; index++)
                    {
                        DataTable dtElement = dst.Tables[index];
                        DataTable dtElementNew = dst.Tables[index].Clone();
                        DataRow[] rows;

                        rows = dtElement.Select("Site='" + SitesArray[i] + "' or Site='" + SitesArrayName[i] + "' ");

                        if (rows != null && rows.Length > 0)
                        {
                            foreach (DataRow row in rows)
                            {
                                row["value"] = GetWarnLevel(row["value"].ToString());
                                dtElementNew.Rows.Add(row.ItemArray);
                            }
                        }
                        else
                        {
                            continue;
                        }

                        x = ""; y = "";
                        //这里补数据
                        if (dtElementNew == null || dtElementNew.Rows.Count <= 0)
                            continue;

                        DateTime bt = DateTime.Parse(dtElementNew.Rows[0][2].ToString());
                        DateTime endt = DateTime.Parse(dtElementNew.Rows[dtElementNew.Rows.Count - 1][2].ToString());
                        for (DateTime dt = bt; dt <= endt; dt = dt.AddDays(1))
                        {
                            bool bl = false;
                            foreach (DataRow dr in dtElementNew.Rows)
                            {
                                if (DateTime.Parse(dr[2].ToString()).ToString("yyyy-MM-dd")
                                    == dt.ToString("yyyy-MM-dd"))
                                {
                                    x = x + "|" + dr[0].ToString();
                                    y = y + "|" + dr[1].ToString();
                                    bl = true;
                                    break;
                                }
                            }
                            if (!bl)
                            {
                                x = x + "|" + ConvertDateTimeInt(DateTime.Parse(dt.ToString("yyyy-MM-dd")));
                                y = y + "|" + "NULL";
                            }
                        }

                        strReturn = strReturn + ",'" + index.ToString() + "':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "'";
                    }
                    if (strReturn != ",")
                        strReturns += "{" + strReturn.TrimStart(',') + "}&";
                }
                if (strReturns != "&")
                    strReturns = strReturns.TrimEnd('&');

                return strReturns;
            }
            catch (Exception ex)
            {
                m_Log.Error("GetAQIChart", ex);
                return ex.ToString();
            }
        }

        //保存数据并生成图片
        public string SaveDrawMap(string data, string strForecastDate, string strPublishDate, string Type)
        {
            //区域名称
            string strAreaName = "null";
            string strAQIQuaLevel = "null";
            if (data != "")
            {
                string[] cells = data.Split('*');
                Dictionary<string, string> AreaData = new Dictionary<string, string>();
                for (int i = 0; i < cells.Length; i++)
                {
                    string[] strSingleArea = cells[i].Split('_');
                    if (strSingleArea[0] == "undefined" || strSingleArea[0] == "")
                        continue;

                    strAreaName = GetNAME(strSingleArea[0]);//区域名称
                    strAQIQuaLevel = strSingleArea[1];//级别  数字型
                    AreaData.Add(strAreaName, strAQIQuaLevel);
                }

                string strExMapBaseUrl = @"F:\sh";
                string strMapName = DateTime.Now.ToString("yyyyMMddHHmmss") + Type + ".GIF";
                DateTime pubTime = Convert.ToDateTime(strPublishDate);

                Dictionary<string, string> ReAreaData = new Dictionary<string, string>();
                string[] reorderAreas = { "嘉定区", "浦东新区", "金山区", "崇明县", "中心城区", "奉贤区", "宝山区", "闵行区", "青浦区", "松江区" };
                for (int i = 0; i < reorderAreas.Length; i++)
                    ReAreaData.Add(reorderAreas[i], AreaData[reorderAreas[i]]);

                CreateIMGIII createImg = new CreateIMGIII(ReAreaData, pubTime, strMapName, strExMapBaseUrl, strForecastDate, Type);
                //生成图片保存的路径
                string imgPath = "";
                if (Type.IndexOf("中暑") >= 0)
                {
                    imgPath = createImg.DealDataII();
                }
                else
                {
                    imgPath = createImg.DealData();
                }
                imgPath = imgPath.Replace("/", "\\");
                return imgPath;
            }
            return "fail";
        }


        /// <summary>
        /// DataTable to json
        /// </summary>
        /// <param name="jsonName">返回json的名称</param>
        /// <param name="dt">转换成json的表</param>
        /// <returns></returns>
        private string DataTableToJson(string jsonName, System.Data.DataTable dt)
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

        public string Query(string userGroup, string name, string startDate, string endDate, string diseaseType, string applyType)
        {
            Database thDB = new Database("DBCONFIGII");

            string userGroupStr = userGroup == "全部" ? " 1=1 " : " GroupName='" + userGroup + "'";
            string diseaseTypeStr = diseaseType == "全部" ? "1=1" : "C.HEATHYTYPE='" + diseaseType + "'";
            string applyTypeStr = applyType == "全部" ? "1=1" : "C.TYPE='" + applyType + "'";
            string nameStr = name == "全部" ? " 1=1 " : " Name='" + name + "'";
            String sqlQuery = @"SELECT C.ID,C.USERID,P.Name,P.GroupName,convert(varchar(19),C.DATE,120),C.TYPE,C.HeathyType,P.Email,P.Phone,ISNULL( C.PROCESSRESULT,'未处理'),C.PROCESSDATE FROM T_CancelRequest C  
                              INNER JOIN T_PubUser P ON P.UserID=C.UserID WHERE  (C.UserID IN (select UserID FROM T_PubUser 
                              WHERE " + nameStr + " and  " + userGroupStr + ")) AND  date BETWEEN '" + startDate + " 00:00:00" + "' AND '" + endDate + " 23:59:59' AND " + diseaseTypeStr + " AND " + applyTypeStr + "";

            DataTable dt = thDB.GetDataTable(sqlQuery);
            return DataTableToJson("data", dt);
        }


    }
}


