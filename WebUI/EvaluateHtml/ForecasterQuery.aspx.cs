using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EvaluateHtml_ForecasterQuery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        H01.Value = DateTime.Now.Date.ToString("yyyy年MM月dd");
        H00.Value = DateTime.Now.AddDays(-3).ToString("yyyy年MM月dd");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

    }
}