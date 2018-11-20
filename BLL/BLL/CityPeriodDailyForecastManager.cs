using System;
using System.Collections.Generic;
using System.Text;
using MMShareBLL.Model;
using MMShareBLL.DAL;


namespace MMShareBLL.BLL 
{
    public class CityPeriodDailyForecastManager
    {
        CityPeriodDailyForecastAccess dal = new CityPeriodDailyForecastAccess();
        /// <summary>
        /// 写入分段预报数据
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Insert(CityPeriodDailyForecast item)
        {
            try
            {
                return dal.Insert(item);
            }
            catch { throw; }
        }
        /// <summary>
        /// 创建新的数据
        /// </summary>
        /// <param name="theDate"></param>
        public bool CreateNew(DateTime date)
        {
            CityPeriodDailyForecast result = new CityPeriodDailyForecast();
            result.ForecastDate = date;
            result.AfternoonAPI = 0;
            result.TomorrowAMAPI = 0;
            result.TomorrowAMAPIModify = 0;
            result.TomorrowPMAPI = 0;
            result.TomorrowPMAPIModify = 0;
            result.TonightAPI = 0;
            result.LastUpdateTime = System.DateTime.Now;
            try
            {
                return dal.Insert(result);
            }
            catch { throw; }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Update(CityPeriodDailyForecast item)
        {
            try
            {
                return dal.Update(item);
            }
            catch { throw; }
        }
        /// <summary>
        /// 根据预报时间查询分段预报
        /// </summary>
        /// <param name="ForecastDate1"></param>
        /// <param name="ForecastDate2"></param>
        /// <returns></returns>
        public Dictionary<DateTime, CityPeriodDailyForecast> QueryByDate(DateTime ForecastDate1, DateTime ForecastDate2)
        {
            try
            {
                return dal.QueryByDate(ForecastDate1, ForecastDate2);
            }
            catch { throw; }
        }
        /// <summary>
        /// 判断否日是否已创建分段预报
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public bool Exists(DateTime date)
        {
            DateTime startTime = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 00:00:00");
            DateTime endTime = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 23:59:59");
            try
            {
                Dictionary<DateTime, CityPeriodDailyForecast> results = dal.QueryByDate(startTime, endTime);
                return results.Count > 0;
            }
            catch { throw; }
        }
    }
}
