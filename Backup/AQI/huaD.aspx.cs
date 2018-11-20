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

public partial class AQI_huaD : System.Web.UI.Page
{
    public bool m_UnLogin;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["SYSTEMUSERID"] == null)
        //{
        //    m_UnLogin = true;
        //    return;
        //}
        DateTime dtNow = DateTime.Now.AddHours(-2);

        H00.Value = dtNow.ToString("yyyy年MM月dd日 HH时");

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string Content = Element.Value;
        DateTime dtFrom = DateTime.Parse(Content);
        string strSQL = string.Format("SELECT  huadongarea.area,PositionName,AQI,PM2_5,PM10,CO,NO2,SO2,O31,O38 FROM huadongarea left JOIN China_RT_CNEMC_Data on huadongarea.area=China_RT_CNEMC_Data.area WHERE TimePoint='{0}'", dtFrom);
        Database m_Database = new Database("SEMCDMC");

        DataTable dtSiteData = m_Database.GetDataTable(strSQL);

        string name = "华东小时监测数据";

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("地点名称,位置点,AQI,PM2.5,PM10,CO,NO2,SO2,O3_1h,O3_8h");
        foreach (DataRow dr in dtSiteData.Rows)
        {
            string dataLine = "";
            for (int i = 0; i < dtSiteData.Columns.Count; i++)
            {
                dataLine = dataLine + dr[i] + ",";
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
