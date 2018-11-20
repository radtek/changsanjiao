using System;
using System.Collections.Generic;
using System.Text;
using Readearth.Data;
using System.Data;

using AQIQuery.aQuery;
using System.Data.SqlClient;

namespace MMShareBLL.DAL
{
   public  class Scheduing
    {
        private Database m_Database;
        DataSetForcast.WeekReportDTDataTable weekDT = new DataSetForcast.WeekReportDTDataTable();
        DataSetForcast.CollectionRateDataTable crDT = new DataSetForcast.CollectionRateDataTable();
        protected static readonly log4net.ILog m_Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Scheduing()
        {
            m_Database = new Database();
        }

       /// <summary>
       /// 得到工作组
       /// </summary>
       /// <returns></returns>
        public string GetWorkGroup() {
            string strSQL = "select (ROW_NUMBER() OVER(ORDER BY LST desc )) as SID,Text,Type,pid,descript,memo,ID FROM D_WorkGroup order by LST ";
            string jsonStr = "";
            try
            {
                DataTable tbUsers = m_Database.GetDataTable(strSQL);
                jsonStr = DataTableToJson("data", tbUsers);
            }
            catch
            {
            }
            return jsonStr;
        }

        /// <summary>
        /// 得到工作组
        /// </summary>
        /// <returns></returns>
        public string GetWorkUser()
        {
            string strSQL = "select (ROW_NUMBER() OVER(ORDER BY UserName )) as SID,* from T_User ";
            string jsonStr = "";
            try
            {
                DataTable tbUsers = m_Database.GetDataTable(strSQL);
                jsonStr = DataTableToJson("data", tbUsers);
            }
            catch
            {
            }
            return jsonStr;
        }

        public string GetWorkGroupUser(string text ,string type)
        {
            string strSQL = "select (ROW_NUMBER() OVER(ORDER BY t1.OrderIndex )) as SID ,t2.Alias,   CASE t1.Enabel   "+
                           " WHEN 1 THEN '启用'  "+
                           " ELSE '禁用'  "+
                           " END AS Enabel   ,t3.Text,t1.* from T_WorkGroup t1 "+
                           " left join D_WorkGroup t3 on t1.DID=t3.ID left join "+
                           " T_User t2 on t1.UserName=t2.UserName   where t3.ID='{0}' and t1.Type='{1}' order by t1.OrderIndex";
            string jsonStr = "";
            try
            {
                if (type == "0")
                    type = "日常值班";
                else
                    type = "节假日值班";

                DataTable tbUsers = m_Database.GetDataTable(string.Format(strSQL,text,type));
                jsonStr = DataTableToJson("data", tbUsers);
            }
            catch
            {
            }
            return jsonStr;
        }
        public string getEndTime(string text, string type, string fromDate)
        {
            if (type == "0")
                type = "日常值班";
            else
                type = "节假日值班";
            string endTime = "";
            string strSQL = string.Format("select '1' as SID , Users,Users,workTime from T_Scheduling  join D_WorkGroup on T_Scheduling.DText=D_WorkGroup.Text where D_WorkGroup.ID='{0}' and T_Scheduling.Type='{1}'", text, type);
            DataTable userGroup = m_Database.GetDataTable(strSQL);
            DataRow[] dataRow = userGroup.Select("workTime='" + DateTime.Parse(fromDate).ToString("yyyy-MM-dd") + "'");
            if (dataRow.Length > 0)
            {
                DataRow[] data = userGroup.Select("Users='" + dataRow[0][1].ToString() + "'");
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i][3].ToString() == DateTime.Parse(fromDate).ToString("yyyy-MM-dd"))
                    {
                        if (i != data.Length-1)
                        {
                            if (DateTime.Parse(data[i + 1][3].ToString()).ToString("yyyy-MM-dd") == DateTime.Parse(data[i][3].ToString()).AddDays(1).ToString("yyyy-MM-dd"))
                            {
                                for (int j = i + 1; j < data.Length; j++)
                                {
                                    if (DateTime.Parse(data[j + 1][3].ToString()).ToString("yyyy-MM-dd") == DateTime.Parse(data[j][3].ToString()).AddDays(1).ToString("yyyy-MM-dd"))
                                        continue;
                                    else
                                    {
                                        endTime = data[j][3].ToString() + "," + data[j][1].ToString();
                                        break;
                                    }

                                }
                                break;
                                   
                            }
                            else
                            {
                                endTime = data[i][3].ToString() + "," + data[i][1].ToString();
                                break;
                            }

                        }
                        else
                        {
                            endTime = data[i][3].ToString() + "," + data[i][1].ToString();
                            break;
                        }
                    }
                    else
                        continue;
                }
            }
            return endTime;

        }
        public string GetWorkGroupUserII(string text, string type)
        {
            string strSQL = "select (ROW_NUMBER() OVER(ORDER BY t1.OrderIndex )) as SID ,t2.Alias, t2.Alias from T_WorkGroup t1 " +
                                 " left join D_WorkGroup t3 on t1.DID=t3.ID left join " +
                                 " T_User t2 on t1.UserName=t2.UserName   where t3.ID='{0}' and t1.Type='{1}' order by t1.OrderIndex";
            string jsonStr = "";
            try
            {
                if (type == "0")
                    type = "日常值班";
                else
                    type = "节假日值班";

                DataTable tbUsers = m_Database.GetDataTable(string.Format(strSQL, text, type));
                jsonStr = DataTableToJson("data", tbUsers);
            }
            catch
            {
            }
            return jsonStr;
        }
        public string changeIndex(string text, string type, string siteID, string index)
        {
            string returnStr = "0";
            try
            {
                if (type == "0")
                    type = "日常值班";
                else
                    type = "节假日值班";
                int indexValue = int.Parse(index);
                int orderID = int.Parse(siteID);
                string strSQL = string.Format("Update T_WorkGroup set orderIndex='{0}' where DID='{1}' and Type='{2}' and orderIndex='{3}';", "999", text, type, (orderID - indexValue).ToString());
                m_Database.Execute(strSQL);
                strSQL = string.Format("Update T_WorkGroup set orderIndex='{0}' where DID='{1}' and Type='{2}' and orderIndex='{3}';", (orderID - indexValue).ToString(), text, type, siteID);
                m_Database.Execute(strSQL);
                strSQL =  string.Format("Update T_WorkGroup set orderIndex='{0}' where DID='{1}' and Type='{2}' and orderIndex='{3}';", siteID, text, type, "999");

                m_Database.Execute(strSQL);
                returnStr = "1";
            }
            catch(Exception ex)
            {
                
            }
            return returnStr;
        }
        /// <summary>
        /// 得到副组
        /// </summary>
        /// <returns></returns>
        public string GetWorkGroupFZ()
        {
            string strSQL = "select * FROM D_WorkGroup where Type='副组' order by LST desc";
            string jsonStr = "";
            try
            {
                DataTable tbUsers = m_Database.GetDataTable(strSQL);
                jsonStr = DataTableToJsonNew("data", tbUsers);
            }
            catch
            {
            }
            return jsonStr;
        }

        public string AddWorkGroup(string name, string pid, 
                                   string Type, string Descript ,string Memo)
        {
            string strSQL = "Insert into D_WorkGroup(Text,PID,Type,Descript,Memo)"+
                            "  values('{0}',{1},'{2}','{3}','{4}')";
            try
            {
                strSQL = string.Format(strSQL, name, pid, Type, Descript, Memo);
                int count = m_Database.Execute(strSQL);
                if (count > 0)
                    return "1";
            }
            catch { }
            return "0";
        }

        public string AddWorkUser(string DID, string userName,
                                   string Type)
        {
            string strSQL = "Insert into T_WorkGroup(DID,userName,Type,Enabel,orderIndex)" +
                            "  values('{0}','{1}','{2}',1,-1)";
            try
            {

                if (Type == "0")
                    Type = "日常值班";
                else
                    Type = "节假日值班";

                int count = 0;
                if (!string.IsNullOrEmpty(userName))
                {
                    foreach (string user in userName.Split(','))
                    {
                        if(!string.IsNullOrEmpty(user)){
                           string strSQLs = string.Format(strSQL, DID, user, Type);
                           try
                           {
                               int c = m_Database.Execute(strSQLs);
                              count += c;
                           }
                           catch { }
                        }
                    }
                    if (count > 0)
                        return "1";
                }
            }
            catch { }
            return "0";
        }


        public string UpdateWorkGroup(string name, string pid,
                                string Type, string Descript, string Memo,string IDS)
        {
            string strSQL = "Update D_WorkGroup set  Text='{0}',PID={1},Type='{2}',Descript='{3}',Memo='{4}' where ID={5}";
            try
            {
                strSQL = string.Format(strSQL, name, pid, Type, Descript, Memo, IDS);
                int count = m_Database.Execute(strSQL);
                if (count > 0)
                    return "1";
            }
            catch(Exception ex) {
                return ex.Message;
            }
            return "0";
        }

        public string UpdateUser(string ID,string DID, string userName,
                               string Type, string Memo, string enabel,string orderIndex)
        {
            string strSQL = "Update T_WorkGroup set  DID='{0}',userName='{1}',Type='{2}',Memo='{3}',Enabel={4},OrderIndex='{6}' where ID={5}";
            try
            {
                strSQL = string.Format(strSQL, DID, userName, Type, Memo, enabel, ID, orderIndex);
                int count = m_Database.Execute(strSQL);
                if (count > 0)
                    return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "0";
        }

        public string DelWorkGroup( string IDS)
        {
            string strSQL = "delete from  D_WorkGroup  where ID={0}";
            try
            {
                strSQL = string.Format(strSQL,IDS);
                int count = m_Database.Execute(strSQL);
                if (count > 0)
                    return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "0";
        }


        public string DelWorkUser(string ID)
        {
            string strSQL = "delete from  T_WorkGroup  where ID='{0}'";
            try
            {
                strSQL = string.Format(strSQL, ID);
                int count = m_Database.Execute(strSQL);
                if (count > 0)
                    return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "0";
        }

        /// <summary>
        /// Data to json
        /// </summary>
        /// <param name="jsonName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string DataTableToJsonNew(string jsonName, System.Data.DataTable dt)
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

        /// <summary>
        /// 得到Scheduing
        /// </summary>
        /// <returns></returns>
        public string GetScheduing(string date)
        {
            string strSQL = "SELECT * FROM T_Scheduling  order by workTime";
            DataTable dt = new DataTable("T_Scheduling1");
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("DText", typeof(string));
            dt.Columns.Add("Users", typeof(string));
            dt.Columns.Add("beginTime", typeof(string));
            dt.Columns.Add("endTime", typeof(string));
            dt.Columns.Add("Type", typeof(string));            
            string jsonStr = "";
            string user = "";
            DateTime time=new DateTime() ;
            string filter = "";
            string startTime = "";
            string endTime = "";
            DataRow[] TimeRow;
            string type = "";
            try
            {
                DataTable tbUsers = m_Database.GetDataTable(strSQL);
                DataTable groupTable= tbUsers.DefaultView.ToTable(true, "DText");
                DataTable groupUserTable = tbUsers.DefaultView.ToTable(true, "Users");
                string group = "";
                foreach (DataRow groupRow in groupTable.Rows)
                {
                    group = groupRow[0].ToString();
                    foreach (DataRow groupUser in groupUserTable.Rows)
                    {
                        user = groupUser[0].ToString();
                        filter = string.Format("DText='{0}' and Users='{1}'", group, user);
                        TimeRow = tbUsers.Select(filter);
                        if (TimeRow.Length > 0)
                        {
                            for (int i = 0; i < TimeRow.Length; i++)
                            {
                                if (i == 0)
                                {
                                    time = DateTime.Parse(TimeRow[i][2].ToString());
                                    startTime = TimeRow[i][2].ToString();
                                    endTime = TimeRow[i][2].ToString();
                                    type = TimeRow[i][5].ToString();
                                    if (TimeRow.Length == 1)
                                    {
                                        DataRow newRow = dt.NewRow();
                                        newRow[0] = DBNull.Value;
                                        newRow[1] = group;
                                        newRow[2] = user;
                                        newRow[3] = startTime;
                                        newRow[4] = DateTime.Parse(endTime).AddDays(1).ToString("yyyy-MM-dd");
                                        newRow[5] = type;
                                        dt.Rows.Add(newRow);
                                    }
                                }
                                else
                                {
                                    if (time.AddDays(1).ToString("yyyy-MM-dd") == TimeRow[i][2].ToString())
                                    {
                                        endTime = TimeRow[i][2].ToString();
                                        time = DateTime.Parse(TimeRow[i][2].ToString());
                                        type = TimeRow[i][5].ToString();
                                        if (i == TimeRow.Length - 1)
                                        {
                                            DataRow newRow = dt.NewRow();
                                            newRow[0] = DBNull.Value;
                                            newRow[1] = group;
                                            newRow[2] = user;
                                            newRow[3] = startTime;
                                            newRow[4] = DateTime.Parse(endTime).AddDays(1).ToString("yyyy-MM-dd");
                                            newRow[5] = type;
                                            dt.Rows.Add(newRow);
                                        }
                                        continue;
                                    }
                                    else
                                    {
                                        DataRow newRow = dt.NewRow();
                                        newRow[0] = DBNull.Value;
                                        newRow[1] = group;
                                        newRow[2] = user;
                                        newRow[3] = startTime;
                                        newRow[4] =DateTime.Parse(endTime).AddDays(1).ToString("yyyy-MM-dd");
                                        newRow[5] = type;
                                        dt.Rows.Add(newRow);
                                        if (i == TimeRow.Length - 1 && TimeRow[i][2].ToString() != startTime)
                                        {
                                            DataRow newRow1 = dt.NewRow();
                                            newRow1[0] = DBNull.Value;
                                            newRow1[1] = group;
                                            newRow1[2] = user;
                                            newRow1[3] = TimeRow[i][2].ToString();
                                            newRow1[4] = DateTime.Parse(TimeRow[i][2].ToString()).AddDays(1).ToString("yyyy-MM-dd");
                                            newRow1[5] = type;
                                            dt.Rows.Add(newRow1);
                                        }
                                        startTime = TimeRow[i][2].ToString();
                                        time = DateTime.Parse(TimeRow[i][2].ToString());

                                    }
                                }

                            }
                        }
                    }
                  

                }
                jsonStr = DataTableToJson("data", dt);
            }
            catch
            {}
            return jsonStr;
        }
        public int returnWeek(string dt)
        {
            int week = 0;
            switch (dt)
            {
                case "Monday":
                    week = 1;
                    break;
                case "Tuesday":
                    week = 2;
                    break;
                case "Wednesday":
                    week = 3;
                    break;
                case "Thursday":
                    week = 4;
                    break;
                case "Friday":
                    week = 5;
                    break;
                case "Saturday":
                    week = 6;
                    break;
                case "Sunday":
                    week = 7;
                    break;
            }
            return week;
        }
        public string AddScheduing(string DText, string Users, string beginTime, string endTime, string Type, string group, string name)
        {
            DateTime beginDate = DateTime.Parse(beginTime);
            DateTime endDate = DateTime.Parse(endTime);
            DateTime nowDate = new DateTime();
            string[] nameArray = name.Split(',');
            string existsSQL;
            string deleteSQL="";
            string strSQL = "";
            int m = 0; int totalUser = nameArray.Length;
            strSQL = "";
            DateTime startTime = beginDate;
            SqlDataReader tempTable;
            for (int i = 0; i <= endDate.Day - beginDate.Day; i++)
            {
                nowDate = beginDate.AddDays(i);
    
                if (DText == "领班")
                {
                    existsSQL = @"SELECT workTime FROM T_Scheduling WHERE workTime='" + nowDate.ToString("yyyy-MM-dd") + "' and DText='" + DText + "' and Type='" + Type + "'";
                    tempTable = m_Database.GetDataReader(existsSQL);
                    if (tempTable.HasRows)
                    {
                        deleteSQL = deleteSQL + "delete FROM T_Scheduling WHERE workTime='" + nowDate.ToString("yyyy-MM-dd") + "' and DText='" + DText + "' and Type='" + Type + "';";
                    }
                    strSQL = strSQL + string.Format("Insert into T_Scheduling([DText],[Users],[workTime],[createUser],LST,Type) " +
                                    " values('{0}','{1}','{2}','{3}','{4}','{5}'); ", DText, nameArray[m], nowDate.ToString("yyyy-MM-dd"), Users, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Type);
                    if (nowDate.DayOfWeek.ToString() == "Sunday" )
                    {
                        m++;
                        if (m >= totalUser)
                            m = 0;
                    }
                    if (nowDate.Day == endDate.Day)
                    {
                        int week = returnWeek(nowDate.DayOfWeek.ToString());

                        for (int k = 0; k < (7 - week); k++)
                        {
                            strSQL = strSQL + string.Format("Insert into T_Scheduling([DText],[Users],[workTime],[createUser],LST,Type) " +
                                    " values('{0}','{1}','{2}','{3}','{4}','{5}'); ", DText, nameArray[m], nowDate.AddDays(k + 1).ToString("yyyy-MM-dd"), Users, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Type);
                        }
                    }
                }
                else if (DText == "主班")
                {
                    existsSQL = @"SELECT workTime FROM T_Scheduling WHERE workTime='" + nowDate.ToString("yyyy-MM-dd") + "' and DText='" + DText + "' and Type='" + Type + "'";
                    tempTable = m_Database.GetDataReader(existsSQL);
                    if (tempTable.HasRows)
                    {
                        deleteSQL =deleteSQL+ "delete FROM T_Scheduling WHERE workTime='" + nowDate.ToString("yyyy-MM-dd") + "' and DText='" + DText + "' and Type='" + Type + "';";
                    }

                    strSQL = strSQL + string.Format("Insert into T_Scheduling([DText],[Users],[workTime],[createUser],LST,Type) " +
                                    " values('{0}','{1}','{2}','{3}','{4}','{5}'); ", DText, nameArray[m], startTime.ToString("yyyy-MM-dd"), Users, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Type);
                    if (nowDate.DayOfWeek.ToString() == "Tuesday")
                    {
                        existsSQL = @"SELECT workTime FROM T_Scheduling WHERE workTime='" + nowDate.AddDays(-1).ToString("yyyy-MM-dd") + "' and DText='副班' and Type='" + Type + "'";
                        tempTable = m_Database.GetDataReader(existsSQL);
                        if (tempTable.HasRows)
                        {
                            deleteSQL = deleteSQL + "delete FROM T_Scheduling WHERE workTime='" + nowDate.AddDays(-1).ToString("yyyy-MM-dd") + "' and DText='副班' and Type='" + Type + "';";
                        }
                        strSQL = strSQL + string.Format("Insert into T_Scheduling([DText],[Users],[workTime],[createUser],LST,Type) " +
                                    " values('{0}','{1}','{2}','{3}','{4}','{5}'); ", "副班", nameArray[m], startTime.AddDays(-1).ToString("yyyy-MM-dd"), Users, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Type);
                    }
                    tempTable.Close();
                    m++;
                    startTime = nowDate.AddDays(1);
                    if (m >= totalUser)
                        m = 0;
                }
                else if (DText == "副班")
                {
                    existsSQL = @"SELECT workTime FROM T_Scheduling WHERE workTime='" + nowDate.ToString("yyyy-MM-dd") + "' and DText='副班' and Type='" + Type + "'";
                    tempTable = m_Database.GetDataReader(existsSQL);
                    if (tempTable.HasRows)
                    {
                        deleteSQL = deleteSQL + "delete FROM T_Scheduling WHERE workTime='" + nowDate.ToString("yyyy-MM-dd") + "' and DText='副班' and Type='" + Type + "';";
                    }
                    tempTable.Close();
                    strSQL = strSQL + string.Format("Insert into T_Scheduling([DText],[Users],[workTime],[createUser],LST,Type) " +
                                " values('{0}','{1}','{2}','{3}','{4}','{5}'); ", "副班", nameArray[m], nowDate.ToString("yyyy-MM-dd"), Users, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Type);
                }

            }

            try
            {
                
                if (deleteSQL != "")
                    m_Database.Execute(deleteSQL);
                int count = m_Database.Execute(strSQL);
                 if (count > 0)
                     return "1";
            }
            catch
            { }
            return "0";
        }

        public string Hello()
        {
            return "ok";
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

        private string DataTableToJsonNEW(string jsonName, System.Data.DataTable dt)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{\'" + jsonName + "\':[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append("" + dt.Columns[j].ToString() + ":" + "\'" + dt.Rows[i][j].ToString() + "\'");
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
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
