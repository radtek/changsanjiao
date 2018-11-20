using System;
using System.Collections.Generic;


namespace MMShareBLL.Model
{
    /// <summary>
    /// 全市数据
    /// </summary>
    public class CitywideData : DataCollectionBase
    {
        private int id;
        private DailyState state;
        private int api;
        private string lastModifyPersion;
        private DateTime lastModifyTime;
        private List<SiteData> siteData;
        private bool isAuto;
        private Duration duration;
        private Site referenceSite;
        private bool fullData = false;

        public CitywideData()
        {
            siteData = new List<SiteData>();
        }

        /// <summary>
        /// 获取或设置日报编号
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 获取或设置日报的日期
        /// </summary>
        public PeriodOfTime Time
        {
            get { return date; }
            set { date = value; }
        }

        /// <summary>
        /// 获取或设置日报的状态
        /// </summary>
        public DailyState State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 获取或设置日报的污染分指数
        /// </summary>
        public int API
        {
            get { return api; }
            set
            {
                if (api < 0 || api > 500)
                    return;
                api = value;
            }
        }

        /// <summary>
        /// 获取或设置日报最后的修改人
        /// </summary>
        public string LastModifyPersion
        {
            get { return lastModifyPersion; }
            set { lastModifyPersion = value; }
        }

        /// <summary>
        /// 获取或设置日报最后的修改时间
        /// </summary>
        public DateTime LastModifyTime
        {
            get { return lastModifyTime; }
            set { lastModifyTime = value; }
        }

        /// <summary>
        /// 获取或设置全市预报的站点数据
        /// </summary>
        public List<SiteData> SiteData
        {
            get { return siteData; }
        }

        public void SetDataCollection(List<SiteData> dataSource)
        {
            this.siteData = dataSource;
        }

        /// <summary>
        /// 指示是否由系统自动生成
        /// </summary>
        public bool IsAuto
        {
            get { return isAuto; }
            set { isAuto = value; }
        }

        public Duration Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        /// <summary>
        /// 获得该项数据的参考点
        /// </summary>
        public Site ReferenceSite
        {
            get { return referenceSite; }
            set { referenceSite = value; }
        }

        /// <summary>
        /// 数据是否完整
        /// </summary>
        public bool FullData
        {
            get { return fullData; }
            set { fullData = value; }
        }
    }
}
