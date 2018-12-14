using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Readearth.Data;
using System.Data;
using System.Text;
using System.IO;

public partial class EvaluateHtml_WRFYear : System.Web.UI.Page
{
    public string m_module;
    protected void Page_Load(object sender, EventArgs e)
    {
        H00.Value = DateTime.Now.ToString("yyyy年");
        m_module = Request["module"];
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Database m_Database = new Database();
        string dateTime = Element.Value;
        string module = m_module;
        string fromTime = DateTime.Parse(dateTime).ToString("yyyy-01-01 00:00:00");
        string toTime = DateTime.Parse(dateTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-01 23:59:59");
        string strSQL = string.Format("SELECT CONVERT(VARCHAR(7),LST,120) as LST,  AVG(PM25), AVG(PM10), AVG(NO2), AVG(O31), AVG(O38),AVG(Score) FROM T_AirQualityEvaluate  WHERE LST between '{0}' and '{1}' and Module = '{2}' Group by CONVERT(VARCHAR(7),LST,120) ORDER BY LST ", fromTime, toTime, module);
        DataTable dt = m_Database.GetDataTable(strSQL);
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("日期,PM2.5,PM10,NO2,O3-1h,O3-8h,AQI");
        foreach (DataRow rows in dt.Rows)
        {
            string dataLine = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if(i==0)
                    dataLine = dataLine + DateTime.Parse(rows[0].ToString()).Month.ToString() + "月"  +",";
                else
                    dataLine = dataLine + Math.Round(double.Parse(rows[i].ToString()), 1) + ",";
            }
            sb.AppendLine(dataLine.Substring(0, dataLine.Length - 1));
        }
        StringWriter SW = new StringWriter(sb);
        SW.Close();
        string name = "";
        if (module == "WRF")
            name = "空气质量模式预报准确率逐月评分";
        else
            name = "空气质量主观预报准确率逐月评分";
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