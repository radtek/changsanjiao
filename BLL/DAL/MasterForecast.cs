#region The ComForecast Copyright & Version History
/*
 * ============================================================== 
 * 
 * ComForecast, Version 1.0
 * 
 * Copyright (c) 2013-2014 上海地听信息科技有限公司.  版权所有.
 * 
 * 张伟锋
 * 
 * 修改：
 *       
 * 张伟锋              2010年11月25日
 * ====================================================================
 * 
 * 功能说明：用户实现环境监测中心的综合预报业务，包括预报单的录入，历史预报信息的调取等。
 *
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Readearth.Data;
using Readearth.Data.Entity;
using MMShareBLL.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ChinaAQI;
using Lucas.AQI2012;
using MASlib;
using Lucas;
using System.Net;
using Readearth.Common;
using WeiBo;
using Aspose.Cells;
using System.IO;
using System.Text.RegularExpressions;
using MMShareBLL.DAL;
using Aspose.Words;
using Aspose.Words.Tables;
using System.Web;
using System.Drawing;
using System.Security.Cryptography;
using Svg;
using Svg.Transforms;

namespace MMShareBLL.DAL
{
    public class MasterForecast
    {
        //用于记录系统错误日志
        protected static readonly log4net.ILog m_Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Database m_database;
        private int m_BackDays;

        public MasterForecast()
        {
            m_database = new Database();
            m_BackDays = int.Parse(ConfigurationManager.AppSettings["BackDays"]);
        }

        public  string Save(string forecaster, string aqi, string poll, string foreTime, string lst)
        {
            string sql = "INSERT INTO T_ForemanDaily (forecaster,AQI,pollution,foreTime,LST) VALUES ('" + forecaster + "','" + aqi + "','" + poll + "','" + foreTime + "','" + lst + "')";
            string sql_fore = "SELECT * FROM T_ForemanDaily WHERE LST='" + lst + "'";
            string update_fore = "UPDATE T_ForemanDaily SET forecaster='" + forecaster + "',AQI='" + aqi + "',pollution='" + poll + "', foreTime='" + foreTime + "' WHERE lst='" + lst + "'";
            //把状态插入到T_StateFO
            string insert = "INSERT INTO T_State (ModuleType,ReTime,DeadLine,State,Type) VALUES ('foremanDaily',GETDATE(),'','3','1')";
            string query = "select * from T_State WHERE ReTime='" + lst + "'and ModuleType='foremanDaily' and State='3'";
            string update = "update T_State set DeadLine=GETDATE() where convert(varchar(100),ReTime,23)='2017-07-06' and ModuleType='foremanDaily' and State='3'";
            try
            {
                //foremanDaily表的操作
                DataTable dTable = m_database.GetDataTable(sql_fore);
                if (dTable != null && dTable.Rows.Count > 0)
                {
                    m_database.Execute(update_fore);
                }
                else
                {
                    m_database.Execute(sql);
                }//先判断是否已经保存过数据   T_StateFO表的操作
                DataTable dt = m_database.GetDataTable(query);
                if (dt.Rows.Count > 0 && dt != null)
                {
                    //保存过数据，状态表里面有状态，需要修改
                    m_database.Execute(update);
                }
                else
                {
                    //未保存过，需要把状态插入到表中
                    m_database.Execute(insert);
                }
                return "ok";
            }
            catch (Exception)
            {
                return "";
            }
        }

        public  string Query(string pubTime)
        {
            string sql_query = "SELECT forecaster,AQI,pollution,foreTime FROM T_ForemanDaily WHERE LST='" + pubTime + "' ORDER BY LST asc";
            m_database = new Database("DBCONFIG");
            DataTable dt = m_database.GetDataTable(sql_query);
            return DataTableToJson("data", dt);
        }

        public  string GetChart(string pubTime)
        {
            DateTime dtime = new DateTime(1970, 1, 1);
            string startTime = (DateTime.Parse(pubTime).AddDays(-14)).ToString();
            TimeSpan ts = DateTime.Parse(pubTime).Subtract(DateTime.Parse(startTime));
            int day = ts.Days;
            string sql = "SELECT DATEDIFF(S,'1970-01-01 00:00:00',CONVERT(CHAR(10),ForeTime, 120)) AS [END],AQI,Pollution,forecaster from T_ForemanDaily where LST between '" + startTime + "' and '" + pubTime + "'";
            DataTable dTable = m_database.GetDataTable(sql);
            if (dTable.Rows.Count <= day)
            {    //缺少数据，需要补
                for (int i = 0; i <= day; i++)
                {
                    DateTime dTime = DateTime.Parse(startTime).AddDays(i);
                    string ss = ((dTime.Subtract(dtime).TotalMilliseconds) / 1000).ToString();
                    //if (ss == "1499126400")
                    //{
                    string condition = "END='" + ss + "'";

                    DataRow[] dr = dTable.Select(condition);
                    //flag = true;


                    if (dr.Length <= 0)
                    {
                        DataRow newrow = dTable.NewRow();
                        newrow["END"] = ss;
                        newrow["aqi"] = "null";
                        newrow["pollution"] = "--";
                        newrow["forecaster"] = "--";
                        dTable.Rows.Add(newrow);
                    }
                    // }
                }
            }
            DataView dv = new DataView(dTable);
            dv.Sort = "END asc";     //排序  要不然图标X轴时间不按顺序
            dTable = dv.ToTable();
            StringBuilder json = new StringBuilder();
            json.Append("[");
            string time = "";
            for (int i = 0; i < dTable.Rows.Count; i++)
            {
                time += dTable.Rows[i][0].ToString() + "|";
                json.Append("{name:\"预报员：" + dTable.Rows[i][3] + "<br/>首要污染物：" + dTable.Rows[i][2] + "\",y:" + dTable.Rows[i][1] + ",color:\'" + GetColor(dTable.Rows[i][1].ToString()) + "\'},");
            }
            json.Append("]");
            json.Append("]");
            time = time.TrimEnd('*');
            string Json = json.ToString();
            Json = Json.Remove(Json.Length - 2, 1);
            string ret = Json + "*" + time;
            return ret;
        }

        public string GetChartII(string pubTime)
        {
            DateTime dtime = new DateTime(1970, 1, 1);
            string startTime = (DateTime.Parse(pubTime).AddDays(-14)).ToString();
            TimeSpan ts = DateTime.Parse(pubTime).Subtract(DateTime.Parse(startTime));
            int day = ts.Days;
            //string sql = "  select distinct  DATEDIFF(S,'1970-01-01 00:00:00',CONVERT(CHAR(10),t1.lst, 120)) AS [END]," +
            //             " convert(varchar(20),t1.AQI) as 'AQI',convert(varchar(20),t1.itemID) as 'Pollution','atuo' as 'forecaster' from T_ChinaShiValue t1 " +
            //             "  inner join " +
            //             "  (select lst,max(AQI) as 'AQI' from T_ChinaShiValue   where ItemID<>'0' and  LST between '" + startTime + "' and '" + pubTime + "'" +
            //             "       group by Lst ) t2  on t1.LST=t2.lst and t1.AQI=t2.AQI" +
            //             "        where  t1.ITEMID<>'0'  ";

            string sql = "  select distinct  DATEDIFF(S,'1970-01-01 00:00:00',CONVERT(CHAR(10),t1.lst, 120)) AS [END]," +
                         " convert(varchar(20),t1.AQI) as 'AQI',convert(varchar(20),t1.Pollution) as 'Pollution','auto' as 'forecaster' from T_ChinaShiValue t1 " +
                          "where ItemID='0' and  LST between '" + startTime + "' and '" + pubTime + "' " ;
            
            DataTable dTable = m_database.GetDataTable(sql);
            if (dTable.Rows.Count <= day)
            {    //缺少数据，需要补
                for (int i = 0; i <= day; i++)
                {
                    DateTime dTime = DateTime.Parse(startTime).AddDays(i);
                    string ss = ((dTime.Subtract(dtime).TotalMilliseconds) / 1000).ToString();
                    //if (ss == "1499126400")
                    //{
                    string condition = "END='" + ss + "'";

                    DataRow[] dr = dTable.Select(condition);
                    //flag = true;


                    if (dr.Length <= 0)
                    {
                        DataRow newrow = dTable.NewRow();
                        newrow["END"] = ss;
                        newrow["aqi"] = "null";
                        newrow["pollution"] = "--";
                        newrow["forecaster"] = "--";
                        dTable.Rows.Add(newrow);
                    }
                    // }
                }
            }
            DataView dv = new DataView(dTable);
            dv.Sort = "END asc";     //排序  要不然图标X轴时间不按顺序
            dTable = dv.ToTable();
            StringBuilder json = new StringBuilder();
            json.Append("[");
            string time = "";
            for (int i = 0; i < dTable.Rows.Count; i++)
            {
                time += dTable.Rows[i][0].ToString() + "|";

                string itemID = dTable.Rows[i][2].ToString();
                string itemName="";
                switch (itemID) {
                    case "1": itemName = "PM2.5"; break;
                    case "2": itemName = "PM10"; break;
                    case "3": itemName = "NO2"; break;
                    case "4": itemName = "03"; break;
                    case "5": itemName = "03"; break;
                }

                json.Append("{name:\"实况首要污染物：" + itemName + "\",y:" + dTable.Rows[i][1] + ",color:\'" + GetColor(dTable.Rows[i][1].ToString()) + "\'},");
            }
            json.Append("]");
            json.Append("]");
            time = time.TrimEnd('*');
            string Json = json.ToString();
            Json = Json.Remove(Json.Length - 2, 1);
            string ret = Json + "*" + time;
            return ret;
        }

        public  string GetColor(string value)
        {
            if (value == "null") return "null";
            float val = float.Parse(value);
            string color = "";
            if (val <= 50) color = "#00ff00";
            else if (val <= 100) color = "#ffff00";
            else if (val <= 150) color = "#ff9900";
            else if (val <= 200) color = "#ff0000";
            else if (val <= 300) color = "#9900ff";
            else if (val > 300) color = "#980000";
            return color;
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
