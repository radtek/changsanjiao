using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace MMShareBLL.Model
{
    /// <summary>
    /// 表示区县的实体
    /// </summary>
    public class County : IGroup
    {
        private int id;
        private string name;
        private List<Site> sites;

        public County()
        {
            sites = new List<Site>();
        }

        public County(int id, string name)
            : this()
        {
            this.id = id;
            this.name = name;
        }

        /// <summary>
        /// 获取或设置该区县的编号
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 获取或设置该区县的名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 获取或设置该区县下的所有监测站点的集合
        /// </summary>
        public List<Site> Sites
        {
            get { return sites; }
        }

        public int[] SiteIds
        {
            get
            {
                int[] intSites = new int[sites.Count];
                for (int i = 0; i < sites.Count; i++)
                {
                    intSites[i] = sites[i].Id;
                }
                return intSites;
            }
        }

        public int[] ItemIdentities
        {
            get { return SiteIds; }
        }

    }
}
