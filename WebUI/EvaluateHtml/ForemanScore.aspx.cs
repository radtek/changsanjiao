using System;
using Readearth.Data;
using System.Data;
using System.Text;
using System.IO;

public partial class EvaluateHtml_ForemanDayForecast : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        H00.Value = DateTime.Now.Date.ToString("yyyy年MM月");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Database m_Database = new Database();
        string dateTime = Element.Value;
        string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
        int totalDays = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).Day;
        string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
        //string[] modle = { "Manual", "WRF" };
        //string[] modleName = { "主观预报", "WRF-chem" };
        string strSQL = string.Format("SELECT  LST,ID, F1, F2, F3, S from  T_ForemanScore  Where LST between '{0}' and '{1}' order by LST;", fromTime, toTime);
        DataTable dt = m_Database.GetDataTable(strSQL);
        DataTable timeTable = dt.DefaultView.ToTable(true, "LST");
        string filter = "";
        string lineStr = "";
        DataRow[] dataRows;
        int day = 0;
        //for (int i = 0; i < modle.Length; i++)
        //{
        //    filter = string.Format("Module='{0}'", modle[i]);
        //    dataRows = dt.Select(filter);
        //    day[i] = dataRows.Length;
        //}
        dataRows = dt.Select();
        day = dataRows.Length;
        StringBuilder sb = new StringBuilder();
        lineStr = "{0},{1},{2},{3},{4},{5}";
        sb.AppendLine("日期,ID,首要污染物正确性评分(f1),AQI预报级别正确性评分（f2）,AQI预报数值误差评分（f3）,空气质量预报精确度评分（S）");
        foreach (DataRow rows in timeTable.Rows)
        {
            filter = string.Format("LST='{0}'", rows[0].ToString());
            dataRows = dt.Select(filter);
            if (dataRows.Length > 0)
            {
                sb.AppendLine(string.Format(lineStr, DateTime.Parse(rows[0].ToString()).ToString("MM月dd日"), dataRows[0][1], dataRows[0][2], dataRows[0][3], dataRows[0][4], dataRows[0][5]));

            }
            else
            {
                sb.AppendLine(string.Format(lineStr, DateTime.Parse(rows[0].ToString()).ToString("MM月dd日"), "", "", "", "", "", ""));
            }
        }

        string tempStr = "";

        if (day - totalDays < 0)
        {
            tempStr = "  注： " + "数据缺" + (totalDays - day).ToString() + "日";
            sb.AppendLine(tempStr);
        }

        StringWriter SW = new StringWriter(sb);
        SW.Close();
        string saveAsFileName = string.Format("{0}.csv", "领班预报评分");
        Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(saveAsFileName));
        Response.ContentType = "application/ms-excel";
        Response.Charset = "GB2312";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.Write(SW);
        Response.Flush();
        Response.End();

    }
}