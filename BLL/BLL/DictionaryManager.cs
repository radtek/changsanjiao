using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

using MMShareBLL.Model;
using MMShareBLL.DAL;

namespace  MMShareBLL.BLL
{
    /// <summary>
    /// 提供对于字典数据的管理能力
    /// </summary>
    public class DictionaryManager
    {
        //Dictionary Code
        private static int SP_WEATHERDATA = 1;
        private static int HOURDURATION = 6;
        private static int DAILYDURATION = 10;
        private static int ONE_MINUTEDURATION = 44;
        private static int STATESITE = 5;
        //private static int CANAMOUNTAPIPARAMETER = 9;
        //private static int DMC_DMS_PARAMETER = 19;
        private static int FIXED_SITE_DATA = 20;
        private static int AUTOCOMPLETE_PERSION = 11;
        private static int AUTOCOMPLETE_PARAMETER = 13;
        private static int GISCATETORY = 21;
        private static int VISIBILITY_SITE = 42;
        private static int PROCESSING_PARAMETERS = 14;
        private static int DATABROWSER_CODE = 57;

        private static DictionaryData dal = new DictionaryData();


        static DictionaryManager()
        {
            try
            {
                getCahceCategories();
                getCacheDictionaryValues();
            }
            catch { throw; }
        }

        static Dictionary<int, AQIDictionary> getCahceCategories()
        {
            if (null == CacheManager.Dictioanry.Categories)
            {
                Dictionary<int, AQIDictionary> cacheCategory = new Dictionary<int, AQIDictionary>();
                Collection<AQIDictionary> theData = dal.GetCategory();
                foreach (AQIDictionary dict in theData)
                    cacheCategory[dict.Code] = dict;
                CacheManager.Dictioanry.Categories = cacheCategory;
                return cacheCategory;
            }
            else
                return CacheManager.Dictioanry.Categories;

        }

        static Dictionary<int, Dictionary<int, DictionaryValueItem>> getCacheDictionaryValues()
        {
            if (null == CacheManager.Dictioanry.DataValues)
            {
                Dictionary<int, Dictionary<int, DictionaryValueItem>> theData = dal.GetDictionary();
                CacheManager.Dictioanry.DataValues = theData;
                return theData;
            }
            else
                return CacheManager.Dictioanry.DataValues;
        }


        public Dictionary<int, Dictionary<int, DictionaryValueItem>> GetData()
        {
            try
            {
                return dal.GetDictionary();
            }
            catch { throw; }
        }

        public Dictionary<int, DictionaryValueItem> GetData(int code)
        {
            try
            {
                return dal.GetDictionary(code);
            }
            catch { throw; }
        }

        public bool Remove(int identity)
        {
            try
            {
                bool success = dal.RemoveDictionary(identity);
                if (success)
                    CacheManager.Dictioanry.ClearValues();
                return success;
            }
            catch { throw; }
        }

        /// <summary>
        /// 创建字典数据
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public bool CreateDictionary(DictionaryValueItem dictionary)
        {
            try
            {
                bool success = dal.CreateDictionary(dictionary);
                if (success)
                    CacheManager.Dictioanry.ClearValues();
                return success;
            }
            catch { throw; }
        }

        /// <summary>
        /// 修改字典数据
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public bool UpdateDictionary(DictionaryValueItem dictionary)
        {
            try
            {
                bool success = dal.UpdateDictionary(dictionary);
                if (success)
                    CacheManager.Dictioanry.ClearValues();
                return success;
            }
            catch { throw; }
        }

        /// <summary>
        /// 删除字典数据
        /// </summary>
        /// <param name="valueId"></param>
        /// <returns></returns>
        public bool RemoveDictionary(int valueId)
        {
            try
            {
                bool success = dal.RemoveDictionary(valueId);
                if (success)
                    CacheManager.Dictioanry.ClearValues();
                return success;
            }
            catch { throw; }
        }

        public int RemoveDictionary(int[] valueIdArray)
        {
            try
            {
                int sum = dal.RemoveDictionary(valueIdArray);
                if (sum > 0)
                    CacheManager.Dictioanry.ClearValues();
                return sum;
            }
            catch { throw; }
        }

        public static Dictionary<int, AQIDictionary> GetDictionaryCategory(bool fromCache)
        {
            try
            {
                return getCahceCategories();
            }
            catch { throw; }
        }

        private static Dictionary<int, Dictionary<int, DictionaryValueItem>> CacheData
        {
            get
            {
                return getCacheDictionaryValues();
            }
        }

        public static Dictionary<int, DictionaryValueItem> GetCacheData(int code)
        {
            Dictionary<int, Dictionary<int, DictionaryValueItem>> cacheData = CacheData;
            if (cacheData.ContainsKey(code))
                return cacheData[code];
            return new Dictionary<int, DictionaryValueItem>();
        }

        public static DictionaryValueItem GetDictionaryValue(int valueId)
        {
            Dictionary<int, Dictionary<int, DictionaryValueItem>> cacheData = CacheData;
            foreach (Dictionary<int, DictionaryValueItem> dicCate in cacheData.Values)
            {
                foreach (DictionaryValueItem dv in dicCate.Values)
                {
                    if (dv.ValueID == valueId)
                        return dv.Clone();
                }
            }
            return null;
        }

        public static DictionaryValueItem GetDictionaryValue(int code, int valueId)
        {
            if (CacheData.ContainsKey(code))
            {
                Dictionary<int, Dictionary<int, DictionaryValueItem>> cacheData = CacheData;
                foreach (DictionaryValueItem dv in cacheData[code].Values)
                {
                    if (dv.ValueID == valueId)
                        return dv.Clone();
                }
            }
            return null;
        }

        ///// <summary>
        ///// 获取统计图形"点位气象小时变化图"的参数配置
        ///// </summary>
        ///// <returns></returns>
        //public static Collection<StatisticParamWeatherData> GetSPWeatherData()
        //{
        //    try
        //    {
        //        Collection<StatisticParamWeatherData> theParams = new Collection<StatisticParamWeatherData>();
        //        Dictionary<int, DictionaryValueItem> dictionaries = CacheData[SP_WEATHERDATA]; // dal.GetDictionary(SP_WEATHERDATA);
        //        ParameterManager facManager = new ParameterManager();

        //        foreach (DictionaryValueItem dictionary in dictionaries.Values)
        //        {

        //            int key = 0;
        //            if (!int.TryParse(dictionary.Key, out key))
        //                continue;

        //            StatisticParamWeatherData param = new StatisticParamWeatherData();
        //            param.Code = SP_WEATHERDATA;

        //            string color = dictionary.StandbyField1;
        //            string seriesType = dictionary.Value;

        //            DataParameter ownerFactor = facManager.GetParameter(key, true);
        //            if (ownerFactor == null)
        //                continue;

        //            param.OwnerFactor = ownerFactor;
        //            param.Color = color;
        //            param.SeriesType = seriesType;
        //            param.ShowZeroValue = "1" == dictionary.StandbyField2;
        //            theParams.Add(param);
        //        }
        //        return theParams;
        //    }
        //    catch { throw; }
        //}

        /// <summary>
        /// 取得监测数据小时均值的频率号
        /// </summary>
        /// <returns></returns>
        public static int GetHourDuration()
        {
            return getDurtion(HOURDURATION);
        }

        /// <summary>
        /// 取得监测数据每分钟数值的频率号
        /// </summary>
        /// <returns></returns>
        public static int GetOneMinuteDuration()
        {
            return getDurtion(ONE_MINUTEDURATION);
        }

        /// <summary>
        /// 取得监测数据日均值的频率号
        /// </summary>
        /// <returns></returns>
        public static int GetDailyDuration()
        {
            return getDurtion(DAILYDURATION);
        }

        private static int getDurtion(int key)
        {
            Dictionary<int, Dictionary<int, DictionaryValueItem>> cacheData = CacheData;
            if (cacheData.ContainsKey(key))
            {
                foreach (DictionaryValueItem dict in cacheData[key].Values)
                {
                    int duration = 0;
                    if (!int.TryParse(dict.Value, out duration))
                        return -100;
                    else
                        return duration;
                }
            }
            return -100;
        }

        /// <summary>
        /// 获取DMS国控点的配置
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, DictionaryValueItem> GetStateSite()
        {
            Dictionary<int, DictionaryValueItem> sites = new Dictionary<int, DictionaryValueItem>();
            Dictionary<int, Dictionary<int, DictionaryValueItem>> cacheData = CacheData;
            if (cacheData.ContainsKey(STATESITE))
            {
                foreach (DictionaryValueItem dict in cacheData[STATESITE].Values)
                {
                    int siteID = 0;
                    if (int.TryParse(dict.Value, out siteID))
                    {
                        sites.Add(siteID, dict);
                    }
                }
            }
            return sites;
        }


        ///// <summary>
        ///// 取得用于生成DBF文件的参数数据
        ///// </summary>
        ///// <param name="avgData"></param>
        ///// <returns></returns>
        //public static Dictionary<int, SiteData> GetFixedSiteData(out SiteData avgData)
        //{
        //    avgData = new SiteData();
        //    Dictionary<int, SiteData> theData = new Dictionary<int, SiteData>();
        //    if (CacheData.ContainsKey(FIXED_SITE_DATA))
        //    {
        //        Dictionary<int, Site> stateSite = new Area().GetStateSite(true);
        //        foreach (Site site in stateSite.Values)
        //        {
        //            SiteData sd = new SiteData();
        //            sd.Site = site;
        //            theData.Add(site.Id, sd);
        //        }
        //        Dictionary<int, DictionaryValueItem> configure = CacheData[FIXED_SITE_DATA];
        //        ParameterManager pm = new ParameterManager();
        //        foreach (DictionaryValueItem dict in configure.Values)
        //        {
        //            int siteId = 0;
        //            if (!int.TryParse(dict.Key, out siteId))
        //                continue;
        //            int paramId = 0;
        //            if (!int.TryParse(dict.Value, out paramId))
        //                continue;
        //            float value = 0;
        //            if (!float.TryParse(dict.StandbyField1, out value))
        //                continue;
        //            DataParameter param = pm.GetParameter(paramId, true);
        //            if (null == param || !param.CanComputeApi)
        //                continue;

        //            if (theData.ContainsKey(siteId))
        //            {
        //                MonitoringData md = new MonitoringData();
        //                md.Thickness = value;
        //                md.Factor = param;
        //                theData[siteId].DataCollection.Add(md);
        //                avgData.DataCollection.Add(md.Clone());
        //            }
        //        }

        //        foreach (SiteData siteData in theData.Values)
        //        {
        //            siteData.DataCollection.ChangeToCalaculatedDataMax();
        //            DataCalculate.CalculateApi(siteData);
        //        }

        //        avgData.DataCollection.ChangeToCalculatedDataAvg();
        //        DataCalculate.CalculateApi(avgData);
        //    }
        //    return theData;
        //}

        /// <summary>
        /// 获得综合预报相关人员的配置
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, List<string>> GetAutoCompleteData()
        {
            Dictionary<int, List<string>> theData = new Dictionary<int, List<string>>();
            Dictionary<int, Dictionary<int, DictionaryValueItem>> cacheData = CacheData;

            //综合预报相关人员
            if (cacheData.ContainsKey(AUTOCOMPLETE_PERSION))
                fillDictData(cacheData[AUTOCOMPLETE_PERSION], theData);

            return theData;
        }

        public static Dictionary<int, List<string>> GetParamAutoCompleteData()
        {
            Dictionary<int, List<string>> theData = new Dictionary<int, List<string>>();

            //气象参数
            Dictionary<int, DictionaryValueItem> paramValues = CacheData[AUTOCOMPLETE_PARAMETER];
            foreach (DictionaryValueItem dict in paramValues.Values)
            {
                int key = 0;
                if (!string.IsNullOrEmpty(dict.Value) && int.TryParse(dict.Key, out key))
                {
                    if (!theData.ContainsKey(key))
                        theData.Add(key, new List<string>());
                    theData[key].Add(dict.Value);
                }
            }
            return theData;
        }

        public static Dictionary<int, string> GetGisCategorys()
        {
            Dictionary<int, string> theData = new Dictionary<int, string>();
            Dictionary<int, DictionaryValueItem> gisCategories = CacheData[GISCATETORY];
            foreach (DictionaryValueItem dict in gisCategories.Values)
            {
                int key = 0;
                if (int.TryParse(dict.Key, out key))
                    theData[key] = dict.Value;
            }
            return theData;
        }

        private static void fillDictData(Dictionary<int, DictionaryValueItem> dataSource, Dictionary<int, List<string>> container)
        {
            foreach (DictionaryValueItem dict in dataSource.Values)
            {
                int key = 0;
                if (!int.TryParse(dict.Key, out key))
                    continue;

                if (!container.ContainsKey(key))
                    container[key] = new List<string>();
                container[key].Add(dict.Value);

            }
        }

        public static Dictionary<int, DictionaryValueItem> Append(Dictionary<int, DictionaryValueItem> d1, Dictionary<int, DictionaryValueItem> d2)
        {
            Dictionary<int, DictionaryValueItem> theData = new Dictionary<int, DictionaryValueItem>();
            foreach (int key in d1.Keys)
                theData.Add(key, d1[key]);
            foreach (int key in d2.Keys)
                theData.Add(key, d2[key]);
            return theData;
        }

        public static int[] GetVisibilitySites()
        {
            Dictionary<int, DictionaryValueItem> data = GetCacheData(VISIBILITY_SITE);
            List<int> reList = new List<int>();
            foreach (DictionaryValueItem item in data.Values)
            {
                int siteId = 0;
                if (int.TryParse(item.Key, out siteId))
                    reList.Add(siteId);
            }
            return reList.ToArray();
        }

        public static List<int> GetProcessingParameters()
        {
            Dictionary<int, DictionaryValueItem> data = GetCacheData(PROCESSING_PARAMETERS);
            List<int> reList = new List<int>();
            foreach (DictionaryValueItem item in data.Values)
            {
                int paramId = 0;
                if (int.TryParse(item.Key, out paramId))
                    reList.Add(paramId);
            }
            return reList;
        }

        /// <summary>
        /// 获取数据浏览默认选择的参数
        /// </summary>
        /// <returns></returns>
        public static int GetDataBrowserDefaultParameter()
        {
            Dictionary<int, DictionaryValueItem> data = GetCacheData(DATABROWSER_CODE);
            foreach (DictionaryValueItem dvi in data.Values)
            {
                int defaultParameter = 0;
                int.TryParse(dvi.Key, out defaultParameter);
                return defaultParameter;
            }
            return 0;
        }
    }
}
