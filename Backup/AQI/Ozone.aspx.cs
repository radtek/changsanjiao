using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Readearth.Data;
using System.Data;

public partial class AQI_Ozone : System.Web.UI.Page
{
    public string m_FromDate;
    public string m_ToDate;
    public string m_Station;
    private Database m_Database;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            m_Station = Request["Station"];
            m_Database = new Database();
            string strSQL = "SELECT MAX(LST) AS maxLST FROM  DMS_DATA WHERE parameterid in (213,214,215,216,8) AND SiteID='" + m_Station + "' AND durationid=10 ";
            DataTable dt = m_Database.GetDataTable(strSQL);
            if (dt.Rows.Count > 0)
            {
                DateTime dTime = DateTime.Parse(dt.Rows[0][0].ToString());
                m_ToDate = dTime.ToString("yyyy年MM月dd日");
                m_FromDate = dTime.AddDays(-7).ToString("yyyy年MM月dd日");
            }

        }
    }
}
