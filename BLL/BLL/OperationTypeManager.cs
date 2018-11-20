using System;
using System.Data;
using System.Collections.Generic;
using MMShareBLL.DAL;
using MMShareBLL.Model;




namespace MMShareBLL.BLL
{
    /// <summary>
    /// WEB资源类别的管理
    /// </summary>
    public class OperationTypeManager
    {
        private readonly OperationTypeDal dal = new OperationTypeDal();
        private const string CACHEKEY = "CategoryList";
        private const string CACHEKEY_B3 = "CategoryList_B3";
        private const string CACHEKEY_LASTNODELIST = "CATEGORY_LASTNODES";

        /// <summary>
        /// 获取所有的业务类别列表
        /// </summary>
        public List<OperationType> Query(bool fromCache)
        {
            List<OperationType> reList = null;
            if (fromCache && CacheManager.Contains(CACHEKEY))
                reList = CacheManager.GetData(CACHEKEY) as List<OperationType>;
            if (null == reList)
            {
                reList = dal.Query();
                CacheManager.SetData(CACHEKEY, reList);
            }
            return reList;
        }

        /// <summary>
        /// 获取所有的叶节点
        /// </summary>
        /// <param name="fromCache"></param>
        /// <returns></returns>
        public List<OperationType> GetAllLastNodes(bool fromCache)
        {
            List<OperationType> reList = null;
            if (fromCache && CacheManager.Contains(CACHEKEY))
                reList = CacheManager.GetData(CACHEKEY_LASTNODELIST) as List<OperationType>;
            if (null == reList)
            {
                reList = new List<OperationType>();
                List<OperationType> allCategories = Query(fromCache);
                foreach (OperationType category in allCategories)
                {
                    if (dal.CheckFinallyNode(category.ID))
                        reList.Add(category.Clone());
                }
                CacheManager.SetData(CACHEKEY_LASTNODELIST, reList);
            }
            return reList;
        }

        /// <summary>
        /// 根据上级类别编号查找其直接子级
        /// </summary>
        /// <param name="categoryId">上级类别编号</param>
        /// <returns></returns>
        public List<OperationType> Query(int parentId)
        {
            return dal.Query(parentId);
        }

        /// <summary>
        /// 根据类别编号查询类别信息
        /// </summary>
        /// <param name="categoryID">类别编号</param>
        /// <returns></returns>
        public OperationType GetByID(int categoryID)
        {
            return dal.GetByID(categoryID);
        }

        /// <summary>
        /// 根据编号查询
        /// </summary>
        /// <param name="categoryID"></param>
        /// <param name="fromCache"></param>
        /// <returns></returns>
        public OperationType GetByID(int categoryID, bool fromCache)
        {
            OperationType theType = null;
            if (fromCache)
            {
                List<OperationType> theTypeList = Query(true);
                foreach (OperationType opType in theTypeList)
                {
                    if (opType.ID == categoryID)
                    {
                        theType = opType.Clone();
                        break;
                    }
                }
            }
            else
                theType = GetByID(categoryID);
            return theType;
        }

        public void getChild(List<OperationType> categoryArray, int parentID, bool isFinallyNode)
        {
            List<OperationType> allCategory = Query(true);

            foreach (OperationType category in allCategory)
            {
                if (category.TwosupNO == parentID)
                {
                    DownContext dc = new DownContext();
                    if (!isFinallyNode)
                        categoryArray.Add(category.Clone());
                    if (isFinallyNode && CheckFinallyNode(category.ID))
                        categoryArray.Add(category.Clone());
                    getChild(categoryArray, category.ID, isFinallyNode);
                }
            }

        }

        /// <summary>
        /// 传入指定的类别编号，判断其是否为最底层的节点
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public bool CheckFinallyNode(int categoryID)
        {
            return dal.CheckFinallyNode(categoryID);
        }

        /// <summary>
        /// 取得B3类WEB资源列表
        /// </summary>
        /// <param name="fromCache">是否从缓存中加载此项</param>
        /// <returns></returns>
        public List<int> GetB3Category(bool fromCache)
        {
            List<int> theData = null;
            if (fromCache && CacheManager.Contains(CACHEKEY_B3))
                theData = CacheManager.GetData(CACHEKEY_B3) as List<int>;
            if (null == theData)
            {
                theData = dal.GetB3Class();
                if (fromCache)
                    CacheManager.SetData(CACHEKEY_B3, theData);
            }
            return theData;
        }

        Dictionary<int, OperationType> dictCategories = null;

        Dictionary<int, OperationType> usedCategories = null;

        /// <summary>
        /// 获取有上下关系的WEB资源类别
        /// </summary>
        /// <returns></returns>
        public List<OperationType> GetRelationData()
        {
            List<OperationType> reList = null;
            if (null != CacheManager.OperationTypeCache.RelationData)
                reList = CacheManager.OperationTypeCache.RelationData;
            if (null == reList)
            {
                reList = new List<OperationType>();
                dictCategories = new Dictionary<int, OperationType>();
                usedCategories = new Dictionary<int, OperationType>();

                List<OperationType> categories = Query(true);

                foreach (OperationType ot in categories)
                {
                    dictCategories[ot.ID] = ot.Clone();
                }
                foreach (OperationType ot in categories)
                {
                    if (usedCategories.ContainsKey(ot.ID))
                        continue;
                    OperationType theType = appendParent(ot.Clone());
                    if (null == theType.Parent && !reList.Contains(theType))
                        reList.Add(theType);
                }
            }
            return reList;
        }

        /// <summary>
        /// 添加父类别
        /// </summary>
        /// <param name="ot"></param>
        /// <returns></returns>
        private OperationType appendParent(OperationType ot)
        {
            usedCategories[ot.ID] = ot;
            if (null != ot.TwosupNO)
            {
                OperationType theParent = null;
                if (usedCategories.ContainsKey(ot.TwosupNO.Value))
                {
                    theParent = usedCategories[ot.TwosupNO.Value];
                    theParent.Items[ot.ID] = ot;
                    ot.Parent = theParent;
                    return theParent;
                }
                else if (dictCategories.ContainsKey(ot.TwosupNO.Value))
                {
                    theParent = dictCategories[ot.TwosupNO.Value].Clone();
                    theParent.Items[ot.ID] = ot;
                    ot.Parent = theParent;
                    return appendParent(theParent);
                }
            }
            return ot;
        }
    }
}

