using System.Collections.Generic;

namespace MMShareBLL.Model
{
    /// <summary>
    /// 分组数据
    /// </summary>
    public class GroupData : DataCollectionBase
    {
        private SiteGroup group;
        private List<SiteData> siteData;

        public GroupData()
        {
            siteData = new List<SiteData>();
        }

        public SiteGroup Group
        {
            get { return group; }
            set { group = value; }
        }

        public List<SiteData> SiteData
        {
            get { return siteData; }
        }

        public void SetSiteData(List<SiteData> dataList)
        {
            siteData = dataList;
        }
    }
}
