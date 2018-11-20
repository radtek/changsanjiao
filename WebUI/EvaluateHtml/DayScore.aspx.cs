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


public partial class EvaluateHtml_DayScore : System.Web.UI.Page
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
        string strSQL = string.Format("SELECT LST, UserID, f1,f2,f_PM25,f_O3,f_PM10,f_NO2,f3,f4,f0,F,FF from T_DayScore  Where LST between '{0}' and '{1}' order by LST;", fromTime, toTime);
        DataTable dt = m_Database.GetDataTable(strSQL);
        DataTable timeTable = dt.DefaultView.ToTable(true, "LST");
        string filter = "";
        DataRow[] dataRows;

        Workbook workbook = new Workbook(); //工作簿 
        Worksheet sheet1 = workbook.Worksheets[0]; //工作表 
        Aspose.Cells.Style styleTitle1 = workbook.Styles[workbook.Styles.Add()];//新增样式 
        styleTitle1.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
        sheet1.Name = "分时段总评分";
        Cells cells = sheet1.Cells;//单元格
        string[] titleName = { "级别评分f1", "首要污染物评分", "精度评分PM2.5", "精度评分O3", "精度评分PM10", "精度评分NO2", "首要污染物精度评分", "其他指标的iAQI精度评分", "污染物附加分", "日评分F" };

        cells.Merge(0, 0, 1, 12);
        cells[0, 0].SetStyle(styleTitle1);
        cells[0, 0].PutValue("日评分");
        cells.Merge(0, 12, 2, 1);
        cells[0, 12].SetStyle(styleTitle1);
        cells[0, 12].PutValue("总评分");
        cells[1, 0].PutValue("日期");
        cells[1, 1].PutValue("预报员");

        for (int j = 0; j < titleName.Length; j++)
        {
            cells[1, j + 2].PutValue(titleName[j]);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            for (int k = 0; k < dt.Columns.Count; k++)
            {
                if (k == 0)
                {
                    cells[i + 2, k].PutValue(Convert.ToDateTime(dt.Rows[i][k]).ToString("MM月dd日"));
                    continue;
                }
                cells[i + 2, k].PutValue(dt.Rows[i][k]);
            }
        }
        string FileName = "分时段总评分" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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
}