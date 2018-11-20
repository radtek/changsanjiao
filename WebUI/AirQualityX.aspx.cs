using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;
using Readearth.Data;
using System.Data;

public partial class AirQualityX : System.Web.UI.Page
{
    public string m_json;
    public string id;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            m_json = Request["json"];
            id = Request["id"];

            if (id.Split(',').Length > 1)
            {
                m_json = Request["json1"];     
            }
            StringBuilder sb = new StringBuilder();
            string[] types = m_json.Split('*');
            if (types.Length > 0)
            {
                m_json = types[0];
                sb.Append("<ul>");
                for (int i = 0; i < types.Length; i++)
                {
                    string[] info = types[i].Substring(1, types[i].Length - 2).Split(new char[] { ';', ':' });
                    if (i == 0)
                    {
                        //if (china.IndexOf(id) >= 0)
                        //    sb.AppendFormat("<li onclick=\"CityChange('{2}','{1}')\"><p class='foucs' id='{1}'>{0}</p></li>", info[1], "L" + i.ToString(), types[i]);
                        //else
                            sb.AppendFormat("<li onclick=\"CityChange('{2}','{1}')\"><p class='foucs' id='{1}'>{0}</p></li>", info[1], "L" + i.ToString(), types[i]);
                    }
                    else
                    {
                        //if (east.IndexOf(id) >= 0)
                        //    sb.AppendFormat("<li ><p class='line' id='{1}'>{0}</p></li>", info[1], "L" + i.ToString());
                        //else 
                            sb.AppendFormat("<li onclick=\"CityChange('{2}','{1}')\"><p class='line' id='{1}'>{0}</p></li>", info[1], "L" + i.ToString(), types[i]);
                    }
                }
                sb.Append("</ul>");
            }

            
            moduleTypes.InnerHtml = sb.ToString();

            Database db = new Database();
            string strSQL = "SELECT max(ForecastDate) AS 'ForecastDate' FROM T_ModelTest WHERE Layers='12' AND Type='16'";
            DataTable dt = db.GetDataTable(strSQL);
            this.H00.Value = DateTime.Parse(dt.Rows[0][0].ToString()).ToString("yyyy-MM-dd");
            this.H00.Attributes.Add("tag", "2");
            this.H00.Attributes.Add("todayDateTime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

    }
}