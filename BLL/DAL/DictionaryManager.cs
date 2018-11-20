using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using Readearth.Data;


namespace MMShareBLL.DAL
{
    /// <summary>
    /// 用于获取、管理系统中的字典
    /// </summary>
    public class DictionaryManager
    {
        Database m_Database;

        public DictionaryManager()
        {
            m_Database = new Database();
        }
        public DictionaryManager(Database db)
        {
            m_Database = db;
        }

        public DataTable GetDictionary(string dictionaryName, string orderIndex)
        {
            //string dicTableName = "D_" + dictionaryName;
            string strSQL = "SELECT DM,MC FROM " + dictionaryName;
            if (orderIndex == "-1")
                strSQL = strSQL + " ORDER BY DM DESC";
            try
            {
                DataSet dt = m_Database.GetDataset(strSQL);
                if (dt.Tables.Count > 0)
                {
                    DataTable dTable = dt.Tables[0];
                    return dTable;
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        public DataTable GetDictionaryByDP(string dictionaryName, string dm)
        {
            string strSQL = "SELECT DM,MC FROM " + dictionaryName + " WHERE DP LIKE '%"+ dm +"%'";
            try
            {
                DataSet dt = m_Database.GetDataset(strSQL);
                if (dt.Tables.Count > 0)
                {
                    DataTable dTable = dt.Tables[0];
                    return dTable;
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
