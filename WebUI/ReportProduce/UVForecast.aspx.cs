using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ReportProduce_UVForecast : System.Web.UI.Page
{
    //标记上传时间
    public string ForecastType = "10";
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime dtNow = DateTime.Now;
        if (dtNow.Hour < 16 && dtNow.Hour > 10)
        {
            ForecastType = "16";
        }
        else if (dtNow.Hour < 10)
        {
            ForecastType = "10";
        }
    }
}