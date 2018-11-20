using System;
using System.Collections.Generic;
using System.Text;
using MMShareBLL.DAL;
using MMShareBLL.Model;


namespace MMShareBLL.BLL
{
    /// <summary>
    /// 提供分区数据的查询(加工后的数据)
    /// </summary>
    public class CountyReportDataQuery
    {
        DailyDataAccess dal = new DailyDataAccess();

        /// <summary>
        /// 获取分区数据，仅含基本信息
        /// </summary>
        /// <param name="day"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public CountyReportData QueryCountyReport(DateTime day, Duration duration)
        {
            try
            {
                day = DataCalculate.ReParseDateTime(day, duration, false);
                return dal.QueryCountyReport(day, duration);
            }
            catch { throw; }
        }



        /// <summary>
        /// 查询分区数据
        /// </summary>
        /// <returns></returns>
        public CountyReportData QueryCountyReportDetail(DateTime day, Duration duration)
        {

            try
            {
                day = DataCalculate.ReParseDateTime(day, duration, false);
                return dal.QueryCountyReportDetail(day, duration);
            }
            catch { throw; }
        }

        public CountyReportData QueryCountyReportDetail2(DateTime day, Duration duration,string countys)
        {

            try
            {
                day = DataCalculate.ReParseDateTime(day, duration, false);
                return dal.QueryCountyReportDetail2(day, duration,countys);
            }
            catch { throw; }
        }

        public Dictionary<DateTime, CountyReportData> QueryCountyReportDetail(DateTime d1, DateTime d2, Duration duration)
        {
            try
            {
                d1 = DataCalculate.ReParseDateTime(d1, duration, false);
                d2 = DataCalculate.ReParseDateTime(d2, duration,true);
                return dal.QueryCountyReportDetail(d1, d2, duration);
            }
            catch { throw; }
        }

        public Dictionary<DateTime, CountyReportData> QueryCountyReportDetail(DateTime d1, DateTime d2, Duration duration, string[] countyId)
        {
            try
            {
                d1 = DataCalculate.ReParseDateTime(d1, duration, false);
                d2 = DataCalculate.ReParseDateTime(d2, duration, true);
                return dal.QueryCountyReportDetail(d1, d2, duration, countyId);
            }
            catch { throw; }
        }

        public Dictionary<DateTime, CountyReportData> QueryCountyReportDetail(DateTime d1, DateTime d2, Duration duration, string[] countyId, int[] parameterList)
        {
            try
            {
                d1 = DataCalculate.ReParseDateTime(d1, duration, false);
                d2 = DataCalculate.ReParseDateTime(d2, duration, true);
                return dal.QueryCountyReportDetail(d1, d2, duration, countyId, parameterList);
            }
            catch { throw; }
        }

        public List<DataParameter> QueryCountyReportParameters(DateTime d1, DateTime d2, Duration duration)
        {
            try
            {
                return new ParameterManager().GetParameters(dal.QueryCountyReportParameters(d1, d2, duration));
            }
            catch { throw; }
        }

        
    }
}
