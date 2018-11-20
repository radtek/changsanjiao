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

public partial class PerDayComment : System.Web.UI.Page
{
    public string userName;
    public string LoginName = "";
    private Database m_Database;
    protected void Page_Load(object sender, EventArgs e)
    {
        userName = Request.Cookies["User"]["name"];
        DateTime dtNow = DateTime.Now;

        H00.Value = dtNow.ToString("yyyy年MM月dd日");
        if (!Page.IsPostBack)
        {
            m_Database = new Database();
            string strSQL = "SELECT Alias FROM T_User WHERE UserName='" + userName + "'";
            DataTable dt = m_Database.GetDataTable(strSQL);
            if (dt.Rows.Count > 0)
                LoginName = m_Database.GetFirstValue(strSQL);
        }
    }
}
