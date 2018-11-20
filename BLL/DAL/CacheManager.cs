using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using MMShareBLL.Model;
using MMShareBLL.BLL;

namespace MMShareBLL.DAL
{
    /// <summary>
    /// 提供应用程序缓存管理
    /// </summary>
    public sealed class CacheManager
    {
        private class CacheData
        {
            public Object Data;

            public DateTime Time = DateTime.Now;

            public CacheData(object data)
            {
                this.Data = data;
            }
        }

        private static Dictionary<string, CacheData> dataContainer = new Dictionary<string, CacheData>();


        //提供几个强类型的缓存

        /// <summary>
        /// 日报部分，强类型的缓存数据  !!现已没使用该缓存
        /// </summary>
        public static class DailyData
        {
            //各区的月均值
            //Dictionary<区编号, Dictionary<时间的Ticks, 月均值>>
            private static Dictionary<int, Dictionary<double, MMShareBLL.Model.SiteData>> monthCountyAvgData = new Dictionary<int, Dictionary<double, MMShareBLL.Model.SiteData>>();

            //各区的日均值
            //Dictionary<区编号, Dictionary<时间的Ticks, 日均值>>
            private static Dictionary<int, Dictionary<double, MMShareBLL.Model.SiteData>> dayCountyAvgData = new Dictionary<int, Dictionary<double, MMShareBLL.Model.SiteData>>();

            //各区各月份各种污染等级的天数
            //Dictionary<区编号, Dictionary<时间的Ticks, Dictionary<污染等级编号, 污染天数>>>
            private static Dictionary<int, Dictionary<double, Dictionary<int, int>>> monthCountyPolluteLevelDays = new Dictionary<int, Dictionary<double, Dictionary<int, int>>>();

            public static Dictionary<int, Dictionary<double, MMShareBLL.Model.SiteData>> Month_County_Avg_Data
            {
                get { return monthCountyAvgData; }
            }

            public static Dictionary<int, Dictionary<double, MMShareBLL.Model.SiteData>> Day_County_Avg_Data
            {
                get { return dayCountyAvgData; }
            }

            public static Dictionary<int, Dictionary<double, Dictionary<int, int>>> Month_County_PolluteLevel_Days
            {
                get { return monthCountyPolluteLevelDays; }
            }

            public static bool HasMonthCountyPolluteLevelDays(int countyId, double dateTicks, int polluteLevel)
            {
                return monthCountyPolluteLevelDays.ContainsKey(countyId) && monthCountyPolluteLevelDays[countyId].ContainsKey(dateTicks) && monthCountyPolluteLevelDays[countyId][dateTicks].ContainsKey(polluteLevel);
            }
            public static void SetCountyMonthPolluteLevelDays(int countyId, double dateTicks, int polluteLevel, int days)
            {
                Dictionary<double, Dictionary<int, int>> countyDays = null;
                if (monthCountyPolluteLevelDays.ContainsKey(countyId))
                    countyDays = monthCountyPolluteLevelDays[countyId];
                else
                {
                    countyDays = new Dictionary<double, Dictionary<int, int>>();
                    monthCountyPolluteLevelDays.Add(countyId, countyDays);
                }
                Dictionary<int, int> levelDays = null;
                if (countyDays.ContainsKey(dateTicks))
                    levelDays = countyDays[dateTicks];
                else
                {
                    levelDays = new Dictionary<int, int>();
                    countyDays.Add(dateTicks, levelDays);
                }
                levelDays[polluteLevel] = days;
            }

            public static bool HasCountyMonthAvgData(int countyId, double dateTicks)
            {
                return monthCountyAvgData.ContainsKey(countyId) && monthCountyAvgData[countyId].ContainsKey(dateTicks);
            }
            public static void SetCountyMonthAvgData(int countyId, double dateTicks, MMShareBLL.Model.SiteData data)
            {
                Dictionary<double,MMShareBLL.Model.SiteData> countyAvgData = null;
                if (monthCountyAvgData.ContainsKey(countyId))
                    countyAvgData = monthCountyAvgData[countyId];
                if (null == countyAvgData)
                {
                    countyAvgData = new Dictionary<double, MMShareBLL.Model.SiteData>();
                    monthCountyAvgData.Add(countyId, countyAvgData);
                }
                countyAvgData[dateTicks] = data;
            }

            public static bool HasCountyDailyAvgData(int countyId, DateTime day)
            {
                day = new DateTime(day.Year, day.Month, day.Day, 0, 0, 0);
                return HasCountyDailyAvgData(countyId, day.Ticks);
            }
            public static bool HasCountyDailyAvgData(int countyId, double dateTicks)
            {
                return dayCountyAvgData.ContainsKey(countyId) && dayCountyAvgData[countyId].ContainsKey(dateTicks);
            }
            public static void SetCountyDailyAvgData(int countyId, DateTime day, MMShareBLL.Model.SiteData data)
            {
                day = new DateTime(day.Year, day.Month, day.Day, 0, 0, 0);
                SetCountyDailyAvgData(countyId, day.Ticks, data);
            }
            public static void SetCountyDailyAvgData(int countyId, double dateTicks, MMShareBLL.Model.SiteData data)
            {
                Dictionary<double, MMShareBLL.Model.SiteData> countyAvgData = null;
                if (dayCountyAvgData.ContainsKey(countyId))
                    countyAvgData = dayCountyAvgData[countyId];
                if (null == countyAvgData)
                {
                    countyAvgData = new Dictionary<double, MMShareBLL.Model.SiteData>();
                    dayCountyAvgData.Add(countyId, countyAvgData);
                }
                countyAvgData[dateTicks] = data;
            }

            public static Dictionary<int, DataTable> VisibilityData = new Dictionary<int, DataTable>();
        }

        public static class Dictioanry
        {
            public static DateTime? DataValues_CacheTime = null;
            public static DateTime? Categories_CacheTime = null;
            public static DateTime? QueryCacheData_CacheTime = null;

            private static Dictionary<int, Dictionary<int, DictionaryValueItem>> dataValues = null;

            public static Dictionary<int, Dictionary<int, DictionaryValueItem>> DataValues
            {
                get { return Dictioanry.dataValues; }
                set
                {
                    if (SystemVariable.EnabledCache)
                    {
                        Dictioanry.dataValues = value;
                        DataValues_CacheTime = DateTime.Now;
                    }
                }
            }

            private static Dictionary<int, AQIDictionary> categories = null;

            public static Dictionary<int, AQIDictionary> Categories
            {
                get { return Dictioanry.categories; }
                set
                {
                    if (SystemVariable.EnabledCache)
                    {
                        Dictioanry.categories = value;
                        Categories_CacheTime = DateTime.Now;
                    }
                }
            }

            private static Dictionary<int, Collection<AQIDictionary>> quyerCacheData = null;

            public static Dictionary<int, Collection<AQIDictionary>> QuyerCacheData
            {
                get { return Dictioanry.quyerCacheData; }
                set
                {
                    if (SystemVariable.EnabledCache)
                    {
                        Dictioanry.quyerCacheData = value;
                        QueryCacheData_CacheTime = DateTime.Now;
                    }
                }
            }

            public static void ClearValues()
            {
                DataValues = null;
                DataValues_CacheTime = null;
            }

            public static void ClearCategories()
            {
                Categories = null;
                Categories_CacheTime = null;
            }

            public static void ClearQueryCacheData()
            {
                quyerCacheData = null;
                QueryCacheData_CacheTime = null;
            }
        }

        /// <summary>
        /// WEB资源数据
        /// </summary>
        public static class OperationTypeCache
        {
            private static List<OperationType> relationData = null;

            public static List<OperationType> RelationData
            {
                get { return OperationTypeCache.relationData; }
                set
                {
                    if (SystemVariable.EnabledCache)
                    {
                        OperationTypeCache.relationData = value;
                        CacheTime = DateTime.Now;
                    }
                }
            }

            public static DateTime? CacheTime;

            public static void Clear()
            {
                relationData = null;
                CacheTime = null;
            }
        }

        public static object GetData(string key)
        {
            if (string.IsNullOrEmpty(key) || !SystemVariable.EnabledCache)
                return null;

            object value = null;
            try
            {
                value = dataContainer[key].Data;
            }
            catch { }
            return value;
        }

        public static void SetData(string key, object value)
        {
            if (SystemVariable.EnabledCache)
            {
                CacheData cd = new CacheData(value);
                dataContainer[key] = cd;
            }
        }

        public static void Clear()
        {
            dataContainer.Clear();
            Dictioanry.ClearValues();
            Dictioanry.ClearCategories();
            Dictioanry.ClearQueryCacheData();
            OperationTypeCache.Clear();
        }

        public static void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                return;
            switch (key)
            {
                case "DICTIONARY_VALUES": Dictioanry.ClearValues(); break;
                case "DICTIONARY_CATEGORIES": Dictioanry.ClearCategories(); break;
                case "DICTIONARY_QUERY_DATA": Dictioanry.ClearQueryCacheData(); break;
                case "WEBRESOURCE_LIST": OperationTypeCache.Clear(); break;
                default: dataContainer.Remove(key); break;
            }

        }

        public static bool Contains(string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;
            return dataContainer.ContainsKey(key);
        }

        public static DataTable ImperativeTypeData
        {
            get
            {
                DataTable table = buildBaseTable();
                DataRow row1 = table.NewRow();
                row1[0] = "DICTIONARY_VALUES";
                if (null != Dictioanry.DataValues)
                    row1[1] = Dictioanry.DataValues.GetType().ToString();
                else
                    row1[1] = "NULL";
                row1[2] = Dictioanry.DataValues_CacheTime;
                table.Rows.Add(row1);
                DataRow row2 = table.NewRow();
                row2[0] = "DICTIONARY_CATEGORIES";
                if (null != Dictioanry.Categories)
                    row2[1] = Dictioanry.Categories.GetType().ToString();
                else
                    row2[1] = "NULL";
                row2[2] = Dictioanry.Categories_CacheTime;
                table.Rows.Add(row2);
                DataRow row3 = table.NewRow();
                row3[0] = "DICTIONARY_QUERY_DATA";
                if (null != Dictioanry.QuyerCacheData)
                    row3[1] = Dictioanry.QuyerCacheData.GetType().ToString();
                else
                    row3[1] = "NULL";
                row3[2] = Dictioanry.QueryCacheData_CacheTime;
                table.Rows.Add(row3);
                DataRow row4 = table.NewRow();
                row4[0] = "WEBRESOURCE_LIST";
                if (null != OperationTypeCache.RelationData)
                    row4[1] = OperationTypeCache.RelationData.GetType().ToString();
                else
                    row4[1] = "NULL";
                row4[2] = OperationTypeCache.CacheTime;
                table.Rows.Add(row4);
                return table;
            }
        }


        public static DataTable CacheDataTalbe
        {
            get
            {
                DataTable table = buildBaseTable();
                foreach (string key in dataContainer.Keys)
                {
                    DataRow newRow = table.NewRow();
                    CacheData cd = dataContainer[key];
                    newRow[0] = key;
                    if (null != cd.Data)
                        newRow[1] = cd.Data.ToString();
                    else
                        newRow[1] = "NULL";
                    newRow[2] = cd.Time;
                    table.Rows.Add(newRow);
                }
                return table;
            }
        }

        private static DataTable buildBaseTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Key");
            table.Columns.Add("Data");
            table.Columns.Add("CacheTime");
            return table;
        }
    }
}
