using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.ObjectModel;
using MMShareBLL.Model;
using MMShareBLL.DAL;

namespace MMShareBLL.BLL
{
    public class ParameterAdapter
    {
        private const int PARAM_SETTING_CODE = 35;

        private const string CACHEKEY_SORTEDLIST = "PARAMETER_CONFIG_SORTED";

        public Dictionary<int, DataParameter> Sort(List<DataParameter> parameters, bool fillSettings)
        {
            Dictionary<int, DataParameter> sorted = new Dictionary<int, DataParameter>();

            List<DictionaryValueItem> sortedParamDictItems = GetSortedParamConfig();
            foreach (DictionaryValueItem dvi in sortedParamDictItems)
            {
                foreach (DataParameter param in parameters)
                {
                    if (param.Code == dvi.Key)
                    {
                        DataParameter theParam = param;

                        if (fillSettings)
                            theParam = FillSetting(param, dvi);

                        sorted[theParam.Id] = theParam;
                        break;
                    }
                }
            }

            foreach (DataParameter param in parameters)
            {
                if (!sorted.ContainsKey(param.Id))
                    sorted[param.Id] = param;
            }

            return sorted;
        }

        public List<DataParameter> SortList(List<DataParameter> parameters, bool fillSettings)
        {
            Dictionary<int, DataParameter> theSorted = Sort(parameters, fillSettings);
            List<DataParameter> theData = new List<DataParameter>();
            foreach (DataParameter param in theSorted.Values)
                theData.Add(param);

            return theData;
        }

        public Dictionary<int, DataParameter> Sort(Dictionary<int, DataParameter> parameters, bool fillSettings)
        {
            List<DataParameter> theParam = new List<DataParameter>();
            foreach (DataParameter param in parameters.Values)
                theParam.Add(param);
            return Sort(theParam, fillSettings);
        }

        public DataParameter FillSetting(DataParameter param, List<DictionaryValueItem> settings)
        {
            if (null == param)
                return null;
            DataParameter newParam = param.Clone();
            if (null == settings)
                return newParam;
            foreach (DictionaryValueItem dvi in settings)
            {
                if (dvi.Key == param.Code)
                {
                    newParam = FillSetting(newParam, dvi);
                    break;
                }
            }
            return newParam;
        }

        public DataParameter FillSetting(DataParameter param)
        {
            List<DictionaryValueItem> sortedParamDictItems = GetSortedParamConfig();
            return FillSetting(param, sortedParamDictItems);
        }

        public DataParameter FillSetting(DataParameter param, DictionaryValueItem dvi)
        {
            if (null == param)
                return null;

            DataParameter newParam = param.Clone();

            if (!string.IsNullOrEmpty(dvi.Value))
                newParam.Color = dvi.Value;
            float min = 0;
            if (float.TryParse(dvi.StandbyField1, out min))
                newParam.Min = min;
            float max = 0;
            if (float.TryParse(dvi.StandbyField2, out max))
                newParam.Max = max;
            if ("Default" == dvi.StandbyField4)
                newParam.DefaultChecked = true;
            if (!string.IsNullOrEmpty(dvi.StandbyField5))
            {
                string[] apiSetting = dvi.StandbyField5.Split(new char[] { ':' });
                float[] apiCalcArray = Pub.ParseToArray(apiSetting[0]);
                if (null != apiCalcArray && apiCalcArray.Length == 6)
                {
                    newParam.ConcentrationLimits = apiCalcArray;
                    newParam.CanComputeApi = true;
                    if (apiSetting.Length >= 2 && "ca" == apiSetting[1])
                        newParam.CalcCitywideApi = true;
                }
            }
            return newParam;
        }

        #region 浓度等级数量不为6个的：如臭氧滑动8小时的浓度分级是4个
        /// <summary>
        /// 浓度等级数量不为6个的：如臭氧滑动8小时的浓度分级是4个
        /// </summary>
        /// <param name="param"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public DataParameter FillSettingFree(DataParameter param, List<DictionaryValueItem> settings)
        {
            if (null == param)
                return null;
            DataParameter newParam = param.Clone();
            if (null == settings)
                return newParam;
            foreach (DictionaryValueItem dvi in settings)
            {
                if (dvi.Key == param.Code)
                {
                    newParam = FillSettingFree(newParam, dvi);
                    break;
                }
            }
            return newParam;
        }

        public DataParameter FillSettingFree(DataParameter param)
        {
            List<DictionaryValueItem> sortedParamDictItems = GetSortedParamConfig();
            return FillSettingFree(param, sortedParamDictItems);
        }

        public DataParameter FillSettingFree(DataParameter param, DictionaryValueItem dvi)
        {
            if (null == param)
                return null;

            DataParameter newParam = param.Clone();

            if (!string.IsNullOrEmpty(dvi.Value))
                newParam.Color = dvi.Value;
            float min = 0;
            if (float.TryParse(dvi.StandbyField1, out min))
                newParam.Min = min;
            float max = 0;
            if (float.TryParse(dvi.StandbyField2, out max))
                newParam.Max = max;
            if ("Default" == dvi.StandbyField4)
                newParam.DefaultChecked = true;
            if (!string.IsNullOrEmpty(dvi.StandbyField5))
            {
                string[] apiSetting = dvi.StandbyField5.Split(new char[] { ':' });
                float[] apiCalcArray = Pub.ParseToArray(apiSetting[0]);
                if (null != apiCalcArray )//&& apiCalcArray.Length == 6
                {
                    newParam.ConcentrationLimits = apiCalcArray;
                    newParam.CanComputeApi = true;
                    if (apiSetting.Length >= 2 && "ca" == apiSetting[1])
                        newParam.CalcCitywideApi = true;
                }
            }
            return newParam;
        }
        #endregion

        public List<DictionaryValueItem> GetSortedParamConfig()
        {
            List<DictionaryValueItem> sortList = null;
            sortList = CacheManager.GetData(CACHEKEY_SORTEDLIST) as List<DictionaryValueItem>;

            if (null == sortList)
            {
                Dictionary<int, DictionaryValueItem> theConfig = DictionaryManager.GetCacheData(PARAM_SETTING_CODE);
                Dictionary<int, List<int>> canSortItems = new Dictionary<int, List<int>>();
                Dictionary<int, DictionaryValueItem> noSort = new Dictionary<int, DictionaryValueItem>();
                Dictionary<int, DictionaryValueItem> sorted = new Dictionary<int, DictionaryValueItem>();

                foreach (int key in theConfig.Keys)
                {
                    DictionaryValueItem dvi = theConfig[key];
                    int sortIndex = 0;
                    List<int> theItems = null;
                    if (int.TryParse(dvi.StandbyField3, out sortIndex))
                    {
                        if (canSortItems.ContainsKey(sortIndex))
                            theItems = canSortItems[sortIndex];
                        else
                        {
                            theItems = new List<int>();
                            canSortItems[sortIndex] = theItems;
                        }
                        theItems.Add(dvi.ValueID);
                    }
                    else
                        noSort.Add(key, dvi);
                }
                int[] keys = new int[canSortItems.Keys.Count];
                canSortItems.Keys.CopyTo(keys, 0);
                Array.Sort(keys);
                sortList = new List<DictionaryValueItem>();
                foreach (int key in keys)
                {
                    List<int> values = canSortItems[key];
                    foreach (int valueKey in values)
                        sortList.Add(theConfig[valueKey]);
                }
                foreach (DictionaryValueItem cannotSortItem in noSort.Values)
                    sortList.Add(cannotSortItem);

                CacheManager.SetData(CACHEKEY_SORTEDLIST, sortList);
            }
            return sortList;
        }
    }
}
