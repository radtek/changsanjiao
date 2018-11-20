using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
//using eshop.DAL;

namespace AQIQuery.DAL
{
    class DatafromDMS
    {
        /// <summary>
        /// 从DMS中直接获取小时数据。
        /// </summary>
        /// <param name="LST1">起始时间</param>
        /// <param name="LST1">结束时间</param>
        /// <param name="SiteID">站点编号</param>
        /// <param name="ParameterID">参数ID号，详见表Parameter</param>
        /// <returns>返回Datatable</returns>
        /// 日期：2014-2-19日于太平洋上空，旧金山时间，东京时间为：2014-2-20 11:39
        /// 用到了存储过程：SiteGroup_GetGroupIDbyName
        public static DataTable GetHourlyDatabyLSP(DateTime LST1, DateTime LST2, int SiteID, int ParameterID)
        {
            SqlParameter[] para = {
                                     new SqlParameter("@LST1", LST1),
                                     new SqlParameter("@LST2", LST2),
                                     new SqlParameter("@SiteID", SiteID),
                                     new SqlParameter("@ParameterID", ParameterID)
                                  };

            DataTable result = null;
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "DMSData_GetHourlyDatabyLSP", para);
                DataTable dt = ds.Tables[0];
                result = dt;

            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine(str);
                //throw;
            }

            return result;
        }

        /// <summary>
        /// 从DMS中直接获取日报数据。
        /// </summary>
        /// <param name="LST1">起始时间</param>
        /// <param name="LST1">结束时间</param>
        /// <param name="SiteID">站点编号</param>
        /// <param name="ParameterID">参数ID号，详见表Parameter</param>
        /// <returns>返回Datatable</returns>
        /// 日期：2014-2-20日于日本海上空，东京时间为：2014-2-20 20:28
        /// 用到了存储过程：DMSData_GetDailyDatabyLSP
        public static DataTable GetDailyDatabyLSP(DateTime LST1, DateTime LST2, int SiteID, int ParameterID)
        {
            SqlParameter[] para = {
                                     new SqlParameter("@LST1", LST1),
                                     new SqlParameter("@LST2", LST2),
                                     new SqlParameter("@SiteID", SiteID),
                                     new SqlParameter("@ParameterID", ParameterID)
                                  };

            DataTable result = null;
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "DMSData_GetDailyDatabyLSP", para);
                DataTable dt = ds.Tables[0];
                DataRow[] dws = dt.Select("ValidNum>=20");
               
                //result.d
                result = dt;
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine(str);
                //throw;
            }

            return result;
        }


    }
}
