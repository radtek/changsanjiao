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

public partial class AQI_AQIMethodCompare : System.Web.UI.Page
{
    public string m_FromDate;
    public string m_ToDate;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DateTime dtNow = DateTime.Now;
            m_FromDate = dtNow.AddDays(-7).ToString("yyyy年MM月dd日");
            m_ToDate = dtNow.ToString("yyyy年MM月dd日");
        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Database m_DatabaseS = new Database("SEMCDMC");
        DataTable dSearch = new DataTable("T_SiteData");
        dSearch.Columns.Add("日期", typeof(string));
        dSearch.Columns.Add("细颗粒物1小时", typeof(string));
        dSearch.Columns.Add("细颗粒物3小时", typeof(string));
        dSearch.Columns.Add("细颗粒物24小时", typeof(string));
        dSearch.Columns.Add("细颗粒物Nowcast", typeof(string));
        dSearch.Columns.Add("细颗粒物S", typeof(string));
        int[] ItemsID = { 901, 902, 903, 904, 905 };
        string ItemID="(901, 902, 903, 904, 905)";
        string Content = Element.Value;
        string[] strElement;
        strElement = Content.Split('|');
        DateTime dtFrom = DateTime.Parse(strElement[0]);
        DateTime dtTo = DateTime.Parse(strElement[1]).AddHours(-1);
        string strWhere = " AND LST BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
        string strSQL = string.Format("SELECT LST,value,ItemID from LT_RT_SiteData where SiteID=0 AND ItemID in{0} {1} ORDER BY LST", ItemID, strWhere);
        DataTable dtSiteData = m_DatabaseS.GetDataTable(strSQL);
        DataTable distinctLst = dtSiteData.DefaultView.ToTable(true, "LST");
        foreach (DataRow row in distinctLst.Rows)
        {
            DataRow newRow = dSearch.NewRow();
            newRow[0] = DateTime.Parse(row[0].ToString()).ToString("yyyy-MM-dd HH:mm").ToString();
            for (int i = 0; i < ItemsID.Length; i++)
            {
                string filter = string.Format("LST = '{0}' AND ItemID ='{1}'", row[0], ItemsID[i]);
                DataRow[] rows = dtSiteData.Select(filter);
                if (rows.Length > 0)
                {
                    if (rows[0][1].ToString() != "")
                        newRow[i + 1] = Math.Round(double.Parse(rows[0][1].ToString()), 4).ToString();
                    else
                        newRow[i + 1] = "";
                }
                else
                    newRow[i + 1] = "";


            }
            dSearch.Rows.Add(newRow);
        }
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("日期,细颗粒物1小时,细颗粒物3小时,细颗粒物24小时,细颗粒物Nowcast,细颗粒物S");
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
        string saveAsFileName = string.Format("{0}.csv", "AQI对比");
        Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(saveAsFileName));
        Response.ContentType = "application/ms-excel";
        Response.Charset = "GB2312";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.Write(SW);
        Response.Flush();
        Response.End();
    }
}
