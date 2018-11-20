using System;
using System.Collections.Generic;
using MMShareBLL.DAL;
using MMShareBLL.Model;


namespace MMShareBLL.BLL
{
    /// <summary>
    /// 对分组数据的查询(加工后的数据)
    /// </summary>
    public class GroupDataQuery
    {
        private DailyDataAccess dal = new DailyDataAccess();

        /// <summary>
        /// 根据编号查询组数据
        /// </summary>
        /// <returns></returns>
        public GroupData QueryGroupData(string groupId, DateTime day, Duration duration)
        {
            try
            {
                day = DataCalculate.ReParseDateTime(day, duration, false);
                return dal.QueryGroupData(groupId, day, duration);
            }
            catch { throw; }
        }

        /// <summary>
        /// 查询某个日期，某个期间的组数据
        /// </summary>
        /// <returns></returns>
        public List<GroupData> QueryGroupData(DateTime day, Duration duration)
        {
            try
            {
                day = DataCalculate.ReParseDateTime(day, duration, false);
                return dal.QueryGroupData(day, duration);
            }
            catch { throw; }
        }

        public Dictionary<DateTime, GroupData> QueryGroupData(DateTime d1, DateTime d2, Duration duration, string groupId)
        {
            try
            {
                d1 = DataCalculate.ReParseDateTime(d1, duration, false);
                d2 = DataCalculate.ReParseDateTime(d2, duration, true);
                return dal.QueryGroupData(d1, d2, duration, groupId);
            }
            catch { throw; }
        }

        public Dictionary<string, Dictionary<DateTime, GroupData>> QueryGroupData(DateTime d1, DateTime d2, Duration duration, string[] groupId)
        {
            try
            {
                d1 = DataCalculate.ReParseDateTime(d1, duration, false);
                d2 = DataCalculate.ReParseDateTime(d2, duration, true);
                return dal.QueryGroupData(d1, d2, duration, groupId);
            }
            catch { throw; }
        }

        public Dictionary<string, Dictionary<DateTime, GroupData>> QueryGroupData(DateTime d1, DateTime d2, Duration duration, string[] groupId,bool flag)
        {
            try
            {
                d1 = DataCalculate.ReParseDateTime(d1, duration, false);
                d2 = DataCalculate.ReParseDateTime(d2, duration, true);
                return dal.QueryGroupData(d1, d2, duration, groupId,true);
            }
            catch { throw; }
        }

        /// <summary>
        /// 获得某个分组的详细数据
        /// </summary>
        public GroupData QueryGroupDetail(string groupId, DateTime day, Duration duration, bool validateSite)
        {
            try
            {
                day = DataCalculate.ReParseDateTime(day, duration, false);
                GroupData data = dal.QueryGroupDetail(groupId, day, duration);
                if (validateSite)
                {
                    Area area = new Area();
                    foreach ( MMShareBLL.Model.SiteData sd in data.SiteData)
                    {
                        Site site = area.GetSite(sd.Site.Id, true);
                        if (null != site)
                            sd.Site = site;
                    }
                }
                return data;
            }
            catch { throw; }
        }

        /// <summary>
        /// 获得组数据的污染参数列表
        /// </summary>
        public List<DataParameter> GetGroupParameters(DateTime day, Duration duration)
        {
            try
            {
                day = DataCalculate.ReParseDateTime(day, duration, false);
                List<int> paramIdList = dal.GetGroupParameters(day, duration);
                return new ParameterManager().GetParameters(paramIdList);
            }
            catch { throw; }
        }

        public List<DataParameter> GetGroupParameters(DateTime d1, DateTime d2, Duration duration, string[] groupId)
        {
            try
            {
                if (groupId.Length == 0)
                    return new List<DataParameter>();
                d1 = DataCalculate.ReParseDateTime(d1, duration, false);
                d2 = DataCalculate.ReParseDateTime(d2, duration, true);
                List<int> paramIdList = dal.GetGroupDataParameters(d1, d2, duration, groupId);
                return new ParameterManager().GetParameters(paramIdList);
            }
            catch { throw; }
        }


        /// <summary>
        /// 查询某个时间分区数据下的站点组编号列表
        /// </summary>
        /// <returns></returns>
        public List<string> QueryCountyReportGroups(DateTime day, Duration duration)
        {
            try
            {
                day = DataCalculate.ReParseDateTime(day, duration, false);
                return dal.QueryCountyReportGroups(day, duration);
            }
            catch { throw; }
        }

        public List<DataParameter> GetGroupParameters(DateTime d1, DateTime d2, Duration duration, string groupId)
        {
            try
            {
                d1 = DataCalculate.ReParseDateTime(d1, duration, false);
                d2 = DataCalculate.ReParseDateTime(d2, duration, true);
                List<int> dataList = dal.GetGroupDataParameters(d1, d2, duration, groupId);
                ParameterManager pm = new ParameterManager();
                return pm.GetParameters(dataList);
            }
            catch { throw; }
        }

        public DateTime? GetLastGroupDate(Duration duration)
        {
            try
            {
                return dal.GetLastGroupDate(duration);
            }
            catch { throw; }
        }
    }
}
