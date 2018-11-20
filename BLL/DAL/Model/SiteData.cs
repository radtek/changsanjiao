

namespace MMShareBLL.Model
{
    /// <summary>
    /// 站点数据
    /// </summary>
    public class SiteData : DataCollectionBase
    {
        private int id;

        private Site site;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 获取或设置该项数据的站点对象
        /// </summary>
        public Site Site
        {
            get { return site; }
            set { site = value; }
        }

    }
}
