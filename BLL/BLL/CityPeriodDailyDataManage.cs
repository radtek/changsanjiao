using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MMShareBLL.Model;
using MMShareBLL.DAL;

namespace MMShareBLL.BLL
{
    public class CityPeriodDailyDataManage
    {
        CityPeriodDailyDataAccess dal;

        public CityPeriodDailyDataManage()
        {
        }

        public CityPeriodDailyDataManage(Dictionary<int, string>  periodDic)
        {
            dal = new CityPeriodDailyDataAccess(periodDic);
        }
        /// <summary>
        /// 加工分段日报数据
        /// </summary>
        /// <param name="day"></param>
        /// <param name="createOperation"></param>
        /// <param name="RTData"></param>
        public void CreateDailyReport(DateTime day, string createOperation, bool RTData)
        {
            try
            {
                dal.CreateDailyReport(day, createOperation, RTData);
            }
            catch { throw; }
        }
        /// <summary>
        /// 获取分时段日报基本数据
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public Dictionary<DateTime,CityPeriodData> getCityPeriodDailyDic(DateTime d1,DateTime d2)
        {
            try
            {
                d1 = DataCalculate.ReParseDateTime(d1, Duration.Day, false);
                d2 = DataCalculate.ReParseDateTime(d2, Duration.Day, true);
                return dal.getCityPeriodDailyDic(d1, d2);
            }
            catch { throw; }
        }

        /// <summary>
        /// 获取分时段日报全部数据
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public Dictionary<DateTime, CityPeriodData> getCityPeriodDailyDataDic(DateTime d1, DateTime d2)
        {
            try
            {
                d1 = DataCalculate.ReParseDateTime(d1, Duration.Day, false);
                d2 = DataCalculate.ReParseDateTime(d2, Duration.Day, true);
                return dal.getCityPeriodDailyDataDic(d1, d2);
            }
            catch { throw; }
        }
        /// <summary>
        /// 加工获取实时分段日报数据
        /// </summary>
        /// <param name="operatorMan"></param>
        /// <returns></returns>
        public Dictionary<DateTime, CityPeriodData> getCityRTPeriodDailyDataDic(DateTime day, string createOperation, bool RTData)
        {
            try
            {
                return dal.getCityRTPeriodDailyDataDic(day, createOperation, RTData);
            }
            catch { throw; }
        }
    }
}
