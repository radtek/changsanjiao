using System.Collections.Generic;
using MMShareBLL.Model;
using MMShareBLL.DAL;
using System.Data;


namespace MMShareBLL.BLL
{
    /// <summary>
    /// 提供对区县和站点列表的访问
    /// </summary>
    public class Area
    {
        private CountyDal countyDal = new CountyDal();
        private SiteDal siteDal = new SiteDal();
        DailyAQIForecast DAF = new DailyAQIForecast();
        DailyDataForecast DDF = new DailyDataForecast();

        //Cache key..
        private const string CACHEKEY_ALLDATA = "COUNTYINFO_LIST";
        private const string CACHEKEY_STATESITE = "STATESITE_LIST";
        private const string CACHEKEY_SITELIST = "SITE_LIST";
        private const string CACHEKEY_SITE = "SITE_";
        private const string CACHEKEY_SITEBYCOUNTYLIST = "SITEBYCOUNTY_LIST";

        public Area()
        {
        }

        /// <summary>
        /// 查询所有区域
        /// </summary>
        /// <returns></returns>
        public DataTable GetALL() {
           return  DAF.GetAllDistrict();
        }

        /// <summary>
        /// 获取站点组
        /// </summary>
        /// <returns></returns>
        public DataTable GetSiteGroup()
        {
            return DAF.GetSiteGroup();
        }

        /// <summary>
        /// 得到参数组
        /// </summary>
        /// <returns></returns>
        public DataTable GetParaGroup() {
            return DDF.GetParsGroupData();
        }

        /// <summary>
        /// 得到参数组数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetParasData() {
            return DDF.GetParsGroup();
        }


        /// <summary>
        /// 获取站点组数据明细
        /// </summary>
        /// <returns></returns>
        public DataTable GetSiteGroupData()
        {
            return DAF.GetSiteGroupData();
        }

        /// <summary>
        /// 查询完整的区县信息，包括该区县下的监测站点的数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, County> GetCounty()
        {
            Dictionary<int, County> counties = countyDal.Query();
            if (null == counties)
                return null;

            Dictionary<int, Site> stateSite = GetStateSite();

            foreach (County county in counties.Values)
            {
                foreach (Site site in county.Sites)
                {
                    if (stateSite.ContainsKey(site.Id))
                        site.SiteType = stateSite[site.Id].SiteType;
                }
            }
            return counties;
        }


        /// <summary>
        /// 查询所有区的数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, County> GetCounty1()
        {
            Dictionary<int, County> counties = countyDal.Query1();
            if (null == counties)
                return null;
            return counties;
        }


        /// <summary>
        /// 查询完整的区县信息，包括该区县下的监测站点的数据
        /// </summary>
        /// <param name="fromCache">是否从缓存中加载数据</param>
        /// <returns></returns>
        public Dictionary<int, County> GetCounty(bool fromCache)
        {
            Dictionary<int, County> theData = null;
            if (fromCache && CacheManager.Contains(CACHEKEY_ALLDATA))
                theData = CacheManager.GetData(CACHEKEY_ALLDATA) as Dictionary<int, County>;
            if (theData == null)
            {
                theData = GetCounty();
                CacheManager.SetData(CACHEKEY_ALLDATA, theData);
            }
            return theData;
        }

        /// <summary>
        /// 根据区县编号获取区县数据
        /// </summary>
        /// <param name="countyID">区县编号</param>
        /// <param name="fromCache">是否从缓存中加载数据</param>
        /// <returns></returns>
        public County GetCounty(int countyID, bool fromCache)
        {
            County theData = null;
            if (fromCache)
            {
                Dictionary<int, County> counties = GetCounty(true);
                foreach (County county in counties.Values)
                {
                    if (county.Id == countyID)
                    {
                        theData = county;
                        break;
                    }
                }
            }
            else
                theData = countyDal.Query(countyID);

            return theData;
        }

        public Dictionary<int, Site> GetSite()
        {
            return siteDal.Query();
        }

        public Dictionary<int, Site> GetSite(bool fromCache)
        {
            Dictionary<int, Site> theData = null;
            if (fromCache && CacheManager.Contains(CACHEKEY_SITELIST))
                theData = CacheManager.GetData(CACHEKEY_SITELIST) as Dictionary<int, Site>;
            if (null == theData)
            {
                theData = siteDal.Query();
                CacheManager.SetData(CACHEKEY_SITELIST, theData);
            }
            return theData;
        }

        public Dictionary<int, Site> GetSiteByCountID(int intCountyID,bool fromCache)
        {
            Dictionary<int, Site> theData = null;
            //if (fromCache && CacheManager.Contains(CACHEKEY_SITEBYCOUNTYLIST))
            //    theData = CacheManager.GetData(CACHEKEY_SITEBYCOUNTYLIST) as Dictionary<int, Site>;
            //if (null == theData)
            //{
                theData = siteDal.Query1(intCountyID);
            //    CacheManager.SetData(CACHEKEY_SITEBYCOUNTYLIST, theData);
            //}
            return theData;
        }

        public List<Site> GetSiteList(bool fromCache)
        {
            Dictionary<int, Site> sites = GetSite(fromCache);
            List<Site> siteList = new List<Site>();
            foreach (Site site in sites.Values)
                siteList.Add(site);
            return siteList;
        }

    


        public List<Site> GetSitesByCountID(int intCountID,bool fromCache)
        {
            Dictionary<int, Site> sites = GetSiteByCountID(intCountID,fromCache);
            List<Site> siteList = new List<Site>();
            foreach (Site site in sites.Values)
                siteList.Add(site);
            return siteList;
        }

        /// <summary>
        /// 根据监测站点编号获取站点数据
        /// </summary>
        /// <param name="siteID">站点编号</param>
        /// <param name="fromCache">是否从缓存中加载数据</param>
        /// <returns></returns>
        public Site GetSite(int siteID, bool fromCache)
        {
            Site theData = null;
            string cacheKey = CACHEKEY_SITE + siteID;
            if (fromCache)
            {
                if (CacheManager.Contains(cacheKey))
                    theData = CacheManager.GetData(cacheKey) as Site;
            }
            if (null == theData)
            {
                theData = siteDal.Query(siteID);
                CacheManager.SetData(cacheKey, theData);
            }
            return theData;
        }

        /// <summary>
        /// 根据一组站点编号，转换出一组站点对象
        /// </summary>
        /// <param name="sites">站点编号的集合</param>
        /// <returns></returns>
        public static List<Site> ParseSite(int[] sites)
        {
            List<Site> siteList = new List<Site>();
            Area area = new Area();
            foreach (int siteId in sites)
            {
                Site site = area.GetSite(siteId, true);
                if (site != null)
                    siteList.Add(site);
            }
            return siteList;
        }

        public Dictionary<int, Site> GetStateSite()
        {
            SiteGroupManager sgm = new SiteGroupManager();
            List<Site> siteList = sgm.QueryCitywideGroup(true).Items;
            Dictionary<int, Site> reData = new Dictionary<int, Site>();
            foreach (Site site in siteList)
                reData[site.Id] = site;
            return reData;
        }

        /// <summary>
        /// 获取国控点列表
        /// </summary>
        /// <param name="fromCache">是否从缓存中加载数据</param>
        /// <returns></returns>
        public Dictionary<int, Site> GetStateSite(bool fromCache)
        {
            Dictionary<int, Site> stateSite = null;
            if (fromCache && CacheManager.Contains(CACHEKEY_STATESITE))
                stateSite = CacheManager.GetData(CACHEKEY_STATESITE) as Dictionary<int, Site>;
            if (null == stateSite)
            {
                stateSite = GetStateSite();
                CacheManager.SetData(CACHEKEY_STATESITE, stateSite);
            }
            return stateSite;
        }

        public List<Site> GetStateSiteList(bool fromCache)
        {
            Dictionary<int, Site> stateSite = GetStateSite(fromCache);
            List<Site> siteList = new List<Site>();
            foreach (Site site in stateSite.Values)
                siteList.Add(site);
            return siteList;
        }

        //private Dictionary<int, SiteType> getStateSiteIds()
        //{
        //    Dictionary<int, SiteType> theData = new Dictionary<int, SiteType>();
        //    Dictionary<int, DictionaryValueItem> stateSite = DictionaryManager.GetStateSite();
        //    foreach (int siteId in stateSite.Keys)
        //    {
        //        DictionaryValueItem dv = stateSite[siteId];
        //        SiteType st = SiteType.National;

        //        if ("NO" == dv.StandbyField1)
        //            st = SiteType.Reference;
        //        theData[siteId] = st;
        //    }

        //    return theData;
        //}

        /// <summary>
        /// 取得国控点编号列表，不包涵参考点
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Site> GetStateSitesNoReference()
        {
            //取得国控点列表
            Dictionary<int, Site> stateSites = GetStateSite(true);
            Dictionary<int, Site> theData = new Dictionary<int, Site>();

            foreach (Site site in stateSites.Values)
            {
                //if (site.SiteType == SiteType.National)
                //{
                    theData.Add(site.Id, site);
                //}
            }
            return theData;
        }

        /// <summary>
        /// 获得参考点
        /// </summary>
        /// <returns></returns>
        public Site GetReferenceSite()
        {
            Dictionary<int, Site> stateSites = GetStateSite(true);
            foreach (Site site in stateSites.Values)
            {
                if (site.SiteType == SiteType.Reference)
                    return site;
            }
            return null;
        }

        public static List<Site> ParseSiteDictionary(Dictionary<int, Site> dict)
        {
            List<Site> theSite = new List<Site>();
            if (null == dict)
                return theSite;
            foreach (Site site in dict.Values)
                theSite.Add(site);
            return theSite;
        }
    }
}

