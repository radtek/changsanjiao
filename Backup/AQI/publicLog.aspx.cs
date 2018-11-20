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

public partial class AQI_publicLog : System.Web.UI.Page
{
    public string m_FromDate;
    public string m_ToDate;
    public bool m_UnLogin;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["SYSTEMUSERID"] == null)
        //{
        //    m_UnLogin = true;
        //    return;
        //}
        if (!Page.IsPostBack)
        {
            DateTime dtNow = DateTime.Now;
            m_FromDate = dtNow.AddDays(-2).ToString("yyyy年MM月dd日");
            m_ToDate = dtNow.ToString("yyyy年MM月dd日");
        }

    }
}
