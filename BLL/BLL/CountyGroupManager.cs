using System.Collections.Generic;
using MMShareBLL.Model;
using MMShareBLL.DAL;


namespace  MMShareBLL.BLL
{
    /// <summary>
    /// 提供分区日报组的管理
    /// </summary>
    public class CountyGroupManager
    {
        CountyGroupDataAccess dal = new CountyGroupDataAccess();

        /// <summary>
        /// 保存分组数据
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public int SaveCountyGroup(CountyGroup group)
        {
            try
            {
                return dal.SaveCountyGroup(group);
            }
            catch { throw; }
        }

        /// <summary>
        /// 查询分区日报组
        /// </summary>
        /// <returns></returns>
        public List<SiteGroup> Query()
        {
            try
            {
                List<SiteGroup> dalData = dal.Query();
                List<SiteGroup> validateData = new List<SiteGroup>();
                SiteGroupManager sgm = new SiteGroupManager();
                foreach (SiteGroup sg in dalData)
                {
                    SiteGroup qData = sgm.Query(sg.GroupID, false);
                    if (null != qData)
                        validateData.Add(qData);
                }
                return validateData;
            }
            catch { throw; }
        }
        /// <summary>
        /// 查询分区日报组
        /// </summary>
        /// <returns></returns>
        public List<SiteGroup> Query(bool isvalidateData)
        {
            try
            {
                List<SiteGroup> dalData = dal.Query();
                List<SiteGroup> validateData = new List<SiteGroup>();
                SiteGroupManager sgm = new SiteGroupManager();
                foreach (SiteGroup sg in dalData)
                {
                    SiteGroup qData = sgm.Query(sg.GroupID, isvalidateData);
                    if (null != qData)
                        validateData.Add(qData);
                }
                return validateData;
            }
            catch { throw; }
        }
    }
}
