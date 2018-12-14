using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EvaluateHtml_PersonYear : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        H00.Value = DateTime.Now.ToString("yyyy年");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

    }
}