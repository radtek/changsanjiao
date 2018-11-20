using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;


namespace AQIQuery.aQuery
{
    public class UpdateTime
    {
        /// <summary>
        /// 根据UpdateName获取最新的更新时
        /// <param name="UpdateName">更新时间的名称，WebNow、AQINow、DMSNow分别代表AQI对外服务、AQI生成系统和DMS系统中的最新数据时间</param>
        /// </summary>
        /// <returns>返回最新的Datetime</returns>
        public static DateTime GetUpdateTime(string UpdateName)
        {
            return DAL.UpdateTime.Updatetime_Get_byName(UpdateName);
        }

        /// <summary>
        /// 对外服务中调用的的最新更新时间方法，更新时间名称为WebNow
        /// </summary>
        /// <returns>返回对外服务的数据最新更新时间，返回数据类型为Datetime</returns>
        public static DateTime GetPublishNow()
        {
            //string UpdateName = "WebNow";
            //return UpdateTime.GetUpdateTime(UpdateName);
            
            DateTime WebNow;
            int minute = int.Parse(ConfigurationManager.AppSettings["WebNowChangeAtMinute"].ToString());
            string UpdateName = "AQINow";
            DateTime AQINow = UpdateTime.GetUpdateTime(UpdateName);
            int NowHour=DateTime.Now.Hour;
            int NowMinute=DateTime.Now.Minute;
            if (AQINow.Hour < NowHour)
            {
                WebNow = AQINow;
            }
            else
            {
                if (NowMinute >= minute)
                {
                    WebNow = AQINow;
                }
                else
                {
                    WebNow = AQINow.AddHours(-1);
                }
            }
            return WebNow;

        }

        /// <summary>
        /// AQI工厂中已经生成好的最新更新时间，但对外服务的更新时间不一定已经更新到这个时间了，更新时间名称为AQINow。如果是对外服务的调用，请使用GetPublishNow方法，
        /// </summary>
        /// <returns>返回对AQI工厂中的数据最新更新时间，返回数据类型为Datetime</returns>
        public static DateTime GetAQIFactoryNow()
        {
            string UpdateName = "AQINow";
            return UpdateTime.GetUpdateTime(UpdateName);
        }

        /// <summary>
        /// 最原始的数据库DMS中数据的最新更新时间，这个时候还不一定已经生成好AQI，更不一定已经对外发布。
        /// </summary>
        /// <returns>返回最原始的数据库的数据最新更新时间，返回数据类型为Datetime</returns>
        public static DateTime GetDMSNow()
        {
            string UpdateName = "DMSNow";
            return UpdateTime.GetUpdateTime(UpdateName);
        }


        /// <summary>
        /// 对外服务中调用的的最新更新时间方法，更新时间名称为WebNow
        /// </summary>
        /// <returns>返回对外服务的数据最新更新时间，返回数据类型为Datetime</returns>
        public static DateTime GetPublishDaily()
        {
            string UpdateName = "WebDaily";
            return UpdateTime.GetUpdateTime(UpdateName);
        }
        /// <summary>
        /// 对外服务中调用的的最新更新时间方法，更新时间名称为WebNow
        /// </summary>
        /// <returns>返回对外服务的数据最新更新时间，返回数据类型为Datetime</returns>
        public static DateTime GetAQIFactoryDaily()
        {
            string UpdateName = "AQIDaily";
            return UpdateTime.GetUpdateTime(UpdateName);
        }



    }
}
