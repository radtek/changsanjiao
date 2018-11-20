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
using Aspose.Cells;
using System.Web.Services;

public partial class EvaluateHtml_DurationScore : System.Web.UI.Page
{
    private static Database m_Database;
    protected void Page_Load(object sender, EventArgs e)
    {
        H00.Value = DateTime.Now.AddMonths(-1).Date.ToString("yyyy年MM月");
        m_Database = new Database();
    }



    #region Click事件
    protected void Button1_Click(object sender, EventArgs e)
    {
        Database m_Database = new Database();
        string dateTime = Element.Value;
        string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
        string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
        string[] modle = { "Manual" };
        string[] modleName = { "气象部门" };
        string[] durationID = { "6", "2", "3" };
        string[] className = { "tableRowChild", "tableRowChild2" };
        string strSQL = string.Format("SELECT LST, UserID,DurationID, f1,f2,f_PM25,f_O3,f_PM10,f_NO2,f3,f4,f0,F from T_DayDurScore  Where LST between '{0}' and '{1}' order by LST;", fromTime, toTime);
        DataTable dt = m_Database.GetDataTable(strSQL);
        DataTable timeTable = dt.DefaultView.ToTable(true, "LST");
        string filter = "";
        DataRow[] dataRows;

        Workbook workbook = new Workbook(); //工作簿 
        Worksheet sheet1 = workbook.Worksheets[0]; //工作表 
        Aspose.Cells.Style styleTitle1 = workbook.Styles[workbook.Styles.Add()];//新增样式 
        styleTitle1.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
        sheet1.Name = "分时段个人日评分";
        Cells cells = sheet1.Cells;//单元格
        string[] titleName = { "级别评分f1", "首要污染物评分", "精度评分PM2.5", "精度评分PM10", "精度评分NO2", "首要污染物精度评分", "其他指标的iAQI精度评分", "污染物附加分", "总分" };
        string[] titleName2 = { "级别评分f1", "首要污染物评分", "精度评分PM2.5", "精度评分O3", "精度评分PM10", "精度评分NO2", "首要污染物精度评分", "其他指标的iAQI精度评分", "污染物附加分", "总分" };

        cells.Merge(0, 2, 1, 9);
        cells[0, 2].SetStyle(styleTitle1);
        cells[0, 2].PutValue("夜间");
        cells.Merge(0, 11, 1, 9);
        cells[0, 11].SetStyle(styleTitle1);
        cells[0, 11].PutValue("上午");
        cells.Merge(0, 20, 1, 10);
        cells[0, 20].SetStyle(styleTitle1);
        cells[0, 20].PutValue("下午");
        cells[1, 0].PutValue("日期");
        cells[1, 1].PutValue("预报员");
        for (int i = 0; i < durationID.Length; i++)
        {
            if (durationID[i] != "3")
            {
                for (int j = 0; j < titleName.Length; j++)
                {
                    cells[1, j + (i * titleName.Length) + 2].PutValue(titleName[j]);
                }
            }
            else
            {
                for (int jj = 0; jj < titleName2.Length; jj++)
                {
                    cells[1, jj + (i * titleName.Length) + 2].PutValue(titleName2[jj]);
                }
            }
        }
        int line = 2;
        for (DateTime startTime = Convert.ToDateTime(fromTime); startTime < Convert.ToDateTime(toTime); startTime = startTime.AddDays(1))
        {
            for (int k = 0; k < durationID.Length; k++)
            {
                filter = string.Format("LST='{0}' and DurationID='{1}'", startTime, durationID[k]);
                dataRows = dt.Select(filter, "LST");
                if (dataRows != null && dataRows.Length > 0)
                {
                    DataRow dr = dataRows[0];
                    if (durationID[k] != "3")
                    {
                        if (durationID[k] == "6")
                        {
                            cells[line, 0].PutValue(Convert.ToDateTime(dataRows[0]["LST"]).ToString("MM月dd日"));
                            cells[line, 1].PutValue(dataRows[0]["UserID"]);
                        }
                        int tt = 0;
                        for (int t = 0; t < titleName.Length + 1; t++)
                        {
                            if (t == 3)
                            {
                                continue;
                            }
                            cells[line, k * titleName.Length + 2 + tt].PutValue(dr[t + 3]);
                            tt++;
                        }
                    }
                    else
                    {
                        for (int ttt = 0; ttt < titleName2.Length; ttt++)
                        {
                            cells[line, k * titleName.Length + 2 + ttt].PutValue(dr[ttt + 3]);
                        }
                    }
                }
            }
            line++;
        }
        string FileName = "分时段个人日评分" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
        string UserAgent = Request.ServerVariables["http_user_agent"].ToLower();
        if (UserAgent.IndexOf("firefox") == -1)
            FileName = HttpUtility.UrlEncode(FileName, Encoding.UTF8);
        Response.ContentType = "application/ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        Response.BinaryWrite(workbook.SaveToStream().ToArray());
        Response.Flush();
        Response.End();

    }
    #endregion



    [WebMethod]
    public static string ReturnOriginData(string dateTime, string type)
    {
        //ID包含RTData+ForeData

        if (type == "RTData")
        {
            string table = "select * from T_shiTable";
        }
        else
        {

        }


        return "";
    }


    [WebMethod]
    public static string ReturnDurationScore(string dateTime)
    {
        string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
        int totalDays = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).Day;
        string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
        string[] modle = { "Manual" };
        string[] modleName = { "气象部门" };
        string[] durationID = { "6", "2", "3" };
        string[] className = { "tableRowChild", "tableRowChild", "tableRowChild", "tableRowChild2" };
        string strSQL = string.Format("SELECT LST,DurationID,Module, f0, f1, f2, f3, f4, F from T_DurationEvaluation  Where LST between '{0}' and '{1}' and Module<>'RT' order by LST;", fromTime, toTime);
        DataTable dt = m_Database.GetDataTable(strSQL);
        DataTable timeTable = dt.DefaultView.ToTable(true, "LST");
        string filter = "";
        DataRow[] dataRows;
        int[] day = new int[1];
        //int duration = 0;
        //int[] durationDay = new int[2];
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < modle.Length; i++)
        {
            filter = string.Format("Module='{0}'", modle[i]);
            dataRows = dt.Select(filter);
            day[i] = dataRows.Length;
            //for (int j = 0; j < durationID.Length; j++)
            //{
            //    filter = string.Format("Module='{0}' and DurationID='{2}'", modle[i],durationID[j]);
            //    dataRows = dt.Select(filter);
            //    duration = duration + (totalDays - dataRows.Length);

            //}
            //durationDay[i] = duration;
        }
        sb.Append("<table   width='100%' border='0' cellpadding='0' cellspacing='0'>");
        sb.Append("<tr>");
        sb.Append("<td class='tabletitleDurationScore'></td>");
        sb.Append("<td class='tabletitleDurationScore'></td>");
        sb.Append("<td class='tabletitleDurationScore' colspan='6'>夜间</td>");
        sb.Append("<td class='tabletitleDurationScore' colspan='6'>上午</td>");
        sb.Append("<td class='tabletitleDurationScore' colspan='6'>下午</td>");
        sb.Append("</tr>");
        sb.Append("<tr>");
        sb.Append("<td class='tabletitleScore'>日期</td>");
        sb.Append("<td class='tabletitleScore'></td>");
        for (int i = 0; i < durationID.Length; i++)
        {
            sb.Append("<td class='tabletitleScore'>污染</br>附加分</br>(f0)</td>");
            sb.Append("<td class='tabletitleScore'>首要污染</br>物正确</br>性评分</br>(f1)</td>");
            sb.Append("<td class='tabletitleScore'>级别准</br>确性评分</br>（f2）</td>");
            sb.Append("<td class='tabletitleScore'>首要污染</br>物iAQI精度</br>正确性评分</br>（f3）</td>");
            sb.Append("<td class='tabletitleScore'>其他污染物</br>iAQI精度</br>正确性评分</br>（f4）</td>");
            sb.Append("<td class='tabletitleScore1'>综合评分</br>（F）</td>");
        }
        sb.Append("</tr>");
        foreach (DataRow rows in timeTable.Rows)
        {
            for (int i = 0; i < modle.Length; i++)
            {
                sb.Append("<tr>");
                if (i == 0)
                    sb.Append("<td class='tableRowChild2' rowspan='1'>" + DateTime.Parse(rows[0].ToString()).ToString("MM月dd日") + "</td>");
                sb.Append("<td class='" + className[i] + "'>" + modleName[i] + "</td>");
                for (int j = 0; j < durationID.Length; j++)
                {
                    filter = string.Format("Module='{0}' and LST='{1}' and DurationID='{2}'", modle[i], rows[0].ToString(), durationID[j]);
                    dataRows = dt.Select(filter);

                    if (dataRows.Length > 0)
                    {
                        sb.Append("<td class='" + className[i] + "'>" + dataRows[0][3] + "</td>");
                        sb.Append("<td class='" + className[i] + "'>" + dataRows[0][4] + "</td>");
                        sb.Append("<td class='" + className[i] + "'>" + dataRows[0][5] + "</td>");
                        sb.Append("<td class='" + className[i] + "'>" + dataRows[0][6] + "</td>");
                        sb.Append("<td class='" + className[i] + "'>" + dataRows[0][7] + "</td>");
                        if (i == 0)
                            sb.Append("<td class='tableRowChildright'>" + dataRows[0][8] + "</td>");
                        else
                            sb.Append("<td class='tableRowChild2right'>" + dataRows[0][8] + "</td>");
                    }
                    else
                    {
                        sb.Append("<td class='" + className[i] + "'></td>");
                        sb.Append("<td class='" + className[i] + "'></td>");
                        sb.Append("<td class='" + className[i] + "'></td>");
                        sb.Append("<td class='" + className[i] + "'></td>");
                        sb.Append("<td class='" + className[i] + "'></td>");
                        sb.Append("<td class='" + className[i] + "'></td>");
                    }
                }
                sb.Append("</tr>");
            }
        }
        int k = 0;
        string tempStr = "";
        for (int i = 0; i < day.Length; i++)
        {
            if (day[i] - totalDays < 0)
            {

                sb.Append("<tr>");
                if (k == 0)
                {
                    if (modleName[i] == "WRF-chem")
                        tempStr = "注：WRF-chem模式预报数据缺" + (totalDays - day[i]).ToString() + "日";
                    else
                        tempStr = "注：" + modleName[i] + "数据缺" + (totalDays - day[i]).ToString() + "日";
                }
                else
                    tempStr = "<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>" + modleName[i] + "数据缺" + (totalDays - day[i]).ToString() + "日";
                k++;
                sb.Append("<td class='tableRowOther' colspan='20'>" + tempStr + "</td>");
                sb.Append("</tr>");
            }
        }
        sb.Append("</table>");
        return sb.ToString();
    }
}