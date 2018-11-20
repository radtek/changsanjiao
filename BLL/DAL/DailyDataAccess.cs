using System;
using System.Collections.Generic;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using MMShareBLL.Model;
using Readearth.Data;


namespace MMShareBLL.DAL
{
    /// <summary>
    /// 提供对日报数据的加工和加工后数据的访问
    /// </summary>
    public class DailyDataAccess
    {
        //Procedures
        private const string PROC_CREATE = "Daily_CreateData";
        private const string PROC_CREATE_AUTO = "Daily_CreateData_Auto";

        private const string PROC_CITYWIDEDAILY_QUERY = "Daily_CitywideDaily_Query";
        private const string PROC_CITYWIDEDAILYDATA_QUERY = "Daily_CitywideData_Query";
        private const string PROC_CITYWIDEDAILYSITEDATA_QUERY = "Daily_CitywideSiteData_Query";
        private const string PROC_CITYWIDEDAILYSITELIST_QUERY = "Daily_CitywideSiteList_Query";

        private const string PROC_CITYWIDEDAILY_QUERY2 = "Daily_CitywideDaily_Query2";
        private const string PROC_CITYWIDEDAILYDATA_QUERY2 = "Daily_CitywideData_Query2";
        private const string PROC_CITYWIDEDAILYDATA_QUERY5 = "Daily_CitywideData_Query5";
        private const string PROC_CITYWIDEDAILYDATA_GETPARAMETERS = "Daily_CitywideDaily_Query2_GetParameters";

        private const string PROC_UPDATESTATUS = "D_DailyUpdateState";

        private const string PROC_GROUPDATA_QUERY = "Daily_GroupData_Query";
        private const string PROC_GROUPDATA_QUERY2 = "Daily_GroupData_Query2";
        private const string PROC_GROUPDATA_QUERY3 = "Daily_GroupData_Query3";
        private const string PROC_GROUPSITEDATE_QUERY = "Daily_GroupSiteData_Query";
        private const string PROC_GROUPDATA_QUERYLIST = "Daily_GroupData_QueryList";
        private const string PROC_GROUPSITEDATA_PARAMETERS = "Daily_GroupSiteDataParameters_Query";
        private const string PROC_GROUPSITEDATA_PARAMETERS3 = "Daily_GroupData_Query3_GetParameterList";
        private const string PROC_GETGROUPDPARAMETERS = "Daily_GroupData_GetParamList";

        private const string PROC_COUNTYREPORT_QUERY = "Daily_CountyReportData_Query";
        private const string PROC_COUNTYREPORTDATA_QUERY = "Daily_CountyReportDetailData_Query";
        private const string PROC_COUNTYREPORT_QUERY2 = "Daily_CountyReportData_Query2";
        private const string PROC_COUNTYREPORTDATA_QUERY2 = "Daily_CountyReportDetailData_Query2";
        private const string PROC_COUNTYREPORTDATA_QUERY3 = "Daily_CountyReportDetailData_Query3";
        private const string PROC_COUNTYREPORTDATA_QUERY5 = "Daily_CountyReportDetailData_Query5";
        private const string PROC_COUNTYREPORT_GETPARAMETERS = "Daily_CountyReport_GetParameters";

        private const string PROC_COUNTYREPORTGROUPS_QUERY = "Daily_CountyGroups_Query";

        private const string PROC_SITEDATA_QUERY = "Daily_SiteData_Query";
        private const string PROC_SITEDATA_QUERY2 = "Daily_SiteData_Query2";
        private const string PROC_SITEDATA_QUERY3 = "Daily_SiteData_Query3";
        private const string PROC_SITEDATA_QUERY4 = "Daily_SiteData_Query4";
        private const string PROC_SITEDATA_QUERY5 = "Daily_SiteData_Query5";
        private const string PROC_SITEDATA_QUERY3_GETPARAMETER = "Daily_SiteData_Query3_GetParameterList";
        private const string PROC_SITEDATA_QUERY_WITHPARAMETER = "Daily_SiteData_Query_WithParameter";
        private const string PROC_SITEAVGDATA_QUERY = "Daily_SiteAvgData_Query";

        private const string PROC_GET_DATABROWSER = "DailyDataBrowser";
        private const string PROC_GET_LASTGROUPDATATIME = "Daily_GroupData_LastDate";


        //Parameters
        private const string PARAM_D1 = "@d1";
        private const string PARAM_D2 = "@d2";
        private const string PARAM_DAY = "@day";
        private const string PARAM_DURATION = "@duration";
        private const string PARAM_Operator = "@operator";
        private const string PARAM_GROUPID = "@groupId";
        private const string PARAM_SITEID = "@siteId";
        private const string PARAM_CITYWIDEDATA = "@citywideData";
        private const string PARAM_COUNTYDATA = "@countyData";
        private const string PARAM_STATUS = "@status";
        private const string PARAM_PARAMETERS = "@parameters";
        private const string PARAM_SITES = "@sites";
        private Database m_DatabaseS;
        public DailyDataAccess()
        {
            m_DatabaseS = new Database("SEMCDMC");
        }
        /// <summary>
        /// 生成日报数据
        /// </summary>
        public void CreateCitywideDaily(DateTime d1, DateTime d2, DateTime day, Duration duration, string createOperator, bool citywideData, bool countyData, string sites)
        {
            SqlParameter paramD1 = new SqlParameter(PARAM_D1, SqlDbType.DateTime);
            SqlParameter paramD2 = new SqlParameter(PARAM_D2, SqlDbType.DateTime);
            SqlParameter paramDay = new SqlParameter(PARAM_DAY, SqlDbType.DateTime);
            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
            SqlParameter paramOperator = new SqlParameter(PARAM_Operator, SqlDbType.NVarChar, 50);
            SqlParameter paramCitywideData = new SqlParameter(PARAM_CITYWIDEDATA, SqlDbType.Bit);
            SqlParameter paramCountyData = new SqlParameter(PARAM_COUNTYDATA, SqlDbType.Bit);
            SqlParameter paramSites = new SqlParameter(PARAM_SITES, SqlDbType.VarChar);

            paramD1.Value = d1;
            paramD2.Value = d2;
            paramDay.Value = day;
            paramDuration.Value = (int)duration;
            paramOperator.Value = createOperator;
            paramCitywideData.Value = citywideData;
            paramCountyData.Value = countyData;
            paramSites.Value = sites;
            try
            {
                m_DatabaseS.ExecuteNonQuery(m_DatabaseS.ConnectionString, CommandType.StoredProcedure, PROC_CREATE, 0, paramDuration, paramOperator, paramD1, paramD2, paramDay, paramCountyData, paramCitywideData, paramSites);
            }
            catch { throw; }
        }

        /// <summary>
        /// 以自动的方式生成日报数据
        /// </summary>
        public void CreateCitywideDailyAuto(DateTime d1, DateTime d2, DateTime day, Duration duration, bool citywideData, bool countyData, string sites)
        {
            SqlParameter paramD1 = new SqlParameter(PARAM_D1, SqlDbType.DateTime);
            SqlParameter paramD2 = new SqlParameter(PARAM_D2, SqlDbType.DateTime);
            SqlParameter paramDay = new SqlParameter(PARAM_DAY, SqlDbType.DateTime);
            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
            SqlParameter paramCitywideData = new SqlParameter(PARAM_CITYWIDEDATA, SqlDbType.Bit);
            SqlParameter paramCountyData = new SqlParameter(PARAM_COUNTYDATA, SqlDbType.Bit);
            SqlParameter paramSites = new SqlParameter(PARAM_SITES, SqlDbType.VarChar);
            paramD1.Value = d1;
            paramD2.Value = d2;
            paramDay.Value = day;
            paramDuration.Value = (int)duration;
            paramCitywideData.Value = citywideData;
            paramCountyData.Value = countyData;
            paramSites.Value = sites;
            try
            {
                m_DatabaseS.ExecuteNonQuery(m_DatabaseS.ConnectionString, CommandType.StoredProcedure, PROC_CREATE_AUTO, 0, paramD1, paramD2, paramDay, paramDuration, paramCountyData, paramCitywideData, paramSites);
            }
            catch { throw; }
        }

        /// <summary>
        /// 查询全市数据(仅获取少量数据，不包含监测因子数据和其下的站点数据)
        /// </summary>
        /// <returns></returns>
        public CitywideData QueryCitywideDaily(DateTime day, Duration duration)
        {
            try
            {
                return QueryCitywideDaily(buildQueryParam(day, duration));
            }
            catch { throw; }
        }

        public CitywideData QueryCitywideDaily2(DateTime day, Duration duration)
        {
            try
            {
                DataTable dt = AQIQuery.aQuery.Data.GroupDailyAQI(day, day, "102", "300");
                //CitywideData data = QueryCitywideDaily(parameters);
                CitywideData data = null;
                if (null != dt&&dt.Rows.Count>0)
                {
                    data = new CitywideData();
                    foreach (DataRow dr in dt.Rows)
                    {
                        //MonitoringData md = new MonitoringData();
                        //md.Factor = new DataParameter();
                        ////md.Factor.Id = reader.GetInt32(1);
                        ////md.Thickness = (float)reader.GetDouble(2);
                        ////md.API = reader.GetInt32(3);
                        //md.Factor.Id = Convert.ToInt32(dr["AQIItemID"]);
                        //md.Thickness = (float)Convert.ToDouble(dr["Value"]);
                        //md.AQI = Convert.ToInt32(dr["AQI"]);
                        //data.DataCollection.Add(md);
                        data.API = Convert.ToInt32(dr["AQI"]);
                    }
                }
                return data;
            }
            catch { throw; }
        }

        public void UpdateCitywideDailyStatus(DateTime day, DailyState status)
        {
            try
            {
                SqlParameter paramDay = new SqlParameter(PARAM_DAY, SqlDbType.DateTime);
                SqlParameter paramStatus = new SqlParameter(PARAM_STATUS, SqlDbType.Int);
                paramDay.Value = day;
                paramStatus.Value = (int)status;
                m_DatabaseS.Execute(PROC_UPDATESTATUS, paramStatus, paramDay);
            }
            catch { throw; }
        }

        public Dictionary<DateTime, CitywideData> QueryCitywideDaily(DateTime d1, DateTime d2, Duration duration)
        {
            try
            {
                SqlParameter[] parameters = buildQueryParam2(d1, d2, duration);
                Dictionary<DateTime, CitywideData> data = new Dictionary<DateTime, CitywideData>();
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_CITYWIDEDAILY_QUERY2, parameters))
                {
                    while (reader.Read())
                    {
                        DateTime lst = reader.GetDateTime(1);
                        data[lst] = pickUpCitywideData(reader);
                    }
                }
                return data;
            }
            catch { throw; }
        }

        private CitywideData QueryCitywideDaily(params SqlParameter[] parameters)
        {
            CitywideData data = null;
            try
            {
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_CITYWIDEDAILY_QUERY, parameters))
                {
                    if (reader.Read())
                    {
                        data = pickUpCitywideData(reader);
                    }
                }
                return data;
            }
            catch { throw; }
        }

        /// <summary>
        /// 查询全市数据，能获得全市各监测因子的浓度均值和API
        /// </summary>
        /// <returns></returns>
        public CitywideData QueryCitywideDailyDetail(DateTime day, Duration duration)
        {
            try
            {
                return QueryCitywideDailyDetail(buildQueryParam(day, duration));
            }
            catch { throw; }
        }

        public CitywideData QueryCitywideDailyDetail2(DateTime day, Duration duration)
        {
            try
            {
                DataTable dt = AQIQuery.aQuery.Data.GroupDailyAQI(day, day, "102", "304,305,301,308,306,307,302,300");
                //CitywideData data = QueryCitywideDaily(parameters);
                CitywideData data = new CitywideData();
                if (null != data)
                {
                    foreach(DataRow dr in dt.Rows){
                            MonitoringData md = new MonitoringData();
                            md.Factor = new DataParameter();
                            //md.Factor.Id = reader.GetInt32(1);
                            //md.Thickness = (float)reader.GetDouble(2);
                            //md.API = reader.GetInt32(3);
                            md.Factor.Id = Convert.ToInt32(dr["AQIItemID"]);
                            md.Thickness = (float)Convert.ToDouble(dr["Value"]);
                            md.AQI = Convert.ToInt32(dr["AQI"]);
                            data.DataCollection.Add(md);
                        
                    }
                }
                return data;
            }
            catch { throw; }
        }

        private CitywideData QueryCitywideDailyDetail(params SqlParameter[] parameters)
        {
            try
            {
                CitywideData data = QueryCitywideDaily(parameters);
                if (null != data)
                {
                    using (SqlDataReader reader = m_DatabaseS.GetDataReader(PROC_CITYWIDEDAILYDATA_QUERY, parameters))
                    {
                        while (reader.Read())
                        {
                            MonitoringData md = new MonitoringData();
                            md.Factor = new DataParameter();
                            md.Factor.Id = reader.GetInt32(1);
                            md.Thickness = (float)reader.GetDouble(2);
                            md.API = reader.GetInt32(3);
                            data.DataCollection.Add(md);
                        }
                    }
                }
                return data;
            }
            catch { throw; }
        }

        public Dictionary<DateTime, CitywideData> QueryCitywideDailyDetail(DateTime d1, DateTime d2, Duration duration)
        {
            try
            {
                SqlParameter[] parameters = buildQueryParam2(d1, d2, duration);
                Dictionary<DateTime, CitywideData> data = QueryCitywideDaily(d1, d2, duration);
                using (SqlDataReader reader = m_DatabaseS.GetDataReader(PROC_CITYWIDEDAILYDATA_QUERY2, parameters))
                {
                    while (reader.Read())
                    {
                        DateTime lst = reader.GetDateTime(4);
                        if (data.ContainsKey(lst))
                        {
                            MonitoringData md = new MonitoringData();
                            md.Factor = new DataParameter();
                            md.Factor.Id = reader.GetInt32(1);
                            md.Thickness = (float)reader.GetDouble(2);
                            md.API = reader.GetInt32(3);
                            data[lst].DataCollection.Add(md);
                        }
                    }
                }
                return data;
            }
            catch { throw; }
        }

        public Dictionary<DateTime, CitywideData> QueryCitywideDailyDetail(DateTime d1, DateTime d2, Duration duration, int[] parameterList)
        {
            try
            {
                SqlParameter[] parameters = buildQueryParam2(d1, d2, duration, parameterList);
                Dictionary<DateTime, CitywideData> data = QueryCitywideDaily(d1, d2, duration);
                using (SqlDataReader reader = m_DatabaseS.GetDataReader(PROC_CITYWIDEDAILYDATA_QUERY5, parameters))
                {
                    while (reader.Read())
                    {
                        DateTime lst = reader.GetDateTime(4);
                        if (data.ContainsKey(lst))
                        {
                            MonitoringData md = new MonitoringData();
                            md.Factor = new DataParameter();
                            md.Factor.Id = reader.GetInt32(1);
                            md.Thickness = (float)reader.GetDouble(2);
                            md.API = reader.GetInt32(3);
                            data[lst].DataCollection.Add(md);
                        }
                    }
                }
                return data;
            }
            catch { throw; }
        }

        public List<int> GetCitywideDailyParameters(DateTime d1, DateTime d2, Duration duration)
        {
            try
            {
                SqlParameter[] parameters = buildQueryParam2(d1, d2, duration);
                using (SqlDataReader reader = m_DatabaseS.GetDataReader(PROC_CITYWIDEDAILYDATA_GETPARAMETERS, parameters))
                {
                    List<int> dataList = new List<int>();
                    while (reader.Read())
                    {
                        dataList.Add(reader.GetInt32(0));
                    }
                    return dataList;
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// 查询日报数据，包涵所有信息
        /// </summary>
        /// <returns></returns>
        public CitywideData QueryCitywideDailyAllData(DateTime day, Duration duration)
        {
            SqlParameter[] parameters = buildQueryParam(day, duration);
            try
            {
                CitywideData data = QueryCitywideDailyDetail(parameters);
                if (null != data)
                {
                    Dictionary<int, MMShareBLL.Model.SiteData> dataCollection = new Dictionary<int, MMShareBLL.Model.SiteData>();
                    using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_CITYWIDEDAILYSITEDATA_QUERY, parameters))
                    {
                        while (reader.Read())
                        {
                            int siteId = reader.GetInt32(1);
                            MMShareBLL.Model.SiteData sd = null;
                            if (dataCollection.ContainsKey(siteId))
                                sd = dataCollection[siteId];
                            else
                            {
                                sd = new MMShareBLL.Model.SiteData();
                                sd.Site = new Site(siteId, null);
                                dataCollection[siteId] = sd;
                            }
                            MonitoringData md = new MonitoringData();
                            md.Factor = new DataParameter();
                            md.Factor.Id = reader.GetInt32(3);
                            md.Thickness = (float)reader.GetDouble(4);
                            md.API = reader.GetInt32(5);
                            sd.DataCollection.Add(md);
                        }
                    }
                    foreach (MMShareBLL.Model.SiteData sd in dataCollection.Values)
                        data.SiteData.Add(sd);
                }
                return data;
            }
            catch { throw; }
        }

        /// <summary>
        /// 得到某个期间参加全市日报计算的站点编号列表
        /// </summary>
        /// <returns></returns>
        public List<int> QueryCitywideSiteList(DateTime day, Duration duration)
        {
            SqlParameter[] param = buildQueryParam(day, duration);
            try
            {
                List<int> dataList = new List<int>();
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_CITYWIDEDAILYSITELIST_QUERY, param))
                {
                    while (reader.Read())
                    {
                        dataList.Add(reader.GetInt32(0));
                    }
                }
                return dataList;
            }
            catch { throw; }
        }

        /// <summary>
        /// 获取分区数据，仅含基本信息
        /// </summary>
        /// <param name="day"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public CountyReportData QueryCountyReport(DateTime day, Duration duration)
        {
            SqlParameter[] parameters = buildQueryParam(day, duration);
            return QueryCountyReport(parameters);
        }

        public Dictionary<DateTime, CountyReportData> QueryCountyReport(DateTime d1, DateTime d2, Duration duration)
        {
            try
            {
                SqlParameter[] parameters = buildQueryParam2(d1, d2, duration);
                Dictionary<DateTime, CountyReportData> data = new Dictionary<DateTime, CountyReportData>();
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_COUNTYREPORT_QUERY2, parameters))
                {
                    while (reader.Read())
                    {
                        DateTime lst = reader.GetDateTime(1);
                        data[lst] = pickUpCountyReportData(reader);
                    }
                }
                return data;
            }
            catch { throw; }
        }

        private CountyReportData QueryCountyReport(SqlParameter[] parameters)
        {
            try
            {
                CountyReportData data = null;
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_COUNTYREPORT_QUERY, parameters))
                {
                    if (reader.Read())
                    {
                        data = pickUpCountyReportData(reader);
                    }
                }
                return data;
            }
            catch { throw; }
        }

        /// <summary>
        /// 查询分区数据
        /// </summary>
        /// <returns></returns>
        public CountyReportData QueryCountyReportDetail(DateTime day, Duration duration)
        {
            SqlParameter[] parameters = buildQueryParam(day, duration);
            try
            {
                CountyReportData crd = QueryCountyReport(parameters);
                if (null == crd)
                    return null;
                Dictionary<string, GroupData> dataCollection = new Dictionary<string, GroupData>();
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_COUNTYREPORTDATA_QUERY, parameters))
                {
                    while (reader.Read())
                    {
                        string groupId = reader[6] as string;
                        if (string.IsNullOrEmpty(groupId))
                            continue;
                        GroupData data = null;
                        if (dataCollection.ContainsKey(groupId))
                            data = dataCollection[groupId];
                        else
                        {
                            data = new GroupData();
                            data.Group = new SiteGroup(groupId, reader[7] as string);
                            data.Date = new PeriodOfTime();
                            data.Date.Value = day;
                            dataCollection[groupId] = data;
                        }
                        MonitoringData md = new MonitoringData();
                        md.Factor = new DataParameter();
                        md.Factor.Id = reader.GetInt32(1);
                        md.Thickness = (float)reader.GetDouble(2);
                        md.API = reader.GetInt32(3);
                        data.DataCollection.Add(md);
                    }
                }

                foreach (GroupData gd in dataCollection.Values)
                    crd.Data.Add(gd);
                return crd;
            }
            catch { throw; }
        }

        public CountyReportData QueryCountyReportDetail2(DateTime day, Duration duration,string countys)
        {
            SqlParameter[] parameters = buildQueryParam(day, duration);
            DataTable dt = AQIQuery.aQuery.Data.GroupDailyAQI(day, day, countys, "304,305,301,308,306,307,302,300");
           
                //CountyReportData crd = QueryCountyReport(parameters);
                //if (null == crd)
                //    return null;
            CountyReportData crd = new CountyReportData();
                Dictionary<string, GroupData> dataCollection = new Dictionary<string, GroupData>();
                foreach (DataRow dr in dt.Rows)
                {
                    string groupId = dr["GroupID"].ToString();
                    if (string.IsNullOrEmpty(groupId))
                        continue;
                    GroupData data = null;
                    if (dataCollection.ContainsKey(groupId))
                        data = dataCollection[groupId];
                    else
                    {
                        data = new GroupData();
                        data.Group = new SiteGroup(groupId,"hehe");
                        data.Date = new PeriodOfTime();
                        data.Date.Value = day;
                        dataCollection[groupId] = data;
                    }
                    MonitoringData md = new MonitoringData();
                    md.Factor = new DataParameter();
                    //md.Factor.Id = reader.GetInt32(1);
                    //md.Thickness = (float)reader.GetDouble(2);
                    //md.API = reader.GetInt32(3);
                    md.Factor.Id = Convert.ToInt32(dr["AQIItemID"].ToString());
                    md.Thickness = (float)Convert.ToDouble(dr["value"]);
                    md.AQI = (int)dr["AQI"];
                    data.DataCollection.Add(md);

                }
                foreach (GroupData gd in dataCollection.Values)
                    crd.Data.Add(gd);
                return crd;
           
        }

        public Dictionary<DateTime, CountyReportData> QueryCountyReportDetail(DateTime d1, DateTime d2, Duration duration)
        {
            try
            {
                Dictionary<DateTime, CountyReportData> data = QueryCountyReport(d1, d2, duration);
                SqlParameter[] parameters = buildQueryParam2(d1, d2, duration);
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_COUNTYREPORTDATA_QUERY2, parameters))
                {
                    Dictionary<DateTime, Dictionary<string, GroupData>> dataContainer = new Dictionary<DateTime, Dictionary<string, GroupData>>();
                    while (reader.Read())
                    {
                        DateTime lst = reader.GetDateTime(4);
                        if (!data.ContainsKey(lst))
                            continue;
                        string groupId = reader[6] as string;
                        Dictionary<string, GroupData> groupData = null;
                        if (dataContainer.ContainsKey(lst))
                            groupData = dataContainer[lst];
                        else
                        {
                            groupData = new Dictionary<string, GroupData>();
                            dataContainer[lst] = groupData;
                        }

                        GroupData gd = null;
                        if (groupData.ContainsKey(groupId))
                            gd = groupData[groupId];
                        else
                        {
                            gd = new GroupData();
                            gd.Group = new SiteGroup(groupId, reader[7] as string);
                            gd.Date = new PeriodOfTime();
                            gd.Date.Start = d1;
                            gd.Date.End = d2;
                            gd.Date.Value = lst;
                            groupData[groupId] = gd;
                        }
                        MonitoringData md = new MonitoringData();
                        md.Factor = new DataParameter();
                        md.Factor.Id = reader.GetInt32(1);
                        md.Thickness = (float)reader.GetDouble(2);
                        md.API = reader.GetInt32(3);
                        gd.DataCollection.Add(md);
                    }
                    foreach (DateTime key in dataContainer.Keys)
                    {
                        CountyReportData crd = data[key];
                        Dictionary<string, GroupData> groups = dataContainer[key];
                        foreach (GroupData gd in groups.Values)
                            crd.Data.Add(gd);
                    }
                }
                return data;
            }
            catch { throw; }
        }

        public Dictionary<DateTime, CountyReportData> QueryCountyReportDetail(DateTime d1, DateTime d2, Duration duration, string[] countyId)
        {
            if (countyId.Length == 0)
                return new Dictionary<DateTime, CountyReportData>();
            try
            {
                Dictionary<DateTime, CountyReportData> data = QueryCountyReport(d1, d2, duration);
                SqlParameter[] parameters = buildCountyDataQueryParam(d1, d2, duration, countyId);
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_COUNTYREPORTDATA_QUERY3, parameters))
                {
                    Dictionary<DateTime, Dictionary<string, GroupData>> dataContainer = new Dictionary<DateTime, Dictionary<string, GroupData>>();
                    while (reader.Read())
                    {
                        DateTime lst = reader.GetDateTime(4);
                        if (!data.ContainsKey(lst))
                            continue;
                        string groupId = reader[6] as string;
                        Dictionary<string, GroupData> groupData = null;
                        if (dataContainer.ContainsKey(lst))
                            groupData = dataContainer[lst];
                        else
                        {
                            groupData = new Dictionary<string, GroupData>();
                            dataContainer[lst] = groupData;
                        }

                        GroupData gd = null;
                        if (groupData.ContainsKey(groupId))
                            gd = groupData[groupId];
                        else
                        {
                            gd = new GroupData();
                            gd.Group = new SiteGroup(groupId, reader[7] as string);
                            gd.Date = new PeriodOfTime();
                            gd.Date.Start = d1;
                            gd.Date.End = d2;
                            gd.Date.Value = lst;
                            groupData[groupId] = gd;
                        }
                        MonitoringData md = new MonitoringData();
                        md.Factor = new DataParameter();
                        md.Factor.Id = reader.GetInt32(1);
                        md.Thickness = (float)reader.GetDouble(2);
                        md.API = reader.GetInt32(3);
                        gd.DataCollection.Add(md);
                    }
                    foreach (DateTime key in dataContainer.Keys)
                    {
                        CountyReportData crd = data[key];
                        Dictionary<string, GroupData> groups = dataContainer[key];
                        foreach (GroupData gd in groups.Values)
                            crd.Data.Add(gd);
                    }
                }
                return data;
            }
            catch { throw; }
        }

        public Dictionary<DateTime, CountyReportData> QueryCountyReportDetail(DateTime d1, DateTime d2, Duration duration, string[] countyId, int[] parameterList)
        {
            if (countyId.Length == 0)
                return new Dictionary<DateTime, CountyReportData>();
            try
            {
                Dictionary<DateTime, CountyReportData> data = QueryCountyReport(d1, d2, duration);
                SqlParameter[] parameters = buildCountyDataQueryParam(d1, d2, duration, countyId, parameterList);
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_COUNTYREPORTDATA_QUERY5, parameters))
                {
                    Dictionary<DateTime, Dictionary<string, GroupData>> dataContainer = new Dictionary<DateTime, Dictionary<string, GroupData>>();
                    while (reader.Read())
                    {
                        DateTime lst = reader.GetDateTime(4);
                        if (!data.ContainsKey(lst))
                            continue;
                        string groupId = reader[6] as string;
                        Dictionary<string, GroupData> groupData = null;
                        if (dataContainer.ContainsKey(lst))
                            groupData = dataContainer[lst];
                        else
                        {
                            groupData = new Dictionary<string, GroupData>();
                            dataContainer[lst] = groupData;
                        }

                        GroupData gd = null;
                        if (groupData.ContainsKey(groupId))
                            gd = groupData[groupId];
                        else
                        {
                            gd = new GroupData();
                            gd.Group = new SiteGroup(groupId, reader[7] as string);
                            gd.Date = new PeriodOfTime();
                            gd.Date.Start = d1;
                            gd.Date.End = d2;
                            gd.Date.Value = lst;
                            groupData[groupId] = gd;
                        }
                        MonitoringData md = new MonitoringData();
                        md.Factor = new DataParameter();
                        md.Factor.Id = reader.GetInt32(1);
                        md.Thickness = (float)reader.GetDouble(2);
                        md.API = reader.GetInt32(3);
                        gd.DataCollection.Add(md);
                    }
                    foreach (DateTime key in dataContainer.Keys)
                    {
                        CountyReportData crd = data[key];
                        Dictionary<string, GroupData> groups = dataContainer[key];
                        foreach (GroupData gd in groups.Values)
                            crd.Data.Add(gd);
                    }
                }
                return data;
            }
            catch { throw; }
        }

        public List<int> QueryCountyReportParameters(DateTime d1, DateTime d2, Duration duration)
        {
            try
            {
                SqlParameter[] parameters = buildQueryParam2(d1, d2, duration);
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_COUNTYREPORT_GETPARAMETERS, parameters))
                {
                    List<int> dataList = new List<int>();
                    while (reader.Read())
                        dataList.Add(reader.GetInt32(0));
                    return dataList;
                }
            }
            catch { throw; }
        }

        private GroupData QueryGroupData(params SqlParameter[] param)
        {
            try
            {
                GroupData data = null;
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_GROUPDATA_QUERY, param))
                {
                    while (reader.Read())
                    {
                        if (null == data)
                        {
                            data = new GroupData();
                            data.Group = new SiteGroup(reader[6] as string, reader[7] as string);
                            data.Date = new PeriodOfTime();
                            data.Date.Value = reader.GetDateTime(4);
                        }
                        MonitoringData md = new MonitoringData();
                        md.Factor = new DataParameter();
                        md.Factor.Id = reader.GetInt32(1);
                        md.Thickness = (float)reader.GetDouble(2);
                        md.API = reader.GetInt32(3);
                        data.DataCollection.Add(md);
                    }
                }
                return data;
            }
            catch { throw; }
        }

        public Dictionary<DateTime, GroupData> QueryGroupData(DateTime d1, DateTime d2, Duration duration, string groupId)
        {
            try
            {
                SqlParameter paramD1 = new SqlParameter(PARAM_D1, SqlDbType.DateTime);
                SqlParameter paramD2 = new SqlParameter(PARAM_D2, SqlDbType.DateTime);
                SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
                SqlParameter paramGroupId = new SqlParameter(PARAM_GROUPID, SqlDbType.VarChar, 50);
                paramD1.Value = d1;
                paramD2.Value = d2;
                paramDuration.Value = duration;
                paramGroupId.Value = groupId;


                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_GROUPDATA_QUERY2, paramD1, paramD2, paramDuration, paramGroupId))
                {
                    Dictionary<DateTime, GroupData> data = new Dictionary<DateTime, GroupData>();
                    while (reader.Read())
                    {
                        DateTime lst = reader.GetDateTime(4);
                        GroupData gd = null;
                        if (data.ContainsKey(lst))
                            gd = data[lst];
                        else
                        {
                            gd = new GroupData();
                            gd.Date = new PeriodOfTime();
                            gd.Date.Value = lst;
                            gd.Date.Start = d1;
                            gd.Date.End = d2;
                            gd.Group = new SiteGroup(reader[6] as string, reader[7] as string);
                            data[lst] = gd;
                        }
                        MonitoringData md = new MonitoringData();
                        md.Factor = new DataParameter();
                        md.Factor.Id = reader.GetInt32(1);
                        md.Thickness = (float)reader.GetDouble(2);
                        md.API = reader.GetInt32(3);
                        gd.DataCollection.Add(md);
                    }
                    return data;
                }
            }
            catch { throw; }
        }

        public List<int> GetGroupDataParameters(DateTime d1, DateTime d2, Duration duration, string gropuID)
        {
            try
            {
                SqlParameter paramD1 = new SqlParameter(PARAM_D1, SqlDbType.DateTime);
                SqlParameter paramD2 = new SqlParameter(PARAM_D2, SqlDbType.DateTime);
                SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
                SqlParameter paramGroupID = new SqlParameter(PARAM_GROUPID, SqlDbType.VarChar, 50);

                paramD1.Value = d1;
                paramD2.Value = d2;
                paramDuration.Value = duration;
                paramGroupID.Value = gropuID;
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_GETGROUPDPARAMETERS, paramD1, paramD2, paramDuration, paramGroupID))
                {
                    List<int> dataList = new List<int>();
                    while (reader.Read())
                        dataList.Add(reader.GetInt32(0));
                    return dataList;
                }
            }
            catch { throw; }
        }

        public Dictionary<string, Dictionary<DateTime, GroupData>> QueryGroupData(DateTime d1, DateTime d2, Duration duration, string[] groupId)
        {
            if (groupId.Length == 0)
                return new Dictionary<string, Dictionary<DateTime, GroupData>>();
            SqlParameter[] parameters = buildGroupDataQueryParam(d1, d2, duration, groupId);
            try
            {
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_GROUPDATA_QUERY3, parameters))
                {
                    Dictionary<string, Dictionary<DateTime, GroupData>> dataContainer = new Dictionary<string, Dictionary<DateTime, GroupData>>();
                    while (reader.Read())
                    {
                        string gropuName = reader.GetString(7);
                        Dictionary<DateTime, GroupData> timeData = null;
                        if (dataContainer.ContainsKey(gropuName))
                            timeData = dataContainer[gropuName];
                        else
                        {
                            timeData = new Dictionary<DateTime, GroupData>();
                            dataContainer[gropuName] = timeData;
                        }
                        GroupData gd = null;
                        DateTime time = reader.GetDateTime(4);

                        if (timeData.ContainsKey(time))
                            gd = timeData[time];
                        else
                        {
                            gd = new GroupData();
                            gd.Group = new SiteGroup(reader.GetString(6), gropuName);
                            timeData[time] = gd;
                        }
                        MonitoringData md = new MonitoringData();
                        int paramId = reader.GetInt32(1);
                        md.Factor = new DataParameter();
                        md.Factor.Id = paramId;
                        md.Thickness = (float)reader.GetDouble(2);
                        md.API = reader.GetInt32(3);
                        gd.DataCollection.Add(md);
                    }
                    return dataContainer;
                }
            }
            catch { throw; }
        }

        public Dictionary<string, Dictionary<DateTime, GroupData>> QueryGroupData(DateTime d1, DateTime d2, Duration duration, string[] groupId,bool flag)
        {
            if (groupId.Length == 0)
                return new Dictionary<string, Dictionary<DateTime, GroupData>>();
             DataTable  dt = AQIQuery.aQuery.Data.GroupHourlyAQI(Convert.ToDateTime("2014-1-1"), Convert.ToDateTime("2014-1-6"), "201,183,195", "106,107,103,202,108,104,105,101,201");
            try
            {
                
                    Dictionary<string, Dictionary<DateTime, GroupData>> dataContainer = new Dictionary<string, Dictionary<DateTime, GroupData>>();
                   foreach(DataRow dr in dt.Rows){
                       string gropuName = dr["d"].ToString();
                        Dictionary<DateTime, GroupData> timeData = null;
                        if (dataContainer.ContainsKey(gropuName))
                            timeData = dataContainer[gropuName];
                        else
                        {
                            timeData = new Dictionary<DateTime, GroupData>();
                            dataContainer[gropuName] = timeData;
                        }
                        GroupData gd = null;
                        DateTime time = DateTime.Parse(dr[""].ToString());

                        if (timeData.ContainsKey(time))
                            gd = timeData[time];
                        else
                        {
                            gd = new GroupData();
                            gd.Group = new SiteGroup(dr[""].ToString(), gropuName);
                            timeData[time] = gd;
                        }
                        MonitoringData md = new MonitoringData();
                        int paramId =int.Parse(dr[""].ToString());
                        md.Factor = new DataParameter();
                        md.Factor.Id = paramId;
                        //md.Thickness = (float)reader.GetDouble(2);
                        md.Thickness = (float)dr[""];
                        //md.API = reader.GetInt32(3);
                        md.API = int.Parse(dr[""].ToString());
                        gd.DataCollection.Add(md);
                    }
                    return dataContainer;
                
            }
            catch { throw; }
        }

        public List<int> GetGroupDataParameters(DateTime d1, DateTime d2, Duration duration, string[] gropuID)
        {
            SqlParameter[] parameters = buildGroupDataQueryParam(d1, d2, duration, gropuID);
            try
            {
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_GROUPSITEDATA_PARAMETERS3, parameters))
                {
                    List<int> dataList = new List<int>();
                    while (reader.Read())
                    {
                        dataList.Add(reader.GetInt32(0));
                    }
                    return dataList;
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// 根据编号查询组数据
        /// </summary>
        /// <returns></returns>
        public GroupData QueryGroupData(string groupId, DateTime day, Duration duration)
        {
            SqlParameter[] param = buildGroupQueryParam(groupId, day, duration);
            try
            {
                return QueryGroupData(param);
            }
            catch { throw; }
        }

        /// <summary>
        /// 查询某个日期，某个期间的组数据
        /// </summary>
        /// <returns></returns>
        public List<GroupData> QueryGroupData(DateTime day, Duration duration)
        {
            SqlParameter[] param = buildQueryParam(day, duration);
            try
            {
                Dictionary<string, GroupData> dataCollection = new Dictionary<string, GroupData>();
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_GROUPDATA_QUERYLIST, param))
                {
                    while (reader.Read())
                    {
                        string groupId = reader[6] as string;
                        if (string.IsNullOrEmpty(groupId))
                            continue;
                        GroupData data = null;
                        if (dataCollection.ContainsKey(groupId))
                            data = dataCollection[groupId];
                        else
                        {
                            data = new GroupData();
                            data.Group = new SiteGroup(groupId, reader[7] as string);
                            data.Date = new PeriodOfTime();
                            data.Date.Value = day;
                            dataCollection[groupId] = data;
                        }
                        MonitoringData md = new MonitoringData();
                        md.Factor = new DataParameter();
                        md.Factor.Id = reader.GetInt32(1);
                        md.Thickness = (float)reader.GetDouble(2);
                        md.API = reader.GetInt32(3);
                        data.DataCollection.Add(md);
                    }
                }
                List<GroupData> dataList = new List<GroupData>();
                foreach (GroupData gd in dataCollection.Values)
                    dataList.Add(gd);
                return dataList;
            }
            catch { throw; }
        }

        public GroupData QueryGroupDetail(string groupId, DateTime day, Duration duration)
        {
            try
            {
                SqlParameter[] param = buildGroupQueryParam(groupId, day, duration);
                GroupData gd = QueryGroupData(param);
                if (null != gd)
                {
                    using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_GROUPSITEDATE_QUERY, param))
                    {
                        List<MMShareBLL.Model.SiteData> dataList = getSiteData(reader);
                        gd.SetSiteData(dataList);
                    }
                }
                return gd;
            }
            catch { throw; }
        }

        public List<int> GetGroupParameters(DateTime day, Duration duration)
        {
            SqlParameter[] param = buildQueryParam(day, duration);
            try
            {
                List<int> paramList = new List<int>();
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_GROUPSITEDATA_PARAMETERS, param))
                {
                    while (reader.Read())
                    {
                        paramList.Add(reader.GetInt32(0));
                    }
                }
                return paramList;
            }
            catch { throw; }
        }

        /// <summary>
        /// 查询某个时间分区数据下的站点组编号列表
        /// </summary>
        /// <returns></returns>
        public List<string> QueryCountyReportGroups(DateTime day, Duration duration)
        {
            SqlParameter[] param = buildQueryParam(day, duration);
            try
            {
                List<string> data = new List<string>();
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_COUNTYREPORTGROUPS_QUERY, param))
                {
                    while (reader.Read())
                        data.Add(reader[0] as string);
                }
                return data;
            }
            catch { throw; }
        }

        /// <summary>
        /// 获得某个期间站点的均值数据
        /// </summary>
        /// <returns></returns>
        public List<MMShareBLL.Model.SiteData> QuerySiteData(DateTime day, Duration duration)
        {
            SqlParameter[] param = buildQueryParam(day, duration);
            try
            {
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_SITEDATA_QUERY, param))
                {
                    return getSiteData(reader);
                }

            }
            catch { throw; }
        }

        public MMShareBLL.Model.SiteData QuerySiteData(DateTime time, int siteId, int[] parameters, Duration duration)
        {
            if (parameters.Length == 0)
                return null;
            SqlParameter paramTime = new SqlParameter(PARAM_DAY, SqlDbType.DateTime);
            SqlParameter paramSiteId = new SqlParameter(PARAM_SITEID, SqlDbType.Int);
            SqlParameter paramParameters = new SqlParameter(PARAM_PARAMETERS, SqlDbType.VarChar, 300);
            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
            paramTime.Value = time;
            paramSiteId.Value = siteId;
            paramDuration.Value = duration;
            string strParameters = "";
            foreach (int paramId in parameters)
                strParameters += paramId + ",";
            strParameters = strParameters.Substring(0, strParameters.Length - 1);
            paramParameters.Value = strParameters;
            try
            {
                MMShareBLL.Model.SiteData data = null;
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_SITEDATA_QUERY4, paramTime, paramSiteId, paramParameters, paramDuration))
                {
                    List<MMShareBLL.Model.SiteData> dataList = getSiteData(reader);
                    if (dataList.Count > 0)
                        data = dataList[0];
                }
                return data;
            }
            catch { throw; }
        }

        public DataTable QuerySiteDataTable(DateTime day, Duration duration)
        {
            SqlParameter[] param = buildQueryParam(day, duration);
            DataTable t = new DataTable();
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(PROC_SITEDATA_QUERY, m_DatabaseS.ConnectionString);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.AddRange(param);
                adapter.Fill(t);
                return t;
            }
            catch { throw; }
        }

        public DataTable QuerySiteDataTable(DateTime d1, DateTime d2, int siteId, Duration duration)
        {
            SqlParameter paramSiteId = new SqlParameter(PARAM_SITEID, SqlDbType.NVarChar, 50);
            SqlParameter paramD1 = new SqlParameter(PARAM_D1, SqlDbType.DateTime);
            SqlParameter paramD2 = new SqlParameter(PARAM_D2, SqlDbType.DateTime);
            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
            paramSiteId.Value = siteId;
            paramD1.Value = d1;
            paramD2.Value = d2;
            paramDuration.Value = (int)duration;

            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(PROC_SITEDATA_QUERY2, m_DatabaseS.ConnectionString);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.Add(paramSiteId);
                adapter.SelectCommand.Parameters.Add(paramD1);
                adapter.SelectCommand.Parameters.Add(paramD2);
                adapter.SelectCommand.Parameters.Add(paramDuration);
                DataTable data = new DataTable();
                adapter.Fill(data);
                return data;
            }
            catch { throw; }
        }


        /// <summary>
        /// DateTime d1, DateTime d2, int[] siteList, int[] paramters, Duration duration
        /// 返回的DataTable包括的列有：ID,SiteID,Duration,ParameterID,Value,API,LST,CreateTime
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <param name="siteList"></param>
        /// <param name="paramters"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public DataTable GetSiteData(DateTime d1, DateTime d2, int[] siteList, int[] paramters, Duration duration)
        {
            //if (siteList.Length == 0)
            //    return new Dictionary<int, Dictionary<DateTime, SiteData>>();

            SqlParameter[] parameters = buildSiteDataQueryParam(d1, d2, siteList, paramters, duration);
            try
            {
                SqlConnection connection = new SqlConnection(m_DatabaseS.ConnectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(PROC_SITEDATA_QUERY_WITHPARAMETER, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter param in parameters)
                {
                    adapter.SelectCommand.Parameters.Add(param);
                }
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
            catch { throw; }
        }

        public Dictionary<int, Dictionary<DateTime, MMShareBLL.Model.SiteData>> GetSiteData(DateTime d1, DateTime d2, int[] siteList, Duration duration)
        {
            if (siteList.Length == 0)
                return new Dictionary<int, Dictionary<DateTime, MMShareBLL.Model.SiteData>>();

            SqlParameter[] parameters = buildSiteDataQueryParam(d1, d2, siteList, duration);
            try
            {
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_SITEDATA_QUERY3, parameters))
                {
                    Dictionary<int, Dictionary<DateTime, MMShareBLL.Model.SiteData>> dataContainer = new Dictionary<int, Dictionary<DateTime, MMShareBLL.Model.SiteData>>();
                    while (reader.Read())
                    {
                        int siteId = reader.GetInt32(1);
                        Dictionary<DateTime, MMShareBLL.Model.SiteData> timeData = null;
                        if (dataContainer.ContainsKey(siteId))
                            timeData = dataContainer[siteId];
                        else
                        {
                            timeData = new Dictionary<DateTime, MMShareBLL.Model.SiteData>();
                            dataContainer[siteId] = timeData;
                        }
                        DateTime time = reader.GetDateTime(6);
                        MMShareBLL.Model.SiteData siteData = null;
                        if (timeData.ContainsKey(time))
                            siteData = timeData[time];
                        else
                        {
                            siteData = new MMShareBLL.Model.SiteData();
                            timeData[time] = siteData;
                        }
                        int parameter = reader.GetInt32(3);
                        MonitoringData md = new MonitoringData();
                        md.Factor = new DataParameter();
                        md.Factor.Id = parameter;
                        md.Thickness = (float)reader.GetDouble(4);
                        md.API = reader.GetInt32(5);
                        siteData.DataCollection.Add(md);
                    }
                    return dataContainer;
                }
            }
            catch { throw; }
        }

        public Dictionary<int, MMShareBLL.Model.SiteData> GetSiteAVGData(DateTime d1, DateTime d2, int[] siteList, Duration duration)
        {
            if (siteList.Length == 0)
                return new Dictionary<int, MMShareBLL.Model.SiteData>();

            SqlParameter[] parameters = buildSiteDataQueryParam(d1, d2, siteList, duration);
            try
            {
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_SITEAVGDATA_QUERY, parameters))
                {
                    Dictionary<int, MMShareBLL.Model.SiteData> dataContainer = new Dictionary<int, MMShareBLL.Model.SiteData>();
                    while (reader.Read())
                    {
                        int siteId = reader.GetInt32(0);
                        MMShareBLL.Model.SiteData siteData = null;
                        if (dataContainer.ContainsKey(siteId))
                            siteData = dataContainer[siteId];
                        else
                        {
                            siteData = new MMShareBLL.Model.SiteData();
                            siteData.ID = siteId;
                            dataContainer[siteId] = siteData;
                        }
                        int parameter = reader.GetInt32(1);
                        
                        MonitoringData md = new MonitoringData();
                        
                        md.Factor = new DataParameter();
                        md.Factor.Id = parameter;
                        md.Thickness = (float)reader.GetDouble(2);
                        siteData.DataCollection.Add(md);
                    }
                    return dataContainer;
                }
            }
            catch { throw; }
        }

        public Dictionary<int, Dictionary<DateTime, MMShareBLL.Model.SiteData>> GetSiteDataDic(DateTime d1, DateTime d2, int[] siteList, int[] paramters, Duration duration)
        {
            if (siteList.Length == 0)
                return new Dictionary<int, Dictionary<DateTime, MMShareBLL.Model.SiteData>>();

            SqlParameter[] parameters = buildSiteDataQueryParam(d1, d2, siteList, paramters, duration);
            try
            {
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_SITEDATA_QUERY5, parameters))
                {
                    Dictionary<int, Dictionary<DateTime, MMShareBLL.Model.SiteData>> dataContainer = new Dictionary<int, Dictionary<DateTime, MMShareBLL.Model.SiteData>>();
                    while (reader.Read())
                    {
                        int siteId = reader.GetInt32(1);
                        Dictionary<DateTime, MMShareBLL.Model.SiteData> timeData = null;
                        if (dataContainer.ContainsKey(siteId))
                            timeData = dataContainer[siteId];
                        else
                        {
                            timeData = new Dictionary<DateTime, MMShareBLL.Model.SiteData>();
                            dataContainer[siteId] = timeData;
                        }
                        DateTime time = reader.GetDateTime(6);
                        MMShareBLL.Model.SiteData siteData = null;
                        if (timeData.ContainsKey(time))
                            siteData = timeData[time];
                        else
                        {
                            siteData = new MMShareBLL.Model.SiteData();
                            timeData[time] = siteData;
                        }
                        int parameter = reader.GetInt32(3);
                        MonitoringData md = new MonitoringData();
                        md.Factor = new DataParameter();
                        md.Factor.Id = parameter;
                        md.Thickness = (float)reader.GetDouble(4);
                        md.API = reader.GetInt32(5);
                        siteData.DataCollection.Add(md);
                    }
                    return dataContainer;
                }
            }
            catch { throw; }
        }

        public List<int> GetSiteDataParameters(DateTime d1, DateTime d2, int[] siteList, Duration duration)
        {
            if (siteList.Length == 0)
                return new List<int>();
            SqlParameter[] parameters = buildSiteDataQueryParam(d1, d2, siteList, duration);
            try
            {
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_SITEDATA_QUERY3_GETPARAMETER, parameters))
                {
                    List<int> dataList = new List<int>();
                    while (reader.Read())
                        dataList.Add(reader.GetInt32(0));
                    return dataList;
                }
            }
            catch { throw; }
        }

        private List<MMShareBLL.Model.SiteData> getSiteData(SqlDataReader reader)
        {
            Dictionary<int, MMShareBLL.Model.SiteData> dataCollection = new Dictionary<int, MMShareBLL.Model.SiteData>();
            while (reader.Read())
            {
                int siteId = reader.GetInt32(1);
                MMShareBLL.Model.SiteData sd = null;
                if (dataCollection.ContainsKey(siteId))
                    sd = dataCollection[siteId];
                else
                {
                    sd = new MMShareBLL.Model.SiteData();
                    sd.Date = new PeriodOfTime();
                    sd.Date.Value = reader.GetDateTime(6);
                    sd.Site = new Site(siteId, null);
                    dataCollection[siteId] = sd;
                }
                MonitoringData md = new MonitoringData();
                md.Factor = new DataParameter();
                md.Factor.Id = reader.GetInt32(3);
                md.Thickness = (float)reader.GetDouble(4);
                md.API = reader.GetInt32(5);
                sd.DataCollection.Add(md);
            }
            List<MMShareBLL.Model.SiteData> dataList = new List<MMShareBLL.Model.SiteData>();
            foreach (MMShareBLL.Model.SiteData sd in dataCollection.Values)
                dataList.Add(sd);
            return dataList;
        }

        private SqlParameter[] buildQueryParam(DateTime day, Duration duration)
        {
            SqlParameter[] paramArray = new SqlParameter[2];
            SqlParameter paramDay = new SqlParameter(PARAM_DAY, SqlDbType.DateTime);
            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
            paramDay.Value = day;
            paramDuration.Value = duration;
            paramArray[0] = paramDay;
            paramArray[1] = paramDuration;
            return paramArray;
        }

        private SqlParameter[] buildQueryParam2(DateTime d1, DateTime d2, Duration duration)
        {
            SqlParameter[] paramArray = new SqlParameter[3];
            SqlParameter paramD1 = new SqlParameter(PARAM_D1, SqlDbType.DateTime);
            SqlParameter paramD2 = new SqlParameter(PARAM_D2, SqlDbType.DateTime);
            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
            paramD1.Value = d1;
            paramD2.Value = d2;
            paramDuration.Value = duration;
            paramArray[0] = paramD1;
            paramArray[1] = paramD2;
            paramArray[2] = paramDuration;
            return paramArray;
        }

        private SqlParameter[] buildQueryParam2(DateTime d1, DateTime d2, Duration duration, int[] parameterList)
        {
            SqlParameter[] paramArray = new SqlParameter[4];
            SqlParameter paramD1 = new SqlParameter(PARAM_D1, SqlDbType.DateTime);
            SqlParameter paramD2 = new SqlParameter(PARAM_D2, SqlDbType.DateTime);
            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
            SqlParameter paramParameterID = new SqlParameter(PARAM_PARAMETERS, SqlDbType.VarChar);

            StringBuilder sbParameterId = new StringBuilder();
            foreach (int pi in parameterList)
            {
                sbParameterId.Append("'");
                sbParameterId.Append(pi.ToString().Replace("'", "''"));
                sbParameterId.Append("',");
            }
            string strParameterIdList = sbParameterId.ToString();
            if (strParameterIdList.Length > 0)
                strParameterIdList = strParameterIdList.Substring(0, strParameterIdList.Length - 1);
            paramParameterID.Value = strParameterIdList;
            paramD1.Value = d1;
            paramD2.Value = d2;
            paramDuration.Value = duration;
            paramArray[0] = paramD1;
            paramArray[1] = paramD2;
            paramArray[2] = paramDuration;
            paramArray[3] = paramParameterID;
            return paramArray;
        }

        private SqlParameter[] buildGroupDataQueryParam(DateTime d1, DateTime d2, Duration duration, string[] groupId)
        {
            SqlParameter paramD1 = new SqlParameter(PARAM_D1, SqlDbType.DateTime);
            SqlParameter paramD2 = new SqlParameter(PARAM_D2, SqlDbType.DateTime);
            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
            SqlParameter paramGroupID = new SqlParameter(PARAM_GROUPID, SqlDbType.VarChar);

            paramD1.Value = d1;
            paramD2.Value = d2;
            paramDuration.Value = duration;

            StringBuilder sbGroupId = new StringBuilder();
            foreach (string gi in groupId)
            {
                sbGroupId.Append("'");
                sbGroupId.Append(gi.Replace("'", "''"));
                sbGroupId.Append("',");
            }
            string strGroupIdList = sbGroupId.ToString();
            if (groupId.Length > 0)
                strGroupIdList = strGroupIdList.Substring(0, strGroupIdList.Length - 1);
            paramGroupID.Value = strGroupIdList;

            return new SqlParameter[] { paramD1, paramD2, paramDuration, paramGroupID };
        }

        private SqlParameter[] buildCountyDataQueryParam(DateTime d1, DateTime d2, Duration duration, string[] countyId)
        {
            SqlParameter paramD1 = new SqlParameter(PARAM_D1, SqlDbType.DateTime);
            SqlParameter paramD2 = new SqlParameter(PARAM_D2, SqlDbType.DateTime);
            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
            SqlParameter paramGroupID = new SqlParameter(PARAM_GROUPID, SqlDbType.VarChar);

            paramD1.Value = d1;
            paramD2.Value = d2;
            paramDuration.Value = duration;

            StringBuilder sbCountyId = new StringBuilder();
            foreach (string ci in countyId)
            {
                sbCountyId.Append("'");
                sbCountyId.Append(ci.Replace("'", "''"));
                sbCountyId.Append("',");
            }
            string strCountyIdList = sbCountyId.ToString();
            if (countyId.Length > 0)
                strCountyIdList = strCountyIdList.Substring(0, strCountyIdList.Length - 1);
            paramGroupID.Value = strCountyIdList;

            return new SqlParameter[] { paramD1, paramD2, paramDuration, paramGroupID };
        }

        private SqlParameter[] buildCountyDataQueryParam(DateTime d1, DateTime d2, Duration duration, string[] countyId, int[] parameterList)
        {
            SqlParameter paramD1 = new SqlParameter(PARAM_D1, SqlDbType.DateTime);
            SqlParameter paramD2 = new SqlParameter(PARAM_D2, SqlDbType.DateTime);
            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
            SqlParameter paramGroupID = new SqlParameter(PARAM_GROUPID, SqlDbType.VarChar);
            SqlParameter paramParameterID = new SqlParameter(PARAM_PARAMETERS, SqlDbType.VarChar);

            paramD1.Value = d1;
            paramD2.Value = d2;
            paramDuration.Value = duration;

            StringBuilder sbCountyId = new StringBuilder();
            foreach (string ci in countyId)
            {
                sbCountyId.Append("'");
                sbCountyId.Append(ci.Replace("'", "''"));
                sbCountyId.Append("',");
            }
            string strCountyIdList = sbCountyId.ToString();
            if (countyId.Length > 0)
                strCountyIdList = strCountyIdList.Substring(0, strCountyIdList.Length - 1);
            paramGroupID.Value = strCountyIdList;

            StringBuilder sbParameterId = new StringBuilder();
            foreach (int pi in parameterList)
            {
                sbParameterId.Append("'");
                sbParameterId.Append(pi.ToString().Replace("'", "''"));
                sbParameterId.Append("',");
            }
            string strParameterIdList = sbParameterId.ToString();
            if (strParameterIdList.Length > 0)
                strParameterIdList = strParameterIdList.Substring(0, strParameterIdList.Length - 1);
            paramParameterID.Value = strParameterIdList;

            return new SqlParameter[] { paramD1, paramD2, paramDuration, paramGroupID, paramParameterID };
        }

        private SqlParameter[] buildSiteDataQueryParam(DateTime d1, DateTime d2, int[] siteList, Duration duration)
        {
            SqlParameter[] parameters = new SqlParameter[4];
            SqlParameter paramSiteId = new SqlParameter(PARAM_SITEID, SqlDbType.VarChar);
            SqlParameter paramD1 = new SqlParameter(PARAM_D1, SqlDbType.DateTime);
            SqlParameter paramD2 = new SqlParameter(PARAM_D2, SqlDbType.DateTime);
            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
            paramD1.Value = d1;
            paramD2.Value = d2;
            paramDuration.Value = (int)duration;
            StringBuilder sbSites = new StringBuilder();
            foreach (int siteId in siteList)
            {
                sbSites.Append(siteId);
                sbSites.Append(",");
            }
            string siteStr = sbSites.ToString();
            siteStr = siteStr.Substring(0, siteStr.Length - 1);
            paramSiteId.Value = siteStr;

            parameters[0] = paramD1;
            parameters[1] = paramD2;
            parameters[2] = paramDuration;
            parameters[3] = paramSiteId;
            return parameters;
        }

        private SqlParameter[] buildSiteDataQueryParam(DateTime d1, DateTime d2, int[] siteList, int[] paramters, Duration duration)
        {
            SqlParameter[] parameters = new SqlParameter[5];


            SqlParameter paramD1 = new SqlParameter(PARAM_D1, SqlDbType.DateTime);
            SqlParameter paramD2 = new SqlParameter(PARAM_D2, SqlDbType.DateTime);
            SqlParameter paramSiteId = new SqlParameter(PARAM_SITEID, SqlDbType.VarChar);
            SqlParameter paramParamters = new SqlParameter(PARAM_PARAMETERS, SqlDbType.VarChar);
            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
            paramD1.Value = d1;
            paramD2.Value = d2;
            paramDuration.Value = (int)duration;
            StringBuilder sbSites = new StringBuilder();
            foreach (int siteId in siteList)
            {
                sbSites.Append(siteId);
                sbSites.Append(",");
            }
            string siteStr = sbSites.ToString();
            siteStr = siteStr.Substring(0, siteStr.Length - 1);
            paramSiteId.Value = siteStr;

            StringBuilder sbParamters = new StringBuilder();
            foreach (int pa in paramters)
            {
                sbParamters.Append(pa);
                sbParamters.Append(",");
            }
            string paramtersStr = sbParamters.ToString();
            paramtersStr = paramtersStr.Substring(0, paramtersStr.Length - 1);
            paramParamters.Value = paramtersStr;

            parameters[0] = paramD1;
            parameters[1] = paramD2;
            parameters[2] = paramSiteId;
            parameters[3] = paramParamters;
            parameters[4] = paramDuration;
            return parameters;
        }


        private SqlParameter[] buildGroupQueryParam(string groupId, DateTime day, Duration duration)
        {
            SqlParameter paramGroupID = new SqlParameter(PARAM_GROUPID, SqlDbType.NVarChar, 50);
            SqlParameter paramDay = new SqlParameter(PARAM_DAY, SqlDbType.DateTime);
            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
            paramGroupID.Value = groupId;
            paramDay.Value = day;
            paramDuration.Value = (int)duration;

            SqlParameter[] paramArray = new SqlParameter[3];
            paramArray[0] = paramGroupID;
            paramArray[1] = paramDay;
            paramArray[2] = paramDuration;

            return paramArray;
        }

        private CitywideData pickUpCitywideData(SqlDataReader reader)
        {
            CitywideData data = new CitywideData();
            data.ID = reader.GetInt32(0);
            data.Time = new PeriodOfTime();
            data.Time.Value = reader.GetDateTime(1);
            data.Duration = (Duration)reader.GetInt32(2);
            data.API = reader.GetInt32(3);
            data.LastModifyTime = reader.GetDateTime(4);
            data.State = (DailyState)reader.GetInt32(5);
            data.LastModifyPersion = reader.GetString(6);
            data.IsAuto = reader.GetBoolean(7);
            object refSiteObj = reader[8];
            object fullData = reader[9];
            if (DBNull.Value != refSiteObj)
                data.ReferenceSite = new Site(Convert.ToInt32(refSiteObj.ToString()), null);
            if (DBNull.Value != fullData)
                data.FullData = Convert.ToBoolean(fullData);
            return data;
        }

        private CountyReportData pickUpCountyReportData(SqlDataReader reader)
        {
            CountyReportData data = new CountyReportData();
            data.ID = reader.GetInt32(0);
            data.Day = reader.GetDateTime(1);
            data.LastModifyTime = reader.GetDateTime(3);
            data.Status = (DailyState)reader.GetInt32(4);
            data.LastModifyPersion = reader.GetString(5);
            data.IsAuto = reader.GetBoolean(6);
            return data;
        }

        public DataTable GetDataBrowserData(DateTime d1, DateTime d2)
        {
            SqlParameter paramD1 = new SqlParameter(PARAM_D1, SqlDbType.DateTime);
            SqlParameter paramD2 = new SqlParameter(PARAM_D2, SqlDbType.DateTime);
            paramD1.Value = d1;
            paramD2.Value = d2;
            try
            {
                SqlConnection connection = new SqlConnection(m_DatabaseS.ConnectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(PROC_GET_DATABROWSER, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.Add(paramD1);
                adapter.SelectCommand.Parameters.Add(paramD2);
                DataTable table = new DataTable();
                adapter.Fill(table);
                //adapter.SelectCommand.Parameters.Clear();
                //using (SqlDataReader rader = m_DatabaseS.GetDataReader( PROC_GET_DATABROWSER, paramD1, paramD2))
                //{
                //    while (rader.Read())
                //    {
                //        Console.WriteLine(rader[0] + "--" + rader[1] + "--" + rader[2] + "--" + rader[3]);
                //    }
                //}

                return table;
            }
            catch { throw; }
        }

        public DateTime? GetLastGroupDate(Duration duration)
        {
            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
            paramDuration.Value = (int)duration;
            DateTime? date = null;
            using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_GET_LASTGROUPDATATIME, paramDuration))
            {
                if (reader.Read())
                {
                    date = Convert.ToDateTime(reader[0]);
                }
            }
            return date;
        }
    }
}

