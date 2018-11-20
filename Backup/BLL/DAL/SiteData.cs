using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using ChinaAQI;
using Readearth.Data;

namespace MMShareBLL.DAL
{
    public class SiteData
    {
        private Database m_Database;
        private Database m_DatabaseS;
        public SiteData()
        {
            m_Database = new Database();
            m_DatabaseS = new Database("SEMCDMC");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="siteIDs"></param>
        /// <returns></returns>
        public string Query(string fromDate, string toDate, string siteIDs, string curSiteID)
        {
            DateTime dtFrom = DateTime.Parse(fromDate);
            DateTime dtTo = DateTime.Parse(toDate);
            int[] itemOrder = { 8, 9, 3, 4, 2, 6, 7, 1, 5 };
            string strSQL = string.Format("SELECT LST,ITEMID,SITEID,CONVERT(decimal(10, 1), VALUE * 1000) AS VALUE,AQI FROM SEMC_DMC.DBO.LT_RT_SiteData WHERE LST BETWEEN '{0}' AND '{1}' AND ITEMID <=9 AND SITEID IN({2})", dtFrom, dtTo, siteIDs);
            DataTable dtSiteData = m_Database.GetDataTable(strSQL);

            DataTable distinctLst = dtSiteData.DefaultView.ToTable(true, "LST");


            StringBuilder sb = new StringBuilder();
            StringBuilder tdValue = new StringBuilder();
            string className = string.Empty;
            sb.AppendLine("<table id='publicLogDataTable'  width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.AppendLine("<tr>");

            //创建抬头
            sb.AppendLine("<td class='tabletitle'style='border-left-style: solid; border-left-width: 1px; border-left-color: #C9C9C9' >日期</td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>2.5</sub></td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>2.5</sub>-24h</td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>10</sub></td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>10</sub>-24h</td>");
            sb.AppendLine("<td class='tabletitle'>NO<sub>2</sub></td>");
            sb.AppendLine("<td class='tabletitle'>O<sub>3</sub>-1h</td>");
            sb.AppendLine("<td class='tabletitle'>O<sub>3</sub>-8h</td>");
            sb.AppendLine("<td class='tabletitle'>SO<sub>2</sub></td>");
            sb.AppendLine("<td class='tabletitle'>CO</td>");
            sb.AppendLine("</tr>");
            int rowID = 0;
            AQIExtention aqiExt;
            string aqiColor;
            int rowIndex = 0;
            foreach (DataRow row in distinctLst.Rows)
            {
                rowIndex++;
                sb.AppendLine(string.Format("<tr  onmouseover='mouseOver(this)' onmouseout='mouseOut(this)' id='{0}'>", rowIndex));
                sb.AppendLine(string.Format("<td class='tablerow' style='border-left-style: solid; border-left-width: 1px; border-left-color: #C9C9C9' >{0}</td>", DateTime.Parse(row[0].ToString()).ToString("yyyy-MM-dd HH:mm")));
                rowID = rowID + 1;
                for (int i = 0; i < itemOrder.Length; i++)
                {
                    string filter = string.Format("LST = '{0}' AND ITEMID = {1}", row[0], itemOrder[i]);
                    DataRow[] rows = dtSiteData.Select(filter);
                    tdValue.Length = 0;
                    for (int j = 0; j < rows.Length; j++)
                    {
                        if (string.Format("{0:000}", rows[j]["SITEID"]) != curSiteID)
                            className = "class='hidden'";
                        else
                            className = "class='show'";
                        if (rows[j]["AQI"].ToString() != "")
                        {
                            int AQIValue = int.Parse(rows[j]["AQI"].ToString());
                            aqiExt = new AQIExtention(AQIValue);
                            aqiColor = string.Format("class='{0}'", aqiExt.Color);
                            if (int.Parse(itemOrder[i].ToString()) == 5)
                            {
                                tdValue.AppendFormat("<div id='H{5}{6}{0:000}' {1}>{3}/<span {2}>{4}</span></div>", rows[j]["SITEID"], className, aqiColor, Math.Round(double.Parse(rows[j]["VALUE"].ToString()) / 1000, 1), rows[j]["AQI"], rowID, i);
                            }
                            else if (int.Parse(itemOrder[i].ToString()) == 8 || int.Parse(itemOrder[i].ToString()) == 3)
                            {
                                tdValue.AppendFormat("<div id='H{4}{5}{0:000}' {1}>{2}</div>", rows[j]["SITEID"], className, rows[j]["VALUE"], rowID, i);
                            }
                            else
                            {
                                tdValue.AppendFormat("<div id='H{5}{6}{0:000}' {1}>{3}/<span {2}>{4}</span></div>", rows[j]["SITEID"], className, aqiColor, rows[j]["VALUE"], rows[j]["AQI"], rowID, i);
                            }
                        }
                        else
                        {
                           if (int.Parse(itemOrder[i].ToString()) == 8 || int.Parse(itemOrder[i].ToString()) == 3)
                            {
                                tdValue.AppendFormat("<div id='H{3}{4}{0:000}' {1}>{2}</div>", rows[j]["SITEID"], className, rows[j]["VALUE"].ToString(), rowID, i);
                            }
                            else
                            {
                                tdValue.AppendFormat("<div id='H{4}{5}{0:000}' {1}>{2}/{3}</div>", rows[j]["SITEID"], className, rows[j]["VALUE"], rows[j]["AQI"], rowID, i);
                            }

                        }
                    }
                    sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", tdValue.ToString()));


                }
                sb.AppendLine("</tr>");
            }


            sb.AppendLine("</table>");

            return sb.ToString();
        }
        //小时站点数据
        public string QueryPerHour(string fromDate, string siteIDs, string curSiteID)
        {
            //fromDate = "2013年08月18日";
            DateTime dtFrom = DateTime.Parse(fromDate);
            DateTime dtTo = DateTime.Parse(fromDate).AddHours(23);
            string strSQL = string.Format(" select LST,6 as ITEMID,SITEID,(30-POWER(value,1.0/2.0)) as VALUE,AQI from T_ForecastSite where DURATIONID =10 and MODULE='wrf' and ITEMID=2 and LST between '{0}' AND '{1}' and SITEID IN({2}) AND PERIOD=24 union SELECT LST,ITEMID,SITEID,VALUE,AQI FROM T_ForecastSite WHERE DURATIONID=10 AND MODULE='wrf' AND LST BETWEEN '{0}' AND '{1}' AND SITEID IN({2}) AND PERIOD=24 ORDER BY LST", dtFrom, dtTo, siteIDs);

            DataTable dtSiteData = m_Database.GetDataTable(strSQL);

            DataTable distinctLst = dtSiteData.DefaultView.ToTable(true, "LST");


            StringBuilder sb = new StringBuilder();
            StringBuilder tdValue = new StringBuilder();
            string className = string.Empty;
            sb.AppendLine("<table id='PerHourDataTable'  width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.AppendLine("<tr>");

            //创建抬头
            sb.AppendLine("<td class='tabletitle'style='border-left-style: solid; border-left-width: 1px; border-left-color: #C9C9C9' >日期</td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>2.5</sub></td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>10</sub></td>");
            sb.AppendLine("<td class='tabletitle'>NO<sub>2</sub></td>");
            sb.AppendLine("<td class='tabletitle'>O<sub>3</sub>-1h</td>");
            sb.AppendLine("<td class='tabletitle'>O<sub>3</sub>-8h</td>");
            sb.AppendLine("<td class='tabletitle'>VI</td>");
            sb.AppendLine("</tr>");
            int rowID = 0;
            AQIExtention aqiExt;
            string aqiColor;
            int rowIndex = 0;
            foreach (DataRow row in distinctLst.Rows)
            {
                rowIndex++;
                sb.AppendLine(string.Format("<tr  onmouseover='mouseOver(this)' onmouseout='mouseOut(this)' id='{0}'>", rowIndex));
                sb.AppendLine(string.Format("<td class='tablerow' style='border-left-style: solid; border-left-width: 1px; border-left-color: #C9C9C9' >{0}</td>", DateTime.Parse(row[0].ToString()).ToString("yyyy-MM-dd HH:mm")));
                rowID = rowID + 1;
                for (int i = 1; i <7; i++)
                {
                    string filter = string.Format("LST = '{0}' AND ITEMID = {1}", row[0], i);
                    DataRow[] rows = dtSiteData.Select(filter);
                    tdValue.Length = 0;
                    for (int j = 0; j < rows.Length; j++)
                    {
                        if (string.Format("{0:000}", rows[j]["SITEID"]) != curSiteID)
                            className = "class='hidden'";
                        else
                            className = "class='show'";
                        if (rows[j]["AQI"].ToString() != "")
                        {
                            int AQIValue = int.Parse(rows[j]["AQI"].ToString());
                            aqiExt = new AQIExtention(AQIValue);
                            aqiColor = string.Format("class='{0}'", aqiExt.Color);
                            
                            tdValue.AppendFormat("<div id='H{5}{6}{0:000}' {1}>{3}/<span {2}>{4}</span></div>", rows[j]["SITEID"], className, aqiColor, rows[j]["VALUE"], rows[j]["AQI"], rowID, i);
                        }
                        
                    }
                    sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", tdValue.ToString()));


                }
                sb.AppendLine("</tr>");
            }


            sb.AppendLine("</table>");

            return sb.ToString();
        }
        //站点日报表
        public string QueryDay(string fromDate)
        {
            DateTime dtFrom = DateTime.Parse(fromDate);
            string strSQL = string.Format("SELECT T_Site.Name,6 as ITEMID, (30-POWER(value,1.0/2.0)) as value,AQI  from T_ForecastSite inner join T_Site on T_ForecastSite.SiteID=T_Site.SiteID  where durationid =7 and module='wrf' and itemid=2 and lst='{0}' and period=24 UNION  SELECT T_Site.Name,T_ForecastSite.ITEMID ,T_ForecastSite.Value,T_ForecastSite.AQI  from T_ForecastSite  inner join T_Site on T_ForecastSite.SiteID=T_Site.SiteID where durationid =7 and module='wrf'  and lst='{0}' and period=24 ORDER BY T_Site.Name", dtFrom);

            DataTable dtSiteData = m_Database.GetDataTable(strSQL);
            


            StringBuilder sb = new StringBuilder();
            StringBuilder tdValue = new StringBuilder();
            string className = string.Empty;
            sb.AppendLine("<table id='PerHourDataTable'  width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.AppendLine("<tr>");

            //创建抬头
            sb.AppendLine("<td class='tabletitleD'style='border-left-style: solid; border-left-width: 1px; border-left-color: #C9C9C9' rowspan='2' >监测点名称</td>");
            sb.AppendLine("<td class='tabletitleD' colspan='2'>PM<sub>2.5</sub></td>");
            sb.AppendLine("<td class='tabletitleD' colspan='2'>PM<sub>10</sub></td>");
            sb.AppendLine("<td class='tabletitleD' colspan='2'>NO<sub>2</sub></td>");
            sb.AppendLine("<td class='tabletitleD' colspan='2'>O<sub>3</sub>-1h</td>");
            sb.AppendLine("<td class='tabletitleD' colspan='2'>O<sub>3</sub>-8h</td>");
            sb.AppendLine("<td class='tabletitleD' colspan='2'>VI</td>");
            sb.AppendLine("<td class='tabletitleD' rowspan='2'>AQI</td>");
            sb.AppendLine("<td class='tabletitleD' rowspan='2'>等级</td>");
            sb.AppendLine("<td class='tabletitleD' rowspan='2'>首要污染物</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='tabletitleD' >浓度</td>");
            sb.AppendLine("<td class='tabletitleD'>AQI</td>");
            sb.AppendLine("<td class='tabletitleD' >浓度</td>");
            sb.AppendLine("<td class='tabletitleD'>AQI</td>");
            sb.AppendLine("<td class='tabletitleD' >浓度</td>");
            sb.AppendLine("<td class='tabletitleD'>AQI</td>");
            sb.AppendLine("<td class='tabletitleD' >浓度</td>");
            sb.AppendLine("<td class='tabletitleD'>AQI</td>");
            sb.AppendLine("<td class='tabletitleD' >浓度</td>");
            sb.AppendLine("<td class='tabletitleD'>AQI</td>");
            sb.AppendLine("<td class='tabletitleD' >浓度</td>");
            sb.AppendLine("<td class='tabletitleD'>AQI</td>");
            sb.AppendLine("</tr>");
            int rowID = 0;
            AQIExtention aqiExt;
            string aqiColor,filter;
            int rowIndex = 0;
            int preItems;
            string siteName;
            strSQL = "SELECT DM,MC FROM D_ITEM";
            DataTable items = m_Database.GetDataTable(strSQL);
            rowID = rowID + 1;
            if (dtSiteData.Rows.Count > 0)
            {
                for (int i = 0; i < dtSiteData.Rows.Count / 6; i++)
                {
                    sb.AppendLine("<tr>");
                    siteName = dtSiteData.Rows[i * 6 + 1][0].ToString();
                    sb.AppendLine(string.Format("<td class='tablerowD'>{0}</td>", siteName));
                    int maxAQI = 0;
                    for (int j = 1; j < 7; j++)
                    {
                        filter = string.Format("Name= '{0}' AND ITEMID = {1}", siteName, j);
                        DataRow[] rows = dtSiteData.Select(filter);
                        tdValue.Length = 0;
                        if (rows.Length > 0)
                        {
                            int AQIValue = int.Parse(rows[0]["AQI"].ToString());
                            aqiExt = new AQIExtention(AQIValue);
                            aqiColor = string.Format("class='{0}'", aqiExt.Color);
                            sb.AppendLine(string.Format("<td class='tablerowD' >{0}</td>", rows[0]["VALUE"]));
                            tdValue.AppendFormat("<div><span {0}>{1}</span></div>", aqiColor, rows[0]["AQI"]);
                            sb.AppendLine(string.Format("<td class='tablerowD'>{0}</td>", tdValue.ToString()));
                        }

                    }
                    filter = string.Format("Name= '{0}'", siteName);
                    maxAQI = int.Parse(dtSiteData.Compute("max(AQI)", filter).ToString() == "" ? "0" : dtSiteData.Compute("max(AQI)", filter).ToString());
                    filter = string.Format("Name= '{0}' AND AQI={1}", siteName, maxAQI);
                    DataRow[] maxRow = dtSiteData.Select(filter);
                    string paraments = "";
                    for (int m = 0; m < maxRow.Length; m++)
                    {

                        preItems = int.Parse(maxRow[m][1].ToString());
                        if (preItems == 6)
                        {
                            paraments = paraments+"";
                        }
                        else
                        {
                            filter = string.Format("DM = {0}", preItems);
                            DataRow[] itemsDataRow = items.Select(filter);
                            paraments = paraments + "   " + itemsDataRow[0][1].ToString();
                        }

                    }
                    aqiExt = new AQIExtention(maxAQI);
                    aqiColor = string.Format("class='{0}'", aqiExt.Color);
                    string sbu = string.Format("<div><span {0}>{1}</span></div>", aqiColor, maxAQI);
                    sb.AppendLine(string.Format("<td class='tablerowD'>{0}</td>", sbu));


                    sb.AppendLine(string.Format("<td class='tablerowD'>{0}</td>", aqiExt.Quality));
                    sb.AppendLine(string.Format("<td class='tablerowD'>{0}</td>", paraments));

                    sb.AppendLine("</tr>");

                }
            }


            sb.AppendLine("</table>");

            return sb.ToString();
        }
        //全国沙尘数据
        public string QueryChinaSC(string fromDate)
        {
            DateTime dtFrom = DateTime.Parse(fromDate);
            string strSQL = string.Format("SELECT  T_Dust_Station.[address],pm10,tsp,vi,meteor_p,meteor_t,meteor_h,meteor_d,meteor_w FROM T_Dusty INNER JOIN T_Dust_Station on T_Dusty.mn=T_Dust_Station.mn WHERE cjTime='{0}'", dtFrom);

            
            DataTable dtSiteData = m_Database.GetDataTable(strSQL);

            int rowIndex = 0;
            string value;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table id='PerHourDataTable'  width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.AppendLine("<tr>");

            //创建抬头
            sb.AppendLine("<td class='tabletitle'style='border-left-style: solid; border-left-width: 1px; border-left-color: #C9C9C9' >地点名称</td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>10</sub>(微克/立方米)</td>");
            sb.AppendLine("<td class='tabletitle'>tsp(微克/立方米)</td>");
            sb.AppendLine("<td class='tabletitle'>能见度（公里）</td>");
            sb.AppendLine("<td class='tabletitle'>气压（hPa）</td>");
            sb.AppendLine("<td class='tabletitle'>气温（°C）</td>");
            sb.AppendLine("<td class='tabletitle'>湿度（%）</td>");
            sb.AppendLine("<td class='tabletitle'>风向（°）</td>");
            sb.AppendLine("<td class='tabletitle'>风速(米/秒)</td>");
            sb.AppendLine("</tr>");
            for(int i=0;i<dtSiteData.Rows.Count;i++)
            {
                rowIndex++;
                 sb.AppendLine(string.Format("<tr  onmouseover='mouseOver(this)' onmouseout='mouseOut(this)' id='{0}'>", rowIndex));
                for(int j=0;j<dtSiteData.Columns.Count;j++)
                {
                    value=dtSiteData.Rows[i][j].ToString();
                    if (j != 0)
                    {
                        if (value == "0")
                            value = "/";
                        else if (j == 1 || j == 2)
                        {
                            double tempValue = Math.Round(double.Parse(value) * 1000, 1);//转化成微克/立方米（PM10,、tsp）
                            if (tempValue > 1000)
                                value = "/";
                            else
                                value = tempValue.ToString();
                        }
                        else 
                            value = Math.Round(double.Parse(value), 1).ToString();
                    }
                    sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", value));

                }
                sb.AppendLine("</tr>");
            }
             sb.AppendLine("</table>");           

            return sb.ToString();
        }
        //华东小时数据
        public string QueryHuaD(string fromDate)
        {
            DateTime dtFrom = DateTime.Parse(fromDate);
            string strSQL = string.Format("SELECT  huadongarea.area,PositionName,AQI,PM2_5,PM10,CO,NO2,SO2,O31,O38 FROM huadongarea left JOIN China_RT_CNEMC_Data on huadongarea.area=China_RT_CNEMC_Data.area WHERE TimePoint='{0}'", dtFrom);

            DataTable dtSiteData = m_DatabaseS.GetDataTable(strSQL);

            int rowIndex = 0;
            string value;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table id='PerHourDataTable'  width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.AppendLine("<tr>");

            //创建抬头
            sb.AppendLine("<td class='tabletitle'style='border-left-style: solid; border-left-width: 1px; border-left-color: #C9C9C9' >地点名称</td>");
            sb.AppendLine("<td class='tabletitle'>位置点</td>");
            sb.AppendLine("<td class='tabletitle'>AQI</td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>2.5</sub></td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>10</sub></td>");
            sb.AppendLine("<td class='tabletitle'>CO</td>");
            sb.AppendLine("<td class='tabletitle'>NO2</td>");
            sb.AppendLine("<td class='tabletitle'>SO2</td>");
            sb.AppendLine("<td class='tabletitle'>O3_1h</td>");
            sb.AppendLine("<td class='tabletitle'>O3_8h</td>");
            sb.AppendLine("</tr>");
            for (int i = 0; i < dtSiteData.Rows.Count; i++)
            {
                rowIndex++;
                sb.AppendLine(string.Format("<tr  onmouseover='mouseOver(this)' onmouseout='mouseOut(this)' id='{0}'>", rowIndex));
                for (int j = 0; j < dtSiteData.Columns.Count; j++)
                {
                    value = dtSiteData.Rows[i][j].ToString();
                    if (value == "0" || value == "0.0")
                        value = "/";
                    sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", value));

                }
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</table>");

            return sb.ToString();
        }
        //长三角日报表
        public string QueryCSJDay(string fromDate)
        {
            DateTime dtFrom = DateTime.Parse(fromDate);
            DateTime dtTo = DateTime.Parse(fromDate).AddHours(23);
            string strSQL = string.Format("SELECT  China_RT_CNEMC_Data.area as Area,Avg(AQI) as AQI,Avg(pm2_5) as [PM2.5],Avg(pm10) as PM10,AVG(co) as CO,Avg(no2) as NO2,Avg(so2) as SO2,MAX(o31) as O31小时,MAX(o38) as O38小时 FROM huadongarea left  join China_RT_CNEMC_Data on huadongarea.area=China_RT_CNEMC_Data.area where TimePoint between '{0}' and '{1}' and ischang in(1) group by China_RT_CNEMC_Data.area,stateid", dtFrom, dtTo);

            DataTable dtSiteData = m_DatabaseS.GetDataTable(strSQL);

            int rowIndex = 0;
            string value;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table id='PerHourDataTable'  width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.AppendLine("<tr>");

            //创建抬头
            sb.AppendLine("<td class='tabletitle'style='border-left-style: solid; border-left-width: 1px; border-left-color: #C9C9C9' >地点名称</td>");
            sb.AppendLine("<td class='tabletitle'>AQI</td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>2.5</sub></td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>10</sub></td>");
            sb.AppendLine("<td class='tabletitle'>CO</td>");
            sb.AppendLine("<td class='tabletitle'>NO2</td>");
            sb.AppendLine("<td class='tabletitle'>SO2</td>");
            sb.AppendLine("<td class='tabletitle'>O3_1h</td>");
            sb.AppendLine("<td class='tabletitle'>O3_8h</td>");
            sb.AppendLine("</tr>");
            foreach (DataRow dr in dtSiteData.Rows)
            {
                rowIndex++;
                sb.AppendLine(string.Format("<tr  onmouseover='mouseOver(this)' onmouseout='mouseOut(this)' id='{0}'>", rowIndex));
                sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>",dr[0].ToString()));
                for (int i = 1; i < dtSiteData.Columns.Count; i++)
                {
                    value = dr[i].ToString();
                    if (value == "0" || value == "0.0"|| value =="")
                        value = "/";
                    else
                        value = Math.Round(Double.Parse(value), 1).ToString();
                    sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", value));

                }
                sb.AppendLine("</tr>");

            }
            sb.AppendLine("</table>");

            return sb.ToString();
        }
        //泛长三角日报表
        public string QueryFCSJDay(string fromDate)
        {
            DateTime dtFrom = DateTime.Parse(fromDate);
            DateTime dtTo = DateTime.Parse(fromDate).AddHours(23);
            string strSQL = string.Format("SELECT  China_RT_CNEMC_Data.area as Area,Avg(AQI) as AQI,Avg(pm2_5) as [PM2.5],Avg(pm10) as PM10,AVG(co) as CO,Avg(no2) as NO2,Avg(so2) as SO2,MAX(o31) as O31小时,MAX(o38) as O38小时 FROM huadongarea left  join China_RT_CNEMC_Data on huadongarea.area=China_RT_CNEMC_Data.area where TimePoint between '{0}' and '{1}' and ischang in(1,2) group by China_RT_CNEMC_Data.area,stateid", dtFrom, dtTo);

            DataTable dtSiteData = m_DatabaseS.GetDataTable(strSQL);

            int rowIndex = 0;
            string value;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table id='PerHourDataTable'  width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.AppendLine("<tr>");

            //创建抬头
            sb.AppendLine("<td class='tabletitle'style='border-left-style: solid; border-left-width: 1px; border-left-color: #C9C9C9' >地点名称</td>");
            sb.AppendLine("<td class='tabletitle'>AQI</td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>2.5</sub></td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>10</sub></td>");
            sb.AppendLine("<td class='tabletitle'>CO</td>");
            sb.AppendLine("<td class='tabletitle'>NO2</td>");
            sb.AppendLine("<td class='tabletitle'>SO2</td>");
            sb.AppendLine("<td class='tabletitle'>O3_1h</td>");
            sb.AppendLine("<td class='tabletitle'>O3_8h</td>");
            sb.AppendLine("</tr>");
            foreach (DataRow dr in dtSiteData.Rows)
            {
                rowIndex++;
                sb.AppendLine(string.Format("<tr  onmouseover='mouseOver(this)' onmouseout='mouseOut(this)' id='{0}'>", rowIndex));
                sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", dr[0].ToString()));
                for (int i = 1; i < dtSiteData.Columns.Count; i++)
                {
                    value = dr[i].ToString();
                    if (value == "0" || value == "0.0")
                        value = "/";
                    else
                        value = Math.Round(Double.Parse(value), 1).ToString();
                    sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", value));

                }
                sb.AppendLine("</tr>");

            }
            sb.AppendLine("</table>");

            return sb.ToString();
        }
        //长三角月报表
        public string QueryCSJMonth(string fromDate)
        {
            DateTime dtFrom = DateTime.Parse(fromDate);
            DateTime dtTo = DateTime.Parse(fromDate).AddMonths(1).AddDays(-1).AddHours(23);
            string strSQL = string.Format("SELECT  China_RT_CNEMC_Data.area as Area,Avg(AQI) as AQI,Avg(pm2_5) as [PM2.5],Avg(pm10) as PM10,AVG(co) as CO,Avg(no2) as NO2,Avg(so2) as SO2,MAX(o31) as O31小时,MAX(o38) as O38小时 FROM huadongarea left  join China_RT_CNEMC_Data on huadongarea.area=China_RT_CNEMC_Data.area where TimePoint between '{0}' and '{1}' and ischang in(1) group by China_RT_CNEMC_Data.area,stateid", dtFrom, dtTo);

            DataTable dtSiteData = m_DatabaseS.GetDataTable(strSQL);

            int rowIndex = 0;
            string value;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table id='PerHourDataTable'  width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.AppendLine("<tr>");

            //创建抬头
            sb.AppendLine("<td class='tabletitle'style='border-left-style: solid; border-left-width: 1px; border-left-color: #C9C9C9' >地点名称</td>");
            sb.AppendLine("<td class='tabletitle'>AQI</td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>2.5</sub></td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>10</sub></td>");
            sb.AppendLine("<td class='tabletitle'>CO</td>");
            sb.AppendLine("<td class='tabletitle'>NO2</td>");
            sb.AppendLine("<td class='tabletitle'>SO2</td>");
            sb.AppendLine("<td class='tabletitle'>O3_1h</td>");
            sb.AppendLine("<td class='tabletitle'>O3_8h</td>");
            sb.AppendLine("</tr>");
            foreach (DataRow dr in dtSiteData.Rows)
            {
                rowIndex++;
                sb.AppendLine(string.Format("<tr  onmouseover='mouseOver(this)' onmouseout='mouseOut(this)' id='{0}'>", rowIndex));
                sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", dr[0].ToString()));
                for (int i = 1; i < dtSiteData.Columns.Count; i++)
                {
                    value = dr[i].ToString();
                    if (value == "0" || value == "0.0")
                        value = "/";
                    else
                        value = Math.Round(Double.Parse(value), 1).ToString();
                    sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", value));

                }
                sb.AppendLine("</tr>");

            }
            sb.AppendLine("</table>");

            return sb.ToString();
        }
        //泛长三角月报表
        public string QueryFCSJMonth(string fromDate)
        {
            DateTime dtFrom = DateTime.Parse(fromDate);
            DateTime dtTo = DateTime.Parse(fromDate).AddMonths(1).AddDays(-1).AddHours(23);
            string strSQL = string.Format("SELECT  China_RT_CNEMC_Data.area as Area,Avg(AQI) as AQI,Avg(pm2_5) as [PM2.5],Avg(pm10) as PM10,AVG(co) as CO,Avg(no2) as NO2,Avg(so2) as SO2,MAX(o31) as O31小时,MAX(o38) as O38小时 FROM huadongarea left  join China_RT_CNEMC_Data on huadongarea.area=China_RT_CNEMC_Data.area where TimePoint between '{0}' and '{1}' and ischang in(1,2) group by China_RT_CNEMC_Data.area,stateid", dtFrom, dtTo);
            DataTable dtSiteData = m_DatabaseS.GetDataTable(strSQL);

            int rowIndex = 0;
            string value;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table id='PerHourDataTable'  width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.AppendLine("<tr>");

            //创建抬头
            sb.AppendLine("<td class='tabletitle'style='border-left-style: solid; border-left-width: 1px; border-left-color: #C9C9C9' >地点名称</td>");
            sb.AppendLine("<td class='tabletitle'>AQI</td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>2.5</sub></td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>10</sub></td>");
            sb.AppendLine("<td class='tabletitle'>CO</td>");
            sb.AppendLine("<td class='tabletitle'>NO2</td>");
            sb.AppendLine("<td class='tabletitle'>SO2</td>");
            sb.AppendLine("<td class='tabletitle'>O3_1h</td>");
            sb.AppendLine("<td class='tabletitle'>O3_8h</td>");
            sb.AppendLine("</tr>");
            foreach (DataRow dr in dtSiteData.Rows)
            {
                rowIndex++;
                sb.AppendLine(string.Format("<tr  onmouseover='mouseOver(this)' onmouseout='mouseOut(this)' id='{0}'>", rowIndex));
                sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", dr[0].ToString()));
                for (int i = 1; i < dtSiteData.Columns.Count; i++)
                {
                    value = dr[i].ToString();
                    if (value == "0" || value == "0.0")
                        value = "/";
                    else
                        value = Math.Round(Double.Parse(value), 1).ToString();
                    sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", value));

                }
                sb.AppendLine("</tr>");

            }
            sb.AppendLine("</table>");

            return sb.ToString();
        }
        public string GetAQIMethodCompare(string fromDate, string toDate)
        {
            string strSQL = "";
            string strReturn = "";
            string str = "";
            string x = "";
            string y = "";
            double minX = 1000000000000;
            string from = DateTime.Parse(fromDate).ToString("yyyy-MM-dd HH:mm:ss");
            string to = DateTime.Parse(toDate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
            string strWhere = " AND LST BETWEEN '" + from + "' AND '" + to + "'";
            Dictionary<string, string> dpoint = new Dictionary<string, string>();
            int[] ItemsID = { 901, 902, 903, 904, 905 };
            for (int i = 0; i < ItemsID.Length; i++)
            {
                strSQL = string.Format("SELECT  DATEDIFF(S,'1970-01-01 00:00:00',LST) AS LST,AQI from LT_RT_SiteData where SiteID=0 AND ItemID={0} {1} ORDER BY LST", ItemsID[i], strWhere);
                str = str + strSQL + ";";

            }
            DataSet ds = m_DatabaseS.GetDataset(str);
            for (int index = 0; index < ds.Tables.Count; index++)
            {
                DataTable dtElement = ds.Tables[index];
                x = ""; y = "";
                foreach (DataRow dr in dtElement.Rows)
                {
                    x = x + "|" + dr[0].ToString();
                    if (double.Parse(dr[0].ToString()) < minX)
                        minX = double.Parse(dr[0].ToString());
                    y = y + "|" + dr[1].ToString();
                }
                strReturn = strReturn + ",'" + index.ToString() + "':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "'";
            }
            if (strReturn != ",")
                strReturn = "{" + strReturn.TrimStart(',') + ",minX:" + minX.ToString() + "}";
            return strReturn;
            
        }
        public string DataShareCompare(string fromDate, string toDate, string itemID)
        {
            //string from = "2014/2/23 16:00:00";
            //string to = "2014/2/26 23:00:00";

            string from = DateTime.Parse(fromDate).ToString("yyyy-MM-dd HH:mm:ss");
            string to = DateTime.Parse(toDate).ToString("yyyy-MM-dd HH:mm:ss");
            string dateFiled = "  DATEDIFF(S,'1970-01-01 00:00:00', collect_time) AS time ";
            string  strWhere="WHERE station='" + itemID + "'  AND collect_time BETWEEN '" + from + "' AND '" + to + "' ORDER BY collect_time ASC";
            string strSQL;
            string strReturn = "";
            string x = "";
            string y = "";
            string z = "";
            strSQL = "SELECT " + dateFiled + " ,temperature from T_NMElement " + strWhere;
            strSQL = strSQL + ";SELECT " + dateFiled + ",wind_direction,wind_speed from T_NMElement " + strWhere;
            strSQL = strSQL + ";SELECT " + dateFiled + ",air_pressure from T_NMElement " + strWhere;
            strSQL = strSQL + ";SELECT " + dateFiled + ",rain_sum from T_NMElement " + strWhere;
            strSQL = strSQL + ";SELECT " + dateFiled + ",relativehumidity from T_NMElement " + strWhere;
            DataSet ds = m_Database.GetDataset(strSQL);
            for (int index = 0; index < ds.Tables.Count; index++)
            {
                DataTable dtElement = ds.Tables[index];
                x = ""; y = ""; z = "";
                if (index == 1)
                {
                    foreach (DataRow dr in dtElement.Rows)
                    {
                        x = x + "|" + dr[0].ToString();
                        if (dr[1].ToString() == "-9999.00")
                            y = y+"";
                        else
                            y = y + "|" + dr[1].ToString();
                        z = z + "|" + dr[2].ToString();
                    }
                    strReturn = strReturn + ",'" + index.ToString() + "':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "*" + z.TrimStart('|') + "'";
                }
                else
                {
                    foreach (DataRow dr in dtElement.Rows)
                    {
                        x = x + "|" + dr[0].ToString();
                        if (dr[1].ToString() == "-9999.00")
                            y = y+"";
                        else 
                            y = y + "|" + dr[1].ToString();
                    }
                    strReturn = strReturn + ",'" + index.ToString() + "':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "'";
                }
            }
            if (strReturn != ",")
                strReturn = "{" + strReturn.TrimStart(',') + "}";
            return strReturn;
          
        }
        public string DataShareCompareMulti(string fromDate, string toDate, string itemID)
        {
            //string from = "2014/2/23 16:00:00";
            //string to = "2014/2/26 23:00:00";

            string from = DateTime.Parse(fromDate).ToString("yyyy-MM-dd HH:mm:ss");
            string to = DateTime.Parse(toDate).ToString("yyyy-MM-dd HH:mm:ss");
            string dateFiled = "  DATEDIFF(S,'1970-01-01 00:00:00', collect_time) AS time ";
            string strWhere = "WHERE collect_time BETWEEN '" + from + "' AND '" + to + "'";
            string strSQL;
            string strReturn = "";
            string x = "";
            string y = "";
            string z = "";
            strSQL = "SELECT " + dateFiled + " ," + itemID + " from T_NMElement " + strWhere + " AND station=58367 ORDER BY collect_time ASC";
            strSQL = strSQL + ";SELECT " + dateFiled + " ," + itemID + " from T_NMElement " + strWhere + " AND station=58361 ORDER BY collect_time ASC";
            strSQL = strSQL + ";SELECT " + dateFiled + " ," + itemID + " from T_NMElement " + strWhere + " AND station=58370 ORDER BY collect_time ASC";
            strSQL = strSQL + ";SELECT " + dateFiled + " ," + itemID + " from T_NMElement " + strWhere + " AND station=58362 ORDER BY collect_time ASC";
            strSQL = strSQL + ";SELECT " + dateFiled + " ," + itemID + " from T_NMElement " + strWhere + " AND station=58462 ORDER BY collect_time ASC";
            strSQL = strSQL + ";SELECT " + dateFiled + " ," + itemID + " from T_NMElement " + strWhere + " AND station=58365 ORDER BY collect_time ASC";
            strSQL = strSQL + ";SELECT " + dateFiled + " ," + itemID + " from T_NMElement " + strWhere + " AND station=58461 ORDER BY collect_time ASC";
            strSQL = strSQL + ";SELECT " + dateFiled + " ," + itemID + " from T_NMElement " + strWhere + " AND station=58460 ORDER BY collect_time ASC";
            strSQL = strSQL + ";SELECT " + dateFiled + " ," + itemID + " from T_NMElement " + strWhere + " AND station=58463 ORDER BY collect_time ASC";
            strSQL = strSQL + ";SELECT " + dateFiled + " ," + itemID + " from T_NMElement " + strWhere + " AND station=58366 ORDER BY collect_time ASC";
            DataSet ds = m_Database.GetDataset(strSQL);
            for (int index = 0; index < ds.Tables.Count; index++)
            {
                DataTable dtElement = ds.Tables[index];
                x = ""; y = ""; 
                foreach (DataRow dr in dtElement.Rows)
                {
                    x = x + "|" + dr[0].ToString();
                    y = y + "|" + dr[1].ToString();
                }
                strReturn = strReturn + ",'" + index.ToString() + "':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "'";
            }
            if (strReturn != ",")
                strReturn = "{" + strReturn.TrimStart(',') + "}";
            return strReturn;

        }
        public string DataShareCompareQX(string fromDate, string toDate, string itemID)
        {
            //string from = "2014/1/1 10:00:00";
            //string to = "2014/1/8 10:00:00";
            string from = DateTime.Parse(fromDate).ToString("yyyy-MM-dd HH:mm:ss");
            string to = DateTime.Parse(toDate).ToString("yyyy-MM-dd HH:mm:ss");
         
            string dateFiled = "  DATEDIFF(S,'1970-01-01 00:00:00', LST) AS LST ";
            string strWhere = " ,VALUE from SEMC_DMC.DBO.LT_RT_SiteData WHERE SITEID=" + itemID + "  AND LST BETWEEN '" + from + "' AND '" + to + "'";
            string strSQL;
            string strReturn = "";
            string x = "";
            string y = "";
            string z = "";
            strSQL = "SELECT " + dateFiled + strWhere + " AND itemID='8' ORDER BY LST ASC ";
            strSQL = strSQL + "SELECT " + dateFiled + strWhere + " AND itemID='3' ORDER BY LST ASC; ";
            strSQL = strSQL + "SELECT " + dateFiled + strWhere + " AND itemID='2' ORDER BY LST ASC; ";
            strSQL = strSQL + "SELECT " + dateFiled + strWhere + " AND itemID='6' ORDER BY LST ASC; ";
            strSQL = strSQL + "SELECT " + dateFiled + strWhere + " AND itemID='7' ORDER BY LST ASC; ";
            strSQL = strSQL + "SELECT " + dateFiled + strWhere + " AND itemID='1' ORDER BY LST ASC; ";
            strSQL = strSQL + "SELECT " + dateFiled + strWhere + " AND itemID='5' ORDER BY LST ASC; ";
            DataSet ds = m_Database.GetDataset(strSQL);
            for (int index = 0; index < ds.Tables.Count; index++)
            {
                DataTable dtElement = ds.Tables[index];
                x = ""; y = "";
                if (index != 6)
                {
                    foreach (DataRow dr in dtElement.Rows)
                    {
                        x = x + "|" + dr[0].ToString();
                        if (dr[1].ToString() != "")
                            y = y + "|" + (float.Parse(dr[1].ToString()) * 1000).ToString();
                        else
                            y = y + "|" + dr[1].ToString();
                    }
                }
                else
                {
                    foreach (DataRow dr in dtElement.Rows)
                    {
                        x = x + "|" + dr[0].ToString();
                        y = y + "|" + dr[1].ToString();
                    }
                }
                strReturn = strReturn + ",'" + index.ToString() + "':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "'";
            }
            if (strReturn != ",")
                strReturn = "{" + strReturn.TrimStart(',') + "}";
            return strReturn;
        }
        public string DataShareCompareMultiQi(string fromDate, string toDate, string itemID)
        {
            //string from = "2014/1/1 10:00:00";
            //string to = "2014/1/8 10:00:00";
            string from = DateTime.Parse(fromDate).ToString("yyyy-MM-dd HH:mm:ss");
            string to = DateTime.Parse(toDate).ToString("yyyy-MM-dd HH:mm:ss");
           
            string dateFiled = "  DATEDIFF(S,'1970-01-01 00:00:00', LST) AS LST ";
            string strWhere = " ,VALUE from SEMC_DMC.DBO.LT_RT_SiteData WHERE itemID=" + itemID + "  AND LST BETWEEN '" + from + "' AND '" + to + "'";
            string strSQL;
            string strReturn = "";
            string x = "";
            string y = "";
            string z = "";
            strSQL = "SELECT " + dateFiled + strWhere + " AND SITEID='183' ORDER BY LST ASC ";
            strSQL = strSQL + "SELECT " + dateFiled + strWhere + " AND SITEID='185' ORDER BY LST ASC;";
            strSQL = strSQL + "SELECT " + dateFiled + strWhere + " AND SITEID='193' ORDER BY LST ASC; ";
            strSQL = strSQL + "SELECT " + dateFiled + strWhere + " AND SITEID='195' ORDER BY LST ASC; ";
            strSQL = strSQL + "SELECT " + dateFiled + strWhere + " AND SITEID='201' ORDER BY LST ASC; ";
            strSQL = strSQL + "SELECT " + dateFiled + strWhere + " AND SITEID='203' ORDER BY LST ASC; ";
            strSQL = strSQL + "SELECT " + dateFiled + strWhere + " AND SITEID='207' ORDER BY LST ASC; ";
            strSQL = strSQL + "SELECT " + dateFiled + strWhere + " AND SITEID='209' ORDER BY LST ASC; ";
            strSQL = strSQL + "SELECT " + dateFiled + strWhere + " AND SITEID='215' ORDER BY LST ASC; ";
            strSQL = strSQL + "SELECT " + dateFiled + strWhere + " AND SITEID='228' ORDER BY LST ASC; ";
            DataSet ds = m_Database.GetDataset(strSQL);
            if (itemID == "5")
            {
                for (int index = 0; index < ds.Tables.Count; index++)
                {
                    DataTable dtElement = ds.Tables[index];
                    x = ""; y = "";

                    foreach (DataRow dr in dtElement.Rows)
                    {
                        x = x + "|" + dr[0].ToString();
                        y = y + "|" + dr[1].ToString();
                    }
                    strReturn = strReturn + ",'" + index.ToString() + "':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "'";
                }
            }
            else
            {
                for (int index = 0; index < ds.Tables.Count; index++)
                {
                    DataTable dtElement = ds.Tables[index];
                    x = ""; y = "";

                    foreach (DataRow dr in dtElement.Rows)
                    {
                        x = x + "|" + dr[0].ToString();
                        if (dr[1].ToString() != "")
                            y = y + "|" + (float.Parse(dr[1].ToString()) * 1000).ToString();
                        else
                            y = y + "|" + dr[1].ToString();
                    }
                    strReturn = strReturn + ",'" + index.ToString() + "':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "'";
                }
            }
            if (strReturn != ",")
                strReturn = "{" + strReturn.TrimStart(',') + "}";
            return strReturn;
        }
        public string tableSimpleQuery(string fromDate, string toDate, string itemID)
        {
            //string dtFrom = "2014/2/23 16:00:00";
            //string dtTo = "2014/2/26 23:00:00";
            DateTime dtFrom = DateTime.Parse(fromDate);
            DateTime dtTo = DateTime.Parse(toDate);
            string strSQL = "SELECT collect_time,temperature,wind_direction,wind_speed,air_pressure,rain_sum,relativehumidity from T_NMElement WHERE station=" + itemID + "  AND collect_time BETWEEN '" + dtFrom + "' AND '" + dtTo + "' ORDER BY collect_time ASC";
            DataTable dt = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table id='tableTable'  width='100%' border='0' cellpadding='0' cellspacing='0' class='tablekuang'>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='tabletitle'>时间</td>");
            sb.AppendLine("<td class='tabletitle'>温度(°C)</td>");
            sb.AppendLine("<td class='tabletitle'>风向(°)</td>");
            sb.AppendLine("<td class='tabletitle'>风速(m/s)</td>");
            sb.AppendLine("<td class='tabletitle'>气压(hPa)</td>");
            sb.AppendLine("<td class='tabletitle'>降雨量(mm)</td>");
            sb.AppendLine("<td class='tabletitle'>相对湿度(%)</td>");
            sb.AppendLine("</tr>");
            int rowIndex = 0;
            foreach (DataRow dr in dt.Rows)
            {
                rowIndex++;
                sb.AppendLine(string.Format("<tr onmouseover='mouseOver(this)' onmouseout='mouseOut(this)' id='{0}'>", rowIndex));
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb.AppendLine(string.Format("<td class='tablerow' id='{1}'>{0}</td>", dr[i].ToString(), rowIndex.ToString() + "4"));
                }
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</table>");

            return sb.ToString();
        }
        public string tableSimpleQueryH(string fromDate, string toDate, string itemID)
        {
            //string dtFrom = "2014/1/1 10:00:00";
            //string dtTo = "2014/1/8 10:00:00";
            DateTime dtFrom = DateTime.Parse(fromDate);
            DateTime dtTo = DateTime.Parse(toDate);
            int[] itemOrder = { 8,3,2, 6, 7, 1, 5 };
            string strSQL = string.Format("SELECT LST,ITEMID,SITEID,CONVERT(decimal(10, 1), VALUE * 1000) AS VALUE FROM SEMC_DMC.DBO.LT_RT_SiteData WHERE LST BETWEEN '{0}' AND '{1}' AND ITEMID <=9 AND SITEID='{2}' ORDER BY LST ASC", dtFrom, dtTo, itemID);
            DataTable dtSiteData = m_Database.GetDataTable(strSQL);

            DataTable distinctLst = dtSiteData.DefaultView.ToTable(true, "LST");


            StringBuilder sb = new StringBuilder();
            StringBuilder tdValue = new StringBuilder();
            string className = string.Empty;
            sb.AppendLine("<table id='HuanjingTable'  width='100%' border='0' cellpadding='0' cellspacing='0' class='tablekuang'>");
            sb.AppendLine("<tr>");

            //创建抬头
            sb.AppendLine("<td class='tabletitle'>日期</td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>2.5</sub></td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>10</sub></td>");
            sb.AppendLine("<td class='tabletitle'>NO<sub>2</sub></td>");
            sb.AppendLine("<td class='tabletitle'>O<sub>3</sub>-1h</td>");
            sb.AppendLine("<td class='tabletitle'>O<sub>3</sub>-8h</td>");
            sb.AppendLine("<td class='tabletitle'>SO<sub>2</sub></td>");
            sb.AppendLine("<td class='tabletitle'>CO(mg/m3)</td>");
            sb.AppendLine("</tr>");
            int rowIndex = 0;
            foreach (DataRow row in distinctLst.Rows)
            {
                rowIndex++;
                sb.AppendLine(string.Format("<tr onmouseover='mouseOver(this)' onmouseout='mouseOut(this)' id='{0}'>", rowIndex));
                sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", DateTime.Parse(row[0].ToString()).ToString("yyyy-MM-dd HH:mm")));
                for (int i = 0; i < itemOrder.Length; i++)
                {
                    string filter = string.Format("LST = '{0}' AND ITEMID = {1}", row[0], itemOrder[i]);
                    DataRow[] rows = dtSiteData.Select(filter);
                    if (rows.Length == 0)
                        sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", "/"));
                    else
                    {
                        for (int j = 0; j < rows.Length; j++)
                        {
                            if (itemOrder[i] != 5)
                            {
                                sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", rows[j]["VALUE"].ToString()));
                            }
                            else
                            {
                                sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", Math.Round(double.Parse(rows[j]["VALUE"].ToString()) / 1000, 1)));
                            }
                        }
                    }
                }
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");

            return sb.ToString();
        }
        public string tableSimpleQueryQ(string fromDate, string toDate, string itemID)
        {

            //string dtFrom = "2014/2/23 16:00:00";
            //string dtTo = "2014/2/26 23:00:00";
            DateTime dtFrom = DateTime.Parse(fromDate);
            DateTime dtTo = DateTime.Parse(toDate);
            int[] itemOrder = {58367,58361,58370,58362, 58462,58365,58461,58460,58463,58366};

            string strSQL = string.Format("SELECT collect_time,station,{2}  FROM T_NMElement WHERE collect_time BETWEEN '{0}' AND '{1}' ORDER BY collect_time ASC", dtFrom, dtTo, itemID);
            DataTable dtSiteData = m_Database.GetDataTable(strSQL);

            DataTable distinctLst = dtSiteData.DefaultView.ToTable(true, "collect_time");
            StringBuilder sb = new StringBuilder();
            StringBuilder tdValue = new StringBuilder();
            string className = string.Empty;
            sb.AppendLine("<table id='HuanjingQTable'  width='100%' border='0' cellpadding='0' cellspacing='0' class='tablekuang'>");
            sb.AppendLine("<tr>");

            //创建抬头
            sb.AppendLine("<td class='tabletitle'>日期</td>");
            sb.AppendLine("<td class='tabletitle'>徐家汇</td>");
            sb.AppendLine("<td class='tabletitle'>闵行</td>");
            sb.AppendLine("<td class='tabletitle'>浦东</td>");
            sb.AppendLine("<td class='tabletitle'>宝山</td>");
            sb.AppendLine("<td class='tabletitle'>松江</td>");
            sb.AppendLine("<td class='tabletitle'>嘉定</td>");
            sb.AppendLine("<td class='tabletitle'>青浦</td>");
            sb.AppendLine("<td class='tabletitle'>金山</td>");
            sb.AppendLine("<td class='tabletitle'>奉贤</td>");
            sb.AppendLine("<td class='tabletitle'>崇明</td>");
            sb.AppendLine("</tr>");
            int rowIndex = 0;
            foreach (DataRow row in distinctLst.Rows)
            {
                rowIndex++;
                sb.AppendLine(string.Format("<tr onmouseover='mouseOver(this)' onmouseout='mouseOut(this)'  id='{0}'>", rowIndex));
                sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", DateTime.Parse(row[0].ToString()).ToString("yyyy-MM-dd HH:mm")));
                for (int i = 0; i < itemOrder.Length; i++)
                {
                    string filter = string.Format("collect_time = '{0}' AND station = {1}", row[0], itemOrder[i]);
                    DataRow[] rows = dtSiteData.Select(filter);
                    if (rows.Length == 0)
                        sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", "/"));
                    else
                    {
                        for (int j = 0; j < rows.Length; j++)
                        {
                            sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>",rows[j][2].ToString()));
                        }
                    }
                }
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");

            return sb.ToString();
            
        }
        public string tableSimpleQueryQH(string fromDate, string toDate, string itemID)
        {
            //string dtFrom = "2014/1/1 10:00:00";
            //string dtTo = "2014/1/8 10:00:00";
            DateTime dtFrom = DateTime.Parse(fromDate);
            DateTime dtTo = DateTime.Parse(toDate);
            int[] itemOrder = { 183, 185, 193, 195, 201, 203, 207, 209, 215, 228 };
            string itemTemp = "(183, 185, 193, 195, 201, 203, 207, 209, 215, 228)";
            string strSQL = string.Format("SELECT LST,ITEMID,SITEID,CONVERT(decimal(10, 1), VALUE * 1000) AS VALUE FROM SEMC_DMC.DBO.LT_RT_SiteData WHERE LST BETWEEN '{0}' AND '{1}' AND ITEMID='{2}' AND SITEID in {3} ORDER BY LST ASC", dtFrom, dtTo, itemID, itemTemp);
            DataTable dtSiteData = m_Database.GetDataTable(strSQL);

            DataTable distinctLst = dtSiteData.DefaultView.ToTable(true, "LST");
            StringBuilder sb = new StringBuilder();
            StringBuilder tdValue = new StringBuilder();
            string className = string.Empty;
            sb.AppendLine("<table id='HuanjingQHTable'  width='100%' border='0' cellpadding='0' cellspacing='0' class='tablekuang'>");
            sb.AppendLine("<tr>");

            //创建抬头
            sb.AppendLine("<td class='tabletitle'>日期</td>");
            sb.AppendLine("<td class='tabletitle'>静安监测站</td>");
            sb.AppendLine("<td class='tabletitle'>卢湾师专附小</td>");
            sb.AppendLine("<td class='tabletitle'>浦东川沙</td>");
            sb.AppendLine("<td class='tabletitle'>浦东张江</td>");
            sb.AppendLine("<td class='tabletitle'>普陀监测站</td>");
            sb.AppendLine("<td class='tabletitle'>青浦淀山湖</td>");
            sb.AppendLine("<td class='tabletitle'>徐汇上师大</td>");
            sb.AppendLine("<td class='tabletitle'>杨浦四漂</td>");
            sb.AppendLine("<td class='tabletitle'>虹口凉城</td>");
            sb.AppendLine("<td class='tabletitle'>浦东监测站</td>");
            sb.AppendLine("</tr>");
            int rowIndex = 0;
            foreach (DataRow row in distinctLst.Rows)
            {
                rowIndex++;
                sb.AppendLine(string.Format("<tr  onmouseover='mouseOver(this)' onmouseout='mouseOut(this)' id='{0}'>", rowIndex));
                sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", DateTime.Parse(row[0].ToString()).ToString("yyyy-MM-dd HH:mm")));
                for (int i = 0; i < itemOrder.Length; i++)
                {
                    string filter = string.Format("LST = '{0}' AND SITEID = {1}", row[0], itemOrder[i]);
                    DataRow[] rows = dtSiteData.Select(filter);
                    if (rows.Length == 0)
                        sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", "/"));
                    else
                    {
                        for (int j = 0; j < rows.Length; j++)
                        {
                            if (itemID!= "5")
                            {
                                sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", rows[j]["VALUE"].ToString()));
                            }
                            else
                            {
                                sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", Math.Round(double.Parse(rows[j]["VALUE"].ToString()) / 1000, 1)));
                            }
                        }
                    }
                }
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");

            return sb.ToString();

        }
        //判断路径是否存在，从而显示最新的数据
        public string QueryFileExists(string html, string todayDate)
        {
            string src = todayDate;
            while (!Directory.Exists(@"E:\\SMMCDatabase\\" + html + "\\" + src + "\\"))
            {
                DateTime dTime = DateTime.Parse(src.Substring(0, 4) + "-" + src.Substring(4, 2) + "-" + src.Substring(6, 2)).AddDays(-1);
                src = dTime.ToString("yyyyMMdd");
            }
            return src;

        }
        public string SiteDataList()
        {
            string strSQL = "SELECT * From NAQPMS_YTSite";
            DataTable dt = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<ul>");
            sb.AppendLine("<li class='liClasstitle'>城市&nbsp&nbsp&nbsp&nbsp站点</li>");
            int i = 0;
            foreach (DataRow rows in dt.Rows)//
            {
                if(i==0)
                    sb.AppendLine("<li id='" + rows["ID"] + "' onmouseover='mouseOver(this)' onmouseout='mouseOut(this)' class='liClassSelect' onclick=\"liClick('" + rows["ID"] + "')\">" + rows["City"] + "&nbsp&nbsp&nbsp&nbsp" + rows["SiteName"] + "</li>");
                else
                    sb.AppendLine("<li id='" + rows["ID"] + "' onmouseover='mouseOver(this)' onmouseout='mouseOut(this)' class='liClass' onclick=\"liClick('" + rows["ID"] + "')\">" + rows["City"] + "&nbsp&nbsp&nbsp&nbsp" + rows["SiteName"] + "</li>");
                i++;
            }
            sb.AppendLine("<ul>");
            return sb.ToString();

        }
        
    }
}
