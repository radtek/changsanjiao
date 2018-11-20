using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Readearth.Data;

namespace MMShareBLL.DAL
{
    public class ServiceManage
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

        public string serviceQuery() 
        {
            m_database = new Database("DBCONFIGII");
            string sql = "SELECT serviceid,address,region,product,secretkey,Receiver,date FROM T_SERVICEINTERFACEMAG";
            DataTable dt = m_database.GetDataTable(sql);
            foreach (DataRow dr in dt.Rows) {
                string type = "";
                foreach (string str in dr["product"].ToString().Split('_')) {
                    switch (str) {
                        case "2": type += "儿童感冒_"; break;
                        case "3": type += "青年感冒_"; break;
                        case "4": type += "老年感冒_"; break;
                        case "5": type += "COPD_"; break;
                        case "6": type += "儿童哮喘_"; break;
                        case "7": type += "中暑_"; break;
                        case "8": type += "重污染_"; break;
                    }
                }
                type = type.TrimEnd('_');
                dr["product"] = type;
            }
            //return dt;
            return DataTableToJson("data", dt);
            //return m_database.GetDataTable(sql);
        }
        public string delService(string s_id) {
            m_database = new Database("DBCONFIGII");
            try { 
                string del = "DELETE FROM T_SERVICEINTERFACEMAG WHERE serviceid='"+s_id+"'";
                m_database.Execute(del);
                return "成功";
            }
            catch(Exception e){
                return "失败：" + e.Message ;
            }
        }

        public string confirm_create(string id,string address, string region, string product, string key,string title,string receiver)
        {
            m_database = new Database("DBCONFIGII");
            try {
                string sql = "";
                product = product.Replace(',', '_');
                if (title.IndexOf("创建") > -1)
                {
                    sql = "INSERT INTO T_SERVICEINTERFACEMAG (address,region,product,secretkey,date,receiver) VALUES('" + address + "','" + region + "','" + product + "','" + key + "',getDate(),'" + receiver + "')";
                }
                else {
                    sql = "UPDATE T_SERVICEINTERFACEMAG SET address='" + address + "',region='" + region + "',product='" + product + "',secretkey='" + key + "',date=GETDATE(),receiver='"+receiver+"' WHERE serviceid=" + id + "";
                }

                m_database.Execute(sql);
                return "成功";
            }
            catch(Exception e){
                return "失败：" + e.Message;
            }
        }
    }
}
