using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

using System.Data;
using Readearth.Data;

namespace MMShareBLL.DAL
{
    /// <summary>
    /// 系统变量
    /// </summary>
    public class SysetmConfig
    {        
        private Database m_DatabaseS;
        private readonly string PROC_SELECT = "S_System_Select";
        private readonly string PROC_UPDATE_CACHEENABLED = "S_System_Update_CacheEnabled";

        private readonly string PARAM_ENABLEDCACHE = "@enabledCache";


        public SysetmConfig()
        {
             m_DatabaseS = new Database("SEMCDMC");
        }

        public object[] GetSystemVariable()
        {
            List<object> reList = new List<object>();
            try
            {
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_SELECT))
                {
                    if (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                            reList.Add(reader[i]);
                    }
                }
                return reList.ToArray();
            }
            catch { throw; }
        }

        public void UpdateCacheEnabled(bool enabled)
        {
            SqlParameter paramEnabled = new SqlParameter(PARAM_ENABLEDCACHE, SqlDbType.Bit);
            paramEnabled.Value = enabled;

            try
            {
                m_DatabaseS.Execute( PROC_UPDATE_CACHEENABLED, paramEnabled);
            }
            catch { throw; }
        }
    }
}
