using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Readearth.Data;
using System.Text;

public partial class ImgList_DiagnosticAnalysis : System.Web.UI.Page
{
    private Database m_Database;
    public string id;
    public string m_select;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            m_Database = new Database();
            id = Request["id"];
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" <label  class='cur-select'></label>");
            sb.AppendLine("<select id='area'>");

            sb.AppendFormat("<option value='{0}' selected='selected'>{1}</option>", "D_ECThin_Layers", "EC高分辨率");
            sb.AppendFormat("<option value='{0}' >{1}</option>", "D_RegionalAnal_Layers", "区域模式分析");

            sb.AppendLine("</select>");
            areaSelect.InnerHtml = sb.ToString();
        }

    }
}