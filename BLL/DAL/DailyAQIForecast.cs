using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using AQIQuery.aQuery;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Readearth.Data;
using AQIQuery.DAL;


namespace MMShareBLL.DAL
{
    /// <summary>
    /// AQI数据查询类   
    /// </summary>
    public class DailyAQIForecast
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        private Database m_DatabaseS;
        DataSetForcast.DailyAQIDataTable DailyAQIDT = new DataSetForcast.DailyAQIDataTable();
        List<int> aqiitem_list = new List<int>() { 106, 107, 102,88888, 103, 202, 108, 104, 105, 101, 201, 109, 110,100 };
        List<int> aqiitem_listRi = new List<int>() { 306, 307,303, 99999, 302, 77777, 308,304, 305, 301, 66666,309, 310, 300 };
        Hashtable hs = new Hashtable();

        public DailyAQIForecast()
        {
            m_DatabaseS = new Database("AQIWEB");
        }

        /// <summary>
        /// 查询全市数据的方法
        /// </summary>
        /// <param name="fromDate">开始日期</param>
        /// <param name="toDate">结束日期</param>
        /// <param name="period">时效</param>
        /// <param name="dataType">查询类型</param>
        /// <returns></returns>
        public string GetFilterDataTables(string fromDate, string toDate, 
                                          string period, string dataType)
        {
            string jsonString = "";
            try
            {
                #region 时间处理
                if (dataType == "ri")
                {
                    // 2014年12月01日:00:00
                    try { fromDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss"); }
                    catch { }
                    int count = GetCount(fromDate, ':');
                    if (count == 2)
                    {
                        fromDate = fromDate.Substring(0, 10) + " 00:00:00";
                        fromDate = fromDate.Replace("年", "-").
                                            Replace("月", "-").
                                            Replace("日", "");
                        fromDate = fromDate.Replace("  ", " ");
                    }
                    try { toDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"); }
                    catch { }
                    count = GetCount(toDate, ':');
                    if (count == 2)
                    {
                        toDate = toDate.Substring(0, 10) + " 23:59:59";
                        toDate = toDate.Replace("年", "-").
                                            Replace("月", "-").
                                            Replace("日", "");
                        toDate = toDate.Replace("  ", " ");

                        DateTime outResult=DateTime.Now;
                        if(!DateTime.TryParse(toDate, out outResult)){ //new code
                            toDate = toDate.Substring(0, 9) + " 23:59:59";
                        }
                    }
                }
                
                #region 
                if (fromDate.Length <= 15)
                    fromDate = fromDate.Replace("年", "-").
                                        Replace("月", "-").
                                        Replace("日", "").
                                        Replace("时", "");

                if (toDate.Length <= 15)
                    toDate = toDate.Replace("年", "-").
                                        Replace("月", "-").
                                        Replace("日", "").
                                        Replace("时", "");

                #endregion
                if (fromDate.Length < 15)
                    fromDate = fromDate + ":00:00";

                if (toDate.Length < 15)
                    toDate = toDate + ":00:00";
                //return (jsonString = (fromDate+"  "+toDate));//测试
                #endregion

                #region 参数处理
                DateTime lst1 = DateTime.Parse(DateTime.Parse(fromDate).ToString("yyyy-MM-dd HH:mm:ss")).AddHours(1);
                DateTime lst2 = DateTime.Parse(DateTime.Parse(toDate).ToString("yyyy-MM-dd HH:mm:ss")).AddHours(1);
                string aqiItems="";
                foreach (int i in aqiitem_list)
                    aqiItems += (i.ToString() + ",");  //处理查询的参数数据

                DataTable dt = null;
                aqiitem_listRi = new List<int>() { 306, 307, 303, 99999, 302, 77777, 308, 304, 305, 301, 66666, 309, 310, 300 };
                if (dataType == "ri") { 
                    aqiItems="";
                   foreach (int i in aqiitem_listRi)  
                    aqiItems += (i.ToString() + ",");  //处理查询的日期参数数据
                }

                if (!string.IsNullOrEmpty(aqiItems))
                    aqiItems = aqiItems.Substring(0, aqiItems.Length - 1);
                #endregion

                #region 查询数据 包含日和小时数据
                if (dataType == "ri") {    //处理日数据
                    dt = Data.GroupDailyAQI(lst1.AddHours(-1), lst2.AddHours(-1), "102", aqiItems);
                    //整理日数据
                    ProcessDT_DAY(dt, lst1.AddHours(-1), lst2.AddHours(-1), period, "", "-1","");
                }
                else
                {   //处理小时数据
                     dt = Data.GroupHourlyAQI(lst1, lst2, "102", aqiItems); 
                    //整理小时数据
                    aqiitem_list = new List<int>() { 106, 107, 102, 88888, 103, 202, 108, 104, 105, 101, 201, 109, 110, 100 };
                    ProcessDT(dt, lst1.AddHours(-1), lst2.AddHours(-1), period,"","-1","");
                }
                #endregion
                
                jsonString = DataTableToJson("data", DailyAQIDT); 
            }
            catch { }
            return jsonString;
        }


        /// <summary>
        /// 获取站点数据
        /// </summary>
        /// <param name="fromDate">开始日期</param>
        /// <param name="toDate">结束日期</param>
        /// <param name="period">时效</param>
        /// <param name="dataType">查询类型</param>
        /// <returns></returns>
        public string GetSiteDataTables(string fromDate, string toDate, 
                                         string period, string dataType, string SiteID)
        {
            string jsonString = "";
            try
            {
                #region 时间处理
                if (dataType == "ri")
                {
                    try { fromDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss"); }
                    catch { }
                    int count = GetCount(fromDate, ':');
                    if (count == 2)
                    {
                        fromDate = fromDate.Substring(0, 10) + " 00:00:00";
                        fromDate = fromDate.Replace("年", "-").
                                            Replace("月", "-").
                                            Replace("日", "");
                        fromDate = fromDate.Replace("  ", " ");
                    }
                    try { toDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"); }
                    catch { }
                    count = GetCount(toDate, ':');
                    if (count == 2)
                    {
                        toDate = toDate.Substring(0, 10) + " 23:59:59";
                        toDate = toDate.Replace("年", "-").
                                            Replace("月", "-").
                                            Replace("日", "");
                        toDate = toDate.Replace("  ", " ");

                        DateTime outResult = DateTime.Now;
                        if (!DateTime.TryParse(toDate, out outResult))
                        { //new code
                            toDate = toDate.Substring(0, 9) + " 23:59:59";
                        }
                    }
                }

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

                #region 参数处理
                DateTime lst1 = DateTime.Parse(DateTime.Parse(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                DateTime lst2 = DateTime.Parse(DateTime.Parse(toDate).ToString("yyyy-MM-dd HH:mm:ss"));
                DataTable dt = new DataTable();
                string sites = "";
                foreach (string site in SiteID.Split(','))
                    sites += site.Split(':')[0] + ",";

                if (sites != "")
                    sites = sites.Substring(0, sites.Length - 1);
                #endregion

                #region 站点数据查询  日  小时
                if (dataType == "ri"){  
                    dt = Data.SiteDailyAQI(lst1, lst2, sites, "304,305,308,301,306,307,302,303,309,310,300");
                    aqiitem_listRi = new List<int>() { 306, 307, 302, 99999, 303, 77777, 308, 304, 305, 301, 66666, 309, 310, 300 };
                    ProcessDT_DAY(dt, lst1, lst2, period,SiteID, "","");
                }
                else{
                    dt = Data.SiteHourlyAQI(lst1, lst2, sites, "106,107,103,102,202,108,104,105,101,201,109,110,100");
                    aqiitem_list = new List<int>() { 106, 107, 102, 88888, 103, 202, 108, 104, 105, 101, 201, 109, 110, 100 };
                    ProcessDT(dt, lst1, lst2, period, SiteID, "","");
                }
                #endregion

                jsonString = DataTableToJsonNew("SiteData", DailyAQIDT);
            }
            catch { }
            return jsonString;
         }

        /// <summary>
        /// 得到字符包含总数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private int GetCount(string str,char c) {
            int count=0;
            foreach (char cr in str){
                if (cr == c)
                    count++;
            }
            return count;
        }

        /// <summary>
        /// 获取分区数据
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="period"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public string GetGroupDataTables(string fromDate, string toDate, 
                                          string period, string dataType, string GroupID)
        {
            string jsonString = "";
            try
            {
                #region 时间处理
                if (dataType == "ri") { 
                    try { fromDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss"); }
                    catch { }
                    int count = GetCount(fromDate, ':');
                    if (count == 2)
                    {
                        fromDate = fromDate.Substring(0, 10)+" 00:00:00";
                        fromDate = fromDate.Replace("年", "-").
                                            Replace("月", "-").
                                            Replace("日", "");
                        fromDate = fromDate.Replace("  ", " ");
                    }
                    try { toDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"); }
                    catch { }
                    count = GetCount(toDate, ':');
                    if (count == 2)
                    {
                        toDate = toDate.Substring(0, 10) + " 23:59:59";
                        toDate = toDate.Replace("年", "-").
                                            Replace("月", "-").
                                            Replace("日", "");
                        toDate = toDate.Replace("  ", " ");

                        DateTime outResult = DateTime.Now;
                        if (!DateTime.TryParse(toDate, out outResult))
                        { //new code
                            toDate = toDate.Substring(0, 9) + " 23:59:59";
                        }
                    }
                }
                
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

                #region 参数处理
                DateTime lst1 = DateTime.Parse(DateTime.Parse(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                DateTime lst2 = DateTime.Parse(DateTime.Parse(toDate).ToString("yyyy-MM-dd HH:mm:ss"));
                DataTable dt = new DataTable();
                string sites = "";
                foreach (string site in GroupID.Split(','))
                    sites += site.Split(':')[0] + ",";

                if (sites != "")
                    sites = sites.Substring(0, sites.Length - 1);

                #endregion

                #region 区域数据查询 日 小时
                if (dataType == "ri"){
                    dt = Data.GroupDailyAQI(lst1, lst2, sites, "304,305,308,301,306,307,302,303,309,310,300");
                    aqiitem_listRi = new List<int>() { 306, 307, 302, 99999, 303, 77777, 308, 304, 305, 301, 66666, 309, 310, 300 };
                    ProcessDT_DAY(dt, lst1, lst2, period, GroupID, "","g");
                }
                else{
                    dt = Data.GroupHourlyAQI(lst1, lst2, sites, "106,107,103,102,202,108,104,105,101,201,109,110,100");
                    aqiitem_list = new List<int>() { 106, 107, 102, 103,88888, 202, 108, 104, 105, 101, 201, 109, 110, 100 };
                    ProcessDT(dt, lst1, lst2, period, GroupID, "","g");
                }
                #endregion 

                jsonString = DataTableToJsonNew("GroupData", DailyAQIDT);
            }
            catch(Exception ex) {

                jsonString = ex.Message;
            }
            return jsonString;
        }

        /// <summary>
        /// 处理小时处理的函数
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="lst1"></param>
        /// <param name="lst2"></param>
        /// <param name="period"></param>
        /// <param name="SiteID"></param>
        /// <param name="flag"></param>
        /// <param name="type"></param>
        private void ProcessDT(DataTable dt, DateTime lst1, DateTime lst2,
                                string period, string SiteID,string flag,string type)
        {
            DailyAQIDT.Clear();
            foreach (string SiteStr in SiteID.Split(','))
            {
                for (DateTime i = lst1; i <= lst2; i = i.AddHours(1))
                {
                    DataRow dailyRow = DailyAQIDT.NewRow();
                    dailyRow["LST"] = i.ToString("yyyy-MM-dd HH:mm:ss");
                    foreach (int j in aqiitem_list){
                        string filter = "";
                        #region 查询条件
                        if (flag == "")
                        {
                            if(SiteStr.IndexOf(':')>=0)
                                filter = "LST='" + i.ToString("yyyy-MM-dd HH:mm:ss") + "' and AQIItemID='" + j + "' and SiteID='" + SiteStr.Split(':')[0] + "'";
                            else
                                filter = "LST='" + i.ToString("yyyy-MM-dd HH:mm:ss") + "' and AQIItemID='" + j + "' and SiteID='" + SiteStr + "'";
                        }
                        else
                        {
                            filter = "LST='" + i.ToString("yyyy-MM-dd HH:mm:ss") + "' and AQIItemID='" + j + "' ";
                        }
                        #endregion 

                        #region DataBindg
                        if (dt != null)
                        {
                            DataRow[] rows = dt.Select(type == "g"?filter.Replace("SiteID", "GroupID"):filter);
                            if (rows != null && rows.Length > 0)
                            {
                                try
                                {
                                    double value = 0d;
                                    if (period == "nd")
                                        value = double.Parse(rows[0]["Value"].ToString());
                                    else
                                        value = double.Parse(rows[0]["AQI"].ToString());

                                    if (value > 0)
                                    {
                                        #region 特殊处理
                                        if (j != 108 && j != 308)
                                        {
                                            if (period == "nd")
                                            {
                                                dailyRow[GetValue(j.ToString())] = (value * 1000).ToString("0.0");
                                            }
                                            else { dailyRow[GetValue(j.ToString())] = Math.Round(value, 1).ToString("0.0"); }
                                        }
                                        else
                                            dailyRow[GetValue(j.ToString())] = Math.Round(value, 3);
                                        #endregion
                                    }
                                }
                                catch { }
                            }
                        }
                        #endregion
                    }

                    if (SiteStr.IndexOf(':') >= 0){
                        dailyRow["SiteName" ] = SiteStr.Split(':')[1];
                        dailyRow["SiteID"] = SiteStr.Split(':')[0];
                    }
                    else {
                        dailyRow["SiteID"] = SiteStr;
                    }
                    DailyAQIDT.Rows.Add(dailyRow);
                }
            }
        }

        /// <summary>
        /// 处理日的函数
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="lst1"></param>
        /// <param name="lst2"></param>
        /// <param name="period"></param>
        /// <param name="SiteID"></param>
        /// <param name="flag"></param>
        /// <param name="type"></param>
        private void ProcessDT_DAY(DataTable dt, DateTime lst1, DateTime lst2, 
                                    string period, string SiteID, string flag,string type)
        {
            DailyAQIDT.Clear();
            foreach (string SiteStr in SiteID.Split(',')){
                for (DateTime i = lst1; i <= lst2; i = i.AddDays(1)){
                    DataRow dailyRow = DailyAQIDT.NewRow();
                    dailyRow["LST"] = i.ToString("yyyy-MM-dd");
                    foreach (int j in aqiitem_listRi)
                    {
                        #region  处理条件查询
                        string filter = "";
                        if (flag == "")
                        {
                            if (SiteStr.IndexOf(':') >= 0)
                                filter = "LST='" + i.ToString("yyyy-MM-dd 00:00:00") + "' and AQIItemID='" + j + "' and SiteID='" + SiteStr.Split(':')[0] + "'";
                            else
                                filter = "LST='" + i.ToString("yyyy-MM-dd 00:00:00") + "' and AQIItemID='" + j + "' and SiteID='" + SiteStr+ "'";
                        }
                        else
                            filter = "LST='" + i.ToString("yyyy-MM-dd 00:00:00") + "' and AQIItemID='" + j + "' ";

                        #endregion

                        #region  DataBind
                        if (dt != null){
                            DataRow[] rows = dt.Select(type == "g" ? filter.Replace("SiteID", "GroupID") : filter);
                            if (rows != null && rows.Length > 0){
                                try {
                                    double value = 0d;
                                    if (period == "nd")
                                        value = double.Parse(rows[0]["Value"].ToString());
                                    else
                                        value = double.Parse(rows[0]["AQI"].ToString());

                                    if (value > 0)
                                    {
                                        #region
                                        if (j != 108 && j != 308)
                                        {
                                            if (period == "nd")
                                            {
                                                dailyRow[GetValue(j.ToString())] = Math.Round(value * 1000, 1);
                                            }
                                            else { dailyRow[GetValue(j.ToString())] = Math.Round(value, 1); }
                                        }
                                        else
                                            dailyRow[GetValue(j.ToString())] = Math.Round(value, 3);
                                        #endregion
                                    }
                                }
                                catch { }
                            }
                        }
                        #endregion
                    }

                    if (SiteStr.IndexOf(':') >= 0){
                        dailyRow["SiteName"] = SiteStr.Split(':')[1];
                        dailyRow["SiteID"] = SiteStr.Split(':')[0];
                    }
                    else{
                        dailyRow["SiteID"] = SiteStr;
                    }
                    DailyAQIDT.Rows.Add(dailyRow);
                }
            }
        }


        /// <summary>
        /// 参数表映射 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetValue(string key) {
            if (hs.Count <= 0) {
                hs["106"] = "SO2";
                hs["107"] = "NO2";
                hs["102"] = "PM10z";
                hs["88888"] = "PM10x";
                hs["103"] = "PM10";
                hs["202"] = "PM10y";
                hs["108"] = "CO";
                hs["104"] = "O31";
                hs["105"] = "O38";
                hs["101"] = "PM2.5";
                hs["201"] = "PM2.5y";
                hs["109"] = "NO";
                hs["110"] = "NOx";
                hs["100"] = "AQI";
                //106 ,107, 102, 103, 202, 108 ,104,105,101, 201,109,110
                hs["306"] = "SO2";
                hs["307"] = "NO2";
                hs["303"] = "PM10x";
                hs["99999"] = "PM10z";
                hs["302"] = "PM10";
                hs["77777"] = "PM10y";
                hs["308"] = "CO";
                hs["304"] = "O31";
                hs["305"] = "O38";
                hs["301"] = "PM2.5";
                hs["66666"] = "PM2.5y";
                hs["309"] = "NO";
                hs["310"] = "NOx";
                hs["300"] = "AQI";

                hs["120"] = "AQI";
            }
            return hs[key].ToString();
          
        }

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
        /// 查询站点组数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetSiteGroupData()
        {
            DataTable table = null;
            try
            {
                table = m_DatabaseS.GetDataTable("select * from Site t1 left join SiteGroupSites t2 "+
                                                 "  on t1.SiteID=t2.SiteID left join SiteGroup t3"+
                                                 "  on t2.Groupid=t3.GroupID " +
                                                 " where t3.GradeID=2 order by t2.OrderID");
            }
            catch (Exception exception)
            {
                Console.WriteLine("SiteGroupData: " + exception.ToString());
            }
            return table;
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
    }
}
