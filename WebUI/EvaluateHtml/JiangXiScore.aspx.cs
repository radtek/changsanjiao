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
using MMShareBLL.DAL;

public partial class EvaluateHtml_JiangXiScore : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        H00.Value = DateTime.Now.AddMonths(-1).Date.ToString("yyyy年MM月");
        HttpCookie newCookie = new HttpCookie("User");
        newCookie.Values.Add("name", "JX");
        newCookie.Values.Add("indexUser", "JX");
        Response.Cookies.Add(newCookie);
        EvalutionCaculateJX ecJX = new EvalutionCaculateJX();
        ecJX.InsertEvaluationTable_JX(DateTime.Now.AddMonths(-1).Date.ToString("yyyy年MM月"));
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Database m_Database = new Database();
        string dateTime = Element.Value;
        string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
        int totalDays = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).Day;
        string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
        string[] modle = { "Manual", "WRF" };
        string[] modleName = { "主观预报", "WRF-chem" };
        string strSQL = string.Format("SELECT  LST,Module, f1, f2, f3, S from  T_JXChinaEvaluation  Where LST between '{0}' and '{1}' and Module<>'RT' order by LST;", fromTime, toTime);
        DataTable dt = m_Database.GetDataTable(strSQL);

        DataTable timeTable = dt.DefaultView.ToTable(true, "LST");
        string filter = "";
        string lineStr = "";
        DataRow[] dataRows;
        int[] day = new int[2];
        for (int i = 0; i < modle.Length; i++)
        {
            filter = string.Format("Module='{0}'", modle[i]);
            dataRows = dt.Select(filter);
            day[i] = dataRows.Length;
        }
        StringBuilder sb = new StringBuilder();
        lineStr = "{0},{1},{2},{3},{4},{5}";
        sb.AppendLine("日期,,首要污染物正确性评分(f1),AQI预报级别正确性评分（f2）,AQI预报数值误差评分（f3）,空气质量预报精确度评分（S）");
        foreach (DataRow rows in timeTable.Rows)
        {
            for (int i = 0; i < modle.Length; i++)
            {
                filter = string.Format("Module='{0}' and LST='{1}'", modle[i], rows[0].ToString());
                dataRows = dt.Select(filter);
                if (dataRows.Length > 0)
                {
                    sb.AppendLine(string.Format(lineStr, DateTime.Parse(rows[0].ToString()).ToString("MM月dd日"), modleName[i], dataRows[0][2], dataRows[0][3], dataRows[0][4], dataRows[0][5]));

                }
                else
                {
                    sb.AppendLine(string.Format(lineStr, DateTime.Parse(rows[0].ToString()).ToString("MM月dd日"), modleName[i], "", "", "", ""));
                }
            }
        }
        int k = 0;
        string tempStr = "";
        for (int i = 0; i < day.Length; i++)
        {
            if (day[i] - totalDays < 0)
            {

                if (k == 0)
                {
                    if (modleName[i] == "WRF-chem")
                        tempStr = "注：WRF-chem模式预报数据缺" + (totalDays - day[i]).ToString() + "日";
                    else
                        tempStr = "注：" + modleName[i] + "数据缺" + (totalDays - day[i]).ToString() + "日";
                }
                else
                    tempStr = "   " + modleName[i] + "数据缺" + (totalDays - day[i]).ToString() + "日";
                k++;
                sb.AppendLine(tempStr);
            }
        }
        StringWriter SW = new StringWriter(sb);
        SW.Close();
        string saveAsFileName = string.Format("{0}.csv", "国家局日评分");
        Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(saveAsFileName));
        Response.ContentType = "application/ms-excel";
        Response.Charset = "GB2312";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.Write(SW);
        Response.Flush();
        Response.End();
    }
}