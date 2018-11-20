using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace AQIQuery.DAL
{
    class ViewLog
    {
        /// <summary>
        /// 写入手机访问日志
        /// 用到了存储过程：[ViewLogMobile_AddNew]
        /// </summary>
        /// <param name="DeviceID">手机的硬件ID</param>
        /// <param name="MobileType">手机类型，共两种选项：iPhone, Android</param>
        /// <param name="IPAddress">IP地址</param>
        /// <param name="DataType">数据类型，选项有：SiteData, GroupData, GroupSite, Warning</param>
        /// <param name="Method">访问的函数名以及参数</param>
        /// <param name="Lon">经度</param>
        /// <param name="Lat">纬度</param>
        /// <returns>返回1代表成功，0代表失败</returns>
        /// 日期：2014-4-25日
        public static int ViewLogMobile_AddNew(string DeviceID, string MobileType, string IPAddress, string DataType, string Method, double Lon, double Lat)
        {
            SqlParameter[] para = {
                                      new SqlParameter("@DeviceID", DeviceID),
                                      new SqlParameter("@MobileType", MobileType),
                                      new SqlParameter("@IPAddress", IPAddress),
                                      new SqlParameter("@DataType", DataType),
                                      new SqlParameter("@Method", Method),
                                      new SqlParameter("@Lon", Lon),
                                      new SqlParameter("@Lat", Lat),
                                  };
            int result = 0;
            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.connectionStringAQI, CommandType.StoredProcedure,
                                                        "[ViewLogMobile_AddNew]", para);
                result = 1;
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("ViewLogMobile_AddNew:"+str);
                //throw;
            }
            return result;
        }
        /// <summary>
        /// 写入Website访问日志
        /// </summary>
        /// <param name="IPAddress">IP地址</param>
        /// <param name="OS">客户端操作系统</param>
        /// <param name="Browser">客户端浏览器</param>
        /// <param name="DataType">数据类型，选项有：SiteData, GroupData, GroupSite, Warning</param>
        /// <param name="Method">访问的函数名以及参数</param>
        /// <returns>返回1代表成功，0代表失败</returns>
        /// 日期：2014-4-27日
        public static int ViewLogWebsite_AddNew(string IPAddress, string OS, string Browser, string DataType, string Method)
        {
            SqlParameter[] para = {
                                      new SqlParameter("@IPAddress", IPAddress),
                                      new SqlParameter("@OS", OS),
                                      new SqlParameter("@Browser", Browser),
                                      new SqlParameter("@DataType", DataType),
                                      new SqlParameter("@Method", Method),
                                  };
            int result = 0;
            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.connectionStringAQI, CommandType.StoredProcedure,
                                                        "[ViewLogWebsite_AddNew]", para);
                result = 1;
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("ViewLogWebsite_AddNew:" + str);
                //throw;
            }
            return result;
        }
        /// <summary>
        /// 写入电话语音访问日志
        /// </summary>
        /// <param name="PhoneNumber">电话号码</param>
        /// <param name="Method">访问的函数名以及参数</param>
        /// <returns>返回1代表成功，0代表失败</returns>
        /// 日期：2014-4-27日
        public static int ViewLogTelephone_AddNew(string PhoneNumber, string Method)
        {
            SqlParameter[] para = {
                                      new SqlParameter("@PhoneNumber", PhoneNumber),
                                      new SqlParameter("@Method", Method),
                                  };
            int result = 0;
            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.connectionStringAQI, CommandType.StoredProcedure,
                                                        "[ViewLogTelephone_AddNew]", para);
                result = 1;
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("ViewLogTelephone_AddNew:" + str);
                //throw;
            }
            return result;
        }

        /// <summary>
        /// 写入区县访问日志
        /// 用到了存储过程：[ViewLogCountyUser_AddNew]
        /// </summary>
        /// <param name="UserID">区县用户</param>
        /// <param name="Method">访问的函数名以及参数</param>
        /// <returns>返回1代表成功，0代表失败</returns>
        /// 日期：2014-4-27日
        public static int ViewLogCountyUser_AddNew(string UserName, string Method)
        {
            SqlParameter[] para = {
                                      new SqlParameter("@UserName", UserName),
                                      new SqlParameter("@Method", Method)
                                  };
            int result = 0;
            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.connectionStringAQI, CommandType.StoredProcedure,
                                                        "[ViewLogCountyUser_AddNew]", para);
                result = 1;
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("ViewLogCountyUser_AddNew:" + str);
                //throw;
            }
            return result;
        }

    
    }
}
