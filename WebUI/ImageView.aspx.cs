using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ImageView : System.Web.UI.Page
{
    public string m_id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        m_id = Request["id"];
        id.InnerHtml = m_id;
    }
}