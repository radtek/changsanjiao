using System;
using System.Collections.Generic;
using System.Text;
using Readearth.Data;
using System.Web;
using System.Data;

namespace MMShareBLL.DAL
{
    public class FileUpload
    {
        private string strSQL;
        private Database m_Database;
        public FileUpload()
        {
            strSQL = "";
            m_Database = new Database("DBCONFIGLN");
        }

        public string GetTable()
        {
            try
            {
                strSQL = "select fileName,fileTitle,fileContent,Type,createTime,creater from T_Files";
                DataTable dt = m_Database.GetDataTable(strSQL);
                return DataTableToJson("data", dt);
            }
            catch(Exception e)
            {
                return e.ToString();
            }
        }

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

        public string UpdateFile(string fileName,string fileTitle,string fileContent) {
            try
            {
                strSQL = "update T_Files set fileTitle='" + fileTitle + "' ,fileContent ='" + fileContent + "' where fileName='" + fileName + "'";
                m_Database.Execute(strSQL);
                return "更新成功！";

            }
            catch(Exception e) { return "更新失败！原因为："+e.ToString(); }
            
        }

        public string DeleteFile(string fileName)
        {
            try
            {
                strSQL = "delete from T_Files where fileName='" + fileName + "'";
                m_Database.Execute(strSQL);
                return "删除成功！";

            }
            catch (Exception e) { return "删除失败！原因为：" + e.Message.ToString(); }

        }
    }

}
