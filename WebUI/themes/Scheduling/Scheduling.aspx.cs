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
using System.Text;
using MMShareBLL.DAL;
using Readearth.Data;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using MMShareBLL.Model;
using MMShareBLL.BLL;

public partial class WorkGroup22 : System.Web.UI.Page
{
    public string m_ForecastDate;
    public string m_ForecastEndDate;
    public string m_FirstTab;
    public string m_UserJson;
    private Database m_Database;
    public bool m_UnLogin;
    SiteDal sd = new SiteDal();
    Area area = new Area();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                userNames.Value = Request.Cookies["User"]["name"].ToString();
            }
            catch { }
        }
    }

   
}
