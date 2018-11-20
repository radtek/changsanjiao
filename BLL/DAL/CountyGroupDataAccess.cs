using System;
using System.Collections.Generic;
using System.Text;
using Littlestarquan.BOSP.DataAccessLibrary;

using System.Data.SqlClient;
using MMShareBLL.Model;
using Readearth.Data;


namespace MMShareBLL.DAL
{
    public class CountyGroupDataAccess : DataAccessComponent<CountyGroup>
    {
        private const string PORC_COUNTYGROUP_ADD = "D_CountyGroup_ADD";
        private const string PROC_COUNTYGROUP_CLEAR = "D_CountyGroup_Clear";
        private const string PROC_COUNTYGROUP_SELECT = "D_CountyGroup_SELECT";
        private Database m_DatabaseS;
        public CountyGroupDataAccess()
        {
            this.CONST_COMMAND_INSERT = PORC_COUNTYGROUP_ADD;
            this.CONST_COMMAND_SELECT = PROC_COUNTYGROUP_SELECT;
            this.CONST_COMMAND_DELETE = PROC_COUNTYGROUP_CLEAR;
            m_DatabaseS = new Database("conStr_SEMC_DMC");//new 
        }

        public int SaveCountyGroup(CountyGroup group)
        {
            int rowAff = 0;
            using (ComponentTransaction tran = ComponentTransaction.BeginComponentTransaction(this))
            {
                try
                {
                    rowAff += this.Delete(null, new Dictionary<string, object>());
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("GroupID", null);
                    foreach (SiteGroup sg in group.Items)
                    {
                        parameters["GroupID"] = sg.GroupID;
                        rowAff += this.Add(null, parameters);
                    }
                    tran.CommitTransaction();
                }
                catch { tran.RollbackTransaction(); throw; }
            }
            return rowAff;
        }

        public List<SiteGroup> Query()
        {
            List<SiteGroup> data = new List<SiteGroup>();
            using (SqlDataReader reader = m_DatabaseS.GetDataReader(PROC_COUNTYGROUP_SELECT))
            {
                while (reader.Read())
                {
                    SiteGroup sg = new SiteGroup();
                    sg.GroupID = reader[1] as string;
                    data.Add(sg);
                }
            }
            return data;
        }
    }
}
