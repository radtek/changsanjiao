using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using Littlestarquan.BOSP.DataAccessLibrary;
using MMShareBLL.Model;
using Readearth.Data;


namespace MMShareBLL.DAL
{
    /// <summary>
    /// 站点分组的数据管理
    /// </summary>
    public class SiteGroupDataAccess : DataAccessComponent<SiteGroup>
    {
        //Procedures
        private const string PROC_ADD = "D_SiteGroup_ADD";
        private const string PROC_UPDATE = "D_SiteGroup_Update";
        private const string PROC_DELETE = "D_SiteGroup_Delete";
        private const string PROC_SELECT = "D_SiteGroup_SELECT";
        private const string PROC_SELECT_NOTCONTAINOZONE = "D_SiteGroup_SELECTNOTCONTAINOZONE";
        private const string PROC_SELECTBYID = "D_SiteGroup_SELECTBYID";
        private const string PROC_SELECTBYIDENTITY = "D_SiteGroup_SELECTBYIDENTITY";
        private const string PROC_EXISTS = "D_SiteGroup_Exists";

        private const string PROC_ITEM_ADD = "D_SiteGroupItems_ADD";
        private const string PROC_ITEM_DELETE = "D_SiteGroupItems_DeleteByGroup";
        private const string PORC_ITEM_DELETE_BY_GROUPID_SITE = "D_SiteGroupItems_DeleteByGroupAndSite";
        private const string PROC_ITEM_SELECT = "D_SiteGroupItems_SelectByGroupID";


        //Parameters
        private const string PARAM_GROUPID = "@GroupID";
        private const string PARAM_GROUPNAME = "@GroupName";
        private Database m_DatabaseS;
        public SiteGroupDataAccess()
        {
            CONST_COMMAND_DELETE = PROC_DELETE;
            CONST_COMMAND_INSERT = PROC_ADD;
            CONST_COMMAND_UPDATE = PROC_UPDATE;
            m_DatabaseS = new Database("conStr_SEMC_DMC");//new conStr_SEMC_DMC  SEMCDMC
        }

        public int CreateGroup(SiteGroup group)
        {
            int rowAff = 0;
            using (ComponentTransaction tran = ComponentTransaction.BeginComponentTransaction(this))
            {
                try
                {

                    rowAff += this.Add(group, "GroupID", "GroupName");
                    this.CONST_COMMAND_INSERT = PROC_ITEM_ADD;
                    foreach (Site site in group.Items)
                    {
                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("GroupID", group.GroupID);
                        parameters.Add("SiteID", site.Id);
                        parameters.Add("SiteType", site.SiteType);
                        rowAff += this.Add(null, parameters);
                    }
                    tran.CommitTransaction();
                }
                catch { tran.RollbackTransaction(); throw; }
            }
            return rowAff;
        }

        public bool AppendItem(string groupID, Site site)
        {
            try
            {
                this.CONST_COMMAND_INSERT = PROC_ITEM_ADD;
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("GroupID", groupID);
                parameters.Add("SiteID", site.Id);
                parameters.Add("SiteType", site.SiteType);
                this.Add(null, parameters);
                return true;
            }
            catch { return false; }
        }

        public bool Exists(string groupName, string groupID)
        {
            SqlParameter paramGroupName = new SqlParameter(PARAM_GROUPNAME, SqlDbType.NVarChar, 50);
            paramGroupName.Value = groupName;
            SqlParameter paramGroupID = new SqlParameter(PARAM_GROUPID, SqlDbType.NVarChar, 100);
            if (string.IsNullOrEmpty(groupID))
                paramGroupID.Value = DBNull.Value;
            else
                paramGroupID.Value = groupID;
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(paramGroupName);
            parameters.Add(paramGroupID);
            object reCount = DataBase.GetStoredProcedureExecuteScalar(PROC_EXISTS, parameters);
            if (DBNull.Value != reCount && null != reCount)
            {
                int count = 0;
                int.TryParse(reCount.ToString(), out count);
                return count != 0;
            }
            return false;
        }

        public int Update(string groupId, string groupName)
        {
            SqlParameter paramGroupId = new SqlParameter(PARAM_GROUPID, SqlDbType.NVarChar, 50);
            SqlParameter paramGroupName = new SqlParameter(PARAM_GROUPNAME, SqlDbType.NVarChar, 50);
            paramGroupId.Value = groupId;
            paramGroupName.Value = groupName;

            return m_DatabaseS.Execute(PROC_UPDATE, paramGroupName, paramGroupId);

        }

        public int Update(SiteGroup group)
        {
            int rowAff = 0;
            using (ComponentTransaction tran = ComponentTransaction.BeginComponentTransaction(this))
            {
                try
                {
                    rowAff += this.Update(group, "GroupID", "GroupName");
                    this.CONST_COMMAND_DELETE = PROC_ITEM_DELETE;
                    Dictionary<string, object> param_remove = new Dictionary<string, object>();
                    param_remove["GroupID"] = group.GroupID;
                    rowAff += this.Delete(null, param_remove);
                    this.CONST_COMMAND_INSERT = PROC_ITEM_ADD;
                    foreach (Site site in group.Items)
                    {
                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("GroupID", group.GroupID);
                        parameters.Add("SiteID", site.Id);
                        parameters.Add("SiteType", site.SiteType);
                        rowAff += this.Add(null, parameters);
                    }
                    tran.CommitTransaction();
                }
                catch { tran.RollbackTransaction(); throw; }
            }
            return rowAff;
        }

        /// <summary>
        /// 删除站点组
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public int Remove(string groupId)
        {
            SqlParameter paramGroupId = new SqlParameter(PARAM_GROUPID, SqlDbType.NVarChar, 50);
            paramGroupId.Value = groupId;
            return m_DatabaseS.Execute(PROC_DELETE, paramGroupId);
        }

        public int Remove(string groupId, List<int> sites)
        {
            int rowAff = 0;
            this.CONST_COMMAND_DELETE = PORC_ITEM_DELETE_BY_GROUPID_SITE;
            using (ComponentTransaction tran = ComponentTransaction.BeginComponentTransaction(this))
            {
                try
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("GroupID", groupId);
                    parameters.Add("SiteID", 0);
                    foreach (int siteId in sites)
                    {
                        parameters["SiteID"] = siteId;
                        rowAff += this.Delete(null, parameters);
                    }
                    tran.CommitTransaction();
                }
                catch { tran.RollbackTransaction(); throw; }
            }
            return rowAff;
        }

        /// <summary>
        /// 根据组编号查询
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public SiteGroup Query(string groupId)
        {
            SqlParameter paramGroupId = new SqlParameter(PARAM_GROUPID, SqlDbType.NVarChar, 50);
            paramGroupId.Value = groupId;
            SiteGroup data = null;
            using (SqlDataReader reader = m_DatabaseS.GetDataReader(PROC_SELECTBYID, paramGroupId))
            {
                List<SiteGroup> queryResult = pickupData(reader);
                if (queryResult.Count > 0)
                    return queryResult[0];
            }
            return data;
        }

        public SiteGroup Query(int groupIdentity)
        {
            SqlParameter paramGroupId = new SqlParameter(PARAM_GROUPID, SqlDbType.NVarChar, 50);
            paramGroupId.Value = groupIdentity;
            SiteGroup data = null;
            using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_SELECTBYIDENTITY, paramGroupId))
            {
                List<SiteGroup> queryResult = pickupData(reader);
                if (queryResult.Count > 0)
                    return queryResult[0];
            }
            return data;
        }

        /// <summary>
        /// 查询所有站点组
        /// </summary>
        /// <returns></returns>
        public List<SiteGroup> Query()
        {
            using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_SELECT))
            {
                return pickupData(reader);
            }
        }

        /// <summary>
        /// 查询所有站点组
        /// </summary>
        /// <returns></returns>
        public List<SiteGroup> QueryNoContainOzone()
        {
            using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_SELECT_NOTCONTAINOZONE))
            {
                return pickupData(reader);
            }
        }

        private List<SiteGroup> pickupData(SqlDataReader reader)
        {
            List<SiteGroup> dataList = new List<SiteGroup>();
            Dictionary<int, SiteGroup> dictData = new Dictionary<int, SiteGroup>();

            while (reader.Read())
            {
                SiteGroup data = null;
                int id = reader.GetInt32(0);
                if (dictData.ContainsKey(id))
                    data = dictData[id];
                else
                {
                    data = new SiteGroup();
                    data.ID = id;
                    data.GroupID = reader.GetString(1);
                    data.GroupName = reader.GetString(2);
                    data.ReadOnly = reader.GetBoolean(3);
                    dictData[id] = data;
                }
                object siteId = reader[4];
                if (DBNull.Value != siteId)
                {
                    Site site = new Site(Convert.ToInt32(siteId), null);
                    object objSiteType = reader[5];
                    if (DBNull.Value != objSiteType)
                        site.SiteType = (SiteType)Convert.ToInt32(objSiteType);
                    data.Items.Add(site);
                }
            }
            foreach (SiteGroup sg in dictData.Values)
                dataList.Add(sg);
            return dataList;
        }
    }
}
