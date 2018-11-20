using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace AQIQuery.aQuery
{
    public class Data
    {
        /// <summary>
        /// 查询站点的小时AQI以及浓度、等级等数据，可查的内容包括AQI、IAQI以及相对于浓度。说明：该函数较为通用，需要了解AQIItemID所代表的内容
        /// </summary>
        /// <param name="AQI_LST1">起始时间,AQI的时间，比统计时间大1个小时</param>
        /// <param name="AQI_LST2">结束时间,AQI的时间，比统计时间大1个小时</param>
        /// <param name="SiteIDs">站点编号组，可选多个站点，用","分隔</param>
        /// <param name="AQIItemIDs">AQIItem编号组，可选多个站点，用","分隔，可选的AQIItemID包括100至210，共22个，详细可参考AQIItem表</param>
        /// <returns>返回DataTable，字段包括</returns>
        public static DataTable SiteHourlyAQI(DateTime AQI_LST1, DateTime AQI_LST2, string SiteIDs, string AQIItemIDs)
        {
            //这里需要判断AQIItemIDs里是否包含了>=300的AQIItemID，如果包含了，则退出。
            return AQIQuery.DAL.DataQuery.Data_RT_Site_Get_bySiteIDandAQIItems(AQI_LST1.AddHours(-1),AQI_LST2.AddHours(-1),SiteIDs,AQIItemIDs);
        }

        /// <summary>
        /// 该方法用于对外服务的调用，查询从最新发布时间开始，最近N小时站点的小时AQI以及浓度、等级等数据，可查的内容包括AQI、IAQI以及相对于浓度。说明：该函数需要了解AQIItemID所代表的内容
        /// </summary>
        /// <param name="SiteIDs">站点编号组，可选多个站点，用","分隔</param>
        /// <param name="AQIItemIDs">AQIItem编号组，可选多个站点，用","分隔，可选的AQIItemID包括100至210，共22个，详细可参考AQIItem表</param>
        /// <returns>返回DataTable，字段包括</returns>
        public static DataTable SiteHourlyAQI(int Hours,string SiteIDs, string AQIItemIDs)
        {
            //这里需要判断AQIItemIDs里是否包含了>=300的AQIItemID，如果包含了，则退出。
            DateTime AQI_LST2 = aQuery.UpdateTime.GetPublishNow();
            DateTime AQI_LST1 = AQI_LST2.AddHours(-Hours + 1);
            return aQuery.Data.SiteHourlyAQI(AQI_LST1, AQI_LST2, SiteIDs, AQIItemIDs);
        }


        /// <summary>
        /// 该方法用于对外服务的调用，查询全部的用于实时发布的参数（包括AQI和IAQI及其对应的各污染物参数），查询时间从最新发布时间开始，最近N小时站点的小时AQI以及浓度、等级等数据，说明：该函数需要了解AQIItemID所代表的内容
        /// </summary>
        /// <param name="SiteIDs">站点编号组，可选多个站点，用","分隔</param>
        /// <returns>返回DataTable，字段包括</returns>
        public static DataTable SiteHourlyAQI(int Hours, string SiteIDs)
        {
            //这里需要判断AQIItemIDs里是否包含了>=300的AQIItemID，如果包含了，则退出。
            DateTime AQI_LST2 = aQuery.UpdateTime.GetPublishNow();
            DateTime AQI_LST1 = AQI_LST2.AddHours(-Hours + 1);
            string AQIItemIDs = aQuery.AQIItem.GetAQIItemRTAQIString(true, true,true);
            //int offset = Hours - 1;
            return aQuery.Data.SiteHourlyAQI(AQI_LST1, AQI_LST2, SiteIDs, AQIItemIDs);
        }


        /// <summary>
        /// 查询分组的小时AQI以及浓度、等级等数据，可查的内容包括AQI、IAQI以及相对于浓度。说明：该函数较为通用，需要了解AQIItemID所代表的内容
        /// </summary>
        /// <param name="AQI_LST1">起始时间,AQI的时间，比统计时间大1个小时</param>
        /// <param name="AQI_LST2">结束时间,AQI的时间，比统计时间大1个小时</param>
        /// <param name="GroupIDs">分组编号组，可选多个站点，用","分隔</param>
        /// <param name="AQIItemIDs">AQIItem编号组，可选多个站点，用","分隔，可选的AQIItemID包括100至210，共22个，详细可参考AQIItem表</param>
        /// <returns>返回DataTable，字段包括</returns>
        public static DataTable GroupHourlyAQI(DateTime AQI_LST1, DateTime AQI_LST2, string GroupIDs, string AQIItemIDs)
        {
            //这里需要判断AQIItemIDs里是否包含了>=300的AQIItemID，如果包含了，则退出。
            return AQIQuery.DAL.DataQuery.Data_RT_Group_Get_byGroupIDandAQIItems(AQI_LST1.AddHours(-1), AQI_LST2.AddHours(-1), GroupIDs, AQIItemIDs);
        }

        /// <summary>
        /// 该方法用于对外服务的调用，查询从最新发布时间开始，最近N小时某分组的小时AQI以及浓度、等级等数据，可查的内容包括AQI、IAQI以及相对于浓度。说明：该函数需要了解AQIItemID所代表的内容
        /// </summary>
        /// <param name="GroupIDs">站点编号组，可选多个站点，用","分隔</param>
        /// <param name="AQIItemIDs">AQIItem编号组，可选多个站点，用","分隔，可选的AQIItemID包括100至210，共22个，详细可参考AQIItem表</param>
        /// <returns>返回DataTable，字段包括</returns>
        public static DataTable GroupHourlyAQI(int Hours, string GroupIDs, string AQIItemIDs)
        {
            DateTime AQI_LST2 = aQuery.UpdateTime.GetPublishNow();
            DateTime AQI_LST1 = AQI_LST2.AddHours(-Hours + 1);
            return aQuery.Data.GroupHourlyAQI(AQI_LST1, AQI_LST2, GroupIDs, AQIItemIDs);
        }

        /// <summary>
        /// 该方法用于对外服务的调用，查询某分组全部的用于实时发布的参数（包括AQI和IAQI及其对应的各污染物参数），查询时间从最新发布时间开始，最近N小时站点的小时AQI以及浓度、等级等数据，说明：该函数需要了解AQIItemID所代表的内容
        /// </summary>
        /// <param name="GroupIDs">站点编号组，可选多个站点，用","分隔</param>
        /// <returns>返回DataTable，字段包括</returns>
        public static DataTable GroupHourlyAQI(int Hours, string GroupIDs)
        {
            DateTime AQI_LST2 = aQuery.UpdateTime.GetPublishNow();
            DateTime AQI_LST1 = AQI_LST2.AddHours(-Hours + 1);
            string AQIItemIDs = aQuery.AQIItem.GetAQIItemRTAQIString(true, true,true);
            return aQuery.Data.GroupHourlyAQI(AQI_LST1, AQI_LST2, GroupIDs, AQIItemIDs);
        }



        /// <summary>
        /// 查询站点的日报AQI以及浓度、等级等数据，可查的内容包括AQI、IAQI以及相对于浓度。说明：该函数较为通用，需要了解AQIItemID所代表的内容
        /// </summary>
        /// <param name="LST1">起始时间,AQI的时间</param>
        /// <param name="LST2">结束时间,AQI的时间</param>
        /// <param name="SiteIDs">站点编号组，可选多个站点，用","分隔</param>
        /// <param name="AQIItemIDs">AQIItem编号组，可选多个站点，用","分隔，可选的AQIItemID包括300至310，共11个，详细可参考AQIItem表</param>
        /// <returns>返回DataTable，字段包括</returns>
        public static DataTable SiteDailyAQI(DateTime LST1, DateTime LST2, string SiteIDs, string AQIItemIDs)
        {
            //这里需要判断AQIItemIDs里是否包含了>=300的AQIItemID，如果包含了，则退出。
            LST1 = Convert.ToDateTime(LST1.ToString("yyyy-MM-dd 0:00:00"));
            return AQIQuery.DAL.DataQuery.Data_Report_Site_Get_bySiteIDandAQIItems(LST1, LST2, SiteIDs, AQIItemIDs,false);
        }

        /// <summary>
        /// 该方法用于对外服务的调用，查询从最新发布日报时间开始，最近N天站点的日报AQI以及浓度、等级等数据，可查的内容包括AQI、IAQI以及相对于浓度。说明：该函数需要了解AQIItemID所代表的内容
        /// </summary>
        /// <param name="SiteIDs">站点编号组，可选多个站点，用","分隔</param>
        /// <param name="AQIItemIDs">AQIItem编号组，可选多个站点，用","分隔，可选的AQIItemID包括300至310，共11个，详细可参考AQIItem表</param>
        /// <returns>返回DataTable，字段包括</returns>
        public static DataTable SiteDailyAQI(int Days, string SiteIDs, string AQIItemIDs)
        {
            //这里需要判断AQIItemIDs里是否包含了>=300的AQIItemID，如果包含了，则退出。
            DateTime LST1 = aQuery.UpdateTime.GetPublishDaily();
            return aQuery.Data.SiteDailyAQI(LST1.AddDays(-Days + 1), LST1, SiteIDs, AQIItemIDs);
        }


        /// <summary>
        /// 该方法用于对外服务的调用，查询全部的用于日报的参数（包括AQI和IAQI及其对应的各污染物参数），查询时间从最新发布时间开始，最近N小时站点的小时AQI以及浓度、等级等数据，说明：该函数需要了解AQIItemID所代表的内容
        /// </summary>
        /// <param name="SiteIDs">站点编号组，可选多个站点，用","分隔</param>
        /// <returns>返回DataTable，字段包括</returns>
        public static DataTable SiteDailyAQI(int Days, string SiteIDs)
        {
            //这里需要判断AQIItemIDs里是否包含了>=300的AQIItemID，如果包含了则退出。
            DateTime LST1 = aQuery.UpdateTime.GetPublishNow();
            string AQIItemIDs = aQuery.AQIItem.GetAQIItemDailyAQIString(true, true);
            //int offset = Hours - 1;
            return aQuery.Data.SiteDailyAQI(LST1.AddDays(-Days + 1), LST1, SiteIDs, AQIItemIDs);
        }


        /// <summary>
        /// 查询分组的小时AQI以及浓度、等级等数据，可查的内容包括AQI、IAQI以及相对于浓度。说明：该函数较为通用，需要了解AQIItemID所代表的内容
        /// </summary>
        /// <param name="LST1">起始时间,AQI的时间，比统计时间大1个小时</param>
        /// <param name="AQI_LST2">结束时间,AQI的时间，比统计时间大1个小时</param>
        /// <param name="GroupIDs">分组编号组，可选多个站点，用","分隔</param>
        /// <param name="AQIItemIDs">AQIItem编号组，可选多个站点，用","分隔，可选的AQIItemID包括100至210，共22个，详细可参考AQIItem表</param>
        /// <returns>返回DataTable，字段包括</returns>
        public static DataTable GroupDailyAQI(DateTime LST1, DateTime LST2, string GroupIDs, string AQIItemIDs)
        {
            //这里需要判断AQIItemIDs里是否包含了>=300的AQIItemID，如果包含了，则退出。
            
            return AQIQuery.DAL.DataQuery.Data_Report_Group_Get_byGroupIDandAQIItems(LST1, LST2, GroupIDs, AQIItemIDs,false);
        }

        /// <summary>
        /// 该方法用于对外服务的调用，查询从最新发布的日报时间开始，最近N天某分组的小时AQI以及浓度、等级等数据，可查的内容包括AQI、IAQI以及相对于浓度。说明：该函数需要了解AQIItemID所代表的内容
        /// </summary>
        /// <param name="GroupIDs">站点编号组，可选多个站点，用","分隔</param>
        /// <param name="AQIItemIDs">AQIItem编号组，可选多个站点，用","分隔，可选的AQIItemID包括300至310，共11个，详细可参考AQIItem表</param>
        /// <returns>返回DataTable，字段包括</returns>
        public static DataTable GroupDailyAQI(int Days, string GroupIDs, string AQIItemIDs)
        {
            DateTime LST1 = aQuery.UpdateTime.GetPublishDaily();
            return aQuery.Data.GroupDailyAQI(LST1.AddDays(-Days + 1), LST1, GroupIDs, AQIItemIDs);
        }

        /// <summary>
        /// 该方法用于对外服务的调用，查询某分组全部的用于实时发布的参数（包括AQI和IAQI及其对应的各污染物参数），查询时间从最新发布时间开始，最近N小时站点的小时AQI以及浓度、等级等数据，说明：该函数需要了解AQIItemID所代表的内容
        /// </summary>
        /// <param name="GroupIDs">站点编号组，可选多个站点，用","分隔</param>
        /// <returns>返回DataTable，字段包括</returns>
        public static DataTable GroupDailyAQI(int Days, string GroupIDs)
        {
            DateTime LST1 = aQuery.UpdateTime.GetPublishDaily();
            string AQIItemIDs = aQuery.AQIItem.GetAQIItemDailyAQIString(true, true);
            return aQuery.Data.GroupDailyAQI(LST1.AddDays(-Days + 1), LST1, GroupIDs, AQIItemIDs);
        }




    }
}
