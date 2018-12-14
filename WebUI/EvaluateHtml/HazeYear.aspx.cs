using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Readearth.Data;
using System.Data;
using System.Text;
using Aspose.Cells;

public partial class EvaluateHtml_HazeYear : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        H00.Value = DateTime.Now.ToString("yyyy年");

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Database m_Database = new Database();
        string dateTime = Element.Value;
        string dtTo = DateTime.Parse(dateTime).ToString("yyyy-12-01 23:00:00");
        string dtFrom = DateTime.Parse(dateTime).Date.AddYears(-1).ToString("yyyy-01-01 00:00:00");
        string strSQL = string.Format("SELECT LST, Score05, Score17, RtAllDay, RTDays, NoneDays05, NoneDays17, FailDay05, FailDay17, CorrectDays05, CorrectDays17 FROM  T_HazeMoth WHERE LST between '{0}' and '{1}' ORDER BY LST DESC", dtFrom, dtTo);
        DataTable hazeTable = m_Database.GetDataTable(strSQL);
        Workbook workbook = new Workbook(); //工作簿 
        Worksheet sheet = workbook.Worksheets[0]; //工作表
        Cells cells = sheet.Cells;//单元格
        Aspose.Cells.Style styleTitle1 = workbook.Styles[workbook.Styles.Add()];//新增样式 
        styleTitle1.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
        cells.Merge(0, 0, 2, 1);
        cells[0, 0].PutValue("日期");
        cells[0, 0].SetStyle(styleTitle1);
        cells.Merge(0, 1, 1, 2);
        cells[0, 1].PutValue("评分");
        cells[0, 1].SetStyle(styleTitle1);
        cells[0, 3].PutValue("霾日");
        cells[0, 3].SetStyle(styleTitle1);
        cells.Merge(0, 4, 1, 3);
        cells[0, 4].PutValue("05时");
        cells[0, 4].SetStyle(styleTitle1);
        cells.Merge(0, 7, 1, 3);
        cells[0, 7].PutValue("17时");
        cells[0, 7].SetStyle(styleTitle1);
        string[] titleName = { "报对","空报","漏报"};
        cells[1, 1].PutValue("05时");
        cells[1, 1].SetStyle(styleTitle1);
        cells[1, 2].PutValue("17时");
        cells[1, 2].SetStyle(styleTitle1);
        cells[1, 3].PutValue("实况");
        cells[1, 3].SetStyle(styleTitle1);
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < titleName.Length; j++)
            {
                cells[1, i * 3 + j + 4].PutValue(titleName[j]);
                cells[1, i * 3 + j + 4].SetStyle(styleTitle1);
            }
        }
        for (int i = 0; i < hazeTable.Rows.Count; i++)
        {
            cells[2 + i, 0].PutValue(DateTime.Parse(hazeTable.Rows[i][0].ToString()).ToString("M月"));
            cells[2 + i, 1].PutValue(hazeTable.Rows[i][1].ToString());
            cells[2 + i, 2].PutValue(hazeTable.Rows[i][2].ToString());
            cells[2 + i, 3].PutValue(int.Parse(hazeTable.Rows[i][3].ToString()) + int.Parse(hazeTable.Rows[i][4].ToString()));
            cells[2 + i, 4].PutValue(hazeTable.Rows[i]["CorrectDays05"].ToString());
            cells[2 + i, 5].PutValue(hazeTable.Rows[i]["NoneDays05"].ToString());
            cells[2 + i, 6].PutValue(hazeTable.Rows[i]["FailDay05"].ToString());
            cells[2 + i, 7].PutValue(hazeTable.Rows[i]["CorrectDays17"].ToString());
            cells[2 + i, 8].PutValue(hazeTable.Rows[i]["NoneDays17"].ToString());
            cells[2 + i, 9].PutValue(hazeTable.Rows[i]["FailDay17"].ToString());
            for (int j = 0; j < 10; j++)
            {
                cells[2 + i, j].SetStyle(styleTitle1);
            }
        }

        string FileName = "霾年评分" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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