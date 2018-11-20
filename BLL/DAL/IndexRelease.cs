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
using System.Collections;
using System.Drawing.Imaging;

namespace MMShareBLL.DAL
{
    public class IndexRelease
    {
        //用于记录20时读取的天数是当天晚上，或者昨天晚上
        string Days = "0";
        //用于记录系统错误日志
        protected static readonly log4net.ILog m_Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Database m_Database;
        private int m_BackDays;
        DataSetForcast.T_HealthyWeatherDataTable t_healthyweather = new DataSetForcast.T_HealthyWeatherDataTable();
        #region sqls
        string insert_sql = "INSERT INTO [T_ScenAirIndex] " +
                               "([ForecastDate]" +
                               ",[PERIOD]" +
                               ",[GuideLines1]" +
                               ",[GuideLines2]" +
                               ",[station]" +
                               ",[Type]" +
                               ",[Grade]) VALUES" +
                               "('{0}'" +
                               ",'{1}'" +
                               ",'{2}'" +
                               ",'{3}'" +
                               ",'{4}'" +
                               ",'{5}'" +
                               ",'{6}')";
        string del_sqlNew = "delete from T_ScenAirIndex where ForecastDate='{0}' and Type in ('{1}')";
        string del_sqlNewII = "delete from T_ScenAirIndex where ForecastDate='{0}' and Type in ('{1}') and PERIOD='{2}'";
        //string del_sqlII = "delete from T_ScenAirForecast where Period='{0}' and  Type in ('{1}') and CONVERT(varchar(10), ForecastDate, 120)='{2}' and forecaster='{3}' and  CONVERT(varchar(10), LST, 120)='{4}'  ";
        string query_forecast_sql = "select ForecastDate,PERIOD,GuideLines1,GuideLines2,Type,Grade,station from T_ScenAirForecast where Type='{1}' and ForecastDate='{0}';";
        string query_forecast_sqlII = "select ForecastDate,PERIOD,GuideLines1,GuideLines2,Type,Grade,station from T_ScenAirForecast where (Type='天气分指数' or Type='霾指数' or Type='空气质量分指数' or Type='综合观景指数') and ForecastDate='{0}';";
        string query_index_sql = "select ForecastDate,PERIOD,GuideLines1,GuideLines2,Type,Grade,station from T_ScenAirIndex where Type='{1}' and ForecastDate='{0}';";
        #endregion
        public IndexRelease()
        {
            m_Database = new Database();
            m_BackDays = int.Parse(ConfigurationManager.AppSettings["BackDays"]);
        }

        //查询空气清洁度预报，T_ScenAirForecast
        public string QueryIndexReleaseForecast(string priod, string type, string forecastTime)
        {
            Database m_DatabaseII = new Database("DBCONFIG");

            StringBuilder strBuilder = new StringBuilder();
            try
            {
                string forecastTimes = forecastTime;
                forecastTime = DateTime.Parse(forecastTimes).ToString("yyyy-MM-dd 08:00:00");
                string sql = string.Format(query_forecast_sql, forecastTime, type);
                DataTable dt = m_DatabaseII.GetDataTable(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    string lv = "";
                    string jdts = "";
                    string xxts = "";
                    string lst = "";
                    string sites = "";
                    DataRow[] rows = null;
                    if (priod == "08")
                    {
                        rows = dt.Select("PERIOD='0' or PERIOD='24' or PERIOD='48'");
                    }
                    else
                    {
                        //查下是否有下午时次的记录   
                        forecastTimes = DateTime.Parse(forecastTimes).AddDays(0).ToString("yyyy-MM-dd 20:00:00");
                        string sqls = string.Format(query_forecast_sql, forecastTimes, type);
                        DataTable dts = m_DatabaseII.GetDataTable(sqls);
                        if (dts != null && dts.Rows.Count > 0)
                        {
                            rows = dts.Select("PERIOD='24' or PERIOD='48' or PERIOD='72' ");
                            Days = "0";
                        }
                        else//查询前一天  薛辉  02-27去掉
                        {
                            //forecastTimes = DateTime.Parse(forecastTimes).AddDays(-1).ToString("yyyy-MM-dd 20:00:00");
                            //string sqls2 = string.Format(query_forecast_sql, forecastTimes, type);
                            //DataTable dts2 = m_DatabaseII.GetDataTable(sqls2);
                            //rows = dts2.Select("PERIOD='48'");
                            //Days = "-1";
                        }
                    }

                    if (rows != null && rows.Length > 0)
                    {
                        foreach (DataRow row in rows)
                        {
                            string lenvel = GetLevelName(row["Grade"].ToString());
                            if (type == "空气清洁度指数")
                            {
                                lenvel = GetLevelName(row["Grade"].ToString());
                            }
                            lv += (lenvel + ",");
                            string gl1 = row["GuideLines1"].ToString().Replace(",", "，"); //薛辉  10-09        
                            string gl2 = row["GuideLines2"].ToString().Replace(",", "，");
                            jdts += (gl1 + ",");
                            xxts += (gl2 + ",");

                            string lsts = DateTime.Parse(row["ForecastDate"].ToString()).AddHours(int.Parse(row["PERIOD"].ToString())).ToString("MM月dd日");//*****************这个前台改了，这个也要改
                            lst += (lsts + ",");
                            string site = GetSiteName(row["station"].ToString());
                            sites += (site + ",");
                        }
                    }
                    strBuilder.Append(lv + "*" + jdts + "*" + xxts + "*" + lst + "*" + sites);
                }
            }
            catch { }
            return strBuilder.ToString();
        }

        //查询综合观景指数预报,T_ScenAirForecast   预报表
        public string QueryIndexReleaseForecastII(string priod, string type, string forecastTime)
        {
            Database m_DatabaseII = new Database("DBCONFIG");

            StringBuilder strBuilder = new StringBuilder();
            try
            {
                string forecastTimes = forecastTime;
                forecastTime = DateTime.Parse(forecastTimes).ToString("yyyy-MM-dd 08:00:00");
                string query_forecast_sqlIIs = "select ForecastDate,PERIOD,GuideLines1,GuideLines2,Type,Grade,station from T_ScenAirForecast where (Type='天气分指数' " +
                                               " or Type='霾指数' or Type='空气质量分指数' or Type='综合观景指数') and ForecastDate='{0}' order by ForecastDate ,period, station , Grade desc";
                string sql = string.Format(query_forecast_sqlIIs, forecastTime, type);
                DataTable dt = m_DatabaseII.GetDataTable(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    string lv = "";
                    string jdts = "";
                    string xxts = "";
                    string lst = "";
                    string sites = "";
                    DataRow[] rows = null;
                    if (priod == "08")
                    {
                        rows = dt.Select("PERIOD='0' or PERIOD='24' or PERIOD='48'");
                    }
                    else
                    {
                        //查下是否有下午时次的记录  
                        forecastTimes = DateTime.Parse(forecastTimes).AddDays(0).ToString("yyyy-MM-dd 20:00:00");
                        string sqls = string.Format(query_forecast_sqlII, forecastTimes, type);
                        dt = m_DatabaseII.GetDataTable(sqls);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            rows = dt.Select("PERIOD='24' or PERIOD='48' or PERIOD='72'");
                            Days = "0";
                        }
                        else
                        {
                            //薛辉去掉02-27
                            //forecastTimes = DateTime.Parse(forecastTimes).AddDays(-1).ToString("yyyy-MM-dd 20:00:00");
                            //string sqls2 = string.Format(query_forecast_sqlII, forecastTimes, type);
                            //DataTable dts2 = m_DatabaseII.GetDataTable(sqls2);
                            //rows = dts2.Select("PERIOD='48'");
                            //Days = "-1";
                        }
                    }

                    if (rows != null && rows.Length > 0)
                    {


                        int s = 0;
                        string[] items = { "0", "24", "48", "72" };
                        string[] sitesS ={"10001A","10002A","10003A","10004A",
                                         "10005A","10006A","10007A","10008A",
                                         "10009A","10010A","10011A","10012A",
                                         "10013A","10014A","10015A","10016A",
                                         "10017A","10018A"};


                        foreach (DataRow row in rows)
                        {

                            string guide1 = "";
                            string guide2 = "";
                            string gl1 = "";
                            string gl2 = "";
                            string tp = row["Type"].ToString();
                            string level = "";
                            string Grades = row["Grade"].ToString();
                            if (tp == "综合观景指数")
                            {
                                level = GetLevelNameII(row["Grade"].ToString());
                                lv += (level + ",");
                                string lsts = DateTime.Parse(row["ForecastDate"].ToString()).AddHours(int.Parse(row["PERIOD"].ToString())).ToString("MM月dd日");//*****************这个前台改了，这个也要改
                                lst += (lsts + ",");
                                string site = GetSiteName(row["station"].ToString());
                                sites += (site + ",");
                            }

                            if (tp == "天气分指数" || tp == "空气质量分指数" || tp == "霾指数" || tp == "综合观景指数")
                            {
                                if (row["GuideLines1"].ToString() != "")
                                {
                                    guide1 = row["GuideLines1"].ToString().Replace(",", "，");
                                    gl1 += (guide1 + ";");
                                }
                                if (row["GuideLines2"].ToString() != "")
                                {
                                    guide2 = row["GuideLines2"].ToString().Replace(",", "，");
                                    gl2 += (guide2 + ";");
                                }
                                s++;
                            }
                            if (s == 1)//以前是判断==3，0828薛辉改动
                            {
                                s = 0;
                                jdts += (gl1 + ",");
                                xxts += (gl2 + ",");
                                gl1 = "";
                                gl2 = "";
                            }
                        }
                    }
                    strBuilder.Append(lv + "*" + jdts + "*" + xxts + "*" + lst + "*" + sites);
                }
            }
            catch { }
            return strBuilder.ToString();
        }

        //查询已保存的空气清洁度，T_ScenAirIndex
        public string QueryIndexRelease(string priod, string type, string forecastTime)
        {
            Database m_DatabaseII = new Database("DBCONFIG");

            StringBuilder strBuilder = new StringBuilder();
            try
            {
                string forecastTimes = forecastTime;
                forecastTime = DateTime.Parse(forecastTimes).ToString("yyyy-MM-dd 08:00:00");
                string sql = string.Format(query_index_sql, forecastTime, type);
                DataTable dt = m_DatabaseII.GetDataTable(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    string lv = "";
                    string jdts = "";
                    string xxts = "";
                    string lst = "";
                    string sites = "";
                    DataRow[] rows = null;
                    if (priod == "08")
                    {
                        rows = dt.Select("PERIOD='0' or PERIOD='24' or PERIOD='48'");
                    }
                    else
                    {
                        //查下是否有下午时次的记录   2016-11-03
                        forecastTimes = DateTime.Parse(forecastTimes).AddDays(0).ToString("yyyy-MM-dd 20:00:00");
                        string sqls = string.Format(query_index_sql, forecastTimes, type);
                        DataTable dts = m_DatabaseII.GetDataTable(sqls);
                        if (dts != null && dts.Rows.Count > 0)
                        {
                            rows = dts.Select("PERIOD='24' or PERIOD='48' or  PERIOD='72'");
                            Days = "0";
                        }
                        else
                        {
                            //薛辉去掉02-27
                            //forecastTimes = DateTime.Parse(forecastTimes).AddDays(-1).ToString("yyyy-MM-dd 20:00:00");
                            //string sqls2 = string.Format(query_forecast_sql, forecastTimes, type);
                            //DataTable dts2 = m_DatabaseII.GetDataTable(sqls2);
                            //rows = dts2.Select("PERIOD='48'");
                            //Days = "-1";
                        }
                    }

                    if (rows != null && rows.Length > 0)
                    {
                        foreach (DataRow row in rows)
                        {
                            string lenvel = GetLevelName(row["Grade"].ToString());
                            if (type == "空气清洁度指数")
                            {
                                lenvel = GetLevelName(row["Grade"].ToString());
                            }
                            lv += (lenvel + ",");
                            string gl1 = row["GuideLines1"].ToString().Replace(",", "，"); //薛辉  10-09        
                            string gl2 = row["GuideLines2"].ToString().Replace(",", "，");
                            jdts += (gl1 + ",");
                            xxts += (gl2 + ",");

                            string lsts = DateTime.Parse(row["ForecastDate"].ToString()).AddHours(int.Parse(row["PERIOD"].ToString())).ToString("MM月dd日");//*****************这个前台改了，这个也要改
                            lst += (lsts + ",");
                            string site = GetSiteName(row["station"].ToString());
                            sites += (site + ",");
                        }
                    }
                    strBuilder.Append(lv + "*" + jdts + "*" + xxts + "*" + lst + "*" + sites);
                }
            }
            catch { }
            return strBuilder.ToString();
        }

        //查询已保存的综合观景指数，T_ScenAirIndex
        public string QueryIndexReleaseII(string priod, string type, string forecastTime)
        {
            Database m_DatabaseII = new Database("DBCONFIG");

            StringBuilder strBuilder = new StringBuilder();
            try
            {
                string forecastTimes = forecastTime;
                forecastTime = DateTime.Parse(forecastTimes).ToString("yyyy-MM-dd 08:00:00");
                string sql = string.Format(query_index_sql, forecastTime, type);
                DataTable dt = m_DatabaseII.GetDataTable(sql);


                if (dt == null || dt.Rows.Count <= 0)
                {
                    forecastTime = DateTime.Parse(forecastTimes).ToString("yyyy-MM-dd 20:00:00");
                    sql = string.Format(query_index_sql, forecastTime, type);
                    dt = m_DatabaseII.GetDataTable(sql);
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    string lv = "";
                    string jdts = "";
                    string xxts = "";
                    string lst = "";
                    string sites = "";
                    DataRow[] rows = null;
                    if (priod == "08")
                    {
                        rows = dt.Select("PERIOD='0' or PERIOD='24' or PERIOD='48'");
                    }
                    else
                    {
                        //查下是否有下午时次的记录   2016-11-03
                        forecastTimes = DateTime.Parse(forecastTimes).AddDays(0).ToString("yyyy-MM-dd 20:00:00");
                        string sqls = string.Format(query_index_sql, forecastTimes, type);
                        DataTable dts = m_DatabaseII.GetDataTable(sqls);
                        if (dts != null && dts.Rows.Count > 0)
                        {
                            rows = dts.Select("PERIOD='24' or PERIOD='48' or PERIOD='72'");
                            Days = "0";
                        }
                        else
                        {
                            //薛辉去掉  02-27
                            //forecastTimes = DateTime.Parse(forecastTimes).AddDays(-1).ToString("yyyy-MM-dd 20:00:00");
                            //string sqls2 = string.Format(query_forecast_sql, forecastTimes, type);
                            //DataTable dts2 = m_DatabaseII.GetDataTable(sqls2);
                            //rows = dts2.Select("PERIOD='48'");
                            //Days = "-1";
                        }
                    }

                    if (rows != null && rows.Length > 0)
                    {
                        foreach (DataRow row in rows)
                        {
                            string lenvel = GetLevelNameII(row["Grade"].ToString());
                            if (type == "综合观景指数")
                            {
                                lenvel = GetLevelNameII(row["Grade"].ToString());
                            }
                            lv += (lenvel + ",");
                            string gl1 = row["GuideLines1"].ToString().Replace(",", "，"); //薛辉  10-09        
                            string gl2 = row["GuideLines2"].ToString().Replace(",", "，");
                            jdts += (gl1 + ",");
                            xxts += (gl2 + ",");

                            string lsts = DateTime.Parse(row["ForecastDate"].ToString()).AddHours(int.Parse(row["PERIOD"].ToString())).ToString("MM月dd日");//*****************这个前台改了，这个也要改
                            lst += (lsts + ",");
                            string site = GetSiteName(row["station"].ToString());
                            sites += (site + ",");
                        }
                    }
                    strBuilder.Append(lv + "*" + jdts + "*" + xxts + "*" + lst + "*" + sites);
                }
            }
            catch { }
            return strBuilder.ToString();
        }

        private string GetSiteID(string siteName)
        {
            string siteID = "";
            switch (siteName)
            {
                case "上海野生动物园": siteID = "10001A"; break;
                case "上海世纪公园": siteID = "10002A"; break;
                case "上海鲜花港": siteID = "10003A"; break;
                case "锦江乐园": siteID = "10004A"; break;
                case "上海金罗店美兰湖景区": siteID = "10005A"; break;
                case "上海马陆葡萄艺术村": siteID = "10012A"; break;//xuehui 03-14
                case "上海欢乐谷": siteID = "10006A"; break;
                case "上海辰山植物园": siteID = "10007A"; break;
                case "枫泾古镇": siteID = "10008A"; break;
                case "金山城市沙滩景区": siteID = "10009A"; break;
                case "廊下生态园": siteID = "10010A"; break;
                case "朱家角古镇": siteID = "10011A"; break;
                case "上海市青少年校外活动营地—东方绿舟": siteID = "10013A"; break;
                case "上海市青少年校外活动营地-东方绿舟": siteID = "10013A"; break;
                case "碧海金沙景区": siteID = "10014A"; break;
                case "上海海湾国家森林公园": siteID = "10015A"; break;
                case "东平国家森林公园": siteID = "10016A"; break;
                case "上海明珠湖·西沙湿地景区": siteID = "10017A"; break;
                case "迪士尼度假区": siteID = "10018A"; break;

            }
            return siteID;
        }

        private string GetLevelName(string level)
        {
            string levelName = "";
            switch (level)
            {
                case "0": levelName = ""; break;
                case "1": levelName = "清洁"; break;
                case "2": levelName = "一般"; break;
                case "3": levelName = "不清洁"; break;
                case "4": levelName = "非常不清洁"; break;
            }
            return levelName;
        }

        private string GetLevel(string levelName)
        {
            string level = "";
            switch (levelName)
            {
                case "清洁": level = "1"; break;
                case "一般": level = "2"; break;
                case "不清洁": level = "3"; break;
                case "非常不清洁": level = "4"; break;
                case "": level = "0"; break;
            }
            return level;
        }
        //王斌 2017.5.8
        private string GetLevelII(string levelName)
        {
            string level = "";
            switch (levelName)
            {
                case "五星": level = "1"; break;
                case "四星": level = "2"; break;
                case "三星": level = "3"; break;
                case "二星": level = "4"; break;
                case "": level = "0"; break;
            }
            return level;
        }

        private string GetSiteName(string site)
        {
            string siteName = "";
            switch (site)
            {
                case "10001A": siteName = "上海野生动物园"; break;
                case "10002A": siteName = "上海世纪公园"; break;
                case "10003A": siteName = "上海鲜花港"; break;
                case "10004A": siteName = "锦江乐园"; break;
                case "10005A": siteName = "上海金罗店美兰湖景区"; break;
                case "10012A": siteName = "上海马陆葡萄艺术村"; break;// xuehui 03-14
                case "10006A": siteName = "上海欢乐谷"; break;
                case "10007A": siteName = "上海辰山植物园"; break;
                case "10008A": siteName = "枫泾古镇"; break;
                case "10009A": siteName = "金山城市沙滩景区"; break;
                case "10010A": siteName = "廊下生态园"; break;
                case "10011A": siteName = "朱家角古镇"; break;
                case "10013A": siteName = "上海市青少年校外活动营地—东方绿舟"; break;
                case "10014A": siteName = "碧海金沙景区"; break;
                case "10015A": siteName = "上海海湾国家森林公园"; break;
                case "10016A": siteName = "东平国家森林公园"; break;
                case "10017A": siteName = "上海明珠湖·西沙湿地景区"; break;
                case "10018A": siteName = "迪士尼度假区"; break;

            }
            return siteName;
        }
        //王斌  2017.5.8
        private string GetLevelNameII(string level)
        {
            string levelName = "";
            switch (level)
            {
                case "0": levelName = ""; break;
                case "1": levelName = "五星"; break;
                case "2": levelName = "四星"; break;
                case "3": levelName = "三星"; break;
                case "4": levelName = "二星"; break;

            }
            return levelName;
        }
        //观景指数等级预报保存
        public string SaveIndexRelease(string texts, string priod, string type, string forecastTime, string user)
        {
            string forecastTimes = "";
            string status = "false";
            string[] vs = texts.Split('@');
            int count = (vs.Length - 1) / 72;
            DataSet ds = new DataSet();
            for (int i = 0; i < count; i++)
            {
                DataTable dt = t_healthyweather.Clone();
                dt.TableName = i.ToString();
                for (int j = 0; j < 18; j++)
                {
                    DataRow row = dt.NewRow();
                    row["area"] = vs[(i * 18) + j];
                    row["level"] = vs[(i * 18) + 54 + j];
                    row["people"] = vs[(i * 18) + 108 + j];
                    row["premunition"] = vs[(i * 18) + 162 + j];
                    row["type"] = type;
                    dt.Rows.Add(row);
                }
                ds.Tables.Add(dt.Copy());
            }
            if (priod == "08")
            {
                forecastTimes = DateTime.Parse(forecastTime).ToString("yyyy-MM-dd 08:00:00");
            }
            else if (priod == "20")
            {
                forecastTimes = DateTime.Parse(forecastTime).ToString("yyyy-MM-dd 20:00:00");
            }
            //else if (priod == "20" && Days == "-1")
            //{
            //    forecastTimes = DateTime.Parse(forecastTime).AddDays(-1).ToString("yyyy-MM-dd 20:00:00");
            //}
            //先删除
            m_Database.Execute(string.Format(del_sqlNew, forecastTimes, type));
            try
            {
                foreach (DataTable dt in ds.Tables)
                {
                    int a = Convert.ToInt32(dt.TableName);
                    foreach (DataRow row in dt.Rows)
                    {
                        string ForeDate = forecastTimes;
                        string PERIOD = "";
                        string gl1 = row["people"].ToString();
                        string gl2 = row["premunition"].ToString();
                        string station = GetSiteID(row["area"].ToString());
                        string Type = row["type"].ToString();
                        string Grade = "";
                        if (Type == "综合观景指数")
                        {
                            Grade = GetLevelII(row["level"].ToString());
                        }
                        else
                        {
                            Grade = GetLevel(row["level"].ToString());
                        }
                        #region
                        if (priod == "08")
                        {
                            if (a == 0)
                            {
                                PERIOD = "0";
                            }
                            else if (a == 1)
                            {
                                PERIOD = "24";
                            }
                            else if (a == 2)
                            {
                                PERIOD = "48";
                            }
                        }
                        else if (priod == "20")
                        {
                            if (a == 0)
                            {
                                PERIOD = "24";
                            }
                            else if (a == 1)
                            {
                                PERIOD = "48";
                            }
                            else if (a == 2)
                            {
                                PERIOD = "72";
                            }
                        }
                        //else if (priod == "20" && Days == "-1")
                        //{
                        //    if (a == 0)
                        //    {
                        //        PERIOD = "48";
                        //    }
                        //    else if (a == 1)
                        //    {
                        //        PERIOD = "";
                        //    }
                        //    else if (a == 2)
                        //    {
                        //        PERIOD = "";
                        //    }
                        //}
                        #endregion
                        string sql = string.Format(insert_sql, ForeDate, PERIOD, gl1, gl2, station, Type, Grade);
                        m_Database.Execute(sql);
                    }
                }
                WriteLog(user, "观景指数等级预报", "" + priod + "时保存", "成功");
                return "true";
            }
            catch (Exception ex)
            {
                WriteLog(user, "观景指数等级预报", "" + priod + "时保存," + ex.Message, "失败");
            }

            if (status == "false")
            {
                WriteLog(user, "观景指数等级预报", "" + priod + "时保存", "失败");
            }
            else
            { WriteLog(user, "观景指数等级预报", "" + priod + "时保存", "成功"); }

            return status;
        }

        public string SaveTempIndexRelease(string texts, string priod, string type, string forecastTime, string user, string tableName)
        {
            string forecastTimes = "";
            string PERIOD = "";
            string status = "false";
            string[] vs = texts.Split('@');
            DataSet ds = new DataSet();
            DataTable dt = t_healthyweather.Clone();
            for (int j = 0; j < 18; j++)
            {
                DataRow row = dt.NewRow();
                row["area"] = vs[j];
                row["level"] = vs[18 + j];
                row["people"] = vs[36 + j];
                row["premunition"] = vs[54 + j];
                row["type"] = type;
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt.Copy());

            if (priod == "08")
            {
                forecastTimes = DateTime.Parse(forecastTime).ToString("yyyy-MM-dd 08:00:00");
            }
            else if (priod == "20")
            {
                forecastTimes = DateTime.Parse(forecastTime).ToString("yyyy-MM-dd 20:00:00");
            }


            #region
            if (priod == "08")
            {
                if (tableName == "dd1er")
                {
                    PERIOD = "0";
                }
                else if (tableName == "dd2er")
                {
                    PERIOD = "24";
                }
                else if (tableName == "dd3er")
                {
                    PERIOD = "48";
                }
            }
            else if (priod == "20")
            {
                if (tableName == "dd1er")
                {
                    PERIOD = "24";
                }
                else if (tableName == "dd2er")
                {
                    PERIOD = "48";
                }
                else if (tableName == "dd3er")
                {
                    PERIOD = "72";
                }
            }

            #endregion
            //先删除
            m_Database.Execute(string.Format(del_sqlNewII, forecastTimes, type, PERIOD));

            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    string ForeDate = forecastTimes;
                    string gl1 = row["people"].ToString();
                    string gl2 = row["premunition"].ToString();
                    string station = GetSiteID(row["area"].ToString());
                    string Type = row["type"].ToString();
                    string Grade = "";
                    if (Type == "综合观景指数")
                    {
                        Grade = GetLevelII(row["level"].ToString());
                    }
                    else
                    {
                        Grade = GetLevel(row["level"].ToString());
                    }

                    string sql = string.Format(insert_sql, ForeDate, PERIOD, gl1, gl2, station, Type, Grade);
                    m_Database.Execute(sql);
                }
                return "true";
            }
            catch { }
            return status;
        }


        //王斌  2017.4.18
        //王斌  2017.4.18  2017.5.2  2017.5.8
        //发布
        public string Publish(string period, string type, string user)
        {
            string isSucess = "成功";
            try
            {
                Database m_database = new Database("DBCONFIG");
                string sql1;
                string dd1 = DateTime.Now.ToString("yyyy-MM-dd") + " " + period + ":00:00";
                sql1 = "SELECT * FROM T_ScenAirIndex WHERE TYPE='" + type + "' " +
                        "AND ForecastDate='" + dd1 + "' and station<>'10018A'  ORDER BY  ForecastDate DESC ";
                DataTable dt = m_database.GetDataTable(sql1);
                if (dt == null || dt.Rows.Count <= 0)
                {
                    isSucess = "失败";
                    WriteLog(user, "观景指数等级预报", period + "时FTP发布，但未保存数据", isSucess);
                    return "请保存数据后发布!";
                }

                string[] periods = { "0", "24", "48" };

                if (period == "20")
                    periods = new string[] { "24", "48", "72" };
                //王斌  2017.5.8
                string fn;
                if (type == "综合观景指数") { fn = "观景指数"; }
                else { fn = type; }
                string[] myFTP = ConfigurationManager.AppSettings["FTP"].ToString().Split(';');
                string url = myFTP[0];
                string f_userName = myFTP[1];
                string f_password = myFTP[2];
                int foreTime = 24;
                foreach (string pd in periods)
                {

                    DataRow[] rows = dt.Select(" period = '" + pd + "'");
                    if (rows != null && rows.Length > 0)
                    {
                        StringBuilder SB = new StringBuilder();
                        int index = 1;
                        foreach (DataRow row in rows)
                        {
                            //1个时间，1个文本
                            string text = index + "\t" + GetClassName(row["station"].ToString()) + "\t" + GetStationName(row["station"].ToString()) + "\t" + GetGrade(row["Grade"].ToString()) + "\t" + GetStar(row["Grade"].ToString()) + "\t" + GetGradeName(row["Grade"].ToString(), type) + "\t" + row["GuideLines1"].ToString() + "\t" + row["GuideLines2"].ToString() + Environment.NewLine;
                            SB.Append(text);
                            index++;
                        }
                        string centent = SB.ToString();
                        //将文本写入到本地

                        string time = DateTime.Now.ToString("yyyyMMdd") + period + foreTime.ToString();
                        string fileName = fn + time + ".txt";   //王斌  2017.5.8
                        string direct = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempText");
                        if (!Directory.Exists(direct))
                        {
                            Directory.CreateDirectory(direct);
                        }
                        string path = Path.Combine(direct, fileName);

                        StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding("GB2312"));
                        sw.Write(centent);
                        sw.Flush();
                        sw.Close();
                        //将本地文本上传的到FTP
                        //string filepath = "yewu/hjqx/zhishu/" + "/" + fileName;
                        //本地测试
                        string filepath = "lang/" + "/" + fileName;
                        Ftp ftp = new Ftp(url, f_userName, f_password);
                        try
                        {
                            ftp.Upload(path, fileName);
                        }
                        catch (Exception ex)
                        {
                            isSucess = "失败";
                            WriteLog(user, "观景指数等级预报", period + "时FTP发布", isSucess);
                            return ex.Message.ToString();
                        }
                    }
                    //day++;
                    foreTime = foreTime + 24;
                }
            }
            catch (Exception ex)
            {
                isSucess = "失败";
            }
            WriteLog(user, "观景指数等级预报", period + "时FTP发布", isSucess);
            return "OK";
        }


        private string GetClassName(string site)
        {
            string className = "";
            switch (site)
            {
                case "10001A": className = "一般类    "; break;
                case "10002A": className = "一般类    "; break;
                case "10003A": className = "鲜花类    "; break;
                case "10004A": className = "游乐场类  "; break;
                case "10005A": className = "一般类    "; break;
                case "10006A": className = "游乐场类  "; break;
                case "10007A": className = "森林类    "; break;
                case "10008A": className = "古镇类    "; break;
                case "10009A": className = "海边类    "; break;
                case "10010A": className = "水果采摘类"; break;
                case "10011A": className = "古镇类    "; break;
                case "10012A": className = "水果采摘类"; break;
                case "10013A": className = "一般类    "; break;
                case "10014A": className = "海边类    "; break;
                case "10015A": className = "森林类    "; break;
                case "10016A": className = "森林类    "; break;
                case "10017A": className = "森林类    "; break;
                case "10018A": className = "游乐场类  "; break;
            }
            return className;
        }

        //获取站点名称
        public string GetStationName(string station)
        {
            switch (station)
            {
                case "10001A": station = "上海野生动物园          "; break;
                case "10002A": station = "上海世纪公园            "; break;
                case "10003A": station = "上海鲜花港              "; break;
                case "10004A": station = "锦江乐园                "; break;
                case "10005A": station = "上海金罗店美兰湖景区    "; break;
                case "10012A": station = "上海马陆葡萄艺术村      "; break;//xuehui 03-14
                case "10006A": station = "上海欢乐谷              "; break;
                case "10007A": station = "上海辰山植物园          "; break;
                case "10008A": station = "枫泾古镇                "; break;
                case "10009A": station = "金山城市沙滩景区        "; break;
                case "10010A": station = "廊下生态园              "; break;
                case "10011A": station = "朱家角古镇              "; break;
                case "10013A": station = "东方绿舟                "; break;
                case "10014A": station = "碧海金沙景区            "; break;
                case "10015A": station = "上海海湾国家森林公园    "; break;
                case "10016A": station = "东平国家森林公园        "; break;
                case "10017A": station = "西沙湿地景区            "; break;
                case "10018A": station = "迪士尼度假区            "; break;
            }
            return station;
        }

        //获取风险等级
        public string GetGrade(string grade)
        {
            switch (grade)
            {
                case "1": grade = "1级"; break;
                case "2": grade = "2级"; break;
                case "3": grade = "3级"; break;
                case "4": grade = "4级"; break;
            }
            return grade;
        }
        //风险等级转换
        public string GetGradeName(string grade, string type)
        {
            if (type == "空气清洁度指数")
            {
                switch (grade)
                {
                    case "1": grade = "清洁      "; break;
                    case "2": grade = "一般      "; break;
                    case "3": grade = "不清洁    "; break;
                    case "4": grade = "非常不清洁"; break;
                }
            }
            else
            {
                switch (grade)
                {
                    case "1": grade = "非常适宜"; break;
                    case "2": grade = "适宜    "; break;
                    case "3": grade = "一般    "; break;
                    case "4": grade = "不适宜  "; break;
                }
            }
            return grade;
        }


        //王斌  2017.5.2
        public string GetStar(string grade)
        {
            switch (grade)
            {
                case "1": grade = "五星"; break;
                case "2": grade = "四星"; break;
                case "3": grade = "三星"; break;
                case "4": grade = "二星"; break;
            }
            return grade;
        }

        public string PublishWB(string period, string type, string user)
        {
            string isSucess = "成功";
            Database m_database = new Database("DBCONFIG");
            string forecastDates = DateTime.Now.ToString("yyyy-MM-dd ") + period + ":00:00";
            string pd = "0";
            if (period == "20")
                pd = "24";

            //获取数据源
            string sqls = " select ForecastDate,PERIOD,GuideLines1,GuideLines2,Type,Grade,station " +
                          "   from T_ScenAirIndex where Type='综合观景指数' " +
                          "   and ForecastDate='" + forecastDates + "' and PERIOD='" + pd + "'";

            Database db = new Database();
            DataTable dt = db.GetDataTable(sqls);
            if (dt == null || dt.Rows.Count <= 0)
            {
                isSucess = "失败";
                WriteLog(user, "观景指数等级预报", period + "时微博发布，但未保存数据", isSucess);
                return "请保存数据后发布";
            }

            try
            {
                string[] sites = { "10007A", "10015A", "10016A", "10017A", "10008A", 
                                   "10011A", "10004A", "10006A", "10018A", "10003A",
                                   "10012A", "10010A", "10009A", "10013A", "10014A", 
                                   "10001A", "10002A", "10005A" };

                string[] sitesCode = { "ChenShan", "HaiWan", "DongPing", "XiSha", "FengJing", 
                                   "ZhuJia", "JinJiang", "HuanLe", "DSN", "XianHua",
                                   "MaLu", "LangXia", "jinshan", "DongFang", "BiHai", 
                                   "YeSheng", "ShiJi", "JinLuo" };

                float[,] sitesXY = {  { 1202, 490 },{ 1408, 565 },{ 1322, 186 },{ 1227, 145 },
                                  { 1137, 571 },{ 1172, 434 },{ 1282, 446 },{ 1217, 459},
                                  { 1412, 441 },{ 1493, 546 },{ 1237, 320 },{ 1193, 601 },
                                  { 1262, 634 },{ 1123, 471 },{ 1347, 585 },{ 1443, 480 },
                                  { 1373, 400 },{ 1278, 311 }};

                string p = AppDomain.CurrentDomain.BaseDirectory.ToString();
                string imgPath = Path.Combine(p, "weibo\\weibo.jpg");
                string iconPath = Path.Combine(p, "weibo\\img");

                System.Drawing.Image imgSrc = System.Drawing.Image.FromFile(imgPath);
                int i = 0;
                int y = 0, y2 = 0, y3 = 0, y4 = 0, y5 = 85, y6 = 730;
                string publishTime = "";
                foreach (string site in sites)
                {
                    DataRow[] rows = dt.Select("station='" + site + "'");
                    string startCount = GetStar(rows[0]["Grade"].ToString());
                    string guidenLine1 = GetText(rows[0]["GuideLines1"].ToString(), 14);
                    string guidenLine2 = GetText(rows[0]["GuideLines2"].ToString(), 34);
                    publishTime = DateTime.Parse(rows[0]["ForecastDate"].
                                          ToString()).ToString("yyyy年MM月dd日HH时");
                    string forecastDate = DateTime.Parse(rows[0]["ForecastDate"].
                                         ToString()).AddHours(int.Parse(rows[0]["Period"].ToString())).ToString("yyyy年MM月dd日");

                    using (Graphics g = Graphics.FromImage(imgSrc))
                    {
                        #region 画星级
                        using (System.Drawing.Font f = new System.Drawing.Font("微软雅黑", 10))
                        {
                            using (Brush b = new SolidBrush(Color.FromArgb(51, 51, 51)))
                            {
                                y = 40 + y;
                                if (y == 40)
                                    y = 45;

                                g.DrawString(startCount, f, b, 310, y);
                            }
                        }
                        #endregion
                        #region 画矩形
                        y2 = 40 + y2;
                        if (y2 == 40) { y2 = 39; }
                        if (i >= 14)
                            y2 = y2 + 1;
                        if (i >= 16)
                            y2 = y2 - 1;

                        Rectangle rectNew = new Rectangle(383, y2, 5, 31);
                        System.Drawing.Drawing2D.LinearGradientBrush brush = new
                            System.Drawing.Drawing2D.LinearGradientBrush(rectNew,
                            GetStarColor(rows[0]["Grade"].ToString()), GetStarColor(rows[0]["Grade"].ToString()),
                            System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
                        g.FillRectangle(brush, rectNew);
                        #endregion
                        #region 画指引1
                        using (System.Drawing.Font f = new System.Drawing.Font("微软雅黑", 10))
                        {
                            using (Brush b = new SolidBrush(Color.FromArgb(51, 51, 51)))
                            {
                                y3 = 40 + y3;
                                if (y3 == 40)
                                    y3 = 45;

                                g.DrawString(guidenLine1, f, b, 390, y3);
                            }
                        }
                        #endregion
                        #region 画指引2
                        using (System.Drawing.Font f = new System.Drawing.Font("微软雅黑", 10))
                        {
                            using (Brush b = new SolidBrush(Color.FromArgb(51, 51, 51)))
                            {
                                y4 = 40 + y4;
                                if (y4 == 40)
                                    y4 = 45;

                                g.DrawString(guidenLine2, f, b, 580, y4);
                            }
                        }
                        #endregion
                        #region 预报时间
                        using (System.Drawing.Font f = new System.Drawing.Font("宋体", 16, FontStyle.Regular))
                        {
                            using (Brush b = new SolidBrush(Color.Black))
                            {
                                g.DrawString("预报时间：" + forecastDate, f, b, 1069, y5);
                            }
                        }
                        #endregion
                        #region 发布时间
                        using (System.Drawing.Font f = new System.Drawing.Font("宋体", 11, FontStyle.Regular))
                        {
                            using (Brush b = new SolidBrush(Color.Black))
                            {
                                g.DrawString("上海中心气象台" + publishTime + "发布", f, b, 1169, y6);
                            }
                        }
                        #endregion
                        #region 画箭头
                        Image IMG = Image.FromFile(Path.Combine(iconPath, sitesCode[i] + "-" + rows[0]["Grade"].ToString() + ".png"));
                        IMG = resizeImage(new Bitmap(IMG), new Size(25, 38));
                        g.DrawImage(IMG, sitesXY[i, 0], sitesXY[i, 1]);
                        #endregion
                    }
                    i++;
                }
                string fontpath = Path.Combine(p, "weibo\\wb.jpg");
                System.Drawing.Imaging.EncoderParameters ep = new System.Drawing.Imaging.EncoderParameters();
                ep.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.MimeType == "image/jpeg")
                        ici = codec;
                }
                imgSrc.Save(fontpath, ici, ep);
                imgSrc.Dispose();
                //发布FTP
                string[] myFTP = ConfigurationManager.AppSettings["FTPWB"].ToString().Split(';');
                string url = myFTP[0];
                string f_userName = myFTP[1];
                string f_password = myFTP[2];
                Ftp ftp = new Ftp(url, f_userName, f_password);
                try
                {
                    string fileName = ("观景指数_" + publishTime + ".jpg");
                    ftp.Upload(fontpath, fileName);
                }
                catch (Exception ex)
                {
                    isSucess = "失败";
                    WriteLog(user, "观景指数等级预报", period + "时微博发布", isSucess);
                    return "FTP服务器异常！";
                }
            }
            catch (Exception ex)
            {
                isSucess = "失败";
                WriteLog(user, "观景指数等级预报", period + "时微博发布", isSucess);
                return ex.Message;
            }
            WriteLog(user, "观景指数等级预报", period + "时微博发布", isSucess);
            return "OK";
        }


        private string GetText(string text, int maxCount)
        {
            string yl = text;
            string[] yls = yl.Replace(',', '，').Split('，');
            string result = text;
            int count = text.Replace(',', '，').Length;
            try
            {
                if (count > maxCount)
                {
                    //超出文本长度就找最近的标点符合短句
                    text = text.Replace(',', '，').Substring(0, maxCount);
                    string[] values = text.Split('，');
                    //判断下最后一个
                    string v1 = values[values.Length - 1];
                    string v2 = yls[values.Length - 1];
                    if (v1 != v2)
                    {
                        result = "";
                        for (int i = 0; i < (values.Length - 1); i++)
                        {
                            result += (values[i] + '，');
                        }
                    }
                    else
                    {
                        result = "";
                        int index = 0;
                        foreach (string v in values)
                        {
                            if (index < values.Length)
                                result += (v + '，');

                            index++;
                        }
                    }
                }
            }
            catch { }
            return result.TrimEnd('，').TrimEnd('；').TrimEnd(';') + '；';
        }

        public string DownloadWB(string period, string type)
        {
            Database m_database = new Database("DBCONFIG");
            string forecastDates = DateTime.Now.ToString("yyyy-MM-dd ") + period + ":00:00";
            string pd = "0";
            if (period == "20")
                pd = "24";

            //获取数据源
            string sqls = " select ForecastDate,PERIOD,GuideLines1,GuideLines2,Type,Grade,station " +
                          "   from T_ScenAirIndex where Type='综合观景指数' " +
                          "   and ForecastDate='" + forecastDates + "' and PERIOD='" + pd + "'";

            Database db = new Database();
            DataTable dt = db.GetDataTable(sqls);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return "请保存数据后发布";
            }

            try
            {
                string[] sites = { "10007A", "10015A", "10016A", "10017A", "10008A", 
                                   "10011A", "10004A", "10006A", "10018A", "10003A",
                                   "10012A", "10010A", "10009A", "10013A", "10014A", 
                                   "10001A", "10002A", "10005A" };

                string[] sitesCode = { "ChenShan", "HaiWan", "DongPing", "XiSha", "FengJing", 
                                       "ZhuJia", "JinJiang", "HuanLe", "DSN", "XianHua",
                                       "MaLu", "LangXia", "jinshan", "DongFang", "BiHai", 
                                       "YeSheng", "ShiJi", "JinLuo" };

                //float[,] sitesXY = {  { 1182, 540 },{ 1317, 545 },{ 1457, 254 },{ 1382, 226 },
                //                  { 1277, 619 },{ 1212, 415 },{ 1327, 484 },{ 1238, 489 },
                //                  { 1442, 480 },{ 1407, 500 },{ 1217, 320 },{ 1192, 600 },
                //                  { 1242, 586 },{ 1142, 460 },{ 1407, 555 },{ 1492, 545 },
                //                  { 1462.5f, 526 },{ 1297, 310 }};
                float[,] sitesXY = {  { 1202, 490 },{ 1408, 565 },{ 1322, 186 },{ 1227, 145 },
                                  { 1137, 571 },{ 1172, 434 },{ 1282, 446 },{ 1217, 459},
                                  { 1412, 441 },{ 1493, 546 },{ 1237, 320 },{ 1193, 601 },
                                  { 1262, 634 },{ 1123, 471 },{ 1347, 585 },{ 1443, 480 },
                                  { 1373, 400 },{ 1278, 311 }};

                string p = AppDomain.CurrentDomain.BaseDirectory.ToString();
                string imgPath = Path.Combine(p, "weibo\\weibo.jpg");
                string iconPath = Path.Combine(p, "weibo\\img");

                System.Drawing.Image imgSrc = System.Drawing.Image.FromFile(imgPath);
                int i = 0;
                int y = 0, y2 = 0, y3 = 0, y4 = 0, y5 = 85, y6 = 730;
                foreach (string site in sites)
                {
                    DataRow[] rows = dt.Select("station='" + site + "'");
                    string startCount = GetStar(rows[0]["Grade"].ToString());
                    string guidenLine1 = GetText(rows[0]["GuideLines1"].ToString(), 14);
                    string guidenLine2 = GetText(rows[0]["GuideLines2"].ToString(), 34);
                    string publishTime = DateTime.Parse(rows[0]["ForecastDate"].
                                          ToString()).ToString("yyyy年MM月dd日HH时");
                    string forecastDate = DateTime.Parse(rows[0]["ForecastDate"].
                                         ToString()).AddHours(int.Parse(rows[0]["Period"].ToString())).ToString("yyyy年MM月dd日");

                    using (Graphics g = Graphics.FromImage(imgSrc))
                    {
                        #region 画星级
                        using (System.Drawing.Font f = new System.Drawing.Font("微软雅黑", 10))
                        {
                            using (Brush b = new SolidBrush(Color.FromArgb(51, 51, 51)))
                            {
                                y = 40 + y;
                                if (y == 40)
                                    y = 45;

                                g.DrawString(startCount, f, b, 310, y);
                            }
                        }
                        #endregion
                        #region 画矩形
                        y2 = 40 + y2;
                        if (y2 == 40) { y2 = 39; }
                        if (i >= 14)
                            y2 = y2 + 1;
                        if (i >= 16)
                            y2 = y2 - 1;

                        Rectangle rectNew = new Rectangle(383, y2, 5, 31);
                        System.Drawing.Drawing2D.LinearGradientBrush brush = new
                            System.Drawing.Drawing2D.LinearGradientBrush(rectNew,
                            GetStarColor(rows[0]["Grade"].ToString()), GetStarColor(rows[0]["Grade"].ToString()),
                            System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
                        g.FillRectangle(brush, rectNew);
                        #endregion
                        #region 画指引1
                        using (System.Drawing.Font f = new System.Drawing.Font("微软雅黑", 10))
                        {
                            using (Brush b = new SolidBrush(Color.FromArgb(51, 51, 51)))
                            {
                                y3 = 40 + y3;
                                if (y3 == 40)
                                    y3 = 45;

                                g.DrawString(guidenLine1, f, b, 390, y3);
                            }
                        }
                        #endregion
                        #region 画指引2
                        using (System.Drawing.Font f = new System.Drawing.Font("微软雅黑", 10))
                        {
                            using (Brush b = new SolidBrush(Color.FromArgb(51, 51, 51)))
                            {
                                y4 = 40 + y4;
                                if (y4 == 40)
                                    y4 = 45;

                                g.DrawString(guidenLine2, f, b, 580, y4);
                            }
                        }
                        #endregion
                        #region 预报时间
                        using (System.Drawing.Font f = new System.Drawing.Font("宋体", 16, FontStyle.Regular))
                        {
                            using (Brush b = new SolidBrush(Color.Black))
                            {
                                g.DrawString("预报时间：" + forecastDate, f, b, 1069, y5);
                            }
                        }
                        #endregion
                        #region 发布时间
                        using (System.Drawing.Font f = new System.Drawing.Font("宋体", 11, FontStyle.Regular))
                        {
                            using (Brush b = new SolidBrush(Color.Black))
                            {
                                g.DrawString("上海中心气象台" + publishTime + "发布", f, b, 1169, y6);
                            }
                        }
                        #endregion
                        #region 画箭头
                        Image IMG = Image.FromFile(Path.Combine(iconPath, sitesCode[i] + "-" + rows[0]["Grade"].ToString() + ".png"));
                        IMG = resizeImage(new Bitmap(IMG), new Size(25, 38));
                        g.DrawImage(IMG, sitesXY[i, 0], sitesXY[i, 1]);
                        #endregion
                    }
                    i++;
                }
                string fontpath = Path.Combine(p, "weibo\\wb.jpg");
                System.Drawing.Imaging.EncoderParameters ep = new System.Drawing.Imaging.EncoderParameters();
                ep.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.MimeType == "image/jpeg")
                        ici = codec;
                }
                imgSrc.Save(fontpath, ici, ep);
                imgSrc.Dispose();
                return fontpath;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            // return fontpath;
        }

        private static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {
            ////获取图片宽度
            //int sourceWidth = imgToResize.Width;
            ////获取图片高度
            //int sourceHeight = imgToResize.Height;

            //float nPercent = 0;
            //float nPercentW = 0;
            //float nPercentH = 0;
            ////计算宽度的缩放比例
            //nPercentW = ((float)size.Width / (float)sourceWidth);
            ////计算高度的缩放比例
            //nPercentH = ((float)size.Height / (float)sourceHeight);

            //if (nPercentH < nPercentW)
            //    nPercent = nPercentH;
            //else
            //    nPercent = nPercentW;
            //期望的宽度
            int destWidth = size.Width; //(int)(sourceWidth * nPercent);
            //期望的高度
            int destHeight = size.Height;// (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            //g.InterpolationMode = InterpolationMode.;
            //绘制图像
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }

        public Color GetStarColor(string grade)
        {
            Color c = Color.Gray;
            switch (grade)
            {
                case "1": c = Color.FromArgb(85, 255, 0); break;
                case "2": c = Color.FromArgb(255, 255, 0); break;
                case "3": c = Color.FromArgb(255, 170, 0); break;
                case "4": c = Color.FromArgb(255, 0, 0); break;
            }
            return c;
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="user">当前登录用户</param>
        /// <param name="module">模块</param>
        /// <param name="explain">操作说明</param>
        /// <param name="status">操作是否成功</param>
        public void WriteLog(string user, string module, string explain, string status)
        {
            Database m_database = new Database("DBCONFIG");
            DateTime dNow = DateTime.Now;
            // explain = explain.Split(':')[0];
            string date = dNow.ToString("yyyy-MM-dd HH:mm:ss");
            string sqlInsert = "insert into t_ScenAirlog (Operator,OpDate,OpModule,OpExplain,Status) values " +
                "('" + user + "','" + date + "','" + module + "','" + explain + "','" + status + "')";
            m_database.Execute(sqlInsert);
        }
        /// <summary>
        /// 获取日志
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="people">操作人</param>
        /// <param name="fun">操作模块</param>
        /// <param name="status">是否成功</param>
        /// <returns></returns>
        public string QueryLog(string startTime, string endTime, string people, string fun, string status)
        {
            people = people == "全部" ? "1 = 1" : "Operator = '" + people + "'";
            fun = fun == "全部" ? "1 = 1" : "OpModule = '" + fun + "'";
            status = status == "全部" ? "1 = 1" : "Status = '" + status + "'";
            string sql = "select Operator,OpDate,OpModule,OpExplain,Status from T_ScenAirLog where " + people + " and " + fun + "" +
                "and " + status + " and OpDate between '" + startTime + "' and '" + endTime + "' order by OpDate desc";
            DataTable dt = m_Database.GetDataTable(sql);
            return DataTableToJson("data", dt);
        }

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
    }
}

