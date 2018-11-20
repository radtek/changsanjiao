
namespace MMShareBLL.Model
{
    /// <summary>
    /// 数值预报站点数据
    /// </summary>
    public class ForecastSiteData : SiteData
    {
        private int forecastType = -1;

        /// <summary>
        /// 预报类型，forecastType * 24 小时预报
        /// </summary>
        public int ForecastType
        {
            get { return forecastType; }
            set { forecastType = value; }
        }


    }
}
