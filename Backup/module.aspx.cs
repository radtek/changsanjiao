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

public partial class module : System.Web.UI.Page
{
    public string m_FromDate;
    public string m_module;
    public string m_polluteStyle;
    protected void Page_Load(object sender, EventArgs e)
    {
            DateTime dtNow = DateTime.Now;
            m_FromDate = dtNow.ToString("yyyy年MM月dd日");
            m_module = Request["module"];
            m_polluteStyle = Request["polluteStyle"];

    }
}
