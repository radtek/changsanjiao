using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
//using eshop.DAL;
using System.Data.SqlClient;

namespace AQIQuery.DAL
{
    class AQIItem
    {
        //

        /// <summary>
        /// 获取需要Process的AQIItems
        /// 用到了存储过程：AQIItem_GetProcess
        /// </summary>
        /// <returns>返回string ，每个站点之间用","分隔开</returns>
        /// 日期：2014-3-15日
        public static string GetAQIItemProcessString(string TimeType)
        {
            // SqlParameter[] para = null;
            string result = string.Empty;
            try
            {
                DataTable dt = AQIItem.GetAQIItemProcess(TimeType);
                if (dt.Equals(null)) return result;
                string AQIItems = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        AQIItems = dt.Rows[i][0].ToString();
                    }
                    else
                    {
                        AQIItems = AQIItems + string.Format(",{0}", dt.Rows[i][0].ToString());
                    }
                }
                result = AQIItems;

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
        /// 获取需要Process的AQIItems
        /// 用到了存储过程：AQIItem_GetProcess
        /// </summary>
        /// <returns>返回DataTable</returns>
        /// 日期：2014-3-15日
        public static DataTable GetAQIItemProcess(string TimeType)
        {
            SqlParameter[] para = {
                                     new SqlParameter("@TimeType", TimeType)
                                  };
            //SqlParameter[] para = null;
            DataTable result = null;
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "AQIItem_GetProcess", para);
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
        /// 获取需要Process的AQIItems
        /// 用到了存储过程：AQIItem_GetProcess
        /// </summary>
        /// <returns>返回string ，每个站点之间用","分隔开</returns>
        /// 日期：2014-3-15日
        public static string GetAQIItemRTAQIString(bool isAQI, bool isIAQI, bool is24hrAQI)
        {
            // SqlParameter[] para = null;
            string result = string.Empty;
            try
            {
                DataTable dt = AQIItem.GetAQIItemRTAQI(isAQI,isIAQI,is24hrAQI);
                if (dt.Equals(null)) return result;
                string AQIItems = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        AQIItems = dt.Rows[i][0].ToString();
                    }
                    else
                    {
                        AQIItems = AQIItems + string.Format(",{0}", dt.Rows[i][0].ToString());
                    }
                }
                result = AQIItems;

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
        /// 获取需要Process的AQIItems
        /// 用到了存储过程：AQIItem_GetRTAQI
        /// </summary>
        /// <returns>返回DataTable</returns>
        /// 日期：2014-3-15日
        public static DataTable GetAQIItemRTAQI(bool isAQI,bool isIAQI,bool is24hrAQI)
        {
            SqlParameter[] para = {
                                     new SqlParameter("@isAQI", isAQI),
                                     new SqlParameter("@isIAQI", isIAQI),
                                     new SqlParameter("@is24hrAQI", is24hrAQI)
                                  };
            DataTable result = null;
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "AQIItem_GetRTAQI", para);
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
        /// 获取需要日报AQI的AQIItems
        /// </summary>
        /// <returns>返回string ，每个站点之间用","分隔开</returns>
        /// 日期：2014-3-19日
        public static string GetAQIItemDailyAQIString(bool isAQI, bool isIAQI)
        {
            // SqlParameter[] para = null;
            string result = string.Empty;
            try
            {
                DataTable dt = AQIItem.GetAQIItemDailyAQI(isAQI, isIAQI);
                if (dt.Equals(null)) return result;
                string AQIItems = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        AQIItems = dt.Rows[i][0].ToString();
                    }
                    else
                    {
                        AQIItems = AQIItems + string.Format(",{0}", dt.Rows[i][0].ToString());
                    }
                }
                result = AQIItems;

            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("GetAQIItemDailyAQIString:"+str);
                //throw;
            }

            return result;
        }

        /// <summary>
        /// 获取需要日报AQI的AQIItems
        /// </summary>
        /// <returns>返回DataTable</returns>
        /// 日期：2014-3-19日
        /// 用到了存储过程：AQIItem_GetDayAQI
        public static DataTable GetAQIItemDailyAQI(bool isAQI, bool isIAQI)
        {
            SqlParameter[] para = {
                                     new SqlParameter("@isAQI", isAQI),
                                     new SqlParameter("@isIAQI", isIAQI)
                                  };
            DataTable result = null;
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "AQIItem_GetDayAQI", para);
                DataTable dt = ds.Tables[0];
                result = dt;
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("AQIItem_GetDayAQI:"+str);
                //throw;
            }

            return result;
        }

        /// <summary>
        /// 根据ParameterIDs获取AQIItems
        /// </summary>
        /// <returns>返回DataTable</returns>
        ///日期：2014-3-17日
        /// 用到了存储过程：AQIItem_Get_byParameterIDs
        public static DataTable GetAQIItemByParameterIDs(string ParameterIDs, string TimeType)
        {
            SqlParameter[] para = {
                                     new SqlParameter("@ParameterIDs", ParameterIDs),
                                     new SqlParameter("@TimeType", TimeType)
                                  };
            DataTable result = null;
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "AQIItem_Get_byParameterIDs", para);
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
        /// 根据ParameterIDs获取AQIItems
        /// </summary>
        /// <returns>返回string ，每个站点之间用","分隔开</returns>
        /// 日期：2014-3-17日
        /// 用到了存储过程：AQIItem_Get_byParameterIDs
        public static string GetAQIItemByParameterIDsString(string ParameterIDs, string TimeType)
        {
            // SqlParameter[] para = null;
            string result = string.Empty;
            try
            {
                DataTable dt = AQIItem.GetAQIItemByParameterIDs(ParameterIDs, TimeType);
                if (dt.Equals(null)) return result;
                string AQIItems = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        AQIItems = dt.Rows[i][0].ToString();
                    }
                    else
                    {
                        AQIItems = AQIItems + string.Format(",{0}", dt.Rows[i][0].ToString());
                    }
                }
                result = AQIItems;

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
