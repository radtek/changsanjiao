using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Readearth.Data;
using System.Data;
using System.Text;

public partial class ImgList_ModuleFree : System.Web.UI.Page
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
            if (id == "ModuleTest")
            {
                string strSQL = "SELECT DM,MC FROM D_ModuleF_Type ";
                DataTable dt = m_Database.GetDataTable(strSQL);
                if (dt.Rows.Count > 0)
                {
                    for(int i=0;i<dt.Rows.Count;i++)
                    {
                        if (i == 0)
                            sb.AppendFormat("<option value='{0}' selected='selected'>{1}</option>", dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString());
                        else
                            sb.AppendFormat("<option value='{0}' >{1}</option>", dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString());                 
                    }
                }
            }
            else 
            {
                sb.AppendFormat("<option value='{0}' selected='selected'>{1}</option>", "D_NMC_Type", "全国NMC");
                sb.AppendFormat("<option value='{0}' >{1}</option>", "D_LQHo_Type", "长三角中心");
     
            }
            sb.AppendLine("</select>");
            areaSelect.InnerHtml = sb.ToString();
        } 

    }
}