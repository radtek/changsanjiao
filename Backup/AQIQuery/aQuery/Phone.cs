using System;
using System.Collections.Generic;
using System.Text;

namespace AQIQuery.aQuery
{
    public class Phone
    {

        /// <summary>
        /// Android设备
        /// </summary>
        /// <param name="DeviceID">DeviceID</param>
        /// <param name="PhoneNumber">电话号码</param>
        /// <param name="SoftVersion">软件版本</param>
        /// <returns>返回1代表成功，0代表失败</returns>
        /// 日期：2014-4-27日
        /// 更新日期：2014-5-8增加了OSVersion
        public static int PhoneAndroid_AddNew(string DeviceID, string PhoneNumber,string OSVersion, double SoftVersion)
        {
            return AQIQuery.DAL.Phone.PhoneAndroid_AddNew(DeviceID, PhoneNumber, OSVersion, SoftVersion);
        }

        /// <summary>
        /// iPhone设备
        /// </summary>
        /// <param name="DeviceID">DeviceID</param>
        /// <param name="PhoneNumber">电话号码</param>
        /// <param name="SoftVersion">软件版本</param>
        /// <returns>返回1代表成功，0代表失败</returns>
        /// 日期：2014-4-27日
        /// 更新日期：2014-5-8增加了OSVersion
        public static int PhoneIPhone_AddNew(string DeviceID, string PhoneNumber,string OSVersion, double SoftVersion)
        {
            return AQIQuery.DAL.Phone.PhoneIPhone_AddNew(DeviceID, PhoneNumber,OSVersion, SoftVersion);
        }

        /// <summary>
        /// 电话设备
        /// </summary>
        /// <param name="PhoneNumber">电话号码</param>
        /// <returns>返回1代表成功，0代表失败</returns>
        /// 日期：2014-4-27日
        public static int PhoneTelPhone_AddNew(string PhoneNumber)
        {

            return AQIQuery.DAL.Phone.PhoneTelPhone_AddNew(PhoneNumber);
        }
    }
}
