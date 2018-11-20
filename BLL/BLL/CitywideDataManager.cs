using System;
using System.Collections.Generic;
using MMShareBLL.DAL;
using MMShareBLL.Model;


namespace MMShareBLL.BLL 
{
    /// <summary>
    /// 关于全市日报报表的管理(加工后的数据)
    /// </summary>
    public class CitywideDataManager
    {

        private DailyDataAccess dal = new DailyDataAccess();

        private const int FINERATE_CODE = 59;

        /// <summary>
        /// 查询全市数据(仅获取少量数据，不包含监测因子数据和其下的站点数据)
        /// </summary>
        /// <returns></returns>
        public CitywideData QueryCitywideDaily(DateTime day, Duration duration)
        {
            try
            {
                day = DataCalculate.ReParseDateTime(day, duration, false);
                CitywideData cd = dal.QueryCitywideDaily(day, duration);
                fillDataSite(cd);
                return cd;
            }
            catch { throw; }
        }

        public CitywideData QueryCitywideDaily2(DateTime day, Duration duration)
        {
            try
            {
                day = DataCalculate.ReParseDateTime(day, duration, false);
                CitywideData cd = dal.QueryCitywideDaily2(day, duration);
                //fillDataSite(cd);
                return cd;
            }
            catch { throw; }
        }

        public Dictionary<DateTime, CitywideData> QueryCitywideDaily(DateTime d1, DateTime d2, Duration duration)
        {
            try
            {
                d1 = DataCalculate.ReParseDateTime(d1, duration, false);
                d2 = DataCalculate.ReParseDateTime(d2, duration, true);
                return dal.QueryCitywideDaily(d1, d2, duration);
            }
            catch { throw; }
        }

        public  MMShareBLL.Model.SiteData QueryCitywideDailyByStandardFormat(DateTime day, Duration duration)
        {
            try
            {
                CitywideData cd = QueryCitywideDaily(day, duration);
                if (null != cd)
                {
                    MMShareBLL.Model.SiteData sd = new MMShareBLL.Model.SiteData();
                    sd.SetDataCollection(new MonitoringDataCollection());
                    sd.DataCollection.MaxApi = cd.API;
                    return sd;
                }
                return null;
            }
            catch { throw; }
        }

        /// <summary>
        /// 返回通用格式的数据
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public Dictionary<DateTime, MMShareBLL.Model.SiteData> QueryCitywideDailyByStandardFormat(DateTime d1, DateTime d2, Duration duration)
        {
            try
            {
                Dictionary<DateTime, CitywideData> data = QueryCitywideDaily(d1, d2, duration);
                Dictionary<DateTime, MMShareBLL.Model.SiteData> rs = new Dictionary<DateTime, MMShareBLL.Model.SiteData>();
                foreach (DateTime time in data.Keys)
                {
                    MMShareBLL.Model.SiteData site = new MMShareBLL.Model.SiteData();
                    site.SetDataCollection(new MonitoringDataCollection());
                    site.DataCollection.MaxApi = data[time].API;
                    rs.Add(time, site);
                }
                return rs;
            }
            catch { throw; }
        }


        public Dictionary<DateTime, CitywideData> QueryCitywideDailyDetail(DateTime d1, DateTime d2, Duration duration, int[] parameterList)
        {
            try
            {
                d1 = DataCalculate.ReParseDateTime(d1, duration, false);
                d2 = DataCalculate.ReParseDateTime(d2, duration, true);
                return dal.QueryCitywideDailyDetail(d1, d2, duration, parameterList);
            }
            catch { throw; }
        }

        /// <summary>
        /// 查询全市数据，能获得全市各监测因子的浓度均值和API
        /// </summary>
        /// <returns></returns>
        public CitywideData QueryCitywideDailyDetail(DateTime day, Duration duration)
        {
            try
            {
                day = DataCalculate.ReParseDateTime(day, duration, false);
                CitywideData cd = dal.QueryCitywideDailyDetail(day, duration);
                fillDataParam(cd);
                fillDataSite(cd);
                return cd;
            }
            catch { throw; }
        }

        public CitywideData QueryCitywideDailyDetail2(DateTime day, Duration duration)
        {
            try
            {
                day = DataCalculate.ReParseDateTime(day, duration, false);
                CitywideData cd = dal.QueryCitywideDailyDetail2(day, duration);
                //fillDataParam(cd);
                //fillDataSite(cd);
                return cd;
            }
            catch { throw; }
        }

        public Dictionary<DateTime, CitywideData> QueryCitywideDailyDetail(DateTime d1, DateTime d2, Duration duration)
        {
            try
            {
                d1 = DataCalculate.ReParseDateTime(d1, duration, false);
                d2 = DataCalculate.ReParseDateTime(d2, duration, true);
                return dal.QueryCitywideDailyDetail(d1, d2, duration);
            }
            catch { throw; }
        }

        public List<DataParameter> GetCitywideDailyParameters(DateTime d1, DateTime d2, Duration duration)
        {
            try
            {
                d1 = DataCalculate.ReParseDateTime(d1, duration, false);
                d2 = DataCalculate.ReParseDateTime(d2, duration, true);
                List<int> dataList = dal.GetCitywideDailyParameters(d1, d2, duration);
                return new ParameterManager().GetParameters(dataList);
            }
            catch { throw; }
        }

        /// <summary>
        /// 查询日报数据，包涵所有信息
        /// </summary>
        /// <returns></returns>
        public CitywideData QueryCitywideDailyAllData(DateTime day, Duration duration)
        {
            try
            {
                day = DataCalculate.ReParseDateTime(day, duration, false);
                CitywideData cd = dal.QueryCitywideDailyAllData(day, duration);
                fillDataParam(cd);
                fillDataSite(cd);
                return cd;
            }
            catch { throw; }
        }

        /// <summary>
        /// 由于原始查询只获得了参数编号，在此根据编号查询填充对象
        /// </summary>
        /// <param name="data"></param>
        private void fillDataParam(CitywideData data)
        {
            if (null == data)
                return;
            ParameterManager pm = new ParameterManager();
            foreach (MonitoringData md in data.DataCollection)
            {
                md.Factor = pm.GetParameter(md.Factor.Id, true);
            }
        }

        /// <summary>
        /// 由于原始查询只获得了站点编号，在此根据编号查询填充站点对象
        /// </summary>
        /// <param name="data"></param>
        private void fillDataSite(CitywideData data)
        {
            if (null == data)
                return;
            Area area = new Area();
            foreach (MMShareBLL.Model.SiteData sd in data.SiteData)
            {
                sd.Site = area.GetSite(sd.Site.Id, true);
            }
            if (null != data.ReferenceSite)
                data.ReferenceSite = area.GetSite(data.ReferenceSite.Id, true);
        }

        /// <summary>
        /// 得到某个期间参加全市日报计算的站点编号列表
        /// </summary>
        /// <returns></returns>
        public List<int> QueryCitywideSiteList(DateTime day, Duration duration)
        {
            try
            {
                day = DataCalculate.ReParseDateTime(day, duration, false);
                return dal.QueryCitywideSiteList(day, duration);
            }
            catch { throw; }
        }

        /// <summary>
        /// 修改全市日报状态
        /// </summary>
        /// <param name="day"></param>
        /// <param name="status"></param>
        public void UpdateCitywideDailyStatus(DateTime day, DailyState status)
        {
            try
            {
                dal.UpdateCitywideDailyStatus(day, status);
            }
            catch { throw; }
        }

        public int getFineDayRate(ref float rate)
        {
            try
            {
                DateTime start = DateTime.Now;
                start = new DateTime(start.Year, 1, 1);
                DateTime end = DateTime.Now;
                end = new DateTime(end.Year, end.Month, end.Day);
                int dayCount = (end - start).Days + 1;
                PolluteLevelManager plm = new PolluteLevelManager();
                int fineDays = 0;
                Dictionary<int, DictionaryValueItem> ps = DictionaryManager.GetCacheData(FINERATE_CODE);
                rate = 0;
                if (null == ps)
                    return 0;
                List<int> psList = new List<int>();
                foreach (DictionaryValueItem dvi in ps.Values)
                {
                    int pid = 0;
                    if (int.TryParse(dvi.Key, out pid) && !psList.Contains(pid))
                        psList.Add(pid);
                }
                if (psList.Count == 0)
                    return 0;
                Dictionary<DateTime, CitywideData> dataList = QueryCitywideDaily(start, end, Duration.Day);
                foreach (CitywideData data in dataList.Values)
                {
                    PolluteLevel level = plm.Query(data.API, true);
                    if (null != level)
                    {
                        if (psList.Contains(level.LevelID))
                            fineDays++;
                    }
                }
                rate = (float)Math.Round(((decimal)fineDays / dayCount) * 100, 1);
                return fineDays;
            }
            catch { throw; }
        }
        /// <summary>
        /// 获取城市空气到某时间d2为止的优良天数、总天数、优良率
        /// </summary>
        /// <param name="d2"></param>
        /// <param name="fineDays"></param>
        /// <param name="dayCount"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public int getFineDayRate(DateTime d2, out int fineDays, out int dayCount, out float rate)
        {
            try
            {
                DateTime start, end;
                start = new DateTime(d2.Year, 1, 1);
                end = new DateTime(d2.Year, d2.Month, d2.Day);
                dayCount = (end - start).Days + 1;
                PolluteLevelManager plm = new PolluteLevelManager();
                fineDays = 0;
                Dictionary<int, DictionaryValueItem> ps = DictionaryManager.GetCacheData(FINERATE_CODE);
                rate = 0;
                if (null == ps)
                    return 0;
                List<int> psList = new List<int>();
                foreach (DictionaryValueItem dvi in ps.Values)
                {
                    int pid = 0;
                    if (int.TryParse(dvi.Key, out pid) && !psList.Contains(pid))
                        psList.Add(pid);
                }
                if (psList.Count == 0)
                    return 0;
                Dictionary<DateTime, CitywideData> dataList = QueryCitywideDaily(start, end, Duration.Day);
                foreach (CitywideData data in dataList.Values)
                {
                    PolluteLevel level = plm.Query(data.API, true);
                    if (null != level)
                    {
                        if (psList.Contains(level.LevelID))
                            fineDays++;
                    }
                }
                rate = (float)Math.Round(((decimal)fineDays / dayCount) * 100, 1);
                return fineDays;
            }
            catch { throw; }
        }
    }
}
