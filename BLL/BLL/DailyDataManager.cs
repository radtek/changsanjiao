using System;
using System.Collections.Generic;
using System.Data;
using MMShareBLL.DAL;
using MMShareBLL.Model;


namespace MMShareBLL.BLL
{
    /// <summary>
    /// 提供对监测数据的管理(对DMS数据库的直接查询)
    /// </summary>
    public class DailyDataManager
    {
        private MonitoringDataDal dal;
        public const int FINERATE_CODE = 59;

        public DailyDataManager()
        {
            dal = new MonitoringDataDal();
        }

        public bool UseOneConnection
        {
            get { return dal.UseOneConnection; }
            set { dal.UseOneConnection = value; }
        }


        /// <summary>
        /// 获取监测数据
        /// </summary>
        /// <param name="sites">监测站点</param>
        /// <param name="parameters">污染因子</param>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="duration">监测数据的频率</param>
        /// <param name="calculateApi">是否把获得的数据进行API计算</param>
        /// <returns></returns>
        public Dictionary<DateTime, Dictionary<int,MMShareBLL.Model.SiteData>> GetData(int[] sites, int[] parameters, DateTime startTime, DateTime endTime, int duration, bool calculateApi, DataValidated validated)
        {
            try
            {
                //设置参数：Prameters..
                ParameterManager fac = new ParameterManager();
                Dictionary<int, DataParameter> theParameters = new Dictionary<int, DataParameter>();
                foreach (int parameterID in parameters)
                {
                    DataParameter theFactor = fac.GetParameter(parameterID, true);
                    if (null != theFactor)
                        theParameters[parameterID] = theFactor;
                }

                //设置参数：Sites..
                Area are = new Area();
                Dictionary<int, Site> theSites = new Dictionary<int, Site>();
                foreach (int siteID in sites)
                {
                    Site theSite = are.GetSite(siteID, true);
                    if (null != theSite)
                        theSites[siteID] = theSite;
                }

                return GetData(theSites, theParameters, startTime, endTime, duration, calculateApi, validated);
            }
            catch { throw; }
        }

        public Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> GetData(Dictionary<int, Site> theSites, Dictionary<int, DataParameter> theParameters, DateTime startTime, DateTime endTime, int duration, bool calculateApi, DataValidated validated)
        {
            try
            {
                if (startTime > endTime)
                    return null;

                //如果参数为空，则会查询所有因子，为了性能考虑，这里手动设置为所有因子
                if (null == theParameters || theParameters.Count == 0)
                    theParameters = new ParameterManager().GetParameter(true);

                //获取数据
                Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> theData = dal.GetData(theSites, theParameters, startTime, endTime, duration, validated);

                if (calculateApi)
                {
                    foreach (Dictionary<int, MMShareBLL.Model.SiteData> timeData in theData.Values)
                    {
                        //计算API
                        foreach (MMShareBLL.Model.SiteData siteData in timeData.Values)
                        {
                            DataCalculate.CalculateApi(siteData);
                        }
                    }
                }
                return theData;
            }
            catch { throw; }
        }

        public DataTable GetData(DateTime start, DateTime end, int parameterId, int duration, DataValidated validated)
        {
            try
            {
                return dal.GetData(start, end, parameterId, duration, validated);
            }
            catch { throw; }
        }

        public DataTable GetData(DateTime start, DateTime end, int parameterId, int siteId, int duration, DataValidated validated)
        {
            try
            {
                return dal.GetData(start, end, parameterId, duration, validated);
            }
            catch { throw; }
        }

        public DataTable GetHourData(DateTime time, int parameterId, DataValidated validated)
        {
            try
            {
                int duration = DictionaryManager.GetHourDuration();
                DateTime start = new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0);
                DateTime end = new DateTime(time.Year, time.Month, time.Day, time.Hour, 59, 59);
                return dal.GetData(start, end, parameterId, duration, validated);
            }
            catch { throw; }
        }

        public DataTable GetHourData(DateTime start, DateTime end, int parameterId, int siteId, DataValidated validated)
        {
            try
            {
                int duration = DictionaryManager.GetHourDuration();
                return dal.GetData(start, end, parameterId, siteId, duration, validated);
            }
            catch { throw; }
        }

        public DataTable GetHourData(DateTime start, DateTime end, int[] parameters, int[] sites, DataValidated validated)
        {
            try
            {
                int duration = DictionaryManager.GetHourDuration();
                return dal.GetData(start, end, parameters, sites, duration, validated);
            }
            catch { throw; }
        }

        /// <summary>
        /// 获得小时均值
        /// </summary>
        /// <param name="sites">监测站点</param>
        /// <param name="parameters">污染因子</param>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="calculateApi">是否把获得的数据进行API计算</param>
        /// <param name="validated">数据是否通过验证</param>
        /// <returns></returns>
        public Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> GetHourData(int[] sites, int[] parameters, DateTime startTime, DateTime endTime, bool calculateApi, DataValidated validated)
        {
            try
            {
                int duration = DictionaryManager.GetHourDuration();
                return GetData(sites, parameters, startTime, endTime, duration, calculateApi, validated);
            }
            catch { throw; }
        }


        /// <summary>
        /// 获得小时均值
        /// </summary>
        /// <param name="sites">监测站点</param>
        /// <param name="parameters">污染因子</param>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> GetHourData(int[] sites, int[] parameters, DateTime startTime, DateTime endTime)
        {
            try
            {
                if (startTime > endTime)
                    return null;
                int duration = DictionaryManager.GetHourDuration();
                //设置参数：Prameters..
                ParameterManager fac = new ParameterManager();
                Dictionary<int, DataParameter> theParameters = new Dictionary<int, DataParameter>();
                foreach (int parameterID in parameters)
                {
                    DataParameter theFactor = fac.GetParameter(parameterID, true);
                    if (null != theFactor)
                        theParameters[parameterID] = theFactor;
                }

                //设置参数：Sites..
                Area are = new Area();
                Dictionary<int, Site> theSites = new Dictionary<int, Site>();
                foreach (int siteID in sites)
                {
                    Site theSite = are.GetSite(siteID, true);
                    if (null != theSite)
                        theSites[siteID] = theSite;
                }
                //如果参数为空，则会查询所有因子，为了性能考虑，这里手动设置为所有因子
                if (null == theParameters || theParameters.Count == 0)
                    theParameters = new ParameterManager().GetParameter(true);

                //获取数据
                Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> theData = dal.GetHourData(theSites, theParameters, startTime, endTime, duration);

                return theData;
            }
            catch { throw; }
        }

        public Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> GetHourData(Dictionary<int, Site> sites, Dictionary<int, DataParameter> parameters, DateTime startTime, DateTime endTime, bool calculateApi, DataValidated validated)
        {
            try
            {
                int duration = DictionaryManager.GetHourDuration();
                return GetData(sites, parameters, startTime, endTime, duration, calculateApi, validated);
            }
            catch { throw; }
        }

        /// <summary>
        /// 获得日均值
        /// </summary>
        /// <param name="sites">监测站点</param>
        /// <param name="parameters">污染因子</param>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="calculateApi">是否从缓存中加载数据</param>
        /// <param name="fromCache">是否把获得的数据进行API计算</param>
        /// <returns></returns>
        public Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> GetDailyData(int[] sites, int[] parameters, DateTime startTime, DateTime endTime, bool calculateApi, DataValidated validated)
        {
            try
            {
                int duration = DictionaryManager.GetDailyDuration();
                return GetData(sites, parameters, startTime, endTime, duration, calculateApi, validated);
            }
            catch { throw; }
        }

        public Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> GetDailyData(Dictionary<int, Site> sites, Dictionary<int, DataParameter> parameters, DateTime startTime, DateTime endTime, bool calculateApi, DataValidated validated)
        {
            try
            {
                int duration = DictionaryManager.GetDailyDuration();
                return GetData(sites, parameters, startTime, endTime, duration, calculateApi, validated);
            }
            catch { throw; }
        }

        /// <summary>
        /// 取得某天的全市各污染物API
        /// </summary>
        /// <param name="day">日期</param>
        /// <returns></returns>
        public MMShareBLL.Model.SiteData GetCityWideApi(DateTime day, DataValidated validated)
        {
            PeriodOfTime pot = DataCalculate.GetData_TimeRange(day);

            SiteGroupManager sgm = new SiteGroupManager();
            SiteGroup sg = sgm.QueryCitywideGroup(true);
            List<int> cList = new List<int>();

            foreach (Site site in sg.Items)
            {
                if (site.SiteType == SiteType.National)
                {
                    cList.Add(site.Id);
                }
            }
            int[] siteArray = new int[cList.Count];
            cList.CopyTo(siteArray, 0);


            //取得所有国控点的日均值
            Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataSource = GetHourData(siteArray, new int[] { }, pot.Start, pot.End, false, validated);

            Dictionary<int, MMShareBLL.Model.SiteData> theData = DataCalculate.AmalgamateTimeSiteData(dataSource);

            foreach (MMShareBLL.Model.SiteData siteData in theData.Values)
            {
                siteData.DataCollection.ChangeToCalaculatedDataAuto();
            }

            MMShareBLL.Model.SiteData cityData = DataCalculate.AmalgamateSiteData(theData);

            //计算平均
            cityData.DataCollection.ChangeToCalculatedDataAvg();
            cityData.Date = new PeriodOfTime();
            cityData.Date.Value = day;
            //计算API
            DataCalculate.CalculateApi(cityData);
            cityData.Site = new Site();
            cityData.Site.Name = "全市";
            return cityData;

        }



        /// <summary>
        /// 根据小时数据获得全市API
        /// </summary>
        /// <param name="startDatetime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        public MMShareBLL.Model.SiteData GetCityWideApi(DateTime startDatetime, DateTime endDateTime, DataValidated validated)
        {
            Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataSource = GetStateSiteHourData(startDatetime, endDateTime, false, validated);
            Dictionary<int, MMShareBLL.Model.SiteData> source2 = DataCalculate.AmalgamateTimeSiteData(dataSource);
            foreach (MMShareBLL.Model.SiteData sd in source2.Values)
                sd.DataCollection.ChangeToCalaculatedDataAuto();
            MMShareBLL.Model.SiteData theData = DataCalculate.AmalgamateSiteData(source2);
            theData.DataCollection.ChangeToCalaculatedDataAuto();
            DataCalculate.CalculateApi(theData);
            return theData;
        }

        /// <summary>
        /// 获取各国控点在某一天各污染因子的日均值
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public Dictionary<int, MMShareBLL.Model.SiteData> GetStateSiteDaily(DateTime day, DataValidated validated)
        {
            try
            {


                SiteGroupManager sgm = new SiteGroupManager();
                SiteGroup cGroup = sgm.QueryCitywideGroup(true);

                //Dictionary<int, Site> stateSites = new Area().GetStateSiteDMS(true);
                Dictionary<int, Site> stateSites = new Dictionary<int, Site>();
                foreach (Site site in cGroup.Items)
                    stateSites[site.Id] = site;
                return GetSitesDaily(day, validated, stateSites);
            }
            catch { throw; }

        }

        public Dictionary<int, MMShareBLL.Model.SiteData> GetSitesDaily(DateTime day, DataValidated validated, Dictionary<int, Site> siteList)
        {
            try
            {
                PeriodOfTime pot = DataCalculate.GetData_TimeRange(day);
                //取得所有国控点的日均值，结构：时间→站点→污染因子→监测数据

                Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataSource = GetHourData(siteList, new Dictionary<int, DataParameter>(), pot.Start, pot.End, false, validated);

                //合并站点数据，合并之后会出现重复的数据
                Dictionary<int, MMShareBLL.Model.SiteData> theData = DataCalculate.AmalgamateTimeSiteData(dataSource);

                //处理数据，并去掉重复的数据，去掉重复时提供两种策略：平均和取最大。此处硬编码为平均
                foreach (MMShareBLL.Model.SiteData siteData in theData.Values)
                {
                    //把每个站点下的不同污染因子的重复的数据给平均掉
                    siteData.DataCollection.ChangeToCalculatedDataAvg();
                    DataCalculate.CalculateApi(siteData);
                }
                //如果没有查询到数据，则填充空白
                if (theData.Values.Count == 0)
                {
                    foreach (Site site in siteList.Values)
                    {
                        MMShareBLL.Model.SiteData emptyData = new MMShareBLL.Model.SiteData();
                        emptyData.Site = site;
                        emptyData.Date = new PeriodOfTime();
                        emptyData.Date.Value = day;
                        theData[site.Id] = emptyData;
                    }
                }

                //现在的数据就是某天各国控点的监测数据了
                return theData;
            }
            catch { throw; }
        }

        /// <summary>
        /// 取得国控点的小时数据
        /// </summary>
        /// <param name="day">日期</param>
        /// <param name="calculateApi">是否计算API</param>
        /// <returns></returns>
        public Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> GetStateSiteHourData(DateTime day, bool calculateApi, DataValidated validated)
        {
            try
            {
                DateTime startTime = DateTime.Parse(day.ToString("yyyy-MM-dd") + " 00:00:00");
                DateTime endTime = DateTime.Parse(day.ToString("yyyy-MM-dd") + " 23:59:59");
                return GetStateSiteHourData(startTime, endTime, calculateApi, validated);
            }
            catch { throw; }
        }

        public Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> GetStateSiteHourData(DateTime startTime, DateTime endTime, bool calculateApi, DataValidated validated)
        {
            try
            {
                Dictionary<int, Site> stateSites = new Area().GetStateSite(true);
                Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> theData = GetHourData(stateSites, new Dictionary<int, DataParameter>(), startTime, endTime, calculateApi, validated);
                return theData;
            }
            catch { throw; }
        }

        public Dictionary<int, MMShareBLL.Model.SiteData> GetStateSiteHourData(DateTime time, DataValidated validated)
        {
            Dictionary<int, Site> stateSites = new Area().GetStateSite(true);
            time = new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0);
            Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataSource = GetHourData(stateSites, new Dictionary<int, DataParameter>(), time, time, false, validated);
            if (dataSource.ContainsKey(time))
            {
                Dictionary<int, MMShareBLL.Model.SiteData> theData = dataSource[time];
                foreach (MMShareBLL.Model.SiteData siteData in theData.Values)
                    DataCalculate.CalculateApi(siteData);
                return theData;
            }
            else
                return new Dictionary<int, MMShareBLL.Model.SiteData>();
        }

        /// <summary>
        /// 获得某一区的日均值
        /// </summary>
        public MMShareBLL.Model.SiteData GetCountyDaily(DateTime day, int countyId, DataValidated validated)
        {
            try
            {
                day = new DateTime(day.Year, day.Month, day.Day, 0, 0, 0);

                MMShareBLL.Model.SiteData theData = null;
                if (CacheManager.DailyData.HasCountyDailyAvgData(countyId, day))
                    theData = CacheManager.DailyData.Day_County_Avg_Data[countyId][day.Ticks];
                if (null == theData)
                {
                    //DateTime startTime = new DateTime(day.Year, day.Month, day.Day, 0, 0, 0);
                    //DateTime endTime = new DateTime(day.Year, day.Month, day.Day, 23, 59, 59);

                    PeriodOfTime pot = DataCalculate.GetData_TimeRange(day);

                    Area areaManager = new Area();
                    County county = areaManager.GetCounty(countyId, true);
                    if (null == county)
                        return new MMShareBLL.Model.SiteData();
                    int[] sites = county.SiteIds;
                    Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataSource = GetHourData(sites, new int[] { }, pot.Start, pot.End, false, validated);

                    //合并站点数据
                    theData = DataCalculate.AmalgamateSiteData(dataSource);

                    //处理数据，计算平均
                    theData.DataCollection.ChangeToCalculatedDataAvg();
                    theData.Site = new Site();
                    theData.Site.Name = county.Name;
                    theData.Site.Id = county.Id;
                    //再计算API
                    DataCalculate.CalculateApi(theData);

                    CacheManager.DailyData.SetCountyDailyAvgData(countyId, day, theData);
                }
                return theData;
            }
            catch { throw; }
        }

        /// <summary>
        /// 区得某一区的日均值
        /// </summary>
        /// <returns></returns>
        public MMShareBLL.Model.SiteData GetCountyDaily(DateTime day, string groupId, DataValidated validated)
        {
            PeriodOfTime pot = DataCalculate.GetData_TimeRange(day);

            Area areaManager = new Area();
            SiteGroup gc = new SiteGroupManager().Query(groupId, true);
            if (null == gc)
                return new MMShareBLL.Model.SiteData();
            int[] sites = new int[gc.Items.Count];
            for (int i = 0; i < sites.Length; i++)
                sites[i] = gc.Items[i].Id;
            Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataSource = GetHourData(sites, new int[] { }, pot.Start, pot.End, false, validated);

            //合并站点数据
            MMShareBLL.Model.SiteData theData = DataCalculate.AmalgamateSiteData(dataSource);

            //处理数据，计算平均
            theData.DataCollection.ChangeToCalculatedDataAvg();
            //再计算API
            DataCalculate.CalculateApi(theData);
            return theData;
        }

        /// <summary>
        /// 取得某日不同区县的日均值
        /// </summary>
        /// <param name="day">时间</param>
        /// <returns></returns>
        public Dictionary<County, MMShareBLL.Model.SiteData> GetCountyDaily(DateTime day, DataValidated validated)
        {
            try
            {
                Area area = new Area();
                Dictionary<int, County> counties = area.GetCounty(true);

                Dictionary<County, MMShareBLL.Model.SiteData> theData = new Dictionary<County, MMShareBLL.Model.SiteData>();

                foreach (County county in counties.Values)
                    theData[county] = GetCountyDaily(day, county.Id, validated);

                return theData;
            }
            catch { throw; }
        }

        public Dictionary<County, MMShareBLL.Model.SiteData> GetCountyDaily(DateTime day, bool machiningData, DataValidated validated)
        {
            if (!machiningData)
                return GetCountyDaily(day, validated);

            List<SiteGroup> counties = new CountyGroupManager().Query();
            Dictionary<County, MMShareBLL.Model.SiteData> theData = new Dictionary<County, MMShareBLL.Model.SiteData>();

            foreach (SiteGroup group in counties)
            {
                County county = new County(group.ID, group.GroupName);
                theData[county] = GetCountyDaily(day, group.GroupID, validated);
            }
            return theData;
        }

        /// <summary>
        /// 获得一段时间内，某一区的小时值
        /// </summary>
        public Dictionary<DateTime, MMShareBLL.Model.SiteData> GetCountyHourData(DateTime startDay, DateTime endDay, int countyId, DataValidated validated)
        {
            try
            {
                Area areaManager = new Area();
                County county = areaManager.GetCounty(countyId, true);
                if (null == county)
                    return new Dictionary<DateTime, MMShareBLL.Model.SiteData>();
                int[] sites = county.SiteIds;
                //startDay = new DateTime(startDay.Year, startDay.Month, startDay.Day, 0, 0, 0);
                //endDay = new DateTime(endDay.Year, endDay.Month, endDay.Day, 23, 59, 59);
                Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataSource = GetHourData(sites, new int[] { }, startDay, endDay, true, validated);
                Dictionary<DateTime, MMShareBLL.Model.SiteData> timeData = DataCalculate.AmalgamateDataByTime(dataSource);
                foreach (MMShareBLL.Model.SiteData data in timeData.Values)
                {
                    data.DataCollection.ChangeToCalaculatedDataAuto();
                    DataCalculate.CalculateApi(data);
                }
                return timeData;
            }
            catch { throw; }
        }

        public Dictionary<DateTime, MMShareBLL.Model.SiteData> GetCountyHourData(DateTime startTime, DateTime endTime, int countyId, int[] parameters, bool calcApi, DataValidated validated)
        {
            try
            {
                Area areaManager = new Area();
                County county = areaManager.GetCounty(countyId, true);
                if (null == county)
                    return new Dictionary<DateTime, MMShareBLL.Model.SiteData>();
                int[] sites = county.SiteIds;
                Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataSource = GetHourData(sites, parameters, startTime, endTime, false, validated);
                Dictionary<DateTime, MMShareBLL.Model.SiteData> timeData = DataCalculate.AmalgamateDataByTime(dataSource);
                foreach (MMShareBLL.Model.SiteData data in timeData.Values)
                {
                    data.DataCollection.ChangeToCalaculatedDataAuto();
                    if (calcApi)
                        DataCalculate.CalculateApi(data);
                }
                return timeData;
            }
            catch { throw; }
        }

        public MMShareBLL.Model.SiteData GetCountyHourData(DateTime time, int countyId, DataValidated validated)
        {
            try
            {
                Area areaManager = new Area();
                County county = areaManager.GetCounty(countyId, true);
                if (null == county)
                    return null;
                int[] sites = county.SiteIds;
                time = new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0);
                Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataSource = GetHourData(sites, new int[] { }, time, time, true, validated);
                Dictionary<int, MMShareBLL.Model.SiteData> countyData = DataCalculate.AmalgamateTimeSiteData(dataSource);
                foreach (MMShareBLL.Model.SiteData sd in countyData.Values)
                    sd.DataCollection.ChangeToCalaculatedDataAuto();
                MMShareBLL.Model.SiteData theCountyData = DataCalculate.AmalgamateSiteData(countyData);
                theCountyData.DataCollection.ChangeToCalaculatedDataAuto();
                theCountyData.Site = new Site();
                theCountyData.Site.Id = countyId;
                theCountyData.Site.Name = county.Name;

                DataCalculate.CalculateApi(theCountyData);
                return theCountyData;
            }
            catch { throw; }
        }

        /// <summary>
        /// 获得某一站点的小时数据
        /// </summary>
        public Dictionary<DateTime, MMShareBLL.Model.SiteData> GetSiteHourData(DateTime startDay, DateTime endDay, int siteId, DataValidated validated)
        {
            try
            {
                startDay = new DateTime(startDay.Year, startDay.Month, startDay.Day, 0, 0, 0);
                endDay = new DateTime(endDay.Year, endDay.Month, endDay.Day, 23, 59, 59);
                return GetSiteHourData(startDay, endDay, siteId, new int[] { }, validated);
            }
            catch { throw; }
        }

        public Dictionary<DateTime, MMShareBLL.Model.SiteData> GetSiteHourData(DateTime startDay, DateTime endDay, int siteId, int[] parameters, DataValidated validated)
        {
            try
            {
                Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataSource = GetHourData(new int[] { siteId }, parameters, startDay, endDay, false, validated);
                Dictionary<DateTime, MMShareBLL.Model.SiteData> timeData = DataCalculate.AmalgamateDataByTime(dataSource);
                Site theSite = new Area().GetSite(siteId, true);
                foreach (MMShareBLL.Model.SiteData data in timeData.Values)
                {
                    data.DataCollection.ChangeToCalaculatedDataAuto();
                    DataCalculate.CalculateApi(data);
                    data.Site = theSite;
                }
                return timeData;
            }
            catch { throw; }
        }


        public MMShareBLL.Model.SiteData GetSiteData(DateTime startDay, DateTime endDay, int siteId, int[] parameters, bool calculateApi, DataValidated validated)
        {
            try
            {
                Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataSource = GetHourData(new int[] { siteId }, parameters, startDay, endDay, false, validated);
                MMShareBLL.Model.SiteData theData = DataCalculate.AmalgamateSiteData(dataSource);
                theData.DataCollection.ChangeToCalaculatedDataAuto();
                if (calculateApi)
                    DataCalculate.CalculateApi(theData);
                return theData;
            }
            catch { throw; }
        }

        /// <summary>
        /// 获取某个站点组在某一时段的小时数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<DateTime, MMShareBLL.Model.SiteData> GetGroupHourData(DateTime startTime, DateTime endTime, string groupId, int[] parameters, bool calcApi, DataValidated validated)
        {
            try
            {
                SiteGroupManager sgm = new SiteGroupManager();
                SiteGroup group = sgm.Query(groupId);
                if (null == group)
                    return new Dictionary<DateTime, MMShareBLL.Model.SiteData>();
                Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataSource = GetHourData(group.ItemIdentities, parameters, startTime, endTime, false, validated);
                Dictionary<DateTime, MMShareBLL.Model.SiteData> timeData = DataCalculate.AmalgamateDataByTime(dataSource);
                foreach (MMShareBLL.Model.SiteData data in timeData.Values)
                {
                    data.DataCollection.ChangeToCalaculatedDataAuto();
                    if (calcApi)
                        DataCalculate.CalculateApi(data);
                }
                return timeData;
            }
            catch { throw; }
        }

        public MMShareBLL.Model.SiteData GetSiteHourData(DateTime time, int siteId, DataValidated validated)
        {
            try
            {
                Site site = new Area().GetSite(siteId, true);
                if (null == site)
                    return null;
                time = new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0);
                Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataSource = GetHourData(new int[] { siteId }, new int[] { }, time, time, false, validated);
                MMShareBLL.Model.SiteData theSiteData = DataCalculate.AmalgamateSiteData(dataSource);
                theSiteData.DataCollection.ChangeToCalaculatedDataAuto();
                theSiteData.Site = site;
                DataCalculate.CalculateApi(theSiteData);
                return theSiteData;
            }
            catch { throw; }
        }

        /// <summary>
        /// 获得某一站点的日均值
        /// </summary>
        public MMShareBLL.Model.SiteData GetSiteDaily(DateTime day, int siteId, bool calculateApi, DataValidated validated)
        {
            try
            {
                //DateTime startTime = new DateTime(day.Year, day.Month, day.Day, 0, 0, 0);
                //DateTime endTime = new DateTime(day.Year, day.Month, day.Day, 23, 59, 59);

                PeriodOfTime pot = DataCalculate.GetData_TimeRange(day);

                Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataSource = GetHourData(new int[] { siteId }, new int[] { }, pot.Start, pot.End, false, validated);
                MMShareBLL.Model.SiteData siteDailyData = DataCalculate.AmalgamateSiteData(dataSource);
                siteDailyData.DataCollection.ChangeToCalaculatedDataAuto();
                if (calculateApi)
                    DataCalculate.CalculateApi(siteDailyData);
                siteDailyData.Site = new Area().GetSite(siteId, true);
                return siteDailyData;
            }
            catch { throw; }
        }

        /// <summary>
        /// 获取某一段时间内，全市在这段时间各小时的API情况
        /// </summary>
        public Dictionary<DateTime, MMShareBLL.Model.SiteData> GetCityWideHourApi(DateTime startTime, DateTime endTime, DataValidated validated)
        {
            try
            {
                //取得在这段时间内，各国控点的监测数据
                Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataSource = GetHourData(new Area().GetStateSitesNoReference(), null, startTime, endTime, true, validated);
                //合并数据，把同一时间内，各国控点的数据进行合并
                Dictionary<DateTime, MMShareBLL.Model.SiteData> theData = DataCalculate.AmalgamateDataByTime(dataSource);

                //处理合并之后的数据
                foreach (MMShareBLL.Model.SiteData data in theData.Values)
                {
                    //计算平均
                    data.DataCollection.ChangeToCalculatedDataAvg();
                    //计算API
                    DataCalculate.CalculateApi(data);
                }

                return theData;
            }
            catch { throw; }
        }

        public Dictionary<DateTime, MMShareBLL.Model.SiteData> GetCityWideHourData(DateTime startTime, DateTime endTime, int[] parameters, bool calculateApi, DataValidated validated)
        {
            try
            {
                Dictionary<int, DataParameter> ps = new Dictionary<int, DataParameter>();
                ParameterManager pm = new ParameterManager();
                foreach (int parameterID in parameters)
                {
                     DataParameter theFactor = pm.GetParameter(parameterID, true);
                    if (null != theFactor)
                        ps[parameterID] = theFactor;
                }

                //取得在这段时间内，各国控点的监测数据
                Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataSource = GetHourData(new Area().GetStateSitesNoReference(), ps, startTime, endTime, calculateApi, validated);
                //合并数据，把同一时间内，各国控点的数据进行合并
                Dictionary<DateTime, MMShareBLL.Model.SiteData> theData = DataCalculate.AmalgamateDataByTime(dataSource);

                //处理合并之后的数据
                foreach (MMShareBLL.Model.SiteData data in theData.Values)
                {
                    //计算平均
                    data.DataCollection.ChangeToCalculatedDataAvg();
                    //计算API
                    DataCalculate.CalculateApi(data);
                }

                return theData;
            }
            catch { throw; }
        }

        /// <summary>
        /// 取得分区小时均值
        /// </summary>
        /// <param name="counties"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <param name="parameters"></param>
        /// <param name="calculateApi"></param>
        /// <returns></returns>
        //public Dictionary<DateTime, Dictionary<int, SiteData>> GetCountyHourData(int[] counties, DateTime startDateTime, DateTime endDateTime, int[] parameters, bool calculateApi, DataValidated validated)
        //{
        //    Dictionary<DateTime, Dictionary<int, SiteData>> theData = new Dictionary<DateTime, Dictionary<int, SiteData>>();
        //    Area areaManager = new Area();
        //    Dictionary<int, County> countyList = areaManager.GetCounty(true);
        //    foreach (int countyId in counties)
        //    {
        //        if (countyList.ContainsKey(countyId))
        //        {
        //            County theCounty = countyList[countyId];
        //            setAreaGroupData(theCounty, theCounty.Id, startDateTime, endDateTime, parameters, calculateApi, theData, validated);
        //        }
        //    }
        //    return theData;
        //}

        //public Dictionary<DateTime, Dictionary<int, SiteData>> GetGroupHourData(int[] groups, DateTime startDateTime, DateTime endDateTime, int[] parameters, bool calculateApi, DataValidated validated)
        //{
        //    Dictionary<DateTime, Dictionary<int, SiteData>> theData = new Dictionary<DateTime, Dictionary<int, SiteData>>();
        //    SiteGroupManager sgm = new SiteGroupManager();
        //    foreach (int groupIdentity in groups)
        //    {
        //        SiteGroup sg = sgm.Query(groupIdentity, true);
        //        if (null != sg)
        //            setAreaGroupData(sg, sg.ID, startDateTime, endDateTime, parameters, calculateApi, theData, validated);
        //    }
        //    return theData;
        //}

        //private void setAreaGroupData(IGroup ig, int identity, DateTime startDateTime, DateTime endDateTime, int[] parameters, bool calculateApi, Dictionary<DateTime, Dictionary<int, SiteData>> theData, DataValidated validated)
        //{

        //    Dictionary<DateTime, Dictionary<int, SiteData>> data = GetHourData(ig.ItemIdentities, parameters, startDateTime, endDateTime, calculateApi, validated);
        //    Dictionary<DateTime, SiteData> countyData = DataCalculate.AmalgamateDataByTime(data);

        //    foreach (SiteData siteData in countyData.Values)
        //    {
        //        siteData.Site = new Site();
        //        siteData.Site.Name = ig.Name;
        //        siteData.DataCollection.ChangeToCalaculatedDataAuto();
        //    }

        //    foreach (DateTime timeKey in countyData.Keys)
        //    {
        //        Dictionary<int, SiteData> timeData = null;
        //        if (theData.ContainsKey(timeKey))
        //            timeData = theData[timeKey];
        //        else
        //        {
        //            timeData = new Dictionary<int, SiteData>();
        //            theData.Add(timeKey, timeData);
        //        }
        //        SiteData hourCountyData = countyData[timeKey];
        //        timeData.Add(identity, hourCountyData);
        //    }
        //}

        public DateTime? GetLastDataHour()
        {
            try
            {
                return dal.GetLastDataHour(DictionaryManager.GetHourDuration());
            }
            catch { throw; }
        }

        public MMShareBLL.Model.SiteData GetLastData()
        {
            DateTime? lastDataHour = GetLastDataHour();
            if (null == lastDataHour)
                return null;
            DateTime startTime = new DateTime(lastDataHour.Value.Year, lastDataHour.Value.Month, lastDataHour.Value.Day, lastDataHour.Value.Hour, 0, 0);
            DateTime endTime = new DateTime(lastDataHour.Value.Year, lastDataHour.Value.Month, lastDataHour.Value.Day, lastDataHour.Value.Hour, 59, 59);
            MMShareBLL.Model.SiteData theData = GetCityWideApi(startTime, endTime, DataValidated.Vaidated);
            return theData;
        }

        //public DataTable GetDataBrowser(DateTime d1, DateTime d2)
        //{
        //    try
        //    {
        //        DailyDataAccess dda = new DailyDataAccess();
        //        return dda.GetDataBrowserData(d1, d2);
        //    }
        //    catch { throw; }
        //}

        //public DataTable GetDataBrowser(DateTime d1, DateTime d2,string sites,string pars)
        //{
        //    try
        //    {
        //        DailyAQIDataAccess dda = new DailyAQIDataAccess();
        //        return dda.GetDataBrowserData(d1, d2, sites, pars);
        //    }
        //    catch { throw; }
        //}

        public DataTable GetAQIData(DateTime d1, DateTime d2, string sites, string pars,int duration,DataValidated dv)
        {
            try
            {

                return dal.GetAQIData(d1, d2, sites, pars, 10, DataValidated.All);
            }
            catch { throw; }
        }



    }
}
