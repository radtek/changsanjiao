using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using Readearth.Data;

public partial class ECCity : System.Web.UI.Page
{
    public string m_column;
    public string m_totalCount;
    public string m_width;
    public string id;
    public string period;
    public string T;
    public string parentText;
    public int count = 0;
    Database m_Database = new Database();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            m_column = Request["column"];
            m_totalCount = Request["totalCount"];
            m_width = Request["width"];
            try
            {
                T = Request["T"];
                parentText = Request["parentText"];
            }
            catch { }
            period = Request["period"];
            StringBuilder sb = new StringBuilder();


            string strSQL = " SELECT DM,MC FROM D_ECCity_Type ORDER BY DM";
            if (!string.IsNullOrEmpty(T)) {
                strSQL = " SELECT DM,MC FROM D_" + T + "_Type ORDER BY DM";
                if (parentText == "综合预报产品") {
                    strSQL = " SELECT DM,MC FROM D_" + T + "_Area ORDER BY DM";
                    count = 4;
                }
            }

            DataTable table = m_Database.GetDataTable(strSQL);
            if (table.Rows.Count > 0)
            {
                sb.Append("<ul>");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (i == count)
                        sb.AppendFormat("<li onclick=\"CityChange('{1}')\"><p class='foucs1' id='{1}'>{0}</p></li>", table.Rows[i][1], "L" + table.Rows[i][0].ToString());
                    else
                        sb.AppendFormat("<li onclick=\"CityChange('{1}')\"><p class='line' id='{1}'>{0}</p></li>", table.Rows[i][1], "L" + table.Rows[i][0].ToString());
                }
                sb.Append("</ul>");

                T = "L" + table.Rows[count][0].ToString();
            }
            moduleTypes.InnerHtml = sb.ToString();
            id = Request["id"];
        }

    }
}