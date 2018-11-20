using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MMShareBLL.Model
{
    public class MonitoringDataCollection : ICollection<MonitoringData>
    {

        //数据源
        private Collection<MonitoringData> dataSource;

        //计算后的数据
        private Collection<MonitoringData> calculatedData;

        //数据源是否已更新
        private bool updated = false;

        //整个数据源的最大API
        private int maxAPI = 0;

        //污染分指数
        private int api;

        //整个数据源的最大AQI
        private int maxAQI = 0;

        //污染分指数
        private int aqi;
        //最小值
        private Dictionary<int, float> min;
        //最大值
        private Dictionary<int, float> max;
        //有效数
        private Dictionary<int, int> valid;

        //有效数:QCCode!=9指标统计中有效数（秦龙2013.6.2添加）
        private Dictionary<int, int> valid2;

        //当前计算类型: 0未计算,1：取平均，2：取最大值，3：根据污染因子自身配置进行取值
        private int currentCalcType = 0;

        public MonitoringDataCollection()
        {
            dataSource = new Collection<MonitoringData>();
            calculatedData = new Collection<MonitoringData>();
            min = new Dictionary<int, float>();
            max = new Dictionary<int, float>();
            valid = new Dictionary<int, int>();
            valid2 = new Dictionary<int, int>();
            //avg = new Dictionary<int, float>();
            //sampleCount = new Dictionary<int, int>();
            //effectiveCount = new Dictionary<int, int>();
        }

        /// <summary>
        /// 取得某一污染因子的浓度值,从计算后的数据容器里获取数据，注意，这里只能获取元数据的复本。
        /// </summary>
        /// <param name="factorId">污染因子的编号</param>
        /// <returns></returns>
        public MonitoringData this[int factorId]
        {
            get
            {
                foreach (MonitoringData d in calculatedData)
                {
                    if (d.Factor != null && d.Factor.Id == factorId)
                        return d.Clone();
                }
                return null;
            }
        }

        public MonitoringData GetDataItem(int parameterId)
        {
            foreach (MonitoringData d in calculatedData)
            {
                if (d.Factor != null && d.Factor.Id == parameterId)
                    return d;
            }
            return null;
        }

        /// <summary>
        /// 从数据源获取未经计算的数据
        /// </summary>
        /// <param name="parameterId"></param>
        /// <returns></returns>
        public List<MonitoringData> GetDataList(int parameterId)
        {
            List<MonitoringData> dataList = new List<MonitoringData>();
            foreach (MonitoringData data in dataSource)
            {
                if (null != data.Factor && data.Factor.Id == parameterId)
                    dataList.Add(data);
            }
            return dataList;
        }

        /// <summary>
        /// 给数据源添加数据项
        /// </summary>
        /// <param name="item"></param>
        public void Add(MonitoringData item)
        {
            if (item == null || item.Factor == null)
                return;
            dataSource.Add(item);
            calculatedData.Add(item);
            if (null != item.API && maxAPI < item.API)
                maxAPI = item.API.Value;
            if (null != item.AQI && maxAQI < item.AQI)
                maxAQI = item.AQI.Value;

            int parameterId = item.Factor.Id;
            if (!max.ContainsKey(parameterId) || (max.ContainsKey(parameterId) && max[parameterId] < item.Thickness))
                max[parameterId] = item.Thickness;
            if (!min.ContainsKey(parameterId) || (min.ContainsKey(parameterId) && min[parameterId] > item.Thickness))
                min[parameterId] = item.Thickness;
            if (item.Valid)
            {
                if (valid.ContainsKey(parameterId))
                    valid[parameterId] += 1;
                else
                    valid[parameterId] = 1;
            }
            if (item.QCCode != 9)//无效
            {
                if (valid2.ContainsKey(parameterId))
                    valid2[parameterId] += 1;
                else
                    valid2[parameterId] = 1;
            }
            //标识数据源已经更新
            updated = true;
        }

        /// <summary>
        /// 清除数据源和已经计算过的数据
        /// </summary>
        public void Clear()
        {
            dataSource.Clear();
            calculatedData.Clear();
            max.Clear();
            min.Clear();
            valid.Clear();
            valid2.Clear();
            maxAPI = 0;
            api = 0;
            updated = true;
        }

        /// <summary>
        /// 判断数据源是否存在该项数据
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(MonitoringData item)
        {
            return dataSource.Contains(item);
        }

        /// <summary>
        /// 判断数据源是否存在该参数的数据
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        public bool Contains(int factorId)
        {
            foreach (MonitoringData d in calculatedData)
            {
                if (d.Factor != null && d.Factor.Id == factorId)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断经过计算的数据中是否存在该参数的数据
        /// </summary>
        /// <param name="parameterId"></param>
        /// <returns></returns>
        public bool ContainsCalculatedData(int parameterId)
        {
            foreach (MonitoringData d in calculatedData)
            {
                if (null != d.Factor && d.Factor.Id == parameterId)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 从数据源中复制数据
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(MonitoringData[] array, int arrayIndex)
        {
            dataSource.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 获得数据源的总数
        /// </summary>
        public int Count
        {
            get { return dataSource.Count; }
        }

        /// <summary>
        /// 永远返回false
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(MonitoringData item)
        {
            updated = true;
            return dataSource.Remove(item);
        }

        public IEnumerator<MonitoringData> GetEnumerator()
        {
            return calculatedData.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return null;
        }

        /// <summary>
        /// 获取或设置最大API，使用DataCalculate.CalculateApi 方法后MaxApi才会被赋予有效值。
        /// </summary>
        public int MaxApi
        {
            get { return maxAPI; }
            set { maxAPI = value; }
        }

        /// <summary>
        /// 获取或设置该组数据的污染分指数，使用DataCalculate.CalculateApi 方法后Api才会被赋予有效值。
        /// </summary>
        public int Api
        {
            get { return api; }
            set { api = value; }
        }

        /// <summary>
        /// 把该站点数据转换成经过计算的（该计算将去掉重复的数据，并把重复的进行平均，来获得新的数据）
        /// </summary>
        public void ChangeToCalculatedDataAvg()
        {
            calculatedData = GetCalculatedData(1);
        }

        /// <summary>
        /// 把该站点数据转换成经过计算的（该计算将去掉重复的数据，但不会平均，而是取最大值）
        /// </summary>
        public void ChangeToCalaculatedDataMax()
        {
            calculatedData = GetCalculatedData(2);
        }

        /// <summary>
        /// 把该站点数据转换成经过计算的（该计算将去掉重复的数据，并根据污染因子自身配置来决定是取最大值还是取平均）
        /// </summary>
        public void ChangeToCalaculatedDataAuto()
        {
            calculatedData = GetCalculatedData(3);
        }

        /// <summary>
        /// 获得经过计算后所得的数据，这些数据去掉了重复的值(当出现相同的监测因子时，则认为是重复的数据，注意在这种情况下，其污染浓度却不一定相同)
        /// </summary>
        /// <param name="valueType">合并方式，1：取平均，2：取最大值，3：根据污染因子自身配置进行取值</param>
        /// <returns></returns>
        public Collection<MonitoringData> GetCalculatedData(int valueType)
        {
            if (updated == false)
                return calculatedData;

            Dictionary<int, Collection<MonitoringData>> repeatableData = new Dictionary<int, Collection<MonitoringData>>();

            foreach (MonitoringData data in dataSource)
            {
                if (repeatableData.ContainsKey(data.Factor.Id))
                    repeatableData[data.Factor.Id].Add(data);
                else
                {
                    Collection<MonitoringData> tempContainer = new Collection<MonitoringData>();
                    tempContainer.Add(data);
                    repeatableData[data.Factor.Id] = tempContainer;
                }
            }
            Collection<MonitoringData> tempCalculatedData = new Collection<MonitoringData>();
            calculatedData.Clear();

            foreach (int key in repeatableData.Keys)
            {
                int repeatableCount = repeatableData[key].Count;
                if (repeatableCount > 1)
                {
                    float max = 0;
                    float sum = 0;
                    foreach (MonitoringData data in repeatableData[key])
                    {
                        if (data.Thickness > max)
                            max = data.Thickness;
                        sum += data.Thickness;
                    }
                    MonitoringData newData = new MonitoringData();
                    newData.Factor = repeatableData[key][0].Factor.Clone();
                    //平均
                    if (valueType == 1)
                        newData.Thickness = sum / repeatableCount;
                    //最大值
                    else if (valueType == 2)
                        newData.Thickness = max;
                    //Auto
                    else if (valueType == 3)
                    {
                        if (newData.Factor.CollectionType == ParameterCollectionType.Avg)
                            newData.Thickness = sum / repeatableCount;
                        else if (newData.Factor.CollectionType == ParameterCollectionType.Max)
                            newData.Thickness = max;
                    }
                    newData.Thickness = (float)Math.Round(newData.Thickness, 3);
                    tempCalculatedData.Add(newData);
                }
                else
                    tempCalculatedData.Add(repeatableData[key][0].Clone());
            }
            updated = false;
            return tempCalculatedData;
        }

        /// <summary>
        /// 计算指标,在调用此方法之前,最好先执行:ChangeToCalculatedDataAvg或其它用于计算的函数.如果不存在相关指标的数据,则返回null
        /// </summary>
        /// <param name="indicators">指标</param>
        /// <param name="parameterId">参数编号</param>
        /// <returns></returns>
        public float? Calculate(StatIndicators indicators, int parameterId)
        {
            if (indicators == StatIndicators.Avg)
            {
                MonitoringData data = this[parameterId];
                if (null != data)
                    return data.Thickness;
            }
            else if (indicators == StatIndicators.Max && max.ContainsKey(parameterId))
                return max[parameterId];
            else if (indicators == StatIndicators.Min && min.ContainsKey(parameterId))
                return min[parameterId];
            else if (indicators == StatIndicators.SampleCount)
            {
                List<MonitoringData> dataList = GetDataList(parameterId);
                return dataList.Count;
            }
            else if (indicators == StatIndicators.Median)
            {
                List<MonitoringData> dataList = GetDataList(parameterId);
                if (dataList.Count == 0)
                    return null;
                if (dataList.Count == 1)
                    return dataList[0].Thickness;
                if (dataList.Count == 2)
                    return (dataList[0].Thickness + dataList[1].Thickness) / 2;

                float[] dataArray = new float[dataList.Count];
                for (int i = 0; i < dataList.Count; i++)
                    dataArray[i] = dataList[i].Thickness;
                Array.Sort<float>(dataArray);
                if (dataArray.Length % 2 != 0)
                    return dataArray[dataArray.Length / 2 + 1];
                else
                {
                    int medianIndex = dataArray.Length / 2;
                    float upper = dataArray[medianIndex];
                    float lower = dataArray[medianIndex + 1];
                    return (upper + lower) / 2;
                }
            }
            else if (indicators == StatIndicators.Thickness)
            {
                MonitoringData data = this[parameterId];
                if (null != data)
                    return data.Thickness;
            }
            else if (indicators == StatIndicators.Valid && valid.ContainsKey(parameterId))
            {
                return valid[parameterId];
            }
            else if (indicators == StatIndicators.Valid2 && valid2.ContainsKey(parameterId))
            {
                return valid2[parameterId];
            }
            return null;
        }
    }
}
