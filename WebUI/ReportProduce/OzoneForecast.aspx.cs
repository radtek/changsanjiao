using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ReportProduce_OzoneForecast : System.Web.UI.Page
{
    public string strToday = "";    
    public string strTomorrow = "";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime dtNow = DateTime.Now;
        strToday = dtNow.ToString("MM月dd日");
        strTomorrow = dtNow.AddDays(1).ToString("MM月dd日");
       
    }
}