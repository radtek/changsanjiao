using System.Collections.Generic;


namespace MMShareBLL.Model
{
    /// <summary>
    /// 区县分组
    /// </summary>
    public class CountyGroup
    {

        private List<SiteGroup> items;

        /// <summary>
        /// 获取该组下的站点组列表
        /// </summary>
        public List<SiteGroup> Items
        {
            get { return items; }
        }

        public void SetItems(List<SiteGroup> data)
        {
            this.items = data;
        }
    }
}
