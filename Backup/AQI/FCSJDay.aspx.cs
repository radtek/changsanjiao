using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Readearth.Data;
using System.Text;
using System.IO;

public partial class AQI_FCSJDay : System.Web.UI.Page
{
    public bool m_UnLogin;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["SYSTEMUSERID"] == null)
        //{
        //    m_UnLogin = true;
        //    return;
        //}
        DateTime dtNow = DateTime.Now.AddHours(-1);

        H00.Value = dtNow.ToString("yyyy年MM月dd日");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string Content = Element.Value;
        DateTime dtFrom = DateTime.Parse(Content);
        DateTime dtTo = DateTime.Parse(Content).AddHours(23);
        string strSQL = string.Format("SELECT  China_RT_CNEMC_Data.area as Area,Avg(AQI) as AQI,Avg(pm2_5) as [PM2.5],Avg(pm10) as PM10,AVG(co) as CO,Avg(no2) as NO2,Avg(so2) as SO2,MAX(o31) as O31小时,MAX(o38) as O38小时 FROM huadongarea left  join China_RT_CNEMC_Data on huadongarea.area=China_RT_CNEMC_Data.area where TimePoint between '{0}' and '{1}' and ischang in(1,2) group by China_RT_CNEMC_Data.area,stateid", dtFrom, dtTo);
        Database m_Database = new Database("SEMCDMC");

        DataTable dtSiteData = m_Database.GetDataTable(strSQL);

        string name = "泛长三角日报";

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("地点名称,AQI,PM2.5,PM10,CO,NO2,SO2,O3_1h,O3_8h");
        foreach (DataRow dr in dtSiteData.Rows)
        {
            string dataLine = "";
            string value;
            for (int i = 0; i < dtSiteData.Columns.Count; i++)
            {
                if (i == 0)
                    value = dr[i].ToString();
                else
                    value = Math.Round(Double.Parse(dr[i].ToString()), 1).ToString();
                dataLine = dataLine + value + ",";
            }
            sb.AppendLine(dataLine.Substring(0, dataLine.Length - 1));
        }
        StringWriter SW = new StringWriter(sb);
        SW.Close();
        string saveAsFileName = string.Format("{0}.csv", name);
        Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(saveAsFileName));
        Response.ContentType = "application/ms-excel";
        Response.Charset = "GB2312";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.Write(SW);
        Response.Flush();
        Response.End();
    }
}
