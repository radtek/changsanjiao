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

public partial class AQI_weekTable : System.Web.UI.Page
{
    public string m_ForecastDate;
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime dtNow = DateTime.Now;

        H00.Value = dtNow.ToString("yyyy年MM月dd日");
        m_ForecastDate = dtNow.ToString("yyyy年MM月dd日");

    }
}
