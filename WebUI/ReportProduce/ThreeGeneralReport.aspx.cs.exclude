using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ReportProduce_ThreeGeneralReport : System.Web.UI.Page
{
    //m每天预报两次（05时，17时）
    //05时表示今天，17时表示明天
    public string strToday = "";
    //05时表示明天，17时表示后天
    public string strTomorrow = "";
    //05时表示后天，17时表示大后天
    public string strAfter = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime dtNow = DateTime.Now;

        if (dtNow.Hour > 12)
        {
            strToday = dtNow.AddDays(1).ToLongDateString();
            strTomorrow = dtNow.AddDays(2).ToLongDateString();
            strAfter = dtNow.AddDays(3).ToLongDateString();
        }
        else
        {
            strToday = dtNow.ToLongDateString();
            strTomorrow = dtNow.AddDays(1).ToLongDateString();
            strAfter = dtNow.AddDays(2).ToLongDateString();
        }
    }
}