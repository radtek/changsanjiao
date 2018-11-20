using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using MMShareBLL.Model;
using MMShareBLL.BLL;
using Readearth.Data;

namespace  MMShareBLL.DAL
{
    /// <summary>
    /// DMS实测数据的获取
    /// </summary>
    public class MonitoringDataDal
    {
        private bool useOneConnection = false;

        public bool UseOneConnection
        {
            get { return useOneConnection; }
            set
            {
                useOneConnection = value;
                if (!useOneConnection)
                    closeConn();
            }
        }
        private Database m_DatabaseS;
        public MonitoringDataDal()
        {
            m_DatabaseS = new Database("SEMCDMC");
        }
        private SqlConnection conn = null;

        //Procedure
        private const string PROC_GETDSMHOURDATA = "DataFullView_GetData";
        private const string PROC_GETAQIDATA = "AQI_GetData";
        private const string PROC_GETDSMHOURDATA2 = "Pro_GetHourData";
        private const string PROC_GETLASTDATAHOUR = "DMS_Data_getLastDataHour";
        private const string DAILY_DMSDATA_QUERY = "Daily_DMSData_Query";
        private const string DAILY_DMSDATA_QUERY2 = "Daily_DMSData_Query2";
        private const string DAILY_DMSDATA_QUERY3 = "Daily_DMSData_Quer3";

        //Parameters
        private const string PARAM_DURATION = "@Duration";
        private const string PARAM_SITES = "@Sites";
        private const string PARAM_PARAMETER = "@Parameter";
        private const string PARAM_LST1 = "@LST1";
        private const string PARAM_LST2 = "@LST2";
        private const string PARAM_D1 = "@d1";
        private const string PARAM_D2 = "@d2";
        private const string PARAM_SITEID = "@siteId";
        private const string PARAM_VALIDATED = "@validated";

        public Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> GetData(Dictionary<int, Site> sites, Dictionary<int, DataParameter> parameters, DateTime startTime, DateTime endTime, int duration, DataValidated validated)
        {

            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, System.Data.SqlDbType.Int);
            SqlParameter paramSites = new SqlParameter(PARAM_SITES, System.Data.SqlDbType.VarChar);
            SqlParameter paramParameter = new SqlParameter(PARAM_PARAMETER, System.Data.SqlDbType.VarChar);
            SqlParameter paramLST1 = new SqlParameter(PARAM_LST1, System.Data.SqlDbType.DateTime);
            SqlParameter paramLST2 = new SqlParameter(PARAM_LST2, System.Data.SqlDbType.DateTime);
            SqlParameter paramValidated = new SqlParameter(PARAM_VALIDATED, System.Data.SqlDbType.Int);
            try
            {
                paramDuration.Value = duration;
                paramLST1.Value = startTime;
                paramLST2.Value = endTime;
                paramValidated.Value = (int)validated;
                StringBuilder strSites = new StringBuilder("");
                StringBuilder strParameter = new StringBuilder("");

                foreach (int key in sites.Keys)
                    strSites.Append(sites[key].Id).Append(",");
                if (strSites.ToString().IndexOf(',') > 0)
                    strSites = strSites.Remove(strSites.Length - 1, 1);
                if (null != parameters)
                {
                    foreach (int key in parameters.Keys)
                        strParameter.Append(parameters[key].Id).Append(",");
                }
                if (strParameter.ToString().IndexOf(',') > 0)
                    strParameter = strParameter.Remove(strParameter.Length - 1, 1);

                paramSites.Value = strSites.ToString();
                paramParameter.Value = strParameter.ToString();

                Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataStore = new Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>>();

                SiteDal sdm = new SiteDal();

                FactorDataManager fdm = new FactorDataManager();
                if (null == parameters || parameters.Count == 0)
                    parameters = fdm.QueryParameter();

                if (null == sites || sites.Count == 0)
                    sites = new SiteDal().Query();
                if (!UseOneConnection)
                {
                    using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_GETDSMHOURDATA,
                        paramParameter, paramSites, paramLST2, paramLST1, paramDuration, paramValidated))
                    {
                        disData(sites, parameters, dataStore, fdm, reader, validated);
                        return dataStore;
                    }
                }
                else
                {
                    if (null == conn)
                        conn = new SqlConnection(m_DatabaseS.ConnectionString);
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();
                    using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_GETDSMHOURDATA,
                        paramParameter, paramSites, paramLST2, paramLST1, paramDuration))
                    {
                        disData(sites, parameters, dataStore, fdm, reader, validated);
                        return dataStore;
                    }
                }
            }
            catch { throw; }

        }

        private static void disData(Dictionary<int, Site> sites, Dictionary<int, DataParameter> parameters, Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataStore, FactorDataManager fdm, SqlDataReader reader, DataValidated validated)
        {
            while (reader.Read())
            {
                MMShareBLL.Model.SiteData qData = null;
                Dictionary<int, MMShareBLL.Model.SiteData> timeData = null;
                DateTime lst = Convert.ToDateTime(reader["LST"]);
                if (dataStore.ContainsKey(lst))
                    timeData = dataStore[lst];
                else
                {
                    timeData = new Dictionary<int, MMShareBLL.Model.SiteData>();
                    dataStore.Add(lst, timeData);
                }

                int siteID = Convert.ToInt32(reader["SiteID"]);

                if (timeData.ContainsKey(siteID))
                    qData = timeData[siteID];
                else
                {
                    qData = new MMShareBLL.Model.SiteData();
                    qData.Date = new PeriodOfTime();
                    qData.Date.Value = new DateTime(lst.Year, lst.Month, lst.Day, lst.Hour, 0, 0);
                    qData.Site = sites[siteID];
                    timeData[siteID] = qData;
                }
                int parameterID = Convert.ToInt32(reader["ParameterID"]);

                MonitoringData factorData = new MonitoringData();

                if (null != parameters && parameters.ContainsKey(parameterID))
                    factorData.Factor = parameters[parameterID];
                else
                {
                    factorData.Factor = fdm.QueryParameter(parameterID);
                    if (null != factorData.Factor)
                        parameters[parameterID] = factorData.Factor;
                }

                factorData.Thickness = float.Parse(reader["Value"].ToString());
                factorData.Valid = Convert.ToBoolean(reader["IsValidData"]);

                //if (!factorData.Valid)
                //{
                factorData.OPCode = Convert.ToInt32(reader["OPCode"]);
                //}
                factorData.QCCode = Convert.ToInt32(reader["QCCode"]);
                factorData.DataID = Convert.ToInt32(reader["DataID"]);
                factorData.OPName = reader["OPName"].ToString();
                qData.DataCollection.Add(factorData);
            }
        }

        public Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> GetHourData(Dictionary<int, Site> sites, Dictionary<int, DataParameter> parameters, DateTime startTime, DateTime endTime, int duration)
        {

            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, System.Data.SqlDbType.Int);
            SqlParameter paramSites = new SqlParameter(PARAM_SITES, System.Data.SqlDbType.VarChar);
            SqlParameter paramParameter = new SqlParameter(PARAM_PARAMETER, System.Data.SqlDbType.VarChar);
            SqlParameter paramLST1 = new SqlParameter(PARAM_LST1, System.Data.SqlDbType.DateTime);
            SqlParameter paramLST2 = new SqlParameter(PARAM_LST2, System.Data.SqlDbType.DateTime);
            try
            {
                paramDuration.Value = duration;
                paramLST1.Value = startTime;
                paramLST2.Value = endTime;
                StringBuilder strSites = new StringBuilder("");
                StringBuilder strParameter = new StringBuilder("");

                if (null != parameters)
                {
                    foreach (int key in parameters.Keys)
                        strParameter.Append(parameters[key].Id).Append(",");
                }
                if (strParameter.ToString().IndexOf(',') > 0)
                    strParameter = strParameter.Remove(strParameter.Length - 1, 1);
                paramParameter.Value = strParameter.ToString();

                Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataStore = new Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>>();

                SiteDal sdm = new SiteDal();

                FactorDataManager fdm = new FactorDataManager();
                if (null == parameters || parameters.Count == 0)
                    parameters = fdm.QueryParameter();

                if (null == sites || sites.Count == 0)
                    sites = new MMShareBLL.DAL.SiteDal().Query();

                foreach (int key in sites.Keys)
                {
                    paramSites.Value = sites[key].Id.ToString();
                    using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_GETDSMHOURDATA2,
                            paramParameter, paramSites, paramDuration, paramLST2, paramLST1))
                    {
                        while (reader.Read())
                        {
                            MMShareBLL.Model.SiteData qData = null;
                            Dictionary<int, MMShareBLL.Model.SiteData> timeData = null;
                            DateTime lst = Convert.ToDateTime(reader["LST"]);
                            if (dataStore.ContainsKey(lst))
                                timeData = dataStore[lst];
                            else
                            {
                                timeData = new Dictionary<int, MMShareBLL.Model.SiteData>();
                                dataStore.Add(lst, timeData);
                            }

                            int siteID = Convert.ToInt32(reader["SiteID"]);

                            if (timeData.ContainsKey(siteID))
                                qData = timeData[siteID];
                            else
                            {
                                qData = new MMShareBLL.Model.SiteData();
                                qData.Date = new PeriodOfTime();
                                qData.Date.Value = new DateTime(lst.Year, lst.Month, lst.Day, lst.Hour, 0, 0);
                                qData.Site = sites[siteID];
                                timeData[siteID] = qData;
                            }
                            int parameterID = Convert.ToInt32(reader["ParameterID"]);

                            MonitoringData factorData = new MonitoringData();

                            if (null != parameters && parameters.ContainsKey(parameterID))
                                factorData.Factor = parameters[parameterID];
                            else
                            {
                                factorData.Factor = fdm.QueryParameter(parameterID);
                                if (null != factorData.Factor)
                                    parameters[parameterID] = factorData.Factor;
                            }

                            factorData.Thickness = float.Parse(reader["Value"].ToString());
                            qData.DataCollection.Add(factorData);
                        }
                    }
                }
                return dataStore;
            }
            catch { throw; }

        }
        public DateTime? GetLastDataHour(int duration)
        {
            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
            paramDuration.Value = duration;
            object theDate = m_DatabaseS.ExecuteScalar(m_DatabaseS.ConnectionString, CommandType.StoredProcedure, PROC_GETLASTDATAHOUR, paramDuration);

            if (null == theDate)
                return null;

            return DateTime.Parse(theDate.ToString());
        }

        ~MonitoringDataDal()
        {
            closeConn();
        }

        private void closeConn()
        {
            if (UseOneConnection && null != conn && conn.State != System.Data.ConnectionState.Closed)
            {
                try
                {
                    conn.Close();
                    conn.Dispose();
                }
                catch { }
            }
        }

        public DataTable GetData(DateTime start, DateTime end, int parameterId, int siteId, int duration, DataValidated validated)
        {
            SqlParameter paramStart = new SqlParameter(PARAM_LST1, SqlDbType.DateTime);
            SqlParameter paramEnd = new SqlParameter(PARAM_LST2, SqlDbType.DateTime);
            SqlParameter paramParameterId = new SqlParameter(PARAM_PARAMETER, SqlDbType.Int);
            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
            SqlParameter paramSiteId = new SqlParameter(PARAM_SITEID, SqlDbType.Int);
            SqlParameter paramValidated = new SqlParameter(PARAM_VALIDATED, System.Data.SqlDbType.Int);

            paramStart.Value = start;
            paramEnd.Value = end;
            paramParameterId.Value = parameterId;
            paramDuration.Value = duration;
            paramSiteId.Value = siteId;
            paramValidated.Value = (int)validated;

            SqlConnection conn = new SqlConnection(m_DatabaseS.ConnectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(DAILY_DMSDATA_QUERY2, conn);
            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            adapter.SelectCommand.Parameters.Add(paramStart);
            adapter.SelectCommand.Parameters.Add(paramEnd);
            adapter.SelectCommand.Parameters.Add(paramParameterId);
            adapter.SelectCommand.Parameters.Add(paramDuration);
            adapter.SelectCommand.Parameters.Add(paramSiteId);
            adapter.SelectCommand.Parameters.Add(paramValidated);
            DataTable data = new DataTable();
            try
            {
                adapter.Fill(data);
                return data;
            }
            catch { throw; }
        }

        public DataTable GetData(DateTime start, DateTime end, int parameterId, int duration, DataValidated validated)
        {
            SqlParameter paramStart = new SqlParameter(PARAM_LST1, SqlDbType.DateTime);
            SqlParameter paramEnd = new SqlParameter(PARAM_LST2, SqlDbType.DateTime);
            SqlParameter paramParameterId = new SqlParameter(PARAM_PARAMETER, SqlDbType.Int);
            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
            SqlParameter paramValidated = new SqlParameter(PARAM_VALIDATED, System.Data.SqlDbType.Int);

            paramStart.Value = start;
            paramEnd.Value = end;
            paramParameterId.Value = parameterId;
            paramDuration.Value = duration;
            paramValidated.Value = (int)validated;

            SqlConnection conn = new SqlConnection(m_DatabaseS.ConnectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(DAILY_DMSDATA_QUERY, conn);
            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            adapter.SelectCommand.Parameters.Add(paramStart);
            adapter.SelectCommand.Parameters.Add(paramEnd);
            adapter.SelectCommand.Parameters.Add(paramParameterId);
            adapter.SelectCommand.Parameters.Add(paramDuration);
            adapter.SelectCommand.Parameters.Add(paramValidated);
            DataTable data = new DataTable();
            try
            {
                adapter.Fill(data);
                return data;
            }
            catch { throw; }
        }

        public DataTable GetData(DateTime start, DateTime end, int[] parameters, int[] sites, int duration, DataValidated validated)
        {
            if (parameters.Length == 0 || sites.Length == 0)
                return new DataTable();
            SqlParameter paramStart = new SqlParameter(PARAM_LST1, SqlDbType.DateTime);
            SqlParameter paramEnd = new SqlParameter(PARAM_LST2, SqlDbType.DateTime);
            SqlParameter paramParameterId = new SqlParameter(PARAM_PARAMETER, SqlDbType.VarChar);
            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
            SqlParameter paramSites = new SqlParameter(PARAM_SITES, SqlDbType.VarChar);
            SqlParameter paramValidated = new SqlParameter(PARAM_VALIDATED, System.Data.SqlDbType.Int);

            paramStart.Value = start;
            paramEnd.Value = end;
            paramDuration.Value = duration;
            paramValidated.Value = (int)validated;

            StringBuilder sbParameters = new StringBuilder();
            foreach (int param in parameters)
            {
                sbParameters.Append(param);
                sbParameters.Append(",");
            }
            string strParameters = sbParameters.ToString();
            strParameters = strParameters.Substring(0, strParameters.Length - 1);
            paramParameterId.Value = strParameters;

            StringBuilder sbSites = new StringBuilder();
            foreach (int site in sites)
            {
                sbSites.Append(site);
                sbSites.Append(",");
            }
            string strSites = sbSites.ToString();
            strSites = strSites.Substring(0, strSites.Length - 1);
            paramSites.Value = strSites;

            SqlConnection conn = new SqlConnection(m_DatabaseS.ConnectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(DAILY_DMSDATA_QUERY3, conn);
            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            adapter.SelectCommand.Parameters.Add(paramStart);
            adapter.SelectCommand.Parameters.Add(paramEnd);
            adapter.SelectCommand.Parameters.Add(paramParameterId);
            adapter.SelectCommand.Parameters.Add(paramDuration);
            adapter.SelectCommand.Parameters.Add(paramSites);
            adapter.SelectCommand.Parameters.Add(paramValidated);
            DataTable data = new DataTable();
            try
            {
                adapter.Fill(data);
                return data;
            }
            catch { throw; }
        }

        public DataTable GetAQIData(DateTime start, DateTime end, string sites, string parameters, int duration, DataValidated validated)
        {
            if (parameters.Length == 0 || sites.Length == 0)
                return new DataTable();
            SqlParameter paramStart = new SqlParameter(PARAM_LST1, SqlDbType.DateTime);
            SqlParameter paramEnd = new SqlParameter(PARAM_LST2, SqlDbType.DateTime);
            SqlParameter paramParameterId = new SqlParameter(PARAM_PARAMETER, SqlDbType.VarChar);
            SqlParameter paramDuration = new SqlParameter(PARAM_DURATION, SqlDbType.Int);
            SqlParameter paramSites = new SqlParameter(PARAM_SITES, SqlDbType.VarChar);
            SqlParameter paramValidated = new SqlParameter(PARAM_VALIDATED, System.Data.SqlDbType.Int);

            paramStart.Value = start;
            paramEnd.Value = end;
            paramDuration.Value = duration;
            paramValidated.Value = (int)validated;
            paramParameterId.Value = parameters;
            paramSites.Value = sites;

            SqlConnection conn = new SqlConnection(m_DatabaseS.ConnectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(PROC_GETAQIDATA, conn);
            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            adapter.SelectCommand.Parameters.Add(paramStart);
            adapter.SelectCommand.Parameters.Add(paramEnd);
            adapter.SelectCommand.Parameters.Add(paramParameterId);
            adapter.SelectCommand.Parameters.Add(paramDuration);
            adapter.SelectCommand.Parameters.Add(paramSites);
            adapter.SelectCommand.Parameters.Add(paramValidated);
            DataTable data = new DataTable();
            try
            {
                adapter.Fill(data);
                return data;
            }
            catch { throw; }
        }
    }

}
