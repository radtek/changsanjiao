using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

using System.Collections.Generic;
using MMShareBLL.Model;
using Readearth.Data;

namespace MMShareBLL.DAL
{
    /// <summary>
    /// Web资源业务类别的数据访问
    /// </summary>
    public class OperationTypeDal
    {
        //PORCEDURES
        private readonly string PROC_GETALL = "UP_TblOperationType_GetList";
        private readonly string PROC_GETBY_PARENT = "UP_TblOperationType_GetListBySupperID";
        private readonly string PROC_GETBY_CATEGORYID = "UP_TblOperationType_QueryByID";
        private readonly string PROC_CHECK_ISFINALLYNODE = "UP_TblOperationType_CheckIsFinallyNode";
        private readonly string PROC_GET_B3 = "getB3_OperationType";


        //Parameters
        private readonly string PARM_PARENT_ID = "@supperID";
        private readonly string PARAM_CATEGORY_ID = "@categoryID";
        private readonly string PARAM_ISFINALLYNODE = "@isFinallyNode";


   private Database m_DatabaseS;
   public OperationTypeDal()
   {
            m_DatabaseS = new Database("SEMCDMC");
        }

        /// <summary>
        /// 获取所有的业务类别列表
        /// </summary>
        public List<OperationType> Query()
        {
            List<OperationType> reList = new List<OperationType>();
            using (SqlDataReader sdr = m_DatabaseS.GetDataReader(PROC_GETALL))
            {
                while (sdr.Read())
                {
                    OperationType model = distillData(sdr);
                    reList.Add(model);
                }
            }
            return reList;
        }

        /// <summary>
        /// 根据上级类别编号查找其直接子级
        /// </summary>
        /// <param name="categoryId">上级类别编号</param>
        /// <returns></returns>
        public List<OperationType> Query(int categoryId)
        {
            List<OperationType> reList = new List<OperationType>();
            SqlParameter paramSupperID = new SqlParameter(PARM_PARENT_ID, SqlDbType.Int);
            paramSupperID.Value = categoryId;
            using (SqlDataReader sdr = m_DatabaseS.GetDataReader( PROC_GETBY_PARENT, paramSupperID))
            {
                while (sdr.Read())
                {
                    OperationType model = distillData(sdr);
                    reList.Add(model);
                }
            }
            return reList;
        }

        /// <summary>
        /// 根据类别编号查询类别信息
        /// </summary>
        /// <param name="categoryID">类别编号</param>
        /// <returns></returns>
        public OperationType GetByID(int categoryID)
        {
            OperationType model = null;
            SqlParameter paramCategoryID = new SqlParameter(PARAM_CATEGORY_ID, SqlDbType.Int);
            paramCategoryID.Value = categoryID;
            using (SqlDataReader sdr = m_DatabaseS.GetDataReader( PROC_GETBY_CATEGORYID, paramCategoryID))
            {
                if (sdr.Read())
                    model = distillData(sdr);
            }
            return model;
        }

        /// <summary>
        /// 传入指定的类别编号，判断其是否为最底层的节点
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public bool CheckFinallyNode(int categoryID)
        {
            SqlParameter paramCategoryID = new SqlParameter(PARAM_CATEGORY_ID, SqlDbType.Int);
            paramCategoryID.Value = categoryID;
            SqlParameter paramIsFinallyNode = new SqlParameter(PARAM_ISFINALLYNODE, SqlDbType.Bit);
            paramIsFinallyNode.Direction = ParameterDirection.Output;
            m_DatabaseS.Execute( PROC_CHECK_ISFINALLYNODE, paramIsFinallyNode, paramCategoryID);
            return Convert.ToBoolean(paramIsFinallyNode.Value);
        }

        /// <summary>
        /// 获取B3类WEB资源列表
        /// </summary>
        /// <returns></returns>
        public List<int> GetB3Class()
        {
            List<int> b3List = new List<int>();
            using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_GET_B3))
            {
                while (reader.Read())
                {
                    b3List.Add(Convert.ToInt32(reader[0]));
                }
            }
            return b3List;
        }

        public OperationType distillData(SqlDataReader sdr)
        {
            OperationType model = new OperationType();
            model.ID = Convert.ToInt32(sdr["fldOID"]);
            model.Name = sdr["fldOPName"] as string;

            if (sdr["fldOPonesupNO"] != DBNull.Value)
                model.OnesupNO = Convert.ToInt32(sdr["fldOPonesupNO"]);
            if (sdr["fldOPtwosupNO"] != DBNull.Value)
                model.TwosupNO = Convert.ToInt32(sdr["fldOPtwosupNO"]);

            model.Remark = sdr["fldOPremark"] as string;
            model.IsDefaultView = false;
            if (sdr["fldDefaultView"] != DBNull.Value)
                model.IsDefaultView = Convert.ToBoolean(sdr["fldDefaultView"]);
            if (Pub.ExistsColumn(sdr, "classStr"))
                model.Type = sdr["classStr"] as string;
            if (Pub.ExistsColumn(sdr, "urlRegexStr"))
                model.BaseUrl = sdr["urlRegexStr"] as string;

            return model;
        }

    }
}

