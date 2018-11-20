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
public partial class AQI_ForecastQuenceDiagram : System.Web.UI.Page
{
    public string m_FromDate;
    public string m_ToDate;
    private Database m_Database;
    public bool m_UnLogin;
    protected void Page_Load(object sender, EventArgs e)
    {
         //如果当前用户没有登录的话，那么返回到登录界面
        //if (Session["SYSTEMUSERID"] == null)
        //{
        //    m_UnLogin = true;
        //    return;
        //}
        if (!Page.IsPostBack)
        {
            m_Database = new Database();
            string strSQL = "SELECT MAX(LST) AS maxLST FROM  T_ForecastSite WHERE (ITEMID = 1 OR  ITEMID = 4) AND durationID =10 AND SiteID =0";
            DataTable dt= m_Database.GetDataTable(strSQL);
            if (dt.Rows.Count > 0)
            {
                m_ToDate = DateTime.Parse(dt.Rows[0][0].ToString()).ToString("yyyy年MM月dd日");
                m_FromDate = DateTime.Parse(dt.Rows[0][0].ToString()).AddDays(-3).ToString("yyyy年MM月dd日");
            }
                
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Database m_Database = new Database();
        DataTable dSearch = new DataTable("T_WrfExport");
        dSearch.Columns.Add("日期", typeof(string));
        dSearch.Columns.Add("O3-1h", typeof(string));
        dSearch.Columns.Add("PM2.5", typeof(string));
        string Content = Element.Value;
        string[] strElement;
        strElement = Content.Split('|');
        DateTime dtFrom = DateTime.Parse(strElement[0]);
        DateTime dtTo = DateTime.Parse(strElement[1]).AddHours(-1);
        string[] itemOrder = { "03-1h", "PM2.5" };
        string strSQL = string.Format("SELECT LST,MC AS ITEM,round(AVG(value),1) AS VALUE,'WRF' AS MODULE from t_forecastsite INNER JOIN D_Item on d_item.Code=t_forecastsite.ITEMID where durationid =10 and module='wrf' and d_item.Code in (1,4) and lst between '{0}' AND '{1}' group by LST,mc order by item,lst", dtFrom, dtTo);
      
        DataTable dtSiteData = m_Database.GetDataTable(strSQL);
        DataTable distinctLst = dtSiteData.DefaultView.ToTable(true, "LST");
        foreach (DataRow row in distinctLst.Rows)
        {
            DataRow newRow = dSearch.NewRow();
            newRow[0] = DateTime.Parse(row[0].ToString()).ToString("yyyy-MM-dd HH:mm").ToString();
            for (int i = 0; i < itemOrder.Length; i++)
            {
                string filter = string.Format("LST = '{0}' AND ITEM ='{1}'", row[0], itemOrder[i]);
                DataRow[] rows = dtSiteData.Select(filter);
                newRow[i + 1] =Math.Round(double.Parse(rows[0]["VALUE"].ToString()), 1).ToString();
       
            }
            dSearch.Rows.Add(newRow);
        }
        string name = "数值预报中的三天预报";
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("日期,03-1h,PM2.5");
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


    
