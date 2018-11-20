using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace AQIQuery.aQuery
{
    public class Site
    {
        /// <summary>
        /// 获取全部AQI站点（包含国控点和市控点）
        /// 用到了存储过程：SiteGroupSites_GetAllAQISite
        /// </summary>
        /// <returns>返回string ，每个站点之间用","分隔开</returns>
        public static string GetAllAQISiteIDString()
        {
            return AQIQuery.DAL.GroupSite.GetAllAQISiteIDString();
        }
        /// <summary>
        /// 获取全部AQI站点（包含国控点和市控点）
        /// 用到了存储过程：SiteGroupSites_GetSitebyGroupID
        /// </summary>
        /// <param name="GroupID">站点分组的ID号</param>
        /// <returns>返回string ，每个站点之间用","分隔开</returns>
        public static string GetSiteIDbyGroupIDString(int GroupID)
        {
            return AQIQuery.DAL.GroupSite.GetSiteIDbyGroupIDString(GroupID);
        }

        /// <summary>
        /// 获取全部AQI站点（包含国控点和市控点）
        /// 用到了存储过程：SiteGroupSites_GetAllAQISite
        /// </summary>
        /// <returns>返回DataTable</returns>
        public static DataTable GetAllAQISiteID()
        {
            return AQIQuery.DAL.GroupSite.GetAllAQISiteID();
        }
        /// <summary>
        /// 获取全部AQI站点（包含国控点和市控点）
        /// 用到了存储过程：SiteGroupSites_GetSitebyGroupID
        /// </summary>
        /// <param name="GroupID">站点分组的ID号</param>
        /// <returns>返回DataTable</returns>
        public static DataTable GetSiteIDbyGroupID(int GroupID)
        {
            return AQIQuery.DAL.GroupSite.GetSiteIDbyGroupID(GroupID);
        }

        /// <summary>
        /// 根据GroupIDs获取其中的AQI站点信息，包括站点编码、站点名称、经度、纬度等信息
        /// </summary>
        /// <param name="GroupID">区县分组编码</param>
        /// <returns>返回DataTable</returns>
        public static DataTable GetSiteInfobyGroupID(int GroupID)
        {
            return AQIQuery.DAL.GroupSite.GetSiteInfobyGroupID(GroupID);
        }

        /// <summary>
        /// 获取所有AQI站点信息，包括站点编码、站点名称、经度、纬度等信息
        /// </summary>
        /// <returns>返回DataTable</returns>
        /// 日期:2014-5-8
        public static DataTable GetAllAQISites()
        {


            return AQIQuery.DAL.GroupSite.Site_GetAllAQISites();
        }
    }
}
