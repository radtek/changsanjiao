
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
    public class HealthyWeather
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
        public HealthyWeather()
        {
            m_Database = new Database();
            m_BackDays = int.Parse(ConfigurationManager.AppSettings["BackDays"]);
        }

        public string SaveHealthyWeather(string texts, string priod, string type, string forecastTime, string user)
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
                    row["level"] = vs[(i * 10) + 60 + j];
                    row["people"] = vs[(i * 10) + 120 + j];
                    row["premunition"] = vs[(i * 10) + 180 + j];
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
            m_DatabaseII.Execute(string.Format(del_sql, priod, "儿童感冒", "青年感冒", "老年感冒",
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
                        string types = "";
                        if (dt.TableName == "0" || dt.TableName == "1")
                            types = "儿童感冒";
                        else if (dt.TableName == "2" || dt.TableName == "3")
                            types = "青年感冒";
                        else if (dt.TableName == "4" || dt.TableName == "5")
                            types = "老年感冒";

                        string sql = string.Format(insert_sql, forecastTime, priod, area, lst, level, people, premunition, types, user);
                        m_DatabaseII.Execute(sql);
                    }
                }
                return "true";
            }
            catch { }
            return status;

        }

        public string SaveTempHealthyWeather(string texts, string priod, string type, string forecastTime, string user, string lst)
        {
            Database m_DatabaseII = new Database("DBCONFIGII");
            string status = "false";
            string[] vs = texts.Split('@');//薛辉  09-26
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
                    row["level"] = vs[(i * 10) + 10 + j];
                    row["people"] = vs[(i * 10) + 20 + j];
                    row["premunition"] = vs[(i * 10) + 30 + j];
                    row["LST"] = DateTime.Parse(lst).ToString("yyyy-MM-dd");
                    #region
                    //if (i % 2 == 1)
                    //{
                    //    if (priod == "10")
                    //        row["LST"] = DateTime.Parse(forecastTime).AddDays(1).ToString("yyyy-MM-dd");
                    //    else
                    //        row["LST"] = DateTime.Parse(forecastTime).AddDays(2).ToString("yyyy-MM-dd");
                    //}
                    //else
                    //{
                    //    if (priod == "17")
                    //        row["LST"] = DateTime.Parse(forecastTime).AddDays(1).ToString("yyyy-MM-dd");
                    //    else
                    //        row["LST"] = DateTime.Parse(forecastTime).AddDays(0).ToString("yyyy-MM-dd");
                    //}
                    #endregion
                    row["type"] = type;
                    dt.Rows.Add(row);
                }
                ds.Tables.Add(dt.Copy());
            }

            lst = DateTime.Parse(forecastTime).Year.ToString() + "年" + lst;
            lst = DateTime.Parse(lst).ToString("yyyy-MM-dd 00:00:00");

            //先删除
            m_DatabaseII.Execute(string.Format(del_sqlII, priod, type,
                                              DateTime.Parse(forecastTime).ToString("yyyy-MM-dd"),
                                              user,
                                              DateTime.Parse(lst).ToString("yyyy-MM-dd")));
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
                        string sql = string.Format(insert_sql, forecastTime, priod, area, lst, level, people, premunition, type, user);
                        m_DatabaseII.Execute(sql);
                    }
                }
                return "true";
            }
            catch { }
            return status;
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

        public string QueryHealthyWeatherForecast(string priod, string type, string forecastTime)
        {

            if (DateTime.Parse(forecastTime).ToString("yyyy-MM-dd")
                != DateTime.Now.ToString("yyyy-MM-dd"))
            {
                return QueryHealthyWeatherForecastHis(priod, type, forecastTime);

            }


            Database m_DatabaseII = new Database("DBCONFIGII");

            //获取预报时间
            string ty = "Guide17";
            if (priod == "10")
                ty = "Guide10";

            string sql_getTime = "select  * from T_HealthyTime where Type='" + ty + "'";
            DataTable dt_time = m_DatabaseII.GetDataTable(sql_getTime);
            string time = dt_time.Rows[0][0].ToString();
            string disTime = DateTime.Parse(dt_time.Rows[0][0].ToString()).ToString("yyyy-MM-dd HH:mm");
            if (priod == "10")
                time = DateTime.Parse(time).AddDays(-1).ToString("yyyy-MM-dd 20:00:00");
            else
                time = DateTime.Parse(time).AddDays(0).ToString("yyyy-MM-dd 08:00:00");

            //这个要稍微大的调整下，跟着T_HealthyTime来查询数据

            StringBuilder strBuilder = new StringBuilder();
            try
            {
                //string forecastTimes=forecastTime;//**
                //forecastTime = DateTime.Parse(forecastTime).AddDays(-1).ToString("yyyy-MM-dd 20:00:00"); //**
                string sql = string.Format(query_forecast_sql, time, type);
                DataTable dt = m_DatabaseII.GetDataTable(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    string lv = "";
                    string ple = "";
                    string pre = "";
                    string lst = "";
                    string sites = "";
                    DataRow[] rows = null;
                    // if (priod == "10")//**//**
                    //{
                    rows = dt.Select("PERIOD='24' or PERIOD='48' or PERIOD='72'");
                    //}//**
                    //else//**
                    //{
                    //    //查下是否有下午时次的记录   2016-11-03//**
                    //    forecastTimes = DateTime.Parse(forecastTimes).AddDays(0).ToString("yyyy-MM-dd 08:00:00");//**
                    //    string sqls = string.Format(query_forecast_sql, forecastTimes, type);
                    //    DataTable dts = m_DatabaseII.GetDataTable(sqls);//**
                    //    if (dts != null && dts.Rows.Count > 0) {
                    //        rows = dts.Select("PERIOD='24' or PERIOD='48'");
                    //    }
                    //    else//**
                    //    {
                    //        //还是去上午的时次//**
                    //       // rows = dt.Select("PERIOD='48' or PERIOD='72'"); //注释时间2017-03-08 薛辉 
                    //    }
                    //}//**

                    if (rows != null && rows.Length > 0)
                    {
                        foreach (DataRow row in rows)
                        {
                            string lenvel = GetLevelName(row["TGrade"].ToString());
                            if (type == "中暑")
                            {
                                lenvel = GetLevelNameII(row["TGrade"].ToString());
                            }
                            lv += (lenvel + ",");
                            string people = row["Group"].ToString().Replace(",", "，");
                            if (type == "中暑")
                            {
                                people = "-9999";
                            }
                            ple += (people + ",");

                            string premunition = "";
                            string gl1 = row["GuideLines1"].ToString().Replace(",", "，"); //薛辉  10-09        
                            string gl2 = row["GuideLines2"].ToString().Replace(",", "，");
                            try
                            {
                                if (gl2.Length <= 0)
                                    gl1 = gl1.Substring(0, gl1.Length - 1) + "。";
                            }
                            catch { }

                            try
                            {
                                if (!string.IsNullOrEmpty(gl2))
                                {
                                    gl2 = gl2.Substring(0, gl2.Length - 1) + "。";
                                }
                            }
                            catch { }
                            premunition = gl1 + gl2;

                            pre += (premunition + ",");
                            string lsts = DateTime.Parse(row["ForecastDate"].ToString()).AddHours(int.Parse(row["PERIOD"].ToString())).ToString("MM月dd日");//*****************这个前台改了，这个也要改
                            lst += (lsts + ",");
                            string site = GetSiteName(row["station"].ToString());
                            sites += (site + ",");
                        }
                    }
                    strBuilder.Append(lv + "*" + ple + "*" + pre + "*" + lst + "*" + sites + "@" + disTime);
                }
            }
            catch { }
            return strBuilder.ToString();
        }
        public string QueryHealthyWeatherForecastHis(string priod, string type, string forecastTime)
        {
            Database m_DatabaseII = new Database("DBCONFIGII");
            StringBuilder strBuilder = new StringBuilder();
            try
            {
                string forecastTimes = forecastTime;
                forecastTime = DateTime.Parse(forecastTime).AddDays(-1).ToString("yyyy-MM-dd 20:00:00");
                string sql = string.Format(query_forecast_sql, forecastTime, type);
                DataTable dt = m_DatabaseII.GetDataTable(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    string lv = "";
                    string ple = "";
                    string pre = "";
                    string lst = "";
                    string sites = "";
                    DataRow[] rows = null;
                    if (priod == "10")
                    {
                        rows = dt.Select("PERIOD='24' or PERIOD='48'");
                    }
                    else
                    {
                        //查下是否有下午时次的记录   2016-11-03
                        forecastTimes = DateTime.Parse(forecastTimes).AddDays(0).ToString("yyyy-MM-dd 08:00:00");
                        string sqls = string.Format(query_forecast_sql, forecastTimes, type);
                        DataTable dts = m_DatabaseII.GetDataTable(sqls);
                        if (dts != null && dts.Rows.Count > 0)
                        {
                            rows = dts.Select("PERIOD='24' or PERIOD='48' or PERIOD='72' ");
                        }
                        else
                        {
                            //还是去上午的时次
                            // rows = dt.Select("PERIOD='48' or PERIOD='72'"); //注释时间2017-03-08 薛辉 
                        }
                    }

                    if (rows != null && rows.Length > 0)
                    {
                        foreach (DataRow row in rows)
                        {
                            string lenvel = GetLevelName(row["TGrade"].ToString());
                            if (type == "中暑")
                            {
                                lenvel = GetLevelNameII(row["TGrade"].ToString());
                            }
                            lv += (lenvel + ",");
                            string people = row["Group"].ToString().Replace(",", "，");
                            if (type == "中暑")
                            {
                                people = "-9999";
                            }
                            ple += (people + ",");

                            string premunition = "";
                            string gl1 = row["GuideLines1"].ToString().Replace(",", "，"); //薛辉  10-09        
                            string gl2 = row["GuideLines2"].ToString().Replace(",", "，");
                            try
                            {
                                if (gl2.Length <= 0)
                                    gl1 = gl1.Substring(0, gl1.Length - 1) + "。";
                            }
                            catch { }

                            try
                            {
                                if (!string.IsNullOrEmpty(gl2))
                                {
                                    gl2 = gl2.Substring(0, gl2.Length - 1) + "。";
                                }
                            }
                            catch { }
                            premunition = gl1 + gl2;

                            pre += (premunition + ",");
                            string lsts = DateTime.Parse(row["ForecastDate"].ToString()).AddHours(int.Parse(row["PERIOD"].ToString())).ToString("MM月dd日");//*****************这个前台改了，这个也要改
                            lst += (lsts + ",");
                            string site = GetSiteName(row["station"].ToString());
                            sites += (site + ",");
                        }
                    }
                    strBuilder.Append(lv + "*" + ple + "*" + pre + "*" + lst + "*" + sites);
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
            string strSQL = "";
            string strReturn = "";
            string x = "";
            string y = "";

            string from = DateTime.Parse(fromDate).ToString("yyyy-MM-dd 00:00:00");
            string to = DateTime.Parse(toDate).ToString("yyyy-MM-dd 23:59:59");

            if (type == "气象")
            {
                #region
                string e1 = "", e2 = "", e3 = "";
                if (eName == "平均温度")
                {
                    e1 = "temperature";
                    e2 = "9";
                    e3 = "9";
                }
                else if (eName == "最高温度")
                {
                    e1 = "maxtemperature";
                    e2 = "10";
                    e3 = "10";
                }
                else if (eName == "最低温度")
                {
                    e1 = "mintemperature";
                    e2 = "11";
                    e3 = "11";
                }
                else if (eName == "风速")
                {
                    e1 = "wind_2min_speed";
                    e2 = "12";
                    e3 = "12";
                }
                else if (eName == "湿度")
                {
                    e1 = "relativehumidity";
                    e2 = "8";
                    e3 = "8";
                }
                else if (eName == "气压")
                {
                    e1 = "pressure";
                    e2 = "16";
                    e3 = "16";
                }
                #endregion

                strSQL += " SELECT DATEDIFF(S,'1970-01-01 00:00:00',CONVERT(CHAR(10),collect_time, 120)) AS [END], " + e1 + " as 'value',collect_time,Station as 'Site' from T_Cimiss_Day where  " +
                         " (collect_time between '" + from + "' and '" + to + "' ) " +
                         " and Station  in ('58367', '58370', '58361', '58362', '58462', " +
                         "'58460', '58461', '58463', '58365', '58366')   ORDER BY collect_time asc ;";//实况


                if (DateTime.Parse(to) >
                    DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59:59")))
                {
                    string too = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
                    string forecastDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 20:00:00");
                    string forecastDateII = DateTime.Now.AddDays(0).ToString("yyyy-MM-dd 08:00:00");
                    strSQL += " SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),DATEADD(hour,24,LST), 120)) AS [END], value,DATEADD(hour,24,LST) as 'LST',Site from T_ForecastWeather1 where  " +
                              " (DATEADD(hour,24,LST) between '" + from + "' and '" + to + "' ) " +
                              " and Site  in ('58367', '58370', '58361', '58362', '58462', " +
                              "'58460', '58461', '58463', '58365', '58366') and ITEMID='" + e2 + "'  and Period='" + period + "' " +
                              "and DurationID='7' and Module='WRF' " +//模式// DateAdd修改于03-21 
                    " UNION    SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),DATEADD(hour,24,LST), 120)) AS [END], value,DATEADD(hour,24,LST) as 'LST',Site from T_ForecastWeather1 where  " +
                             " (DATEADD(hour,24,LST) between '" + from + "' and '" + to + "' ) " +
                             " and Site  in ('58367', '58370', '58361', '58362', '58462', " +
                             "'58460', '58461', '58463', '58365', '58366') and ITEMID='" + e2 + "'  and Period<>'24' " +
                             "and DurationID='7' and Module='WRF' and ForecastDate='" + forecastDate + "' ORDER BY LST asc ;";//模式// DateAdd修改于03-21 



                    if (duration == "上午")
                    {
                        strSQL += " SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),DATEADD(hour,24,LST), 120)) AS [END], value,DATEADD(hour,24,LST) as 'LST',Site from T_ForecastWeather1 where  " +
                                      " (DATEADD(hour,24,LST) between '" + from + "' and '" + to + "' ) and substring(CONVERT(CHAR(19),DATEADD(hour,0,ForecastDate), 120),12,14)='20:00:00' " +
                                      " and Site  in ('58367', '58370', '58361', '58362', '58462', " +
                                      "'58460', '58461', '58463', '58365', '58366') and ITEMID='" + e3 + "' and Period='" + period + "' " +
                                      "and DurationID='7' and Module='ACityf' " +//人工预报
                             "UNION SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),DATEADD(hour,24,LST), 120)) AS [END], value,DATEADD(hour,24,LST) as 'LST',Site from T_ForecastWeather1 where  " +
                                      " (DATEADD(hour,24,LST) between '" + from + "' and '" + to + "' ) and substring(CONVERT(CHAR(19),DATEADD(hour,0,ForecastDate), 120),12,14)='20:00:00' " +
                                      " and Site  in ('58367', '58370', '58361', '58362', '58462', " +
                                      "'58460', '58461', '58463', '58365', '58366') and ITEMID='" + e3 + "' and Period<>'24' " +
                                      "and DurationID='7' and Module='ACityf' and  ForecastDate='" + forecastDate + "'  ORDER BY LST asc ;";//人工预报
                    }
                    else
                    {

                        //特殊处理下 
                        string maxDate = forecastDateII;
                        string mxSQL = "  SELECT top 1ForecastDate from T_ForecastWeather1 where  " +
                                       " Site  in ('58367', '58370', '58361', '58362', '58462', '58460', '58461', '58463', '58365', '58366') " +
                                       " and substring(CONVERT(CHAR(19),DATEADD(hour,0,ForecastDate), 120),12,14)='08:00:00' " +
                                       " and ITEMID='" + e3 + "'  and DurationID='7' and Module='ACityf'  " +
                                       " ORDER BY ForecastDate desc ; ";
                        DataTable max_dt = m_Database.GetDataTable(mxSQL);
                        if (max_dt != null && max_dt.Rows.Count > 0)
                        {
                            maxDate = max_dt.Rows[0][0].ToString();
                        }

                        strSQL += " SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),DATEADD(hour,24,LST), 120)) AS [END], value,DATEADD(hour,24,LST) as 'LST',Site from T_ForecastWeather1 where  " +
                                       " (DATEADD(hour,24,LST) between '" + from + "' and '" + to + "' ) and substring(CONVERT(CHAR(19),DATEADD(hour,0,ForecastDate), 120),12,14)='08:00:00' " +
                                       " and Site  in ('58367', '58370', '58361', '58362', '58462', " +
                                       "'58460', '58461', '58463', '58365', '58366') and ITEMID='" + e3 + "' and Period='" + period + "' " +
                                       "and DurationID='7' and Module='ACityf' " +//人工预报
                          " UNION   SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),DATEADD(hour,24,LST), 120)) AS [END], value,DATEADD(hour,24,LST) as 'LST',Site from T_ForecastWeather1 where  " +
                                       " (DATEADD(hour,24,LST) between '" + from + "' and '" + to + "' ) and substring(CONVERT(CHAR(19),DATEADD(hour,0,ForecastDate), 120),12,14)='08:00:00' " +
                                       " and Site  in ('58367', '58370', '58361', '58362', '58462', " +
                                       "'58460', '58461', '58463', '58365', '58366') and ITEMID='" + e3 + "' and Period<>'24'" +
                                       "and DurationID='7' and Module='ACityf' and ForecastDate='" + maxDate + "' ORDER BY LST asc ;";//人工预报
                    }
                }
                else
                {
                    #region
                    strSQL += " SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),DATEADD(hour,24,LST), 120)) AS [END], value,DATEADD(hour,24,LST) as 'LST',Site from T_ForecastWeather1 where  " +
                               " (DATEADD(hour,24,LST) between '" + from + "' and '" + to + "' ) " +
                               " and Site  in ('58367', '58370', '58361', '58362', '58462', " +
                               "'58460', '58461', '58463', '58365', '58366') and ITEMID='" + e2 + "'  and Period='" + period + "' " +
                               "and DurationID='7' and Module='WRF' ORDER BY LST asc ;";//模式// DateAdd修改于03-21 

                    if (duration == "上午")
                    {
                        strSQL += " SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),DATEADD(hour,24,LST), 120)) AS [END], value,DATEADD(hour,24,LST) as 'LST',Site from T_ForecastWeather1 where  " +
                                      " (DATEADD(hour,24,LST) between '" + from + "' and '" + to + "' ) and substring(CONVERT(CHAR(19),DATEADD(hour,0,ForecastDate), 120),12,14)='20:00:00' " +
                                      " and Site  in ('58367', '58370', '58361', '58362', '58462', " +
                                      "'58460', '58461', '58463', '58365', '58366') and ITEMID='" + e3 + "' and Period='" + period + "' " +
                                      "and DurationID='7' and Module='ACityf' ORDER BY LST asc ;";//人工预报
                    }
                    else
                    {

                        strSQL += " SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),DATEADD(hour,24,LST), 120)) AS [END], value,DATEADD(hour,24,LST) as 'LST',Site from T_ForecastWeather1 where  " +
                                       " (DATEADD(hour,24,LST) between '" + from + "' and '" + to + "' ) and substring(CONVERT(CHAR(19),DATEADD(hour,0,ForecastDate), 120),12,14)='08:00:00' " +
                                       " and Site  in ('58367', '58370', '58361', '58362', '58462', " +
                                       "'58460', '58461', '58463', '58365', '58366') and ITEMID='" + e3 + "' and Period='" + period + "' " +
                                       "and DurationID='7' and Module='ACityf' ORDER BY LST asc ;";//人工预报
                    }
                    #endregion
                }
            }
            else
            {

                #region
                string e1 = "", e2 = "";
                if (eName == "PM25")
                {
                    e1 = "1";
                    e2 = "1";
                }
                else if (eName == "PM10")
                {
                    e1 = "2";
                    e2 = "2";
                }
                else if (eName == "O38H")
                {
                    e1 = "5";
                    e2 = "5";
                }
                else if (eName == "NO2")
                {
                    e1 = "3";
                    e2 = "3";
                }
                else if (eName == "CO")
                {
                    e1 = "6";
                    e2 = "6";
                }
                else if (eName == "SO2")
                {
                    e1 = "7";
                    e2 = "7";
                }
                if (eName == "CO")
                {
                    eName = eName + "*1000";
                }
                #endregion


                strSQL += " SELECT DATEDIFF(S,'1970-01-01 00:00:00',CONVERT(CHAR(10),time_point, 120)) AS [END], " + eName + " as 'value',Time_Point,Station_Code as 'Site' from T_24ShAQI where  " +
                " (Time_Point between '" + from + "' and '" + to + "' ) " +
                " and Station_Code  in ('58367', '58370', '58361', '58362', '58462', " +
                "'58460', '58461', '58463', '58365', '58366')   ORDER BY Time_Point asc ;";//实况


                if (DateTime.Parse(to) >
              DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59:59")))
                {
                    string too = DateTime.Now.AddDays(0).ToString("yyyy-MM-dd 00:00:00");
                    string forecastDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 20:00:00");
                    string forecastDateII = DateTime.Now.AddDays(0).ToString("yyyy-MM-dd 08:00:00");
                    string forecastDateIII = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
                    string forecastDateIIII = DateTime.Now.AddDays(0).ToString("yyyy-MM-dd 00:00:00");
                    strSQL += " SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),LST, 120)) AS [END], value,lst,Site from T_ForecastSite where  " +
                   " (LST between '" + from + "' and '" + to + "' )  and   substring(CONVERT(CHAR(19),LST, 120),12,14)='20:00:00' " +
                   " and Site  in ('58367', '58370', '58361', '58362', '58462', " +
                   "'58460', '58461', '58463', '58365', '58366') and ITEMID='" + e1 + "' and Period='" + period + "' " +
                   "and DurationID='7' and Module='WRF' " +//模式
                  "  UNION  SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),LST, 120)) AS [END], value,lst,Site from T_ForecastSite where  " +
                   " (LST between '" + too + "' and '" + to + "' )  and   substring(CONVERT(CHAR(19),LST, 120),12,14)='20:00:00' " +
                   " and Site  in ('58367', '58370', '58361', '58362', '58462', " +
                   "'58460', '58461', '58463', '58365', '58366') and ITEMID='" + e1 + "' and Period<>'24' " +
                   "and DurationID='7' and Module='WRF' and ForecastDate='" + forecastDate + "' ORDER BY LST asc; ";

                    if (duration == "上午")
                    {

                        strSQL += " SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),LST, 120)) AS [END], value,lst,Site from T_ForecastGroup where  " +
                              " (LST between '" + from + "' and '" + to + "' ) " +
                              "  and ITEMID='" + e2 + "' and Period='" + period + "'  " +
                              "and durationID='7' and Module='GENERAL'  and LST not in (" +
                              "    select  lst from  T_ForecastGroup where ForecastDate='" + forecastDateIII + "' " +
                              "  and ITEMID='" + e2 + "' and durationID='7' and Module='GENERAL'	" +
                              "	  group by lst " +
                             ") " +
                              " UNION  SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),LST, 120)) AS [END], value,lst,Site from T_ForecastGroup where  " +
                              "   ITEMID='" + e2 + "'   " +
                              "and durationID='7' and Module='GENERAL' AND  ForecastDate='" + forecastDateIII + "' ORDER BY LST asc;";
                    }
                    else
                    {

                        //strSQL += " SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),LST, 120)) AS [END], value,lst,Site from T_ForecastGroup where  " +
                        //     " (LST between '" + from + "' and '" + to + "' ) " +
                        //     "  and ITEMID='" + e2 + "' and Period='" + period + "'  " +
                        //     "and durationID='7' and Module='GENERAL'  " +
                        //     " UNION  SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),LST, 120)) AS [END], value,lst,Site from T_ForecastGroup where  " +
                        //     "   ITEMID='" + e2 + "' and Period<>'24'  " +
                        //     "and durationID='7' and Module='GENERAL' AND  ForecastDate=( SELECT max(ForecastDate) from T_ForecastGroup where  " +
                        //     "   ITEMID='" + e2 + "' and Period<>'24'  " +
                        //     "and durationID='7' and Module='GENERAL' ) ORDER BY LST asc;";

                        strSQL += " SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),LST, 120)) AS [END], value,lst,Site from T_ForecastGroup where  " +
                            " (LST between '" + from + "' and '" + to + "' ) " +
                            "  and ITEMID='" + e2 + "' and Period='" + period + "'  " +
                            "and durationID='7' and Module='GENERAL'  and LST not in (" +
                            "    select  lst from  T_ForecastGroup where ForecastDate='" + forecastDateIIII + "' " +
                            "  and ITEMID='" + e2 + "' and durationID='7' and Module='GENERAL'	" +
                            "	  group by lst " +
                           ") " +
                            " UNION  SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),LST, 120)) AS [END], value,lst,Site from T_ForecastGroup where  " +
                            "   ITEMID='" + e2 + "'  " +
                            "and durationID='7' and Module='GENERAL' AND  ForecastDate='" + forecastDateIIII + "' ORDER BY LST asc;";
                    }

                }
                else
                {

                    strSQL += " SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),LST, 120)) AS [END], value,lst,Site from T_ForecastSite where  " +
                    " (LST between '" + from + "' and '" + to + "' )  and   substring(CONVERT(CHAR(19),LST, 120),12,14)='20:00:00' " +
                    " and Site  in ('58367', '58370', '58361', '58362', '58462', " +
                    "'58460', '58461', '58463', '58365', '58366') and ITEMID='" + e1 + "' and Period='" + period + "' " +
                    "and DurationID='7' and Module='WRF' ORDER BY LST asc ;";//模式

                    strSQL += " SELECT DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(CHAR(10),LST, 120)) AS [END], value,lst,Site from T_ForecastGroup where  " +
                    " (LST between '" + from + "' and '" + to + "' ) " +
                    "  and ITEMID='" + e2 + "' and Period='" + period + "'  " +
                    "and durationID='7' and Module='GENERAL' ORDER BY LST asc ;";//人工预报
                }
            }

            string[] SitesArray = { "58367", "58370", "58361", "58362", "58462", "58460", "58461", "58463", "58365", "58366" };

            //先全部查询出来
            Database m_Databases = new Database("DBCONFIGIII");
            DataSet dst = m_Databases.GetDataset(strSQL);

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

                        if (type == "环境" && index == 2)
                        {
                            rows = dtElement.Select("1=1");
                        }
                        else
                        {
                            rows = dtElement.Select("Site='" + SitesArray[i] + "'");
                        }

                        if (rows != null && rows.Length > 0)
                        {
                            foreach (DataRow row in rows)
                            {
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
                                if (DateTime.Parse(dr[2].ToString()).ToString("yyyy-MM-dd 00:00:00")
                                    == dt.ToString("yyyy-MM-dd 00:00:00"))
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

        public string GetWorkGroup()
        {
            Database m_Databases = new Database("DBCONFIGII");
            string strSQL = "select (ROW_NUMBER() OVER(ORDER BY LST desc )) as SID,Text,Type,pid,descript,memo,ID FROM D_HealtyType order by LST ";
            string jsonStr = "";
            try
            {
                DataTable tbUsers = m_Databases.GetDataTable(strSQL);
                DataRow newRow = tbUsers.NewRow();
                newRow["SID"] = "9";
                newRow["Text"] = "全部";
                newRow["pid"] = "0";
                newRow["Type"] = "默认";
                newRow["memo"] = "";
                newRow["ID"] = "-1";
                tbUsers.Rows.InsertAt(newRow, 0);
                jsonStr = DataTableToJson("data", tbUsers);
            }
            catch
            {
            }
            return jsonStr;
        }

        public string GetFYZY(string text, string type)
        {

            Database m_Databases = new Database("DBCONFIGII");
            string strSQL = "select (ROW_NUMBER() OVER(ORDER BY t1.item )) as SID,t1.Type,t1.guideLines1,t1.guideLines2,t1.Item,replace(replace(replace(replace(replace(t2.MC,'O3','(O3)'),'SO2','(SO2)'),'PM2.5','(PM2.5)'),'NO2','(NO2)'),'PM10','(PM10)'),t3.Mc,t1.Standard,t1.Months,t2.DvF,t2.DvT " +
                           " from T_HealthyGuidelines  t1  left join D_HealthyStandard t2 on t1.Standard=t2.DM LEFT join  " +
                           "D_HealthyMonths t3 on t1.Months=t3.DM  where t1.item='{0}' and t1.Type='{1}' order by t1.[Type],t1.[Item],t1.[Standard],t1.[Months] ";

            string strSQL2 = "select (ROW_NUMBER() OVER(ORDER BY t1.item )) as SID,t1.Type,t1.guideLines1,t1.guideLines2,t1.Item,replace(replace(replace(replace(replace(t2.MC,'O3','(O3)'),'SO2','(SO2)'),'PM2.5','(PM2.5)'),'NO2','(NO2)'),'PM10','(PM10)'),t3.Mc,t1.Standard,t1.Months,t2.DvF,t2.DvT " +
                           " from T_HealthyGuidelines  t1  left join D_HealthyStandard t2 on t1.Standard=t2.DM LEFT join  " +
                           "D_HealthyMonths t3 on t1.Months=t3.DM  where {0} order by t1.[Type],t1.[Item],t1.[Standard],t1.[Months] ";



            //string strSQL = "select (ROW_NUMBER() OVER(ORDER BY t1.item )) as SID,t2.MC,t2.DP" +
            //               " from T_HealthyGuidelines  t1  left join D_HealthyStandard t2 on t1.Standard=t2.DM LEFT join  " +
            //               "D_HealthyMonths t3 on t1.Months=t3.DM  order by t1.[Type],t1.[Item],t1.[Standard],t1.[Months] where t1.item='{0}' and t1.Type='{1}'";
            string jsonStr = "";
            try
            {
                switch (text)
                {
                    case "2":
                        text = "儿童感冒";
                        break;
                    case "3":
                        text = "成人感冒";
                        break;
                    case "4":
                        text = "老人感冒";
                        break;
                    case "5":
                        text = "COPD";
                        break;
                    case "6":
                        text = "儿童哮喘";
                        break;
                    case "7":
                        text = "中暑";
                        break;
                    case "8":
                        text = "重污染";
                        break;
                }
                switch (type)
                {
                    case "1":
                        type = "降温";
                        break;
                    case "2":
                        type = "温差";
                        break;
                    case "3":
                        type = "风力";
                        break;
                    case "4":
                        type = "湿度";
                        break;
                    case "5":
                        type = "温度";
                        break;
                    case "6":
                        type = "高温";
                        break;
                    case "7":
                        type = "中暑指数";
                        break;
                    case "8":
                        type = "PM2.5";
                        break;
                    case "9":
                        type = "PM10";
                        break;
                    case "10":
                        type = "O3";
                        break;
                    case "11":
                        type = "NO2";
                        break;
                    case "12":
                        type = "SO2";
                        break;
                }

                string where = "";
                string value = "";
                if (text == "-1" && type != "-1")
                {
                    where = " t1.item='{0}' ";
                    value = type;
                }
                if (type == "-1" && text != "-1")
                {
                    where = " t1.Type='{0}'";
                    value = text;
                }
                if (type == "-1" && text == "-1")
                {
                    where = "1=1";
                    value = "1=1";
                }
                DataTable tbUsers = null;
                if (!string.IsNullOrEmpty(where))
                {
                    try
                    {
                        where = string.Format(where, value);
                    }
                    catch { }
                    tbUsers = m_Databases.GetDataTable(string.Format(strSQL2, where));
                }
                else
                {
                    tbUsers = m_Databases.GetDataTable(string.Format(strSQL, type, text));
                }
                int index = 1;
                foreach (DataRow row in tbUsers.Rows)
                {
                    row["SID"] = index;
                    index++;
                }

                jsonStr = DataTableToJson("data", tbUsers);
            }
            catch (Exception ex)
            {
            }
            return jsonStr;
        }

        /// <summary>
        /// 保存指引信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="itemName"></param>
        /// <param name="zy1"></param>
        /// <param name="zy2"></param>
        /// <param name="tj"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public string SaveFYZY(string type, string itemName, string zy1, string zy2, string tj, string months)
        {
            Database m_Databases = new Database("DBCONFIGII");
            string flag = "0";
            //大于等于12并且小于等于13
            //首先插入到D表
            //D_HealthyStandard
            //D_HealthyMonths
            //查看有没有对应的字典表，如果有直接拿过来用
            tj = tj.Replace("大于等于", "≥").Replace("小于等于", "≤").Replace("大于", ">").Replace("小于", "<").Replace("等于", "=");
            string DvF = "-9999";//左区间
            string DvT = "9999";//右区间
            if (tj.IndexOf("X") >= 0)
            {
                tj = tj.Split('X')[0];
                if (tj.IndexOf("≥") >= 0 || tj.IndexOf(">") >= 0)
                {
                    DvT = "9999";
                    if (tj.IndexOf("≥") >= 0)
                        DvF = "[" + tj.Replace("≥", "");// xuehui 2017-08-04
                    else
                        DvF = tj.Replace(">", "");
                }
                if (tj.IndexOf("≤") >= 0 || tj.IndexOf("<") >= 0)
                {
                    DvF = "-9999";
                    if (tj.IndexOf("≤") >= 0)
                        DvT = tj.Replace("≤", "") + "]";
                    else
                        DvT = tj.Replace("<", "");
                }
                if (tj.IndexOf("=") >= 0)
                {
                    DvT = "9999";
                    DvF = tj.Replace("=", "");
                }
                tj = itemName + tj;
            }
            if (tj.IndexOf("并且") >= 0)
            {
                string t1 = tj.Split(new string[] { "并且" }, StringSplitOptions.None)[0];
                string t2 = tj.Split(new string[] { "并且" }, StringSplitOptions.None)[1];
                //>=12并且<=13
                if (t1.IndexOf("≥") >= 0)
                {
                    DvF = "[" + t1.Replace("≥", "");// xuehui 08-04
                    t1 = t1.Replace("≥", "") + "≤";
                }
                else if (t1.IndexOf(">") >= 0)
                {
                    DvF = t1.Replace(">", "");
                    t1 = t1.Replace(">", "") + "<";
                }
                else if (t1.IndexOf("=") >= 0)
                {
                    t1 = t1.Replace("=", "") + "=";
                }

                if (t2.IndexOf("≤") >= 0)
                {
                    DvT = t2.Replace("≤", "") + "]";
                }
                else if (t2.IndexOf("<") >= 0)
                {
                    DvT = t2.Replace("<", "");
                }
                tj = t1 + itemName + t2;
            }

            string sql1 = "select * from dbo.D_HealthyStandard where MC='" + tj + "' and Code='" + itemName.Replace("风力", "风速") + "'";
            string sql1_1 = "select max(DM) from dbo.D_HealthyStandard ";
            DataTable dt1 = m_Databases.GetDataTable(sql1);
            DataTable dt1_1 = m_Databases.GetDataTable(sql1_1);

            string standMaxNo = "0";
            bool isInsert = false;
            if (dt1_1 != null && dt1_1.Rows.Count > 0)
            {
                try
                {
                    standMaxNo = (int.Parse(dt1_1.Rows[0][0].ToString()) + 1).ToString();
                }
                catch { }
            }
            if (dt1 != null && dt1.Rows.Count > 0)
            {
                standMaxNo = dt1.Rows[0][0].ToString();
            }
            else
            {
                //新增字典表
                string inert = " insert into D_HealthyStandard(DM,MC,DP,DvF,DvT,Code) values('{0}','{1}',NULL,'{2}','{3}','{4}') ";
                string[] tjx = tj.Split(new string[] { itemName }, StringSplitOptions.None);
                inert = string.Format(inert, standMaxNo, tj, DvF, DvT, itemName.Replace("风力", "风速"));
                int count = m_Databases.Execute(inert);
                if (count > 0) { isInsert = true; }
            }

            months = months.Replace("Checkbox", "").TrimEnd(',');
            string sql2 = "select * from dbo.D_HealthyMonths where MC='" + months + "' and Code='" + itemName + "'";
            string sql2_2 = "select max(DM) from dbo.D_HealthyMonths ";
            DataTable dt2 = m_Databases.GetDataTable(sql2);
            DataTable dt2_2 = m_Databases.GetDataTable(sql2_2);
            string monthMaxNo = "0";

            if (dt2_2 != null && dt2_2.Rows.Count > 0)
            {
                monthMaxNo = (int.Parse(dt2_2.Rows[0][0].ToString()) + 1).ToString();
            }

            if (dt2 != null && dt2.Rows.Count > 0)
            {
                monthMaxNo = dt2.Rows[0][0].ToString();
            }
            else
            {
                //新增字典表
                string inert = "insert into D_HealthyMonths(DM,MC,DP,Code) values('{0}','{1}','{2}','{3}')";
                inert = string.Format(inert, monthMaxNo, months, standMaxNo, itemName);
                m_Databases.Execute(inert);
            }

            //最后插入主表
            string insert_tHeathyLine = "INSERT INTO [dbo].[T_HealthyGuidelines]" +
                                       "([Item]" +
                                       ",[Standard]" +
                                       ",[Months]" +
                                       ",[GuideLines1]" +
                                       ",[Type]" +
                                       ",[GuideLines2])" +
                                 " VALUES" +
                                       "('{0}'  ,'{1}'  ,'{2}','{3}' ,'{4}' ,'{5}') ";
            int counts = m_Databases.Execute(string.Format(insert_tHeathyLine, itemName, standMaxNo, monthMaxNo, zy1, type, zy2));
            if (counts > 0)
            {
                flag = "1";
                if (isInsert)
                {
                    string up_1 = "update D_HealthyMonths set DP=(DP+','+'" + standMaxNo + "') where code='" + itemName + "' and DM='" + monthMaxNo + "'";
                    m_Databases.Execute(up_1);
                }
            }
            return flag;
        }


        public string DeleteFYZY(string standID, string monthID,
           string oldType, string oldItem)
        {
            Database m_Databases = new Database("DBCONFIGII");
            //删除掉记录
            try
            {
                string del1 = "delete from T_HealthyGuidelines where Type='" + oldType + "' and Item='" + oldItem
                            + "' and Standard='" + standID + "'and  Months='" + monthID + "'";

                // string del2 = "delete from D_HealthyMonths where DM='" + monthID + "' and Code='" + oldItem + "'";
                //string del3 = "delete from D_HealthyStandard where DM='" + standID + "' and Code='" + oldItem + "'";
                m_Databases.Execute(del1);
                // m_Database.Execute(del2);
                // m_Database.Execute(del3);
                return "1";
            }
            catch
            {
                return "0";
            }
        }


        public string UpdateFYZY(string type, string itemName, string zy1, string zy2, string tj, string months, string standID, string monthID,
            string oldType, string oldItem)
        {
            Database m_Databases = new Database("DBCONFIGII");
            //先删除掉记录
            try
            {
                string del1 = "delete from T_HealthyGuidelines where Type='" + oldType + "' and Item='" + oldItem
                            + "' and Standard='" + standID + "'and  Months='" + monthID + "'";

                //string del2 = "delete from D_HealthyMonths where DM='" + monthID + "' and Code='"+oldItem+"'";
                // string del3 = "delete from D_HealthyStandard where DM='" + standID + "' and Code='" + oldItem + "'";
                m_Databases.Execute(del1);
                //m_Database.Execute(del2);
                // m_Database.Execute(del3);

            }
            catch
            {
                return "0";
            }

            string flag = "0";
            //大于等于12并且小于等于13
            //首先插入到D表
            //D_HealthyStandard
            //D_HealthyMonths
            //查看有没有对应的字典表，如果有直接拿过来用
            tj = tj.Replace("大于等于", "≥").Replace("小于等于", "≤").Replace("大于", ">").Replace("小于", "<").Replace("等于", "=");
            string DvF = "-9999";//左区间
            string DvT = "9999";//右区间
            if (tj.IndexOf("X") >= 0)
            {
                tj = tj.Split('X')[0];
                if (tj.IndexOf("≥") >= 0 || tj.IndexOf(">") >= 0)
                {
                    DvT = "9999";
                    if (tj.IndexOf("≥") >= 0)
                        DvF = "[" + tj.Replace("≥", ""); // xuehui 08-04
                    else
                        DvF = tj.Replace(">", "");

                }
                if (tj.IndexOf("≤") >= 0 || tj.IndexOf("<") >= 0)
                {
                    DvF = "-9999";
                    if (tj.IndexOf("≤") >= 0)
                        DvT = tj.Replace("≤", "") + "]";
                    else
                        DvT = tj.Replace("<", "");
                }
                if (tj.IndexOf("=") >= 0)
                {
                    DvT = "9999";
                    DvF = tj.Replace("=", "");
                }
                tj = itemName + tj;
            }
            if (tj.IndexOf("并且") >= 0)
            {
                string t1 = tj.Split(new string[] { "并且" }, StringSplitOptions.None)[0];
                string t2 = tj.Split(new string[] { "并且" }, StringSplitOptions.None)[1];
                //>=12并且<=13
                if (t1.IndexOf("≥") >= 0)
                {
                    DvF = "[" + t1.Replace("≥", "");
                    t1 = t1.Replace("≥", "") + "≤";
                }
                else if (t1.IndexOf(">") >= 0)
                {
                    DvF = t1.Replace(">", "");
                    t1 = t1.Replace(">", "") + "<";
                }
                else if (t1.IndexOf("=") >= 0)
                {
                    t1 = t1.Replace("=", "") + "=";
                }

                if (t2.IndexOf("≤") >= 0)
                {
                    DvT = t2.Replace("≤", "") + "]";
                }
                else if (t2.IndexOf("<") >= 0)
                {
                    DvT = t2.Replace("<", "");
                }
                tj = t1 + itemName + t2;
            }

            string sql1 = "select * from dbo.D_HealthyStandard where MC='" + tj + "' and Code='" + itemName.Replace("风力", "风速") + "'";
            string sql1_1 = "select max(DM) from dbo.D_HealthyStandard ";
            DataTable dt1 = m_Databases.GetDataTable(sql1);
            DataTable dt1_1 = m_Databases.GetDataTable(sql1_1);

            string standMaxNo = "0";
            bool isInsert = false;
            if (dt1_1 != null && dt1_1.Rows.Count > 0)
            {
                try
                {
                    standMaxNo = (int.Parse(dt1_1.Rows[0][0].ToString()) + 1).ToString();
                }
                catch { }
            }
            if (dt1 != null && dt1.Rows.Count > 0)
            {
                standMaxNo = dt1.Rows[0][0].ToString();
            }
            else
            {
                //新增字典表
                string inert = " insert into D_HealthyStandard(DM,MC,DP,DvF,DvT,Code) values('{0}','{1}',NULL,'{2}','{3}','{4}') ";
                string[] tjx = tj.Split(new string[] { itemName }, StringSplitOptions.None);
                inert = string.Format(inert, standMaxNo, tj, DvF, DvT, itemName.Replace("风力", "风速"));
                int count = m_Databases.Execute(inert);
                if (count > 0) { isInsert = true; }
            }


            months = months.Replace("Checkbox", "").TrimEnd(',');
            string sql2 = "select * from dbo.D_HealthyMonths where MC='" + months + "' and Code='" + itemName + "'";
            string sql2_2 = "select max(DM) from dbo.D_HealthyMonths ";
            DataTable dt2 = m_Databases.GetDataTable(sql2);
            DataTable dt2_2 = m_Databases.GetDataTable(sql2_2);
            string monthMaxNo = "0";

            if (dt2_2 != null && dt2_2.Rows.Count > 0)
            {
                monthMaxNo = (int.Parse(dt2_2.Rows[0][0].ToString()) + 1).ToString();
            }

            if (dt2 != null && dt2.Rows.Count > 0)
            {
                monthMaxNo = dt2.Rows[0][0].ToString();
            }
            else
            {
                //新增字典表
                string inert = "insert into D_HealthyMonths(DM,MC,DP,Code) values('{0}','{1}','{2}','{3}')";
                inert = string.Format(inert, monthMaxNo, months, standMaxNo, itemName);
                m_Databases.Execute(inert);
            }

            //最后插入主表
            string insert_tHeathyLine = "INSERT INTO [dbo].[T_HealthyGuidelines]" +
                                       "([Item]" +
                                       ",[Standard]" +
                                       ",[Months]" +
                                       ",[GuideLines1]" +
                                       ",[Type]" +
                                       ",[GuideLines2])" +
                                 " VALUES" +
                                       "('{0}'  ,'{1}'  ,'{2}','{3}' ,'{4}' ,'{5}') ";
            int counts = m_Databases.Execute(string.Format(insert_tHeathyLine, itemName, standMaxNo, monthMaxNo, zy1, type, zy2));
            if (counts > 0)
            {
                flag = "1";
                //更新D_HeathyMonth的DP字段
                if (isInsert)
                {

                    string up_1 = "update D_HealthyMonths set DP=(DP+','+'" + standMaxNo + "') where code='" + itemName + "' and DM='" + monthMaxNo + "'";
                    m_Databases.Execute(up_1);
                }
            }
            return flag;

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

        public string QueryUser(string healthyType, string groupName)
        {
            Database thDB = new Database("DBCONFIGII");
            groupName = HttpUtility.UrlDecode(groupName);
            string strWhere = groupName == "" ? "" : "WHERE groupName='" + groupName + "'";
            if (healthyType != "")
            {
                strWhere = strWhere == "" ? "WHERE" : strWhere + " AND";
                strWhere = strWhere + " PHealthyType like '%" + healthyType + "%'";
            }
            /*     //原先的
               string strSQl = @"Select UserID,Name,GroupName,Phone,Email,STUFF(cast((select DISTINCT ','+CHealthyType from V_USERINFO t2 where t1.UserID = t2.UserID for xml path('')) as varchar(100)),1,1,'') as HealthyType,
                   STUFF(cast((select DISTINCT ','+CMessage_PubLvl from V_USERINFO t2 where t1.UserID = t2.UserID for xml path('')) as varchar(100)),1,1,'') as Message_PubLvl,
                   STUFF(cast((select DISTINCT ','+CMessage_PubTime from V_USERINFO t2 where t1.UserID = t2.UserID for xml path('')) as varchar(100)),1,1,'') as Message_PubTime,
                   STUFF(cast((select DISTINCT ','+CEmail_PubLvl from V_USERINFO t2 where t1.UserID = t2.UserID for xml path('')) as varchar(100)),1,1,'') as Email_PubLvl, 
                   STUFF(cast((select DISTINCT ','+CEmail_PubTime from V_USERINFO t2 where t1.UserID = t2.UserID for xml path('')) as varchar(100)),1,1,'') as Email_PubTime,region,Remark
                   from V_USERINFO t1 " + strWhere + " GROUP BY UserID,Name,GroupName,Phone,Email, region,Remark";
             * */
            string strSQl = @"Select UserID,Name,GroupName,Phone,Email,CanMessage,CanEmail,STUFF(cast((select DISTINCT ','+CHealthyType from V_USERINFO t2 where t1.UserID = t2.UserID for xml path('')) as varchar(100)),1,1,'') as HealthyType,
                STUFF(cast((select DISTINCT ','+CMessage_PubLvl from V_USERINFO t2 where t1.UserID = t2.UserID for xml path('')) as varchar(100)),1,1,'') as Message_PubLvl,
                STUFF(cast((select DISTINCT ','+CMessage_PubTime from V_USERINFO t2 where t1.UserID = t2.UserID for xml path('')) as varchar(100)),1,1,'') as Message_PubTime,
                STUFF(cast((select DISTINCT ','+CEmail_PubLvl from V_USERINFO t2 where t1.UserID = t2.UserID for xml path('')) as varchar(100)),1,1,'') as Email_PubLvl, 
                STUFF(cast((select DISTINCT ','+CEmail_PubTime from V_USERINFO t2 where t1.UserID = t2.UserID for xml path('')) as varchar(100)),1,1,'') as Email_PubTime,region,Remark
                from V_USERINFO t1 " + strWhere + " GROUP BY UserID,Name,GroupName,Phone,Email, region,CanEmail,CanMessage, Remark";
            DataTable theDt = thDB.GetDataTable(strSQl);
            //wb   2017.07.03    //判断是否支持短信邮件发送，不支持则全为空
            foreach (DataRow row in theDt.Rows)
            {
                if (row["CanMessage"].ToString().Trim() == "False")
                {
                    row["Message_PubLvl"] = "";
                    row["Message_PubTime"] = "";
                }
                if (row["CanEmail"].ToString().Trim() == "False")
                {
                    row["Email_PubLvl"] = "";
                    row["Email_PubTime"] = "";
                }
            }
            theDt.Columns.Remove("CanMessage");
            theDt.Columns.Remove("CanEmail");
            return DataTableToJson("data", theDt);
        }

        public string QueryUserII(string postionAreas, string groupName)
        {
            Database thDB = new Database("DBCONFIGII");
            groupName = HttpUtility.UrlDecode(groupName);
            string strWhere = groupName == "" ? "" : "WHERE groupName='" + groupName + "'";
            postionAreas = postionAreas.Replace("全部", "");
            if (postionAreas != "")
            {
                strWhere = strWhere == "" ? "WHERE" : strWhere + " AND";
                strWhere = strWhere + " Region like '%" + postionAreas + "%'";
            }
            string strSQl = @"Select UserID,Name,GroupName,Phone,Email,STUFF(cast((select DISTINCT ','+CHealthyType from V_USERINFO t2 where t1.UserID = t2.UserID for xml path('')) as varchar(100)),1,1,'') as HealthyType,
                STUFF(cast((select DISTINCT ','+CMessage_PubLvl from V_USERINFO t2 where t1.UserID = t2.UserID for xml path('')) as varchar(100)),1,1,'') as Message_PubLvl,
                STUFF(cast((select DISTINCT ','+CMessage_PubTime from V_USERINFO t2 where t1.UserID = t2.UserID for xml path('')) as varchar(100)),1,1,'') as Message_PubTime,
                STUFF(cast((select DISTINCT ','+CEmail_PubLvl from V_USERINFO t2 where t1.UserID = t2.UserID for xml path('')) as varchar(100)),1,1,'') as Email_PubLvl, 
                STUFF(cast((select DISTINCT ','+CEmail_PubTime from V_USERINFO t2 where t1.UserID = t2.UserID for xml path('')) as varchar(100)),1,1,'') as Email_PubTime,region,Remark
                from V_USERINFO t1 " + strWhere + " GROUP BY UserID,Name,GroupName,Phone,Email, region,Remark";
            DataTable theDt = thDB.GetDataTable(strSQl);
            return DataTableToJson("data", theDt);
        }

        public string QueryLog(string IsAll, string sendStatus, string start, string end, string healthyType, string sendType, string Alias)
        {
            Database thDB = new Database("DBCONFIGII");

            start = DateTime.Parse(start).ToString("yyyy-MM-dd");
            end = DateTime.Parse(end).ToString("yyyy-MM-dd");

            String isalll = IsAll == "1" ? "是" : "否";

            string sql_user = " select * from T_user where Alias='" + Alias + "'";
            DataTable dt_user = thDB.GetDataTable(sql_user);
            string companyName = "";
            if (dt_user != null && dt_user.Rows.Count > 0)
            {
                companyName = dt_user.Rows[0]["WindowsUser"].ToString();
            }

            string strWhere = "WHERE T.SENDDATE BETWEEN '" + start + " 00:00:00" + "' AND '" + end + " 23:59:59' AND T.ISALL='" + isalll + "' AND T.WindowsUser='" + companyName + "'";
            if (sendStatus != "全部")
            {
                String st = sendStatus == "成功" ? "成功" : "失败";
                strWhere += "AND T.sendstatus='" + st + "'";
            }
            if (sendType != "全部")
            {
                strWhere += "AND T.TYPE='" + sendType + "'";
            }
            if (healthyType != "")
            {
                if (healthyType != "全部")
                {
                    strWhere += " AND T.HealthyType ='" + healthyType + "'";
                }
            }
            //string strSQl = " select * from ( Select SENDUSER,RECEIVEUSER,TYPE,HealthyType,EMAIL,PHONE,CASE SendStatus WHEN 1 THEN '成功'" +
            //                   "ELSE '失败'" +
            //                   "END AS SendStatus," +
            //                   "CONVERT(VARCHAR(19),SENDDATE,121) as 'SENDDATE',CASE ISALL WHEN 1 THEN '是'" +
            //                   "ELSE '否'END AS ISALL FROM V_SENDLOG where Type<>'FTP' " +
            //                   "union all" +
            //                  " Select SENDUSER,reciver,TYPE,HealthyType,EMAIL,PHONE,CASE SendStatus WHEN 1 THEN '成功'" +
            //                  " ELSE '失败'" +
            //                  " END AS SendStatus," +
            //                  " CONVERT(VARCHAR(19),SENDDATE,121) as 'SENDDATE',CASE ISALL WHEN 1 THEN '是'" +
            //                  " ELSE '否'END AS ISALL FROM V_SENDLOGII where Type='FTP' ) T  " + strWhere;
            // xuehui 12-07 
            string strSQl = " select * from ( Select SENDUSER,RECEIVEUSER,TYPE,HealthyType,t1.EMAIL,PHONE,CASE SendStatus WHEN 1 THEN '成功'" +
                              "ELSE '失败'" +
                              "END AS SendStatus," +
                              "CONVERT(VARCHAR(19),SENDDATE,121) as 'SENDDATE',CASE ISALL WHEN 1 THEN '是'" +
                              "ELSE '否'END AS ISALL,t2.WindowsUser FROM V_SENDLOG t1 left join T_User t2  on t1.SendUser=t2.Alias  where Type<>'FTP' " +
                              "union all" +
                             " Select SENDUSER,reciver,TYPE,HealthyType,t3.EMAIL,PHONE,CASE SendStatus WHEN 1 THEN '成功'" +
                             " ELSE '失败'" +
                             " END AS SendStatus," +
                             " CONVERT(VARCHAR(19),SENDDATE,121) as 'SENDDATE',CASE ISALL WHEN 1 THEN '是'" +
                             " ELSE '否'END AS ISALL,t4.WindowsUser FROM V_SENDLOGII  t3 left join T_User t4  on t3.SendUser=t4.Alias where Type='FTP' ) T  " + strWhere;


            DataTable theDt = thDB.GetDataTable(strSQl);
            return DataTableToJson("data", theDt);
        }

        public string GetCancel()
        {
            Database thDB = new Database("DBCONFIGII");
            string strSQl = "SELECT T.ID,T.USERID,R.NAME,R.PHONE,R.EMAIL,TYPE,CONVERT(VARCHAR(19),DATE,121) FROM T_CancelRequest T INNER JOIN T_PUBUSER R ON T.UserID=R.UserID WHERE PROCESSRESULT<>'已处理'";// Handled=0 xuehui 1011
            DataTable theDt = thDB.GetDataTable(strSQl);
            return DataTableToJson("data", theDt);
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

        public string QueryGroup(string station, string area)
        {
            Database thDB = new Database("DBCONFIGII");
            area = HttpUtility.UrlDecode(area);
            string strSQl;
            string[] area1 = area.Split(',');
            if (station != "")
            {
                strSQl = @"select Alias,UserName,SN,Description+'-'+cast(ID as nvarchar(50)) as station,email,postionarea,datetime 
                     from T_User a ,T_Classes b WHERE a.BZ=b.ID and a.BZ =" + Convert.ToInt32(station);
            }
            else
            {
                strSQl = @"select Alias,UserName,SN,Description+'-'+cast(ID as nvarchar(50)) as station,email,postionarea,datetime 
                     from T_User a ,T_Classes b WHERE a.BZ=b.ID";
            }
            if (area1.Length < 10 && area1.Length > 0 && area1[0] != "")
            {
                if (area1.Length == 1)
                {
                    strSQl += @" and postionarea like '%" + area1[0] + "%'";
                }
                else
                {
                    strSQl += @" and (postionarea like '%" + area1[0] + "%'";
                    for (int i = 1; i < area1.Length; i++)
                    {
                        strSQl += @" or postionarea like '%" + area1[i] + "%'";
                    }
                    strSQl += ")";
                }
            }
            DataTable theDt = thDB.GetDataTable(strSQl);
            return DataTableToJson("data", theDt);
        }


        public string QueryGroupII(string station, string area)
        {
            Database thDB = new Database("DBCONFIGII");
            area = HttpUtility.UrlDecode(area);
            string strSQl;
            string[] area1 = area.Split(',');
            if (station != "" && station != "init")
            {
                strSQl = @"select Alias,UserName,SN,Description+'-'+cast(ID as nvarchar(50)) as station,email,postionarea,WindowsUser from T_User a ,T_Classes b WHERE a.BZ=b.ID and a.BZ =" + Convert.ToInt32(station);
            }
            else
            {
                strSQl = @"select Alias,UserName,SN,Description+'-'+cast(ID as nvarchar(50)) as station,email,postionarea,WindowsUser from T_User a ,T_Classes b WHERE a.BZ=b.ID";
            }
            if (area1.Length < 10 && area1.Length > 0 && area1[0] != "")
            {
                if (area1.Length == 1)
                {
                    strSQl += @" and postionarea like '%" + area1[0] + "%'";
                }
                else
                {
                    strSQl += @" and (postionarea like '%" + area1[0] + "%'";
                    for (int i = 1; i < area1.Length; i++)
                    {
                        strSQl += @" or postionarea like '%" + area1[i] + "%'";
                    }
                    strSQl += ")";
                }
            }
            DataTable theDt = thDB.GetDataTable(strSQl);
            return DataTableToJson("data", theDt);
        }
        public string CanSave(string userName, string type, string forecastDate) {
            string sqlUser = " SELECT * FROM dbo.T_User WHERE Alias='"+ userName + "' AND WindowsUser='长三角'";
            string sqlFore = " SELECT * FROM dbo.T_HealthyForecast WHERE ForecastDate='"+ forecastDate + " 08:00:00' and Type LIKE '%"+type+"%'";
            DataTable dtUser = m_Database.GetDataTable(sqlUser);
            if (!(dtUser != null && dtUser.Rows.Count > 0)) {
                return "true";
            }
            DataTable dtFore = m_Database.GetDataTable(sqlFore);
            if (!(dtFore != null && dtFore.Rows.Count > 0)) {
                return "false";
            }
            return "true";
        }


    }
}


