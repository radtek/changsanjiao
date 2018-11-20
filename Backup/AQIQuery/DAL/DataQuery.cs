using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
//using eshop.DAL;
using System.Data;

namespace AQIQuery.DAL
{
    class DataQuery
    {
        /// <summary>
        /// 查询站点的小时AQI以及浓度、等级等数据，可查的内容包括AQI、IAQI以及相对于浓度。说明：该函数较为通用，需要了解AQIItemID所代表的内容
        /// </summary>
        /// <param name="LST1">起始时间,统计时间</param>
        /// <param name="LST2">结束时间,统计时间</param>
        /// <param name="SiteIDs">站点编号组，可选多个站点，用","分隔</param>
        /// <param name="AQIItemIDs">AQIItem编号组，可选多个站点，用","分隔，可选的AQIItemID包括100至210，共22个，详细可参考AQIItem表</param>
        /// <returns>返回DataTable，字段包括</returns>
        /// 用到的存储过程Data_RT_Site_Get_bySiteIDandAQIItems
        public static DataTable Data_RT_Site_Get_bySiteIDandAQIItems(DateTime LST1, DateTime LST2, string SiteIDs, string AQIItemIDs)
        {
            SqlParameter[] para = {
                                     new SqlParameter("@LST1", LST1),
                                     new SqlParameter("@LST2", LST2),
                                     new SqlParameter("@SiteIDs", SiteIDs),
                                     new SqlParameter("@AQIItemIDs", AQIItemIDs)
                                  };
            DataTable result = null;
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "Data_RT_Site_Get_bySiteIDandAQIItems", para);
                DataTable dt = ds.Tables[0];
                result = dt;
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("Data_RT_Site_Get_bySiteIDandAQIItems:"+str);
                //throw;
            }

            return result;
        }

        /// <summary>
        /// 查询分组的小时AQI以及浓度、等级等数据，可查的内容包括AQI、IAQI以及相对于浓度。说明：该函数较为通用，需要了解AQIItemID所代表的内容
        /// </summary>
        /// <param name="LST1">起始时间,统计时间</param>
        /// <param name="LST2">结束时间,统计时间</param>
        /// <param name="GroupIDs">分组编号组，可选多个站点，用","分隔</param>
        /// <param name="AQIItemIDs">AQIItem编号组，可选多个站点，用","分隔，可选的AQIItemID包括100至210，共22个，详细可参考AQIItem表</param>
        /// <returns>返回DataTable，字段包括</returns>
        /// 用到的存储过程Data_RT_Group_Get_byGroupIDandAQIItems
        public static DataTable Data_RT_Group_Get_byGroupIDandAQIItems(DateTime LST1, DateTime LST2, string GroupIDs, string AQIItemIDs)
        {

            SqlParameter[] para = {
                                     new SqlParameter("@LST1", LST1),
                                     new SqlParameter("@LST2", LST2),
                                     new SqlParameter("@GroupIDs", GroupIDs),
                                     new SqlParameter("@AQIItemIDs", AQIItemIDs),
                                  };
            //SqlParameter[] para = null;
            DataTable result = null;
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "Data_RT_Group_Get_byGroupIDandAQIItems", para);
                DataTable dt = ds.Tables[0];
                result = dt;
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("Data_RT_Group_Get_byGroupIDandAQIItems:" + str);
                //throw;
            }

            return result;
        }




        /// <summary>
        /// 查询站点日报以上统计时间单元的AQI以及浓度、等级等数据，可查的内容包括AQI、IAQI以及相对于浓度。说明：该函数较为通用，需要了解AQIItemID所代表的内容
        /// </summary>
        /// <param name="LST1">起始时间,统计时间</param>
        /// <param name="LST2">结束时间,统计时间</param>
        /// <param name="SiteIDs">站点编号组，可选多个站点，用","分隔</param>
        /// <param name="AQIItemIDs">AQIItem编号组，可选多个站点，用","分隔，可选的AQIItemID包括300以上的，详细可参考AQIItem表</param>
        /// <returns>返回DataTable，字段包括</returns>
        /// 用到的存储过程Data_Report_Site_Get_bySiteIDandAQIItems
        public static DataTable Data_Report_Site_Get_bySiteIDandAQIItems(DateTime LST1, DateTime LST2, string SiteIDs, string AQIItemIDs, bool isMore)
        {
            SqlParameter[] para = {
                                     new SqlParameter("@LST1", LST1),
                                     new SqlParameter("@LST2", LST2),
                                     new SqlParameter("@SiteIDs", SiteIDs),
                                     new SqlParameter("@AQIItemIDs", AQIItemIDs),
                                     new SqlParameter("@isMore", isMore),
                                     
                                  };
            DataTable result = null;
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "Data_Report_Site_Get_bySiteIDandAQIItems", para);
                DataTable dt = ds.Tables[0];
                result = dt;
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("Data_Report_Site_Get_bySiteIDandAQIItems:" + str);
                //throw;
            }

            return result;
        }
        /// <summary>
        /// 查询分组日统计以上的AQI以及浓度、等级等数据，可查的内容包括AQI、IAQI以及相对于浓度。说明：该函数较为通用，需要了解AQIItemID所代表的内容
        /// </summary>
        /// <param name="LST1">起始时间,统计时间</param>
        /// <param name="LST2">结束时间,统计时间</param>
        /// <param name="GroupIDs">分组编号组，可选多个站点，用","分隔</param>
        /// <param name="AQIItemIDs">AQIItem编号组，可选多个站点，用","分隔，可选的AQIItemID包括300以上的所有ID，共22个，详细可参考AQIItem表</param>
        /// <returns>返回DataTable，字段包括</returns>
        /// 用到的存储过程Data_Report_Group_Get_byGroupIDandAQIItems
        public static DataTable Data_Report_Group_Get_byGroupIDandAQIItems(DateTime LST1, DateTime LST2, string GroupIDs, string AQIItemIDs, bool isMore)
        {

            SqlParameter[] para = {
                                     new SqlParameter("@LST1", LST1),
                                     new SqlParameter("@LST2", LST2),
                                     new SqlParameter("@GroupIDs", GroupIDs),
                                     new SqlParameter("@AQIItemIDs", AQIItemIDs),
                                     new SqlParameter("@isMore", isMore),
                                  };
            //SqlParameter[] para = null;
            DataTable result = null;
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "Data_Report_Group_Get_byGroupIDandAQIItems", para);
                DataTable dt = ds.Tables[0];
                result = dt;
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("Data_Report_Group_Get_byGroupIDandAQIItems:" + str);
                //throw;
            }

            return result;
        }


    
    }
}
