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
using System.Text;
using System.IO;

public partial class AQI_StationPerHour : System.Web.UI.Page
{
    public bool m_UnLogin;
    public string m_FirstTab;
    public string m_UserLimit;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["SYSTEMUSERID"] == null)
        //{
        //    m_UnLogin = true;
        //    return;
        //}
        DateTime dtNow = DateTime.Now.AddDays(-1);

        H00.Value = dtNow.ToString("yyyy年MM月dd日");
        CreateSitePanel();

    }
    private void CreateSitePanel()
    {
        Database db = new Database();
        string strSQL = "SELECT SITEID,NAME FROM T_SITE WHERE (IsGuokong = 1 ) ORDER BY SiteID";
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
    protected void Button1_Click(object sender, EventArgs e)
    {
        Database m_Database = new Database();
        DataTable dSearch = new DataTable("T_AirQuality");
        dSearch.Columns.Add("日期", typeof(string));
        dSearch.Columns.Add("PM2.5", typeof(string));
        dSearch.Columns.Add("PM10", typeof(string));
        dSearch.Columns.Add("NO2", typeof(string));
        dSearch.Columns.Add("O3-1h", typeof(string));
        dSearch.Columns.Add("O3-8h", typeof(string));
        dSearch.Columns.Add("VI", typeof(string));
        string Content = Element.Value;
        string[] strElement;
        strElement = Content.Split('|');
        DateTime dtFrom = DateTime.Parse(strElement[0]);
        DateTime dtTo = DateTime.Parse(strElement[0]).AddHours(23);
        string strSQL = string.Format(" select LST,6 as ITEMID,SITEID,(30-POWER(value,1.0/2.0)) as VALUE,AQI from T_ForecastSite where DURATIONID =10 and MODULE='wrf' and ITEMID=2 and LST between '{0}' AND '{1}' and SITEID={2} AND PERIOD=24 union SELECT LST,ITEMID,SITEID,VALUE,AQI FROM T_ForecastSite WHERE DURATIONID=10 AND MODULE='wrf' AND LST BETWEEN '{0}' AND '{1}' AND SITEID={2} AND PERIOD=24 ORDER BY LST", dtFrom, dtTo, strElement[1]);
        DataTable dtSiteData = m_Database.GetDataTable(strSQL);
        DataTable distinctLst = dtSiteData.DefaultView.ToTable(true, "LST");
        foreach (DataRow row in distinctLst.Rows)
        {
            DataRow newRow = dSearch.NewRow();
            newRow[0] = DateTime.Parse(row[0].ToString()).ToString("yyyy-MM-dd HH:mm").ToString();
            for (int i = 0; i <6; i++)
            {
                string filter = string.Format("LST = '{0}' AND ITEMID = {1}", row[0], i+1);
                DataRow[] rows = dtSiteData.Select(filter);
                for (int j = 0; j < rows.Length; j++)
                {
                    newRow[i + 1] = string.Format("{0}/{1}", rows[j]["VALUE"], rows[j]["AQI"]);

                }
            }
            dSearch.Rows.Add(newRow);
        }
        string name = "";
        strSQL = "SELECT SITEID,NAME FROM T_SITE WHERE SITEID='" + strElement[1] + "'";
        DataTable dtSiteName = m_Database.GetDataTable(strSQL);
        name = dtSiteName.Rows[0][1].ToString();
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("日期,PM2.5,PM10,NO2,O3-1h,O3-8h,VI");
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
