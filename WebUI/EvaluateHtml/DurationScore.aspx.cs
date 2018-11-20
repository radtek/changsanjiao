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

public partial class EvaluateHtml_DurationScore : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        H00.Value = DateTime.Now.AddMonths(-1).Date.ToString("yyyy年MM月");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Database m_Database = new Database();
        string dateTime = Element.Value;
        string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
        string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
        string[] modle = { "WRF", "Manual", "ManualSubmit", "ManualCenter" };
        string[] modleName = { "WRF-chem", "气象部门", "环保部门", "两家合作" };
        string[] durationID = {"6","2","3" };
        string[] className = { "tableRowChild", "tableRowChild2" };
        string strSQL = string.Format("SELECT   f0, f1, f2, f3, f4, F,LST,DurationID,Module from T_DurationEvaluation  Where LST between '{0}' and '{1}' and Module<>'RT' order by LST;", fromTime, toTime);
        DataTable dt = m_Database.GetDataTable(strSQL);
        DataTable timeTable = dt.DefaultView.ToTable(true, "LST");
        string filter = "";
        DataRow[] dataRows;

        Workbook workbook = new Workbook(); //工作簿 
        Worksheet sheet1 = workbook.Worksheets[0]; //工作表 
        Aspose.Cells.Style styleTitle1 = workbook.Styles[workbook.Styles.Add()];//新增样式 
        styleTitle1.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
        sheet1.Name = "分时段日评分";
        Cells cells = sheet1.Cells;//单元格
        string[] titleName = {"污染附加分(f0)","首要污染物正确性评分(f1)","级别准确性评分（f2）","首要污染物iAQI精度正确性评分（f3）","其他污染物iAQI精度正确性评分（f4）","综合评分（F）" };
        cells.Merge(0, 2, 1, 6);
        cells[0, 2].SetStyle(styleTitle1);
        cells[0, 2].PutValue("夜间");
        cells.Merge(0, 8, 1, 6);
        cells[0, 8].SetStyle(styleTitle1);
        cells[0, 8].PutValue("上午");
        cells.Merge(0, 14, 1, 6);
        cells[0, 14].SetStyle(styleTitle1);
        cells[0, 14].PutValue("下午");
        cells[1,0].PutValue("日期");
        for (int i = 0; i < durationID.Length; i++)
        {
            for (int j = 0; j < titleName.Length; j++)
            {
                cells[1, j +(i*titleName.Length)+2].PutValue(titleName[j]);
            }
        }
        for (int k = 0; k < timeTable.Rows.Count; k++)
        {
            for (int i = 0; i < modle.Length; i++)
            {
                if (i == 0)
                {
                    cells.Merge(4*k+2, 0, 4, 1);
                    cells[4 * k + 2, 0].PutValue(DateTime.Parse(timeTable.Rows[k][0].ToString()).ToString("MM月dd日"));

                }

                cells[4 * k + 2+i, 1].PutValue(modleName[i]);
                for (int j = 0; j < durationID.Length; j++)
                {

                    filter = string.Format("Module='{0}' and LST='{1}' and DurationID='{2}'", modle[i], timeTable.Rows[k][0].ToString(), durationID[j]);
                    dataRows = dt.Select(filter);
                    for (int m = 0; m < titleName.Length; m++)
                    {
                        if (dataRows.Length > 0)
                            cells[4 * k + 2 + i, m + (j * titleName.Length) + 2].PutValue(dataRows[0][m]);
                        else
                            cells[4 * k + 2 + i, m + (j * titleName.Length) + 2].PutValue("");
                    }
                }
            }
        }
        string FileName = "分时段日评分" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
        string UserAgent = Request.ServerVariables["http_user_agent"].ToLower();
        if (UserAgent.IndexOf("firefox") == -1)
            FileName = HttpUtility.UrlEncode(FileName, Encoding.UTF8);

        Response.ContentType = "application/ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        Response.BinaryWrite(workbook.SaveToStream().ToArray());
        Response.Flush();
        Response.End();

    }
}