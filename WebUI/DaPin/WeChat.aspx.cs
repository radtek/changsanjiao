using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services.Description;
using Readearth.Data;
using System.Web.Services;
using System.Data;
using System.Text;

public partial class HealthyWeather_WeChat : System.Web.UI.Page
{
    public static Database m_Database;
    protected void Page_Load(object sender, EventArgs e)
    {
        m_Database = new Database("DBCONFIGII");
    }
    private static string DataTableToJson1(string jsonName, System.Data.DataTable dt)
    {
        StringBuilder Json = new StringBuilder();
        Json.Append("{\"" + jsonName + "\":[");
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Json.Append("[");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Json.Append("\"" + dt.Rows[i][j].ToString() + "\"");
                    if (j < dt.Columns.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
                Json.Append("]");
                if (i < dt.Rows.Count - 1)
                {
                    Json.Append(",");
                }
            }
        }
        Json.Append("]}");
        return Json.ToString();
    }
    [WebMethod]
    public static string GetData() {
        WebReference.Publish webInterface = new WebReference.Publish();
        webInterface.Url = "http://222.66.83.21:8282/HeathWS/Publish.asmx";
        DataSet ds = new DataSet();
        ds = webInterface.GetCrows("shjkqxyb");
        DataTable dt = ds.Tables[0];
        return DataTableToJson1("json", dt);
    }
}