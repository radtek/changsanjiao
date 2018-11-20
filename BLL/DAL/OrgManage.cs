using System;
using System.Collections.Generic;
using System.Text;
using Readearth.Data;
using System.Data;

namespace MMShareBLL.DAL
{
   public class OrgManage
    {
        public Database m_Database;
        /// <summary>
        /// DataTable to json
        /// </summary>
        /// <param name="jsonName">返回json的名称</param>
        /// <param name="dt">转换成json的表</param>
        /// <returns></returns>
        private string DataTableToJson(string jsonName, System.Data.DataTable dt)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("[");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append("\"" + dt.Rows[i][j].ToString() + "\"");
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("]");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }

        public string GetData() {
            m_Database = new Database("DBCONFIGII");
            String sql = "SELECT id,description,org FROM T_ShanghaiArea";
            DataTable dt=m_Database.GetDataTable(sql);
            return DataTableToJson("data",dt);
        }
        public void delOrg(string idd) {
            m_Database = new Database("DBCONFIGII");
            String del = "DELECT FROM T_ShanghaiArea WHERE ID='" + idd + "'";
            m_Database.Execute(del);
        }
        public void edit(string idd, string regions, string orgName)
        {
            m_Database = new Database("DBCONFIGII");
            String insert = "UPDATE T_ShanghaiArea SET description='" + regions + "',org='" + orgName + "' WHERE ID='" + idd + "'";
            m_Database.Execute(insert);
        }
    }
}
