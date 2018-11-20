using System;
using System.Collections.Generic;
using System.Text;
using Readearth.Data;
using System.Data;

namespace MMShareBLL.DAL
{
    public class FTP_Manage
    {
        Database m_database;
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
        //xuehui  2017.6.15
        public string ftpQuery(string regions)
        {
            m_database = new Database("DBCONFIGII");
            String sql = "";
            if (regions == "全部")
            {
                sql = "SELECT ftpID,userName,password,address,port,content,CreatDate,region,reciver,Products FROM FTP_Manage";
            }
            else
            {
                sql = "SELECT ftpID,userName,password,address,port,content,CreatDate,region,reciver,Products FROM FTP_Manage WHERE region='" + regions + "'";
            }
            DataTable dt = m_database.GetDataTable(sql);
            return DataTableToJson("data", dt);
        }

        //xuehui  2017.6.15
        public void confirm(string accouts, string passwords, string addresses, string ports, string regions, string idd, string contents, string reciver, string Products)
        {
            m_database = new Database("DBCONFIGII");
            String sql;
            if (idd != "")
            {
                sql = @"UPDATE FTP_Manage SET userName='" + accouts + "',password='" + passwords + "',address='" + addresses + "',port='" + ports + "',content='" + contents + "',region='" + regions + "',reciver='" + reciver + "',CreatDate= GETDATE(),Products='" + Products + "' WHERE ftpID='" + idd + "'";
            }
            else
            {
                sql = @"INSERT INTO FTP_Manage (userName,password,address,port,region,CreatDate,content,reciver,products) VALUES('" + accouts + "','" + passwords + "','" + addresses + "','" + ports + "','" + regions + "',GETDATE(),'" + contents + "','" + reciver + "','" + Products + "')";
            }
            m_database.Execute(sql);
        }

        public void delFTP(string FTPID)
        {
            m_database = new Database("DBCONFIGII");
            String strDel = "DELETE FROM FTP_Manage WHERE ftpID='" + FTPID + "'";
            m_database.Execute(strDel);
        }

        public int count(string regions)
        {
            m_database = new Database("DBCONFIGII");
            int count;
            String sql = "";
            sql = "SELECT REGION FROM FTP_Manage";
            DataTable dt = m_database.GetDataTable(sql);
            count = dt.Rows.Count;
            return count;
        }



        public DataTable serviceQuery()
        {
            m_database = new Database("DBCONFIGII");
            string sql = "SELECT serviceid,address,region,product,secretkey FROM T_SERVICEINTERFACEMAG";
            DataTable dt = m_database.GetDataTable(sql);
            //for (int i = 0; i < dt.Rows.Count; i++) {
            //    DataRow dr = dt.Rows[i];
            //    for(int j=0;j<dr.)
            //}
            string type = "";
            foreach (DataRow dr in dt.Rows)
            {
                foreach (string str in dr["product"].ToString().Split(','))
                {
                    switch (str)
                    {
                        case "2": type += "儿童感冒,"; break;
                        case "3": type += "青年感冒,"; break;
                        case "4": type += "老年感冒,"; break;
                        case "5": type += "COPD,"; break;
                        case "6": type += "儿童哮喘,"; break;
                        case "7": type += "中暑,"; break;
                        case "8": type += "重污染,"; break;
                    }
                }
                type = type.TrimEnd(',');
                dr["product"] = type;
            }
            return dt;
        }
    }
}
