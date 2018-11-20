using System;
using System.Collections.Generic;
using System.Text;
using MMShareBLL.Model;
using MMShareBLL.DAL;


namespace MMShareBLL.BLL
{
    /// <summary>
    /// 用于管理站点分组
    /// </summary>
    public class SiteGroupManager
    {
        MMShareBLL.DAL.SiteGroupDataAccess dal = new MMShareBLL.DAL.SiteGroupDataAccess();

        private const string NUMPRXFIX = "SG";
        private static Random rand = new Random();

        private const string CITYWIDEGROUPCODE = "SG00000001";
        private const string CITYWIDEAQIGROUPCODE = "SG00000002";

        /// <summary>
        /// 生成随机组编号
        /// </summary>
        /// <returns></returns>
        private string buildGroupNum()
        {
            StringBuilder sbNum = new StringBuilder(NUMPRXFIX);
            for (int i = 0; i < 8; i++)
            {
                var nType = rand.Next(0, 2);
                if (nType == 0)
                    sbNum.Append(rand.Next(0, 10));
                else
                    sbNum.Append((char)rand.Next(65, 91));
            }
            return sbNum.ToString();
        }

        /// <summary>
        /// 创建一个新组，返回新的组编号
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public string CreateGroup(SiteGroup group)
        {
            try
            {
                group.GroupID = buildGroupNum();
                dal.CreateGroup(group);
                return group.GroupID;
            }
            catch { throw; }
        }

        /// <summary>
        /// 判断是否存在同名的组名称
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public bool Exists(string groupName, string groupID)
        {
            try
            {
                return dal.Exists(groupName, groupID);
            }
            catch { throw; }
        }

        /// <summary>
        /// 为组添加站点
        /// </summary>
        /// <returns></returns>
        public int AppendItems(string groupId, List<Site> sites)
        {
            try
            {
                int successSum = 0;
                for (int i = sites.Count - 1; i >= 0; i--)
                {
                    bool success = dal.AppendItem(groupId, sites[i]);
                    if (success)
                        successSum++;
                }
                return successSum;
            }
            catch { throw; }
        }

        /// <summary>
        /// 更新站点组，仅更新组名称
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public int Update(string groupId, string groupName)
        {
            try
            {
                return dal.Update(groupId, groupName);
            }
            catch { throw; }
        }

        /// <summary>
        /// 更新站点组，并更新其下的站点列表
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public int Update(SiteGroup group)
        {
            try
            {
                return dal.Update(group);
            }
            catch { throw; }
        }

        /// <summary>
        /// 删除组
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public int Remove(string groupId)
        {
            try
            {
                return dal.Remove(groupId);
            }
            catch { throw; }
        }

        /// <summary>
        /// 移除组下的某些站点
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="sites"></param>
        /// <returns></returns>
        public int Remove(string groupId, List<int> sites)
        {
            try
            {
                return dal.Remove(groupId, sites);
            }
            catch { throw; }
        }

        public SiteGroup Query(string groupId)
        {
            try
            {
                return Query(groupId, false);
            }
            catch { throw; }
        }

        public SiteGroup Query(int groupIdentity)
        {
            try
            {
                return Query(groupIdentity, false);
            }
            catch { throw; }
        }

        /// <summary>
        /// 根据组编号查询，包括该组下的站点列表 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public SiteGroup Query(string groupId, bool validateData)
        {
            try
            {
                SiteGroup group = dal.Query(groupId);
                if (validateData)
                    validateItems(group);
                return group;
            }
            catch { throw; }
        }

        public SiteGroup Query(int groupIdentity, bool validateData)
        {
            try
            {
                SiteGroup group = dal.Query(groupIdentity);
                if (validateData)
                    validateItems(group);
                return group;
            }
            catch { throw; }
        }

        /// <summary>
        /// 查询所有分组信息，包括该组下的站点列表
        /// </summary>
        /// <param name="validateData">是否校验站点列表</param>
        /// <returns></returns>
        public List<SiteGroup> Query(bool validateData)
        {
            try
            {
                List<SiteGroup> data = dal.Query();
                if (validateData)
                {
                    foreach (SiteGroup group in data)
                        validateItems(group);
                }
                return data;
            }
            catch { throw; }
        }

        /// <summary>
        /// 查询所有分组信息(不包含臭氧)，包括该组下的站点列表
        /// </summary>
        /// <param name="validateData">是否校验站点列表</param>
        /// <returns></returns>
        public List<SiteGroup> QueryNoContainOzone(bool validateData)
        {
            try
            {
                List<SiteGroup> data = dal.QueryNoContainOzone();
                if (validateData)
                {
                    foreach (SiteGroup group in data)
                        validateItems(group);
                }
                return data;
            }
            catch { throw; }
        }

        /// <summary>
        /// 获得全市日报组
        /// </summary>
        /// <returns></returns>
        public SiteGroup QueryCitywideGroup(bool validateData)
        {
            try
            {
                SiteGroup sg = Query(CITYWIDEGROUPCODE,true);
                return sg;
            }
            catch { throw; }
        }


        /// <summary>
        /// 获得全市日AQI报组
        /// </summary>
        /// <returns></returns>
        public SiteGroup QueryCitywideAQIGroup(bool validateData)
        {
            try
            {
                SiteGroup sg = Query(CITYWIDEAQIGROUPCODE, true);
                return sg;
            }
            catch { throw; }
        }


        private void validateItems(SiteGroup group)
        {
            if (null == group)
                return;
            Area area = new Area();
            List<Site> valiData = new List<Site>();
            foreach (Site site in group.Items)
            {
                SiteType st = site.SiteType;
                Site qSite = area.GetSite(site.Id, true);
                if (null != qSite)
                {
                    valiData.Add(qSite);
                    qSite.SiteType = st;
                }
            }
            group.SetItems(valiData);
        }

        public Dictionary<int, Site> GetStateSite()
        {
            List<Site> siteList = QueryCitywideGroup(true).Items;
            Dictionary<int, Site> reData = new Dictionary<int, Site>();
            foreach (Site site in siteList)
                reData[site.Id] = site;
            return reData;
        }

        public static bool IsCitywideGroup(string groupID)
        {
            return CITYWIDEGROUPCODE == groupID;
        }
    }
}
