#region The ComForecast Copyright & Version History
/*
 * ============================================================== 
 * 
 * ComForecast, Version 1.0
 * 
 * Copyright (c) 2013-2014 �Ϻ�������Ϣ�Ƽ����޹�˾.  ��Ȩ����.
 * 
 * ��ΰ��
 * 
 * �޸ģ�
 *       
 * ��ΰ��              2010��11��25��
 * ====================================================================
 * 
 * ����˵�����û�ʵ�ֻ���������ĵ��ۺ�Ԥ��ҵ�񣬰���Ԥ������¼�룬��ʷԤ����Ϣ�ĵ�ȡ�ȡ�
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
using System.Collections;
namespace MMShareBLL.DAL
{
    public class ComForecastII
    {
        //���ڼ�¼ϵͳ������־
        protected static readonly log4net.ILog m_Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataSetForcast.EvalutionDTDataTable evalutionDt = new DataSetForcast.EvalutionDTDataTable();
        DataSetForcast.ForecastDataTable forecastDt = new DataSetForcast.ForecastDataTable();//Ԥ����ʵ��ķ�������
        DataSetForcast.EvalutionDTDataTable evaDt = new DataSetForcast.EvalutionDTDataTable();
        DataSetForcast.FODataTable fODt = new DataSetForcast.FODataTable();
        private Database m_Database;
        private int m_BackDays;
        public ComForecastII()
        {
            m_Database = new Database();
            m_BackDays = int.Parse(ConfigurationManager.AppSettings["BackDays"]);

        }
        /// <summary>
        /// ���ݵ�ǰ�����ʱ�䣬��ȡ�ۺ�Ԥ������ʱ��
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
        /// ���س���ǰ���������
        /// </summary>
        /// <param name="fromDate">��ʼ����</param>
        /// <param name="toDate">��������</param>
        /// <param name="SiteID">վ����</param>
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
            //212������ 213��ϩ�� 214�������� 215����VOC  8������
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
            catch(Exception ex)
            {
                m_Log.Error("GetOzoneChart", ex);
                return ex.ToString();
            }
        }


        /// <summary>
        /// �������ܽ���������
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
            string strWhere = " [END] BETWEEN '" + DateTime.Parse(fromDate).AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + DateTime.Parse(toDate).AddDays(1).AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss") + "' AND STATION='" + station + "'";
            string dateFiled = " DATEDIFF(S,'1970-01-01 00:00:00', DATEADD(hour,1,[END])) AS [END] ";

            string dateFileAdd = "  DATEDIFF(S,'1970-01-01 00:00:00',DATEADD(hour,1,LST)) AS LST ";
            string timeWhere = " LST BETWEEN '" + DateTime.Parse(fromDate).AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + DateTime.Parse(toDate).AddDays(1).AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss") + "'";

            //Cl,NO3,SO4,Na,NH4,K,Mg,Ca,[PM2#5(ug/m3)]
            strSQL = "SELECT " + dateFiled + " ,Cl from T_AirPercent WHERE [Cl-quality]='V' AND " + strWhere + " ORDER BY [END] ";
            strSQL = strSQL + ";SELECT " + dateFiled + ",NO3 from T_AirPercent WHERE [NO3-quality]='V' AND " + strWhere + " ORDER BY [END] ";
            strSQL = strSQL + ";SELECT " + dateFiled + ",SO4 from T_AirPercent WHERE [SO4-quality]='V' AND " + strWhere + " ORDER BY [END] ";
            strSQL = strSQL + ";SELECT " + dateFiled + ",Na from T_AirPercent WHERE [Na-quality]='V' AND " + strWhere + " ORDER BY [END] ";
            strSQL = strSQL + ";SELECT " + dateFiled + ",NH4 from T_AirPercent WHERE [NH4-quality]='V' AND " + strWhere + " ORDER BY [END] ";
            strSQL = strSQL + ";SELECT " + dateFiled + ",K from T_AirPercent WHERE [K-quality]='V' AND " + strWhere + " ORDER BY [END] ";
            strSQL = strSQL + ";SELECT " + dateFiled + ",Mg from T_AirPercent WHERE [Mg-quality]='V' AND " + strWhere + " ORDER BY [END] ";
            strSQL = strSQL + ";SELECT " + dateFiled + ",Ca from T_AirPercent WHERE [Ca-quality]='V' AND " + strWhere + " ORDER BY [END] ";
            string str = "";
            //56 OC(��ѧ)  57 EC(��ѧ) 58 OC(��ѧ) 59 EC(��ѧ) 228 �ֶ� 250�ζ� �ֶ�-���� �ζ�-����
            if (station == "CM")
            {
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",Value  from DMS_DATA WHERE ParameterID=56  AND SiteID=228  AND DurationID=10  AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",Value  from DMS_DATA WHERE ParameterID=57  AND SiteID=228  AND DurationID=10  AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",Value  from DMS_DATA WHERE ParameterID=58  AND SiteID=228  AND DurationID=10  AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",Value  from DMS_DATA WHERE ParameterID=59  AND SiteID=228  AND DurationID=10  AND " + timeWhere + " ORDER BY LST ";
                str = "SELECT DATEDIFF(S,'1970-01-01 00:00:00', LST) AS [END] ,ROUND(Value*1000,1) as Value from Data_RT_Site WHERE LST BETWEEN '" + from + "' AND '" + to + "' AND  SiteID=249 AND AQIItemID=101 ORDER BY LST ";
            }
            if (station == "QP")
            {
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",Value  from DMS_DATA WHERE ParameterID=56  AND SiteID=250  AND DurationID=10   AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",Value  from DMS_DATA WHERE ParameterID=57  AND SiteID=250  AND DurationID=10   AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",Value  from DMS_DATA WHERE ParameterID=58  AND SiteID=250  AND DurationID=10   AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",Value  from DMS_DATA WHERE ParameterID=59  AND SiteID=250  AND DurationID=10   AND " + timeWhere + "  ORDER BY LST ";
                str = "SELECT DATEDIFF(S,'1970-01-01 00:00:00', LST) AS [END] ,ROUND(Value*1000,1) as Value from Data_RT_Site WHERE LST BETWEEN '" + from + "' AND '" + to + "' AND  SiteID=203 AND AQIItemID=101 ORDER BY LST ";
            }
            if (station == "NJ")
            {
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",Value  from DMS_DATA WHERE ParameterID=56  AND SiteID=9999  AND DurationID=10   AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",Value  from DMS_DATA WHERE ParameterID=57  AND SiteID=9999  AND DurationID=10   AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",Value  from DMS_DATA WHERE ParameterID=58  AND SiteID=9999  AND DurationID=10   AND " + timeWhere + "  ORDER BY LST ";
                strSQL = strSQL + ";SELECT " + dateFileAdd + ",Value  from DMS_DATA WHERE ParameterID=59  AND SiteID=9999  AND DurationID=10   AND " + timeWhere + "  ORDER BY LST ";
                //str = "SELECT DATEDIFF(S,'1970-01-01 00:00:00', LST) AS [END] ,ROUND(Value*1000,1) as Value from Data_RT_Site WHERE LST BETWEEN '" + from + "' AND '" + to + "' AND  SiteID=203 AND AQIItemID=101 ORDER BY LST ";
                str = "SELECT DATEDIFF(S,'1970-01-01 00:00:00', TimePoint) AS [END] ,PM2_5 from [semc_dmc].[dbo].[China_RT_CNEMC_Data] WHERE TimePoint BETWEEN '" + from + "' AND '" + to + "' AND  Area = '�Ͼ�' AND PositionName ='�ݳ���' ORDER BY TimePoint ";
            }
            Database  m_DatabaseAQI = new Database("AQIWEB");
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
            catch(Exception ex)
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
        /// ����AQI�Ա�����
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
            //��AQI��ʱ���ѯ�����ݱ��
            if (itemID == "0")
                queryField = ",AQI,PARAMETER,LST AS BJTIME";

            try
            {
                string strStr = "  UNION SELECT " + dateFiled + queryField + " FROM T_ForecastGroup WHERE ITEMID=" + itemID + " AND durationID IN(2,3,6) ";
                //ʵ�⣨T_ObsDataGroup��
                if (typeID.IndexOf("0") >= 0)
                {
                    strSQL = "SELECT " + dateFiled + queryField + "  FROM  T_ObsDataGroup WHERE ITEMID=" + itemID + " AND durationID IN(2,3,6) " + strWhere + " ORDER BY LST ASC";
                    dt = m_Database.GetDataTable(strSQL);
                    strReturn = strReturn + GetLineStr(dt, "0");
                }

            if (typeID.IndexOf("1") >= 0)
            {
                //�պ�and  not VALUE is null
                strSQL = "SELECT " + dateFiled + queryField + " FROM T_ForecastGroup WHERE ITEMID=" + itemID + " AND durationID IN(2,3,6) AND PERIOD="
                    + period + " AND module = 'manual' " + strWhere + strStr + strWhere + " AND module = 'manual'  AND PERIOD="+period1+"  AND ForecastDate= '" + DateTime.Parse(maxForecast).ToString("yyyy/M/d 18:00") + "'  ORDER BY LST ASC";
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
                    //��ֵCMAQ
                    strSQL = "SELECT " + dateFiled + queryField + " FROM T_ForecastGroup WHERE ITEMID=" + itemID + " AND durationID IN(2,3,6) AND PERIOD="
                        + period + " AND module ='CMAQ' " + strWhere + strStr + strWhere + " AND module = 'CMAQ'  AND PERIOD=" + period1 + "   AND ForecastDate= '" + DateTime.Parse(maxForecast).ToString("yyyy/M/d 20:00") + "' ORDER BY LST ASC";
                    dt = m_Database.GetDataTable(strSQL);
                    strReturn = strReturn + GetLineStr(dt, "3");
                }
                if (strReturn != ",")
                    strReturn = "{" + strReturn.TrimStart(',') + "}";
                return strReturn;
            }
            catch(Exception ex)
            {
                m_Log.Error("GetAQICompare", ex);
                return ex.ToString();
            }
        }



        /// <summary>
        /// ����PM2.5�ͳ���������
        /// </summary>
        /// <param name="forecastDate">Ԥ��ʱЧ</param>
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

            //����
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
            //��ȡ��ʱ�䣬��������ʱ��Ϊ�գ���ô�Զ��Ե�ǰʱ��Ϊ��ʱ��
            DateTime dtNow = GetManualForecastDate(hour);

            string toDate = dtNow.ToString("yyyy-MM-dd 23:59:59");
            string fromDate = dtNow.AddDays(-m_BackDays).ToString("yyyy-MM-dd");

            //�����ۺ�Ԥ����ʵ��
            Entity entity = new Entity(m_Database, "Forecast");
            string strSQL = string.Format("FORECASTDATE = '{0}'", dtNow);//��ʱ����ÿ���18��
            strSQL = entity.BuildQuerySQL(strSQL, "");

            //����ʵ������SQL
            if (flag == "2")//Ԥ���ع˱�־
                strSQL = strSQL + ";" + CreateRealtimeReSeeSQL(fromDate, toDate);
            else 
                strSQL = strSQL + ";" + CreateRealtimeSQL(fromDate, toDate);

            //�����ο��ۺ�Ԥ��SQL
            if (tag == "zhonghe")
            {
                if(flag=="2")
                    strSQL = strSQL + ";" + CreateComReSeeSQL(fromDate, toDate, period, Module);
                else 
                    strSQL = strSQL + ";" + CreateComSQL(fromDate, toDate, period, Module);
            }
            else
                strSQL = strSQL + ";" + CreateComReviseSQL(fromDate, toDate, period, Module);

            //������ʷ�ۺ�Ԥ��
            strSQL = strSQL + " UNION ALL " + CreatComforecastSQL(flag, dtNow.ToString("yyyy-MM-dd HH:00:00"), Module);


            //����ģʽԤ��SQL����ֵģʽ����ʱ�����ۺ�Ԥ����ʱ���ǰһ��ı���ʱ��20��
            strSQL = strSQL + ";" + CreateModuleSQL(dtNow, moduleStyle);
            try
            {
                DataSet dSet = m_Database.GetDataset(strSQL);
                StringBuilder sb = new StringBuilder("{");

            //������json
            DataTable dTable = dSet.Tables[0];
            string json = GetForecastJSON(dTable);
            sb.Append(json);
            if (json != "")
                sb.Append(",");

            //����ʵ�����ۺ�Ԥ����ģʽ���ݵ�json
            for (int i = 0; i < 3; i++)
            {
                //����json������ǰ̨��ֵ
                dTable = dSet.Tables[i + 1];
                json = GetGroupJSON(dTable, i, "H");//ʵ��typeID = 0;//�ۺ�Ԥ��typeID = 1//ģʽtypeID = 2

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
            catch(Exception ex)
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
                strSQL = "SELECT LST,durationID, ITEMID, Value, AQI,Parameter,Quality  FROM T_ObsDataGroup Where " + durationIDSQL + "ORDER BY ITEMID";
                dtShiceGroup = m_Database.GetDataTable(strSQL);
            }
            DataSet ds = tableUnion(dtForecastGroup, dtShiceGroup, startDate, endDate, forecasPeriod, period);
            DataTable dtAQI = tableAQI(dtForecastGroup, dtShiceGroup, startDate, endDate, forecasPeriod, period);
            ds.Tables.Add(dtAQI);
            return ds;
        }

        //����ѡ�������������ݱ��
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
        /// ����ɸѡ
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="period"></param>
        /// <param name="forecasPeriod"></param>
        /// <param name="dataType"></param>
        /// <param name="dataModule"></param>
        /// <returns></returns>
        public void GetEvaluationDataTables(string fromDate, string toDate, string period, string forecasPeriod, string dataType, string dataModule)
        {
            DataSet ds = StrSQLString(fromDate, toDate, period, forecasPeriod, dataType, dataModule);
            int count = 0;
            string[] durationID;
            if (forecasPeriod != "")
            {
                durationID = forecasPeriod.Split(',');
                count = durationID.Length;
            }
             TableEvaluationString(ds, count,fromDate,toDate);  
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
            dSearch.Columns.Add("Quality", typeof(string));
            dSearch.Columns.Add("Parameter", typeof(string));
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
                            newRow[3] = dr[2].ToString() + "Сʱ";
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
                                    float shice = 0f;
                                    float.TryParse(shiceRow[0][4].ToString(), out shice);
                                    newRow[4] = shice.ToString();
                                    newRow["Quality"] = shiceRow[0]["Quality"].ToString();
                                    newRow["Parameter"] = shiceRow[0]["Parameter"].ToString();
                                }

                            }
                            else
                            {
                                newRow[4] = DBNull.Value;
                                newRow[5] = DBNull.Value;
                                newRow["Quality"] = "";
                                newRow["Parameter"] = "";
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
                                    newRow[3] = period + "Сʱ";
                                    string[] duratonSpan = GetDurationSpan(timePeriod, duraID);
                                    string span = string.Format("{0}:00", int.Parse(duratonSpan[0])) + "-" + string.Format("{0}:00", int.Parse(duratonSpan[1]));
                                    newRow[2] = span;
                                    string Filter = string.Format("DM={0}", duraID);
                                    DataRow[] rowDuation = timePeriod.Select(Filter);
                                    newRow[1] = rowDuation[0][2].ToString();

                                    string value = dr[5].ToString();
                                    newRow[5] = value;
                                    float shice = 0f;
                                    float.TryParse(dr[4].ToString(),out shice);
                                    newRow[4] = shice.ToString();
                                    newRow["Quality"] = shiceRow[0]["Quality"].ToString();
                                    newRow["Parameter"] = shiceRow[0]["Parameter"].ToString();
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
            #region 
            forecastDt.Clear();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < temp.Tables.Count; i++)
            {
                DataTable dt = temp.Tables[i];
                if (dt.TableName == "table0")
                {
                    sb.AppendLine("<table id='table0'  width='100%' border='0' cellpadding='0' cellspacing='0' class='tablekuang'>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td class='tabletitle' rowspan='2'>����</td>");
                    sb.AppendLine("<td class='tabletitle' rowspan='2'>ʱ������</td>");
                    sb.AppendLine("<td class='tabletitle' rowspan='2'>ʱ������</td>");
                    sb.AppendLine("<td class='tabletitle' rowspan='2'>Ԥ��ʱЧ</td>");
                    sb.AppendLine("<td class='tabletitle' colspan='2'>ʵ��</td>");
                    sb.AppendLine("<td class='tabletitle' colspan='2'>�ۺ�Ԥ��</td>");
                    sb.AppendLine("<td class='tabletitle' colspan='2'>CMAQ</td>");
                    sb.AppendLine("<td class='tabletitle' colspan='2'>WRF-CHEM</td>");
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td class='tabletitle'>AQI</td>");
                    sb.AppendLine("<td class='tabletitle'>��Ҫ��Ⱦ��</td>");
                    sb.AppendLine("<td class='tabletitle'>AQI</td>");
                    sb.AppendLine("<td class='tabletitle'>��Ҫ��Ⱦ��</td>");
                    sb.AppendLine("<td class='tabletitle'>AQI</td>");
                    sb.AppendLine("<td class='tabletitle'>��Ҫ��Ⱦ��</td>");
                    sb.AppendLine("<td class='tabletitle'>AQI</td>");
                    sb.AppendLine("<td class='tabletitle'>��Ҫ��Ⱦ��</td>");
                    sb.AppendLine("</tr>");

                }
                else
                {
                    sb.AppendLine(string.Format("<table id='{0}' width='100%' border='0' cellpadding='0' cellspacing='0' class='tablekuang'>", dt.TableName));
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td class='tabletitle' rowspan='2'>����</td>");
                    sb.AppendLine("<td class='tabletitle' rowspan='2'>ʱ������</td>");
                    sb.AppendLine("<td class='tabletitle' rowspan='2'>ʱ������</td>");
                    sb.AppendLine("<td class='tabletitle' rowspan='2'>Ԥ��ʱЧ</td>");
                    switch (dt.TableName)
                    {
                        case "table1":
                            sb.AppendLine("<td class='tabletitle' colspan='4'>PM2.5Ũ��</td>");
                            sb.AppendLine("<td class='tabletitle' colspan='4'>PM2.5AQI</td>");
                            break;
                        case "table2":
                            sb.AppendLine("<td class='tabletitle' colspan='4'>PM10Ũ��</td>");
                            sb.AppendLine("<td class='tabletitle' colspan='4'>PM10AQI</td>");
                            break;
                        case "table3":
                            sb.AppendLine("<td class='tabletitle' colspan='4'>NO2Ũ��</td>");
                            sb.AppendLine("<td class='tabletitle' colspan='4'>NO2AQI</td>");
                            break;
                        case "table4":
                            sb.AppendLine("<td class='tabletitle' colspan='4'>03-1hŨ��</td>");
                            sb.AppendLine("<td class='tabletitle' colspan='4'>03-1hAQI</td>");
                            break;
                        case "table5":
                            sb.AppendLine("<td class='tabletitle' colspan='4'>03-8hŨ��</td>");
                            sb.AppendLine("<td class='tabletitle' colspan='4'>03-8hAQI</td>");
                            break;

                    }
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td class='tabletitle'>ʵ��</td>");
                    sb.AppendLine("<td class='tabletitle'>�ۺ�Ԥ��</td>");
                    sb.AppendLine("<td class='tabletitle'>CMAQ</td>");
                    sb.AppendLine("<td class='tabletitle'>WRF-CHEM</td>");
                    sb.AppendLine("<td class='tabletitle'>ʵ��</td>");
                    sb.AppendLine("<td class='tabletitle'>�ۺ�Ԥ��</td>");
                    sb.AppendLine("<td class='tabletitle'>CMAQ</td>");
                    sb.AppendLine("<td class='tabletitle'>WRF-CHEM</td>");
                    sb.AppendLine("</tr>");

                }
                int m = 0;
                int k = 0;
                AQIExtention aqiExt;
                foreach (DataRow dr in dt.Rows)
                {
                    //�ж�Ҫ��ӵ������м���ֻ���״β�ѯ������
                    if (i == 0) 
                    {
                        DataRow forecastRow = forecastDt.NewRow();
                        forecastDt.Rows.Add(forecastRow);
                    }

                    k++;
                    string tableName = dt.TableName.ToString();
                    int items = int.Parse(tableName.Substring(5, 1));
                    sb.AppendLine(string.Format("<tr  onmouseover='mouseOver(this)' onmouseout='mouseOut(this)' id='{0}'>", tableName + k.ToString()));
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j == 0)
                        {
                            if (m == 0 || m % count == 0)
                            {
                                sb.AppendLine(string.Format("<td class='tablerow'  rowspan='{1}' id='{2}{3}{4}'>{0}</td>", dr[j].ToString(), count, tableName, k, j));
                                //�������
                                forecastDt.Rows[m]["RQ"] = dr[j].ToString();
                            }
                                
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
                                //
                                forecastDt.Rows[m][j + m] = value;

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
            #endregion 
        }
        public DataSet tableUnion(DataTable dt, DataTable dm, DateTime startDate, DateTime endTime, string durationID, string period)
        {   
            DataSet tmpTable = new DataSet();
            try
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
                dSearch.Columns.Add("Quality", typeof(string));
                dSearch.Columns.Add("Parameter", typeof(string));
             
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
                                    newRow[3] = dr[2].ToString() + "Сʱ";
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

                                            double value = 0d;
                                            double.TryParse(shiceRow[0][3].ToString(), out value);
                                            newRow[4] = Math.Round(value, 1);
                                            double.TryParse(shiceRow[0][4].ToString(), out value);
                                            newRow[8] = value.ToString();

                                            newRow["Quality"] = shiceRow[0]["Quality"].ToString();
                                            newRow["Parameter"] = shiceRow[0]["Parameter"].ToString();
                                        }

                                    }
                                    else
                                    {
                                        newRow[4] = DBNull.Value;
                                        newRow[8] = DBNull.Value;
                                        newRow["Quality"] = "";
                                        newRow["Parameter"] = "";
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
                                            newRow[3] = period + "Сʱ";
                                            string[] duratonSpan = GetDurationSpan(timePeriod, duraID);
                                            string span = string.Format("{0}:00", int.Parse(duratonSpan[0])) + "-" + string.Format("{0}:00", int.Parse(duratonSpan[1]));
                                            newRow[2] = span;
                                            string Filter = string.Format("DM={0}", duraID);
                                            DataRow[] rowDuation = timePeriod.Select(Filter);
                                            newRow[1] = rowDuation[0][2].ToString();

                                            double value = double.Parse(shiceRow[0][3].ToString());
                                            newRow[4] = Math.Round(value, 1);
                                            newRow[8] = int.Parse(shiceRow[0][4].ToString());
                                            newRow["Quality"] = shiceRow[0]["Quality"].ToString();
                                            newRow["Parameter"] = shiceRow[0]["Parameter"].ToString();
                                            temp.Rows.Add(newRow);
                                        }

                                    }

                                }
                            }
                        }
                    }
                    tmpTable.Tables.Add(temp);
                }
            }
            catch(Exception ex) { }
            return tmpTable;
        }
        /// <summary>
        /// ����ģʽ���ƻ�ȡģʽԤ��������
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public string GetModuleForecast(string hour, string module)
        {
            ////����ģʽԤ��SQL
            //��ȡ��ʱ�䣬��������ʱ��Ϊ�գ���ô�Զ��Ե�ǰʱ��Ϊ��ʱ��
            DateTime dtNow = GetManualForecastDate(hour);

            string strSQL = CreateModuleSQL(dtNow, module);

            DataTable dTable = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder("{");
            string json;
            //����ʵ�����ۺ�Ԥ����ģʽ���ݵ�json
            if (dTable.Rows.Count > 0)
            {
                //����json������ǰ̨��ֵ
                json = GetGroupJSON(dTable, 2, "H");//ʵ��typeID = 0;//�ۺ�Ԥ��typeID = 1//ģʽtypeID = 2

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
        /// �������ں�ʱЧ�����ۺ�Ԥ��������
        /// </summary>
        /// <param name="hour">�����ڰ�����ǰ̨��˳����Ϣ</param>
        /// <param name="period">24Сʱ��48Сʱ</param>
        /// <returns></returns>
        public string GetForecastByPeriod(string hour, string period, string Module)
        {
            //��ȡ��ʱ�䣬��������ʱ��Ϊ�գ���ô�Զ��Ե�ǰʱ��Ϊ��ʱ��
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

                //����json������ǰ̨��ֵ
                DataTable dTable = dSet.Tables[j];
                string json = GetGroupJSON(dTable, typeID, "H");//ʵ��typeID = 0;//�ۺ�Ԥ��typeID = 1//ģʽtypeID = 2
                if (json != "")
                {
                    sb.Append(json);
                    sb.Append(",");
                }
            }
            //��׼��sb
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
        /// �������ں�ʱЧ�����ۺ�Ԥ��������
        /// </summary>
        /// <param name="days">�����ڰ�����ǰ̨��˳����Ϣ</param>
        /// <param name="period">24Сʱ��48Сʱ</param>
        /// <returns></returns>
        public string GetComForecast(string days, string period)
        {

            return "";
        }

        /// <summary>
        /// ����Ũ��ֵ����Ⱦ��ID������Ũ�Ⱥ�AQI�����ֵ����������ɫ��ʶ
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

        public  int ToAQI(string value, string itemID)
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
        /// ��ȡԤ������JSON
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
                //ȥ������ġ�,��
                if (sb.Length > 1)
                {
                    sb.Remove(sb.Length - 1, 1);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// ���ݴ���ķֶ�Ԥ������ȡ�ܹ���ǰ̨ʶ���JSON
        /// </summary>
        /// <param name="dtGroup">�ֶ��뱨��</param>
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
                //ȥ������ġ�,��
                if (sb.Length > 1)
                {
                    sb.Remove(sb.Length - 1, 1);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// ������ʱ���ģʽ���ͣ�������Ӧģʽ���ݵ�SQL���
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
            //���ߣ���ΰ��   ���ڣ�2013��08��13��     ��ʱ���Ӵ��룬Ϊ���ܹ�ȷ�����Կ���ģʽԤ�����ݣ��ڵ�ǰԤ��ʱ�������û��ģʽ���ݣ���ô��ȡǰһ���ģʽԤ������

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
        /// ���ݿ�ʼʱ��ͽ���ʱ�䷵��ʵ�����ݵ�SQL
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
            toDate=DateTime.Parse(toDate).AddDays(1).ToString("yyyy-MM-dd 23:59:59");
            string strSQL = String.Format("SELECT  DATEDIFF(DAY, LST, '{0}') AS LST, DURATIONID, ITEMID ,VALUE,AQI FROM T_OBSDATAGROUP WHERE LST BETWEEN '{1}' AND '{0}' AND EXISTS(SELECT 1 FROM D_DurationTest WHERE (CODE = 1) AND (DM = DURATIONID))", toDate, fromDate);
            return strSQL;
        }

        /// <summary>
        /// ����Ԥ��ʱ���Ԥ��ʱЧ�������ۺ�Ԥ�����ݵ�SQL���
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
        /// ������ʱ�䷵����ʷԤ������
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
            //DATEDIFF(DAY, LST, '{0}') - 1������ġ�-1������ǰ̨�����к�ƫ����
            string dt;
            dt = DateTime.Parse(forecastDate).AddDays(1).ToString("yyyy-MM-dd HH:00:00");
            string strSQL = String.Format("SELECT  DATEDIFF(DAY, LST, '{0}') AS LST, DURATIONID, ITEMID ,VALUE,AQI FROM T_FORECASTGROUP WHERE FORECASTDATE = '{0}' AND LST>='{2}' AND MODULE = '{1}' AND EXISTS(SELECT 1 FROM D_DurationTest WHERE (CODE = 1) AND (DM = DURATIONID))", forecastDate, Module, dt);
            return strSQL;
        }
        public string SelectComReviseSQL(string hour, string period, string Module,string moduleStyle)
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
                        sb = GetForecast("0", hour, period, Module, "gengZ",moduleStyle);
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
            string strSQL = string.Format("FORECASTDATE = '{0}'", dtNow);//��ʱ����ÿ���18��
            strSQL = entity.BuildQuerySQL(strSQL, "");

            DataTable datePreTable = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder("{");

            //������json
            string json = GetForecastJSON(datePreTable);
            sb.Append(json);
            if (json != "")
                sb.Append(",");

            strSQL = String.Format("SELECT  DATEDIFF(DAY, LST, '{0}') - 1 AS LST, DURATIONID, ITEMID ,VALUE,AQI,Parameter FROM T_FORECASTGROUP WHERE FORECASTDATE = '{0}'", forecastDateTime);
            DataTable dTable = m_Database.GetDataTable(strSQL);

            //����ʵ�����ۺ�Ԥ����ģʽ���ݵ�json
            if (dTable.Rows.Count > 0)
            {
                //����json������ǰ̨��ֵ
                json = GetGroupJSON(dTable, 1, "H");//ʵ��typeID = 0;//�ۺ�Ԥ��typeID = 1//ģʽtypeID = 2

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
        /////// ������������
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
                //����json������ǰ̨��ֵ
                string json = GetGroupJSON(dTable, 1, "PH");//ʵ��typeID = 0;//�ۺ�Ԥ��typeID = 1//ģʽtypeID = 2

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
        /// ����Ԥ��Ԥ���������ݵ�ǰ��ID�ж��Ƿ���Ҫ������ƽ������������ƽ��
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

                //24СʱԤ��Ԥ��
                StringBuilder sb = new StringBuilder("{H09:\"");

                //ҹ��
                AQIExtention aqiExt = ConvertAQIDescription(tbContent, dtForecastDate, 6, 24);

                if (aqiExt != null)
                    sb.AppendFormat("{0}ҹ�䣬{1}��{2}��{3}��", dtForecastDate.ToString("M��d��"), aqiExt.AQI, aqiExt.Quality, aqiExt.FirstItem);
                //����
                aqiExt = ConvertAQIDescription(tbContent, dtForecastDate, 2, 24);
                if (aqiExt != null)
                    sb.AppendFormat("{0}���磬{1}��{2}��{3}��", dtForecastDate.AddDays(1).ToString("d��"), aqiExt.AQI, aqiExt.Quality, aqiExt.FirstItem);
                //����
                aqiExt = ConvertAQIDescription(tbContent, dtForecastDate, 3, 24);
                if (aqiExt != null)
                    sb.AppendFormat("���磬{0}��{1}��{2}��", aqiExt.AQI, aqiExt.Quality, aqiExt.FirstItem);


                //48СʱԤ��Ԥ��
                sb.Append("\",H10:\"");
                //ҹ��
                aqiExt = ConvertAQIDescription(tbContent, dtForecastDate, 6, 48);
                if (aqiExt != null)
                    sb.AppendFormat("{0}ҹ�䣬{1}��{2}��{3}��", dtForecastDate.AddDays(1).ToString("M��d��"), aqiExt.AQI, aqiExt.Quality, aqiExt.FirstItem);
                //����
                aqiExt = ConvertAQIDescription(tbContent, dtForecastDate, 2, 48);
                if (aqiExt != null)
                    sb.AppendFormat("{0}���磬{1}��{2}��{3}��", dtForecastDate.AddDays(2).ToString("d��"), aqiExt.AQI, aqiExt.Quality, aqiExt.FirstItem);
                //����
                aqiExt = ConvertAQIDescription(tbContent, dtForecastDate, 3, 48);
                if (aqiExt != null)
                    sb.AppendFormat("���磬{0}��{1}��{2}��", aqiExt.AQI, aqiExt.Quality, aqiExt.FirstItem);


                //24СʱԤ��Ԥ��
                //ҹ��
                sb.Append("\",PH10:\"");
                StringBuilder sm = new StringBuilder();
                string firstItem = "";
                aqiExt = ConvertAQIDescription(tbContent, dtForecastDate, 6, 24);
                if (aqiExt != null)
                {
                    if (Module != "Modify" && Module != "SMCModify")
                    {
                        sb.AppendFormat("Ԥ��{0}ҹ�䣬�ֶ�ָ��Ϊ{1}��", dtForecastDate.ToString("M��d��"), ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true));
                    }
                    if (ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('��')[1].ToString() == "��")
                    {
                        firstItem = "-";
                    }
                    else
                    {
                        firstItem = aqiExt.FirstPItemNoByGrade;
                    }

                    sm.AppendFormat("{0}ҹ��,{1},{2},{3},", dtForecastDate.ToString("d��"), ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('��')[0].ToString(), ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('��')[1].ToString() == "" ? "-" : ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('��')[1].ToString(), stringFormat(firstItem));
                }
                else
                {
                    sm.AppendFormat("{0}ҹ��,{1},{2},{3},", dtForecastDate.ToString("d��"), "/", "/", "/");
                }
                //����
                aqiExt = ConvertAQIDescription(tbContent, dtForecastDate, 2, 24);
                if (aqiExt != null)
                {
                    if (Module != "Modify" && Module != "SMCModify")
                    {
                        sb.AppendFormat("{0}���磬{1}��", dtForecastDate.AddDays(1).ToString("d��"), ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, false));
                    }
                    else
                    {
                        sb.AppendFormat("Ԥ��{0}���磬�ֶ�ָ��Ϊ{1}��", dtForecastDate.AddDays(1).ToString("M��d��"), ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true));
                    }
                    if (ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('��')[1].ToString() == "��")
                    {
                        firstItem = "-";
                    }
                    else
                    {
                        firstItem = aqiExt.FirstPItemNoByGrade;
                    }
                    sm.AppendFormat("{0}����,{1},{2},{3},", dtForecastDate.AddDays(1).ToString("d��"), ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('��')[0].ToString(), ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('��')[1].ToString() == "" ? "-" : ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('��')[1].ToString(), stringFormat(firstItem));
                }
                else
                {
                    sm.AppendFormat("{0}����,{1},{2},{3},", dtForecastDate.AddDays(1).ToString("d��"), "/", "/", "/");
                }
                //����
                aqiExt = ConvertAQIDescription(tbContent, dtForecastDate, 3, 24);
                if (aqiExt != null)
                {
                    sb.AppendFormat("���磬{0}��", ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, false));
                    if (ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('��')[1].ToString() == "��")
                    {
                        firstItem = "-";
                    }
                    else
                    {
                        firstItem = aqiExt.FirstPItemNoByGrade;
                    }
                    sm.AppendFormat("{0}����,{1},{2},{3},", dtForecastDate.AddDays(1).ToString("d��"), ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('��')[0].ToString(), ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('��')[1].ToString() == "" ? "-" : ParseAQIForSM(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true).Split('��')[1].ToString(), stringFormat(firstItem));
                }
                else
                {
                    sm.AppendFormat("{0}����,{1},{2},{3},", dtForecastDate.AddDays(1).ToString("d��"), "/", "/", "/");

                }
                sb.Append("\"");
                sm.Append(detail);
                sm.Append(",�Ϻ��л���������� �Ϻ���������̨,");
                sm.AppendFormat("{0}{1}ʱ����", dtForecastDate.ToString("M��d��"), DateTime.Now.Hour.ToString());
                string[] message = sm.ToString().Split(',');
                string existsSQL, updateSQL, insertSQL;
                existsSQL = @"SELECT foreDate FROM tb_AirForecast WHERE foreDate='" + dtForecastDate.ToString("yyyy��M��d��") + "'";
                updateSQL = @"UPDATE tb_AirForecast SET Seg1='" + message[0].ToString() + "',AQI1='" + message[1].ToString() + "',Grade1='" + message[2].ToString() + "',Param1='" + message[3].ToString() + "',Seg2='" + message[4].ToString() + "',AQI2='" + message[5].ToString() + "',Grade2='" + message[6].ToString() + "',Param2='" + message[7].ToString() + "',Seg3='" + message[8].ToString() + "',AQI3='" + message[9].ToString() + "',Grade3='" + message[10].ToString() + "',Param3='" + message[11].ToString() + "',Detail='" + message[12].ToString() + "',Sign='" + message[13].ToString() + "',publishTime='" + message[14].ToString() + "' WHERE foreDate='" + dtForecastDate.ToString("yyyy��M��d��") + "'";
                insertSQL = @"INSERT INTO tb_AirForecast VALUES('" + dtForecastDate.ToString("yyyy��M��d��") + "', '" + message[0].ToString() + "','" + message[1].ToString() + "','" + message[2].ToString() + "','" + message[3].ToString() + "','" + message[4].ToString() + "','" + message[5].ToString() + "','" + message[6].ToString() + "','" + message[7].ToString() + "','" + message[8].ToString() + "','" + message[9].ToString() + "','" + message[10].ToString() + "','" + message[11].ToString() + "','" + message[12].ToString() + "','" + message[13].ToString() + "','" + message[14].ToString() + "')";
                m_Database.Execute(existsSQL, updateSQL, insertSQL);


                //������ƽ��
                int period = 24;
                double rowsDate;
                if (itemID != "")
                {
                    string rowID = divID.Substring(1, 1);
                    if (rowID == "5")
                        period = 48;

                    string strFilter = string.Format("ForecastDate = '{0}' AND durationID = {1} AND PERIOD = {2} AND ITEMID = {3}", dtForecastDate, 7, period, itemID);
                    DataRow[] rows = tbContent.Select(strFilter);
                    if (rows[0][6] == DBNull.Value)//ָ��ģʽ��pm2.5 pm10����ָ��ʱ��Σ�7ָȫ�죬1�ϰ�ҹ����24Сʱ����48Сʱ���µ�AQIֵ
                        sb.AppendFormat(",{0}:''", divID);//Ϊ��ҲҪ���ϡ�����������ǰ̨���л��ᱨ��
                    else
                    {
                        aqiExt = new AQIExtention(int.Parse(rows[0][6].ToString()), int.Parse(itemID));
                        string aqiColor = string.Format("class='{0}'", aqiExt.Color);
                        string str = rows[0][5].ToString();//ָ��ģʽ��pm2.5 pm10����ָ��ʱ��Σ�7ָȫ�죬1�ϰ�ҹ����24Сʱ����48Сʱ���µ�VALUEֵ
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
        /// ���ն���Ҫ��������֯AQI��ֵ����׼���£�
        /// 1����Ԥ������AQI�����ϼ�����10��
        /// 2���Է�Χ����������ԭ���Ǹ�λ���ճ�5��0���շ�Χ��ԭ��Ϊ�ͽ�ԭ����72-92�ճ�70-90��73-93�ճ�75-95��
        /// 3�������������¼������������
        ///     a)         0-20����Ϊ1-20��
        ///     b)         50-70����Ϊ55-75
        ///     c)         100-120����Ϊ105-125
        ///     d)         150-170����Ϊ155-175
        ///     e)         200-220����Ϊ205-225
        ///     f)          300-320����Ϊ305-325
        /// 4�������󣬶Է�Χ���������ֱ���ȼ��������������һ���ȼ��ڣ�֮����һ���ȼ����ɣ�����������ȼ��ڣ����ɵ͵�������Ϊ**��**�������������Ⱦ��
        /// </summary>
        /// <param name="aqi"></param>
        /// <returns></returns>
        private string ParseAQIForSM(int aqi, string firstParameter, bool showDescription)
        {
            int i = aqi % 10;

            string description = "��Ҫ��Ⱦ��";
            if (showDescription == false)
                description = "";
            description = "��" + description + firstParameter;

            //2���Է�Χ����������ԭ���Ǹ�λ���ճ�5��0���շ�Χ��ԭ��Ϊ�ͽ�ԭ����72-92�ճ�70-90��73-93�ճ�75-95��
            if (i < 3)
                i = 0;
            else if (i > 7)
                i = 10;
            else
                i = 5;
            //1����Ԥ������AQI�����ϼ�����10��
            int fAQI = aqi / 10 * 10 - 10 + i;
            if (fAQI <= 0)
                fAQI = 1;
            int tAQI = aqi / 10 * 10 + 10 + i;

            //3�������������¼������������
            if (fAQI == 50 || fAQI == 100 || fAQI == 150 || fAQI == 200 || fAQI == 300)
            {
                fAQI = fAQI + 5;
                tAQI = tAQI + 5;
            }

            //4�������󣬶Է�Χ���������ֱ���ȼ��������������һ���ȼ��ڣ�֮����һ���ȼ����ɣ�����������ȼ��ڣ����ɵ͵�������Ϊ**��**�������������Ⱦ��
            AQIExtention fAqiExt = new AQIExtention(fAQI);
            AQIExtention tAqiExt = new AQIExtention(tAQI);
            string strGrade = fAqiExt.Quality;

            if (strGrade != tAqiExt.Quality)
            {
                strGrade = string.Format("{0}��{1}", strGrade, tAqiExt.Quality);
            }
            else if (strGrade == "��")//�ŵ�����£�����ʾ��ҳ��Ⱦ��
                description = "";
            if (strGrade == "�����Ⱦ���ж���Ⱦ")
                strGrade = "��ȵ��ж���Ⱦ";
            if (strGrade == "�ж���Ⱦ���ض���Ⱦ")
                strGrade = "�жȵ��ض���Ⱦ";
            if (strGrade == "�ض���Ⱦ��������Ⱦ")
                strGrade = "�ضȵ�������Ⱦ";

            string aqiSM = string.Format("{0}-{1}��{2}{3}", fAQI, tAQI, strGrade, description);

            return aqiSM;
        }




        /// <summary>
        /// �������ݷ���ָ��ʱ���AQI����
        /// </summary>
        /// <param name="tbContent">Ԥ������</param>
        /// <param name="dtForecastDate">��ʱ��</param>
        /// <param name="durationID">�ֶ�ID</param>
        /// <param name="period">Ԥ��ʱЧ</param>
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
        /// �����ۺ�Ԥ�����ֿ�����Ԥ������Ԥ������
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

            //����Ԥ����
            try
            {
                forecastDate = SaveForm(parts[0], Flag);

                //����Ԥ������
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
                return "����ɹ�";
            }
            catch (Exception ex)
            {
                m_Log.Error("SaveComForecastReSee", ex);
                return ex.ToString();
            }

        }



        /// <summary>
        /// �ύ��
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
            string formDate = GetManualForecastDate(keyValue[1]).ToString("yyyy-MM-dd HH:mm:ss");//�ۺ�Ԥ����ʱ��

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
            //���ھ͸��£������ھͲ���
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
        /// ��Ԥ����Ϣ�������ݿ⣬ͨ���������ݣ��ó�4��ʱЧ
        /// ���ݸ�ʽ˵����"H3141:1/2"��
        /// H����ǩ��ʶ������Ҫչʾ�ͱ༭�ı�ǩ
        /// 3����ʾ�кţ����ڼ����ۺ�Ԥ����Ԥ��ʱ�䣬
        /// 1����ʾ�������ͣ�ʵ��typeID = 0;//�ۺ�Ԥ��typeID = 1//ģʽtypeID = 2
        /// 4����ʾʱ�Σ���Ϊ1��0-6h��,2��6-12h��,3��12-18h��,4��18-24��,5��0-6h��,6��6-18h��,7��0-24h�����洢���ֵ��D_DurationTest����
        /// 1����ʾ��Ⱦ�1��PM2.5��,2��PM10��,3��NO2��,4��03-1h��,5��03-8h�����洢���ֵ��D_Item����
        /// :����ʾ���ݷָ���ǰ���������ݵ����������沿������Ⱦ��Ũ�Ⱥ�AQIֵ
        /// /����Ⱦ��Ũ�Ⱥ�AQIֵ�ķָ���ʶ
        /// ���ߣ���ΰ��      ���ڣ�2013��06��30��      
        /// </summary>
        /// <param name="forecastContent">��ʱ��</param>
        /// <returns>���ɹ�����true�����򷵻�false</returns>
        private bool SaveContent(string forecastDate, string forecastContent, string Module)
        {
            string strSQL;
            try
            {
                strSQL = string.Format("DELETE T_ForecastGroup_temp WHERE FORECASTDATE = '{0}' AND MODULE = '{1}'; INSERT INTO T_ForecastGroup_temp SELECT LST,ForecastDate,PERIOD,Module,durationID,ITEMID,Value,AQI,GROUPID,Parameter FROM  T_ForecastGroup WHERE FORECASTDATE = '{0}' AND MODULE = '{1}'", forecastDate, Module);
                m_Database.Execute(strSQL);
                strSQL = string.Format("DELETE T_ForecastGroup WHERE FORECASTDATE = '{0}' AND MODULE = '{1}'", forecastDate, Module);
                DataTable dt = CaculateContent(DateTime.Parse(forecastDate), forecastContent, Module);
                m_Database.Execute(strSQL);//ɾ�����м�¼
                return m_Database.BulkCopy(dt);
            }
            catch(Exception ex)
            {
                m_Log.Error("SaveContent", ex);
                return false;
            }

        }

        /// <summary>
        /// ����Ԥ�����ݣ������ر��
        /// </summary>
        /// <param name="forecastDate">��ʱ��</param>
        /// <param name="forecastContent">Ԥ������</param>
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
                //��ȡ�кţ���������е�Ԥ������
                rowIndex = int.Parse(keyValue[0].Substring(1, 1));
                lst = startDate.AddDays(rowIndex - m_BackDays - 1);

                //��ȡ�ֶ�ID
                durationID = int.Parse(keyValue[0].Substring(3, 1));
                newRow[3] = durationID;

                string[] durationSpan = GetDurationSpan(timePeriod, durationID);
                lst = lst.AddHours(int.Parse(durationSpan[0]));
                newRow[0] = lst;
                newRow[1] = forecastDate;

                //�뿪ʼʱ����Ȼ�ȡԤ��ʱЧ

                TimeSpan dateDiff = lst.Subtract(forecastDate);
                if (dateDiff.TotalHours >= 24)
                    period = 48;
                newRow[2] = period;

                //��ȡ��Ⱦ������ID
                itemID = int.Parse(keyValue[0].Substring(4, 1));
                newRow[4] = itemID;


                //��ȡ��Ⱦ��Ũ��
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
            //������죨6-20h����5��ҹ��20-6h����6��ȫ�죨0-24h����7 ������

            //��ȡ��Ⱦ���ID
            DataTable dicItem = dt.DefaultView.ToTable(true, "ITEMID");


            DataTable tmpTable = dt.Clone();

            //�洢�ֶ�ID
            int durationID = 5;

            DateTime stopDatetime;
            DateTime fromDatetime;
            DateTime endDatetime;
            string[] durationSpan;
            foreach (DataRow r in dicItem.Rows)
            {
                //����ÿһ����Ⱦ����Ҫ��ʼ��
                fromDatetime = forecastDate.Date;
                stopDatetime = forecastDate.AddDays(2);//2��ʱЧ��48СʱԤ��
                durationSpan = GetDurationSpan(timePeriod, durationID);//��ȡ�ֶ�ʱ�䷶Χ
                fromDatetime = fromDatetime.AddHours(int.Parse(durationSpan[0]));
                endDatetime = fromDatetime.Date.AddHours(int.Parse(durationSpan[1]));
                while (fromDatetime < stopDatetime.Date)
                {

                    DataRow newRow = AddNewRow(dt, tmpTable, forecastDate, fromDatetime, endDatetime, durationID, r[0], timePeriod, Module);
                    tmpTable.Rows.Add(newRow);
                    fromDatetime = fromDatetime.AddDays(1);
                    endDatetime = endDatetime.AddDays(1);
                }


                //1��ʱЧ��48СʱԤ��
                fromDatetime = forecastDate.Date.AddDays(1);
                stopDatetime = fromDatetime.AddDays(2); //2��ʱЧ��48СʱԤ��
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
        /// ����Duration�ֵ��������ӦID��ʱ������
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
        /// �ڱ��в���һ�У���Ҫ�Ǽ���ֶ�Ϊ5,6,7�ģ�����ƽ��ֵ�ļ�����Ҫ���ǵ��ֶε�ʱ��
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tmpTable"></param>
        /// <param name="forecastDate"></param>
        /// <param name="fromDatetime"></param>
        /// <param name="toDatetime"></param>
        /// <param name="durationID"></param>
        /// <param name="itemID"></param>
        /// <param name="timePeriod">�ֶ��ֵ��</param>
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
                string[] durationSpan;//��ȡ�ֶ�ʱ�䷶Χ
                int span;

                for (int i = 0; i < rows.Length; i++)
                {
                    durationSpan = GetDurationSpan(timePeriod, int.Parse(rows[i][3].ToString()));//��ȡ�ֶ�ʱ�䷶Χ
                    span = int.Parse(durationSpan[1]) - int.Parse(durationSpan[0]);
                    if (span < 0)
                        span = 24 + span;//����
                    if (rows[i][5] != DBNull.Value)
                    {
                        sumValue = sumValue + double.Parse(rows[i][5].ToString()) * span;
                        totalHours = totalHours + span;
                    }
                }
                //����ƽ��ֵ
                if (totalHours == 0)//��û��ֵ�������
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
        //����ѡ�������Ͷ���
        public string PublicData(string checkBoxSelect, string content, string forecastDate, string phones, string phonesDX, string publishTime, string userName, string ForecastStyle)
        {
            DateTime forecastTime = DateTime.Parse(forecastDate).AddHours(18);
            try
            {
                string strSQL = @"UPDATE tb_AirForecast SET publishTime='" + publishTime + "' WHERE foreDate='" + forecastTime.ToString("yyyy��M��d��") + "'";
                m_Database.Execute(strSQL);
                StringBuilder returnString = new StringBuilder("{");
                string duanXin = "";
                string SHBJMess = "";
                string XJZXMess = "";
                string SSFBXT = "";
                string weiboMess = "";
                string TencentWeiBoStr;
                string DXMess = "";
                //�ƶ�����
                if (checkBoxSelect.IndexOf("0") >= 0)
                {
                    duanXin = SendSM(content, forecastDate, phones, userName, "1", ForecastStyle);
                    returnString.Append(duanXin);
                    returnString.Append("��");
                }
                //�л�����
                if (checkBoxSelect.IndexOf("1") >= 0)
                {
                    SHBJMess = SHBJ(forecastDate, userName, "1", "", ForecastStyle);
                    returnString.Append(SHBJMess);
                    returnString.Append("��");
                }
                //��������
                if (checkBoxSelect.IndexOf("2") >= 0)
                {
                    XJZXMess = XJZX(forecastDate, userName, "1", "", ForecastStyle);
                    returnString.Append(XJZXMess);
                    returnString.Append("��");
                }
                //ʵʱ����ϵͳ
                if (checkBoxSelect.IndexOf("3") >= 0)
                {
                    SSFBXT = InsertDataBase(content, forecastDate, userName, "1", ForecastStyle);
                    returnString.Append(SSFBXT);
                    returnString.Append("��");
                }
                //����΢��
                if (checkBoxSelect.IndexOf("4") >= 0)
                {
                    weiboMess = weiBo(content, forecastDate, userName, "1", ForecastStyle);
                    returnString.Append(weiboMess);
                    returnString.Append("��");
                }
                //����
                if (checkBoxSelect.IndexOf("5") >= 0)
                {
                    DXMess = SendSMDX(content, forecastDate, phonesDX, userName, "1", ForecastStyle);
                    returnString.Append(DXMess);
                    returnString.Append("��");
                }
                //��Ѷ΢��
                if (checkBoxSelect.IndexOf("6") >= 0)
                {
                    TencentWeiBoStr = TencentWeiBo(content, forecastDate, userName, "1", ForecastStyle);
                    returnString.Append(TencentWeiBoStr);
                    returnString.Append("��");
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

                //string[] conAry = content.Split('��');
                //string firstStr = conAry[1];
                //string zjStr = conAry[4];
                //int indexPos = content.IndexOf(firstStr);
                //int zjindexPos = content.IndexOf(zjStr);
                //if (conAry.Length > 7)
                //{
                //    string endStr = conAry[7];
                //    int endindexPos = content.IndexOf(endStr);
                //    content = conAry[0] + "��" + conAry[2] + "��" + conAry[3] + "��" + conAry[5] + "��" + conAry[6] + "��" + conAry[8] + "��" + conAry[9];
                //}
                //else
                //{
                //    content = conAry[0] + "��" + conAry[2] + "��" + conAry[3] + "��" + conAry[5] + "��" + conAry[6];
                //}

                string TencentReturn = SendWeiBo.SendTencent(content);
                if (TencentReturn == "�ɹ�")
                {
                    sendInfo = "���ͳɹ�";
                    returnStrInfo = "��Ѷ΢�����ͳɹ�";
                }
                else
                {
                    sendInfo = "����ʧ��";
                    returnStrInfo = "��Ѷ΢������ʧ�ܣ�ԭ����" + TencentReturn;
                }
                if (count == 1)
                    strSQL = "INSERT INTO T_SendLog VALUES('" + DateTime.Now.ToString() + "','��Ѷ΢��','" + ForecastStyle + "', '" + content + "', '" + userName + "', '" + sendInfo + "','1','','" + forecastDate + "')";
                else
                    strSQL = "UPDATE T_SendLog SET Recount='" + count + "',Message='" + sendInfo + "'  WHERE DateTime='" + forecastDate + "' AND PublicStyle='��Ѷ΢��'";
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

                if (sinaReturn == "�ɹ�")
                {
                    sendInfo = "���ͳɹ�";
                    returnStrInfo = "����΢�����ͳɹ�";

                }
                else
                {
                    sendInfo = "����ʧ��";
                    returnStrInfo = "����΢������ʧ�ܣ�ԭ����" + sinaReturn;
                }
                if (count == 1)
                    strSQL = "INSERT INTO T_SendLog VALUES('" + DateTime.Now.ToString() + "','����΢��','" + ForecastStyle + "', '" + content + "', '" + userName + "', '" + sendInfo + "','1','','" + forecastDate + "')";
                else
                    strSQL = "UPDATE T_SendLog SET Recount='" + count + "',Message='" + sendInfo + "'  WHERE DateTime='" + forecastDate + "' AND PublicStyle='����΢��'";
                m_Database.Execute(strSQL);
                return returnStrInfo;
            }
            catch (Exception ex)
            {
                m_Log.Error("weiBo", ex);
                return ex.Message;
            }
        }
        //�������л����֡�����
        public string SHBJ(string forecastDate, string userName, string countNum, string content, string ForecastStyle)
        {
            DateTime forecastTime = DateTime.Parse(forecastDate).AddHours(18);
            string strSQL;
            string sendInfo;
            string returnStrInfo = "";
            string sentContent;
            int count = int.Parse(countNum); 
            try
            {
                if (content != "")
                {
                    strSQL = "SELECT * FROM tb_AirForecast WHERE foreDate='" + forecastTime.ToString("yyyy��M��d��") + "'";
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
                    returnStrInfo = string.Format("�л����ַ���{0}", result);
                    if (result == "�ɹ�")
                        sendInfo = "���ͳɹ�";
                    else
                        sendInfo = "����" + result;

                }
                catch (Exception ex)
                {
                    sendInfo = "����ʧ��";
                    returnStrInfo = string.Format("�л����ַ���ʧ��,ԭ����{0}", ex.ToString());
                }
                if (count == 1)
                    strSQL = "INSERT INTO T_SendLog VALUES('" + DateTime.Now.ToString() + "','�л�����','" + ForecastStyle + "', '" + sentContent + "', '" + userName + "', '" + sendInfo + "','1','','" + forecastDate + "')";
                else
                    strSQL = "UPDATE T_SendLog SET Recount='" + count + "',Message='" + sendInfo + "'  WHERE DateTime='" + forecastDate + "' AND PublicStyle='�л�����'";
                m_Database.Execute(strSQL);
                return returnStrInfo;
            }
            catch (Exception ex)
            {
                m_Log.Error("SHBJ", ex);
                return ex.Message;
            }

        }
        //������������
        public string XJZX(string forecastDate, string userName, string countNum, string content, string ForecastStyle)
        {
            DateTime forecastTime = DateTime.Parse(forecastDate).AddHours(18);
            string strSQL;
            string returnStrInfo = "";
            string sentContent;
            string foreContent = "";
            string sendInfo;
            int count = int.Parse(countNum); 
            try
            {
                if (content != "")
                {
                    strSQL = "SELECT * FROM tb_AirForecast WHERE foreDate='" + forecastTime.ToString("yyyy��M��d��") + "'";
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


                ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["WebService"];
                string constring = settings.ConnectionString;
                string[] parts = constring.Split(new char[] { ';', '=' }, StringSplitOptions.None);
                Lucas.importAQI imAQI = new Lucas.importAQI();
                imAQI.Url = "http://www.envir.gov.cn/aqiforseec/importAQI.asmx";
                //WebProxy proxy = new WebProxy();
                //proxy = new WebProxy(parts[1], true);  //�����������Ϣ��ͨ��config�ļ�����            
                //proxy.Credentials = new NetworkCredential(parts[7], parts[9], parts[3]); //����UserName, Password��Domain��ͨ��config�ļ�����                      
                //imAQI.Proxy = proxy;
                try
                {
                    string resultString = "";
                    resultString = imAQI.importAQIForecast(sentContent, "|", parts[11]);
                    returnStrInfo = string.Format("�������ķ���{0}", resultString);
                    sendInfo = "����"+resultString;
                }
                catch (Exception ex)
                {
                    sendInfo = "����ʧ��";
                    returnStrInfo = string.Format("�������ķ���ʧ��,ԭ����{0}", ex.ToString());
                }
                if (count == 1)
                    strSQL = "INSERT INTO T_SendLog VALUES('" + DateTime.Now.ToString() + "','��������','" + ForecastStyle + "', '" + sentContent + "', '" + userName + "', '" + sendInfo + "','1','','" + forecastDate + "')";
                else
                    strSQL = "UPDATE T_SendLog SET Recount='" + count + "',Message='" + sendInfo + "'  WHERE DateTime='" + forecastDate + "' AND PublicStyle='��������'";
                m_Database.Execute(strSQL);
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
            if (ForecastStyle == "�ۺ�Ԥ��")
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
                        strSQL = "SELECT * FROM tb_AirForecast WHERE foreDate='" + forecastTime.ToString("yyyy��M��d��") + "'";
                        DataTable db = m_Database.GetDataTable(strSQL);
                        db.TableName = "tb_AirForecast";
                        Database m_DatabaseNew = new Database("SEMCAIR");
                        strSQL = "TRUNCATE TABLE tb_AirForecast";
                        m_DatabaseNew.Execute(strSQL);//ɾ�����м�¼
                        strSQL = "TRUNCATE TABLE semc_air_1hr.DBO.tb_AirForecast";
                        m_DatabaseNew.Execute(strSQL);
                        bool returnStr = m_DatabaseNew.BulkCopy(db);
                        strSQL = "insert into semc_air_1hr.dbo.tb_AirForecast(foreDate,Seg1,AQI1,Grade1,Param1,Seg2,AQI2,Grade2,Param2,Seg3,AQI3,Grade3,Param3,Detail,Sign,publishTime) select foreDate,Seg1,AQI1,Grade1,Param1,Seg2,AQI2,Grade2,Param2,Seg3,AQI3,Grade3,Param3,Detail,Sign,publishTime from tb_AirForecast";
                        m_DatabaseNew.Execute(strSQL);
                        if (returnStr)
                        {
                            sendInfo = "���ͳɹ�";
                            returnStrInfo = "ʵʱ����ϵͳ���ͳɹ�";
                        }
                        else
                        {
                            sendInfo = "����ʧ��";
                            returnStrInfo = "ʵʱ����ϵͳ����ʧ��";
                        }

                    }
                    catch (Exception ex)
                    {
                        sendInfo = "����ʧ��";
                        returnStrInfo = "ʵʱ����ϵͳ����ʧ�ܣ�" + ex.ToString();
                    }

                }
                catch (Exception ex)
                {
                    sendInfo = "����ʧ��";
                    returnStrInfo = string.Format("ʵʱ����ϵͳ����ʧ��,ԭ����{0}", ex.ToString());

                }
                if (count == 1)
                    strSQL = "INSERT INTO T_SendLog VALUES('" + DateTime.Now.ToString() + "','ʵʱ����ϵͳ','" + ForecastStyle + "', '" + content + "', '" + userName + "', '" + sendInfo + "','1','','" + forecastDate + "')";
                else
                    strSQL = "UPDATE T_SendLog SET Recount='" + count + "',Message='" + sendInfo + "'  WHERE DateTime='" + forecastDate + "' AND PublicStyle='ʵʱ����ϵͳ'";
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
        /// ʵ��Ԥ�����ݵĶ��ŷ��ͣ������ط��ͽ�����ѷ��͵�������
        /// <param name="content">��������</param>
        /// <param name="forecastDate">Ԥ������</param>
        /// <param name="phones">�绰����</param>
        /// <param name="userName">������Ա</param>
        /// <param name="count">���ʹ���</param>
        /// <param name="ForecastStyle">Ԥ������</param>
        /// <returns>�����Ƿ��ͳɹ��Ľ��</returns>
        public string SendSMDX(string content, string forecastDate, string phones, string userName, string countNum, string ForecastStyle)
        {

            string returnMsg = "";
            string strSQL = "";
            string sendInfo = "���ͳɹ�";
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
                    returnMsg = "������ͨ����ʧ��,ԭ����" + loginReturn;
                    sendInfo = "����ʧ��";
                }
                else
                {
                    sendMessParams[0] = loginReturn;
                    sendMessParams[1] = "NORM";
                    sendMessParams[2] = phones;
                    sendMessParams[3] = DateTime.Now;
                    sendMessParams[5] = "";
                    sendMessParams[6] = false;
                    //��ȡ��Ҫ���͵��ֻ�����
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
                            sendInfo = "����ʧ��";
                            returnMsg = "������ͨ����ʧ��,ԭ����" + loginReturn;
                        }
                        else
                        {
                            sendInfo = "���ͳɹ�";
                            returnMsg = "������ͨ���ͳɹ���";
                        }

                    }
                    else
                    {
                        returnMsg = "������ͨʧ�ܣ�û��Ҫ���͵��ֻ�����";
                        sendInfo = "����ʧ��";
                    }
                }
                string[] loginOff = new string[1];
                loginOff[0] = loginReturn;
                WebServiceHelper.InvokeWebService(url, "UCPPlatWebServiceService", "logoff", loginOff);
                if (count == 1)
                    strSQL = "INSERT INTO T_SendLog VALUES('" + DateTime.Now.ToString() + "','������ͨ','" + ForecastStyle + "', '" + content + "', '" + userName + "', '" + sendInfo + "','1','" + phones + "','" + forecastDate + "')";
                else
                    strSQL = "UPDATE T_SendLog SET Recount='" + count + "',Message='" + sendInfo + "'  WHERE DateTime='" + forecastDate + "' AND PublicStyle='������ͨ'";
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
        /// <summary>
        /// ʵ��Ԥ�����ݵĶ��ŷ��ͣ������ط��ͽ�����ѷ��͵�������
        /// <param name="content">��������</param>
        /// <param name="forecastDate">Ԥ������</param>
        /// <param name="phones">�绰����</param>
        /// <param name="userName">������Ա</param>
        /// <param name="count">���ʹ���</param>
        /// <param name="ForecastStyle">Ԥ������</param>
        /// <returns>�����Ƿ��ͳɹ��Ľ��</returns>
        public string SendSM(string content, string forecastDate, string phones, string userName, string  countNum, string ForecastStyle)
        {
            try
            {
                MasSender masSender = new MasSender();
                string returnMsg = string.Empty;
                string strSQL = "";
                int j = 0;
                int count = int.Parse(countNum);
                //��ȡ��Ҫ���͵��ֻ�����
                if (phones != "")
                {
                    string[] mobiles = phones.Split(',');
                    string phonesJoin = string.Join(",", mobiles);
                    //���Ͷ���
                    int ret = masSender.SendSM(mobiles, content);
                    masSender.Relese();
                    string sendInfo;
                    if (ret == 0)
                        sendInfo = "���ͳɹ�";
                    else
                        sendInfo = "����ʧ��";
                    if (count == 1)
                        strSQL = "INSERT INTO T_SendLog VALUES('" + DateTime.Now.ToString() + "','�ƶ�����','" + ForecastStyle + "', '" + content + "', '" + userName + "', '" + sendInfo + "','1','" + phonesJoin + "','" + forecastDate + "')";
                    else
                        strSQL = "UPDATE T_SendLog SET Recount='" + count + "',Message='" + sendInfo + "'  WHERE DateTime='" + forecastDate + "' AND PublicStyle='�ƶ�����'";
                    m_Database.Execute(strSQL);
                    switch (ret)
                    {
                        case 0:
                            returnMsg = string.Format("�����ƶ����ųɹ�");
                            break;
                        case -1:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ�����������ݿ����,���Ϊ{0}", ret);
                            break;
                        case -2:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ�������ݿ�ر�ʧ��,���Ϊ{0}", ret);
                            break;
                        case -3:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ�������ݿ�������,���Ϊ{0}", ret);
                            break;
                        case -4:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ�������ݿ�ɾ������,���Ϊ{0}", ret);
                            break;
                        case -5:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ�������ݿ��ѯ���󣬱��Ϊ{0}", ret);
                            break;
                        case -6:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ�������ݿ�������󣬱��Ϊ{0}", ret);
                            break;
                        case -7:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ����API����Ƿ������Ϊ{0}", ret);
                            break;
                        case -8:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ���ǲ������������Ϊ{0}", ret);
                            break;
                        case -9:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ����û�г�ʼ�����ʼ��ʧ�ܣ����Ϊ{0}", ret);
                            break;
                        case -10:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ����API�ӿڴ�����ͣ��ʧЧ��״̬�����Ϊ{0}", ret);
                            break;
                        case -11:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ���Ƕ�������δ���ӣ����Ϊ{0}", ret);
                            break;
                        case 1:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ���Ƿ�������Ϊ�գ����Ϊ{0}", ret);
                            break;
                        case 2:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ���Ƿ��������д��ڱ���ֹ���飬���Ϊ{0}", ret);
                            break;
                        case 3:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ�����ֻ����벻��ȷ�����Ϊ{0}", ret);
                            break;
                        case 4:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ�����ֻ�����Ϊ��Ӫ������ֹ�����Ϊ{0}", ret);
                            break;
                        case 5:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ�����ֻ�������ں������У����Ϊ{0}", ret);
                            break;
                        case 6:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ�����ֻ����벻���ڰ������У����Ϊ{0}", ret);
                            break;
                        case 7:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ������ҵǷ�ѣ����Ϊ{0}", ret);
                            break;
                        case 8:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ����ͨѶ�쳣�����Ϊ{0}", ret);
                            break;
                        case 101:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ����ϵͳ���󣬱��Ϊ{0}", ret);
                            break;
                        case 102:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ�ԭ���Ƕ��������޷������ֻ������Ϊ{0}", ret);
                            break;
                        default:
                            returnMsg = string.Format("�����ƶ�����ʧ�ܣ����Ϊ{0}", ret);
                            break;
                    }
                }
                else
                    returnMsg = "�����ƶ�����ʧ�ܣ�û��Ҫ���͵��ֻ�����";
                m_Log.Info("SendSM:" + returnMsg);
                return returnMsg;
            }
            catch (Exception ex)
            {
                m_Log.Error("SendSM", ex);
                return ex.Message;
            }
        }
        public string changeEdits(string forecastDate,string module)
        {
            string forecastTime = DateTime.Parse(forecastDate).ToString("yyyy��M��d��");
            int hour=int.Parse(DateTime.Now.Hour.ToString());
            string strSQL = "SELECT * FROM tb_AirForecast WHERE foreDate='" + forecastTime + "'";
            DataTable dt = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table id='changeDatable'  width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.AppendLine("<tr>");

            //����̧ͷ
            sb.AppendLine("<td class='tableEditorRight'>ʱ��</td>");
            sb.AppendLine("<td class='tableEditor'>��������</td>");
            sb.AppendLine("<td class='tableEditor'>��Ҫ��Ⱦ��</td>");
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
            sb.AppendLine(string.Format("<label id='lableEditor' class='lableStyle'>����������</label><div class='tableAirSign divInputTypeNew' id='Detail' contenteditable='true'>{0}</div>", dt.Rows[0]["Detail"]));
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
            existsSQL = @"SELECT foreDate FROM tb_AirForecast WHERE foreDate='" + dtForecastDate.ToString("yyyy��M��d��") + "'";
            updateSQL = @"UPDATE tb_AirForecast SET Seg1='" + SegValue[0] + "',AQI1='" + AQIValue[0] + "',Grade1='" + GradeValue[0] + "',Param1='" + ParamValue[0] + "',Seg2='" + SegValue[1] + "',AQI2='" + AQIValue[1] + "',Grade2='" + GradeValue[1] + "',Param2='" + ParamValue[1] + "',Seg3='" + SegValue[2] + "',AQI3='" + AQIValue[2] + "',Grade3='" + GradeValue[2] + "',Param3='" + ParamValue[2] + "',Detail='" + Detail + "',Sign='" + SignValue + "',publishTime='" + PublicTime + "' WHERE foreDate='" + dtForecastDate.ToString("yyyy��M��d��") + "'";
            insertSQL = @"INSERT INTO tb_AirForecast VALUES('" + dtForecastDate.ToString("yyyy��M��d��") + "', '" + SegValue[0] + "','" + AQIValue[0] + "','" + GradeValue[0] + "','" + ParamValue[0] + "','" + SegValue[1] + "','" + AQIValue[1] + "','" + GradeValue[1] + "','" + ParamValue[1] + "','" + SegValue[2] + "','" + AQIValue[2] + "','" + GradeValue[2] + "','" + ParamValue[2] + "','" + Detail + "','" + SignValue + "','" + PublicTime + "')";
            try
            {
                int count = m_Database.Execute(existsSQL, updateSQL, insertSQL);
                if (count > 0)
                    return "���³ɹ�";
                else
                    return "����ʧ��";
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

            //����̧ͷ
            sb.AppendLine("<td class='tableStateLeft'>���</td>");
            sb.AppendLine("<td class='tableStateMiddle' >��������</td>");
            sb.AppendLine("<td class='tableState'>״̬</td>");
            sb.AppendLine("<td class='tableState'>���·���</td>");
            sb.AppendLine("<td class='tableStateRight'>����</td>");
            sb.AppendLine("</tr>");
            int index = 1;
            if (publicStyle.IndexOf('0') >= 0)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine(string.Format("<td class='tableStateLeft'>{0}</td>", index++));
                sb.AppendLine(string.Format("<td class='tableStateMiddle'>�ƶ��û�</td>"));
                sb.AppendLine(string.Format("<td class='tableState'><img id='ydUserImg'  src='images/wait.gif'/></td>"));
                sb.AppendLine(string.Format("<td class='tableState'><a href=\"javascript:reSentMobile();\"><img src='images/send.gif'/></a></td>"));
                sb.AppendLine(string.Format("<td class='tableStateRight' id='returnYDMessage'>���ڷ���</td>"));
                sb.AppendLine("</tr>");
            }
            if (publicStyle.IndexOf('1') >= 0)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine(string.Format("<td class='tableStateLeft'>{0}</td>", index++));
                sb.AppendLine(string.Format("<td class='tableStateMiddle'>�л�����</td>"));
                sb.AppendLine(string.Format("<td class='tableState'><img  id='shbjImg' src='images/hc.png'/></td>"));
                sb.AppendLine(string.Format("<td class='tableState'><a href=\"javascript:reSHBJ();\"><img src='images/send.gif'/></a></td>"));
                sb.AppendLine(string.Format("<td class='tableStateRight' id='returnSHBJMessage'>���ڷ���</td>"));
                sb.AppendLine("</tr>");
            }
            if (publicStyle.IndexOf('2') >= 0)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine(string.Format("<td class='tableStateLeft'>{0}</td>", index++));
                sb.AppendLine(string.Format("<td class='tableStateMiddle'>��������</td>"));
                sb.AppendLine(string.Format("<td class='tableState'><img id='xjzxImg' src='images/hc.png'/></td>"));
                sb.AppendLine(string.Format("<td class='tableState'><a href=\"javascript:reXJZX();\"><img src='images/send.gif'/></a></td>"));
                sb.AppendLine(string.Format("<td class='tableStateRight' id='returnXJZXMessage'>���ڷ���</td>"));
                sb.AppendLine("</tr>");
            }
            if (publicStyle.IndexOf('3') >= 0)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine(string.Format("<td class='tableStateLeft'>{0}</td>", index++));
                sb.AppendLine(string.Format("<td class='tableStateMiddle'>ʵʱ����ϵͳ</td>"));
                sb.AppendLine(string.Format("<td class='tableState'><img  id='ssfbImg' src='images/hc.png'/></td>"));
                sb.AppendLine(string.Format("<td class='tableState'><a href=\"javascript:reSSFB();\"><img src='images/send.gif'/></a></td>"));
                sb.AppendLine(string.Format("<td class='tableStateRight' id='returnSSFBMessage'>���ڷ���</td>"));
                sb.AppendLine("</tr>");
            }
            if (publicStyle.IndexOf('4') >= 0)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine(string.Format("<td class='tableStateLeft'>{0}</td>", index++));
                sb.AppendLine(string.Format("<td class='tableStateMiddle'>����΢��</td>"));
                sb.AppendLine(string.Format("<td class='tableState'><img id='xlwbImg' src='images/hc.png'/></td>"));
                sb.AppendLine(string.Format("<td class='tableState'><a href=\"javascript:reXLWB();\"><img src='images/send.gif'/></a></td>"));
                sb.AppendLine(string.Format("<td class='tableStateRight' id='returnXLWBMessage'>���ڷ���</td>"));
                sb.AppendLine("</tr>");
            }
            if (publicStyle.IndexOf('5') >= 0)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine(string.Format("<td class='tableStateLeft'>{0}</td>", index++));
                sb.AppendLine(string.Format("<td class='tableStateMiddle'>��ͨ����</td>"));
                sb.AppendLine(string.Format("<td class='tableState'><img  id='dtdxImg' src='images/hc.png'/></td>"));
                sb.AppendLine(string.Format("<td class='tableState'><a href=\"javascript:reLTDX();\"><img src='images/send.gif'/></a></td>"));
                sb.AppendLine(string.Format("<td class='tableStateRight' id='returnLTDXMessage'>���ڷ���</td>"));
                sb.AppendLine("</tr>");
            }
            if (publicStyle.IndexOf('6') >= 0)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine(string.Format("<td class='tableStateLeft'>{0}</td>", index++));
                sb.AppendLine(string.Format("<td class='tableStateMiddle'>��Ѷ΢��</td>"));
                sb.AppendLine(string.Format("<td class='tableState'><img id='txwbImg' src='images/hc.png'/></td>"));
                sb.AppendLine(string.Format("<td class='tableState'><a href=\"javascript:reTXWB();\"><img src='images/send.gif'/></a></td>"));
                sb.AppendLine(string.Format("<td class='tableStateRight' id='returnTXWBMessage'>���ڷ���</td>"));
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</table>");
            return sb.ToString();
        }


        private void ProcessDataForecast(DataSet temp, int count, string fromDate, string toDate)
        {
            #region ���ݴ���
            forecastDt.Clear();
            int z = 0;
            int w = 0;
            int h = 0;
            int t =2;
            for (int i = 0; i < temp.Tables.Count; i++)
            {
                DataTable dt = temp.Tables[i];
                string tableName = dt.TableName.ToString();
                int items = int.Parse(tableName.Substring(5, 1));
                int m = 0;
                int k = 0;
                int x = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    #region �ж�Ҫ��ӵ������м���ֻ���״β�ѯ������
                    if (i == 0)
                    {
                        DataRow forecastRow = forecastDt.NewRow();
                        forecastDt.Rows.Add(forecastRow);
                    }
                    k++;
                    if (z == 0)
                        x = 0;
                    else
                        x = 4;

                    if (i == temp.Tables.Count - 1) {
                        t = 0;
                    }

                    for (int j = x; j < dt.Columns.Count - t; j++)
                    {
                        if (j == 0)
                        {
                            if (m == 0 || m % count == 0)
                            {
                                //�������
                                forecastDt.Rows[m]["RQ"] = dr[j].ToString();
                            }
                        }
                        else
                        {
                            #region �����Ӧ��Ԥ����ʵ������
                            string value = "";
                            if ((items == 0 && (j == 4 || j == 6 || j == 8 || j == 10)) || ((j == 9 || j == 8 || j == 10 || j == 11) && items != 0))
                            {
                                if (dr[j].ToString() != "")
                                    value = dr[j].ToString();
                                else
                                    value = dr[j].ToString() == "" ? "/ " : dr[j].ToString();

                            }
                            else
                                value = dr[j].ToString() == "" ? "/ " : dr[j].ToString();


                            forecastDt.Rows[m][w + 1] = value;
                            w++;
                            h = w;
                            #endregion
                        }
                    }
                    w = z;
                    m++;
                   #endregion 
                }
                z = h;
                w = h;
            }
            #endregion 
        
        }


        public DataTable GetEvaluationStringII(string fromDate, string toDate)
        {
            //��ȡ�������ݣ�����ʱ���
            string strSQL = "SELECT * FROM T_Evaluation WHERE LST between '{0}' and '{1}' order by Lst";
            DateTime startDate = DateTime.Parse(fromDate);
            DateTime endDate = DateTime.Parse(toDate);
            strSQL = string.Format(strSQL, startDate, endDate);
            DataTable db = m_Database.GetDataTable(strSQL);
            StringBuilder htmlSbd = new StringBuilder();
            evaDt.Clear();
            #region �������ݴ���
                foreach (DataRow row in db.Rows)
                {
                    string LST = row["LST"].ToString().Trim();
                    DataRow[] rows = evaDt.Select("LST = '" + LST + "' ");
                    if (rows == null || rows.Length <= 0)
                    {
                        DataRow newRow = evaDt.NewRow();
                        newRow["LST"] = row["LST"].ToString().Trim();
                        evaDt.Rows.Add(newRow);
                        rows = evaDt.Select("LST = '" + LST + "' ");
                    }
                    if (rows != null && rows.Length > 0)
                    {
                        if (row["duration"].ToString() == "ҹ��")
                            rows[0]["Night"] = row["Score"].ToString().Trim();

                        if (row["duration"].ToString() == "����")
                            rows[0]["AM"] = row["Score"].ToString().Trim();

                        if (row["duration"].ToString() == "����")
                            rows[0]["PM"] = row["Score"].ToString().Trim();

                        if (row["duration"].ToString() == "ȫ��")
                        {
                            rows[0]["DayAVG"] = row["Score"].ToString().Trim();
                            rows[0]["SocreCount"] = row["ScoreCount"].ToString().Trim();
                        }
                    }
                }
                #endregion ss
            return evaDt;
        }



        /// <summary>
        /// ��ѯ��������
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="count"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string GetEvaluationString( string fromDate, string toDate) {
            //��ȡ�������ݣ�����ʱ���
            string strSQL = "SELECT * FROM T_Evaluation WHERE LST between '{0}' and '{1}' order by Lst";
            DateTime startDate = DateTime.Parse(fromDate);
            DateTime endDate = DateTime.Parse(toDate);
            strSQL = string.Format(strSQL, startDate, endDate);
            DataTable db = m_Database.GetDataTable(strSQL);

            StringBuilder sb = new StringBuilder();
            string str = "<table id='{0}' width='100%' border='0' cellpadding='0' cellspacing='0' class='tablekuang'>";
            sb.AppendLine(string.Format(str, "table1"));
            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<th class='tabletitle'>��ʱ��</th>");
            sb.AppendLine("<th class='tabletitle' >ҹ��</th>");
            sb.AppendLine("<th class='tabletitle' >����</th>");
            sb.AppendLine("<th class='tabletitle' >����</th>");
            sb.AppendLine("<th class='tabletitle' >��ƽ��</th>");
            sb.AppendLine("<th class='tabletitle' >�ܼƵ÷�</th>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");
            sb.AppendLine("<tbody>");
            sb.AppendLine("{0}");
            sb.AppendLine("</tbody>");
            sb.Append("</table>");

            StringBuilder htmlSbd = new StringBuilder();
            if (db != null && db.Rows.Count > 0) {
                evaDt.Clear();
                #region �������ݴ���
                foreach (DataRow row in db.Rows){
                    string LST=row["LST"].ToString().Trim();
                    DataRow[] rows=evaDt.Select("LST = '" + LST + "' ");
                    if (rows == null || rows.Length <= 0)
                    {
                        DataRow newRow = evaDt.NewRow();
                        newRow["LST"] = row["LST"].ToString().Trim();
                        evaDt.Rows.Add(newRow);
                        rows = evaDt.Select("LST = '" + LST + "' ");
                    }
                    if (rows != null && rows.Length > 0)
                    {
                        if (row["duration"].ToString() == "ҹ��")
                            rows[0]["Night"] = row["Score"].ToString().Trim();

                        if (row["duration"].ToString() == "����")
                            rows[0]["AM"] = row["Score"].ToString().Trim();

                        if (row["duration"].ToString() == "����")
                            rows[0]["PM"] = row["Score"].ToString().Trim();

                        if (row["duration"].ToString() == "ȫ��")
                        {
                            rows[0]["DayAVG"] = row["Score"].ToString().Trim();
                            rows[0]["SocreCount"] = row["ScoreCount"].ToString().Trim();
                        }
                    }
                }
                #endregion ss

                string tr = "<tr  onmouseover='mouseOver(this)' onmouseout='mouseOut(this)' id='{0}'>";
                float totleScore = 0f;
                foreach (DataRow row in evaDt.Rows) {   
                
                    htmlSbd.Append(string.Format(tr, Guid.NewGuid().ToString()));
                    foreach(DataColumn dc in evaDt.Columns)
                    {
                        string td = "<td class='tablerow' id='{1}'>{0}</td>";
                        string value=row[dc.ColumnName].ToString();
                        if (string.IsNullOrEmpty(value)) {
                            value = "/";
                        }
                        if (dc.ColumnName == "SocreCount") {

                            float v=0f;
                            float.TryParse(row[dc.ColumnName].ToString(),out v);
                            totleScore += (v);
                        }
                        if (dc.ColumnName == "LST") {
                            value = DateTime.Parse(value).ToString("yyyy��MM��dd��");
                        }
                        htmlSbd.Append(string.Format(td, value, Guid.NewGuid().ToString()));
                    }
                    htmlSbd.Append("</tr>");
                }
                //����ܼ�
                htmlSbd.Append(string.Format(tr, Guid.NewGuid().ToString()));
                string tdS = "<td   class='tablerow' id='{1}'>{0}</td>";
                string tdE = "<td class='tablerow' rowspan='5' id='{1}'>{0}</td>";
                htmlSbd.Append(string.Format(tdS, "", Guid.NewGuid().ToString()));
                htmlSbd.Append(string.Format(tdS, "", Guid.NewGuid().ToString()));
                htmlSbd.Append(string.Format(tdS, "", Guid.NewGuid().ToString()));
                htmlSbd.Append(string.Format(tdS, "", Guid.NewGuid().ToString()));
                htmlSbd.Append(string.Format(tdS, "", Guid.NewGuid().ToString()));
                htmlSbd.Append(string.Format(tdE, totleScore.ToString(), Guid.NewGuid().ToString()));
                htmlSbd.Append("</tr>");
            }

            string htmlstr = string.Format(sb.ToString(),
                             htmlSbd.ToString());
            return htmlstr ;
        }

        private bool ProcessLevel(string l,string r) {
            Hashtable hs = new Hashtable();
            hs.Add("��", "1");
            hs.Add("��", "2");
            hs.Add("�����Ⱦ", "3");
            hs.Add("�ж���Ⱦ", "4");
            hs.Add("�ض���Ⱦ", "5");
            hs.Add("������Ⱦ", "6");

            int x=int.Parse(hs[l].ToString());
            int y=int.Parse(hs[r].ToString());
            int z = x - y;
            if (z < 0) 
                z = y - x;

            if (z >= 2) //��ȴ���2
                return true;
            else
                return false;
        }

        public void TableEvaluationString(DataSet temp, int count, string fromDate, string toDate)
        {
            try
            {
                ProcessDataForecast(temp, count, fromDate, toDate);
                GetEvalautionDT(fromDate, toDate);
                ProcessEvaluationData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ������������� 
        /// </summary>
        private void ProcessEvaluationData() {
            //�����˵����ݴ������ݿ�
            string existsSQL = @"SELECT ID FROM T_Evaluation WHERE LST='{0}' and duration='{1}'";
            string updateSQL = @"UPDATE T_Evaluation SET Score='{0}' , ScoreCount='{1}' where LST = '{2}' and duration='{3}'";
            string insertSQL = @"INSERT INTO T_Evaluation VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
            string lastTime="";
            
            foreach (DataRow row in forecastDt.Rows) {
                string rq=row["RQ"].ToString();

                string sdmc = row["SDMC"].ToString();
                if (string.IsNullOrEmpty(rq))
                    rq = lastTime;
                
                string score = Math.Round(decimal.Parse(row["SDQJ"].ToString()),0).ToString();//����2
                string scoreCount = "0"; 
                if (rq.IndexOf('��') < 0 && rq != "") {
                    scoreCount = Math.Round(decimal.Parse(rq),0).ToString();
                    rq = lastTime;
                }

                string dateTime = DateTime.Parse(rq).ToString("yyyy-MM-dd");
                string existsSQLs = string.Format(existsSQL, dateTime, sdmc);
                string updateSQLs = string.Format(updateSQL, score, scoreCount, dateTime,sdmc);
                string insertSQLs = string.Format(insertSQL, dateTime, dateTime, "24", sdmc, "", score, scoreCount);//ȡ��ǰ��¼�û�
                m_Database.Execute(existsSQLs, updateSQLs, insertSQLs);  
                lastTime=rq;
            }
        }


        public  int GetAQILevel(int AQI)
        {
            int level = 0;
            if (0<=AQI && AQI<=50)
            {
                level = 0;
            }
            else if (51 <= AQI && AQI <= 100)
            {
                level = 1;
            }
            else if (101 <= AQI && AQI <= 150)
            {
                level = 2;
            }
            else if (151 <= AQI && AQI <= 200)
            {
                level = 3;
            }
            else if (201 <= AQI && AQI <= 300)
            {
                level = 4;
            }
            else if(AQI>300){
                level = 5;
            }
            return level;
        }


        /// <summary>
        /// ���ݹ����㷨�õ�ÿ��ʱ�ο��˵�����
        /// </summary>
        /// <returns></returns>
        private void GetEvalautionDT(string fromDate, string toDate)
        {
            //��ѯ��ʵ����ָ���Ƿ����ŵȼ� Ҫ���ǳ��������
            bool isAllGood = true;
            bool O31h_bool = false;//031h�Ƿ���뿼��
            bool O31h_bool2  = false;//O31h�Ƿ�Ϊ0���뿼��
            DataTable dt = forecastDt;
            Hashtable result_hst = new Hashtable();
            if (dt != null && dt.Rows.Count > 0)
            {
                string lstDate="";//����
                foreach (DataRow row in dt.Rows)
                {
                    string sdmc = row["SDMC"].ToString();//ʱ������
                    string rq = row["RQ"].ToString();//����
                    string quality = row["Quality"].ToString().Trim();
                    string O31hValue= row["O31x1"].ToString();//����1СʱAQIֵ
                    string firstPollutant = row["AQIy"].ToString();//��Ҫ��Ⱦ��
                    string O31hForecast = row["O31y"].ToString();//����1СʱԤ��ֵ
                    ArrayList lst = new ArrayList();
                    #region ��ѯPM25 ��PM10 ��NO2��031h��038hʵ������
                    string PM25 = row["PM25x1"].ToString();
                    string PM10 = row["PM10x1"].ToString();
                    string N02 = row["NO2x1"].ToString();
                    string O31h = row["O31x1"].ToString();
                    string O38h = row["O38x1"].ToString();

                    int PM25i=0;
                    int.TryParse(PM25,out PM25i);
                    PM25i = GetAQILevel(PM25i);
                    lst.Add(PM25i);

                    int PM10i = 0;
                    int.TryParse(PM10, out PM10i);
                    PM10i = GetAQILevel(PM10i);
                    lst.Add(PM10i);

                    int N02i = 0;
                    int.TryParse(N02, out N02i);
                    N02i = GetAQILevel(N02i);
                    lst.Add(N02i);

                    int O31hi = 0;
                    int.TryParse(O31h, out O31hi);
                    O31hi = GetAQILevel(O31hi);
                    lst.Add(O31hi);

                    int O38hi = 0;
                    int.TryParse(O38h, out O38hi);
                    O38hi = GetAQILevel(O38hi);
                    lst.Add(O38hi);

                    //��ѯ��ָ���Ƿ�Ϊ�ŵȼ�,���ǳ������
                    if (PM25i != 0)
                        isAllGood = false;
                    else if (PM10i != 0)
                        isAllGood = false;
                    else if (N02i != 0)
                        isAllGood = false;
                   #endregion

                    string rqs = rq;
                    if (rq.Trim() == "")
                    {
                        rqs = lstDate;
                    }

                    #region �жϳ���1Сʱ����������ʱ��Ҫ��Ҫ���뿼��
                    if (sdmc == "����")
                    {
                        DateTime nowTime = DateTime.Parse(rqs);
                        DateTime beginTime = DateTime.Parse(nowTime.Year + "-03" + "-16");
                        DateTime endTime = DateTime.Parse(nowTime.Year + "-11" + "-15");
                        //����1Сʱ�Ƿ����İ���(3��16����11��15��)
                        if (nowTime >= beginTime && nowTime <= endTime)
                        {
                            #region 
                            float result = 0f;
                            float.TryParse(O31hValue, out result);
                            //��Ҫ�ж�Ԥ��Ա��û���������Ϣ(����ֻ�жϳ���1Сʱ)
                            if (string.IsNullOrEmpty(O31hForecast) && O31hForecast == "/")
                            {
                                //δ���Ϊ�������
                                //1����δ��Ҹ�ʱ�γ���ʵ��δ�ﵽ���˱�׼����2����������������뿼�ˣ�
                                if (result <= 100f || firstPollutant.IndexOf("O3-1h") < 0)
                                {
                                    O31h_bool2=true;
                                    isAllGood = true;
                                }
                                else {  
                                    //2����δ��Ҹ�ʱ�γ���ʵ���ﵽ���˱�׼�������ָ����Ԥ��ֵΪ��0����¼�Ϳ���
                                    isAllGood = false;
                                    O31h_bool = true;//����1Сʱ���뿼��
                                    O31h_bool2 = true;
                                }
                            }
                            else
                            {
                                //�жϳ���1Сʱ���������100���ҳ�Ϊ��Ҫ��Ⱦ��
                                if (result > 100f && firstPollutant.IndexOf("O3-1h") >= 0)
                                {
                                     O31h_bool = true;//����1Сʱ���뿼��

                                    if (O31hi != 0)//����1Сʱ��ָ���Ƿ�Ϊ����
                                        isAllGood = false;
                                }
                            }
                            #endregion
                        }
                    }
                   #endregion 

          
                    string fx=ProcessData(row, isAllGood, lst, O31h_bool, O31h_bool2,rqs);
                    //f �ܷ�= fҹ�� �� 0.3 + f���� �� 0.3 + f���� �� 0.3 + f���� �� 0.1
  
                    if (rqs != lstDate && lstDate!="")//��ƽ��
                    {
                        //�ж��Ƿ��˵ڶ���ʱ���
                        float yw = 0f;
                        try
                        {
                            float.TryParse(result_hst["ҹ��"].ToString(), out yw);
                        }
                        catch { }
                        float sw = 0f;
                        try
                        {
                            float.TryParse(result_hst["����"].ToString(), out sw);
                        }
                        catch { }
                        float xw = 0f;
                        try
                        {
                            float.TryParse(result_hst["����"].ToString(), out xw);
                        }
                        catch { }
                        float rpj = 0f;
                        try
                        {
                            float.TryParse(result_hst["ȫ��"].ToString(), out rpj);//��ƽ��
                        }
                        catch { }
                        string Fcount = (yw * 0.3 +
                                         sw * 0.3 +
                                         xw * 0.3 +
                                         rpj * 0.1).ToString();

                        row["RQ"] = Fcount;//��ĩβ��һ����ӿ��˵��ܷ�
                        result_hst.Clear();//����
                    }
                    lstDate = rqs;
                    result_hst[sdmc] = fx;
                    row["SDQJ"] = fx;//�ֶο��˷����浽��Ӧ�У������´�ȡ��
                }
            }
        }

       /// <summary>
       /// ��ϣ�������
       /// </summary>
       /// <param name="hasTbl"></param>
       /// <param name="cmp"></param>
       /// <returns></returns>
        private DictionaryEntry[] SortHashtable(Hashtable hasTbl, IComparer cmp)
        {
            DictionaryEntry[] dic = new DictionaryEntry[hasTbl.Count];
            hasTbl.CopyTo(dic, 0);
            Array.Sort(dic, cmp);
            return dic;
        }

        /// <summary>
        /// ������󿼺����
        /// </summary>
        /// <param name="row"></param>
        /// <param name="flag"></param>
        private string ProcessData(DataRow row, bool flag, ArrayList lst, bool O31h_bool,bool O31h_bool2,string rqs)
        {
            string F = "0";

            #region f1  ��Ҫ��Ⱦ����ȷ��
            // --  �����Ԥ����Ҫ��Ⱦ���ʵ����ȫ��ͬ����100�֣�
            string f1 = "0";
            string aqiy = row["AQIy"].ToString().Trim();//ʵ����Ҫ��Ⱦ��
            string aqiz = row["AQIk"].ToString().Trim();//Ԥ����Ҫ��Ⱦ��
            //�����ж����Ⱦ��
            int sucessCount = 0;//ƥ������
            int faildCount = 0;//û��ƥ������
            foreach (string str in aqiz.Split(' '))
            {
                if (aqiy.IndexOf(str.Trim()) >= 0)
                {
                    sucessCount++;
                }
                else
                {
                    faildCount++;
                }
            }

            //�����Ԥ������2����������Ҫ��Ⱦ�
            //ʵ����Ҫ��Ⱦ��Ϊ����һ�
            //�÷�Ϊf1=100 �� 1 / NԤ��
            //��NԤ��ΪԤ����Ⱦ���������
            if (aqiz.Split(' ').Length > 1 &&
                sucessCount > 0)
            {
                f1 = (100 * 1 / aqiz.Split(' ').Length).ToString();
            }
            //�����ʵ������2����������Ҫ��Ⱦ�
            //Ԥ��Ϊ����һ���100�֣�
            else if (aqiy.Split(';').Length > 1 &&
                sucessCount > 0)
            {
                f1 = "100";
            }
            //�����Ԥ����Ҫ��Ⱦ���ʵ����ȫ��ͬ����100�֣�
            else if (aqiz.Split(' ').Length == sucessCount)
            {
                f1 = "100";
            }
            else if (aqiz.Split(' ').Length == faildCount)
            {
                //�����Ԥ����Ҫ��Ⱦ���ʵ����ȫ��ͬ����0�֣�
                f1 = "0";
            }
            #endregion
            //------------------------
            #region f2������׼ȷ�֣�
            //�ж�ʵ���Ļ�������Ҳ����˵�ǻ��ʵ���Ļ�������) ,ע�� �����������ߵġ�
            //ͳ��Ƶ�ʢڸ�ʱ����ڳ���Ƶ�����������������𣨻�һ�����𣩡�
            string f2 = "0";
            int[] sort = { 0, 0, 0, 0, 0, 0 };
            #region init
            Hashtable hst_sort = new Hashtable();
            hst_sort.Add("��", 0);
            hst_sort.Add("��", 0);
            hst_sort.Add("�����Ⱦ", 0);
            hst_sort.Add("�ж���Ⱦ", 0);
            hst_sort.Add("�ض���Ⱦ", 0);
            hst_sort.Add("������Ⱦ", 0);
            #endregion
            foreach (int str in lst)
            {
                switch (str)
                {
                    case 0://�� 0~50
                        sort[0] = (++sort[0]);
                        hst_sort["��"]=sort[0];
                        break;
                    case 1://�� 51~100
                        sort[1] = (++sort[1]);
                        hst_sort["��"]=sort[1];
                        break;
                    case 2://�����Ⱦ 101~150
                        sort[2] = (++sort[2]);
                        hst_sort["�����Ⱦ"]=sort[2];
                        break;
                    case 3://�ж���Ⱦ 151~200
                        sort[3] = (++sort[3]);
                        hst_sort["�ж���Ⱦ"]=sort[3];
                        break;
                    case 4://�ض���Ⱦ 201~300
                        sort[4] = (++sort[4]);
                        hst_sort["�ض���Ⱦ"]=sort[4];
                        break;
                    default:
                        //������Ⱦ > 300
                        sort[5] = (++sort[5]);
                        hst_sort["������Ⱦ"]=sort[5];
                        break;
                }
            }
            //����
            string skValue = ""; //ʵ����������
            HashComparerByValue hashCmp = new HashComparerByValue(System.Windows.Forms.SortOrder.Descending);
            DictionaryEntry[] dirs=SortHashtable(hst_sort, hashCmp);
            for(int i=0;i< 2;i++){ 
                 //ȡǰ������һ��
                if (skValue.IndexOf(dirs[i].Key.ToString()) < 0 &&
                   int.Parse(dirs[i].Value.ToString()) > 0)
                {
                    skValue = skValue + dirs[i].Key.ToString() + "��";
                }
            }
            if (!string.IsNullOrEmpty(skValue)) { 
                 //��ʼ����ʵ�������������
                //��ѯԤ���Ļ�������
                string queryString = "select * from  tb_AirForecast  where foreDate='{0}' ";
                queryString = string.Format(queryString,rqs);
                DataTable dt=m_Database.GetDataTable(queryString);
                string ybValue = "";
                string memo = "";//��������
                if (dt != null && dt.Rows.Count > 0)
                {

                    memo = dt.Rows[0]["Detail"].ToString().Trim();

                    if (row["SDMC"].ToString() == "ҹ��")
                        ybValue = dt.Rows[0]["Grade1"].ToString().Trim();
                    if (row["SDMC"].ToString() == "����")
                        ybValue = dt.Rows[0]["Grade2"].ToString().Trim();
                    if (row["SDMC"].ToString() == "����")
                        ybValue = dt.Rows[0]["Grade3"].ToString().Trim();
                    //��ƽ��û�����ݣ�����Ҫ�ı��ֶΣ���һ�����ݽ�ȥ
                }
                //���ʵ���Ļ���������ȫ��Ԥ���Ļ������𸲸ǣ����ж�Ϊ��ȫ׼ȷ����100�֣�
                string[] sks = skValue.Split(new string[] { "��" },StringSplitOptions.RemoveEmptyEntries);
                int sksCount = 0;
                for (int i = 0; i < sks.Length; i++) {
                    if (ybValue.IndexOf(sks[i].ToString()) >= 0) {
                        sksCount++;
                    }
                }
                if (sksCount == sks.Length) { 
                   //��ȫ����  100��
                    f2 = "100";
                }
                //���ʵ���Ļ������𲿷ֱ�Ԥ���Ļ������𸲸� ��50�֣�
                else if (sksCount > 0) {
                    f2 = "50";
                }
                //���ʵ���Ļ���������ȫû�б�Ԥ���Ļ������𸲸� ��0�֣�
                else if (sksCount == 0) {
                    f2 = "0";
                }
                //========�ж�ʵ���Ƿ����������
                if (sks.Length > 1)
                {
                    if (ProcessLevel(sks[0], sks[1]))
                    {
                        //��ȡԤ��������ı仯����
                        sks[0] = sks[0].Replace("��Ⱦ", "");
                        sks[1] = sks[1].Replace("��Ⱦ", "");

                        //���ʵ����Ҫ�仯����������Ԥ��û�У��ڻ�������÷ֵĻ����Ͽ�20�֣�
                        if (string.IsNullOrEmpty(memo))
                        {
                            f2 = (float.Parse(f2) - 20f).ToString();
                        }
                        else if (memo.IndexOf(sks[0]) >= 0 &&
                               memo.IndexOf(sks[1]) >= 0)
                        {
                            //���ʵ����Ҫ�仯�����������������/��ͼ�����Ԥ������������ڻ�������÷ֵĻ����ϼ�20�֣�
                            f2 = (float.Parse(f2) + 20f).ToString();
                        }
                        else if (memo.IndexOf(sks[0]) >= 0 ||
                               memo.IndexOf(sks[1]) >= 0)
                        {
                            //���ʵ����Ҫ�仯�����������������/��ͼ�����Ԥ��������һ�����ڻ�������÷ֵĻ����Ͽ�10�֣�
                            f2 = (float.Parse(f2) - 10f).ToString();
                        }
                        else if (memo.IndexOf(sks[0]) < 0 &&
                               memo.IndexOf(sks[1]) < 0)
                        {
                            //���ʵ����Ҫ�仯�����������������/��ͼ�����Ԥ���������������������ϣ��ڻ�������÷ֵĻ����Ͽ�20�֡�
                            f2 = (float.Parse(f2) - 20f).ToString();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(memo))  
                            //���ʵ������Ҫ�仯����������Ԥ���У��ڻ�������÷ֵĻ����Ͽ�10�֣�
                            f2 = (float.Parse(f2) - 10f).ToString();  
                    }
                }
                else {
                    if (!string.IsNullOrEmpty(memo))
                          //���ʵ������Ҫ�仯����������Ԥ���У��ڻ�������÷ֵĻ����Ͽ�10�֣�
                        f2 = (float.Parse(f2) - 10f).ToString(); 
                    
                }
                //========�ܼ���׼ȷ�Ե÷����Ϊ100�֣����Ϊ0�֣��÷ָ���100��/����0������100��/0�ּ��㡣
                if (float.Parse(f2) > 100)
                    f2 = "100";
                else if (float.Parse(f2) < 0)
                    f2 = "0";
            }

            #endregion
            //------------------------
            #region f3����Ҫ��Ⱦ��iAQI����)
            string f3 = "0";
            //��ȡ��Ҫ��Ⱦ��
            //row["AQIy"].ToString();
            float f3x = ProcessF3("AQIz", "AQIx", row);
            f3 = f3x.ToString();
            #endregion
            //------------------------
            #region f4������Ⱦ��IAQI���� �ų���Ҫ��Ⱦ��
            string f4 = "0";
            int count = 0;
             
            //Ҫ�жϳ���1h����� PM25
             float f4x = 0f;
             if (row["AQIy"].ToString().IndexOf("PM2.5") < 0)
             {
                 f4x=ProcessF3("PM25y1", "PM25x1", row);
                 count++;
             }
            //PM10
             float f4y = 0f;
             if (row["AQIy"].ToString().IndexOf("PM10") < 0)
             {
                 f4y = ProcessF3("PM10y1", "PM10x1", row);
                 count++;
             }
            //NO2
             float f4z = 0f;
             if (row["AQIy"].ToString().IndexOf("NO2") < 0)
             {
                  f4z = ProcessF3("NO2y1", "NO2x1", row);
                 count++;
             }
            //031h  --����O31h��� 
            float f4k = 0f;
            if (O31h_bool)
            {
                if (!O31h_bool2)
                {
                    if (row["AQIy"].ToString().IndexOf("O3-1h") < 0)
                    {
                        f4k = ProcessF3("O31y1", "O31x1", row);
                        count++;
                    }
                }
            }
            //O38h
            float f4q = 0f;
            if (row["AQIy"].ToString().IndexOf("O3-8h") < 0)
            {
                 f4q = ProcessF3("O38y1", "O38x1", row);
            }
            //����ƽ��
            f4 = ((f3x + f4y + f4z + f4k + f4q) / count).ToString();
            #endregion 
            //------------------------
            #region f0����ȾԤ�����ӷ��
            string f0 = "0";
            string sk = row["Parameter"].ToString().Trim();
            string yb = row[" "].ToString().Trim();
            //��ʵ��������ȼ�������Ⱦʱ������AQI���ӷ֣�f0������
            if (sk.IndexOf('��') < 0 && 
                sk.IndexOf('��') < 0 &&
                sk.IndexOf('/')>0)
            {
                f0 = ProcessFO(sk, yb);
            }
            #endregion
            //------------------------
            #region ��������
            if (flag)
            {
                //ȫ�ŵȼ�  
                //F = 0.3 �� f1 + 0.7�� f4 + f0    
                F = (0.3f * float.Parse(f2) +
                    (0.7f * (float.Parse(f3)+float.Parse(f4))) + 
                   float.Parse(f0)).ToString();//f1 = f2  f4 = f3
            }
            else
            {
                //���ŵȼ� 
                //F = 0.1 �� f1 +0.2 �� f2+ 0.7 �� f3 + f0 
                F = (0.1f * float.Parse(f1) +
                     0.2f * float.Parse(f2) +
                     0.3f * float.Parse(f3) +
                     0.4f * float.Parse(f4) +
                     float.Parse(f0)).ToString();//f1 = f2  f4 = f3
            }
            #endregion
            return F;
        }

        /// ����F0���
        /// </summary>
        /// <param name="sk">ʵ������</param>
        /// <param name="yb">Ԥ������</param>
        private string ProcessFO(string sk,string yb) {
            string F0 = "0";
            if (fODt.Rows.Count <= 0) { 
                #region ������ݽ�ȥ
                DataRow row0 = fODt.NewRow();
                row0["����"] = "����";
                row0["����"] = "0";
                row0["���"] = "0";
                row0["�ж�"] = "-2";
                row0["�ض�"] = "-4";
                row0["����"] = "-8";
                fODt.Rows.Add(row0);
                DataRow row1 = fODt.NewRow();
                row1["����"] = "���";
                row1["����"] = "0";
                row1["���"] = "2";
                row1["�ж�"] = "0";
                row1["�ض�"] = "-2";
                row1["����"] = "-4";
                fODt.Rows.Add(row1);
                DataRow row2 = fODt.NewRow();
                row2["����"] = "�ж�";
                row2["����"] = "-2";
                row2["���"] = "0";
                row2["�ж�"] = "4";
                row2["�ض�"] = "0";
                row2["����"] = "-2";
                fODt.Rows.Add(row2);
                DataRow row3 = fODt.NewRow();
                row3["����"] = "�ض�";
                row3["����"] = "-4";
                row3["���"] = "-2";
                row3["�ж�"] = "0";
                row3["�ض�"] = "8";
                row3["����"] = "1";
                fODt.Rows.Add(row3);
                DataRow row4 = fODt.NewRow();
                row4["����"] = "����";
                row4["����"] = "-8";
                row4["���"] = "-4";
                row4["�ж�"] = "-2";
                row4["�ض�"] = "1";
                row4["����"] = "10";
                fODt.Rows.Add(row4);
                #endregion 
            }
            DataRow[] rows=fODt.Select("[����] like '%"+sk+"%'");
            if (rows != null && rows.Length > 0) {
                if (yb.IndexOf('��')>=0 ||
                    yb.IndexOf('��')>=0)
                {
                    F0 = rows[0][0].ToString();
                }
                else {
                    yb=yb.Replace("��Ⱦ", "");
                    F0 = rows[0][yb].ToString();
                }
            }
            return F0;
        }

        /// ����F3���
        /// </summary>
        /// <param name="x">Ԥ��ֵ</param>
        /// <param name="y">ʵ��ֵ</param>
        private float ProcessF3(string x,string y,DataRow row) {
            string f3iPM25 = row[x].ToString().Trim();//Ԥ��ֵ
            string f3iPM25s = row[y].ToString().Trim();//ʵ��ֵ
            float f3iPM25V1 = 0f;
            float.TryParse(f3iPM25, out f3iPM25V1);
            float f3iPM25V2 = 0f;
            float.TryParse(f3iPM25s, out f3iPM25V2);

            float mx = f3iPM25V2 > 50f ? f3iPM25V2 : 50;//max(ʵ��,50)

            float z = (1 - (Math.Abs(f3iPM25V1 - f3iPM25V2) / mx));
            string f3i = ( (z>0?z:0)* 100).ToString();////ȡmax(xx,0)
            float f3iV = 0f; //------------------------------------------
            float.TryParse(f3i, out f3iV);
            return f3iV;
        }

        private string ProcessJson(string xml) {

            string filter = "</table>|\r\n<table id='table{0}' width='100%' border='0' cellpadding='0' cellspacing='0' class='tablekuang'>";
            xml = xml.Replace(string.Format(filter, "2"), "");
            xml = xml.Replace(string.Format(filter, "3"), "");
            xml = xml.Replace(string.Format(filter, "4"), "");
            xml = xml.Replace(string.Format(filter, "5"), "");
            filter = "<table id='table{0}'  width='100%' border='0' cellpadding='0' cellspacing='0' class='tablekuang'>";
            xml = xml.Replace(string.Format(filter, "0"), "");
            xml=xml.Replace("|", "").Replace("</table>","");
            xml = xml + "\r\n</table>";
            xml = string.Format(xml, "style=\"visiblity:hidden;\"");
            return xml;
        }

  

    }
}
