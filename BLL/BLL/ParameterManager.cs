using System;
using System.Collections.Generic;
using MMShareBLL.DAL;
using MMShareBLL.Model;


namespace MMShareBLL.BLL
{
    /// <summary>
    /// 提供对染污因子的管理
    /// </summary>
    public class ParameterManager
    {
        //cache keys..

        private const string CACHEKEY_PARAMETER = "DMS_PARAMETER_";
        private const string CACHEKEY_DMS_PARAMETER_LIST = "DMS_PARAMETER_LIST";
        private const string CACHEKEY_DMS_CANCALCAPI_PARAMS = "DMS_CANCALCAPI_PARAMS";
        private const string CACHEKEY_SHAREAXIS_PARAMETER_LIST = "SHAREAXIS_PARAMETER_LIST";

        private const string CACHEKEY_PARAM_INGESTCODE = "CACHEKEY_PARAM_INGESTCODE_";

        private const int DMS_DICTIONARY_CODE = 9;
        private const int DMC_PARAMETERMAPPING = 35;
        private const int DMS_CITYWIDEDATAPARAMETERS = 14;
        private const int DMS_CITYWIDEAQIDATAPARAMETERS = 100;
        private const int PARAMETER_SHAREAXIS = 86;
        
        private const string DAILY_DBF_STATESITEDAY_PARAMETER = "DAILY_STATESITEDAY_DBF_PARAMETER";
        private const string DAILY_DBF_CITYWIDEHOUR_PARAMETER = "DAILY_CITYWIDEHOU_DBF_PARAMETER";
        private const string DAILY_DBF_STATESITE_OZONE_PARAMETER = "DAILY_STATESITE_OZONE_DBF_PARAMETER";
        private const string DAILY_DBF_STATESITE_OZONE_NODATA_PARAM = "DAILY_STATESITE_OZONE_DBF_NODATE_PARAMETER";
        private const string DAILY_DBF_CHANGSHANJIAO_R = "DAILY_CHANGSHANJIAO_R_PARAMETER";
        private const string DAILY_DBF_CHANGSHANJIAO_F = "DAILY_CHANGSHANJIAO_F_PARAMETER";
        private const string FORECAST_DBF_STATESITE_PARAMETER = "FORECAST_STATESITE_DBF_PARAMETER";

        private const int DICTIONARY_CODE_STATESITEDAY = 28;

        private const int DICTIONARY_CODE_CITYWIDEHOUR = 48;

        private const int DICTIONARY_CODE_STATESITE_OZONE = 49;

        private const int DICTIONARY_CODE_FORECAST_STATESITEDATA = 53;

        private const int DICTIONARY_CODE_CHANGSHANJIAO_R = 60;

        private const int DICTIONARY_CODE_CHANGSHANJIAO_F = 61;

        private FactorDataManager dal;
        private ParameterAdapter paramAdapter = new ParameterAdapter();

        public ParameterManager()
        {
            dal = new FactorDataManager();
        }

        /// <summary>
        /// 根据编号获取污染因子(DMS)
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="fromCache">是否从缓存中加载</param>
        /// <returns></returns>
        public DataParameter GetParameter(int id, bool fromCache)
        {
            try
            {
                DataParameter theData = null;
                string cacheKey = CACHEKEY_PARAMETER + id;
                if (fromCache && CacheManager.Contains(cacheKey))
                    theData = CacheManager.GetData(cacheKey) as DataParameter;
                if (theData == null)
                {
                    if (id != -8)
                        theData = dal.QueryParameter(id);
                    else
                    {
                        theData = dal.QueryParameter(8);
                        theData.Id = -8;
                        theData.Name = "O3(8H)";
                        theData.Byname = "O3(8H)";
                    }
                    theData = paramAdapter.FillSetting(theData);
                    if (theData != null)
                        CacheManager.SetData(cacheKey, theData.Clone());
                }
                return theData;
            }
            catch { throw; }
        }

        public DataParameter GetParameter(string ingestCode, bool fromCache)
        {
            try
            {
                DataParameter param = null;
                string cacheKey = CACHEKEY_PARAM_INGESTCODE + ingestCode;
                if (fromCache && CacheManager.Contains(cacheKey))
                    param = CacheManager.GetData(cacheKey) as DataParameter;
                if (null == param)
                {
                    param = dal.QueryParameter(ingestCode);
                    CacheManager.SetData(cacheKey, param);
                }
                return param;
            }
            catch { throw; }
        }

        /// <summary>
        /// 得到DMS污染因子列表
        /// </summary>
        /// <param name="fromCache">是否从缓存中加载</param>
        /// <returns></returns>
        public Dictionary<int, DataParameter> GetParameter(bool fromCache)
        {
            try
            {
                Dictionary<int, DataParameter> theData = null;
                if (fromCache)
                    theData = CacheManager.GetData(CACHEKEY_DMS_PARAMETER_LIST) as Dictionary<int, DataParameter>;
                if (null == theData)
                {
                    theData = dal.QueryParameter();
                    theData = paramAdapter.Sort(theData, true);
                    if (fromCache)
                        CacheManager.SetData(CACHEKEY_DMS_PARAMETER_LIST, theData);
                }
                return theData;
            }
            catch { throw; }
        }

        /// <summary>
        /// 从缓存中获取DMS可计算API的污染因子列表
        /// </summary>
        /// <returns></returns>
        public List<DataParameter> GetCanCalcApiParameterFromCache()
        {
            try
            {
                List<DataParameter> theData = null;
                theData = CacheManager.GetData(CACHEKEY_DMS_CANCALCAPI_PARAMS) as List<DataParameter>;
                if (null == theData)
                {
                    theData = new List<DataParameter>();
                    Dictionary<int, DataParameter> parameters = GetParameter(true);
                    foreach (DataParameter param in parameters.Values)
                    {
                        if (param.CanComputeApi)
                            theData.Add(param.Clone());
                    }
                    CacheManager.SetData(CACHEKEY_DMS_CANCALCAPI_PARAMS, theData);
                }
                return theData;
            }
            catch { throw; }
        }

        /// <summary>
        /// 获得全市、分区日报默认参数列表
        /// </summary>
        /// <returns></returns>
        public List<DataParameter> GetDailyDataParameters()
        {
            Dictionary<int, DictionaryValueItem> mapping = DictionaryManager.GetCacheData(DMS_CITYWIDEDATAPARAMETERS);
            List<DataParameter> parameters = new List<DataParameter>();
            foreach (DictionaryValueItem dvi in mapping.Values)
            {
                int paramID = 0;
                if (int.TryParse(dvi.Key, out paramID))
                {
                    DataParameter param = GetParameter(paramID, true);
                    if (null != param)
                    {
                        parameters.Add(param);
                    }
                }
            }
            return parameters;
        }

        /// <summary>
        /// 获得全市、分区AQI日报默认参数列表
        /// </summary>
        /// <returns></returns>
        public List<DataParameter> GetDailyAQIDataParameters()
        {
            Dictionary<int, DictionaryValueItem> mapping = DictionaryManager.GetCacheData(DMS_CITYWIDEAQIDATAPARAMETERS);
            List<DictionaryValueItem> dic = new List<DictionaryValueItem>();
            foreach (DictionaryValueItem dvi in mapping.Values)
            {
                dic.Add(dvi);
            }
            dic.Sort(new Comparison<DictionaryValueItem>(delegate(DictionaryValueItem v1, DictionaryValueItem v2)
            {
                int i1, i2;
                if (int.TryParse(v1.StandbyField2, out i1) && int.TryParse(v2.StandbyField2, out i2))
                {
                    return i1 - i2;
                };
                return 0;
            }));
            List<DataParameter> parameters = new List<DataParameter>();
            foreach (DictionaryValueItem dvi in dic)
            {
                int paramID = 0;
                if (int.TryParse(dvi.Key, out paramID))
                {
                    DataParameter param = GetParameter(paramID, true);
                    if (null != param)
                        parameters.Add(param);
                }
            }
            return parameters;
        }

        /// <summary>
        /// 获得加工的参数列表
        /// </summary>
        /// <returns></returns>
        public List<DataParameter> GetMachiningParameters()
        {
            Dictionary<int, DictionaryValueItem> mapping = DictionaryManager.GetCacheData(43);
            List<DataParameter> parameters = new List<DataParameter>();
            foreach (DictionaryValueItem dvi in mapping.Values)
            {
                int paramID = 0;
                if (int.TryParse(dvi.Key, out paramID))
                {
                    DataParameter param = GetParameter(paramID, true);
                    if (null != param)
                        parameters.Add(param);
                }
            }
            return parameters;
        }
        /// <summary>
        /// 得到共轴污染因子列表
        /// </summary>
        /// <param name="fromCache">是否从缓存中加载</param>
        /// <returns></returns>
        public Dictionary<int, string> GetShareAxisParameter(bool fromCache)
        {
            try
            {
                Dictionary<int, string> theData = null;
                if (fromCache)
                    theData = CacheManager.GetData(CACHEKEY_SHAREAXIS_PARAMETER_LIST) as Dictionary<int, string>;
                if (null == theData)
                {
                    theData = new Dictionary<int, string>();
                    Dictionary<int, DictionaryValueItem> dictionary = DictionaryManager.GetCacheData(PARAMETER_SHAREAXIS);
                    foreach (DictionaryValueItem item in dictionary.Values)
                    {
                        int key;
                        if (int.TryParse(item.Key, out key) && !String.IsNullOrEmpty(item.Value))
                        {
                            theData[key] = item.Value;
                        }
                    }
                    if (fromCache)
                        CacheManager.SetData(CACHEKEY_SHAREAXIS_PARAMETER_LIST, theData);
                }
                return theData;
            }
            catch { throw; }
        }
        public List<DataParameter> GetParameters(List<int> paramList)
        {
            if (paramList.Count == 0)
                return new List<DataParameter>();
            List<DataParameter> parameters = new List<DataParameter>();
            foreach (int paramId in paramList)
            {
                DataParameter param = GetParameter(paramId, true);
                if (null == param)
                {
                    param = new DataParameter();
                    param.Id = paramId;
                    param.Name = null;
                }
                parameters.Add(param);
            }
            parameters = new ParameterAdapter().SortList(parameters, false);
            return parameters;
        }

        /// <summary>
        /// 从缓存中获取日报DBF参数列表
        /// </summary>
        /// <returns></returns>
        public static List<DataParameter> GetDbfParameter(ReportDataSource ds)
        {
            List<DataParameter> theParam = null;
            string cacheKey = "";
            int dictCode = 0;
            if (ds == ReportDataSource.Daily_StateSiteDailyData_Dbf)
            {
                cacheKey = DAILY_DBF_STATESITEDAY_PARAMETER;
                dictCode = DICTIONARY_CODE_STATESITEDAY;
            }
            else if (ds == ReportDataSource.Daily_CiwywideHourData_Dbf)
            {
                cacheKey = DAILY_DBF_CITYWIDEHOUR_PARAMETER;
                dictCode = DICTIONARY_CODE_CITYWIDEHOUR;
            }
            else if (ds == ReportDataSource.Daily_Ozone_Dbf || ds == ReportDataSource.Daily_Ozone_Email)
            {
                cacheKey = DAILY_DBF_STATESITE_OZONE_PARAMETER;
                dictCode = DICTIONARY_CODE_STATESITE_OZONE;
            }
            else if (ds == ReportDataSource.Forecast_Dbf)
            {
                cacheKey = FORECAST_DBF_STATESITE_PARAMETER;
                dictCode = DICTIONARY_CODE_FORECAST_STATESITEDATA;
            }
            else if (ds == ReportDataSource.Daily_Changshanjiao_r)
            {
                cacheKey = DAILY_DBF_CHANGSHANJIAO_R;
                dictCode = DICTIONARY_CODE_CHANGSHANJIAO_R;
            }
            else if (ds == ReportDataSource.Daily_Changshanjiao_f)
            {
                cacheKey = DAILY_DBF_CHANGSHANJIAO_F;
                dictCode = DICTIONARY_CODE_CHANGSHANJIAO_F;
            }
            if (CacheManager.Contains(cacheKey))
                theParam = CacheManager.GetData(cacheKey) as List<DataParameter>;

            List<DataParameter> ozoneDontShowValue = new List<DataParameter>();
            if (null == theParam)
            {
                theParam = new List<DataParameter>();
                Dictionary<int, DictionaryValueItem> dictData = DictionaryManager.GetCacheData(dictCode);
                if (null != dictData)
                {
                    ParameterManager pm = new ParameterManager();
                    List<int> paramList = new List<int>();
                    foreach (DictionaryValueItem dvi in dictData.Values)
                    {
                        int paramId = 0;
                        if (int.TryParse(dvi.Key, out paramId) && !paramList.Contains(paramId))
                        {
                            DataParameter param = pm.GetParameter(paramId, true);
                            if (null == param)
                            {
                                param = new DataParameter();
                            }
                            DataParameter newParam = param.Clone();
                            if ("1" != dvi.StandbyField1)
                                ozoneDontShowValue.Add(newParam.Clone());
                            if (!string.IsNullOrEmpty(dvi.Value))
                                newParam.Name = dvi.Value;
                            theParam.Add(newParam);
                            paramList.Add(paramId);
                        }
                    }
                    CacheManager.SetData(cacheKey, theParam);
                    if (ds == ReportDataSource.Daily_Ozone_Dbf)
                        CacheManager.SetData(DAILY_DBF_STATESITE_OZONE_NODATA_PARAM, ozoneDontShowValue);
                }
            }
            return theParam;
        }

        public static List<DataParameter> GetOzoneDbfNoDataParameters()
        {
            List<DataParameter> theList = null;
            if (CacheManager.Contains(DAILY_DBF_STATESITE_OZONE_NODATA_PARAM))
                theList = CacheManager.GetData(DAILY_DBF_STATESITE_OZONE_NODATA_PARAM) as List<DataParameter>;
            if (null == theList)
            {
                theList = new List<DataParameter>();
                Dictionary<int, DictionaryValueItem> dictData = DictionaryManager.GetCacheData(DICTIONARY_CODE_STATESITE_OZONE);
                if (null != dictData)
                {
                    ParameterManager pm = new ParameterManager();

                    foreach (DictionaryValueItem dvi in dictData.Values)
                    {
                        int paramId = 0;
                        if ("1" != dvi.StandbyField1 && int.TryParse(dvi.Key, out paramId))
                        {
                            DataParameter param = pm.GetParameter(paramId, true);
                            if (null != param)
                                theList.Add(param);
                        }
                    }
                }
                CacheManager.SetData(DAILY_DBF_STATESITE_OZONE_NODATA_PARAM, theList);
            }
            return theList;
        }

        public static List<DataParameter> Sort(List<DataParameter> dataSoruce)
        {
            dataSoruce.Sort(new Comparison<DataParameter>(delegate(DataParameter f1, DataParameter f2) { return f1.OrderId - f2.OrderId; }));
            return dataSoruce;
        }
    }
}
