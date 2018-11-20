using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Readearth.Data;
using ChinaAQI;
using System.Text;
using System.IO;

public partial class AQI_siteDay : System.Web.UI.Page
{
    public bool m_UnLogin;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["SYSTEMUSERID"] == null)
        //{
        //    m_UnLogin = true;
        //    return;
        //}
        DateTime dtNow = DateTime.Now.AddDays(-1);

        H00.Value = dtNow.ToString("yyyy年MM月dd日");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
         string filter;
         Database m_Database = new Database();
         DataTable dSearch = new DataTable("T_AirQuality");
         dSearch.Columns.Add("监测点名称", typeof(string));
         dSearch.Columns.Add("PM2.5（浓度）", typeof(string));
         dSearch.Columns.Add("PM2.5（AQI）", typeof(string));
         dSearch.Columns.Add("PM10（浓度）", typeof(string));
         dSearch.Columns.Add("PM10（AQI）", typeof(string));
         dSearch.Columns.Add("NO2（浓度）", typeof(string));
         dSearch.Columns.Add("NO2（AQI）", typeof(string));
         dSearch.Columns.Add("O3-1h（浓度）", typeof(string));
         dSearch.Columns.Add("O3-1h（AQI）", typeof(string));
         dSearch.Columns.Add("O3-8h（浓度）", typeof(string));
         dSearch.Columns.Add("O3-8h（AQI）", typeof(string));
         dSearch.Columns.Add("VI（浓度）", typeof(string));
         dSearch.Columns.Add("VI（AQI）", typeof(string));
         dSearch.Columns.Add("空气污染指数AQI", typeof(string));
         dSearch.Columns.Add("空气质量等级", typeof(string));
         dSearch.Columns.Add("主要污染物", typeof(string));


         string Content = Element.Value;
         DateTime dtFrom = DateTime.Parse(Content);
         string strSQL = string.Format("SELECT T_Site.Name,6 as ITEMID, (30-POWER(value,1.0/2.0)) as value,AQI  from T_ForecastSite inner join T_Site on T_ForecastSite.SiteID=T_Site.SiteID  where durationid =7 and module='wrf' and itemid=2 and lst='{0}' and period=24 UNION  SELECT T_Site.Name,T_ForecastSite.ITEMID ,T_ForecastSite.Value,T_ForecastSite.AQI  from T_ForecastSite  inner join T_Site on T_ForecastSite.SiteID=T_Site.SiteID where durationid =7 and module='wrf'  and lst='{0}' and period=24 ORDER BY T_Site.Name", dtFrom);
         DataTable dtSiteData = m_Database.GetDataTable(strSQL);
         string siteName;
         int preItems;
         AQIExtention aqiExt;
         strSQL = "SELECT DM,MC FROM D_ITEM";
         DataTable items = m_Database.GetDataTable(strSQL);
         for (int i = 0; i < dtSiteData.Rows.Count / 6; i++)
         {
             DataRow newRow = dSearch.NewRow();
             siteName = dtSiteData.Rows[i * 6 + 1][0].ToString();
             newRow[0] = siteName;
             int maxAQI = 0;
             for (int j = 1; j < 7; j++)
             {
                 filter = string.Format("Name= '{0}' AND ITEMID = {1}", siteName, j);
                 DataRow[] rows = dtSiteData.Select(filter);
                 if (rows.Length > 0)
                 {
                     newRow[2*j-1] = rows[0]["VALUE"];
                     newRow[2*j] = rows[0]["AQI"];
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
                     paraments = paraments + "";
                 }
                 else
                 {
                     filter = string.Format("DM = {0}", preItems);
                     DataRow[] itemsDataRow = items.Select(filter);
                     paraments = paraments + "   " + itemsDataRow[0][1].ToString();
                 }

             }

             aqiExt = new AQIExtention(maxAQI);
             newRow[13] = maxAQI;
             newRow[14] = aqiExt.Quality;
             newRow[15] = paraments;
             dSearch.Rows.Add(newRow);
         }

         string name = "站点日报(" + dtFrom.ToString("yyyy-MM-dd")+")";
       
         StringBuilder sb = new StringBuilder();
         sb.AppendLine("监测站名称,PM2.5（浓度）,PM2.5（AQI）,PM10（浓度）,PM10.5（AQI）,NO2（浓度）,NO2（AQI）,O3-1h（浓度）,O3-1h（AQI）,O3-8h（浓度）,O3-8h（AQI）,VI（浓度）,VI（AQI）,AQI,空气质量等级,首要污染物");
         foreach (DataRow dr in dSearch.Rows)
         {
             string dataLine = "";
             for (int i = 0; i < dSearch.Columns.Count; i++)
             {
                 dataLine = dataLine + dr[i] + ",";
             }
             sb.AppendLine(dataLine.Substring(0, dataLine.Length - 1));
         }
         StringWriter SW = new StringWriter(sb);
         SW.Close();
         string saveAsFileName = string.Format("{0}.csv", name);
         Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(saveAsFileName));
         Response.ContentType = "application/ms-excel";
         Response.Charset = "GB2312";
         Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
         Response.Write(SW);
         Response.Flush();
         Response.End();

    }
}
