using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMShareBLL.DAL;
using System.Text;
public partial class PublicAspx : System.Web.UI.Page
{
    public string m_json;
    public string id;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            m_json = Request["json"];
            StringBuilder sb = new StringBuilder();
            string[] types = m_json.Split('*');
            if (types.Length > 1)
            {
                m_json = types[0];
                sb.Append("<ul>");
                for (int i = 0; i < types.Length; i++)
                {
                    string[] info = types[i].Substring(1, types[i].Length - 2).Split(new char[] { ';', ':' });
                    if(i==0)
                        sb.AppendFormat("<li onclick=\"TypeChange('{2}','{1}')\"><p class='foucs' id='{1}'>{0}</p></li>", info[1], "L"+i.ToString(), types[i]);
                    else
                        sb.AppendFormat("<li onclick=\"TypeChange('{2}','{1}')\"><p class='line' id='{1}'>{0}</p></li>", info[1], "L" + i.ToString(), types[i]);
                }
                sb.Append("</ul>");
            }
            moduleTypes.InnerHtml = sb.ToString();
            id = Request["id"];
        }

    }
}