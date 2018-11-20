using System;
using System.Text;
using System.Data;
using Readearth.Data;
using System.Configuration;
using System.IO;

namespace MMShareBLL.DAL
{
    public class WarningMsg { 

        private Database m_Database;

        private string ftpIP;
        private string ftpUser;
        private string ftpPwd;
    //     <add key = "ftpIP1" value="127.0.0.1"/>
    //<add key = "ftpUser1" value="administrator"/>
    //<add key = "ftpPwd1" value="diting2018"/>
        public WarningMsg()
        {
            m_Database = new Database();
            ftpIP = ConfigurationManager.AppSettings["ftpIPWarn"];
            ftpUser = ConfigurationManager.AppSettings["ftpUserWarn"];
            ftpPwd = ConfigurationManager.AppSettings["ftpPwdWarn"];
        }

        public static void SortAsFileCreationTime(ref FileInfo[] arrFi)
        {
            Array.Sort(arrFi, delegate (FileInfo x, FileInfo y) { return y.CreationTime.CompareTo(x.CreationTime); });
        }
    
    /// <summary>
    /// 将数据上传到ftp上
    /// </summary>
    /// <param name="sendmessage"></param>
    /// <returns></returns>
    public string SendFtp(string sendmessage,string warninggrade,string beforeUpdate,string user,string title)
        {
            Ftp ftp = new Ftp(ftpIP, ftpUser, ftpPwd);
            string dir = ConfigurationManager.AppSettings["warnSaveTxt"];
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dir);
            DirectoryInfo di = new DirectoryInfo(filePath);
            FileInfo[] arrFi = di.GetFiles();
            SortAsFileCreationTime(ref arrFi);
            string fileSaveFullName = Path.Combine(filePath, arrFi[0].ToString());   //这里由于需求改了直接从保存的文本度
            sendmessage = File.ReadAllText(fileSaveFullName);
            string status = "ok";
            DateTime dNow = DateTime.Now;   //时间写在这里尽量保证每一次循环所发送的文件的名称不一样
            string time = dNow.ToString("yyyyMMddHHmm");
            string fileName = arrFi[0].ToString();
            string sqlInsert = "insert into T_WarningReport (id,[user],PublicTime,Title,updatebefor,warninggrade,Content,duration,status) values ('"+time+"',"+
                "'"+user+"','"+dNow.ToString("yyyy-MM-dd HH:mm:ss")+"','"+title+"','"+beforeUpdate+"','"+warninggrade+"','"+sendmessage+"','0','{0}')";
            string direct = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ftpTextWaring");
            if (!Directory.Exists(direct)) {
                Directory.CreateDirectory(direct);
            }
            string fileFullName = Path.Combine(direct, fileName);
            File.WriteAllText(fileFullName, sendmessage);
            try
            {
                ftp.Upload(fileFullName, fileName);    //第一个目录是本地目录，第二个是ftp目录，不包含地址
                sqlInsert = string.Format(sqlInsert,"成功");
            }
            catch (Exception ex)
            {
                sqlInsert = string.Format(sqlInsert, "失败");
                status = "error"+ ex.Message;
                File.Delete(fileFullName);
            }
            m_Database.Execute(sqlInsert);
            return status;
        }
        public string GetWarningTable(string text, string type,string status)
        {
            string condition = "";
            if (text == "全部")
                condition = "1=1 ";
            else
                condition = "[Title]='" + text + "' ";

            if (type == "全部")
                condition += "and 1=1 ";
            else
                condition += "and [warninggrade]='" + type + "' ";
            if (status == "全部")
                condition += "and 1=1 ";
            else
                condition += "and [status]='" + status + "' ";

            string sql = string.Format("Select [ID],[User],[PublicTime],[Title],[warninggrade],[status],[Content] from [T_WarningReport] Where "+condition+" order by PublicTime desc ",text,type);
            DataTable tbUsers = m_Database.GetDataTable(sql);
            string jsonStr = DataTableToJson("data", tbUsers);
            return jsonStr;
        }

        public string GetWarningType(string text, string type)
        {
            string sql = "Select * from [D_WarningType]";
            DataTable tbUsers = m_Database.GetDataTable(sql);
            string jsonStr = DataTableToJson("data", tbUsers);
            return jsonStr;
        }

        public string DelWarningMsg(string IwaningNum)
        {
            string excode = "删除条数为零";
            string sql = "Delete [T_WarningReport] Where [ID] = '" + IwaningNum + "'";
            try
            {
                int num = m_Database.Execute(sql);
                if (num > 0)
                   return "OK";
            }
            catch (Exception ex)
            {
                excode = ex.Message;

            }
            return excode;
        }


        public string InsertMsg(string InsertRecord)
        {
            string excode = "插入条数为零";
            InsertRecord = InsertRecord.Replace("number",DateTime.Now.ToString("yyyyMMddHHmmss"));
            InsertRecord = InsertRecord.Replace("time", DateTime.Now.ToString("yyyyMMdd HH:mm:00"));
            string sql = string.Format("INSERT INTO [dbo].[T_WarningReport] VALUES({0})", InsertRecord);
            try
            {
                int num = m_Database.Execute(sql);
                if (num > 0)
                    return "OK";
            }
            catch (Exception ex)
            {
                excode = ex.Message;

            }
            return excode;
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

    }
}


   