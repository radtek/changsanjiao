using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Readearth.Data;
using System.Text;
using System.Windows.Forms;
using WebExcel;
using System.IO;
public partial class AQI_AirQuality : System.Web.UI.Page
{
    public bool m_UnLogin;
    public string m_FirstTab;
    public string m_UserLimit;
    protected void Page_Load(object sender, EventArgs e)
    {
       
        //如果当前用户没有登录的话，那么返回到登录界面
        //if (Session["SYSTEMUSERID"] == null)
        //{
        //    m_UnLogin = true;
        //    return;
        //}
        DateTime dtNow = DateTime.Now;

        H00.Value = dtNow.AddHours(-24).ToString("yyyy年MM月dd日 HH时");
        H01.Value = dtNow.ToString("yyyy年MM月dd日 HH时");
        m_UserLimit = Request.Cookies["User"]["indexUser"] + "," + Request.Cookies["User"]["name"];
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
        string[] user = m_UserLimit.Split(',');
        if (user[1].ToString() == "1")
        {
            sbTabs.Append("<li><span id='T000' class='tabHighlight'>全市平均</span></li>");
            m_FirstTab = "T000";
        }


        for (int i = 0; i < dtSite.Rows.Count; i++)
        {
            DataRow row = dtSite.Rows[i];

            if (i == 0)
            {
                sb.AppendFormat("<div id='P{0}' class='popup_text_disable'>{1}</div>", row[0], row[1]);
                if (user[1].ToString() == "1")
                    sbTabs.AppendFormat("<li><span id='T{0}'><a href=\"javascript:tabClick('T{0}');\">{1}<img src='images/b_close.png' class='close_ico' onclick=\"closeTab('T{0}');\" /></a></span></li>", row[0], row[1]);
                else
                {
                    sbTabs.AppendFormat("<li><span id='T{0}' class='tabHighlight'>{1}</span></li>", row[0], row[1]);
                    m_FirstTab = string.Format("T{0}", row[0]);
                }
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
        string strSQL = string.Format("SELECT LST,ITEMID,SITEID,CONVERT(decimal(10, 1), VALUE * 1000) AS VALUE,AQI FROM SEMC_DMC.DBO.LT_RT_SiteData WHERE LST BETWEEN '{0}' AND '{1}' AND ITEMID <=9 AND SITEID ='{2}'", dtFrom, dtTo, strElement[2]);
        DataTable dtSiteData = m_Database.GetDataTable(strSQL);
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
            strSQL = "SELECT SITEID,NAME FROM T_SITE WHERE SITEID='" + strElement[2] + "'";
            DataTable dtSiteName = m_Database.GetDataTable(strSQL);
            name = dtSiteName.Rows[0][1].ToString();
        }
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("日期,PM2.5,PM2.5-24h,PM10,PM10-24h,NO2,O3-1h,O3-8h,SO2,CO");
        foreach (DataRow dr in dSearch.Rows)
        {
            string dataLine="";
            for (int i = 0; i < dSearch.Columns.Count; i++)
            {
                dataLine = dataLine+dr[i] + ",";
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
