
namespace MMShareBLL.Model
{
    /// <summary>
    /// 提供监测及数值预报数据的基本操作
    /// </summary>
    public  class DataCollectionBase
    {
        protected MonitoringDataCollection dataCollection;

        public DataCollectionBase()
        {
            dataCollection = new MonitoringDataCollection();
        }

        protected PeriodOfTime date;

        /// <summary>
        /// 获取或设置数据所属的时间
        /// </summary>
        public PeriodOfTime Date
        {
            get { return date; }
            set { date = value; }
        }

        /// <summary>
        /// 根据参数编号获得数据
        /// </summary>
        /// <param name="factorID"></param>
        /// <returns></returns>
        public MonitoringData this[int factorID]
        {
            get
            {
                foreach (MonitoringData d in dataCollection)
                {
                    if (d.Factor != null && d.Factor.Id == factorID)
                        return d.Clone();
                }
                return null;
            }
        }

        /// <summary>
        /// 根据参数代码获得数据
        /// </summary>
        /// <param name="ingestCode"></param>
        /// <returns></returns>
        public MonitoringData this[string ingestCode]
        {
            get
            {
                if (string.IsNullOrEmpty(ingestCode))
                    return null;
                foreach (MonitoringData d in dataCollection)
                {
                    if (null != d.Factor && ingestCode == d.Factor.Code)
                        return d.Clone();
                }
                return null;
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public MonitoringDataCollection DataCollection
        {
            get { return dataCollection; }
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="theData"></param>
        public void SetDataCollection(MonitoringDataCollection theData)
        {
            dataCollection = theData;
        }
    }
}
