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

public partial class JhForecast_Default : System.Web.UI.Page
{
    public string m_DateStart;

    protected void Page_Load(object sender, EventArgs e)
    {
        m_DateStart = "2013-06-18";
    }
}
