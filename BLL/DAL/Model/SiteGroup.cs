using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.Model
{
    /// <summary>
    /// 站点组
    /// </summary>
    public class SiteGroup : IGroup
    {
        private int id;
        private string groupCode;
        private string groupName;
        private bool readOnly;
        private List<Site> items;

        public SiteGroup()
        {
            items = new List<Site>();
        }

        public SiteGroup(string groupCode, string groupName)
            : this()
        {
            this.groupCode = groupCode;
            this.groupName = groupName;
        }

        /// <summary>
        /// 获取或设置组标识
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 获取或设置组代码
        /// </summary>
        public string GroupID
        {
            get { return groupCode; }
            set { groupCode = value; }
        }

        /// <summary>
        /// 获取或设置组名称
        /// </summary>
        public string GroupName
        {
            get { return groupName; }
            set { groupName = value; }
        }

        /// <summary>
        /// 指示是否只读
        /// </summary>
        public bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }

        /// <summary>
        /// 获取该组下的站点列表
        /// </summary>
        public List<Site> Items
        {
            get { return items; }
        }

        public void SetItems(List<Site> items)
        {
            this.items = items;
        }

        public int[] ItemIdentities
        {
            get
            {
                int[] idetities = new int[items.Count];

                for (int i = 0; i < idetities.Length; i++)
                    idetities[i] = items[i].Id;
                return idetities;
            }
        }

        public string Name
        {
            get { return groupName; }
        }
    }
}
