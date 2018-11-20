using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Readearth.Data;
using AQIQuery.DAL;
using AQIQuery.aQuery;


namespace MMShareBLL.DAL
{
    /// <summary>
    /// 日报数据查询的类
    /// </summary>
    public class DailyDataForecast
    {
        private Database m_DatabaseS;
        private Database m_Database_dmc;
        DataSetForcast.DailyDataTable DailyDT = new DataSetForcast.DailyDataTable();
        ParaDatas pd = new ParaDatas();

        public DailyDataForecast()
        {
            m_DatabaseS = new Database("AQIWEB");
            m_Database_dmc = new Database("conStr_SEMC_DMC");
        }

        /// <summary>
        /// 全市数据
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="Paras"></param>
        /// <returns></returns>
        public string GetFilterDataTables(string fromDate, string toDate, string Paras)
        {
            string jsonString = "";
            try
            {
                #region 时间处理

                if (fromDate.Length <= 17)
                    fromDate = fromDate.Replace("年", "-").
                                        Replace("月", "-").
                                        Replace("日", "").
                                        Replace("时", ":00:00");

                if (toDate.Length <= 17)
                    toDate = toDate.Replace("年", "-").
                                       Replace("月", "-").
                                       Replace("日", "").
                                       Replace("时", ":59:59");

                #endregion

                DateTime lst1 = DateTime.Parse(DateTime.Parse(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                DateTime lst2 = DateTime.Parse(DateTime.Parse(toDate).ToString("yyyy-MM-dd HH:mm:ss"));
                string parastr = "";
                DataTable dt;
   
                foreach (string par in Paras.Split('#'))
                    parastr += par.Split(':')[0] + ",";

                if (parastr != "")
                    parastr = parastr.Substring(0, parastr.Length - 1);

                dt = pd.CityHourlyData(lst1, lst2, parastr);
                ProcessDT(dt, lst1, lst2, Paras,"88888:全市数据");
                jsonString = DataTableToJsonNew("CityData", DailyDT);
            }
            catch { }
            return jsonString;
        }

        /// <summary>
        /// 获取站点数据
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="Paras">站点参数</param>
        /// <returns></returns>
        public string GetSiteDataTables(string fromDate, string toDate ,string Paras, string SiteID)
        {
            string jsonString = "";
            try
            {
                #region 时间处理
              
                if (fromDate.Length <= 17)
                    fromDate = fromDate.Replace("年","-").
                                        Replace("月","-").
                                        Replace("日","").
                                        Replace("时",":00:00");

                if (toDate.Length <= 17)
                    toDate = toDate.Replace("年", "-").
                                       Replace("月", "-").
                                       Replace("日", "").
                                       Replace("时", ":59:59");

                #endregion

                DateTime lst1 = DateTime.Parse(DateTime.Parse(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                DateTime lst2 = DateTime.Parse(DateTime.Parse(toDate).ToString("yyyy-MM-dd HH:mm:ss"));
                string sites = "";
                string parastr = "";
                DataTable dt;
                foreach (string site in SiteID.Split('#'))
                    sites += site.Split(':')[0] + ",";

                if (sites != "")
                    sites = sites.Substring(0, sites.Length - 1);

                foreach (string par in Paras.Split('#'))
                    parastr += par.Split(':')[0] + ",";

                if (parastr != "")
                    parastr = parastr.Substring(0, parastr.Length - 1);

                dt = pd.SiteHourlyData(lst1, lst2, sites, parastr);
                ProcessDT(dt, lst1, lst2, Paras, SiteID);
                jsonString = DataTableToJsonNew("SiteData", DailyDT);
            }
            catch { }
            return jsonString;
         }

        private int GetCount(string str,char c) {
            int count=0;
            foreach (char cr in str){
                if (cr == c)
                    count++;
            }
            return count;
        }

        /// <summary>
        /// 查询站点参数数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="lst1"></param>
        /// <param name="lst2"></param>
        /// <param name="paras"></param>
        /// <param name="SiteID"></param>
        /// <param name="flag"></param>
        /// <param name="type"></param>
        private void ProcessDT(DataTable dt, DateTime lst1, DateTime lst2,  string paras, string SiteID)
        {
            DailyDT.Clear();
            DailyDT.Columns.Clear();
            DailyDT.Columns.Add("LST");
            foreach (string SiteStr in SiteID.Split('#'))
            {
                for (DateTime i = lst1; i <= lst2; i = i.AddHours(1))
                {
                    DataRow dailyRow = DailyDT.NewRow();
                    dailyRow["LST"] = i.ToString("yyyy-MM-dd HH:mm:ss");
                    #region 参数绑定值
                    foreach (string j in paras.Split('#'))
                    {
                        try { DailyDT.Columns.Add(j.Split(':')[1]); }catch { }
                        string filter = "";
                        if (SiteStr.IndexOf("全市数据")>=0)
                        {
                            filter = "LST='" + i.ToString("yyyy-MM-dd HH:mm:ss") +
                                   "' and ParameterID='" + j.Split(':')[0] + "'";
                        }
                        else
                        {
                            filter = "LST='" + i.ToString("yyyy-MM-dd HH:mm:ss") +
                                     "' and ParameterID='" + j.Split(':')[0] + "' and SiteID='" + SiteStr.Split(':')[0] + "'";
                        }
                        if (dt != null)
                        {
                            DataRow[] rows = dt.Select(filter);
                            if (rows != null && rows.Length > 0)
                            {
                                try
                                {
                                    double value = 0d;
                                    value = double.Parse(rows[0]["Value"].ToString());
                                    if (value > 0)
                                        dailyRow[j.Split(':')[1]] = Math.Round(value, 3);
                                } catch { }
                            }
                        }
                    }
                    #endregion
                    try{ DailyDT.Columns.Add("siteID");DailyDT.Columns.Add("siteName");}catch { }
                    dailyRow["SiteName" ] = SiteStr.Split(':')[1];
                    dailyRow["SiteID"] = SiteStr.Split(':')[0];
                    DailyDT.Rows.Add(dailyRow);
                }
            }
        }

        //private void ProcessDT_DAY(DataTable dt, DateTime lst1, DateTime lst2, string period, string SiteID, string flag,string type)
        //{
        //    DailyDT.Clear();
        //    foreach (string SiteStr in SiteID.Split(','))
        //    {
        //        for (DateTime i = lst1; i <= lst2; i = i.AddDays(1))
        //        {
        //            DataRow dailyRow = DailyAQIDT.NewRow();
        //            dailyRow["LST"] = i.ToString("yyyy-MM-dd");
        //            foreach (int j in aqiitem_listRi)
        //            {
        //                string filter = "";
        //                if (flag == "")
        //                {
        //                    if (SiteStr.IndexOf(':') >= 0)
        //                        filter = "LST='" + i.ToString("yyyy-MM-dd 00:00:00") + "' and AQIItemID='" + j + "' and SiteID='" + SiteStr.Split(':')[0] + "'";
        //                    else
        //                        filter = "LST='" + i.ToString("yyyy-MM-dd 00:00:00") + "' and AQIItemID='" + j + "' and SiteID='" + SiteStr+ "'";
        //                }
        //                else
        //                    filter = "LST='" + i.ToString("yyyy-MM-dd 00:00:00") + "' and AQIItemID='" + j + "' ";

        //                if (dt != null)
        //                {
        //                    DataRow[] rows = dt.Select(type == "g" ? filter.Replace("SiteID", "GroupID") : filter);
        //                    if (rows != null && rows.Length > 0)
        //                    {
        //                        try
        //                        {
        //                            double value = 0d;
        //                            if (period == "nd")
        //                                value = double.Parse(rows[0]["Value"].ToString());
        //                            else
        //                                value = double.Parse(rows[0]["AQI"].ToString());

        //                            if (value > 0)
        //                            {
        //                                #region
        //                                if (j != 108 && j != 308)
        //                                {
        //                                    if (period == "nd")
        //                                    {
        //                                        dailyRow[GetValue(j.ToString())] = Math.Round(value * 1000, 1);
        //                                    }
        //                                    else { dailyRow[GetValue(j.ToString())] = Math.Round(value, 1); }
        //                                }
        //                                else
        //                                    dailyRow[GetValue(j.ToString())] = Math.Round(value, 3);
        //                                #endregion
        //                            }
        //                        }
        //                        catch { }
        //                    }
        //                }
        //            }

        //            if (SiteStr.IndexOf(':') >= 0)
        //            {
        //                dailyRow["SiteName"] = SiteStr.Split(':')[1];
        //                dailyRow["SiteID"] = SiteStr.Split(':')[0];
        //            }
        //            else
        //            {
        //                dailyRow["SiteID"] = SiteStr;
        //            }
        //            DailyAQIDT.Rows.Add(dailyRow);
        //        }
        //    }
        //}

        /// <summary>
        /// DataTable to json
        /// </summary>
        /// <param name="jsonName">返回json的名称</param>
        /// <param name="dt">转换成json的表</param>
        /// <returns></returns>
        private   string DataTableToJson(string jsonName, System.Data.DataTable dt)
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
        /// 查询区域数据
        /// </summary>
        /// <returns></returns>
        public  DataTable GetAllDistrict()
        {
            DataTable table = null;
            try
            {
                table =  m_DatabaseS.GetDataTable("exec SiteGroup_GetAllDistrict");
            }
            catch (Exception exception)
            {
                Console.WriteLine("SiteGroup_GetAllDistrict: " + exception.ToString());
            }
            return table;
        }


        /// <summary>
        /// 查询站点组数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetSiteGroup()
        {
            DataTable table = null;
            try
            {
                table = m_DatabaseS.GetDataTable("select * from SiteGroup where GradeID=2 and GroupName!='国控点' and  GroupName!='全市平均' and GroupName!='对照点'    order by OrderID");
            }
            catch (Exception exception)
            {
                Console.WriteLine("SiteGroup: " + exception.ToString());
            }
            return table;
        }

        /// <summary>
        /// 查询参数组数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetParsGroup() { 
          // PRO_FactorGroup_SELECT_InfoAll
            DataTable table = null;
            try
            {
                table = m_Database_dmc.GetDataTable("exec PRO_FactorGroup_SELECT_InfoAll");
            }
            catch (Exception exception)
            {
                Console.WriteLine("SiteGroup: " + exception.ToString());
            }
            return table;
        }


        /// <summary>
        /// 查询参数组数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetParsGroupData()
        {
            DataTable table = null;
            try
            {
                table = m_Database_dmc.GetDataTable("select * from D_FactorGroup"); //这里没有排序字段
            }
            catch (Exception exception)
            {
                Console.WriteLine("ParsGroupData: " + exception.ToString());
            }
            return table;
        }

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
    }
}
