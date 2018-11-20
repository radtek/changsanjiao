using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace AQIQuery.aQuery
{
    public  class SiteGroup
    {
        /// <summary>
        /// 获取GroupID，根据GroupName，限制为二级节点，名字不会重复，所以不会获取到两个或以上的GoupID。
        /// 用到了存储过程：SiteGroup_GetGroupIDbyName
        /// </summary>
        /// <param name="GroupName">二级分组名称</param>
        /// <returns>返回int，如果返回值为-1，说明返回无效</returns>
        public static int GetGroupIDbyName(string GroupName)
        {
            return AQIQuery.DAL.Group.GetGroupIDbyName(GroupName);
        }

        /// <summary>
        /// 获取所以分区ID和名称
        /// 用到了存储过程：SiteGroup_GetAllDistrict
        /// </summary>
        /// <returns>返回DataTable</returns>
        public static DataTable GetAllDistrict()
        {
            return AQIQuery.DAL.Group.GetAllDistrict();
        }

        /// <summary>
        /// 日期：2014-3-16日
        /// 获取所以分区ID和名称
        /// 用到了存储过程：SiteGroup_GetAllDistrict
        /// </summary>
        /// <returns>返回string, 各GroupID之间用","分隔</returns>
        public static string GetAllDistrictGroupIDsString()
        {
            return AQIQuery.DAL.Group.GetAllDistrictGroupIDsString();
        }

        /// <summary>
        /// 日期：2014-3-16日
        /// 获取全市平均和所以分区ID
        /// 用到了存储过程：SiteGroup_GetAllCityorDistrict
        /// </summary>
        /// <returns>返回string, 各GroupID之间用","分隔</returns>
        public static string GetAllCityorDistrictGroupIDsString(bool isCity, bool isDistrict)
        {


            return AQIQuery.DAL.Group.GetAllCityorDistrictGroupIDsString(isCity, isDistrict);
        }


        /// 日期：2014-3-17
        /// 获取通过SiteIDs获取这些SiteIDs所在的GroupID，GroupName等
        /// 用到了存储过程：SiteGroup_GetGroup_bySiteIDs
        /// </summary>
        /// <SiteIDs>站点编号，以逗号分隔</SiteIDs>
        /// <mustBeProecss>必须是需要加工的分组吗</mustBeProecss>
        /// <returns>返回string, 各GroupID之间用","分隔</returns>
        public static DataTable GetGetGroup_bySiteIDs(string SiteIDs, bool mustBeProcess)
        {
            return AQIQuery.DAL.Group.GetGetGroup_bySiteIDs(SiteIDs, mustBeProcess);
        }


         /// 日期：2014-3-17
        /// 获取通过SiteIDs获取这些SiteIDs所在的GroupID，GroupName等
        /// 用到了存储过程：SiteGroup_GetGroup_bySiteIDs
        /// </summary>
        /// <SiteIDs>站点编号，以逗号分隔</SiteIDs>
        /// <mustBeProecss>必须是需要加工的分组吗</mustBeProecss>
        /// <returns>返回string, 各GroupID之间用","分隔</returns>
        public static string GetGetGroup_bySiteIDsString(string SiteIDs, bool mustBeProcess)
        {
            return AQIQuery.DAL.Group.GetGetGroup_bySiteIDsString(SiteIDs, mustBeProcess);
        }

        /// <summary>
        /// 根据GroupIDs获取其中的AQI站点编号
        /// 用到了存储过程：SiteGroupSites_GetSitebyGroupIDs
        /// </summary>
        /// <param name="GroupIDs">多个分组的ID号，用","分隔</param>
        /// <returns>返回DataTable</returns>
        public static DataTable GetSiteIDbyGroupIDs(string GroupIDs)
        {
            return AQIQuery.DAL.GroupSite.GetSiteIDbyGroupIDs(GroupIDs);
        }

        /// <summary>
        /// 根据GroupIDs获取其中的AQI站点编号
        /// 用到了存储过程：SiteGroupSites_GetSitebyGroupIDs
        /// </summary>
        /// <param name="GroupIDs">多个分组的ID号，用","分隔</param>
        /// <returns>返回string ，每个站点之间用","分隔开</returns>
        public static string GetSiteIDbyGroupIDsString(string GroupIDs)
        {
            return AQIQuery.DAL.GroupSite.GetSiteIDbyGroupIDsString(GroupIDs);
        }

    }
}
