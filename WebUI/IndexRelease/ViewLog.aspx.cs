using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Readearth.Data;

public partial class IndexRelease_ViewLog : System.Web.UI.Page
{
    public static Database m_Database;
    protected void Page_Load(object sender, EventArgs e)
    {
        m_Database = new Database("DBCONFIG");
    }

    [WebMethod]
    public static DataTable GetPeople() {
        string sql = "select Alias from T_User";
        DataTable dt = m_Database.GetDataTable(sql);
        return dt;
    }
}