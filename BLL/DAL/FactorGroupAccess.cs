using System;
using System.Collections.Generic;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using MMShareBLL.Model;
using Readearth.Data;


namespace MMShareBLL.DAL
{
    public class FactorGroupAccess
    {
        //Procedures
        /*
--1 添加监测因子分组PRO_FactorGroup_Insert
--2 修改监测因子分组PRO_FactorGroup_UpdateByGroupID
--3 删除监测因子分组－同时删除分组中的因子PRO_FactorGroup_DeleteGroup
--4 添加因子给组PRO_FactorGroup_InsertItemsByGroupID
--5 删除某个组中的所有因子PRO_FactorGroupItems_DeleteByGroupID
--6 查询所有组及其中的因子PRO_FactorGroup_SELECT_InfoAll
--7 查询所有分组PRO_FactorGroup_SELECT
--8 修改，根据分组编号PRO_FactorGroup_UpdateByID
         * 
         * */
        private const string PROC_QUERY1 = "PRO_FactorGroup_SELECT";
        private const string PROC_QUERY2 = "PRO_FactorGroup_SELECT_InfoAll";
        private const string PROC_QUERY3 = "PRO_CountyFactorGroup_SELECT_InfoAll";


        private const string PROC_ADDGROUP = "PRO_FactorGroup_Insert";
        private const string PROC_ADDGROUPITEM = "PRO_FactorGroup_InsertItemsByGroupID";

        private const string PROC_UPDATEGROUP = "PRO_FactorGroup_UpdateByGroupID";
        private const string PROC_UPDATEGROUPBYID = "PRO_FactorGroup_UpdateByID";

        private const string PROC_DELETEGROUP = "PRO_FactorGroup_DeleteGroup";
        private const string PROC_DELETEGROUPITEM = "PRO_FactorGroupItems_DeleteByGroupID";

        //Parameters
        private const string PARAM_ID = "@ID";
        private const string PARAM_GroupID = "@GroupID";
        private const string PARAM_GroupName = "@GroupName";
        private const string PARAM_ParameterId = "@ParameterId";



        private Database m_DatabaseS;
        public FactorGroupAccess()
        {
            m_DatabaseS = new Database("SEMCDMC");
        }

        #region 添加 2

        /// <summary>
        /// --1 添加监测因子分组PRO_FactorGroup_Insert
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CreateGroup(FactorsGroup item)
        {
            try
            {
                SqlParameter[] allParams = getAllParams(item, 2);
                List<SqlParameter> theParams = new List<SqlParameter>();
                theParams.AddRange(allParams);
                //theParams.RemoveAt(0);
                int count = m_DatabaseS.Execute(PROC_ADDGROUP, theParams.ToArray());
               if (count > 0)
                   return true;
            }
            catch { throw; }
            return false;
        }
        /// <summary>
        /// --4 添加因子给组PRO_FactorGroup_InsertItemsByGroupID
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CreateGroupItem(FactorGroupItem item)
        {
            try
            {
                SqlParameter[] allParams = getAllParams2(item,2);
                List<SqlParameter> theParams = new List<SqlParameter>();
                theParams.AddRange(allParams);
                //theParams.RemoveAt(0);
                return m_DatabaseS.Execute(PROC_ADDGROUPITEM, theParams.ToArray()) > 0;
            }
            catch { throw; }
        }
        #endregion

        #region 查询 2

        /// <summary>
        /// --7 查询所有分组PRO_FactorGroup_SELECT
        /// </summary>
        /// <param name="fldDictionaryID"></param>
        /// <returns></returns>
        public DataTable QueryTable()
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(PROC_QUERY1, m_DatabaseS.ConnectionString);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable data = new DataTable();
                adapter.Fill(data);
                return data;
            }
            catch { throw; }
        }

        /// <summary>
        /// --6 查询所有组及其中的因子PRO_FactorGroup_SELECT_InfoAll
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, FactorsGroup> QueryDic()
        {
            try
            {
                Dictionary<int, FactorsGroup> factorData = new Dictionary<int, FactorsGroup>();

                using (SqlDataReader sdr = m_DatabaseS.GetDataReader( PROC_QUERY2))
                {
                    FactorsGroup factor = null;

                    while (sdr.Read())
                    {
                        int cid = Convert.ToInt32(sdr["ID"]);
                        if (factorData.ContainsKey(cid))
                            factor = factorData[cid];
                        else
                        {
                            factor = new FactorsGroup();
                            factorData.Add(cid, factor);
                        }
                        factor.ID = cid;
                        factor.GroupID = sdr["GroupID"] as string;
                        factor.GroupName = sdr["GroupName"] as string;
                        DistillFactorDataDMS(sdr, ref factor);
                    }
                }
                return factorData;
            }
            catch { throw; }
        }

        /// <summary>
        /// --7 查询区县数据查询中的因子PRO_CountyFactorGroup_SELECT_InfoAll
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, FactorsGroup> QueryCountyDic()
        {
            try
            {
                Dictionary<int, FactorsGroup> factorData = new Dictionary<int, FactorsGroup>();

                using (SqlDataReader sdr = m_DatabaseS.GetDataReader(PROC_QUERY3))
                {
                    FactorsGroup factor = null;

                    while (sdr.Read())
                    {
                        int cid = Convert.ToInt32(sdr["ID"]);
                        if (factorData.ContainsKey(cid))
                            factor = factorData[cid];
                        else
                        {
                            factor = new FactorsGroup();
                            factorData.Add(cid, factor);
                        }
                        factor.ID = cid;
                        factor.GroupID = sdr["GroupID"] as string;
                        factor.GroupName = sdr["GroupName"] as string;
                        DistillFactorDataDMS(sdr, ref factor);
                    }
                }
                return factorData;
            }
            catch { throw; }
        }


        public void DistillFactorDataDMS(SqlDataReader sdr, ref FactorsGroup factorGroup)
        {
            if (sdr["ParameterId"] == DBNull.Value)
                return;
            DataParameter fac = new DataParameter();
            fac.Id = Convert.ToInt32(sdr["ParameterID"]);
            fac.Name = sdr["Name"] as string;
            fac.Byname = sdr["Description"] as string;
            fac.Code = sdr["IngestCode"] as string;

            if (DBNull.Value != sdr["UnitID"])
            {
                fac.Unit = new Unit();
                fac.Unit.Id = Convert.ToInt32(sdr["UnitID"]);
                fac.Unit.Name = sdr["Unit"] as string;
            }
            fac.DefaultChecked = false;
            try
            {
                if (sdr["DefaultChecked"] != null && sdr["DefaultChecked"].ToString() != "" && DBNull.Value != sdr["DefaultChecked"])
                {
                    if ("default" == (sdr["DefaultChecked"] as string).ToLower())
                        fac.DefaultChecked = true;
                }
            }
            catch { }
            factorGroup.FactorItem.Add(fac);
        }
        #endregion

        #region 删除 2

        /// <summary>
        /// --5 删除某个组中的所有因子PRO_FactorGroupItems_DeleteByGroupID
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool DeleteGroupItems(string GroupID)
        {
            try
            {
                SqlParameter paramGroupID = new SqlParameter(PARAM_GroupID, SqlDbType.NVarChar, 50);
                paramGroupID.Value = GroupID;
                return m_DatabaseS.Execute( PROC_DELETEGROUPITEM, paramGroupID)> 0;
            }
            catch { throw; }
        }
        /// <summary>
        /// --3 删除监测因子分组－同时删除分组中的因子PRO_FactorGroup_DeleteGroup
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool DeleteGroup(string GroupID)
        {
            try
            {
                SqlParameter paramGroupID = new SqlParameter(PARAM_GroupID, SqlDbType.NVarChar, 50);
                paramGroupID.Value = GroupID;
                return m_DatabaseS.Execute( PROC_DELETEGROUP, paramGroupID) > 0;
            }
            catch { throw; }
        }
        #endregion

        #region 更新 2

        /// <summary>
        /// --8 修改，根据分组编号PRO_FactorGroup_UpdateByID
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool UpdataByID(FactorsGroup item)
        {
            try
            {
                SqlParameter[] allParams = getAllParams(item,3);
                List<SqlParameter> theParams = new List<SqlParameter>();
                theParams.AddRange(allParams);
                //theParams.RemoveAt(0);
                return m_DatabaseS.Execute(PROC_UPDATEGROUPBYID, theParams.ToArray()) > 0;
            }
            catch { throw; }
        }
        /// <summary>
        /// --3 删除监测因子分组－同时删除分组中的因子PRO_FactorGroup_DeleteGroup
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool UpdateByGroupID(FactorsGroup item)
        {
            try
            {
                SqlParameter[] allParams = getAllParams(item,3);
                List<SqlParameter> theParams = new List<SqlParameter>();
                theParams.AddRange(allParams);
                //theParams.RemoveAt(0);
                return m_DatabaseS.Execute( PROC_UPDATEGROUP, theParams.ToArray()) >0;
            }
            catch { throw; }
        }
        #endregion

        private SqlParameter[] getAllParams(FactorsGroup item, int x)
        {
            SqlParameter[] theParams;
            if (x == 3)
            {
                theParams = new SqlParameter[]{
                new SqlParameter(PARAM_ID,SqlDbType.Int),
                new SqlParameter(PARAM_GroupID,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_GroupName,SqlDbType.NVarChar,50)
            };

                theParams[0].Value = item.ID;
                theParams[1].Value = item.GroupID;
                theParams[2].Value = item.GroupName;
                return theParams;
            }
            if (x == 2)
            {
                theParams = new SqlParameter[]{
                new SqlParameter(PARAM_GroupID,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_GroupName,SqlDbType.NVarChar,50)
            };
                theParams[0].Value = item.GroupID;
                theParams[1].Value = item.GroupName;
                return theParams;
            }
            if (x == 1)
            {
                theParams = new SqlParameter[]{
                new SqlParameter(PARAM_GroupID,SqlDbType.NVarChar,50)};
                return theParams;
            }
            else
            {
                theParams = new SqlParameter[]{
                new SqlParameter(PARAM_ID,SqlDbType.Int)
            };
                theParams[0].Value = item.ID;
            }
            return theParams;
        }
        private SqlParameter[] getAllParams2(FactorGroupItem item,int x)
        {
            SqlParameter[] theParams;
            if (x == 3)
            {
                theParams = new SqlParameter[]{
                new SqlParameter(PARAM_ID,SqlDbType.Int),
                new SqlParameter(PARAM_GroupID,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_ParameterId,SqlDbType.Int)

            };
                theParams[0].Value = item.ID;
                theParams[1].Value = item.GroupID;
                theParams[2].Value = item.ParameterId;
                return theParams;
            }
            if (x == 2)
            {
                theParams = new SqlParameter[]{
                new SqlParameter(PARAM_GroupID,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_ParameterId,SqlDbType.Int)
            };
                theParams[0].Value = item.GroupID;
                theParams[1].Value = item.ParameterId;
                return theParams;
            }
            else
            {
                theParams = new SqlParameter[]{
                new SqlParameter(PARAM_GroupID,SqlDbType.NVarChar,50)
            };
                theParams[0].Value = item.GroupID;
            }

            return theParams;
        }
        /*
        public List<FactorsGroup> Query(int fldValueID)
        {
            try
            {
                SqlParameter paramFldValueID = new SqlParameter(PARAM_FldValueID, SqlDbType.Int);
                List<HazeLevelDicValue> theData = new List<HazeLevelDicValue>();
                using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.StoredProcedure, PROC_QUERYBYID, paramFldValueID))
                {
                    while (reader.Read())
                    {
                        HazeLevelDicValue haze = new HazeLevelDicValue();

                        haze.FldValueID = Convert.ToInt32(reader["FldValueID"]);
                        haze.FldDictionaryID = Convert.ToInt32(reader["FldDictionaryID"]);
                        haze.FldKey = reader["FldKey"] as string;
                        haze.FldValue = reader["FldValue"] as string;
                        haze.FldStandbyField1 = reader["FldStandbyField1"] as string;
                        haze.FldStandbyField2 = reader["FldStandbyField2"] as string;
                        haze.fldIsDel = Convert.ToInt32(reader["fldIsDel"]);
                        haze.FldAddTime = DateTime.Parse(reader["FldAddTime"].ToString());
                        haze.FldUpdateTime = DateTime.Parse(reader["FldUpdateTime"].ToString());

                        theData.Add(haze);
                    }
                }
                return theData;
            }
            catch { throw; }
        }

        public bool Update(HazeLevelDicValue item)
        {
            try
            {
                item.FldDictionaryID = FldDictionaryID;
                SqlParameter[] theParams = getAllParams(item);
                return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.StoredProcedure, PROC_UPDATE, theParams) == 1;
            }
            catch { throw; }
        }

        public bool Remove(int id)
        {
            try
            {
                SqlParameter paramId = new SqlParameter(PARAM_FldValueID, SqlDbType.Int);
                paramId.Value = id;
                return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.StoredProcedure, PROC_REMOVE, paramId) == 1;
            }
            catch { throw; }
        }*/
    }
}
