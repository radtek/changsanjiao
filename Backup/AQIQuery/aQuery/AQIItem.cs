using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace AQIQuery.aQuery
{
    public class AQIItem
    {
        /// <summary>
        /// 根据ParameterIDs获取AQIItems
        /// 用到了存储过程：AQIItem_Get_byParameterIDs
        /// </summary>
        /// <returns>返回DataTable</returns>
        ///日期：2014-3-17日
        public static DataTable GetAQIItemByParameterIDs(string ParameterIDs, string TimeType)
        {
            return AQIQuery.DAL.AQIItem.GetAQIItemByParameterIDs(ParameterIDs, TimeType);

        }
        /// <summary>
        /// 根据ParameterIDs获取AQIItems
        /// 用到了存储过程：AQIItem_Get_byParameterIDs
        /// </summary>
        /// <returns>返回string ，每个站点之间用","分隔开</returns>
        /// 日期：2014-3-17日
        public static string GetAQIItemByParameterIDsString(string ParameterIDs, string TimeType)
        {
            return AQIQuery.DAL.AQIItem.GetAQIItemByParameterIDsString(ParameterIDs,TimeType);
        }

        /// <summary>
        /// 获取需要Process的AQIItems
        /// 用到了存储过程：AQIItem_GetProcess
        /// </summary>
        /// <returns>返回string ，每个站点之间用","分隔开</returns>
        /// 日期：2014-3-15日
        public static string GetAQIItemRTAQIString(bool isAQI, bool isIAQI,bool is24hrAQI)
        {
            return DAL.AQIItem.GetAQIItemRTAQIString(isAQI, isIAQI, is24hrAQI);
        }

        /// <summary>
        /// 获取需要日报AQI的AQIItems
        /// </summary>
        /// <returns>返回DataTable</returns>
        /// 日期：2014-3-19日
        /// 用到了存储过程：AQIItem_GetDayAQI
        public static DataTable GetAQIItemDailyAQI(bool isAQI, bool isIAQI)
        {

            return AQIQuery.DAL.AQIItem.GetAQIItemDailyAQI(isAQI,isIAQI);
        }

        /// <summary>
        /// 获取需要日报AQI的AQIItems
        /// </summary>
        /// <returns>返回string ，每个站点之间用","分隔开</returns>
        /// 日期：2014-3-19日
        public static string GetAQIItemDailyAQIString(bool isAQI, bool isIAQI)
        {


            return AQIQuery.DAL.AQIItem.GetAQIItemDailyAQIString(isAQI,isIAQI);
        }
        /// <summary>
        /// 获取需要Process的AQIItems
        /// 用到了存储过程：AQIItem_GetProcess
        /// </summary>
        /// <returns>返回string ，每个站点之间用","分隔开</returns>
        /// 日期：2014-3-15日
        public static string GetAQIItemProcessString(string TimeType)
        {
            return AQIQuery.DAL.AQIItem.GetAQIItemProcessString(TimeType);
        }

        /// <summary>
        /// 获取需要Process的AQIItems
        /// 用到了存储过程：AQIItem_GetProcess
        /// </summary>
        /// <returns>返回DataTable</returns>
        /// 日期：2014-3-15日
        public static DataTable GetAQIItemProcess(string TimeType)
        {


            return AQIQuery.DAL.AQIItem.GetAQIItemProcess(TimeType);
        }

    }
}
