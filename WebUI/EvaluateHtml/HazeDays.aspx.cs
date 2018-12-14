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

public partial class EvaluateHtml_HazeDays : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        H00.Value = DateTime.Now.AddMonths(-1).Date.ToString("yyyy年MM月");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Database m_Database = new Database();
        string dateTime = Element.Value;
        string dtFrom = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
        string dtTo = DateTime.Parse(dateTime).Date.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
        string strSQL = string.Format("SELECT LST,Forecast05, Forecast17, ForecastGrade05, ForecastGrade17, RtAllDay, RtDay, Score05, Score17, NoneDays05, NoneDays17, FailDay05,FailDay17, CorrectDays05, CorrectDays17 FROM T_HazeEvaluate WHERE LST BETWEEN '{0}' and '{1}' ORDER by LST", dtFrom, dtTo);
        DataTable hazeTable = m_Database.GetDataTable(strSQL);
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("日期,05时霾预报,17时霾预报,预报05时,预报17时,夜+白实况,白实况,05时得分,17时得分,05时空报,17时空报,05时漏报,17时漏报");
        foreach (DataRow rows in hazeTable.Rows)
        {
            sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}", DateTime.Parse(rows[0].ToString()).ToString("yyyy月MM日dd日"), rows["Forecast05"], rows["Forecast17"], rows["ForecastGrade05"], rows["ForecastGrade17"], rows["RtAllDay"], rows["RtDay"], rows["Score05"], rows["Score17"], rows["NoneDays05"], rows["NoneDays17"], rows["FailDay05"], rows["FailDay17"]));
        }
        StringWriter SW = new StringWriter(sb);
        SW.Close();
        string saveAsFileName = string.Format("{0}.csv", "逐日霾评分");
        Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(saveAsFileName));
        Response.ContentType = "application/ms-excel";
        Response.Charset = "GB2312";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.Write(SW);
        Response.Flush();
        Response.End();
    }
}