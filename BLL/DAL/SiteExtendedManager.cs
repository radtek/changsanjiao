
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using MMShareBLL.Model;
using MMShareBLL.BLL;

namespace MMShareBLL.DAL
{
    public class SiteExtendedManager
    {
        /// <summary>
        /// 图片 
        /// </summary>
        //public const String CONST_NATION_SITE_IMAGE = "<img title=\"国控站点\" src=\"Images/i_nation.png\" style=\"vertical-align:middle;\">";
        //public const String CONST_AREA_SITE_IMAGE = "<img title=\"分区站点\" src=\"Images/i_nation.png\" style=\"vertical-align:middle;\">";
        //public const String CONST_BLANK_STRING = "<img src=\"../Img/empty.GIF\" style=\"width:16px;\"/>";

        public const String CONST_NATION_SITE_IMAGE = "★";
        public const String CONST_AREA_SITE_IMAGE = "◆";
        public const String CONST_BLANK_STRING = "";

        private static IDictionary<int, Site> _nationSiteData = null;
        private static IDictionary<int, KeyValuePair<Site, SiteGroup>> _areaSiteData = null;

        private static object _flag = new object();
        private static DateTime _lastNationDataRefreshTime = DateTime.Now;
        private static DateTime _lastAreaDataRefreshTime = DateTime.Now;

        private static Area _areaComponent = new Area();
        private static CountyGroupManager _countyGroupComponent = new CountyGroupManager();

        private static IDictionary<int, Site> NationSiteDictionary
        {
            get
            {
                lock (_flag)
                {
                    if (DateTime.Now.Subtract(_lastNationDataRefreshTime).Hours > 1)
                    {
                        _nationSiteData = null;

                        _lastNationDataRefreshTime = DateTime.Now;
                    }

                    if (_nationSiteData == null)
                    {
                        _nationSiteData = _areaComponent.GetStateSite();
                    }
                }
                return _nationSiteData;
            }
        }

        private static IDictionary<int, KeyValuePair<Site, SiteGroup>> AreaSiteGroupDictionary
        {
            get
            {
                lock (_flag)
                {
                    if (DateTime.Now.Subtract(_lastAreaDataRefreshTime).Hours > 1)
                    {
                        _areaSiteData = null;

                        _lastAreaDataRefreshTime = DateTime.Now;
                    }

                    if (_areaSiteData == null)
                    {
                        lock (_flag)
                        {
                            _areaSiteData = GetSiteGroupDictionary();
                        }
                    }
                }

                return _areaSiteData;
            }
        }

        private static IDictionary<int, KeyValuePair<Site, SiteGroup>> GetSiteGroupDictionary()
        {
            IDictionary<int, KeyValuePair<Site, SiteGroup>> all = new Dictionary<int, KeyValuePair<Site, SiteGroup>>();

            // 所有分区组(包括组内的站点)
            List<SiteGroup> groupList = _countyGroupComponent.Query();

            foreach (SiteGroup g in groupList)
            {
                foreach (Site s in g.Items)
                {
                    if (!all.ContainsKey(s.Id))
                    {
                        all[s.Id] = new KeyValuePair<Site, SiteGroup>(s, g);
                    }
                }
            }

            return all;
        }


        /// <summary>
        /// 国控站点列表
        /// </summary>
        public static IList<Site> NationSiteList
        {
            get
            {
                return new ReadOnlyCollection<Site>(new List<Site>(NationSiteDictionary.Values));
            }
        }

        /// <summary>
        /// 分区站点的列表
        /// </summary>
        public static IList<Site> AreaSiteList
        {
            get
            {
                return new ReadOnlyCollection<Site>((new List<KeyValuePair<Site, SiteGroup>>(AreaSiteGroupDictionary.Values)).ConvertAll<Site>(delegate(KeyValuePair<Site, SiteGroup> item) { return item.Key; }));
            }
        }

        /// <summary>
        /// 分区站点与分组的列表
        /// </summary>
        public static IList<KeyValuePair<Site, SiteGroup>> AreaSiteGroupList
        {
            get
            {
                return new ReadOnlyCollection<KeyValuePair<Site, SiteGroup>>(
                    new List<KeyValuePair<Site, SiteGroup>>(AreaSiteGroupDictionary.Values));
            }
        }


        /// <summary>
        /// 是否过国控站点
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static bool IsNationSite(int siteId)
        {
            return NationSiteDictionary.ContainsKey(siteId);
        }


        /// <summary>
        /// 是否过国控站点
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsNationSite(Site data)
        {
            bool result = false;
            if (data != null)
            {
                result = IsNationSite(data.Id);
            }

            return result;
        }

        /// <summary>
        /// 是否分区站点
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static bool IsAreaSite(int siteId)
        {
            return AreaSiteGroupDictionary.ContainsKey(siteId);
        }

        /// <summary>
        /// 是否分区站点
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsAreaSite(Site data)
        {
            bool result = false;
            if (data != null)
            {
                result = IsAreaSite(data.Id);
            }

            return result;
        }

        /// <summary>
        /// 获取分区站点对应的分组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SiteGroup GetAreaSiteGroup(Site data)
        {
            SiteGroup result = null;
            if (data != null)
            {
                result = GetAreaSiteGroup(data.Id);
            }

            return result;
        }

        /// <summary>
        /// 获取分区站点对应的分组
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static SiteGroup GetAreaSiteGroup(int siteId)
        {
            SiteGroup result = null;

            KeyValuePair<Site, SiteGroup> data = new KeyValuePair<Site,SiteGroup>();

            if (AreaSiteGroupDictionary.TryGetValue(siteId, out data))
            {
                result = data.Value;
            }

            return result;
        }
    }
}
