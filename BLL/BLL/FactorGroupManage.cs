using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using MMShareBLL.DAL;
using MMShareBLL.Model;

namespace MMShareBLL.BLL
{
    public class FactorGroupManage
    {
        private const string CACHEKEY_ALLDATA_DMS = "FACTORSGROUPDATALIST_DMS";
        private const string NUMPRXFIX = "PGC";

        FactorGroupAccess fga = new FactorGroupAccess();
        #region 添加 2
        public bool CreateGroup(FactorsGroup item)
        {
            try
            {
                return fga.CreateGroup(item);
            }
            catch { throw; }
        }
        /// <summary>
        /// 添加因子给组
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CreateGroupItem(FactorGroupItem item)
        {
            try
            {
                return fga.CreateGroupItem(item);
            }
            catch { throw; }
        }
        /// <summary>
        /// 添加因子给组
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CreateGroupItemList(List<FactorGroupItem> itemList)
        {
            if(null==itemList||itemList.Count<1)
                return false;
            bool result=true;
            foreach (FactorGroupItem item in itemList)
            {
                result &= CreateGroupItem(item);
            }
            return result;
        }
        #endregion

        #region 查询 2
        /// <summary>
        /// 返回所有监测因子分组信息
        /// </summary>
        /// <param name="fldDictionaryID"></param>
        /// <returns></returns>
        public DataTable Query()
        {
            try
            {
                return fga.QueryTable();
            }
            catch { throw; }
        }
        /// <summary>
        /// 返回所有监测因子分组信息及分组下监测因子信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, FactorsGroup> QueryDic(bool fromCache)
        {

            try
            {
                Dictionary<int, FactorsGroup> theData = null;
                if (fromCache && CacheManager.Contains(CACHEKEY_ALLDATA_DMS))
                    theData = CacheManager.GetData(CACHEKEY_ALLDATA_DMS) as Dictionary<int, FactorsGroup>;

                if (theData == null || theData.Count<=0)
                {
                    theData = fga.QueryDic();
                    foreach (FactorsGroup fg in theData.Values)
                    {
                        fg.FactorItem = new ParameterAdapter().SortList(fg.FactorItem, false);
                    }
                    CacheManager.SetData(CACHEKEY_ALLDATA_DMS, theData);
                }
                return theData;
            }
            catch { throw; }
        }

        /// <summary>
        /// 返回区县监测因子分组信息及分组下监测因子信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, FactorsGroup> QueryCountyDic(bool fromCache)
        {

            try
            {
                Dictionary<int, FactorsGroup> theData = null;
                if (fromCache && CacheManager.Contains(CACHEKEY_ALLDATA_DMS))
                    theData = CacheManager.GetData(CACHEKEY_ALLDATA_DMS) as Dictionary<int, FactorsGroup>;
                if (theData == null)
                {
                    theData = fga.QueryCountyDic();
                    foreach (FactorsGroup fg in theData.Values)
                    {
                        fg.FactorItem = new ParameterAdapter().SortList(fg.FactorItem, false);
                    }
                    CacheManager.SetData(CACHEKEY_ALLDATA_DMS, theData);
                }
                return theData;
            }
            catch { throw; }
        }



        #endregion

        #region 删除 2
        public bool DeleteGroupItems(string GroupID)
        {
            try
            {
                return fga.DeleteGroupItems( GroupID);
            }
            catch { throw; }
        }
        public bool DeleteGroup(string GroupID)
        {
            try
            {
                return fga.DeleteGroup(GroupID);
            }
            catch { throw; }
        }
        #endregion

        #region 更新 2
        public bool UpdataByID(FactorsGroup item)
        {
            try
            {
                return fga.UpdataByID(item);
            }
            catch { throw; }
        }

        public bool UpdateByGroupID(FactorsGroup item)
        {
            try
            {
                return fga.UpdateByGroupID(item);
            }
            catch { throw; }
        }
        #endregion

        /// <summary>
        /// 生成随机组编号
        /// </summary>
        /// <returns></returns>
        public string buildGroupNum()
        {
            StringBuilder sbNum = new StringBuilder(NUMPRXFIX);
            Random rand = new Random();
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
    }
}
