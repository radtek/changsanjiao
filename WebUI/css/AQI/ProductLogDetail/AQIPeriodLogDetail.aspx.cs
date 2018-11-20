using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AQI_ProductLogDetail_AQIPeriodLogDetail : System.Web.UI.Page
{
    public string strTodayAQI = "0";
    public string strTodayItemID = "0";
    public string strTomorrowAQI = "0";
    public string strTomorrowItemID = "0";
    public string strAfterAQI = "0";
    public string strAfterItemID = "0";
    protected void Page_Load(object sender, EventArgs e)
    {
        strTodayAQI = Request["todayAQI"];
        strTodayItemID = Request["todayItemID"];
    }
}