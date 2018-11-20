using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using MMShareBLL.Model;
using Readearth.Data;

namespace MMShareBLL.DAL
{
    /// <summary>
    /// 分段预报数据操作
    /// </summary>
    public class CityPeriodDailyForecastAccess
    {
        CityPeriodDailyForecast cpdf = new CityPeriodDailyForecast();
        public CityPeriodDailyForecastAccess()
        {
            m_DatabaseS = new Database("SEMCDMC");
        }
        private Database m_DatabaseS;
        //Procedures
        private const string PROC_CITYPERIODDAILYFORECAST_INSERT = "proc_CityPeriodDailyForecast_Insert";
        private const string PROC_CITYPERIODDAILYFORECAST_UPDATE = "proc_CityPeriodDailyForecast_Update";
        private const string PROC_CITYPERIODDAILYFORECAST_SELECTBYDATE = "proc_CityPeriodDailyForecast_SelectByDate";

        //Parameters
        private const string PARAM_ID = "@ID";
        private const string PARAM_FORECASTDATE = "@ForecastDate";
        private const string PARAM_AFTERNOONAPI = "@AfternoonAPI";
        private const string PARAM_AFTERNOONAPIRANGE = "@AfternoonAPIRange";
        private const string PARAM_AFTERNOONGRADERANGE = "@AfternoonGradeRange";
        private const string PARAM_TONIGHTAPI = "@TonightAPI";
        private const string PARAM_TONIGHTAPIRANGE = "@TonightAPIRange";
        private const string PARAM_TONIGHTGRADERANGE = "@TonightGradeRange";
        private const string PARAM_TOMORROWAMAPI = "@TomorrowAMAPI";
        private const string PARAM_TOMORROWAMAPIRANGE = "@TomorrowAMAPIRange";
        private const string PARAM_TOMORROWAMGRADERANGE = "@TomorrowAMGradeRange";
        private const string PARAM_TOMORROWPMAPI = "@TomorrowPMAPI";
        private const string PARAM_TOMORROWPMAPIRANGE = "@TomorrowPMAPIRange";
        private const string PARAM_TOMORROWPMGRADERANGE = "@TomorrowPMGradeRange";
        private const string PARAM_TOMORROWAMAPIMODIFY = "@TomorrowAMAPIModify";
        private const string PARAM_TOMORROWAMAPIMODIFYRANGE = "@TomorrowAMAPIModifyRange";
        private const string PARAM_TOMORROWAMGRADEMODIFYRANGE = "@TomorrowAMGradeModifyRange";
        private const string PARAM_TOMORROWPMAPIMODIFY = "@TomorrowPMAPIModify";
        private const string PARAM_TOMORROWPMAPIMODIFYRANGE = "@TomorrowPMAPIModifyRange";
        private const string PARAM_TOMORROWPMGRADEMODIFYRANGE = "@TomorrowPMGradeModifyRange";
        private const string PARAM_LASTUPDATEPERSON = "@LastUpdatePerson";
        private const string PARAM_LASTUPDATETIME = "@LastUpdateTime";
        private const string PARAM_RESULTFORECAST = "@ResultForecast";
        private const string PARAM_RESULTFACT = "@ResultFact";

        private const string PARAM_FORECASTDATE1 = "@ForecastDate1";
        private const string PARAM_FORECASTDATE2 = "@ForecastDate2";
        /// <summary>
        /// 写入分段预报数据
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Insert(CityPeriodDailyForecast item)
        {
            try
            {
                SqlParameter[] parameters = getInsertUpdateParameters(item);
                int rowsAffected = 0;
                rowsAffected = m_DatabaseS.Execute( PROC_CITYPERIODDAILYFORECAST_INSERT, parameters);
                return rowsAffected == 1;
            }
            catch { throw; }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Update(CityPeriodDailyForecast item)
        {
            try
            {
                SqlParameter[] dp = getInsertUpdateParameters(item);
                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter paramResultId = new SqlParameter(PARAM_ID, SqlDbType.Int);
                paramResultId.Value = item.ID;
                parameters.AddRange(dp);
                parameters.Add(paramResultId);
                parameters.RemoveAt(0);
                int rowsAffected = m_DatabaseS.Execute( PROC_CITYPERIODDAILYFORECAST_UPDATE, parameters.ToArray());

                return rowsAffected == 1;
            }
            catch { throw; }
        }
        /// <summary>
        /// 根据预报时间查询分段预报
        /// </summary>
        /// <param name="ForecastDate1"></param>
        /// <param name="ForecastDate2"></param>
        /// <returns></returns>
        public Dictionary<DateTime, CityPeriodDailyForecast> QueryByDate(DateTime ForecastDate1, DateTime ForecastDate2)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter d1 = new SqlParameter(PARAM_FORECASTDATE1, SqlDbType.DateTime);
            SqlParameter d2 = new SqlParameter(PARAM_FORECASTDATE2, SqlDbType.DateTime);
            d1.Value = ForecastDate1;
            d2.Value = ForecastDate2;
            parameters.Add(d1);
            parameters.Add(d2);
            try
            {
                Dictionary<DateTime, CityPeriodDailyForecast> data = new Dictionary<DateTime, CityPeriodDailyForecast>();
                using (SqlDataReader reader = m_DatabaseS.GetDataReader(PROC_CITYPERIODDAILYFORECAST_SELECTBYDATE, parameters.ToArray()))
                {
                    while (reader.Read())
                    {
                        DateTime t = reader.GetDateTime(1);
                        if (data.ContainsKey(t))
                            continue;
                        CityPeriodDailyForecast cpdf = getCityPeriodDailyForecastReader(reader);
                        data.Add(t, cpdf);
                    }
                }
                return data;
            }
            catch { throw; }
        }

        private CityPeriodDailyForecast getCityPeriodDailyForecastReader(SqlDataReader reader)
        {
            CityPeriodDailyForecast cpdf = new CityPeriodDailyForecast();
            cpdf.ID = reader.GetInt32(0);
            cpdf.ForecastDate = reader.GetDateTime(1);

            cpdf.AfternoonAPI = reader.GetInt32(2);
            if (DBNull.Value != reader[3])
                cpdf.AfternoonAPIRange = reader.GetString(3);
            if (DBNull.Value != reader[4])
                cpdf.AfternoonGradeRange = reader.GetString(4);
            
            cpdf.TonightAPI = reader.GetInt32(5);
            if (DBNull.Value != reader[6])
                cpdf.TonightAPIRange = reader.GetString(6);
            if (DBNull.Value != reader[7])
                cpdf.TonightGradeRange = reader.GetString(7);
            
            cpdf.TomorrowAMAPI = reader.GetInt32(8);
            if (DBNull.Value != reader[9])
                cpdf.TomorrowAMAPIRange = reader.GetString(9);
            if (DBNull.Value != reader[10])
                cpdf.TomorrowAMGradeRange = reader.GetString(10);
            
            cpdf.TomorrowPMAPI = reader.GetInt32(11);
            if (DBNull.Value != reader[12])
                cpdf.TomorrowPMAPIRange = reader.GetString(12);
            if (DBNull.Value != reader[13])
                cpdf.TomorrowPMGradeRange = reader.GetString(13);
            
            cpdf.TomorrowAMAPIModify = reader.GetInt32(14);
            if (DBNull.Value != reader[15])
                cpdf.TomorrowAMAPIModifyRange = reader.GetString(15);
            if (DBNull.Value != reader[16])
                cpdf.TomorrowAMGradeModifyRange = reader.GetString(16);
            
            cpdf.TomorrowPMAPIModify = reader.GetInt32(17);
            if (DBNull.Value != reader[18])
                cpdf.TomorrowPMAPIModifyRange = reader.GetString(18);
            if (DBNull.Value != reader[19])
                cpdf.TomorrowPMGradeModifyRange = reader.GetString(19);
            
            if (DBNull.Value != reader[20])
                cpdf.LastUpdatePerson = reader.GetString(20);
            cpdf.LastUpdateTime = reader.GetDateTime(21);
            if (DBNull.Value != reader[22])
                cpdf.ResultForecast = reader.GetString(22);
            if (DBNull.Value != reader[23])
                cpdf.ResultFact = reader.GetString(23);
            return cpdf;
        }

        private SqlParameter[] getInsertUpdateParameters(CityPeriodDailyForecast item)
        {
            SqlParameter[] parameters = {
					new SqlParameter(PARAM_FORECASTDATE, SqlDbType.DateTime),
					new SqlParameter(PARAM_AFTERNOONAPI, SqlDbType.Int),
					new SqlParameter(PARAM_AFTERNOONAPIRANGE, SqlDbType.VarChar,50),
					new SqlParameter(PARAM_AFTERNOONGRADERANGE, SqlDbType.VarChar,50),
					new SqlParameter(PARAM_TONIGHTAPI, SqlDbType.Int),
					new SqlParameter(PARAM_TONIGHTAPIRANGE, SqlDbType.VarChar,50),
					new SqlParameter(PARAM_TONIGHTGRADERANGE, SqlDbType.VarChar,50),
					new SqlParameter(PARAM_TOMORROWAMAPI, SqlDbType.Int),
					new SqlParameter(PARAM_TOMORROWAMAPIRANGE, SqlDbType.VarChar,50),
					new SqlParameter(PARAM_TOMORROWAMGRADERANGE, SqlDbType.VarChar,50),
					new SqlParameter(PARAM_TOMORROWPMAPI, SqlDbType.Int),
					new SqlParameter(PARAM_TOMORROWPMAPIRANGE, SqlDbType.VarChar,50),
					new SqlParameter(PARAM_TOMORROWPMGRADERANGE, SqlDbType.VarChar,50),
					new SqlParameter(PARAM_TOMORROWAMAPIMODIFY, SqlDbType.Int),
					new SqlParameter(PARAM_TOMORROWAMAPIMODIFYRANGE, SqlDbType.VarChar,50),
					new SqlParameter(PARAM_TOMORROWAMGRADEMODIFYRANGE, SqlDbType.VarChar,50),
					new SqlParameter(PARAM_TOMORROWPMAPIMODIFY, SqlDbType.Int),
					new SqlParameter(PARAM_TOMORROWPMAPIMODIFYRANGE, SqlDbType.VarChar,50),
					new SqlParameter(PARAM_TOMORROWPMGRADEMODIFYRANGE, SqlDbType.VarChar,50),
					new SqlParameter(PARAM_LASTUPDATEPERSON, SqlDbType.VarChar,30),
                    new SqlParameter(PARAM_LASTUPDATETIME, SqlDbType.DateTime),
					new SqlParameter(PARAM_RESULTFORECAST, SqlDbType.VarChar,int.MaxValue),
					new SqlParameter(PARAM_RESULTFACT, SqlDbType.VarChar,int.MaxValue)
                };

            if (null != item.ForecastDate.ToString())
                parameters[0].Value = item.ForecastDate;
            else
                parameters[0].Value = DBNull.Value;
            
            if (null != item.AfternoonAPI.ToString())
                parameters[1].Value = item.AfternoonAPI;
            else
                parameters[1].Value = DBNull.Value;
            if (!String.IsNullOrEmpty(item.AfternoonAPIRange))
                parameters[2].Value = item.AfternoonAPIRange;
            else
                parameters[2].Value = DBNull.Value;
            if (!String.IsNullOrEmpty(item.AfternoonGradeRange))
                parameters[3].Value = item.AfternoonGradeRange;
            else
                parameters[3].Value = DBNull.Value;
            
            if (null != item.TonightAPI.ToString())
                parameters[4].Value = item.TonightAPI;
            else
                parameters[4].Value = DBNull.Value;
            if (!String.IsNullOrEmpty(item.TonightAPIRange))
                parameters[5].Value = item.TonightAPIRange;
            else
                parameters[5].Value = DBNull.Value;
            if (!String.IsNullOrEmpty(item.TonightGradeRange))
                parameters[6].Value = item.TonightGradeRange;
            else
                parameters[6].Value = DBNull.Value;
            
            if (null != item.TomorrowAMAPI.ToString())
                parameters[7].Value = item.TomorrowAMAPI;
            else
                parameters[7].Value = DBNull.Value;
            if (!String.IsNullOrEmpty(item.TomorrowAMAPIRange))
                parameters[8].Value = item.TomorrowAMAPIRange;
            else
                parameters[8].Value = DBNull.Value;
            if (!String.IsNullOrEmpty(item.TomorrowAMGradeRange))
                parameters[9].Value = item.TomorrowAMGradeRange;
            else
                parameters[9].Value = DBNull.Value;
            
            if (null != item.TomorrowPMAPI.ToString())
                parameters[10].Value = item.TomorrowPMAPI;
            else
                parameters[10].Value = DBNull.Value;
            if (!String.IsNullOrEmpty(item.TomorrowPMAPIRange))
                parameters[11].Value = item.TomorrowPMAPIRange;
            else
                parameters[11].Value = DBNull.Value;
            if (!String.IsNullOrEmpty(item.TomorrowPMGradeRange))
                parameters[12].Value = item.TomorrowPMGradeRange;
            else
                parameters[12].Value = DBNull.Value;
            
            if (null != item.TomorrowAMAPIModify.ToString())
                parameters[13].Value = item.TomorrowAMAPIModify;
            else
                parameters[13].Value = DBNull.Value;
            if (!String.IsNullOrEmpty(item.TomorrowAMAPIModifyRange))
                parameters[14].Value = item.TomorrowAMAPIModifyRange;
            else
                parameters[14].Value = DBNull.Value;
            if (!String.IsNullOrEmpty(item.TomorrowAMGradeModifyRange))
                parameters[15].Value = item.TomorrowAMGradeModifyRange;
            else
                parameters[15].Value = DBNull.Value;
            
            if (null != item.TomorrowPMAPIModify.ToString())
                parameters[16].Value = item.TomorrowPMAPIModify;
            else
                parameters[16].Value = DBNull.Value;
            if (!String.IsNullOrEmpty(item.TomorrowPMAPIModifyRange))
                parameters[17].Value = item.TomorrowPMAPIModifyRange;
            else
                parameters[17].Value = DBNull.Value;
            if (!String.IsNullOrEmpty(item.TomorrowPMGradeModifyRange))
                parameters[18].Value = item.TomorrowPMGradeModifyRange;
            else
                parameters[18].Value = DBNull.Value;
            
            if (null != item.LastUpdatePerson)
                parameters[19].Value = item.LastUpdatePerson;
            else
                parameters[19].Value = DBNull.Value;

            if (null != item.LastUpdateTime)
                parameters[20].Value = item.LastUpdateTime;
            else
                parameters[20].Value = DBNull.Value;

            if (null != item.ResultForecast)
                parameters[21].Value = item.ResultForecast;
            else
                parameters[21].Value = DBNull.Value;

            if (null != item.ResultFact)
                parameters[22].Value = item.ResultFact;
            else
                parameters[22].Value = DBNull.Value;
            return parameters;
        }

    }
}
