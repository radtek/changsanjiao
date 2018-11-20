using System;
using System.Collections.Generic;
using MMShareBLL.Model;
using MMShareBLL.DAL;

namespace MMShareBLL.BLL
{
    /// <summary>
    /// 提供对于数值预报数据的相关计算操作
    /// </summary>
    public sealed class DataCalculate
    {
        private DataCalculate() { }

        private const int DAILY_DATERANGE_CODE = 34;

        //Cache Key
        private const string FORECAST_PARAMTERS = "FORECAST_PARAMTERS";
        private const string CACHEKEY_DAY_STARTHOUR = "DAILY_DATA_STARTHOUR";
        private const string CACHEKEY_DAY_ENDHOUR = "DAILY_DATA_ENDHOUR";

        private const int FORECAST_DBF_PARAM_CODE = 53;

        private static readonly int[] apiLevel = new int[] { 50, 100, 200, 300, 400, 500 };
        private static readonly int[] apiH8Level = new int[] { 50, 100, 200, 300 };

        /// <summary>
        /// 计算API
        /// </summary>
        /// <param name="thincknessRange">与API分级中相对应的浓度列表</param>
        /// <param name="thinckness">污染物浓度</param>
        /// <returns></returns>
        public static int CalculateApi(float[] thincknessRange, float thinckness)
        {
            try
            {
                int[] api = new int[] { 50, 100, 200, 300, 400, 500 };
                thinckness = (float)Math.Round(thinckness, 3);
                float C = thinckness;

                float result = 0.0f;

                int length = api.Length;

                float leastthincknessRange = thincknessRange[0];
                float mostthincknessRange = thincknessRange[length - 1];

                float leastApi = api[0];
                float mostApi = api[length - 1];

                // 处理小于最小标准值，和大于最大标准值的情况
                if (C < leastthincknessRange)
                {
                    result = leastApi * (C / leastthincknessRange);
                    goto CalcEnd;
                }

                if (C > mostthincknessRange)
                {
                    //如果超过标准值，则返回API最大值
                    //原有按比例计算方式：mostApi * C / mostthincknessRange;
                    result = mostApi;
                    goto CalcEnd;
                }

                // 处理正好等于标准值的情况
                for (int i = 0; i < length; i++)
                {
                    if (thincknessRange[i] == C)
                    {
                        result = api[i];
                        goto CalcEnd;
                    }
                }

                // 处理在数值区间的情况
                int pos = -1;
                for (int i = 0; i < length; i++)
                {
                    if (i <= length - 2)
                    {
                        if (C > thincknessRange[i] && C < thincknessRange[i + 1])
                        {
                            pos = i;
                            break;
                        }
                    }
                }
                if (pos != -1)
                {
                    int I_Small = api[pos];
                    int I_Big = api[pos + 1];

                    float C_Small = thincknessRange[pos];
                    float C_Big = thincknessRange[pos + 1];

                    result = (I_Big - I_Small) * (C - C_Small) / (C_Big - C_Small) + I_Small;
                    goto CalcEnd;
                }

            CalcEnd:
                {
                    //double floorValue = Math.Floor(result);
                    //if (result != floorValue && (floorValue + 1) < 500)
                    //    return (int)floorValue + 1;
                    result = (float)Math.Ceiling(Math.Round(result, 3));
                    if (result > 500)
                        result = 500;
                    return Convert.ToInt32(result);

                }
            }
            catch
            {
                //log.....
                return -1010101;
            }
        }

        #region 计算滑动8小时的API
        /// <summary>
        /// 计算滑动8小时的API
        /// </summary>
        /// <param name="thincknessRange"></param>
        /// <param name="thinckness"></param>
        /// <returns></returns>
        public static int CalculateH8Api(float[] thincknessRange, float thinckness)
        {
            try
            {
                int[] api = new int[] { 50, 100, 200, 300 };
                thinckness = (float)Math.Round(thinckness, 3);
                float C = thinckness;

                float result = 0.0f;

                int length = api.Length;

                float leastthincknessRange = thincknessRange[0];
                float mostthincknessRange = thincknessRange[length - 1];

                float leastApi = api[0];
                float mostApi = api[length - 1];

                // 处理小于最小标准值，和大于最大标准值的情况
                if (C < leastthincknessRange)
                {
                    result = leastApi * (C / leastthincknessRange);
                    goto CalcEnd;
                }

                if (C > mostthincknessRange)
                {
                    //如果超过标准值，则返回API最大值
                    //原有按比例计算方式：mostApi * C / mostthincknessRange;
                    result = mostApi;
                    goto CalcEnd;
                }

                // 处理正好等于标准值的情况
                for (int i = 0; i < length; i++)
                {
                    if (thincknessRange[i] == C)
                    {
                        result = api[i];
                        goto CalcEnd;
                    }
                }

                // 处理在数值区间的情况
                int pos = -1;
                for (int i = 0; i < length; i++)
                {
                    if (i <= length - 2)
                    {
                        if (C > thincknessRange[i] && C < thincknessRange[i + 1])
                        {
                            pos = i;
                            break;
                        }
                    }
                }
                if (pos != -1)
                {
                    int I_Small = api[pos];
                    int I_Big = api[pos + 1];

                    float C_Small = thincknessRange[pos];
                    float C_Big = thincknessRange[pos + 1];

                    result = (I_Big - I_Small) * (C - C_Small) / (C_Big - C_Small) + I_Small;
                    goto CalcEnd;
                }

            CalcEnd:
                {
                    //double floorValue = Math.Floor(result);
                    //if (result != floorValue && (floorValue + 1) < 500)
                    //    return (int)floorValue + 1;
                    result = (float)Math.Ceiling(Math.Round(result, 3));
                    if (result > 300)
                        result = 300;
                    return Convert.ToInt32(result);

                }
            }
            catch
            {
                //log.....
                return -1010101;
            }
        }
        #endregion


        /// <summary>
        /// 反算浓度
        /// </summary>
        /// <param name="thincknessRange">与API分级中相对应的浓度列表</param>
        /// <param name="api">API</param>
        /// <returns></returns>
        public static decimal InverseThinckness(float[] thincknessRange, int api)
        {
            int[] apiRange = new int[] { 0, 0 };
            decimal[] tRange = new decimal[] { 0, 0 };
            for (var i = 0; i < 6; i++)
            {
                if (api < apiLevel[i])
                {
                    apiRange[1] = apiLevel[i];
                    tRange[1] = new decimal(thincknessRange[i]);
                    if (i > 0)
                    {
                        apiRange[0] = apiLevel[i - 1];
                        tRange[0] = new decimal(thincknessRange[i - 1]);
                    }
                    break;
                }
                else if (api == apiLevel[i])
                    return new decimal(thincknessRange[i]);
            }
            decimal tR = tRange[1] - tRange[0];
            decimal aR = new decimal(apiRange[1] - apiRange[0]);
            decimal sR = new decimal(api - apiRange[0]);
            return sR * tR / aR + tRange[0];
        }

        public static PeriodOfTime GetData_TimeRange(DateTime day)
        {
            int startHour = 0, endHour = 0;
            bool maySetCache = false;
            if (!CacheManager.Contains(CACHEKEY_DAY_STARTHOUR) || !int.TryParse(CacheManager.GetData(CACHEKEY_DAY_STARTHOUR) as string, out startHour))
                maySetCache = true;
            if (!CacheManager.Contains(CACHEKEY_DAY_ENDHOUR) || !int.TryParse(CacheManager.GetData(CACHEKEY_DAY_ENDHOUR) as string, out endHour))
                maySetCache = true;
            if (maySetCache)
            {
                startHour = -12;
                endHour = 11;
                Dictionary<int, DictionaryValueItem> dictData = DictionaryManager.GetCacheData(DAILY_DATERANGE_CODE);
                if (null != dictData)
                {
                    int key = 0;
                    int value = 0;
                    bool useConfig = false;
                    foreach (DictionaryValueItem dict in dictData.Values)
                    {
                        if (int.TryParse(dict.Key, out key) && int.TryParse(dict.Value, out value))
                        {
                            useConfig = true;
                            break;
                        }
                    }
                    if (useConfig)
                    {
                        startHour = key;
                        endHour = value;
                        CacheManager.SetData(CACHEKEY_DAY_STARTHOUR, startHour);
                        CacheManager.SetData(CACHEKEY_DAY_ENDHOUR, endHour);
                    }
                }
            }
            DateTime startDateTime = new DateTime(day.Year, day.Month, day.Day, 0, 0, 0);
            DateTime endDateTime = new DateTime(day.Year, day.Month, day.Day, 0, 59, 59);
            endDateTime = endDateTime.AddHours(endHour);
            startDateTime = startDateTime.AddHours(startHour);
            if (day.Hour >= startDateTime.Hour)
            {
                startDateTime = new DateTime(day.Year, day.Month, day.Day, Math.Abs(startHour), 0, 0);
                endDateTime = endDateTime.AddDays(1);
            }

            PeriodOfTime dateRange = new PeriodOfTime();
            dateRange.Date = new DateTime(day.Year, day.Month, day.Day);
            if (day.Hour > endHour)
            {
                dateRange.Date = dateRange.Date.AddDays(1);
            }


            dateRange.Value = day;
            dateRange.Start = startDateTime;
            dateRange.End = endDateTime;

            return dateRange;
        }

        public static PeriodOfTime GetData_TimeRange(DateTime day, Duration duration)
        {
            PeriodOfTime t = new PeriodOfTime();
            if (duration == Duration.Day)
            {
                day = new DateTime(day.Year, day.Month, day.Day);
                return GetData_TimeRange(day);
            }
            else if (duration == Duration.Month)
            {
                t.Start = new DateTime(day.Year, day.Month, 1);
                DateTime nextMonth = day.AddMonths(1);
                t.End = nextMonth.AddDays(-1);
                //DateTime nextMonth = new DateTime(day.Year, day.Month + 1, 1);
                //t.End = new DateTime(day.Year, day.Month, nextMonth.AddDays(-1).Day);
                t.Value = t.Start;
            }
            else if (duration == Duration.Quarter)
            {
                int sm = getQuarterStartMonth(day);
                t.Start = new DateTime(day.Year, sm, 1);
                DateTime tempDate = day.AddMonths(3);
                DateTime t_end = new DateTime(tempDate.Year, tempDate.Month, 1);
                t.End = t_end.AddDays(-1);
                t.Value = t.Start;
            }
            else if (duration == Duration.HalfYear)
            {
 
            }
            else if (duration == Duration.Year)
            {
                t.Start = new DateTime(day.Year, 1, 1);
                t.End = new DateTime(day.Year, 12, 31);
                t.Value = t.Start;
            }
            return t;
        }

        public static DateTime ReParseDateTime(DateTime time, Duration duration, bool last)
        {
            if (last)
            {
                switch (duration)
                {
                    case Duration.Hour: time = new DateTime(time.Year, time.Month, time.Day, time.Hour, 59, 59); break;
                    case Duration.Day: time = new DateTime(time.Year, time.Month, time.Day, 23, 59, 59); break;
                    case Duration.Month:
                        {
                            time = new DateTime(time.Year, time.Month, 1, 23, 59, 59);
                            time = time.AddMonths(1);
                            time = time.AddDays(-1);
                            break;
                        }
                    case Duration.Quarter:
                        {
                            time = new DateTime(time.Year, getQuarterEndMonth(time), 1, 23, 59, 59);
                            time = time.AddMonths(1);
                            time = time.AddDays(-1);
                            break;
                        }
                    case Duration.HalfYear:
                        {
                            time = new DateTime(time.Year, time.Month, 1, 23, 59, 59);
                            time = time.AddMonths(1);
                            time = time.AddDays(-1);
                            break;
                        }
                    case Duration.Year: time = new DateTime(time.Year, 12, 31, 23, 59, 59); break;
                }
            }
            else
            {
                switch (duration)
                {
                    case Duration.Hour: time = new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0); break;
                    case Duration.Day: time = new DateTime(time.Year, time.Month, time.Day); break;
                    case Duration.Month: time = new DateTime(time.Year, time.Month, 1); break;
                    case Duration.Quarter: time = new DateTime(time.Year, getQuarterStartMonth(time), 1); break;
                    case Duration.HalfYear: time = new DateTime(time.Year, time.Month, time.Day); break;
                    case Duration.Year: time = new DateTime(time.Year, 1, 1); break;
                }
            }
            return time;
        }

        public static int getDurationIdFromDMCToDMS(Duration durationId)
        {
            int result = 0;
            switch (durationId)
            {
                case Duration.Hour: result = 10; break;
                case Duration.Day: result = 11; break;
                //case Duration.Month: ; break;
                //case Duration.Quarter: ; break;
                //case Duration.Year: ; break;
            }
            return result;
        }

        private static int getQuarterStartMonth(DateTime datetime)
        {
            int month = datetime.Month;
            return ((int)((month - 1) / 3)) * 3 + 1;
        }

        private static int getQuarterEndMonth(DateTime datetime)
        {
            int month = datetime.Month;
            if (month % 3 == 0)
                return month;
            return datetime.AddMonths((3 - month % 3)).Month;
        }

        //private static int getHalfYearStartMonth(DateTime datetime)
        //{
            
        //}

        private static int getHalfYearEndMonth(DateTime datetime)
        {
            int month = datetime.Month;
            if (month <= 6)
                return 6;
            else
                return 12;
        }

        /// <summary>
        /// 计算某一站点的空气污染指数
        /// </summary>
        /// <param name="siteData"></param>
        public static void CalculateApi(DataCollectionBase siteData)
        {
            int maxApi = 0;
            int siteApi = 0;
            foreach (MonitoringData parameterData in siteData.DataCollection)
            {
                if (parameterData.Factor.CanComputeApi)
                {
                    int api = DataCalculate.CalculateApi(parameterData.Factor.ConcentrationLimits, parameterData.Thickness);
                    if (api > maxApi && parameterData.Factor.CalcCitywideApi)
                        maxApi = api;
                    if (parameterData.Factor.CalcCitywideApi && api > siteApi)
                        siteApi = api;
                    parameterData.API = api;
                }
            }
            siteData.DataCollection.MaxApi = maxApi;
            siteData.DataCollection.Api = siteApi;
        }

        /// <summary>
        /// 提供一组站点数据，把这些站点的所有监测数据整合到一个站点里，也就是说，合并这些站点数据
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <returns></returns>
        public static MMShareBLL.Model.SiteData AmalgamateSiteData(Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataSource)
        {
            MMShareBLL.Model.SiteData avgData = new MMShareBLL.Model.SiteData();
            Dictionary<int, int> counter = new Dictionary<int, int>();

            foreach (Dictionary<int, MMShareBLL.Model.SiteData> value in dataSource.Values)
            {
                foreach (MMShareBLL.Model.SiteData siteData in value.Values)
                {
                    foreach (MonitoringData paramData in siteData.DataCollection)
                    {
                        int factorId = paramData.Factor.Id;
                        //if (!avgData.DataCollection.Contains(factorId))
                        avgData.DataCollection.Add(paramData.Clone());
                    }
                }
            }
            return avgData;
        }

        /// <summary>
        /// 合并站点数据
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static MMShareBLL.Model.SiteData AmalgamateSiteData(Dictionary<int, MMShareBLL.Model.SiteData> dataSource)
        {
            MMShareBLL.Model.SiteData theData = new MMShareBLL.Model.SiteData();
            foreach (MMShareBLL.Model.SiteData data in dataSource.Values)
            {
                foreach (MonitoringData d in data.DataCollection)
                    theData.DataCollection.Add(d);
            }
            return theData;
        }

        /// <summary>
        /// 合并站点数据，把不同时间的站点数据合并到一个站点里
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <returns></returns>
        public static Dictionary<int, MMShareBLL.Model.SiteData> AmalgamateTimeSiteData(Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataSource)
        {
            Dictionary<int, MMShareBLL.Model.SiteData> theData = new Dictionary<int, MMShareBLL.Model.SiteData>();

            foreach (Dictionary<int, MMShareBLL.Model.SiteData> timeData in dataSource.Values)
            {
                foreach (MMShareBLL.Model.SiteData data in timeData.Values)
                {
                    if (theData.ContainsKey(data.Site.Id))
                    {
                        MMShareBLL.Model.SiteData sumData = theData[data.Site.Id];
                        foreach (MonitoringData md in data.DataCollection)
                            sumData.DataCollection.Add(md);
                    }
                    else
                        theData[data.Site.Id] = data;
                }
            }
            return theData;
        }

        public static Dictionary<int, MMShareBLL.Model.SiteData> AmalgamateSiteTimeData(Dictionary<int, Dictionary<DateTime, MMShareBLL.Model.SiteData>> dataSource)
        {
            Dictionary<int, MMShareBLL.Model.SiteData> theData = new Dictionary<int, MMShareBLL.Model.SiteData>();
            foreach (int siteId in dataSource.Keys)
            {
                Dictionary<DateTime, MMShareBLL.Model.SiteData> siteData = dataSource[siteId];

                foreach (MMShareBLL.Model.SiteData data in siteData.Values)
                {
                    if (theData.ContainsKey(siteId))
                    {
                        MMShareBLL.Model.SiteData sumData = theData[siteId];
                        foreach (MonitoringData md in data.DataCollection)
                            sumData.DataCollection.Add(md);
                    }
                    else
                        theData[siteId] = data;
                }
            }
            return theData;
        }

        /// <summary>
        /// 合并数据：把同一时间内，不同站点的数据进行合并
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static Dictionary<DateTime, MMShareBLL.Model.SiteData> AmalgamateDataByTime(Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> dataSource)
        {
            Dictionary<DateTime, MMShareBLL.Model.SiteData> theData = new Dictionary<DateTime, MMShareBLL.Model.SiteData>();
            foreach (DateTime time in dataSource.Keys)
            {
                MMShareBLL.Model.SiteData hiData = null;
                if (theData.ContainsKey(time))
                    hiData = theData[time];
                else
                {
                    hiData = new MMShareBLL.Model.SiteData();
                    theData[time] = hiData;
                }
                foreach (MMShareBLL.Model.SiteData siteData in dataSource[time].Values)
                {
                    foreach (MonitoringData md in siteData.DataCollection)
                        hiData.DataCollection.Add(md);
                }
            }
            return theData;
        }

        /// <summary>
        /// 合并两各数据源的数据
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> AmalgamateData(Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> source1, Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> source2)
        {
            Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>> theData = new Dictionary<DateTime, Dictionary<int, MMShareBLL.Model.SiteData>>();
            foreach (DateTime time in source1.Keys)
            {
                theData[time] = source1[time];
            }
            foreach (DateTime time in source2.Keys)
            {
                Dictionary<int, MMShareBLL.Model.SiteData> data2 = source2[time];
                if (theData.ContainsKey(time))
                {
                    Dictionary<int, MMShareBLL.Model.SiteData> theSiteDataList = theData[time];
                    foreach (int siteId in data2.Keys)
                    {
                        MMShareBLL.Model.SiteData siteData2 = data2[siteId];
                        if (theSiteDataList.ContainsKey(siteId))
                        {
                            MMShareBLL.Model.SiteData siteData1 = theSiteDataList[siteId];
                            foreach (MonitoringData md in siteData2.DataCollection)
                                siteData1.DataCollection.Add(md);
                        }
                        else
                            theSiteDataList[siteId] = siteData2;
                    }
                }
                else
                    theData[time] = data2;
            }
            return theData;
        }

        /// <summary>
        /// 合并时间数据
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static MMShareBLL.Model.SiteData AmalgamateTimeData(Dictionary<DateTime, MMShareBLL.Model.SiteData> dataSource)
        {
            MMShareBLL.Model.SiteData theData = new MMShareBLL.Model.SiteData();
            foreach (MMShareBLL.Model.SiteData siteData in dataSource.Values)
            {
                foreach (MonitoringData data in siteData.DataCollection)
                    theData.DataCollection.Add(data);
            }
            return theData;
        }

        public static DataCollectionBase AmalgamateTimeData(Dictionary<DateTime, DataCollectionBase> dataSource)
        {
            DataCollectionBase theData = new DataCollectionBase();
            foreach (DataCollectionBase siteData in dataSource.Values)
            {
                foreach (MonitoringData data in siteData.DataCollection)
                    theData.DataCollection.Add(data);
            }
            return theData;
        }


        public static Dictionary<DateTime, MMShareBLL.Model.SiteData> ParseToSiteData(Dictionary<DateTime, ForecastSiteData> dataSource)
        {
            Dictionary<DateTime, MMShareBLL.Model.SiteData> theData = new Dictionary<DateTime, MMShareBLL.Model.SiteData>();
            foreach (DateTime key in dataSource.Keys)
                theData.Add(key, dataSource[key]);
            return theData;
        }

        public static string BuildTimeString(DateTime time, Duration duration)
        {
            switch (duration)
            {
                case Duration.Hour: return time.ToString("yyyy-MM-dd HH:00");
                case Duration.Day: return time.ToString("yyyy-MM-dd");
                case Duration.Month: return time.ToString("yyyy-MM");
                case Duration.Quarter: return time.ToString("yyyy") + "第" + ((time.Month - 1) / 3 + 1) + "季";
                case Duration.Year: return time.ToString("yyyy年");
            }
            return time.ToString();
        }

        public static int GetReportFooterDayNum(DateTime t)
        {
            t = new DateTime(t.Year, t.Month, t.Day);
            if (t < new DateTime(t.Year, 7, 1))
                return (int)(t - (new DateTime(t.Year, 1, 1))).TotalDays + 1;
            else
                return (int)(t - (new DateTime(t.Year, 7, 1))).TotalDays + 1;
        }

        public static int[] CalulateApiRange(int api)
        {
            int[] apiAmbit = new int[] { 50, 100, 200, 300, 400, 500 };
            if (api == 0)
                return new int[] { 0, 0 };
            if (api > 500)
                return new int[] { 0, 0 };
            var floor = api - 10;
            var ceil = api + 10;
            if (floor <= 0)
                floor = 1;
            //if (floor <= 0)
            //    return new int[] { 1, 21 };
            //int r1 = 0, r2 = 0;
            //for (var i = 0; i < apiAmbit.Length; i++)
            //{
            //    var ambit = apiAmbit[i];
            //    if (api == ambit)
            //        return new int[] { ambit, ambit + 20 };
            //    if (api > ambit)
            //        r1 = ambit;
            //    if (api < ambit)
            //    {
            //        r2 = ambit;
            //        break;
            //    }
            //}
            //if (floor <= r1)
            //{
            //    var j = r1 - floor + 1;
            //    floor += j;
            //    ceil += j;
            //}
            //if (ceil >= r2)
            //{
            //    var j = ceil - r2;
            //    floor -= j;
            //    ceil -= j;
            //}
            return new int[] { floor, ceil };
        }
        ///// <summary>
        ///// 根据API值计算对应API范围及等级范围
        ///// </summary>
        ///// <param name="api">预报API值</param>
        ///// <returns>string[2]：第一个是API范围，第二个是等级范围</returns>
        //public static string[] CalulateApiRangeAndGradeRange(int api)
        //{
        //    if (api > 0)
        //    {
        //        PolluteLevelManager pl = new PolluteLevelManager();
        //        string apiStr = "";
        //        string gradeStr = "";
        //        int[] aRange = CalulateApiRange(api);
        //        PolluteLevel pLevel1 = pl.Query(aRange[0], true);
        //        PolluteLevel pLevel2 = pl.Query(aRange[1], true);
        //        if (null != pLevel1 && null != pLevel2)
        //        {
        //            if (pLevel1.LevelID == pLevel2.LevelID)
        //            {
        //                gradeStr = pLevel1.Grade;
        //            }
        //            else
        //            {
        //                gradeStr = pLevel1.Grade + "到" + pLevel2.Grade;
        //            }
        //        }
        //        else
        //            gradeStr = "未知";
        //        apiStr = aRange[0].ToString() + "-" + aRange[1];
        //        return new string[] { apiStr, gradeStr };
        //    }
        //    return new string[] { "未知", "未知" };
        //}
        /// <summary>
        /// 根据预测各时段API反算综合预报API
        /// </summary>
        /// <param name="AfternoonAPI">预测下午API</param>
        /// <param name="TonightAPI">预测夜间API</param>
        /// <param name="TomorrowPMAPI">预测上午API</param>
        /// <returns></returns>
        public static int periodForeacastAPIToCalculateAPI(int AfternoonAPI, int TonightAPI, int TomorrowPMAPI)
        {
            if (AfternoonAPI <= 0 || TonightAPI <= 0 || TomorrowPMAPI <= 0)
            {
                return 0;
            }
            else
            {
                Dictionary<int, DataParameter> parameters = GetDbfParameters();
                decimal afternoonInverValue = InverseThinckness(parameters[7].ConcentrationLimits, AfternoonAPI);//按PM10反算浓度
                decimal tonightAPIInverValue = InverseThinckness(parameters[7].ConcentrationLimits, TonightAPI);
                decimal tomorrowPMAPIInverValue = InverseThinckness(parameters[7].ConcentrationLimits, TomorrowPMAPI);
                decimal value = (afternoonInverValue + tonightAPIInverValue * 2 + tomorrowPMAPIInverValue) / 4;
                return CalculateApi(parameters[7].ConcentrationLimits, (float)value);
            }
        }

        private static Dictionary<int, DataParameter> GetDbfParameters()
        {
            Dictionary<int, DataParameter> theParam = null;
            if (CacheManager.Contains(FORECAST_PARAMTERS))
                theParam = CacheManager.GetData(FORECAST_PARAMTERS) as Dictionary<int, DataParameter>;
            if (null == theParam)
            {
                Dictionary<int, DictionaryValueItem> dictData = DictionaryManager.GetCacheData(FORECAST_DBF_PARAM_CODE);
                theParam = new Dictionary<int, DataParameter>();
                if (null != dictData)
                {
                    ParameterManager pm = new ParameterManager();
                    foreach (DictionaryValueItem dvi in dictData.Values)
                    {
                        int key = 0;
                        if (int.TryParse(dvi.Key, out key) && !theParam.ContainsKey(key))
                        {
                            DataParameter param = pm.GetParameter(key, true);
                            if (null != param && param.CanComputeApi)
                            {
                                DataParameter newParam = param.Clone();
                                if (!string.IsNullOrEmpty(dvi.Value))
                                    newParam.Name = dvi.Value.ToString();
                                theParam.Add(key, newParam);
                            }
                        }
                    }
                    CacheManager.SetData(FORECAST_PARAMTERS, theParam);
                }
            }
            return theParam;
        }
    }

}
