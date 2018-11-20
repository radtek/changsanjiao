using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace AQIQuery.DAL
{
    class Phone
    {

        /// <summary>
        /// Android设备
        /// </summary>
        /// <param name="DeviceID">DeviceID</param>
        /// <param name="PhoneNumber">电话号码</param>
        /// <param name="SoftVersion">软件版本</param>
        /// <returns>返回1代表成功，0代表失败</returns>
        /// 日期：2014-4-27日
        public static int PhoneAndroid_AddNew(string DeviceID, string PhoneNumber,string OSVersion, double SoftVersion)
        {
            SqlParameter[] para = {
                                      new SqlParameter("@DeviceID", DeviceID),
                                      new SqlParameter("@PhoneNumber", PhoneNumber),
                                      new SqlParameter("@OSVersion", OSVersion),
                                      new SqlParameter("@SoftVersion", SoftVersion)
                                  };
            int result = 0;
            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.connectionStringAQI, CommandType.StoredProcedure,
                                                        "[PhoneAndroid_AddNew]", para);
                result = 1;
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("PhoneAndroid_AddNew:" + str);
                //throw;
            }
            return result;
        }

        /// <summary>
        /// iPhone设备
        /// </summary>
        /// <param name="DeviceID">DeviceID</param>
        /// <param name="PhoneNumber">电话号码</param>
        /// <param name="SoftVersion">软件版本</param>
        /// <returns>返回1代表成功，0代表失败</returns>
        /// 日期：2014-4-27日
        public static int PhoneIPhone_AddNew(string DeviceID, string PhoneNumber,string OSVersion, double SoftVersion)
        {
            SqlParameter[] para = {
                                      new SqlParameter("@DeviceID", DeviceID),
                                      new SqlParameter("@PhoneNumber", PhoneNumber),
                                      new SqlParameter("@OSVersion", OSVersion),
                                      new SqlParameter("@SoftVersion", SoftVersion)
                                  };
            int result = 0;
            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.connectionStringAQI, CommandType.StoredProcedure,
                                                        "[PhoneIPhone_AddNew]", para);
                result = 1;
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("PhoneIPhone_AddNew:" + str);
                //throw;
            }
            return result;
        }

        /// <summary>
        /// 电话设备
        /// </summary>
        /// <param name="PhoneNumber">电话号码</param>
        /// <returns>返回1代表成功，0代表失败</returns>
        /// 日期：2014-4-27日
        public static int PhoneTelPhone_AddNew(string PhoneNumber)
        {
            SqlParameter[] para = {
                                      new SqlParameter("@PhoneNumber", PhoneNumber),
                                  };
            int result = 0;
            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.connectionStringAQI, CommandType.StoredProcedure,
                                                        "[PhoneIPhone_AddNew]", para);
                result = 1;
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("PhoneIPhone_AddNew:" + str);
                //throw;
            }
            return result;
        }
    }
}
