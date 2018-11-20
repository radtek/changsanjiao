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
    /// Web��Դҵ���������ݷ���
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
        /// ��ȡ���е�ҵ������б�
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
        /// �����ϼ�����Ų�����ֱ���Ӽ�
        /// </summary>
        /// <param name="categoryId">�ϼ������</param>
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
        /// ��������Ų�ѯ�����Ϣ
        /// </summary>
        /// <param name="categoryID">�����</param>
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
        /// ����ָ��������ţ��ж����Ƿ�Ϊ��ײ�Ľڵ�
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
        /// ��ȡB3��WEB��Դ�б�
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

