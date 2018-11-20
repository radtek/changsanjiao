using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SingleProvince_GuidanceSingle : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie newCookie = new HttpCookie("User");
        newCookie.Values.Add("name", "JX");
        newCookie.Values.Add("indexUser", "JX");
        Response.Cookies.Add(newCookie);
    }
}