using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data;
using Readearth.Data;
using MMShareBLL.DAL;

public partial class EvaluateHtml_AirQuality : System.Web.UI.Page
{
    public bool m_UnLogin;
    public string m_FirstTab;
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime dtNow = DateTime.Now;

        H00.Value = dtNow.AddHours(-24).ToString("yyyy年MM月dd日 HH时");
        H01.Value = dtNow.ToString("yyyy年MM月dd日 HH时");
        CreateSitePanel();
    }
    /// <summary>
    /// 创建国控点面板和缺省的tab页
    /// </summary>
    private void CreateSitePanel()
    {
        Database db = new Database();
        string strSQL = "SELECT SITEID,NAME FROM T_SITE WHERE (IsGuokong = 1 OR IsGuokong = 2) ORDER BY SiteID";
        DataTable dtSite = db.GetDataTable(strSQL);
        StringBuilder sb = new StringBuilder();
        StringBuilder sbTabs = new StringBuilder();

        for (int i = 0; i < dtSite.Rows.Count; i++)
        {
            DataRow row = dtSite.Rows[i];

            if (i == 0)
            {
                sb.AppendFormat("<div id='P{0}' class='popup_text_disable'>{1}</div>", row[0], row[1]);
                sbTabs.AppendFormat("<li><span id='T{0}' class='tabHighlight'>{1}</span></li>", row[0], row[1]);
                m_FirstTab = string.Format("T{0}", row[0]);
            }
            else
                sb.AppendFormat("<div id='P{0}' class='popup_text'><a href='#' onclick='addSite(this,{0})'>{1}</a></div>", row[0], row[1]);
        }
        sbTabs.Append("");
        Add_popup.InnerHtml = sb.ToString();
        tabItem.InnerHtml = sbTabs.ToString() + tabItem.InnerHtml;

        sb.Length = 0;
        sbTabs.Length = 0;
    }
    protected void Button1_Click1(object sender, EventArgs e)
    {

        Database m_Database = new Database();
        DataTable dSearch = new DataTable("T_AirQuality");
        dSearch.Columns.Add("日期", typeof(string));
        dSearch.Columns.Add("PM2.5", typeof(string));
        dSearch.Columns.Add("PM2.5-24h", typeof(string));
        dSearch.Columns.Add("PM10", typeof(string));
        dSearch.Columns.Add("PM10-24h", typeof(string));
        dSearch.Columns.Add("NO2", typeof(string));
        dSearch.Columns.Add("O3-1h", typeof(string));
        dSearch.Columns.Add("O3-8h", typeof(string));
        dSearch.Columns.Add("SO2", typeof(string));
        dSearch.Columns.Add("CO", typeof(string));
        string Content = Element.Value;
        string[] strElement;
        strElement = Content.Split('|');
        DateTime dtFrom = DateTime.Parse(strElement[0]);
        DateTime dtTo = DateTime.Parse(strElement[1]);
        int[] itemOrder = { 8, 9, 3, 4, 2, 6, 7, 1, 5 };
        EvalutionCaculate EvalutionCaculate = new EvalutionCaculate();
        string formatUrl = string.Format("http://219.233.250.38:8087/semcshare/PatrolHandler.do?provider=MMShareBLL.DAL.AirData&method=AirQualityPuDong&fromDate={0}&toDate={1}&siteIDs={2}", dtFrom, dtTo, strElement[2]);
        string forecastStr =EvalutionCaculate.GetValues(formatUrl);
        DataTable dtSiteData = EvalutionCaculate.JsonToDataTable(forecastStr);

        DataTable distinctLst = dtSiteData.DefaultView.ToTable(true, "LST");
        foreach (DataRow row in distinctLst.Rows)
        {
            DataRow newRow = dSearch.NewRow();
            newRow[0] = DateTime.Parse(row[0].ToString()).ToString("yyyy-MM-dd HH:mm").ToString();
            for (int i = 0; i < itemOrder.Length; i++)
            {
                string filter = string.Format("LST = '{0}' AND ITEMID = {1}", row[0], itemOrder[i]);
                DataRow[] rows = dtSiteData.Select(filter);
                for (int j = 0; j < rows.Length; j++)
                {
                    if (int.Parse(itemOrder[i].ToString()) == 5)
                        newRow[i + 1] = string.Format("{0}/{1}", Math.Round(double.Parse(rows[j]["VALUE"].ToString()) / 1000, 1), rows[j]["AQI"]);
                    else if (int.Parse(itemOrder[i].ToString()) == 8 || int.Parse(itemOrder[i].ToString()) == 3)
                        newRow[i + 1] = string.Format("{0}", rows[j]["VALUE"]);
                    else
                        newRow[i + 1] = string.Format("{0}/{1}", rows[j]["VALUE"], rows[j]["AQI"]);

                }
            }
            dSearch.Rows.Add(newRow);
        }
        string name = "";
        if (strElement[2] == "000")
            name = "全市平均";
        else
        {
            string  strSQL = "SELECT SITEID,NAME FROM T_SITE WHERE SITEID='" + strElement[2] + "'";
            DataTable dtSiteName = m_Database.GetDataTable(strSQL);
            name = dtSiteName.Rows[0][1].ToString();
        }
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("日期,PM2.5,PM2.5-24h,PM10,PM10-24h,NO2,O3-1h,O3-8h,SO2,CO");
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
