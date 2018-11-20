using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AQI_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DateTime now = DateTime.Now;
            H00.InnerHtml = now.AddDays(-1).ToString("yyyy-MM-dd");
            H01.InnerHtml = now.AddDays(-2).ToString("yyyy-MM-dd");
            H02.InnerHtml = now.AddDays(-3).ToString("yyyy-MM-dd");
            H03.InnerHtml = now.AddDays(-4).ToString("yyyy-MM-dd");
            H04.InnerHtml = now.AddDays(-5).ToString("yyyy-MM-dd");
            H05.InnerHtml = now.AddDays(-6).ToString("yyyy-MM-dd");
            H06.Value = now.ToString("yyyy-MM-dd");
        }

    }
}