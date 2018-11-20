using System;
using System.Collections.Generic;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using Readearth.Data;
using MMShareBLL.Model;


namespace MMShareBLL.DAL
{
    public class CityPeriodDailyDataAccess
    {
        //Procedures
        private const string PROC_CITYPERIODDAILYDATA = "Proc_CityPeriodDailyData";
        private const string PROC_CITYPERIODDAILY_QUERY = "Proc_CityPeriodDaily_Query";
        private const string PROC_CITYPERIODDAILYDATA_QUERY = "Proc_CityPeriodDailyData_Query";

        //Parameters
        private const string PARAM_D1 = "@d1";
        private const string PARAM_D2 = "@d2";
        private const string PARAM_DAY = "@day";
        private const string PARAM_OPERATOR = "@operator";
        private const string PARAM_RT = "@RT";
        
        private Dictionary<int, string> periodDic = new Dictionary<int,string>();

        public CityPeriodDailyDataAccess()
        {
            m_DatabaseS = new Database("SEMCDMC");
        }
        private Database m_DatabaseS;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="periodDictionary">分段配置字典</param>
        public CityPeriodDailyDataAccess(Dictionary<int,string> periodDictionary)
        {
            periodDic = periodDictionary;
            m_DatabaseS = new Database("SEMCDMC");
        }
       

        /// <summary>
        /// 加工分段日报数据
        /// </summary>
        /// <param name="day"></param>
        /// <param name="createOperation"></param>
        /// <param name="RTData"></param>
        public void CreateDailyReport(DateTime day, string createOperation, bool RTData)
        {
            SqlParameter paramDay = new SqlParameter(PARAM_DAY, SqlDbType.DateTime);
            SqlParameter paramOperator = new SqlParameter(PARAM_OPERATOR, SqlDbType.NVarChar, 50);
            SqlParameter paramRT = new SqlParameter(PARAM_RT, SqlDbType.Bit);
            paramDay.Value = day;
            paramOperator.Value = createOperation;
            paramRT.Value = RTData;
            try
            {
                m_DatabaseS.Execute(PROC_CITYPERIODDAILYDATA, paramDay,paramOperator, paramRT);
            }
            catch { throw; }
        }
        /// <summary>
        /// 获取分时段日报基本数据
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public Dictionary<DateTime,CityPeriodData> getCityPeriodDailyDic(DateTime d1,DateTime d2)
        {
            SqlParameter[] parameters = buildQueryParam(d1,d2);
            try
            {
                Dictionary<DateTime, CityPeriodData> data = new Dictionary<DateTime, CityPeriodData>();
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_CITYPERIODDAILY_QUERY, parameters))
                {
                    while (reader.Read())
                    {
                        DateTime t = reader.GetDateTime(1);
                        CityPeriodData cpd = null;
                        if (data.ContainsKey(t))
                        {
                            continue;
                        }
                        cpd = new CityPeriodData();
                        cpd.DailyDate = t;
                        cpd.API = reader.GetInt32(4);
                        cpd.LastModifyTime = reader.GetDateTime(5);
                        cpd.LastModifyPersion = reader.GetString(6);
                        cpd.FullData = reader.GetBoolean(7);
                        data.Add(t, cpd);
                    }
                }
                return data;
            }
            catch { throw; }
        }
        /// <summary>
        /// 获取分时段日报全部数据
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public Dictionary<DateTime, CityPeriodData> getCityPeriodDailyDataDic(DateTime d1, DateTime d2)
        {
            SqlParameter[] parameters = buildQueryParam(d1, d2);
            try
            {
                Dictionary<DateTime, CityPeriodData> data = new Dictionary<DateTime, CityPeriodData>();
                using (SqlDataReader reader = m_DatabaseS.GetDataReader(PROC_CITYPERIODDAILYDATA_QUERY, parameters))
                {
                    while (reader.Read())
                    {
                        DateTime t = reader.GetDateTime(1);
                        int periodCode = reader.GetInt32(2);

                        CityPeriodData cpd = null;
                        PeriodData pd = null;
                        MonitoringData md = new MonitoringData();
                        md.Factor = new DataParameter();
                        md.Factor.Id = reader.GetInt32(8);
                        md.Thickness = (float)reader.GetDouble(9);
                        md.API = reader.GetInt32(10);

                        if (data.ContainsKey(t))
                        {
                            cpd = data[t];
                            if (cpd.PeriodDataDic.ContainsKey(periodCode))
                                cpd.PeriodDataDic[periodCode].DataCollection.Add(md);
                            else
                            {
                                pd = new PeriodData();
                                pd.PeriodCode = periodCode;
                                pd.PeriodDate = reader.GetString(3);
                                if (periodDic.ContainsKey(pd.PeriodCode))
                                    pd.PeriodName = periodDic[pd.PeriodCode];
                                pd.DataCollection.Add(md);
                                cpd.PeriodDataDic.Add(periodCode, pd);
                            }
                        }
                        else
                        {
                            cpd = new CityPeriodData();
                            cpd.DailyDate = t;
                            cpd.API = reader.GetInt32(4);
                            cpd.LastModifyTime = reader.GetDateTime(5);
                            cpd.LastModifyPersion = reader.GetString(6);
                            cpd.FullData = reader.GetBoolean(7);
                            pd = new PeriodData();
                            pd.PeriodCode = periodCode;
                            pd.PeriodDate = reader.GetString(3);
                            if (periodDic.ContainsKey(pd.PeriodCode))
                                pd.PeriodName = periodDic[pd.PeriodCode];
                            pd.DataCollection.Add(md);
                            cpd.PeriodDataDic.Add(periodCode, pd);
                            data.Add(t, cpd);
                        }
                    }
                }
                return data;
            }
            catch { throw; }
        }
        /// <summary>
        /// 加工获取实时分段日报数据
        /// </summary>
        /// <param name="operatorMan"></param>
        /// <returns></returns>
        public Dictionary<DateTime, CityPeriodData> getCityRTPeriodDailyDataDic(DateTime day, string createOperation, bool RTData)
        {
            SqlParameter[] paramArray = new SqlParameter[3];
            SqlParameter paramDay = new SqlParameter(PARAM_DAY, SqlDbType.DateTime);
            SqlParameter paramOperator = new SqlParameter(PARAM_OPERATOR, SqlDbType.NVarChar, 50);
            SqlParameter paramRT = new SqlParameter(PARAM_RT, SqlDbType.Bit);
            paramDay.Value = day;
            paramOperator.Value = createOperation;
            paramRT.Value = RTData;
            paramArray[0] = paramDay;
            paramArray[1] = paramOperator;
            paramArray[2] = paramRT;
            try
            {
                Dictionary<DateTime, CityPeriodData> data = new Dictionary<DateTime, CityPeriodData>();
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_CITYPERIODDAILYDATA, paramArray))
                {
                    while (reader.Read())
                    {
                        //DateTime t = reader.GetDateTime(1);
                        DateTime t = day;
                        int periodCode = reader.GetInt32(0);//

                        CityPeriodData cpd = null;
                        PeriodData pd = null;
                        MonitoringData md = new MonitoringData();
                        md.Factor = new DataParameter();
                        md.Factor.Id = reader.GetInt32(6);
                        md.Thickness = (float)reader.GetDouble(7);
                        md.API = reader.GetInt32(8);

                        if (data.ContainsKey(t))
                        {
                            cpd = data[t];
                            if (cpd.PeriodDataDic.ContainsKey(periodCode))
                                cpd.PeriodDataDic[periodCode].DataCollection.Add(md);
                            else
                            {
                                pd = new PeriodData();
                                pd.PeriodCode = periodCode;
                                pd.PeriodDate = reader.GetString(2);//periodName几日上午或下午或夜间
                                pd.PeriodName = reader.GetString(3);//periodHourRange小时范围
                                pd.FullData = reader.GetBoolean(5);
                                pd.DataCollection.Add(md);
                                cpd.PeriodDataDic.Add(periodCode, pd);
                            }
                        }
                        else
                        {
                            cpd = new CityPeriodData();
                            cpd.DailyDate = t;
                            cpd.API = 0;
                            cpd.LastModifyTime = t;
                            cpd.LastModifyPersion = reader.GetString(4);
                            cpd.FullData = reader.GetBoolean(5);
                            pd = new PeriodData();
                            pd.PeriodCode = periodCode;
                            pd.PeriodDate = reader.GetString(2);//periodName几日上午或下午或夜间
                            pd.PeriodName = reader.GetString(3);//periodHourRange小时范围
                            pd.FullData = reader.GetBoolean(5);
                            pd.DataCollection.Add(md);
                            cpd.PeriodDataDic.Add(periodCode, pd);
                            data.Add(t, cpd);
                        }
                    }
                }
                return data;
            }
            catch { throw; }
        }

        private SqlParameter[] buildQueryParam(DateTime d1,DateTime d2)
        {
            SqlParameter[] paramArray = new SqlParameter[2];
            SqlParameter paramD1 = new SqlParameter(PARAM_D1, SqlDbType.DateTime);
            SqlParameter paramD2 = new SqlParameter(PARAM_D2, SqlDbType.DateTime);
            paramD1.Value = d1;
            paramD2.Value = d2;
            paramArray[0] = paramD1;
            paramArray[1] = paramD2;
            return paramArray;
        }
    }
}
