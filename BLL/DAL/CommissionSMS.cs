#region The ComForecast Copyright & Version History
/*
 * ============================================================== 
 * 
 * ComForecast, Version 1.0
 * 
 * Copyright (c) 2013-2014 上海地听信息科技有限公司.  版权所有.
 * 
 * 张伟锋
 * 
 * 修改：
 *       
 * 张伟锋              2010年11月25日
 * ====================================================================
 * 
 * 功能说明：用户实现环境监测中心的综合预报业务，包括预报单的录入，历史预报信息的调取等。
 *
 */
#endregion
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
namespace MMShareBLL.DAL
{
    public class CommissionSMS
    {
        protected static readonly log4net.ILog m_Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Database m_Database;
        private int m_BackDays;
        public CommissionSMS()
        {
            m_Database = new Database();
            m_BackDays = int.Parse(ConfigurationManager.AppSettings["BackDays"]);

        }
        /// <summary>
        /// 根据当前传入的时间，获取综合预报的起报时间
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        private DateTime GetManualForecastDate(string hour)
        {
            DateTime dtNow = DateTime.Now.Date.AddHours(18);
            if (hour != "")
                dtNow = DateTime.Parse(hour).AddHours(18);

            return dtNow;
        }

        /// <summary>
        /// 返回臭氧前驱体的数据
        /// </summary>
        /// <param name="fromDate">开始日期</param>
        /// <param name="toDate">结束日期</param>
        /// <param name="SiteID">站点编号</param>
        /// <returns></returns>
        public string GetOzoneChart(string fromDate, string toDate, string siteID)
        {
            string strSQL = "";
            string strReturn = "";
            string x = "";
            string y = "";
            double minX = 1000000000000;
            string from = DateTime.Parse(fromDate).ToString("yyyy-MM-dd HH:mm:ss");
            string to = DateTime.Parse(toDate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
            string strWhere = " [LST] BETWEEN '" + from + "' AND '" + to + "' AND SiteID='" + siteID + "' AND durationid=10";
            string dateFiled = " DATEDIFF(S,'1970-01-01 00:00:00', [LST]) AS [LST] ";
            //212：烷烃 213：烯烃 214：芳香烃 215：总VOC  8：臭氧
            strSQL = "SELECT " + dateFiled + " ,VALUE from DMS_DATA WHERE parameterid =213 AND " + strWhere + " ORDER BY [LST] ";
            strSQL = strSQL + ";SELECT " + dateFiled + ",VALUE from DMS_DATA WHERE parameterid =214 AND " + strWhere + " ORDER BY [LST] ";
            strSQL = strSQL + ";SELECT " + dateFiled + ",VALUE from DMS_DATA WHERE parameterid =215 AND " + strWhere + " ORDER BY [LST] ";
            strSQL = strSQL + ";SELECT " + dateFiled + ",VALUE from DMS_DATA WHERE parameterid =216 AND " + strWhere + " ORDER BY [LST] ";
            strSQL = strSQL + ";SELECT " + dateFiled + ",VALUE*1000 from DMS_DATA WHERE parameterid =8 AND " + strWhere + " ORDER BY [LST] ";
            try
            {
                DataSet ds = m_Database.GetDataset(strSQL);
                for (int index = 0; index < ds.Tables.Count; index++)
                {
                    DataTable dtElement = ds.Tables[index];
                    x = ""; y = "";
                    foreach (DataRow dr in dtElement.Rows)
                    {
                        x = x + "|" + dr[0].ToString();
                        if (double.Parse(dr[0].ToString()) < minX)
                            minX = double.Parse(dr[0].ToString());
                        y = y + "|" + dr[1].ToString();
                    }
                    strReturn = strReturn + ",'" + index.ToString() + "':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "'";
                }
                if (strReturn != ",")
                    strReturn = "{" + strReturn.TrimStart(',') + ",minX:" + minX.ToString() + "}";
                return strReturn;
            }
            catch (Exception ex)
            {
                m_Log.Error("GetOzoneChart", ex);
                return ex.ToString();
            }
        }


        /// <summary>
        /// 返回气溶胶曲线数据
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetAQIChart(string fromDate, string toDate, string station)
        {
            string strSQL = "";
            string strReturn = "";
            string x = "";
            string y = "";

            string from = DateTime.Parse(fromDate).ToString("yyyy-MM-dd HH:mm:ss");
            string to = DateTime.Parse(toDate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");

            
             //新的方法 现在需要禁掉，如果以后要可以启用
            //string strWhere = " [END] BETWEEN '" + DateTime.Parse(fromDate).AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + DateTime.Parse(toDate).AddDays(1).AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss") + "' AND STATION='" + station + "'";
            //string dateFiled = " DATEDIFF(S,'1970-01-01 00:00:00', DATEADD(hour,1,[END])) AS [END] ";

            //string dateFileAdd = "  DATEDIFF(S,'1970-01-01 00:00:00',DATEADD(hour,1,LST)) AS LST ";
            //string timeWhere = " LST BETWEEN '" + DateTime.Parse(fromDate).AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + DateTime.Parse(toDate).AddDays(1).AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss") + "'";

            //END  变成  Start  上面代码如果启用也要改过来
            string strWhere = " [Start] BETWEEN '" + from + "' AND '" + to + "' AND STATION='" + station + "'";
            string dateFiled = " DATEDIFF(S,'1970-01-01 00:00:00', [Start]) AS [END] ";

            string dateFileAdd = "  DATEDIFF(S,'1970-01-01 00:00:00', LST) AS LST ";
            string timeWhere = " LST BETWEEN '" + from + "' AND '" + to + "'";


            //Cl,NO3,SO4,Na,NH4,K,Mg,Ca,[PM2#5(ug/m3)]
           // if (station != "NJ")
            //{
                strSQL = "SELECT " + dateFiled + " ,Cl from T_AirPercent WHERE [Cl-quality]='V' AND " + strWhere + " ORDER BY [END] ";
                strSQL = strSQL + ";SELECT " + dateFiled + ",NO3 from T_AirPercent WHERE [NO3-quality]='V' AND " + strWhere + " ORDER BY [END] ";
                strSQL = strSQL + ";SELECT " + dateFiled + ",SO4 from T_AirPercent WHERE [SO4-quality]='V' AND " + strWhere + " ORDER BY [END] ";
                strSQL = strSQL + ";SELECT " + dateFiled + ",Na from T_AirPercent WHERE [Na-quality]='V' AND " + strWhere + " ORDER BY [END] ";
                strSQL = strSQL + ";SELECT " + dateFiled + ",NH4 from T_AirPercent WHERE [NH4-quality]='V' AND " + strWhere + " ORDER BY [END] ";
                strSQL = strSQL + ";SELECT " + dateFiled + ",K from T_AirPercent WHERE [K-quality]='V' AND " + strWhere + " ORDER BY [END] ";
                strSQL = strSQL + ";SELECT " + dateFiled + ",Mg from T_AirPercent WHERE [Mg-quality]='V' AND " + strWhere + " ORDER BY [END] ";
                strSQL = strSQL + ";SELECT " + dateFiled + ",Ca from T_AirPercent WHERE [Ca-quality]='V' AND " + strWhere + " ORDER BY [END] ";
           // }
            //else {
            //    strSQL = "SELECT '1970-01-01 00:00:00' ,-1  ";
            //    strSQL = strSQL + ";SELECT '1970-01-01 00:00:00',-1  ";
            //    strSQL = strSQL + ";SELECT '1970-01-01 00:00:00',-1  ";
            //    strSQL = strSQL + ";SELECT '1970-01-01 00:00:00',-1  ";
            //    strSQL = strSQL + ";SELECT '1970-01-01 00:00:00',-1  ";
            //    strSQL = strSQL + ";SELECT '1970-01-01 00:00:00',-1  ";
            //    strSQL = strSQL + ";SELECT '1970-01-01 00:00:00',-1  ";
            //    strSQL = strSQL + ";SELECT '1970-01-01 00:00:00',-1  ";
            //}
            string str = "";
            //56 OC(热学)  57 EC(热学) 58 OC(光学) 59 EC(光学) 228 浦东 浦东-崇明 271-青浦,OC需要用OC*1.4表示    张伟锋    2014-08-10
            if (station == "CM")
            {
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",(Value*1.4) AS Value  from DMS_DATA WHERE QCCode<>'9' and ParameterID=56  AND SiteID=228  AND DurationID=10  AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",Value  from DMS_DATA WHERE QCCode<>'9' and ParameterID=57  AND SiteID=228  AND DurationID=10  AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",(Value*1.4) AS Value  from DMS_DATA WHERE QCCode<>'9' and ParameterID=58  AND SiteID=228  AND DurationID=10  AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",Value  from DMS_DATA WHERE QCCode<>'9' and ParameterID=59  AND SiteID=228  AND DurationID=10  AND " + timeWhere + " ORDER BY LST ";
                str = "SELECT DATEDIFF(S,'1970-01-01 00:00:00', LST) AS [END] ,ROUND(Value*1000,1) as Value from Data_RT_Site WHERE LST BETWEEN '" + from + "' AND '" + to + "' AND  SiteID=249 AND AQIItemID=101 ORDER BY LST ";
            }
            if (station == "QP")
            {
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",(Value*1.4) AS Value  from DMS_DATA WHERE QCCode<>'9' and ParameterID=56  AND SiteID=271  AND DurationID=10   AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",Value  from DMS_DATA WHERE QCCode<>'9' and ParameterID=57  AND SiteID=271  AND DurationID=10   AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",Value  from DMS_DATA WHERE QCCode<>'9' and ParameterID=58  AND SiteID=271  AND DurationID=10   AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",(Value*1.4) AS Value  from DMS_DATA WHERE QCCode<>'9' and ParameterID=59  AND SiteID=271  AND DurationID=10   AND " + timeWhere + "  ORDER BY LST ";
                str = "SELECT DATEDIFF(S,'1970-01-01 00:00:00', LST) AS [END] ,ROUND(Value*1000,1) as Value from Data_RT_Site WHERE LST BETWEEN '" + from + "' AND '" + to + "' AND  SiteID=203 AND AQIItemID=101 ORDER BY LST ";
            }
            if (station == "NJ")
            {
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",(Value*1.4) AS Value  from T_OCECData WHERE ParameterID=56  AND SiteID=3110  AND DurationID=10   AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",Value  from T_OCECData WHERE ParameterID=57  AND SiteID=3110  AND DurationID=10   AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",Value  from T_OCECData WHERE ParameterID=58  AND SiteID=3110  AND DurationID=10   AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",(Value*1.4) AS Value  from T_OCECData WHERE ParameterID=59  AND SiteID=3110  AND DurationID=10   AND " + timeWhere + "  ORDER BY LST ";
                //str = "SELECT DATEDIFF(S,'1970-01-01 00:00:00', LST) AS [END] ,ROUND(Value*1000,1) as Value from Data_RT_Site WHERE LST BETWEEN '" + from + "' AND '" + to + "' AND  SiteID=203 AND AQIItemID=101 ORDER BY LST ";
                str = "SELECT DATEDIFF(S,'1970-01-01 00:00:00', TimePoint) AS [END] ,PM2_5 from [semc_dmc].[dbo].[China_RT_CNEMC_Data] WHERE TimePoint BETWEEN '" + from + "' AND '" + to + "' AND  Area = '南京' AND PositionName ='草场门' ORDER BY TimePoint ";
            }
            if (station == "PD")
            {
                //create by 薛辉 on 2014-08-14
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",(Value*1.4) AS Value  from DMS_DATA WHERE QCCode<>'9' and ParameterID=56  AND SiteID=228  AND DurationID=10   AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",Value  from DMS_DATA WHERE QCCode<>'9' and ParameterID=57  AND SiteID=228  AND DurationID=10   AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",Value  from DMS_DATA WHERE QCCode<>'9' and ParameterID=58  AND SiteID=228  AND DurationID=10   AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",(Value*1.4) AS Value  from DMS_DATA WHERE QCCode<>'9' and ParameterID=59  AND SiteID=228  AND DurationID=10   AND " + timeWhere + "  ORDER BY LST ";
                str = "SELECT DATEDIFF(S,'1970-01-01 00:00:00', LST) AS [END] ,ROUND(Value*1000,1) as Value from Data_RT_Site WHERE LST BETWEEN '" + from + "' AND '" + to + "' AND  SiteID=228 AND AQIItemID=101 ORDER BY LST ";
            }
            Database m_DatabaseAQI = new Database("AQIWEB");
            DataTable dt = m_DatabaseAQI.GetDataTable(str);
            try
            {
                DataSet ds = m_Database.GetDataset(strSQL);
                ds.Tables.Add(dt);
                for (int index = 0; index < ds.Tables.Count; index++)
                {
                    DataTable dtElement = ds.Tables[index];
                    x = ""; y = "";
                    foreach (DataRow dr in dtElement.Rows)
                    {
                        x = x + "|" + dr[0].ToString();
                        y = y + "|" + dr[1].ToString();
                    }
                    strReturn = strReturn + ",'" + index.ToString() + "':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "'";
                }
                if (strReturn != ",")
                    strReturn = "{" + strReturn.TrimStart(',') + "}";
                return strReturn;
            }
            catch (Exception ex)
            {
                m_Log.Error("GetAQIChart", ex);
                return ex.ToString();
            }
        }


        private double GetPeriod(DateTime dt)
        {
            double period = 6;
            if (dt.Hour == 6)
                period = 6;
            else if (dt.Hour == 12)
                period = 8;
            else if (dt.Hour == 20)
                period = 10;

            return period;
        }

        private string GetLineStr(DataTable dt, string index)
        {
            string x = "";
            string y = "";
            string z = "";
            DateTime minTime = DateTime.Now;
            DateTime nextTime;
            double lastx = 0;
            double dperiod = 6;
            int times = 0;
            string strReturn = "";
            if (dt.Rows.Count >= 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (times == 0)
                    {
                        lastx = double.Parse(dr[0].ToString());
                        x = x + "|" + lastx.ToString();
                        y = y + "|" + (dr[1].ToString() == "" ? " " : dr[1].ToString());
                        z = z + "|" + dr[2].ToString();
                        minTime = DateTime.Parse(dr[3].ToString());
                        dperiod = GetPeriod(minTime);
                    }
                    else
                    {
                        nextTime = DateTime.Parse(dr[3].ToString());
                        while (minTime.AddHours(dperiod) < nextTime)
                        {
                            lastx = lastx + dperiod * 3600;
                            x = x + "|" + lastx.ToString();
                            y = y + "|" + " ";
                            z = z + "|" + " ";
                            minTime = minTime.AddHours(dperiod);
                            dperiod = GetPeriod(minTime);
                        }
                        lastx = double.Parse(dr[0].ToString());
                        x = x + "|" + lastx.ToString();
                        y = y + "|" + (dr[1].ToString() == "" ? " " : dr[1].ToString());
                        z = z + "|" + dr[2].ToString();
                        minTime = nextTime;
                        dperiod = GetPeriod(minTime);
                    }
                    times++;
                }
                strReturn = ",'" + index + "':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "*" + z.TrimStart('|') + "'";
            }
            return strReturn;
        }

        /// <summary>
        /// 返回AQI对比数据
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="typeID"></param>
        /// <returns></returns>
        public string GetAQICompare(string fromDate, string toDate, string typeID, string period, string itemID)
        {
            string strSQL = "";
            string strReturn = "";
            DataTable dt;
            string from = DateTime.Parse(fromDate).ToString("yyyy-MM-dd HH:mm:ss");
            string to = DateTime.Parse(toDate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");

            strSQL = "SELECT  MAX(ForecastDate) AS ForecastDate FROM T_ForecastGroup WHERE LST BETWEEN '" + from + "' AND '" + to + "'";
            dt = m_Database.GetDataTable(strSQL);
            string maxForecast = dt.Rows[0][0].ToString();
            string strWhere = " AND LST BETWEEN '" + from + "' AND '" + to + "'";
            string dateFiled = "  DATEDIFF(S,'1970-01-01 00:00:00', LST) AS LST ";
            string period1 = "";
            if (period == "24")
                period1 = "48";
            else
                period1 = "72";
            Dictionary<string, string> dpoint = new Dictionary<string, string>();
            string queryField = ",VALUE,PARAMETER,LST AS BJTIME";
            //是AQI的时候查询的内容变成
            if (itemID == "0")
                queryField = ",AQI,PARAMETER,LST AS BJTIME";

            try
            {
                string strStr = "  UNION SELECT " + dateFiled + queryField + " FROM T_ForecastGroup WHERE ITEMID=" + itemID + " AND durationID IN(2,3,6) ";
                //实测（T_ObsDataGroup）
                if (typeID.IndexOf("0") >= 0)
                {
                    strSQL = "SELECT " + dateFiled + queryField + "  FROM  T_ObsDataGroup WHERE ITEMID=" + itemID + " AND durationID IN(2,3,6) " + strWhere + " ORDER BY LST ASC";
                    dt = m_Database.GetDataTable(strSQL);
                    strReturn = strReturn + GetLineStr(dt, "0");
                }

                if (typeID.IndexOf("1") >= 0)
                {
                    //终合and  not VALUE is null
                    strSQL = "SELECT " + dateFiled + queryField + " FROM T_ForecastGroup WHERE ITEMID=" + itemID + " AND durationID IN(2,3,6) AND PERIOD="
                        + period + " AND module = 'manual' " + strWhere + strStr + strWhere + " AND module = 'manual'  AND PERIOD=" + period1 + "  AND ForecastDate= '" + DateTime.Parse(maxForecast).ToString("yyyy/M/d 18:00") + "'  ORDER BY LST ASC";
                    dt = m_Database.GetDataTable(strSQL);
                    strReturn = strReturn + GetLineStr(dt, "1");
                }

                if (typeID.IndexOf("2") >= 0)
                {
                    //WRF
                    strSQL = "SELECT " + dateFiled + queryField + " FROM T_ForecastGroup WHERE ITEMID=" + itemID + " AND durationID IN(2,3,6) AND PERIOD="
                        + period + " AND module ='WRF' " + strWhere + strStr + strWhere + " AND module = 'WRF'  AND PERIOD=" + period1 + "   AND ForecastDate= '" + DateTime.Parse(maxForecast).ToString("yyyy/M/d 0:00") + "'  ORDER BY LST ASC";
                    dt = m_Database.GetDataTable(strSQL);
                    strReturn = strReturn + GetLineStr(dt, "2");
                }

                if (typeID.IndexOf("3") > 0)
                {
                    //数值CMAQ
                    strSQL = "SELECT " + dateFiled + queryField + " FROM T_ForecastGroup WHERE ITEMID=" + itemID + " AND durationID IN(2,3,6) AND PERIOD="
                        + period + " AND module ='CMAQ' " + strWhere + strStr + strWhere + " AND module = 'CMAQ'  AND PERIOD=" + period1 + "   AND ForecastDate= '" + DateTime.Parse(maxForecast).ToString("yyyy/M/d 20:00") + "' ORDER BY LST ASC";
                    dt = m_Database.GetDataTable(strSQL);
                    strReturn = strReturn + GetLineStr(dt, "3");
                }
                if (strReturn != ",")
                    strReturn = "{" + strReturn.TrimStart(',') + "}";
                return strReturn;
            }
            catch (Exception ex)
            {
                m_Log.Error("GetAQICompare", ex);
                return ex.ToString();
            }
        }



        /// <summary>
        /// 返回PM2.5和臭氧的数据
        /// </summary>
        /// <param name="forecastDate">预报时效</param>
        /// <returns></returns>
        public string GetForecastCompare(string forecastDate, string forecastToDate)
        {
            string strSQL = "";
            string strReturn = "";
            string x = "";
            string y = "";
            string z = "";
            DateTime maxPeriod;
            string forecastMax;
            string strWhere = "";
            DataTable dt;

            strSQL = "SELECT LST,ForecastDate  FROM  T_ForecastSite WHERE (ITEMID = 1 OR  ITEMID = 4) AND durationID =10 AND SiteID =0 AND PERIOD=24 AND LST in (SELECT MAX(LST) AS maxLST FROM  T_ForecastSite WHERE (ITEMID = 1 OR  ITEMID = 4) AND durationID =10 AND SiteID =0 AND PERIOD=24)";
            dt = m_Database.GetDataTable(strSQL);
            maxPeriod = DateTime.Parse(dt.Rows[0][0].ToString());
            forecastMax = DateTime.Parse(dt.Rows[0][1].ToString()).ToString("yyyy-MM-dd HH:mm:ss");

            string forecast = DateTime.Parse(forecastDate).ToString("yyyy-MM-dd HH:mm:ss");
            DateTime forecastTo = DateTime.Parse(forecastToDate);
            string dateFiled = "  DATEDIFF(S,'1970-01-01 00:00:00', LST) AS LST ";
            string queryField = ",VALUE";
            strWhere = " AND LST BETWEEN  '" + forecast + "' AND '" + forecastTo.ToString("yyyy-MM-dd 23:00:00") + "'";
            if (forecastTo > maxPeriod || forecastTo.ToString("yyyy-MM-dd") == maxPeriod.ToString("yyyy-MM-dd"))
            {
                strSQL = "SELECT " + dateFiled + queryField + " FROM T_ForecastSite WHERE ITEMID=1 AND durationID =10 AND SiteID =0 AND PERIOD=24 "
                     + " AND module ='WRF' " + strWhere + " UNION SELECT " + dateFiled + queryField + " FROM T_ForecastSite WHERE ITEMID=1 AND durationID =10 AND SiteID =0 AND module ='WRF' AND ForecastDate='" + forecastMax + "' AND  LST  BETWEEN  '" + maxPeriod + "' AND '" + forecastTo.ToString("yyyy-MM-dd 23:00:00") + "' ORDER BY LST ASC";
            }
            else
            {
                strSQL = "SELECT " + dateFiled + queryField + " FROM T_ForecastSite WHERE ITEMID=1 AND durationID =10 AND SiteID =0 AND PERIOD=24 " + " AND module ='WRF' " + strWhere + " ORDER BY LST ASC";
            }
            //PM2.5

            dt = m_Database.GetDataTable(strSQL);
            x = ""; y = ""; z = "";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    x = x + "|" + dr[0].ToString();
                    y = y + "|" + (dr[1].ToString() == "" ? " " : dr[1].ToString());
                    z = z + "|" + "";
                }
                strReturn = strReturn + ",'0':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "*" + z.TrimStart('|') + "'";
            }

            //臭氧
            if (forecastTo > maxPeriod)
            {
                strSQL = "SELECT " + dateFiled + queryField + " FROM T_ForecastSite WHERE ITEMID=4 AND durationID =10 AND SiteID =0 AND PERIOD=24 "
                     + " AND module ='WRF' " + strWhere + " UNION SELECT " + dateFiled + queryField + " FROM T_ForecastSite WHERE ITEMID=4 AND durationID =10 AND SiteID =0 AND module ='WRF' AND ForecastDate='" + forecastMax + "' AND   LST  BETWEEN  '" + maxPeriod + "' AND '" + forecastTo.ToString("yyyy-MM-dd 23:00:00") + "' ORDER BY LST ASC";
            }
            else
            {
                strSQL = "SELECT " + dateFiled + queryField + " FROM T_ForecastSite WHERE ITEMID=4 AND durationID =10 AND SiteID =0 AND PERIOD=24 " + " AND module ='WRF' " + strWhere + " ORDER BY LST ASC";
            }
            dt = m_Database.GetDataTable(strSQL);
            x = ""; y = ""; z = "";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    x = x + "|" + dr[0].ToString();
                    y = y + "|" + (dr[1].ToString() == "" ? " " : dr[1].ToString());
                }
                strReturn = strReturn + ",'1':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "*" + z.TrimStart('|') + "'";
            }

            if (strReturn != "," && strReturn != "")
                strReturn = "{" + strReturn.TrimStart(',') + "}";
            return strReturn;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        public string GetForecast(string flag, string hour, string period, string Module, string tag, string moduleStyle)
        {
            //获取起报时间，如果传入的时间为空，那么自动以当前时间为起报时间
            DateTime dtNow = GetManualForecastDate(hour);

            string toDate = dtNow.ToString("yyyy-MM-dd 23:59:59");
            string fromDate = dtNow.AddDays(-m_BackDays).ToString("yyyy-MM-dd");

            //创建综合预报表单实体
            Entity entity = new Entity(m_Database, "Forecast");
            string strSQL = string.Format("FORECASTDATE = '{0}'", dtNow);//起报时间是每天的18点
            strSQL = entity.BuildQuerySQL(strSQL, "");

            //创建实况数据SQL
            if (flag == "2")//预报回顾标志
                strSQL = strSQL + ";" + CreateRealtimeReSeeSQL(fromDate, toDate);
            else
                strSQL = strSQL + ";" + CreateRealtimeSQL(fromDate, toDate);

            //创建参考综合预报SQL
            if (tag == "zhonghe")
            {
                if (flag == "2")
                    strSQL = strSQL + ";" + CreateComReSeeSQL(fromDate, toDate, period, Module);
                else
                    strSQL = strSQL + ";" + CreateComSQL(fromDate, toDate, period, Module);
            }
            else
                strSQL = strSQL + ";" + CreateComReviseSQL(fromDate, toDate, period, Module);

            //创建历史综合预报
            strSQL = strSQL + " UNION ALL " + CreatComforecastSQL(flag, dtNow.ToString("yyyy-MM-dd HH:00:00"), Module);


            //创建模式预报SQL，数值模式的起报时间是综合预报起报时间的前一天的北京时间20点
            strSQL = strSQL + ";" + CreateModuleSQL(dtNow, moduleStyle);
            try
            {
                DataSet dSet = m_Database.GetDataset(strSQL);
                StringBuilder sb = new StringBuilder("{");

                //创建表单json
                DataTable dTable = dSet.Tables[0];
                string json = GetForecastJSON(dTable);
                sb.Append(json);
                if (json != "")
                    sb.Append(",");

                //生成实况，综合预报，模式数据的json
                for (int i = 0; i < 3; i++)
                {
                    //创建json，便于前台赋值
                    dTable = dSet.Tables[i + 1];
                    json = GetGroupJSON(dTable, i, "H");//实况typeID = 0;//综合预报typeID = 1//模式typeID = 2

                    if (json != "")
                    {
                        sb.Append(json);
                        sb.Append(",");
                    }
                }
                sb.Append(string.Format("nowDateTime:'{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                if (sb.Length > 1)
                {
                    sb.Append("}");
                }
                else
                    sb.Length = 0;

                return sb.ToString();
            }
            catch (Exception ex)
            {
                m_Log.Error("GetForecast", ex);
                return ex.ToString();
            }

        }
        public DataSet StrSQLString(string fromDate, string toDate, string period, string forecasPeriod, string dataType, string dataModule)
        {
            DateTime startDate = DateTime.Parse(fromDate);
            DateTime endDate = DateTime.Parse(toDate).AddDays(1);
            string[] durationID;
            string[] modualArray;
            int count = 0;
            string strSQL, durationIDSQL;
            string timeSQL = "LST BETWEEN  '" + startDate + "' AND '" + endDate + "' AND PERIOD =" + period + "";
            durationIDSQL = "LST BETWEEN  '" + startDate + "' AND '" + endDate + "'";
            if (forecasPeriod != "")
            {
                durationID = forecasPeriod.Split(',');
                count = durationID.Length;
                strSQL = "(";
                for (int i = 0; i < durationID.Length; i++)
                {
                    strSQL = strSQL + durationID[i] + ",";
                }
                strSQL = strSQL.Substring(0, strSQL.Length - 1) + ")";
                durationIDSQL = durationIDSQL + " AND durationID IN" + strSQL;
                timeSQL = timeSQL + " AND durationID IN" + strSQL;
            }

            if (dataModule != "")
            {
                modualArray = dataModule.Split(',');
                strSQL = "(";
                for (int i = 0; i < modualArray.Length; i++)
                {
                    strSQL = strSQL + "'" + modualArray[i] + "',";
                }
                strSQL = strSQL.Substring(0, strSQL.Length - 1) + ")";
                timeSQL = timeSQL + " AND Module  IN" + strSQL;

            }
            else
            {
                timeSQL = timeSQL + " AND Module  =''";
            }
            strSQL = "select T.*, B.MC,B.indexD from (SELECT LST, ForecastDate, PERIOD, Module, durationID, ITEMID, Value, AQI,Parameter FROM T_ForecastGroup WHERE " + timeSQL + ") T" + " INNER JOIN " + "(SELECT DM,MC,indexD FROM D_DurationTest)  B on  T.durationID=B.DM ORDER BY T.LST,B.indexD";

            DataTable dtForecastGroup = m_Database.GetDataTable(strSQL);
            DataTable dtShiceGroup = new DataTable();
            if (dataType != "")
            {
                strSQL = "SELECT LST,durationID, ITEMID, Value, AQI,Parameter FROM T_ObsDataGroup Where " + durationIDSQL + "ORDER BY ITEMID";
                dtShiceGroup = m_Database.GetDataTable(strSQL);
            }
            DataSet ds = tableUnion(dtForecastGroup, dtShiceGroup, startDate, endDate, forecasPeriod, period);
            DataTable dtAQI = tableAQI(dtForecastGroup, dtShiceGroup, startDate, endDate, forecasPeriod, period);
            ds.Tables.Add(dtAQI);
            return ds;
        }
        //根据选择条件创建数据表格
        public string GetFilterDataTables(string fromDate, string toDate, string period, string forecasPeriod, string dataType, string dataModule)
        {
            DataSet ds = StrSQLString(fromDate, toDate, period, forecasPeriod, dataType, dataModule);
            int count = 0;
            string[] durationID;
            if (forecasPeriod != "")
            {
                durationID = forecasPeriod.Split(',');
                count = durationID.Length;
            }
            string jsonString = tableString(ds, count);
            return jsonString;
        }

        /// <summary>
        /// 重载筛选
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="period"></param>
        /// <param name="forecasPeriod"></param>
        /// <param name="dataType"></param>
        /// <param name="dataModule"></param>
        /// <returns></returns>
        public string GetFilterDataTablesII(string fromDate, string toDate, string period, string forecasPeriod, string dataType, string dataModule)
        {
            DataSet ds = StrSQLString(fromDate, toDate, period, forecasPeriod, dataType, dataModule);
            int count = 0;
            string[] durationID;
            if (forecasPeriod != "")
            {
                durationID = forecasPeriod.Split(',');
                count = durationID.Length;
            }
            string jsonString = tableStringII(ds, count);
            return jsonString;
        }


        public DataTable tableAQI(DataTable dt, DataTable dm, DateTime startDate, DateTime endTime, string duration, string period)
        {
            DataTable dSearch = new DataTable("T_ForecastAQI");
            dSearch.Columns.Add("LST", typeof(string));
            dSearch.Columns.Add("sectionStr", typeof(string));
            dSearch.Columns.Add("timeSpan", typeof(string));
            dSearch.Columns.Add("PERIOD", typeof(string));
            dSearch.Columns.Add("shiceAQI", typeof(int));
            dSearch.Columns.Add("shiceParameter", typeof(string));
            dSearch.Columns.Add("comAQI", typeof(int));
            dSearch.Columns.Add("comParameter", typeof(string));
            dSearch.Columns.Add("CMAQAQI", typeof(int));
            dSearch.Columns.Add("CMAQParameter", typeof(string));
            dSearch.Columns.Add("WRFAQI", typeof(int));
            dSearch.Columns.Add("WRFParameter", typeof(string));
            string strFilter = "";
            string strSQL = "SELECT DM,MC,Description FROM D_DurationTest";
            DataTable timePeriod = m_Database.GetDataTable(strSQL);
            DataTable temp = dSearch.Clone();
            temp.TableName = string.Format("table{0}", 0);
            string[] durationItems = duration.Split(',');
            for (int j = 0; j < Math.Abs(endTime.Day - startDate.Day); j++)
            {
                if (duration != "")
                {
                    for (int k = 0; k < durationItems.Length; k++)
                    {
                        strFilter = string.Format("ITEMID={0} AND LST>='{1}' AND LST<'{2}' AND durationID={3}", 0, startDate.AddDays(j).ToString(), startDate.AddDays(j + 1).ToString(), int.Parse(durationItems[k].ToString()));
                        DataRow[] rows = dt.Select(strFilter);

                        if (rows.Length > 0)
                        {
                            DataRow dr = rows[0];
                            int duraID = int.Parse(durationItems[k].ToString());
                            DataRow newRow = temp.NewRow();
                            newRow[0] = DateTime.Parse(dr[0].ToString()).ToLongDateString();
                            newRow[3] = dr[2].ToString() + "小时";
                            string[] duratonSpan = GetDurationSpan(timePeriod, duraID);
                            string span = string.Format("{0}:00", int.Parse(duratonSpan[0])) + "-" + string.Format("{0}:00", int.Parse(duratonSpan[1]));
                            newRow[2] = span;
                            string Filter = string.Format("DM={0}", duraID);
                            DataRow[] rowDuation = timePeriod.Select(Filter);
                            newRow[1] = rowDuation[0][2].ToString();
                            foreach (DataRow dataR in rows)
                            {
                                string modualType = dataR[3].ToString();
                                switch (modualType)
                                {
                                    case "CMAQ":
                                        if (dataR[7].ToString() == "")
                                            newRow[8] = DBNull.Value;
                                        else
                                            newRow[8] = int.Parse(dataR[7].ToString());
                                        if (dataR[8].ToString() == "")
                                            newRow[9] = DBNull.Value;
                                        else
                                            newRow[9] = dataR[8].ToString();
                                        break;
                                    case "Manual":
                                        if (dataR[7].ToString() == "")
                                            newRow[6] = DBNull.Value;
                                        else
                                            newRow[6] = int.Parse(dataR[7].ToString());
                                        if (dataR[8].ToString() == "")
                                            newRow[7] = DBNull.Value;
                                        else
                                            newRow[7] = dataR[8].ToString();
                                        break;
                                    case "WRF":
                                        if (dataR[7].ToString() == "")
                                            newRow[10] = DBNull.Value;
                                        else
                                            newRow[10] = int.Parse(dataR[7].ToString());
                                        if (dataR[8].ToString() == "")
                                            newRow[11] = DBNull.Value;
                                        else
                                            newRow[11] = dataR[8].ToString();
                                        break;
                                }
                            }
                            if (dm.Rows.Count > 0)
                            {
                                DataRow[] shiceRow = dm.Select(strFilter);
                                if (shiceRow.Length > 0)
                                {
                                    string para = shiceRow[0][5].ToString();
                                    newRow[5] = para;
                                    int n4 = 0;
                                    int.TryParse(shiceRow[0][4].ToString(),out n4);
                                    newRow[4] = n4;
                                }

                            }
                            else
                            {
                                newRow[4] = DBNull.Value;
                                newRow[5] = DBNull.Value;
                            }
                            temp.Rows.Add(newRow);
                        }
                        else
                        {
                            if (dm.Rows.Count > 0)
                            {
                                DataRow[] shiceRow = dm.Select(strFilter);
                                if (shiceRow.Length > 0)
                                {
                                    DataRow dr = shiceRow[0];
                                    int duraID = int.Parse(durationItems[k].ToString());
                                    DataRow newRow = temp.NewRow();
                                    newRow[0] = DateTime.Parse(dr[0].ToString()).ToLongDateString();
                                    newRow[3] = period + "小时";
                                    string[] duratonSpan = GetDurationSpan(timePeriod, duraID);
                                    string span = string.Format("{0}:00", int.Parse(duratonSpan[0])) + "-" + string.Format("{0}:00", int.Parse(duratonSpan[1]));
                                    newRow[2] = span;
                                    string Filter = string.Format("DM={0}", duraID);
                                    DataRow[] rowDuation = timePeriod.Select(Filter);
                                    newRow[1] = rowDuation[0][2].ToString();

                                    string value = dr[5].ToString();
                                    newRow[5] = value;
                                    newRow[4] = int.Parse(dr[4].ToString());
                                    temp.Rows.Add(newRow);
                                }

                            }

                        }

                    }
                }
            }

            return temp;

        }
        public string tableString(DataSet temp, int count)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < temp.Tables.Count; i++)
            {
                DataTable dt = temp.Tables[i];
                if (dt.TableName == "table0")
                {
                    sb.AppendLine("<table id='table0'  width='100%' border='0' cellpadding='0' cellspacing='0' class='tablekuang'>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td class='tabletitle' rowspan='2'>日期</td>");
                    sb.AppendLine("<td class='tabletitle' rowspan='2'>时段名称</td>");
                    sb.AppendLine("<td class='tabletitle' rowspan='2'>时段区间</td>");
                    sb.AppendLine("<td class='tabletitle' rowspan='2'>预报时效</td>");
                    sb.AppendLine("<td class='tabletitle' colspan='2'>实测</td>");
                    sb.AppendLine("<td class='tabletitle' colspan='2'>综合预报</td>");
                    sb.AppendLine("<td class='tabletitle' colspan='2'>CMAQ</td>");
                    sb.AppendLine("<td class='tabletitle' colspan='2'>WRF-CHEM</td>");
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td class='tabletitle'>AQI</td>");
                    sb.AppendLine("<td class='tabletitle'>首要污染物</td>");
                    sb.AppendLine("<td class='tabletitle'>AQI</td>");
                    sb.AppendLine("<td class='tabletitle'>首要污染物</td>");
                    sb.AppendLine("<td class='tabletitle'>AQI</td>");
                    sb.AppendLine("<td class='tabletitle'>首要污染物</td>");
                    sb.AppendLine("<td class='tabletitle'>AQI</td>");
                    sb.AppendLine("<td class='tabletitle'>首要污染物</td>");
                    sb.AppendLine("</tr>");

                }
                else
                {
                    sb.AppendLine(string.Format("<table id='{0}' width='100%' border='0' cellpadding='0' cellspacing='0' class='tablekuang'>", dt.TableName));
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td class='tabletitle' rowspan='2'>日期</td>");
                    sb.AppendLine("<td class='tabletitle' rowspan='2'>时段名称</td>");
                    sb.AppendLine("<td class='tabletitle' rowspan='2'>时段区间</td>");
                    sb.AppendLine("<td class='tabletitle' rowspan='2'>预报时效</td>");
                    switch (dt.TableName)
                    {
                        case "table1":
                            sb.AppendLine("<td class='tabletitle' colspan='4'>PM2.5浓度</td>");
                            sb.AppendLine("<td class='tabletitle' colspan='4'>PM2.5AQI</td>");
                            break;
                        case "table2":
                            sb.AppendLine("<td class='tabletitle' colspan='4'>PM10浓度</td>");
                            sb.AppendLine("<td class='tabletitle' colspan='4'>PM10AQI</td>");
                            break;
                        case "table3":
                            sb.AppendLine("<td class='tabletitle' colspan='4'>NO2浓度</td>");
                            sb.AppendLine("<td class='tabletitle' colspan='4'>NO2AQI</td>");
                            break;
                        case "table4":
                            sb.AppendLine("<td class='tabletitle' colspan='4'>03-1h浓度</td>");
                            sb.AppendLine("<td class='tabletitle' colspan='4'>03-1hAQI</td>");
                            break;
                        case "table5":
                            sb.AppendLine("<td class='tabletitle' colspan='4'>03-8h浓度</td>");
                            sb.AppendLine("<td class='tabletitle' colspan='4'>03-8hAQI</td>");
                            break;

                    }
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td class='tabletitle'>实测</td>");
                    sb.AppendLine("<td class='tabletitle'>综合预报</td>");
                    sb.AppendLine("<td class='tabletitle'>CMAQ</td>");
                    sb.AppendLine("<td class='tabletitle'>WRF-CHEM</td>");
                    sb.AppendLine("<td class='tabletitle'>实测</td>");
                    sb.AppendLine("<td class='tabletitle'>综合预报</td>");
                    sb.AppendLine("<td class='tabletitle'>CMAQ</td>");
                    sb.AppendLine("<td class='tabletitle'>WRF-CHEM</td>");
                    sb.AppendLine("</tr>");

                }
                int m = 0;
                int k = 0;
                AQIExtention aqiExt;
                foreach (DataRow dr in dt.Rows)
                {
                    k++;
                    string tableName = dt.TableName.ToString();
                    int items = int.Parse(tableName.Substring(5, 1));
                    sb.AppendLine(string.Format("<tr  onmouseover='mouseOver(this)' onmouseout='mouseOut(this)' id='{0}'>", tableName + k.ToString()));
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j == 0)
                        {
                            if (m == 0 || m % count == 0)
                                sb.AppendLine(string.Format("<td class='tablerow'  rowspan='{1}' id='{2}{3}{4}'>{0}</td>", dr[j].ToString(), count, tableName, k, j));
                        }
                        else
                        {
                            if ((items == 0 && (j == 4 || j == 6 || j == 8 || j == 10)) || ((j == 9 || j == 8 || j == 10 || j == 11) && items != 0))
                            {
                                if (dr[j].ToString() != "")
                                {
                                    aqiExt = new AQIExtention(int.Parse(dr[j].ToString()), items);
                                    string aqiColor = string.Format("class='{0}'", aqiExt.Color);
                                    sb.AppendLine(string.Format("<td class='tablerow' id='{2}{3}{4}'><span {0}>{1}</span></td>", aqiColor, int.Parse(dr[j].ToString()), tableName, k, j));

                                }
                                else
                                {
                                    string value = dr[j].ToString() == "" ? "/ " : dr[j].ToString();
                                    sb.AppendLine(string.Format("<td class='tablerow' id='{1}{2}{3}'>{0}</td>", value, tableName, k, j));
                                }

                            }
                            else
                            {
                                string value = dr[j].ToString() == "" ? "/ " : dr[j].ToString();
                                sb.AppendLine(string.Format("<td class='tablerow' id='{1}{2}{3}'>{0}</td>", value, tableName, k, j));
                            }

                        }

                    }
                    sb.AppendLine("</tr>");
                    m++;

                }
                sb.AppendLine("</table>|");
            }
            string json = sb.ToString();
            int posi = json.LastIndexOf("|");
            string returnJson = json.Substring(0, posi);
            return returnJson;
        }




        public DataSet tableUnion(DataTable dt, DataTable dm, DateTime startDate, DateTime endTime, string durationID, string period)
        {

            DataTable dSearch = new DataTable("T_ForecastSearch");
            dSearch.Columns.Add("LST", typeof(string));
            dSearch.Columns.Add("sectionStr", typeof(string));
            dSearch.Columns.Add("timeSpan", typeof(string));
            dSearch.Columns.Add("PERIOD", typeof(string));
            dSearch.Columns.Add("shiCeValue", typeof(double));
            dSearch.Columns.Add("comValue", typeof(double));
            dSearch.Columns.Add("CMAQValue", typeof(double));
            dSearch.Columns.Add("WRFValue", typeof(double));

            dSearch.Columns.Add("shiCeAQI", typeof(int));
            dSearch.Columns.Add("comAQI", typeof(int));
            dSearch.Columns.Add("CMAQAQI", typeof(int));
            dSearch.Columns.Add("WRFAQI", typeof(int));
            DataSet tmpTable = new DataSet();
            string strFilter = "";

            string strSQL = "SELECT DM,MC,Description FROM D_DurationTest";
            DataTable timePeriod = m_Database.GetDataTable(strSQL);
            for (int i = 1; i <= 5; i++)
            {

                DataTable temp = dSearch.Clone();
                temp.TableName = string.Format("table{0}", i);
                string[] durationItems = durationID.Split(',');
                TimeSpan ts = endTime - startDate;
                int s = ts.Days;
                for (int j = 0; j < Math.Abs(s); j++)
                {
                    if (durationID != "")
                    {
                        for (int k = 0; k < durationItems.Length; k++)
                        {
                            strFilter = string.Format("ITEMID={0} AND LST>='{1}' AND LST<'{2}' AND durationID={3}", i, startDate.AddDays(j).ToString(), startDate.AddDays(j + 1).ToString(), int.Parse(durationItems[k].ToString()));
                            DataRow[] rows = dt.Select(strFilter);

                            if (rows.Length > 0)
                            {
                                DataRow dr = rows[0];
                                int duraID = int.Parse(durationItems[k].ToString());
                                DataRow newRow = temp.NewRow();
                                newRow[0] = DateTime.Parse(dr[0].ToString()).ToLongDateString();
                                newRow[3] = dr[2].ToString() + "小时";
                                string[] duratonSpan = GetDurationSpan(timePeriod, duraID);
                                string span = string.Format("{0}:00", int.Parse(duratonSpan[0])) + "-" + string.Format("{0}:00", int.Parse(duratonSpan[1]));
                                newRow[2] = span;
                                string Filter = string.Format("DM={0}", duraID);
                                DataRow[] rowDuation = timePeriod.Select(Filter);
                                newRow[1] = rowDuation[0][2].ToString();
                                foreach (DataRow dataR in rows)
                                {
                                    string modualType = dataR[3].ToString();
                                    switch (modualType)
                                    {
                                        case "CMAQ":
                                            if (dataR[6].ToString() == "")
                                            {
                                                newRow[6] = DBNull.Value;
                                            }
                                            else
                                            {
                                                newRow[6] = Math.Round(double.Parse(dataR[6].ToString()), 1);
                                            }
                                            if (dataR[7].ToString() == "")
                                            {
                                                newRow[10] = DBNull.Value;
                                            }
                                            else
                                            {
                                                newRow[10] = int.Parse(dataR[7].ToString());
                                            }
                                            break;
                                        case "Manual":
                                            if (dataR[6].ToString() == "")
                                            {
                                                newRow[5] = DBNull.Value;

                                            }
                                            else
                                            {
                                                newRow[5] = Math.Round(double.Parse(dataR[6].ToString()), 1);
                                            }
                                            if (dataR[7].ToString() == "")
                                            {
                                                newRow[9] = DBNull.Value;

                                            }
                                            else
                                            {

                                                newRow[9] = int.Parse(dataR[7].ToString());
                                            }
                                            break;
                                        case "WRF":
                                            if (dataR[6].ToString() == "")
                                            {
                                                newRow[7] = DBNull.Value;
                                            }
                                            else
                                            {
                                                newRow[7] = Math.Round(double.Parse(dataR[6].ToString()), 1);
                                            }
                                            if (dataR[7].ToString() == "")
                                            {
                                                newRow[11] = DBNull.Value;
                                            }
                                            else
                                            {
                                                newRow[11] = int.Parse(dataR[7].ToString());
                                            }
                                            break;
                                    }
                                }
                                if (dm.Rows.Count > 0)
                                {
                                    DataRow[] shiceRow = dm.Select(strFilter);
                                    if (shiceRow.Length > 0)
                                    {
                                        double sc3 = 0d;
                                        double.TryParse(shiceRow[0][3].ToString(),out sc3);
                                        double value = sc3;
                                        newRow[4] = Math.Round(value, 1);
                                        int sc4 = 0;
                                        int.TryParse(shiceRow[0][4].ToString(),out sc4);
                                        newRow[8] = sc4;
                                    }

                                }
                                else
                                {
                                    newRow[4] = DBNull.Value;
                                    newRow[8] = DBNull.Value;
                                }
                                temp.Rows.Add(newRow);
                            }
                            else
                            {
                                if (dm.Rows.Count > 0)
                                {
                                    DataRow[] shiceRow = dm.Select(strFilter);
                                    if (shiceRow.Length > 0)
                                    {
                                        DataRow dr = shiceRow[0];
                                        int duraID = int.Parse(durationItems[k].ToString());
                                        DataRow newRow = temp.NewRow();
                                        newRow[0] = DateTime.Parse(dr[0].ToString()).ToLongDateString();
                                        newRow[3] = period + "小时";
                                        string[] duratonSpan = GetDurationSpan(timePeriod, duraID);
                                        string span = string.Format("{0}:00", int.Parse(duratonSpan[0])) + "-" + string.Format("{0}:00", int.Parse(duratonSpan[1]));
                                        newRow[2] = span;
                                        string Filter = string.Format("DM={0}", duraID);
                                        DataRow[] rowDuation = timePeriod.Select(Filter);
                                        newRow[1] = rowDuation[0][2].ToString();

                                        double sc3 = 0d;
                                        double.TryParse(shiceRow[0][3].ToString(), out sc3);
                                        double value = sc3;
                                        newRow[4] = Math.Round(value, 1);
                                        int sc4 = 0;
                                        int.TryParse(shiceRow[0][4].ToString(),out sc4);
                                        newRow[8] = sc4;
                                        temp.Rows.Add(newRow);
                                    }

                                }

                            }
                        }
                    }
                }
                tmpTable.Tables.Add(temp);
            }
            return tmpTable;
        }
        /// <summary>
        /// 根据模式名称获取模式预报的数据
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public string GetModuleForecast(string hour, string module)
        {
            ////创建模式预报SQL
            //获取起报时间，如果传入的时间为空，那么自动以当前时间为起报时间
            DateTime dtNow = GetManualForecastDate(hour);

            string strSQL = CreateModuleSQL(dtNow, module);

            DataTable dTable = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder("{");
            string json;
            //生成实况，综合预报，模式数据的json
            if (dTable.Rows.Count > 0)
            {
                //创建json，便于前台赋值
                json = GetGroupJSON(dTable, 2, "H");//实况typeID = 0;//综合预报typeID = 1//模式typeID = 2

                if (json != "")
                {
                    sb.Append(json);
                    sb.Append(",");
                }

            }

            if (sb.Length > 1)
            {
                sb.Remove(sb.Length - 1, 1);
                sb.Append("}");
            }
            else
                sb.Length = 0;

            return sb.ToString();

        }
        /// <summary>
        /// 根据日期和时效返回综合预报的数据
        /// </summary>
        /// <param name="hour">此日期包含在前台的顺序信息</param>
        /// <param name="period">24小时，48小时</param>
        /// <returns></returns>
        public string GetForecastByPeriod(string hour, string period, string Module)
        {
            //获取起报时间，如果传入的时间为空，那么自动以当前时间为起报时间
            string[] part = hour.Split(';');
            string strSQL = "";

            StringBuilder sb = new StringBuilder("{");
            for (int i = 0; i < part.Length - 1; i++)
            {
                string date = part[i];
                DateTime dtNow = DateTime.Parse(date);
                string toDate = dtNow.ToString("yyyy-MM-dd 23:59:59");
                string fromDate = dtNow.ToString("yyyy-MM-dd");


                strSQL = strSQL + CreateComSQL(fromDate, toDate, period, Module) + ";";
                if (part.Length == 2)
                    strSQL = strSQL + CreateRealtimeSQL(fromDate, toDate);


            }
            DataSet dSet = m_Database.GetDataset(strSQL);
            int typeID = 1;
            for (int j = 0; j < dSet.Tables.Count; j++)
            {
                if (part.Length == 2)
                {
                    m_BackDays = int.Parse(part[1]);
                    typeID = 1 - j;
                }
                else
                    m_BackDays = j;

                //创建json，便于前台赋值
                DataTable dTable = dSet.Tables[j];
                string json = GetGroupJSON(dTable, typeID, "H");//实况typeID = 0;//综合预报typeID = 1//模式typeID = 2
                if (json != "")
                {
                    sb.Append(json);
                    sb.Append(",");
                }
            }
            //标准化sb
            if (sb.Length > 1)
            {
                sb.Remove(sb.Length - 1, 1);
                sb.Append("}");
            }
            else
                sb.Length = 0;

            return sb.ToString();

        }

        /// <summary>
        /// 根据日期和时效返回综合预报的数据
        /// </summary>
        /// <param name="days">此日期包含在前台的顺序信息</param>
        /// <param name="period">24小时，48小时</param>
        /// <returns></returns>
        public string GetComForecast(string days, string period)
        {

            return "";
        }

        /// <summary>
        /// 根据浓度值和污染物ID，返回浓度和AQI的组合值，并具有颜色标识
        /// </summary>
        /// <param name="value"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public string ConvertToAQI(string value, string itemID)
        {

            int AQIValue = ToAQI(value, itemID);
            AQIExtention aqiExt = new AQIExtention(AQIValue, int.Parse(itemID));
            string aqiColor = string.Format("class='{0}'", aqiExt.Color);
            return string.Format("{0}/<span {1}>{2}</span>", value, aqiColor, AQIValue);
        }

        public int ToAQI(string value, string itemID)
        {
            int AQIValue = 0;
            double inputValue = double.Parse(value) / 1000;
            switch (itemID)
            {
                case "1":
                    AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 24, 11, 180);
                    break;
                case "2":
                    AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 7, 11, 180);
                    break;
                case "3":
                    AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 22, 10, 0);
                    break;
                case "4":
                    AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 8, 10, 0);
                    break;
                case "5":
                    AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 8, 16, 16);
                    break;

            }

            return AQIValue;
        }
        /// <summary>
        /// 获取预报表单的JSON
        /// </summary>
        /// <param name="forecast"></param>
        /// <returns></returns>
        private string GetForecastJSON(DataTable forecast)
        {
            StringBuilder sb = new StringBuilder();
            if (forecast.Rows.Count > 0)
            {

                for (int i = 0; i < forecast.Columns.Count; i++)
                {
                    sb.Append(string.Format("{0}:'{1}',", forecast.Columns[i].ColumnName, forecast.Rows[0][i]));
                }
                //去掉多余的“,”
                if (sb.Length > 1)
                {
                    sb.Remove(sb.Length - 1, 1);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 根据传入的分段预报表，获取能够被前台识别的JSON
        /// </summary>
        /// <param name="dtGroup">分段与报表</param>
        /// <returns></returns>
        private string GetGroupJSON(DataTable dtGroup, int typeID, string tag)
        {
            StringBuilder sb = new StringBuilder();
            if (dtGroup.Rows.Count > 0)
            {
                AQIExtention aqiExt;
                string aqiColor = "";
                int spanDays;
                DataTable dtGroupFilter = null;
                if (dtGroup.Columns.Count > 5)
                {
                    dtGroupFilter = dtGroup.DefaultView.ToTable(false, "LST", "DURATIONID", "ITEMID", "VALUE", "AQI", "Parameter");
                }
                else
                {
                    dtGroupFilter = dtGroup;
                }
                foreach (DataRow row in dtGroupFilter.Rows)
                {
                    if (dtGroup.Columns[0].DataType == typeof(DateTime))
                    {
                        DateTime forcast = DateTime.Parse(dtGroup.Rows[0][1].ToString());
                        TimeSpan timeSpan = forcast.Date - DateTime.Parse(row[0].ToString()).Date;
                        spanDays = timeSpan.Days - 1;
                    }
                    else
                    {
                        spanDays = int.Parse(row[0].ToString());
                    }
                    if (row[4].ToString() != "" && row[2].ToString() != "")
                    {
                        aqiExt = new AQIExtention(int.Parse(row[4].ToString()), int.Parse(row[2].ToString()));
                        aqiColor = string.Format("class='{0}'", aqiExt.Color);
                    }
                    if (int.Parse(row[2].ToString()) == 0 && dtGroupFilter.Columns.Count > 5)
                        sb.Append(string.Format("{7}{0}{1}{2}{3}:\"<span {5}>{6}</span>/{4}\",", m_BackDays - spanDays, typeID, row[1], 6, row[5], aqiColor, row[4], tag));
                    else
                        sb.Append(string.Format("{7}{0}{1}{2}{3}:\"{4}/<span {5}>{6}</span>\",", m_BackDays - spanDays, typeID, row[1], row[2], row[3], aqiColor, row[4], tag));

                }
                //去掉多余的“,”
                if (sb.Length > 1)
                {
                    sb.Remove(sb.Length - 1, 1);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 根据起报时间和模式类型，返回相应模式数据的SQL语句
        /// </summary>
        /// <param name="forecastDate"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        private string CreateModuleSQL(DateTime dtNow, string moduleType)
        {
            string forecastDate;
            string lastForecastDate;
            if (moduleType == "WRF")
            {
                forecastDate = dtNow.Date.ToString("yyyy-MM-dd 00:00:00");
                lastForecastDate = dtNow.AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
            }
            else
            {
                forecastDate = dtNow.AddDays(-1).ToString("yyyy-MM-dd 20:00:00");
                lastForecastDate = dtNow.AddDays(-1).ToString("yyyy-MM-dd 20:00:00");
            }

            string strSQL = String.Format("SELECT TOP 1 LST FROM T_FORECASTGROUP WHERE FORECASTDATE = '{0}' AND MODULE = '{1}' AND EXISTS(SELECT 1 FROM D_DurationTest WHERE (CODE = 1) AND (DM = DURATIONID))", forecastDate, moduleType);
            //作者：张伟锋   日期：2013年08月13日     临时增加代码，为了能够确保可以看到模式预报数据，在当前预报时间下如果没有模式数据，那么获取前一天的模式预报数据

            DataTable dt = m_Database.GetDataTable(strSQL);
            if (dt.Rows.Count == 0)
            {
                strSQL = String.Format("SELECT  DATEDIFF(DAY, LST, '{0}') AS LST, DURATIONID, ITEMID ,VALUE,AQI FROM T_FORECASTGROUP WHERE FORECASTDATE = '{1}' AND MODULE = '{2}' AND EXISTS(SELECT 1 FROM D_DurationTest WHERE (CODE = 1) AND (DM = DURATIONID)) AND LST >='{3}'", dtNow.AddDays(-1).ToString("yyyy-MM-dd 20:00:00"), lastForecastDate, moduleType, dtNow.ToString("yyyy-MM-dd 20:00:00"));

            }
            else
                strSQL = String.Format("SELECT  DATEDIFF(DAY, LST, '{0}') AS LST, DURATIONID, ITEMID ,VALUE,AQI FROM T_FORECASTGROUP WHERE FORECASTDATE = '{1}' AND MODULE = '{2}' AND EXISTS(SELECT 1 FROM D_DurationTest WHERE (CODE = 1) AND (DM = DURATIONID))", dtNow.AddDays(-1).ToString("yyyy-MM-dd 20:00:00"), forecastDate, moduleType);


            return strSQL;
        }

        /// <summary>
        /// 根据开始时间和结束时间返回实况数据的SQL
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        private string CreateRealtimeSQL(string fromDate, string toDate)
        {
            string strSQL = String.Format("SELECT  DATEDIFF(DAY, LST, '{0}') AS LST, DURATIONID, ITEMID ,VALUE,AQI FROM T_OBSDATAGROUP WHERE LST BETWEEN '{1}' AND '{0}' AND EXISTS(SELECT 1 FROM D_DurationTest WHERE (CODE = 1) AND (DM = DURATIONID))", toDate, fromDate);
            return strSQL;
        }
        private string CreateRealtimeReSeeSQL(string fromDate, string toDate)
        {
            fromDate = DateTime.Parse(fromDate).AddDays(1).ToString("yyyy-MM-dd");
            toDate = DateTime.Parse(toDate).AddDays(1).ToString("yyyy-MM-dd 23:59:59");
            string strSQL = String.Format("SELECT  DATEDIFF(DAY, LST, '{0}') AS LST, DURATIONID, ITEMID ,VALUE,AQI FROM T_OBSDATAGROUP WHERE LST BETWEEN '{1}' AND '{0}' AND EXISTS(SELECT 1 FROM D_DurationTest WHERE (CODE = 1) AND (DM = DURATIONID))", toDate, fromDate);
            return strSQL;
        }

        /// <summary>
        /// 根据预报时间和预报时效，返回综合预报数据的SQL语句
        /// </summary>
        /// <param name="forecastDate"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        private string CreateComSQL(string fromDate, string toDate, string period, string Module)
        {
            string strSQL = String.Format("SELECT  DATEDIFF(DAY, LST, '{0}') AS LST, DURATIONID, ITEMID ,VALUE,AQI FROM T_FORECASTGROUP WHERE LST BETWEEN '{1}' AND '{0}' AND MODULE = '{3}' AND PERIOD = {2} AND EXISTS(SELECT 1 FROM D_DurationTest WHERE (CODE = 1) AND (DM = DURATIONID))", toDate, fromDate, period, Module);
            return strSQL;
        }
        private string CreateComReSeeSQL(string fromDate, string toDate, string period, string Module)
        {
            fromDate = DateTime.Parse(fromDate).AddDays(1).ToString("yyyy-MM-dd");
            toDate = DateTime.Parse(toDate).AddDays(1).ToString("yyyy-MM-dd 23:59:59");
            string strSQL = String.Format("SELECT  DATEDIFF(DAY, LST, '{0}') AS LST, DURATIONID, ITEMID ,VALUE,AQI FROM T_FORECASTGROUP WHERE LST BETWEEN '{1}' AND '{0}' AND MODULE = '{3}' AND PERIOD = {2} AND EXISTS(SELECT 1 FROM D_DurationTest WHERE (CODE = 1) AND (DM = DURATIONID))", toDate, fromDate, period, Module);
            return strSQL;
        }
        private string CreateComReviseSQL(string fromDate, string toDate, string period, string Module)
        {
            int hour = int.Parse(DateTime.Now.Hour.ToString());
            string endDate = DateTime.Parse(toDate).ToString("yyyy-MM-dd 05:59:59");
            string strSQL = "";
            strSQL = String.Format("SELECT  DATEDIFF(DAY, LST, '{0}') AS LST, DURATIONID, ITEMID ,VALUE,AQI FROM T_FORECASTGROUP WHERE LST BETWEEN '{1}' AND '{0}' AND MODULE = '{3}' AND PERIOD = {2} AND EXISTS(SELECT 1 FROM D_DurationTest WHERE (CODE = 1) AND (DM = DURATIONID))", endDate, fromDate, period, Module);
            return strSQL;
        }
        /// <summary>
        /// 根据起报时间返回历史预报数据
        /// </summary>
        /// <param name="forecastDate"></param>
        /// <returns></returns>
        private string CreateHistoryComSQL(string forecastDate, string Module)
        {
            string strSQL = "";
            int hour = int.Parse(DateTime.Now.Hour.ToString());
            string dt;
            strSQL = String.Format("SELECT  DATEDIFF(DAY, LST, '{0}') - 1 AS LST, DURATIONID, ITEMID ,VALUE,AQI FROM T_FORECASTGROUP WHERE FORECASTDATE = '{0}' AND MODULE = '{1}' AND EXISTS(SELECT 1 FROM D_DurationTest WHERE (CODE = 1) AND (DM = DURATIONID))", forecastDate, Module);
            return strSQL;
        }
        private string CreateHistoryComReviseSQL(string forecastDate, string Module)
        {
            //DATEDIFF(DAY, LST, '{0}') - 1，这里的“-1”，是前台表格的行号偏移量
            string dt;
            dt = DateTime.Parse(forecastDate).AddDays(1).ToString("yyyy-MM-dd HH:00:00");
            string strSQL = String.Format("SELECT  DATEDIFF(DAY, LST, '{0}') AS LST, DURATIONID, ITEMID ,VALUE,AQI FROM T_FORECASTGROUP WHERE FORECASTDATE = '{0}' AND LST>='{2}' AND MODULE = '{1}' AND EXISTS(SELECT 1 FROM D_DurationTest WHERE (CODE = 1) AND (DM = DURATIONID))", forecastDate, Module, dt);
            return strSQL;
        }
        public string SelectComReviseSQL(string hour, string period, string Module, string moduleStyle)
        {
            string strSQL = "";
            DateTime dtNow = GetManualForecastDate(hour);
            string sb = "";
            if (Module == "Manual")
            {
                strSQL = CreatComforecastSQL("0", dtNow.ToString("yyyy-MM-dd HH:00:00"), "Modify");
                DataTable dt = m_Database.GetDataTable(strSQL);
                if (dt.Rows.Count > 0)
                {
                    sb = GetForecast("0", hour, period, "Modify", "gengZ", moduleStyle);

                }
                else
                {
                    strSQL = CreatComforecastSQL("0", dtNow.ToString("yyyy-MM-dd HH:00:00"), Module);
                    dt = m_Database.GetDataTable(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        sb = GetForecast("0", hour, period, Module, "zhonghe", moduleStyle);
                    }
                    else
                    {
                        sb = GetForecast("1", hour, period, Module, "gengZ", moduleStyle);
                    }

                }
            }
            else
            {
                strSQL = CreatComforecastSQL("0", dtNow.ToString("yyyy-MM-dd HH:00:00"), "SMCModify");
                DataTable dt = m_Database.GetDataTable(strSQL);
                if (dt.Rows.Count > 0)
                {
                    sb = GetForecast("0", hour, period, "SMCModify", "gengZ", moduleStyle);

                }
                else
                {
                    strSQL = CreatComforecastSQL("0", dtNow.ToString("yyyy-MM-dd HH:00:00"), Module);
                    dt = m_Database.GetDataTable(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        sb = GetForecast("0", hour, period, Module, "gengZ", moduleStyle);
                    }
                    else
                    {
                        sb = GetForecast("1", hour, period, Module, "gengZ", moduleStyle);
                    }

                }

            }

            return sb;

        }
        public string CreatComforecastSQL(string flag, string forecastDate, string Module)
        {
            string sb = "";
            if (flag == "0")
                sb = CreateHistoryComSQL(forecastDate, Module);
            else
            {
                forecastDate = DateTime.Parse(forecastDate).AddDays(-1).ToString("yyyy-MM-dd HH:00:00");
                sb = CreateHistoryComReviseSQL(forecastDate, Module);
            }
            return sb;

        }
        public string BuildPreconsation(string forecastDate)
        {
            DateTime dtNow = DateTime.Now.Date.AddHours(18);
            if (forecastDate != "")
                dtNow = DateTime.Parse(forecastDate).AddHours(18);
            string forecastDateTime = dtNow.ToString("yyyy-MM-dd HH:00:00");
            Entity entity = new Entity(m_Database, "Forecast");
            string strSQL = string.Format("FORECASTDATE = '{0}'", dtNow);//起报时间是每天的18点
            strSQL = entity.BuildQuerySQL(strSQL, "");

            DataTable datePreTable = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder("{");

            //创建表单json
            string json = GetForecastJSON(datePreTable);
            sb.Append(json);
            if (json != "")
                sb.Append(",");

            strSQL = String.Format("SELECT  DATEDIFF(DAY, LST, '{0}') - 1 AS LST, DURATIONID, ITEMID ,VALUE,AQI,Parameter FROM T_FORECASTGROUP WHERE FORECASTDATE = '{0}'", forecastDateTime);
            DataTable dTable = m_Database.GetDataTable(strSQL);

            //生成实况，综合预报，模式数据的json
            if (dTable.Rows.Count > 0)
            {
                //创建json，便于前台赋值
                json = GetGroupJSON(dTable, 1, "H");//实况typeID = 0;//综合预报typeID = 1//模式typeID = 2

                if (json != "")
                {
                    sb.Append(json);
                    sb.Append(",");
                }

            }
            sb.Append(string.Format("nowDateTime:'{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            if (sb.Length > 1)
            {
                sb.Append("}");
            }
            else
                sb.Length = 0;

            return sb.ToString();

        }
        ///////
        /////// 创建汇总数据
        ///////
        public string BuildCollect(string forecastDate, string postJson, string Module)
        {
            DateTime dtForecastDate = DateTime.Parse(forecastDate);
            dtForecastDate = dtForecastDate.AddHours(18);
            StringBuilder sb = new StringBuilder("{");
            //Module = "Module";
            DataTable dTable = CaculateContent(dtForecastDate, postJson, Module);
            if (dTable.Rows.Count > 0)
            {
                //创建json，便于前台赋值
                string json = GetGroupJSON(dTable, 1, "PH");//实况typeID = 0;//综合预报typeID = 1//模式typeID = 2

                if (json != "")
                {
                    sb.Append(json);
                    sb.Append(",");
                }

            }
            if (sb.Length > 1)
            {
                sb.Remove(sb.Length - 1, 1);
                sb.Append("}");
            }
            else
                sb.Length = 0;

            return sb.ToString();

        }
        /// <summary>
        /// 创建预报预览，并根据当前的ID判断是否需要计算日平均，并计算日平均
        /// </summary>
        /// <param name="postJson"></param>
        /// <param name="rowID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public string BuildPreview(string forecastDate, string postJson, string divID, string itemID, string detail, string Module)
        {
            try
            {
                DateTime dtForecastDate = GetManualForecastDate(forecastDate);

                DataTable tbContent = CaculateContent(dtForecastDate, postJson, Module);

                //24小时预报预览
                StringBuilder sb = new StringBuilder("{H09:\"");

                //夜间
                AQIExtention aqiExt = ConvertAQIDescription(tbContent, dtForecastDate, 6, 24);

                if (aqiExt != null)
                    sb.AppendFormat("{0}夜间，{1}，{2}，{3}；", dtForecastDate.ToString("M月d日"), aqiExt.AQI, aqiExt.Quality, aqiExt.FirstItem);
                //上午
                aqiExt = ConvertAQIDescription(tbContent, dtForecastDate, 2, 24);
                if (aqiExt != null)
                    sb.AppendFormat("{0}上午，{1}，{2}，{3}，", dtForecastDate.AddDays(1).ToString("d日"), aqiExt.AQI, aqiExt.Quality, aqiExt.FirstItem);
                //下午
                aqiExt = ConvertAQIDescription(tbContent, dtForecastDate, 3, 24);
                if (aqiExt != null)
                    sb.AppendFormat("下午，{0}，{1}，{2}。", aqiExt.AQI, aqiExt.Quality, aqiExt.FirstItem);


                //48小时预报预览
                sb.Append("\",H10:\"");
                //夜间
                aqiExt = ConvertAQIDescription(tbContent, dtForecastDate, 6, 48);
                if (aqiExt != null)
                    sb.AppendFormat("{0}夜间，{1}，{2}，{3}；", dtForecastDate.AddDays(1).ToString("M月d日"), aqiExt.AQI, aqiExt.Quality, aqiExt.FirstItem);
                //上午
                aqiExt = ConvertAQIDescription(tbContent, dtForecastDate, 2, 48);
                if (aqiExt != null)
                    sb.AppendFormat("{0}上午，{1}，{2}，{3}，", dtForecastDate.AddDays(2).ToString("d日"), aqiExt.AQI, aqiExt.Quality, aqiExt.FirstItem);
                //下午
                aqiExt = ConvertAQIDescription(tbContent, dtForecastDate, 3, 48);
                if (aqiExt != null)
                    sb.AppendFormat("下午，{0}，{1}，{2}。", aqiExt.AQI, aqiExt.Quality, aqiExt.FirstItem);


                //24小时预报预览
                //夜间
                sb.Append("\",PH10:\"");
                StringBuilder sm = new StringBuilder();
                string firstItem = "";
                aqiExt = ConvertAQIDescription(tbContent, dtForecastDate, 6, 24);
                if (aqiExt != null)
                {
                    if (Module != "Modify" && Module != "SMCModify")
                    {
                        sb.AppendFormat("预计{0}夜间，分段指数为{1}；", dtForecastDate.ToString("M月d日"), ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true));
                    }
                    if (ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('，')[1].ToString() == "优")
                    {
                        firstItem = "-";
                    }
                    else
                    {
                        firstItem = aqiExt.FirstPItemNoByGrade;
                    }

                    sm.AppendFormat("{0}夜间,{1},{2},{3},", dtForecastDate.ToString("d日"), ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('，')[0].ToString(), ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('，')[1].ToString() == "" ? "-" : ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('，')[1].ToString(), stringFormat(firstItem));
                }
                else
                {
                    sm.AppendFormat("{0}夜间,{1},{2},{3},", dtForecastDate.ToString("d日"), "/", "/", "/");
                }
                //上午
                aqiExt = ConvertAQIDescription(tbContent, dtForecastDate, 2, 24);
                if (aqiExt != null)
                {
                    if (Module != "Modify" && Module != "SMCModify")
                    {
                        sb.AppendFormat("{0}上午，{1}；", dtForecastDate.AddDays(1).ToString("d日"), ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, false));
                    }
                    else
                    {
                        sb.AppendFormat("预计{0}上午，分段指数为{1}；", dtForecastDate.AddDays(1).ToString("M月d日"), ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true));
                    }
                    if (ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('，')[1].ToString() == "优")
                    {
                        firstItem = "-";
                    }
                    else
                    {
                        firstItem = aqiExt.FirstPItemNoByGrade;
                    }
                    sm.AppendFormat("{0}上午,{1},{2},{3},", dtForecastDate.AddDays(1).ToString("d日"), ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('，')[0].ToString(), ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('，')[1].ToString() == "" ? "-" : ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('，')[1].ToString(), stringFormat(firstItem));
                }
                else
                {
                    sm.AppendFormat("{0}上午,{1},{2},{3},", dtForecastDate.AddDays(1).ToString("d日"), "/", "/", "/");
                }
                //下午
                aqiExt = ConvertAQIDescription(tbContent, dtForecastDate, 3, 24);
                if (aqiExt != null)
                {
                    sb.AppendFormat("下午，{0}。", ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, false));
                    if (ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('，')[1].ToString() == "优")
                    {
                        firstItem = "-";
                    }
                    else
                    {
                        firstItem = aqiExt.FirstPItemNoByGrade;
                    }
                    sm.AppendFormat("{0}下午,{1},{2},{3},", dtForecastDate.AddDays(1).ToString("d日"), ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('，')[0].ToString(), ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('，')[1].ToString() == "" ? "-" : ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('，')[1].ToString(), stringFormat(firstItem));
                }
                else
                {
                    sm.AppendFormat("{0}下午,{1},{2},{3},", dtForecastDate.AddDays(1).ToString("d日"), "/", "/", "/");

                }
                sb.Append("\"");
                sm.Append(detail);
                sm.Append(",上海市环境监测中心 上海中心气象台,");
                sm.AppendFormat("{0}{1}时发布", dtForecastDate.ToString("M月d日"), DateTime.Now.Hour.ToString());
                string[] message = sm.ToString().Split(',');
                string existsSQL, updateSQL, insertSQL;
                existsSQL = @"SELECT foreDate FROM tb_AirForecast WHERE foreDate='" + dtForecastDate.ToString("yyyy年M月d日") + "'";
                updateSQL = @"UPDATE tb_AirForecast SET Seg1='" + message[0].ToString() + "',AQI1='" + message[1].ToString() + "',Grade1='" + message[2].ToString() + "',Param1='" + message[3].ToString() + "',Seg2='" + message[4].ToString() + "',AQI2='" + message[5].ToString() + "',Grade2='" + message[6].ToString() + "',Param2='" + message[7].ToString() + "',Seg3='" + message[8].ToString() + "',AQI3='" + message[9].ToString() + "',Grade3='" + message[10].ToString() + "',Param3='" + message[11].ToString() + "',Detail='" + message[12].ToString() + "',Sign='" + message[13].ToString() + "',publishTime='" + message[14].ToString() + "' WHERE foreDate='" + dtForecastDate.ToString("yyyy年M月d日") + "'";
                insertSQL = @"INSERT INTO tb_AirForecast VALUES('" + dtForecastDate.ToString("yyyy年M月d日") + "', '" + message[0].ToString() + "','" + message[1].ToString() + "','" + message[2].ToString() + "','" + message[3].ToString() + "','" + message[4].ToString() + "','" + message[5].ToString() + "','" + message[6].ToString() + "','" + message[7].ToString() + "','" + message[8].ToString() + "','" + message[9].ToString() + "','" + message[10].ToString() + "','" + message[11].ToString() + "','" + message[12].ToString() + "','" + message[13].ToString() + "','" + message[14].ToString() + "')";
                m_Database.Execute(existsSQL, updateSQL, insertSQL);


                //计算日平均
                int period = 24;
                double rowsDate;
                if (itemID != "")
                {
                    string rowID = divID.Substring(1, 1);
                    if (rowID == "5")
                        period = 48;

                    string strFilter = string.Format("ForecastDate = '{0}' AND durationID = {1} AND PERIOD = {2} AND ITEMID = {3}", dtForecastDate, 7, period, itemID);
                    DataRow[] rows = tbContent.Select(strFilter);
                    if (rows[0][6] == DBNull.Value)//指定模式（pm2.5 pm10）下指定时间段（7指全天，1上半夜）（24小时或者48小时）下的AQI值
                        sb.AppendFormat(",{0}:''", divID);//为空也要加上“‘’”否则，前台序列化会报错。
                    else
                    {
                        aqiExt = new AQIExtention(int.Parse(rows[0][6].ToString()), int.Parse(itemID));
                        string aqiColor = string.Format("class='{0}'", aqiExt.Color);
                        string str = rows[0][5].ToString();//指定模式（pm2.5 pm10）下指定时间段（7指全天，1上半夜）（24小时或者48小时）下的VALUE值
                        rowsDate = Math.Round(double.Parse(str), 1);
                        str = rowsDate.ToString("f1");
                        sb.AppendFormat(",{0}:\"{1}/<span {2}>{3}</span>\"", divID, str, aqiColor, rows[0][6]);
                    }



                }

                sb.Append("}");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                m_Log.Error("BuildPreview", ex);
                return "";
            }

        }
        public string stringFormat(string items)
        {
            string returnItem = "-";
            switch (items)
            {
                case "PM2.5":
                    returnItem = "PM<sub>2.5</sub>"; break;
                case "PM10":
                    returnItem = "PM<sub>10</sub>"; break;
                case "NO2":
                    returnItem = "NO<sub>2</sub>"; break;
                case "O3":
                    returnItem = "O<sub>3</sub>"; break;
            }
            return returnItem;
        }
        /// <summary>
        /// 按照短信要求，重新组织AQI的值，标准如下：
        /// 1、在预报出的AQI基础上加正负10。
        /// 2、对范围进行修正，原则是个位数凑成5或0，凑范围的原则为就近原则，如72-92凑成70-90，73-93凑成75-95。
        /// 3、修正后还有以下几种特殊情况，
        ///     a)         0-20修正为1-20；
        ///     b)         50-70修正为55-75
        ///     c)         100-120修正为105-125
        ///     d)         150-170修正为155-175
        ///     e)         200-220修正为205-225
        ///     f)          300-320修正为305-325
        /// 4、修正后，对范围的两个至分别求等级，如果两个都在一个等级内，之描述一个等级即可，如果在两个等级内，可由低到高描述为**到**，如良到轻度污染。
        /// </summary>
        /// <param name="aqi"></param>
        /// <returns></returns>
        private string ParseAQIForSM(int aqi, string firstParameter, bool showDescription)
        {
            int i = aqi % 10;

            string description = "首要污染物";
            if (showDescription == false)
                description = "";
            description = "，" + description + firstParameter;

            //2、对范围进行修正，原则是个位数凑成5或0，凑范围的原则为就近原则，如72-92凑成70-90，73-93凑成75-95。
            if (i < 3)
                i = 0;
            else if (i > 7)
                i = 10;
            else
                i = 5;
            //1、在预报出的AQI基础上加正负10。
            int fAQI = aqi / 10 * 10 - 10 + i;
            if (fAQI <= 0)
                fAQI = 1;
            int tAQI = aqi / 10 * 10 + 10 + i;

            //3、修正后还有以下几种特殊情况，
            if (fAQI == 50 || fAQI == 100 || fAQI == 150 || fAQI == 200 || fAQI == 300)
            {
                fAQI = fAQI + 5;
                tAQI = tAQI + 5;
            }

            //4、修正后，对范围的两个至分别求等级，如果两个都在一个等级内，之描述一个等级即可，如果在两个等级内，可由低到高描述为**到**，如良到轻度污染。
            AQIExtention fAqiExt = new AQIExtention(fAQI);
            AQIExtention tAqiExt = new AQIExtention(tAQI);
            string strGrade = fAqiExt.Quality;

            if (strGrade != tAqiExt.Quality)
            {
                strGrade = string.Format("{0}到{1}", strGrade, tAqiExt.Quality);
            }
            else if (strGrade == "优")//优的情况下，不显示首页污染物
                description = "";
            if (strGrade == "轻度污染到中度污染")
                strGrade = "轻度到中度污染";
            if (strGrade == "中度污染到重度污染")
                strGrade = "中度到重度污染";
            if (strGrade == "重度污染到严重污染")
                strGrade = "重度到严重污染";

            string aqiSM = string.Format("{0}-{1}，{2}{3}", fAQI, tAQI, strGrade, description);

            return aqiSM;
        }




        /// <summary>
        /// 根据数据返回指定时间的AQI描述
        /// </summary>
        /// <param name="tbContent">预报内容</param>
        /// <param name="dtForecastDate">起报时间</param>
        /// <param name="durationID">分段ID</param>
        /// <param name="period">预报时效</param>
        /// <returns></returns>
        private AQIExtention ConvertAQIDescription(DataTable tbContent, DateTime dtForecastDate, int durationID, int period)
        {

            string strFilter = string.Format("ForecastDate = '{0}' AND durationID = {1} AND PERIOD = {2}", dtForecastDate, durationID, period);
            DataRow[] rows = tbContent.Select(strFilter, "AQI DESC");
            if (rows[0][6] == DBNull.Value)
                return null;
            int items = int.Parse(rows[0][4].ToString());
            int AQI = int.Parse(rows[0][6].ToString());
            if (items == 5)
            {
                if (int.Parse(rows[1][4].ToString()) == 0)
                {
                    items = int.Parse(rows[2][4].ToString());
                    AQI = int.Parse(rows[2][6].ToString());
                }
                else
                {
                    items = int.Parse(rows[1][4].ToString());
                    AQI = int.Parse(rows[1][6].ToString());
                }
            }

            AQIExtention aqiExt = new AQIExtention(AQI, items);
            return aqiExt;


        }

        /// <summary>
        /// 保存综合预报，分开处理预报表单和预报内容
        /// </summary>
        /// <param name="postJson"></param>
        /// <returns></returns>
        public string SaveEdits(string postJson, string Module)
        {
            string forecastDate;
            string Flag = "";
            string[] parts = postJson.Split(';');
            if (Module == "Manual" || Module == "SMC")
                Flag = "Former";
            else
                Flag = "Modify";

            //处理预报表单
            try
            {
                forecastDate = SaveForm(parts[0], Flag);

                //处理预报内容
                if (parts.Length > 1)
                    SaveContent(forecastDate, parts[1], Module);
                return "{success:true}";
            }
            catch (Exception ex)
            {
                m_Log.Error("SaveEdits", ex);
                return ex.ToString();
            }


        }
        public string SaveComForecastReSee(string forecastDate, string wether24, string wether48, string polution24, string polution48)
        {
            string flag = "Former";
            string forecastTime = DateTime.Parse(forecastDate).ToString("yyyy-MM-dd 18:00:00");
            string existsSQL, updateSQL, insertSQL;
            try
            {
                existsSQL = "SELECT ForecastDate FROM T_Forecast WHERE ForecastDate='" + forecastTime + "' AND Flag='" + flag + "'";
                updateSQL = "UPDATE T_Forecast SET UpdateDate=GETDATE(), WeatherReview24='" + wether24 + "',WeatherReview48='" + wether48 + "',PolutionReview24='" + polution24 + "',PolutionReview48='" + polution48 + "' WHERE ForecastDate='" + forecastTime + "' AND Flag='" + flag + "'";
                insertSQL = "INSERT INTO T_Forecast(ForecastDate,WeatherReview24,WeatherReview48,PolutionReview24,PolutionReview48,Flag,UpdateDate) VALUES('" + forecastTime + "', '" + wether24 + "', '" + wether48 + "', '" + polution24 + "', '" + polution48 + "','" + flag + "',GetDate())";
                m_Database.Execute(existsSQL, updateSQL, insertSQL);
                return "保存成功";
            }
            catch (Exception ex)
            {
                m_Log.Error("SaveComForecastReSee", ex);
                return ex.ToString();
            }

        }



        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="formContent"></param>
        /// <returns></returns>
        private string SaveForm(string formContent, string Module)
        {
            try
            {
                Entity entity = new Entity(m_Database, "Forecast");
                FilterOV filterOV = new FilterOV();
                string[] parts = formContent.Split(',');

                string[] keyValue = parts[0].Split(':');
                PropertyOV propertyOV = entity.GetPropertyOV(keyValue[0]);
                string formDate = GetManualForecastDate(keyValue[1]).ToString("yyyy-MM-dd HH:mm:ss");//综合预报起报时间

                propertyOV.ShowValue = formDate;
                filterOV.Add(propertyOV);
                string whereCause = string.Format(" WHERE {0} = '{1}' AND Flag='{2}'", propertyOV.Name, formDate, Module);
                string existsSQL = string.Format("SELECT ForecastDate FROM {0} {1}", entity.TableName, whereCause);

                for (int i = 1; i < parts.Length; i++)
                {
                    keyValue = parts[i].Split(':');
                    propertyOV = entity.GetPropertyOV(keyValue[0]);
                    propertyOV.ShowValue = keyValue[1];
                    filterOV.Add(propertyOV);
                }
                entity.EntityState = EntityStateContants.esUpdate;
                entity.SaveHistory = true;
                string updateSQL = entity.BuildSQL(filterOV) + whereCause;
                string formatStrSQL = string.Format("SET Flag='{0}',", Module);
                updateSQL = updateSQL.Replace("SET", formatStrSQL);
                entity.EntityState = EntityStateContants.esInsert;
                string insertSQL = entity.BuildSQL(filterOV);
                int indexStart = insertSQL.IndexOf(')');
                string tempStr = insertSQL.Insert(indexStart, ",Flag");
                string formatStr = string.Format(",'{0}'", Module);
                insertSQL = tempStr.Insert(tempStr.Length - 1, formatStr);
                //存在就更新，不存在就插入
                int ret = m_Database.Execute(existsSQL, updateSQL, insertSQL);


                if (ret == 1)
                    return formDate;
                else
                    return "";
            }
            catch (Exception ex)
            {
                m_Log.Error("SaveForm", ex);
                return ex.ToString();
            }
        }

        /// <summary>
        /// 把预报信息存入数据库，通过分析数据，得出4段时效
        /// 数据格式说明："H3141:1/2"，
        /// H：标签标识，即需要展示和编辑的标签
        /// 3：表示行号，用于计算综合预报的预报时间，
        /// 1：表示数据类型，实况typeID = 0;//综合预报typeID = 1//模式typeID = 2
        /// 4：表示时段，分为1（0-6h）,2（6-12h）,3（12-18h）,4（18-24）,5（0-6h）,6（6-18h）,7（0-24h），存储在字典表D_DurationTest表中
        /// 1：表示污染物，1（PM2.5）,2（PM10）,3（NO2）,4（03-1h）,5（03-8h），存储在字典表D_Item表中
        /// :：表示数据分隔，前部分是数据的描述，后面部分是污染物浓度和AQI值
        /// /：污染物浓度和AQI值的分隔标识
        /// 作者：张伟锋      日期：2013年06月30日      
        /// </summary>
        /// <param name="forecastContent">起报时间</param>
        /// <returns>入库成功返回true，否则返回false</returns>
        private bool SaveContent(string forecastDate, string forecastContent, string Module)
        {
            string strSQL;
            try
            {
                strSQL = string.Format("DELETE T_ForecastGroup_temp WHERE FORECASTDATE = '{0}' AND MODULE = '{1}'; INSERT INTO T_ForecastGroup_temp SELECT LST,ForecastDate,PERIOD,Module,durationID,ITEMID,Value,AQI,GROUPID,Parameter FROM  T_ForecastGroup WHERE FORECASTDATE = '{0}' AND MODULE = '{1}'", forecastDate, Module);
                m_Database.Execute(strSQL);
                strSQL = string.Format("DELETE T_ForecastGroup WHERE FORECASTDATE = '{0}' AND MODULE = '{1}'", forecastDate, Module);
                DataTable dt = CaculateContent(DateTime.Parse(forecastDate), forecastContent, Module);
                m_Database.Execute(strSQL);//删除已有记录
                return m_Database.BulkCopy(dt);
            }
            catch (Exception ex)
            {
                m_Log.Error("SaveContent", ex);
                return false;
            }

        }

        /// <summary>
        /// 计算预报内容，并返回表格
        /// </summary>
        /// <param name="forecastDate">起报时间</param>
        /// <param name="forecastContent">预报内容</param>
        /// <returns></returns>
        private DataTable CaculateContent(DateTime forecastDate, string forecastContent, string Module)
        {
            DateTime startDate = forecastDate.Date;
            DateTime lst;


            DataTable dt = new DataTable("T_ForecastGroup");
            dt.Columns.Add("LST", typeof(DateTime));
            dt.Columns.Add("ForecastDate", typeof(DateTime));
            dt.Columns.Add("PERIOD", typeof(int));

            dt.Columns.Add("durationID", typeof(int));
            dt.Columns.Add("ITEMID", typeof(int));
            dt.Columns.Add("Value", typeof(double));
            dt.Columns.Add("AQI", typeof(int));
            dt.Columns.Add("GROUPID", typeof(int));
            dt.Columns.Add("Module", typeof(string));
            dt.Columns.Add("Parameter", typeof(string));

            string[] parts = forecastContent.Split(',');
            string[] keyValue;
            int rowIndex = 0;
            int itemID;
            int durationID;
            int period = 24;
            float value;
            int AQI;

            string strSQL = "SELECT DM,MC FROM D_DurationTest";
            DataTable timePeriod = m_Database.GetDataTable(strSQL);
            for (int i = 0; i < parts.Length - 1; i++)
            {
                DataRow newRow = dt.NewRow();
                keyValue = parts[i].Split(':');
                //获取行号，并计算此行的预报日期
                rowIndex = int.Parse(keyValue[0].Substring(1, 1));
                lst = startDate.AddDays(rowIndex - m_BackDays - 1);

                //获取分段ID
                durationID = int.Parse(keyValue[0].Substring(3, 1));
                newRow[3] = durationID;

                string[] durationSpan = GetDurationSpan(timePeriod, durationID);
                lst = lst.AddHours(int.Parse(durationSpan[0]));
                newRow[0] = lst;
                newRow[1] = forecastDate;

                //与开始时间相比获取预报时效

                TimeSpan dateDiff = lst.Subtract(forecastDate);
                if (dateDiff.TotalHours >= 24)
                    period = 48;
                newRow[2] = period;

                //获取污染物类型ID
                itemID = int.Parse(keyValue[0].Substring(4, 1));
                newRow[4] = itemID;


                //获取污染物浓度
                if (keyValue[1].IndexOf('/') > 0)
                {
                    keyValue = keyValue[1].Split('/');
                    value = float.Parse(keyValue[0]);
                    AQI = int.Parse(keyValue[1]);
                    newRow[5] = Math.Round(value, 1);
                    newRow[6] = AQI;
                }
                else
                {
                    newRow[5] = DBNull.Value;
                    newRow[6] = DBNull.Value;
                }
                newRow[7] = 2;
                newRow[8] = Module;
                newRow[9] = DBNull.Value;
                dt.Rows.Add(newRow);
            }

            DataTable otherTable = CaculateOthers(dt, timePeriod, forecastDate, Module);
            dt.Merge(otherTable);
            DataTable maxTable = CaculateMax(dt, timePeriod, forecastDate, Module);
            dt.Merge(maxTable);


            return dt;
        }


        private DataTable CaculateMax(DataTable dt, DataTable timePeriod, DateTime forecastDate, string Module)
        {
            int maxAQI, preItems = 0;
            string filter, paraments = "";
            DataRow[] rows;
            DataTable tmpTable = dt.Clone();

            string strSQL = "SELECT DM,MC FROM D_ITEM";
            DataTable items = m_Database.GetDataTable(strSQL);
            for (int i = 1; i < 8; i++)
            {
                int period = 24;
                for (int j = 0; j < 2; j++)
                {
                    DataRow newRow = tmpTable.NewRow();
                    if (period == 24)
                        period = 48;
                    else
                        period = 24;
                    filter = string.Format("durationID = {0} AND PERIOD = '{1}'", i, period);
                    maxAQI = int.Parse(dt.Compute("max(AQI)", filter).ToString() == "" ? "0" : dt.Compute("max(AQI)", filter).ToString());
                    if (maxAQI == 0)
                    {
                        filter = string.Format("durationID = {0} AND PERIOD = '{1}'", i, period);
                    }
                    else
                    {
                        filter = string.Format("durationID = {0} AND PERIOD = '{1}' AND AQI={2}", i, period, maxAQI);
                    }
                    rows = dt.Select(filter);

                    newRow[0] = DateTime.Parse(rows[0][0].ToString());
                    newRow[1] = forecastDate;
                    newRow[2] = period;
                    newRow[3] = i;
                    newRow[4] = 0;
                    paraments = "";
                    for (int m = 0; m < rows.Length; m++)
                    {

                        preItems = int.Parse(rows[m][4].ToString());
                        filter = string.Format("DM = {0}", preItems);
                        DataRow[] itemsDataRow = items.Select(filter);
                        paraments = paraments + "   " + itemsDataRow[0][1].ToString();

                    }
                    if (maxAQI == 0)
                    {
                        newRow[5] = DBNull.Value;
                        newRow[6] = DBNull.Value;
                        newRow[9] = DBNull.Value;
                    }
                    else
                    {
                        newRow[5] = Math.Round(double.Parse(rows[0][5].ToString()), 1);
                        newRow[6] = maxAQI;
                        newRow[9] = paraments;

                    }
                    newRow[7] = 2;
                    newRow[8] = Module;
                    tmpTable.Rows.Add(newRow);

                }
            }
            return tmpTable;

        }


        private DataTable CaculateOthers(DataTable dt, DataTable timePeriod, DateTime forecastDate, string Module)
        {
            //计算白天（6-20h）：5、夜晚（20-6h）：6、全天（0-24h）：7 的数据

            //获取污染物的ID
            DataTable dicItem = dt.DefaultView.ToTable(true, "ITEMID");


            DataTable tmpTable = dt.Clone();

            //存储分段ID
            int durationID = 5;

            DateTime stopDatetime;
            DateTime fromDatetime;
            DateTime endDatetime;
            string[] durationSpan;
            foreach (DataRow r in dicItem.Rows)
            {
                //对于每一种污染物需要初始化
                fromDatetime = forecastDate.Date;
                stopDatetime = forecastDate.AddDays(2);//2段时效，48小时预报
                durationSpan = GetDurationSpan(timePeriod, durationID);//获取分段时间范围
                fromDatetime = fromDatetime.AddHours(int.Parse(durationSpan[0]));
                endDatetime = fromDatetime.Date.AddHours(int.Parse(durationSpan[1]));
                while (fromDatetime < stopDatetime.Date)
                {

                    DataRow newRow = AddNewRow(dt, tmpTable, forecastDate, fromDatetime, endDatetime, durationID, r[0], timePeriod, Module);
                    tmpTable.Rows.Add(newRow);
                    fromDatetime = fromDatetime.AddDays(1);
                    endDatetime = endDatetime.AddDays(1);
                }


                //1段时效，48小时预报
                fromDatetime = forecastDate.Date.AddDays(1);
                stopDatetime = fromDatetime.AddDays(2); //2段时效，48小时预报
                while (fromDatetime < stopDatetime)
                {
                    DataRow newRow = AddNewRow(dt, tmpTable, forecastDate, fromDatetime, fromDatetime.AddDays(1), 7, r[0], timePeriod, Module);
                    tmpTable.Rows.Add(newRow);

                    fromDatetime = fromDatetime.AddDays(1);
                }

            }

            return tmpTable;
        }

        /// <summary>
        /// 根据Duration字典表，返回相应ID的时间区间
        /// </summary>
        /// <param name="dcDuration"></param>
        /// <param name="durationID"></param>
        /// <returns></returns>
        private string[] GetDurationSpan(DataTable dcDuration, int durationID)
        {
            string filter = string.Format("DM ={0}", durationID);
            DataRow[] rows = dcDuration.Select(filter);
            string mcValue = rows[0][1].ToString();
            return mcValue.Split('-');

        }

        /// <summary>
        /// 在表中插入一行，主要是计算分段为5,6,7的，对于平均值的计算需要考虑到分段的时间
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tmpTable"></param>
        /// <param name="forecastDate"></param>
        /// <param name="fromDatetime"></param>
        /// <param name="toDatetime"></param>
        /// <param name="durationID"></param>
        /// <param name="itemID"></param>
        /// <param name="timePeriod">分段字典表</param>
        /// <returns></returns>
        private DataRow AddNewRow(DataTable dt, DataTable tmpTable, DateTime forecastDate, DateTime fromDatetime, DateTime toDatetime, int durationID, object itemID, DataTable timePeriod, string Module)
        {
            int period = 24;
            DataRow newRow = tmpTable.NewRow();
            string filter = string.Format("ITEMID = {0} AND LST >= '{1}' AND LST < '{2}' AND durationID<>6", itemID, fromDatetime, toDatetime);

            string value;
            newRow[0] = fromDatetime;
            newRow[1] = forecastDate;
            TimeSpan dateDiff = fromDatetime.Subtract(forecastDate);
            if (dateDiff.TotalHours >= 12)
                period = 48;
            newRow[2] = period;
            newRow[3] = durationID;
            newRow[4] = itemID;
            string str = itemID.ToString();
            if (str == "4" || str == "5")
            {
                value = dt.Compute("max(Value)", filter).ToString();
            }
            else
            {
                DataRow[] rows = dt.Select(filter);
                //value = dt.Compute("avg(Value)", filter).ToString();
                double sumValue = 0;
                int totalHours = 0;
                string[] durationSpan;//获取分段时间范围
                int span;

                for (int i = 0; i < rows.Length; i++)
                {
                    durationSpan = GetDurationSpan(timePeriod, int.Parse(rows[i][3].ToString()));//获取分段时间范围
                    span = int.Parse(durationSpan[1]) - int.Parse(durationSpan[0]);
                    if (span < 0)
                        span = 24 + span;//跨天
                    if (rows[i][5] != DBNull.Value)
                    {
                        sumValue = sumValue + double.Parse(rows[i][5].ToString()) * span;
                        totalHours = totalHours + span;
                    }
                }
                //计算平均值
                if (totalHours == 0)//当没有值的情况下
                    value = "";
                else
                    value = Convert.ToString((sumValue / totalHours));
            }

            if (value == "")
            {
                newRow[5] = DBNull.Value;
                newRow[6] = DBNull.Value;
            }
            else
            {
                newRow[5] = Math.Round(double.Parse(value), 1);
                newRow[6] = ToAQI(newRow[5].ToString(), itemID.ToString());
            }
            newRow[7] = 2;
            newRow[8] = Module;
            newRow[9] = DBNull.Value;


            return newRow;


        }
        //根据选择来发送短信
        public string PublicData(string checkBoxSelect, string content, string forecastDate, string phones, string phonesDX, string publishTime, string userName, string ForecastStyle)
        {
            DateTime forecastTime = DateTime.Parse(forecastDate).AddHours(18);
            try
            {
                string strSQL = @"UPDATE tb_AirForecast SET publishTime='" + publishTime + "' WHERE foreDate='" + forecastTime.ToString("yyyy年M月d日") + "'";
                m_Database.Execute(strSQL);
                StringBuilder returnString = new StringBuilder("{");
                string duanXin = "";
                string SHBJMess = "";
                string XJZXMess = "";
                string SSFBXT = "";
                string weiboMess = "";
                string TencentWeiBoStr;
                string DXMess = "";
                //移动短信
                if (checkBoxSelect.IndexOf("0") >= 0)
                {
                    duanXin = SendSM(content, forecastDate, phones, userName, "1", ForecastStyle);
                    returnString.Append(duanXin);
                    returnString.Append("，");
                }
                //市环保局
                if (checkBoxSelect.IndexOf("1") >= 0)
                {
                    SHBJMess = SHBJ(forecastDate, userName, "1", "", ForecastStyle);
                    returnString.Append(SHBJMess);
                    returnString.Append("，");
                }
                //宣教中心
                if (checkBoxSelect.IndexOf("2") >= 0)
                {
                    XJZXMess = XJZX(forecastDate, userName, "1", "", ForecastStyle);
                    returnString.Append(XJZXMess);
                    returnString.Append("，");
                }
                //实时发布系统
                if (checkBoxSelect.IndexOf("3") >= 0)
                {
                    SSFBXT = InsertDataBase(content, forecastDate, userName, "1", ForecastStyle);
                    returnString.Append(SSFBXT);
                    returnString.Append("，");
                }
                //新浪微博
                if (checkBoxSelect.IndexOf("4") >= 0)
                {
                    weiboMess = weiBo(content, forecastDate, userName, "1", ForecastStyle);
                    returnString.Append(weiboMess);
                    returnString.Append("，");
                }
                //电信
                if (checkBoxSelect.IndexOf("5") >= 0)
                {
                    DXMess = SendSMDX(content, forecastDate, phonesDX, userName, "1", ForecastStyle);
                    returnString.Append(DXMess);
                    returnString.Append("，");
                }
                //腾讯微博
                if (checkBoxSelect.IndexOf("6") >= 0)
                {
                    TencentWeiBoStr = TencentWeiBo(content, forecastDate, userName, "1", ForecastStyle);
                    returnString.Append(TencentWeiBoStr);
                    returnString.Append("，");
                }
                returnString.Remove(returnString.Length - 1, 1);
                returnString.Append("}");
                string sendMess = returnString.ToString();
                m_Log.Info("PublicData:" + sendMess);
                return sendMess;
            }
            catch (Exception ex)
            {
                m_Log.Error("PublicData", ex);
                return ex.Message;
            }
        }
        public string TencentWeiBo(string content, string forecastDate, string userName, string countNum, string ForecastStyle)
        {
            string strSQL;
            string sendInfo;
            string returnStrInfo = "";
            int count = int.Parse(countNum);
            try
            {
                SendWeiBo SendWeiBo = new SendWeiBo();

                //string[] conAry = content.Split('，');
                //string firstStr = conAry[1];
                //string zjStr = conAry[4];
                //int indexPos = content.IndexOf(firstStr);
                //int zjindexPos = content.IndexOf(zjStr);
                //if (conAry.Length > 7)
                //{
                //    string endStr = conAry[7];
                //    int endindexPos = content.IndexOf(endStr);
                //    content = conAry[0] + "，" + conAry[2] + "，" + conAry[3] + "，" + conAry[5] + "，" + conAry[6] + "，" + conAry[8] + "，" + conAry[9];
                //}
                //else
                //{
                //    content = conAry[0] + "，" + conAry[2] + "，" + conAry[3] + "，" + conAry[5] + "，" + conAry[6];
                //}

                string TencentReturn = SendWeiBo.SendTencent(content);
                if (TencentReturn == "成功")
                {
                    sendInfo = "发送成功";
                    returnStrInfo = "腾讯微博发送成功";
                }
                else
                {
                    sendInfo = "发送失败";
                    returnStrInfo = "腾讯微博发送失败，原因是" + TencentReturn;
                }
                if (count == 1)
                    strSQL = "INSERT INTO T_SendLog VALUES('" + DateTime.Now.ToString() + "','腾讯微博','" + ForecastStyle + "', '" + content + "', '" + userName + "', '" + sendInfo + "','1','','" + forecastDate + "')";
                else
                    strSQL = "UPDATE T_SendLog SET Recount='" + count + "',Message='" + sendInfo + "'  WHERE DateTime='" + forecastDate + "' AND PublicStyle='腾讯微博'";
                m_Database.Execute(strSQL);
                return returnStrInfo;
            }
            catch (Exception ex)
            {
                m_Log.Error("TencentWeiBo", ex);
                return ex.Message;
            }


        }
        public string weiBo(string content, string forecastDate, string userName, string countNum, string ForecastStyle)
        {
            string strSQL;
            string sendInfo;
            string returnStrInfo = "";
            int count = int.Parse(countNum);
            try
            {
                SendWeiBo SendWeiBo = new SendWeiBo();
                string sinaReturn = SendWeiBo.SendSina(content);

                if (sinaReturn == "成功")
                {
                    sendInfo = "发送成功";
                    returnStrInfo = "新浪微博发送成功";

                }
                else
                {
                    sendInfo = "发送失败";
                    returnStrInfo = "新浪微博发送失败，原因是" + sinaReturn;
                }
                if (count == 1)
                    strSQL = "INSERT INTO T_SendLog VALUES('" + DateTime.Now.ToString() + "','新浪微博','" + ForecastStyle + "', '" + content + "', '" + userName + "', '" + sendInfo + "','1','','" + forecastDate + "')";
                else
                    strSQL = "UPDATE T_SendLog SET Recount='" + count + "',Message='" + sendInfo + "'  WHERE DateTime='" + forecastDate + "' AND PublicStyle='新浪微博'";
                m_Database.Execute(strSQL);
                return returnStrInfo;
            }
            catch (Exception ex)
            {
                m_Log.Error("weiBo", ex);
                return ex.Message;
            }
        }
        //发到“市环保局”代码
        public string SHBJ(string forecastDate, string userName, string countNum, string content, string ForecastStyle)
        {
            //DateTime.Parse(DateTime.Parse(forecastDate).ToShortDateString()).AddHours(18);
            DateTime forecastTime = DateTime.Parse(DateTime.Parse(forecastDate).ToShortDateString()).AddHours(18);
            string strSQL;
            string sendInfo;
            string returnStrInfo = "";
            string sentContent;
            int count = int.Parse(countNum);
            try
            {
                if (content != "")
                {
                    strSQL = "SELECT * FROM tb_AirForecast WHERE foreDate='" + forecastTime.ToString("yyyy年M月d日") + "'";
                    DataTable db = m_Database.GetDataTable(strSQL);
                    string foreContent = "";
                    if (db.Rows.Count > 0)
                    {
                        for (int i = 0; i < db.Columns.Count - 1; i++)
                        {
                            foreContent = foreContent + "|" + db.Rows[0][i + 1].ToString();
                        }
                    }
                    sentContent = foreContent.Remove(0, 1);
                }
                else
                {
                    sentContent = content;

                }

                PushAQIOC poc = new PushAQIOC();
                poc.Url = @"http://211.144.123.230/AQIforSEIC/PushAQIOC.asmx";


                poc.UseDefaultCredentials = true;

                poc.PreAuthenticate = true;

                poc.Credentials = System.Net.CredentialCache.DefaultCredentials;
                ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["WebService"];
                string constring = settings.ConnectionString;
                string[] parts = constring.Split(new char[] { ';', '=' }, StringSplitOptions.None);
                try
                {
                    string result = "";
                    result = poc.importAQIForecast(sentContent, "|", parts[11]);
                    returnStrInfo = string.Format("市环保局发送{0}", result);
                    if (result == "成功")
                        sendInfo = "发送成功";
                    else
                        sendInfo = "发送" + result;

                }
                catch (Exception ex)
                {
                    sendInfo = "发送失败";
                    returnStrInfo = string.Format("市环保局发送失败,原因是{0}", ex.ToString());
                }
                if (count == 1)
                    strSQL = "INSERT INTO T_SendLog VALUES('" + DateTime.Now.ToString() + "','市环保局','" + ForecastStyle + "', '" + sentContent + "', '" + userName + "', '" + sendInfo + "','1','','" + forecastDate + "')";
                else
                    strSQL = "UPDATE T_SendLog SET Recount='" + count + "',Message='" + sendInfo + "'  WHERE DateTime='" + forecastDate + "' AND PublicStyle='市环保局'";
                m_Database.Execute(strSQL);
                return returnStrInfo;
            }
            catch (Exception ex)
            {
                m_Log.Error("SHBJ", ex);
                return ex.Message;
            }

        }
        //发到宣教中心
        public string XJZX(string forecastDate, string userName, string countNum, string content, string ForecastStyle)
        {
            DateTime forecastTime = DateTime.Parse(DateTime.Parse(forecastDate).ToShortDateString()).AddHours(18);
            string strSQL;
            string returnStrInfo = "";
            string sentContent;
            string foreContent = "";
            string sendInfo;
            int count = int.Parse(countNum);
            try
            {
                try
                {
                    if (content != "")
                    {
                        strSQL = "SELECT * FROM tb_AirForecast WHERE foreDate='" + forecastTime.ToString("yyyy年M月d日") + "'";
                        DataTable db = m_Database.GetDataTable(strSQL);
                        if (db.Rows.Count > 0)
                        {
                            for (int i = 0; i < db.Columns.Count - 1; i++)
                            {
                                foreContent = foreContent + "|" + db.Rows[0][i + 1].ToString();
                            }
                        }
                          sentContent = foreContent.Remove(0, 1);
                    }
                    else
                    {
                        sentContent = content;
                    }
                }
                catch (Exception ex) {
                    throw ex;
                }

                ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["WebService"];
                string constring = settings.ConnectionString;
                string[] parts = constring.Split(new char[] { ';', '=' }, StringSplitOptions.None);
                Lucas.importAQI imAQI = new Lucas.importAQI();
                imAQI.Url = "http://www.envir.gov.cn/aqiforseec/importAQI.asmx";
                //WebProxy proxy = new WebProxy();
                //proxy = new WebProxy(parts[1], true);  //代理服务器信息可通过config文件配置            
                //proxy.Credentials = new NetworkCredential(parts[7], parts[9], parts[3]); //参数UserName, Password，Domain可通过config文件配置                      
                //imAQI.Proxy = proxy;
                try
                {
                    string resultString = "";
                    resultString = imAQI.importAQIForecast(sentContent, "|", parts[11]);
                    returnStrInfo = string.Format("宣教中心发送{0}", resultString);
                    sendInfo = "发送" + resultString;
                }
                catch (Exception ex)
                {
                    sendInfo = "发送失败";
                    returnStrInfo = string.Format("宣教中心发送失败,原因是{0}", ex.ToString());
                }
                if (count == 1)
                    strSQL = "INSERT INTO T_SendLog VALUES('" + DateTime.Now.ToString() + "','宣教中心','" + ForecastStyle + "', '" + sentContent + "', '" + userName + "', '" + sendInfo + "','1','','" + forecastDate + "')";
                else
                    strSQL = "UPDATE T_SendLog SET Recount='" + count + "',Message='" + sendInfo + "'  WHERE DateTime='" + forecastDate + "' AND PublicStyle='宣教中心'";

                try
                {
                    m_Database.Execute(strSQL);
                }
                catch(Exception ex) {
                    throw ex;
                }
                return returnStrInfo;
            }
            catch (Exception ex)
            {
                m_Log.Error("XJZX", ex);
                return ex.Message;
            }

        }
        public string InsertDataBase(string content, string forecastDate, string userName, string countNum, string ForecastStyle)
        {
            DateTime forecastTime = DateTime.Parse(forecastDate).AddHours(18);
            string sendInfo;
            string returnStrInfo = "";
            int count = int.Parse(countNum);
            string flag = "";
            if (ForecastStyle == "综合预报")
                flag = "Former";
            else
                flag = "Modify";
            try
            {
                string strSQL, existsSQL, updateSQL, insertSQL;
                try
                {
                    existsSQL = "SELECT ForecastDate FROM T_Forecast WHERE ForecastDate='" + forecastTime + "' AND Flag='" + flag + "'";
                    updateSQL = "UPDATE T_Forecast SET UpdateDate=GETDATE(), Message='" + content + "' WHERE ForecastDate='" + forecastTime + "' AND Flag='" + flag + "'";
                    insertSQL = "INSERT INTO T_Forecast(ForecastDate,Message,Flag,UpdateDate) VALUES('" + forecastTime + "', '" + content + "','" + flag + "',GetDate())";
                    m_Database.Execute(existsSQL, updateSQL, insertSQL);

                    try
                    {
                        strSQL = "SELECT * FROM tb_AirForecast WHERE foreDate='" + forecastTime.ToString("yyyy年M月d日") + "'";
                        DataTable db = m_Database.GetDataTable(strSQL);
                        db.TableName = "tb_AirForecast";
                        Database m_DatabaseNew = new Database("SEMCAIR");
                        strSQL = "TRUNCATE TABLE tb_AirForecast";
                        m_DatabaseNew.Execute(strSQL);//删除已有记录
                        strSQL = "TRUNCATE TABLE semc_air_1hr.DBO.tb_AirForecast";
                        m_DatabaseNew.Execute(strSQL);
                        bool returnStr = m_DatabaseNew.BulkCopy(db);
                        strSQL = "insert into semc_air_1hr.dbo.tb_AirForecast(foreDate,Seg1,AQI1,Grade1,Param1,Seg2,AQI2,Grade2,Param2,Seg3,AQI3,Grade3,Param3,Detail,Sign,publishTime) select foreDate,Seg1,AQI1,Grade1,Param1,Seg2,AQI2,Grade2,Param2,Seg3,AQI3,Grade3,Param3,Detail,Sign,publishTime from tb_AirForecast";
                        m_DatabaseNew.Execute(strSQL);
                        if (returnStr)
                        {
                            sendInfo = "发送成功";
                            returnStrInfo = "实时发布系统发送成功";
                        }
                        else
                        {
                            sendInfo = "发送失败";
                            returnStrInfo = "实时发布系统发送失败";
                        }

                    }
                    catch (Exception ex)
                    {
                        sendInfo = "发送失败";
                        returnStrInfo = "实时发布系统发送失败，" + ex.ToString();
                    }

                }
                catch (Exception ex)
                {
                    sendInfo = "发送失败";
                    returnStrInfo = string.Format("实时发布系统发送失败,原因是{0}", ex.ToString());

                }
                if (count == 1)
                    strSQL = "INSERT INTO T_SendLog VALUES('" + DateTime.Now.ToString() + "','实时发布系统','" + ForecastStyle + "', '" + content + "', '" + userName + "', '" + sendInfo + "','1','','" + forecastDate + "')";
                else
                    strSQL = "UPDATE T_SendLog SET Recount='" + count + "',Message='" + sendInfo + "'  WHERE DateTime='" + forecastDate + "' AND PublicStyle='实时发布系统'";
                m_Database.Execute(strSQL);
                return returnStrInfo;
            }
            catch (Exception ex)
            {
                m_Log.Error("InsertDataBase", ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// 实现预报内容的短信发送，并返回发送结果，把发送的情况入库
        /// <param name="content">发送内容</param>
        /// <param name="forecastDate">预报日期</param>
        /// <param name="phones">电话号码</param>
        /// <param name="userName">发送人员</param>
        /// <param name="count">发送次数</param>
        /// <param name="ForecastStyle">预报类型</param>
        /// <returns>返回是否发送成功的结果</returns>
        public string SendSMDX(string content, string forecastDate, string phones, string userName, string countNum, string ForecastStyle)
        {

            string returnMsg = "";
            string strSQL = "";
            string sendInfo = "发送成功";
            string sendContent = "";
            string contentTemp = "";
            string messageReturn = "";
            string endContent = "";
            string messageReturnStr = "";
            string url = "http://10.200.254.21:9090/ucp/services/UCPPlatService";
            string[] loginParams = new string[2];
            loginParams[0] = "huanbaosend";
            loginParams[1] = "huanbaosend";
            object[] sendMessParams = new object[7];
            int count = int.Parse(countNum);
            try
            {
                string loginReturn = WebServiceHelper.InvokeWebService(url, "UCPPlatWebServiceService", "login", loginParams).ToString();
                if (loginReturn.IndexOf("ERROR") >= 0)
                {
                    returnMsg = "电信联通发送失败,原因是" + loginReturn;
                    sendInfo = "发送失败";
                }
                else
                {
                    sendMessParams[0] = loginReturn;
                    sendMessParams[1] = "NORM";
                    sendMessParams[2] = phones;
                    sendMessParams[3] = DateTime.Now;
                    sendMessParams[5] = "";
                    sendMessParams[6] = false;
                    //获取需要发送的手机号码
                    if (phones != "")
                    {
                        if (content.Length > 200)
                        {
                            contentTemp = content;
                            int countLength = 0;
                            if (content.Length % 190 == 0)
                                countLength = content.Length / 190;
                            else
                            {
                                countLength = content.Length / 190 + 1;
                                endContent = content.Substring(content.Length / 190 * 190);
                            }
                            for (int i = 1; i < content.Length / 190 + 1; i++)
                            {
                                sendContent = contentTemp.Substring(0, 190);
                                contentTemp = contentTemp.Substring(190);
                                sendMessParams[4] = countLength.ToString() + "/" + i.ToString() + " " + sendContent;
                                messageReturn = WebServiceHelper.InvokeWebService(url, "UCPPlatWebServiceService", "sendSMS", sendMessParams).ToString();
                                messageReturnStr = messageReturnStr + messageReturn;

                            }
                            if (endContent != "")
                            {
                                sendMessParams[4] = countLength.ToString() + "/" + countLength.ToString() + " " + endContent;
                                messageReturn = WebServiceHelper.InvokeWebService(url, "UCPPlatWebServiceService", "sendSMS", sendMessParams).ToString();
                                messageReturnStr = messageReturnStr + messageReturn;
                            }


                        }
                        else
                        {
                            sendMessParams[4] = content;
                            messageReturn = WebServiceHelper.InvokeWebService(url, "UCPPlatWebServiceService", "sendSMS", sendMessParams).ToString();
                            messageReturnStr = messageReturn;
                        }
                        if (messageReturnStr.IndexOf("ERROR") >= 0)
                        {
                            sendInfo = "发送失败";
                            returnMsg = "电信联通发送失败,原因是" + loginReturn;
                        }
                        else
                        {
                            sendInfo = "发送成功";
                            returnMsg = "电信联通发送成功！";
                        }

                    }
                    else
                    {
                        returnMsg = "电信联通失败，没有要发送的手机号码";
                        sendInfo = "发送失败";
                    }
                }
                string[] loginOff = new string[1];
                loginOff[0] = loginReturn;
                WebServiceHelper.InvokeWebService(url, "UCPPlatWebServiceService", "logoff", loginOff);
                if (count == 1)
                    strSQL = "INSERT INTO T_SendLog VALUES('" + DateTime.Now.ToString() + "','电信联通','" + ForecastStyle + "', '" + content + "', '" + userName + "', '" + sendInfo + "','1','" + phones + "','" + forecastDate + "')";
                else
                    strSQL = "UPDATE T_SendLog SET Recount='" + count + "',Message='" + sendInfo + "'  WHERE DateTime='" + forecastDate + "' AND PublicStyle='电信联通'";
                m_Database.Execute(strSQL);
                m_Log.Info("SendSM:" + returnMsg);
                return returnMsg;
            }
            catch (Exception ex)
            {
                m_Log.Error("SendSMDX", ex);
                return ex.Message;
            }
        }



        //public string SendSM(string content, string forecastDate, string phones, string userName, string countNum, string ForecastStyle){
        //    string returnMsg = "";
        //    string strSQL = "";
        //    string sendInfo = "发送成功";
        //    string sendContent = "";
        //    string contentTemp = "";
        //    string messageReturn = "";
        //    string endContent = "";
        //    string messageReturnStr = "";
        //    string url = "http://10.200.254.21:9090/ucp/services/UCPPlatService";
        //    string[] loginParams = new string[2];
        //    loginParams[0] = "huanbaosend";
        //    loginParams[1] = "huanbaosend";
        //    object[] sendMessParams = new object[7];
        //    int count = int.Parse(countNum);
        //    try
        //    {
        //        string loginReturn = WebServiceHelper.InvokeWebService(url, "UCPPlatWebServiceService", "login", loginParams).ToString();
        //        if (loginReturn.IndexOf("ERROR") >= 0)
        //        {
        //            returnMsg = "发送移动短信失败,原因是" + loginReturn;
        //            sendInfo = "发送失败";
        //        }
        //        else
        //        {
        //            sendMessParams[0] = loginReturn;
        //            sendMessParams[1] = "NORM";
        //            sendMessParams[2] = phones;
        //            sendMessParams[3] = DateTime.Now;
        //            sendMessParams[5] = "";
        //            sendMessParams[6] = false;
        //            //获取需要发送的手机号码
        //            if (phones != "")
        //            {
        //                if (content.Length > 200)
        //                {
        //                    contentTemp = content;
        //                    int countLength = 0;
        //                    if (content.Length % 190 == 0)
        //                        countLength = content.Length / 190;
        //                    else
        //                    {
        //                        countLength = content.Length / 190 + 1;
        //                        endContent = content.Substring(content.Length / 190 * 190);
        //                    }
        //                    for (int i = 1; i < content.Length / 190 + 1; i++)
        //                    {
        //                        sendContent = contentTemp.Substring(0, 190);
        //                        contentTemp = contentTemp.Substring(190);
        //                        sendMessParams[4] = countLength.ToString() + "/" + i.ToString() + " " + sendContent;
        //                        messageReturn = WebServiceHelper.InvokeWebService(url, "UCPPlatWebServiceService", "sendSMS", sendMessParams).ToString();
        //                        messageReturnStr = messageReturnStr + messageReturn;

        //                    }
        //                    if (endContent != "")
        //                    {
        //                        sendMessParams[4] = countLength.ToString() + "/" + countLength.ToString() + " " + endContent;
        //                        messageReturn = WebServiceHelper.InvokeWebService(url, "UCPPlatWebServiceService", "sendSMS", sendMessParams).ToString();
        //                        messageReturnStr = messageReturnStr + messageReturn;
        //                    }


        //                }
        //                else
        //                {
        //                    sendMessParams[4] = content;
        //                    messageReturn = WebServiceHelper.InvokeWebService(url, "UCPPlatWebServiceService", "sendSMS", sendMessParams).ToString();
        //                    messageReturnStr = messageReturn;
        //                }
        //                if (messageReturnStr.IndexOf("ERROR") >= 0)
        //                {
        //                    sendInfo = "发送失败";
        //                    returnMsg = "发送移动短信失败,原因是" + loginReturn;
        //                }
        //                else
        //                {
        //                    sendInfo = "发送成功";
        //                    returnMsg = "发送移动短信成功";
        //                }

        //            }
        //            else
        //            {
        //                returnMsg = "移动失败，没有要发送的手机号码";
        //                sendInfo = "发送失败";
        //            }
        //        }
        //        string[] loginOff = new string[1];
        //        loginOff[0] = loginReturn;
        //        WebServiceHelper.InvokeWebService(url, "UCPPlatWebServiceService", "logoff", loginOff);
        //        if (count == 1)
        //            strSQL = "INSERT INTO T_SendLog VALUES('" + DateTime.Now.ToString() + "','移动短信','" + ForecastStyle + "', '" + content + "', '" + userName + "', '" + sendInfo + "','1','" + phones + "','" + forecastDate + "')";
        //        else
        //            strSQL = "UPDATE T_SendLog SET Recount='" + count + "',Message='" + sendInfo + "'  WHERE DateTime='" + forecastDate + "' AND PublicStyle='移动短信'";
        //        m_Database.Execute(strSQL);
        //        m_Log.Info("SendSM:" + returnMsg);
        //        return returnMsg;
        //    }
        //    catch (Exception ex)
        //    {
        //        m_Log.Error("SendSMDX", ex);
        //        return ex.Message;
        //    }
        //}

        /// <summary>
        /// 实现预报内容的短信发送，并返回发送结果，把发送的情况入库
        /// <param name="content">发送内容</param>
        /// <param name="forecastDate">预报日期</param>
        /// <param name="phones">电话号码</param>
        /// <param name="userName">发送人员</param>
        /// <param name="count">发送次数</param>
        /// <param name="ForecastStyle">预报类型</param>
        /// <returns>返回是否发送成功的结果</returns>
        public string SendSM(string content, string forecastDate, string phones, string userName, string countNum, string ForecastStyle)
        {
            try
            {
                MasSender masSender = new MasSender();
                string returnMsg = string.Empty;
                string strSQL = "";
                int count = int.Parse(countNum);
                //获取需要发送的手机号码
                if (phones != "")
                {
                    string[] mobiles = phones.Split(',');
                    string phonesJoin = string.Join(",", mobiles);
                    //发送短信
                    int ret = masSender.SendSM(mobiles, content);
                    masSender.Relese();
                    string sendInfo;
                    if (ret == 0)
                        sendInfo = "发送成功";
                    else
                        sendInfo = "发送失败";
                    if (count == 1)
                        strSQL = "INSERT INTO T_SendLog VALUES('" + DateTime.Now.ToString() + "','移动短信','" + ForecastStyle + "', '" + content + "', '" + userName + "', '" + sendInfo + "','1','" + phonesJoin + "','" + forecastDate + "')";
                    else
                        strSQL = "UPDATE T_SendLog SET Recount='" + count + "',Message='" + sendInfo + "'  WHERE DateTime='" + forecastDate + "' AND PublicStyle='移动短信'";
                    m_Database.Execute(strSQL);
                    switch (ret)
                    {
                        case 0:
                            returnMsg = string.Format("发送移动短信成功");
                            break;
                        case -1:
                            returnMsg = string.Format("发送移动短信失败，原因是连接数据库出错,编号为{0}", ret);
                            break;
                        case -2:
                            returnMsg = string.Format("发送移动短信失败，原因是数据库关闭失败,编号为{0}", ret);
                            break;
                        case -3:
                            returnMsg = string.Format("发送移动短信失败，原因是数据库插入错误,编号为{0}", ret);
                            break;
                        case -4:
                            returnMsg = string.Format("发送移动短信失败，原因是数据库删除错误,编号为{0}", ret);
                            break;
                        case -5:
                            returnMsg = string.Format("发送移动短信失败，原因是数据库查询错误，编号为{0}", ret);
                            break;
                        case -6:
                            returnMsg = string.Format("发送移动短信失败，原因是数据库参数错误，编号为{0}", ret);
                            break;
                        case -7:
                            returnMsg = string.Format("发送移动短信失败，原因是API编码非法，编号为{0}", ret);
                            break;
                        case -8:
                            returnMsg = string.Format("发送移动短信失败，原因是参数超长，编号为{0}", ret);
                            break;
                        case -9:
                            returnMsg = string.Format("发送移动短信失败，原因是没有初始化或初始化失败，编号为{0}", ret);
                            break;
                        case -10:
                            returnMsg = string.Format("发送移动短信失败，原因是API接口处于暂停（失效）状态，编号为{0}", ret);
                            break;
                        case -11:
                            returnMsg = string.Format("发送移动短信失败，原因是短信网关未连接，编号为{0}", ret);
                            break;
                        case 1:
                            returnMsg = string.Format("发送移动短信失败，原因是发送内容为空，编号为{0}", ret);
                            break;
                        case 2:
                            returnMsg = string.Format("发送移动短信失败，原因是发送内容中存在被禁止词组，编号为{0}", ret);
                            break;
                        case 3:
                            returnMsg = string.Format("发送移动短信失败，原因是手机号码不正确，编号为{0}", ret);
                            break;
                        case 4:
                            returnMsg = string.Format("发送移动短信失败，原因是手机号码为运营商所禁止，编号为{0}", ret);
                            break;
                        case 5:
                            returnMsg = string.Format("发送移动短信失败，原因是手机号码存在黑名单中，编号为{0}", ret);
                            break;
                        case 6:
                            returnMsg = string.Format("发送移动短信失败，原因是手机号码不存在白名单中，编号为{0}", ret);
                            break;
                        case 7:
                            returnMsg = string.Format("发送移动短信失败，原因是企业欠费，编号为{0}", ret);
                            break;
                        case 8:
                            returnMsg = string.Format("发送移动短信失败，原因是通讯异常，编号为{0}", ret);
                            break;
                        case 101:
                            returnMsg = string.Format("发送移动短信失败，原因是系统错误，编号为{0}", ret);
                            break;
                        case 102:
                            returnMsg = string.Format("发送移动短信失败，原因是短信内容无法到达手机，编号为{0}", ret);
                            break;
                        default:
                            returnMsg = string.Format("发送移动短信失败，编号为{0}", ret);
                            break;
                    }
                }
                else
                    returnMsg = "发送移动短信失败，没有要发送的手机号码";
                m_Log.Info("SendSM:" + returnMsg);
                return returnMsg;
            }
            catch (Exception ex)
            {
                m_Log.Error("SendSM", ex);
                return ex.Message;
            }
        }

        public string changeEdits(string forecastDate, string module)
        {
            string forecastTime = DateTime.Parse(forecastDate).ToString("yyyy年M月d日");
            int hour = int.Parse(DateTime.Now.Hour.ToString());
            string strSQL = "SELECT * FROM tb_AirForecast WHERE foreDate='" + forecastTime + "'";
            DataTable dt = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table id='changeDatable'  width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.AppendLine("<tr>");

            //创建抬头
            sb.AppendLine("<td class='tableEditorRight'>时段</td>");
            sb.AppendLine("<td class='tableEditor'>空气质量</td>");
            sb.AppendLine("<td class='tableEditor'>首要污染物</td>");
            sb.AppendLine("<td class='tableEditor'>AQI</td>");
            sb.AppendLine("</tr>");
            if (dt.Rows.Count > 0)
            {
                if (module == "Modify" && hour <= 15)
                {
                    sb.AppendLine("<tr>");
                    sb.AppendLine(string.Format("<td class='tableRowEditorRight' style='line-height: 27px;display:none'><div contenteditable='true' class='divInputTypeNew' id='{1}' >{0}</div>", dt.Rows[0]["Seg1"], "Seg1"));
                    sb.AppendLine(string.Format("<td class='tableRowEditor'  style='line-height: 27px;display:none'><div contenteditable='true' id='{1}' class='divInputTypeNew'>{0}</div>", dt.Rows[0]["Grade1"], "Grade1"));
                    sb.AppendLine(string.Format("<td class='tableRowEditor' style='display:none'><div  id='{1}' class='divInputTypeNew'>{0}</div>", dt.Rows[0]["Param1"], "Param1"));
                    sb.AppendLine(string.Format("<td class='tableRowEditor' style='line-height: 27px ;display:none'><div contenteditable='true' id='{1}' class='divInputTypeNew'>{0}</div>", dt.Rows[0]["AQI1"], "AQI1"));
                    sb.AppendLine("</tr>");
                    for (int i = 2; i < 4; i++)
                    {
                        sb.AppendLine("<tr>");
                        sb.AppendLine(string.Format("<td class='tableRowEditorRight' style='line-height: 27px'><div contenteditable='true' class='divInputTypeNew' id='{1}' >{0}</div>", dt.Rows[0]["Seg" + i.ToString()], "Seg" + i.ToString()));
                        sb.AppendLine(string.Format("<td class='tableRowEditor'  style='line-height: 27px'><div contenteditable='true' id='{1}' class='divInputTypeNew'>{0}</div>", dt.Rows[0]["Grade" + i.ToString()], "Grade" + i.ToString()));
                        sb.AppendLine(string.Format("<td class='tableRowEditor'><div  id='{1}' class='divInputTypeNew'>{0}</div>", dt.Rows[0]["Param" + i.ToString()], "Param" + i.ToString()));
                        sb.AppendLine(string.Format("<td class='tableRowEditor' style='line-height: 27px'><div contenteditable='true' id='{1}' class='divInputTypeNew'>{0}</div>", dt.Rows[0]["AQI" + i.ToString()], "AQI" + i.ToString()));
                        sb.AppendLine("</tr>");
                    }
                }
                else
                {
                    for (int i = 1; i < 4; i++)
                    {
                        sb.AppendLine("<tr>");
                        sb.AppendLine(string.Format("<td class='tableRowEditorRight' style='line-height: 27px'><div contenteditable='true'  class='divInputTypeNew' id='{1}' >{0}</div>", dt.Rows[0]["Seg" + i.ToString()], "Seg" + i.ToString()));
                        sb.AppendLine(string.Format("<td class='tableRowEditor'  style='line-height: 27px'><div contenteditable='true' id='{1}' class='divInputTypeNew'>{0}</div>", dt.Rows[0]["Grade" + i.ToString()], "Grade" + i.ToString()));
                        sb.AppendLine(string.Format("<td class='tableRowEditor'><div  id='{1}' class='divInputTypeNew'>{0}</div>", dt.Rows[0]["Param" + i.ToString()], "Param" + i.ToString()));
                        sb.AppendLine(string.Format("<td class='tableRowEditor' style='line-height: 27px'><div contenteditable='true' id='{1}' class='divInputTypeNew'>{0}</div>", dt.Rows[0]["AQI" + i.ToString()], "AQI" + i.ToString()));
                        sb.AppendLine("</tr>");
                    }
                }
            }
            sb.AppendLine("</table>");
            sb.AppendLine(string.Format("<label id='lableEditor' class='lableStyle'>发布描述：</label><div class='tableAirSign divInputTypeNew' id='Detail' contenteditable='true'>{0}</div>", dt.Rows[0]["Detail"]));
            sb.AppendLine(string.Format("<div class='tableAirSign' id='Sign' contenteditable='true'>{0}</div>", dt.Rows[0]["Sign"]));
            sb.AppendLine(string.Format("<div class='tableAirSign' id='publishTimeChange' contenteditable='true'>{0}</div>", dt.Rows[0]["publishTime"]));
            return sb.ToString();

        }
        public string ReEditsSave(string SegValueStr, string AQIValueStr, string GradeValueStr, string ParamValueStr, string SignValue, string PublicTime, string Detail, string forecastDate)
        {
            if (ParamValueStr.IndexOf("SUB") > 0)
                ParamValueStr = repalceStr(ParamValueStr, "SUB", "sub");
            string[] SegValue = SegValueStr.Split('|');
            string[] AQIValue = AQIValueStr.Split('|');
            string[] GradeValue = GradeValueStr.Split('|');
            string[] ParamValue = ParamValueStr.Split('|');
            if (Detail == "<br>")
                Detail = "";
            DateTime dtForecastDate = DateTime.Parse(forecastDate);
            string existsSQL, updateSQL, insertSQL;
            existsSQL = @"SELECT foreDate FROM tb_AirForecast WHERE foreDate='" + dtForecastDate.ToString("yyyy年M月d日") + "'";
            updateSQL = @"UPDATE tb_AirForecast SET Seg1='" + SegValue[0] + "',AQI1='" + AQIValue[0] + "',Grade1='" + GradeValue[0] + "',Param1='" + ParamValue[0] + "',Seg2='" + SegValue[1] + "',AQI2='" + AQIValue[1] + "',Grade2='" + GradeValue[1] + "',Param2='" + ParamValue[1] + "',Seg3='" + SegValue[2] + "',AQI3='" + AQIValue[2] + "',Grade3='" + GradeValue[2] + "',Param3='" + ParamValue[2] + "',Detail='" + Detail + "',Sign='" + SignValue + "',publishTime='" + PublicTime + "' WHERE foreDate='" + dtForecastDate.ToString("yyyy年M月d日") + "'";
            insertSQL = @"INSERT INTO tb_AirForecast VALUES('" + dtForecastDate.ToString("yyyy年M月d日") + "', '" + SegValue[0] + "','" + AQIValue[0] + "','" + GradeValue[0] + "','" + ParamValue[0] + "','" + SegValue[1] + "','" + AQIValue[1] + "','" + GradeValue[1] + "','" + ParamValue[1] + "','" + SegValue[2] + "','" + AQIValue[2] + "','" + GradeValue[2] + "','" + ParamValue[2] + "','" + Detail + "','" + SignValue + "','" + PublicTime + "')";
            try
            {
                int count = m_Database.Execute(existsSQL, updateSQL, insertSQL);
                if (count > 0)
                    return "更新成功";
                else
                    return "更新失败";
            }
            catch (Exception ex)
            {
                m_Log.Error("ReEditsSave", ex);
                return ex.ToString();
            }


        }
        public string repalceStr(string strString, string oldStr, string newStr)
        {
            while (strString.IndexOf(oldStr) > 0)
                strString = strString.Replace(oldStr, newStr);
            return strString;
        }
        public string getContentEdits(string forecastDate)
        {
            string date = DateTime.Parse(forecastDate).ToString("yyyy/M/d 18:00:00");
            string strSQL = "SELECT Message FROM T_Forecast WHERE ForecastDate='" + date + "'";
            string content;
            DataTable dt = m_Database.GetDataTable(strSQL);
            if (dt.Rows.Count > 0)
                content = dt.Rows[0][0].ToString();
            else
                content = "";
            return content;

        }
        public string getPublicState(string publicStyle)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table id='PublicStateDatable'  width='100%' border='0' cellpadding='0' cellspacing='0' style='table-layout: fixed'>");
            sb.AppendLine("<tr>");

            //创建抬头
            sb.AppendLine("<td class='tableStateLeft'>序号</td>");
            sb.AppendLine("<td class='tableStateMiddle' >发布渠道</td>");
            sb.AppendLine("<td class='tableState'>状态</td>");
            sb.AppendLine("<td class='tableState'>重新发送</td>");
            sb.AppendLine("<td class='tableStateRight'>描述</td>");
            sb.AppendLine("</tr>");
            int index = 1;
            if (publicStyle.IndexOf('0') >= 0)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine(string.Format("<td class='tableStateLeft'>{0}</td>", index++));
                sb.AppendLine(string.Format("<td class='tableStateMiddle'>移动用户</td>"));
                sb.AppendLine(string.Format("<td class='tableState'><img id='ydUserImg'  src='images/wait.gif'/></td>"));
                sb.AppendLine(string.Format("<td class='tableState'><a href=\"javascript:reSentMobile();\"><img src='images/send.gif'/></a></td>"));
                sb.AppendLine(string.Format("<td class='tableStateRight' id='returnYDMessage'>正在发送</td>"));
                sb.AppendLine("</tr>");
            }
            if (publicStyle.IndexOf('5') >= 0)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine(string.Format("<td class='tableStateLeft'>{0}</td>", index++));
                sb.AppendLine(string.Format("<td class='tableStateMiddle'>联通电信</td>"));
                sb.AppendLine(string.Format("<td class='tableState'><img  id='dtdxImg' src='images/hc.png'/></td>"));
                sb.AppendLine(string.Format("<td class='tableState'><a href=\"javascript:reLTDX();\"><img src='images/send.gif'/></a></td>"));
                sb.AppendLine(string.Format("<td class='tableStateRight' id='returnLTDXMessage'>正在发送</td>"));
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='tableStateRight' colspan='5'><a href=\"javascript:hideT();\">关闭</a></td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</table>");
            return sb.ToString();
        }

        public string tableStringII(DataSet temp, int count)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("<table id='{0}' width='100%' border='0' cellpadding='0' cellspacing='0' class='tablekuang'>", temp.Tables[0].TableName));
            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<th class='tabletitle' rowspan='2'>日期</th>");
            sb.AppendLine("<th class='tabletitle' rowspan='2'>时段名称</th>");
            sb.AppendLine("<th class='tabletitle' rowspan='2'>时段区间</th>");
            sb.AppendLine("<th class='tabletitle' rowspan='2'>预报时效</th>");

            sb.AppendLine("<th class='tabletitle' rowspan='2'>发布用户</th>");
            sb.AppendLine("<th class='tabletitle' rowspan='2'>分段得分</th>");
            sb.AppendLine("<th class='tabletitle' rowspan='2'>总计得分</th>");

            sb.AppendLine("<th class='tabletitle' colspan='4'>PM2.5浓度</th>");
            sb.AppendLine("<th class='tabletitle' colspan='4'>PM2.5AQI</th>");
            sb.AppendLine("<th class='tabletitle' colspan='4'>PM10浓度</th>");
            sb.AppendLine("<th class='tabletitle' colspan='4'>PM10AQI</th>");
            sb.AppendLine("<th class='tabletitle' colspan='4'>NO2浓度</th>");
            sb.AppendLine("<th class='tabletitle' colspan='4'>NO2AQI</th>");
            sb.AppendLine("<th class='tabletitle' colspan='4'>03-1h浓度</th>");
            sb.AppendLine("<th class='tabletitle' colspan='4'>03-1hAQI</th>");
            sb.AppendLine("<th class='tabletitle' colspan='4'>03-8h浓度</th>");
            sb.AppendLine("<th class='tabletitle' colspan='4'>03-8hAQI</th>");
            sb.AppendLine("<th class='tabletitle' colspan='2'>AQI实测</th>");
            sb.AppendLine("<th class='tabletitle' colspan='2'>AQI综合预报</th>");
            sb.AppendLine("<th class='tabletitle' colspan='2'>AQI(CMAQ)</th>");
            sb.AppendLine("<th class='tabletitle' colspan='2'>AQI(WRF-CHEM)</th>");

            sb.AppendLine("</tr>");

            #region
            sb.AppendLine("<tr>");
            sb.AppendLine("<th class='tabletitle'>实测</th>");
            sb.AppendLine("<th class='tabletitle'>综合预报</th>");
            sb.AppendLine("<th class='tabletitle'>CMAQ</th>");
            sb.AppendLine("<th class='tabletitle'>WRF-CHEM</th>");

            sb.AppendLine("<th class='tabletitle'>实测</th>");
            sb.AppendLine("<th class='tabletitle'>综合预报</th>");
            sb.AppendLine("<th class='tabletitle'>CMAQ</th>");
            sb.AppendLine("<th class='tabletitle'>WRF-CHEM</th>");

            sb.AppendLine("<th class='tabletitle'>实测</th>");
            sb.AppendLine("<th class='tabletitle'>综合预报</th>");
            sb.AppendLine("<th class='tabletitle'>CMAQ</th>");
            sb.AppendLine("<th class='tabletitle'>WRF-CHEM</th>");

            sb.AppendLine("<th class='tabletitle'>实测</th>");
            sb.AppendLine("<th class='tabletitle'>综合预报</th>");
            sb.AppendLine("<th class='tabletitle'>CMAQ</th>");
            sb.AppendLine("<th class='tabletitle'>WRF-CHEM</th>");

            sb.AppendLine("<th class='tabletitle'>实测</th>");
            sb.AppendLine("<th class='tabletitle'>综合预报</th>");
            sb.AppendLine("<th class='tabletitle'>CMAQ</th>");
            sb.AppendLine("<th class='tabletitle'>WRF-CHEM</th>");

            sb.AppendLine("<th class='tabletitle'>实测</th>");
            sb.AppendLine("<th class='tabletitle'>综合预报</th>");
            sb.AppendLine("<th class='tabletitle'>CMAQ</th>");
            sb.AppendLine("<th class='tabletitle'>WRF-CHEM</th>");

            sb.AppendLine("<th class='tabletitle'>实测</th>");
            sb.AppendLine("<th class='tabletitle'>综合预报</th>");
            sb.AppendLine("<th class='tabletitle'>CMAQ</th>");
            sb.AppendLine("<th class='tabletitle'>WRF-CHEM</th>");

            sb.AppendLine("<th class='tabletitle'>实测</th>");
            sb.AppendLine("<th class='tabletitle'>综合预报</th>");
            sb.AppendLine("<th class='tabletitle'>CMAQ</th>");
            sb.AppendLine("<th class='tabletitle'>WRF-CHEM</th>");

            sb.AppendLine("<th class='tabletitle'>实测</th>");
            sb.AppendLine("<th class='tabletitle'>综合预报</th>");
            sb.AppendLine("<th class='tabletitle'>CMAQ</th>");
            sb.AppendLine("<th class='tabletitle'>WRF-CHEM</th>");

            sb.AppendLine("<th class='tabletitle'>实测</th>");
            sb.AppendLine("<th class='tabletitle'>综合预报</th>");
            sb.AppendLine("<th class='tabletitle'>CMAQ</th>");
            sb.AppendLine("<th class='tabletitle'>WRF-CHEM</th>");

            sb.AppendLine("<th class='tabletitle'>AQI</th>");
            sb.AppendLine("<th class='tabletitle'>首要污染物</th>");

            sb.AppendLine("<th class='tabletitle'>AQI</th>");
            sb.AppendLine("<th class='tabletitle'>首要污染物</th>");

            sb.AppendLine("<th class='tabletitle'>AQI</th>");
            sb.AppendLine("<th class='tabletitle'>首要污染物</th>");

            sb.AppendLine("<th class='tabletitle'>AQI</th>");
            sb.AppendLine("<th class='tabletitle'>首要污染物</th>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");
            #endregion
            sb.AppendLine("<tbody>");
            sb.AppendLine("</tbody>");

            sb.Append("</table>");



            #region 赋值
            //int m = 0;
            //int k = 0;
            //    AQIExtention aqiExt;
            //    for (int i = 0; i < temp.Tables.Count; i++)
            //    { 
            //        DataTable dt = temp.Tables[i];
            //    foreach (DataRow dr in dt.Rows)
            //    {

            //        k++;
            //        string tableName = dt.TableName.ToString();
            //        int items = int.Parse(tableName.Substring(5, 1));
            //        sb.AppendLine(string.Format("<tr  onmouseover='mouseOver(this)' onmouseout='mouseOut(this)' id='{0}'>", tableName + k.ToString()));
            //        for (int j = 0; j < dt.Columns.Count; j++)
            //        {
            //            if (j == 0)
            //            {
            //                if (m == 0 || m % count == 0)
            //                    sb.AppendLine(string.Format("<td class='tablerow'  rowspan='{1}' id='{2}{3}{4}'>{0}</td>", dr[j].ToString(), count, tableName, k, j));
            //            }
            //            else
            //            {
            //                if ((items == 0 && (j == 4 || j == 6 || j == 8 || j == 10)) || ((j == 9 || j == 8 || j == 10 || j == 11) && items != 0))
            //                {
            //                    if (dr[j].ToString() != "")
            //                    {
            //                        aqiExt = new AQIExtention(int.Parse(dr[j].ToString()), items);
            //                        string aqiColor = string.Format("class='{0}'", aqiExt.Color);
            //                        sb.AppendLine(string.Format("<td class='tablerow' id='{2}{3}{4}'><span {0}>{1}</span></td>", aqiColor, int.Parse(dr[j].ToString()), tableName, k, j));

            //                    }
            //                    else
            //                    {
            //                        string value = dr[j].ToString() == "" ? "/ " : dr[j].ToString();
            //                        sb.AppendLine(string.Format("<td class='tablerow' id='{1}{2}{3}'>{0}</td>", value, tableName, k, j));
            //                    }

            //                }
            //                else
            //                {
            //                    string value = dr[j].ToString() == "" ? "/ " : dr[j].ToString();
            //                    sb.AppendLine(string.Format("<td class='tablerow' id='{1}{2}{3}'>{0}</td>", value, tableName, k, j));
            //                }

            //            }

            //        }
            //        sb.AppendLine("</tr>");
            //        m++;

            //    }
            //    }
            //    sb.AppendLine("</table>|");
            //}

            #endregion
            string json = sb.ToString();//ProcessJson(sb.ToString());
            //int posi = json.LastIndexOf("|");
            //string returnJson = json.Substring(0, posi);
            return json;
        }

        private string ProcessJson(string xml)
        {

            string filter = "</table>|\r\n<table id='table{0}' width='100%' border='0' cellpadding='0' cellspacing='0' class='tablekuang'>";
            xml = xml.Replace(string.Format(filter, "2"), "");
            xml = xml.Replace(string.Format(filter, "3"), "");
            xml = xml.Replace(string.Format(filter, "4"), "");
            xml = xml.Replace(string.Format(filter, "5"), "");
            filter = "<table id='table{0}'  width='100%' border='0' cellpadding='0' cellspacing='0' class='tablekuang'>";
            xml = xml.Replace(string.Format(filter, "0"), "");
            xml = xml.Replace("|", "").Replace("</table>", "");
            xml = xml + "\r\n</table>";
            xml = string.Format(xml, "style=\"visiblity:hidden;\"");
            return xml;
        }

        public string GetSendLog()
        {
            string jsonString = "";
            try
            {
                DataTable dt = m_Database.GetDataTable(
                       "select TOP 10  * from T_SendLog where PublicResource='市教委' order by DateTime desc");
                jsonString = DataTableToJsonNew("SendLog", dt);
            }
            catch (Exception ex)
            {
                jsonString = ex.Message;
            }
            return jsonString;
        }

        public string GetGroup()
        {
            string jsonString = "";
            try
            {
                DataTable dt = m_Database.GetDataTable(
                       "select * From D_MasGroup where ParentID='4'");
                jsonString = DataTableToJsonNew("Group", dt);
            }
            catch (Exception ex){
                jsonString = ex.Message;
            }
            return jsonString;
        }

        /// <summary>
        /// 得到移动手机用户
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public string CreatePeople(string groupID)
        {
            string m_PeopleJson = "";
            string strSQL = "select Phone,Name,Company from T_Phones where Phone  in ( " +
                           " select Phone from T_Phones t1 left join T_MasGroup t2 on" +
                           " t1.ID=PhoneID left join D_MasGroup  t3 on t2.Groupid= t3.groupid" +
                           " where t3.GroupID=4 and t1.Type='移动') ORDER BY company";

            DataTable tbUsers = m_Database.GetDataTable(string.Format(strSQL, groupID));
            StringBuilder sb = new StringBuilder();
            int count = 0;
            for (int a = 0; a < tbUsers.Rows.Count; a++)
            {
                count++;
                DataRow row = tbUsers.Rows[a];
                string m = "";
                if (row[1].ToString().Length == 2)
                    m = row[1].ToString().Substring(0, 1) + "&nbsp&nbsp" + row[1].ToString().Substring(1, 1);
                else
                    m = row[1].ToString();
                if (a == tbUsers.Rows.Count - 1)
                    sb.Append(string.Format("{0}:{1}:{2}", row[0].ToString(), m, row["Company"].ToString()));
                else
                    sb.Append(string.Format("{0}:{1}:{2}", row[0].ToString(), m, row["Company"].ToString()) + ",");
            }
            //this.mCount.InnerText = "(" + count + "/" + count + ")";
            return (m_PeopleJson = sb.ToString());
        }

        /// <summary>
        /// 得到电信用户手机号码
        /// </summary>
        /// <returns></returns>
        public string CreatedianxinUser(string groupID)
        {
            string m_dianxinUser = "";
            string strSQL = "select Phone,Name,Company from T_Phones where Phone  in ( " +
                         " select Phone from T_Phones t1 left join T_MasGroup t2 on" +
                         " t1.ID=PhoneID left join D_MasGroup  t3 on t2.Groupid= t3.groupid" +
                         " where t3.GroupID={0} and t1.Type<>'移动') ORDER BY company";

            DataTable tbUsers = m_Database.GetDataTable(string.Format(strSQL,groupID));
            StringBuilder sb = new StringBuilder();
            int count = 0;
            for (int a = 0; a < tbUsers.Rows.Count; a++)
            {
                count++;
                DataRow row = tbUsers.Rows[a];
                string m = "";
                if (row[1].ToString().Length == 2)
                    m = row[1].ToString().Substring(0, 1) + "&nbsp&nbsp" + row[1].ToString().Substring(1, 1);
                else
                    m = row[1].ToString();
                if (a == tbUsers.Rows.Count - 1)
                    sb.Append(string.Format("{0}:{1}:{2}", row[0].ToString(), m, row["Company"].ToString()));
                else
                    sb.Append(string.Format("{0}:{1}:{2}", row[0].ToString(), m, row["Company"].ToString()) + ",");
            }
            return (m_dianxinUser = sb.ToString());
            //this.uCount.InnerText = "(" + count + "/" + count + ")";
        }


        public string GetSendLogii()
        {
            string jsonString = "";
            try
            {
                DataTable dt = m_Database.GetDataTable(
                       "select TOP 7  * from T_SendLog where PublicResource='临时短信' order by DateTime desc");
                jsonString = DataTableToJsonNew("SendLog", dt);
            }
            catch (Exception ex)
            {
                jsonString = ex.Message;
            }
            return jsonString;
        }

        public string GetSendLogNB()
        {
            string jsonString = "";
            try
            {
                DataTable dt = m_Database.GetDataTable(
                       "select TOP 7  * from T_SendLog where PublicResource='预警内部短信' order by DateTime desc");
                jsonString = DataTableToJsonNew("SendLog", dt);
            }
            catch (Exception ex)
            {
                jsonString = ex.Message;
            }
            return jsonString;
        }


        public string GetSendLogYZ()
        {
            string jsonString = "";
            try
            {
                DataTable dt = m_Database.GetDataTable(
                       "select TOP 7  * from T_SendLog where PublicResource='上海一周展望' order by DateTime desc");
                jsonString = DataTableToJsonNew("SendLog", dt);
            }
            catch (Exception ex)
            {
                jsonString = ex.Message;
            }
            return jsonString;
        }

        public string GetSendLogQT()
        {
            string jsonString = "";
            try
            {
                DataTable dt = m_Database.GetDataTable(
                       "select TOP 7  * from T_SendLog where PublicResource='预警全体短信' order by DateTime desc");
                jsonString = DataTableToJsonNew("SendLog", dt);
            }
            catch (Exception ex)
            {
                jsonString = ex.Message;
            }
            return jsonString;
        }

        /// <summary>
        /// Data to json
        /// </summary>
        /// <param name="jsonName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string DataTableToJsonNew(string jsonName, System.Data.DataTable dt)
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

        /// <summary>
        /// 查询预警人员信息
        /// </summary>
        /// <returns></returns>
        public  string GetUsers() {

            string jsonStr = "";
            try
            {
                string strSQL = "select (ROW_NUMBER() OVER(ORDER BY company )) as pID,[Name],Gender,Company,[Group],Fax,Tel,Email,x.Phone,[Type]," +
                               " CASE y.Enabel   "+
                                   " WHEN 1 THEN '启用'  "+
                                  "  ELSE '禁用'  "+
                               " END AS Enabel"+
                                      " from T_Phones x  left join  ( "+
                                "  select t2.GroupID,phone,enabel   from T_Phones t1 left join T_MasGroup t2 on"+
                                "  t1.ID=PhoneID left join D_MasGroup  t3 on t2.Groupid= t3.groupid"+
                              "  )  y on x.Phone=y.Phone  where y.GroupID=2 ORDER BY company";

                DataTable tbUsers = m_Database.GetDataTable(strSQL);
                jsonStr = DataTableToJson("data", tbUsers); 
            }
            catch { 
              
            }
            return jsonStr;
        }

        /// <summary>
        /// 创建移动号码
        /// </summary>
        public string CreatePeoples(string type)
        {
            string m_PeopleJson="";
            string strSQL = "select Phone,Name,Company from T_Phones where Phone  in ( " +
                           " select Phone from T_Phones t1 left join T_MasGroup t2 on" +
                           " t1.ID=PhoneID left join D_MasGroup  t3 on t2.Groupid= t3.groupid" +
                           " where t3.GroupName='{0}' and t1.Type='移动') ORDER BY company";
            try
            {
                DataTable tbUsers = m_Database.GetDataTable(string.Format(strSQL, type));
                StringBuilder sb = new StringBuilder();
                int count = 0;
                for (int a = 0; a < tbUsers.Rows.Count; a++)
                {
                    count++;
                    DataRow row = tbUsers.Rows[a];
                    string m = "";
                    if (row[1].ToString().Length == 2)
                        m = row[1].ToString().Substring(0, 1) + "&nbsp&nbsp" + row[1].ToString().Substring(1, 1);
                    else
                        m = row[1].ToString();
                    if (a == tbUsers.Rows.Count - 1)
                        sb.Append(string.Format("{0}:{1}:{2}", row[0].ToString(), m, row["Company"].ToString()));
                    else
                        sb.Append(string.Format("{0}:{1}:{2}", row[0].ToString(), m, row["Company"].ToString()) + ",");
                }
                m_PeopleJson = sb.ToString();
            }
            catch {
                m_PeopleJson = "";
            }
            return m_PeopleJson;
        }

        /// <summary>
        /// 创建电信号
        /// </summary>
        public string CreatedianxinUsers(string type)
        {
            string m_dianxinUser="";
            string strSQL = "select Phone,Name,Company from T_Phones where Phone  in ( " +
                         " select Phone from T_Phones t1 left join T_MasGroup t2 on" +
                         " t1.ID=PhoneID left join D_MasGroup  t3 on t2.Groupid= t3.groupid" +
                         " where t3.GroupName='{0}' and t1.Type<>'移动') ORDER BY company";
            try
            {
                DataTable tbUsers = m_Database.GetDataTable(string.Format(strSQL,type));
                StringBuilder sb = new StringBuilder();
                int count = 0;
                for (int a = 0; a < tbUsers.Rows.Count; a++)
                {
                    count++;
                    DataRow row = tbUsers.Rows[a];
                    string m = "";
                    if (row[1].ToString().Length == 2)
                        m = row[1].ToString().Substring(0, 1) + "&nbsp&nbsp" + row[1].ToString().Substring(1, 1);
                    else
                        m = row[1].ToString();
                    if (a == tbUsers.Rows.Count - 1)
                        sb.Append(string.Format("{0}:{1}:{2}", row[0].ToString(), m, row["Company"].ToString()));
                    else
                        sb.Append(string.Format("{0}:{1}:{2}", row[0].ToString(), m, row["Company"].ToString()) + ",");
                }
                m_dianxinUser = sb.ToString();
            }
            catch { 
              
            }
            return m_dianxinUser;
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


    }
}
