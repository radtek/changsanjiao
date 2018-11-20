using System;
using System.Collections.Generic;
using System.Text;

namespace AQIQuery.aQuery
{
    public class ViewLog
    {
        /// <summary>
        /// 写入iPhone手机访问日志
        /// </summary>
        /// <param name="DeviceID">手机的硬件ID</param>
        /// <param name="IPAddress">IP地址</param>
        /// <param name="DataType">数据类型，选项有：SiteData, GroupData, GroupSite, Warning</param>
        /// <param name="Method">访问的函数名以及参数</param>
        /// <param name="Lon">经度</param>
        /// <param name="Lat">纬度</param>
        /// <returns>返回1代表成功，0代表失败</returns>
        /// 日期：2014-4-25日
        public static int AddViewLogiPhone(string DeviceID, string IPAddress, string DataType, string Method, double Lon, double Lat)
        {
            string MobileType = "iPhone";
            return AQIQuery.DAL.ViewLog.ViewLogMobile_AddNew(DeviceID, MobileType, IPAddress, DataType, Method, Lon, Lat);
        }
        /// <summary>
        /// 写入Android手机访问日志
        /// </summary>
        /// <param name="DeviceID">手机的硬件ID</param>
        /// <param name="IPAddress">IP地址</param>
        /// <param name="DataType">数据类型，选项有：SiteData, GroupData, GroupSite, Warning</param>
        /// <param name="Method">访问的函数名以及参数</param>
        /// <param name="Lon">经度</param>
        /// <param name="Lat">纬度</param>
        /// <returns>返回1代表成功，0代表失败</returns>
        /// 日期：2014-4-25日
        public static int AddViewLogAndroid(string DeviceID, string IPAddress, string DataType, string Method, double Lon, double Lat)
        {
            string MobileType = "Android";
            return AQIQuery.DAL.ViewLog.ViewLogMobile_AddNew(DeviceID, MobileType, IPAddress, DataType, Method, Lon, Lat);
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
            return AQIQuery.DAL.ViewLog.ViewLogWebsite_AddNew(IPAddress, OS, Browser, DataType, Method);
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
            return AQIQuery.DAL.ViewLog.ViewLogTelephone_AddNew(PhoneNumber,Method);
        }


        /// <summary>
        /// 写入区县访问日志
        /// 用到了存储过程：[ViewLogCountyUser_AddNew]
        /// </summary>
        /// <param name="UserID">区县用户ID</param>
        /// <param name="Method">访问的函数名以及参数</param>
        /// <returns>返回1代表成功，0代表失败</returns>
        /// 日期：2014-4-27日
        public static int AddViewLogCountyUser(string UserName, string Method)
        {
            return AQIQuery.DAL.ViewLog.ViewLogCountyUser_AddNew(UserName, Method);
        }
    }
}
