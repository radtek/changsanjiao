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

public partial class EvaluateHtml_Haze : System.Web.UI.Page
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
        int totalDays = DateTime.Parse(dateTime).Date.AddMonths(1).AddDays(-1).Day;
        string strSQL = string.Format("SELECT  LST,Score05, Score17, RtAllDay as RtAllDay05,RTDays as RtAllDay17, NoneDays05, NoneDays17, FailDay05, FailDay17, CorrectDays05, CorrectDays17 FROM T_HazeMoth WHERE LST='{0}'", dtFrom);
        DataTable hazeTable = m_Database.GetDataTable(strSQL);
        string[] name = { "05时（08-20）", "17时（20-20）" };
        string[] nameStr = { "05", "17" };
        string[] titleName = { "实况", "准确预报", "空报", "漏报", "预报评分" };
        string[] titleStr = { "RtAllDay", "CorrectDays", "NoneDays", "FailDay", "Score" };
        StringBuilder sb = new StringBuilder();
        Workbook workbook = new Workbook(); //工作簿 
        Worksheet sheet = workbook.Worksheets[0]; //工作表
        Cells cells = sheet.Cells;//单元格
        cells.Merge(0, 0, 1, 2);
        cells[0, 2].PutValue("无霾");
        cells[0, 3].PutValue("有霾");
        cells.Merge(1, 0, 5, 1);
        cells[1, 0].PutValue("05时（08-20）");
        cells.Merge(6, 0, 5, 1);
        cells[6, 0].PutValue("17时（20-20）");
        Aspose.Cells.Style styleTitle1 = workbook.Styles[workbook.Styles.Add()];//新增样式 
        styleTitle1.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
        for (int i = 0; i < name.Length; i++)
        {
            cells.Merge(i*5+1, 0, 5, 1);
            cells[i * 5 + 1, 0].PutValue(name[i]);
            cells[i * 5 + 1, 0].SetStyle(styleTitle1);
            for (int j = 0; j < titleName.Length; j++)
            {
                cells[i * 5 + 1 + j, 1].PutValue(titleName[j]);
                cells[i * 5 + 1 + j, 1].SetStyle(styleTitle1);
                if (hazeTable.Rows.Count > 0)
                {
                    
                    if (titleName[j] != "预报评分")
                    {
                        cells[i * 5 + 1 + j, 2].PutValue((totalDays - int.Parse(hazeTable.Rows[0][titleStr[j] + nameStr[i]].ToString())).ToString());
                        cells[i * 5 + 1 + j, 2].SetStyle(styleTitle1);
                        cells[i * 5 + 1 + j, 3].PutValue(hazeTable.Rows[0][titleStr[j] + nameStr[i]].ToString());
                        cells[i * 5 + 1 + j, 3].SetStyle(styleTitle1);
                    }
                    else
                    {
                        cells.Merge(i * 5 + 1 + j, 2, 1, 2);
                        cells[i * 5 + 1 + j, 2].PutValue(hazeTable.Rows[0][titleStr[j] + nameStr[i]].ToString());
                        cells[i * 5 + 1 + j, 2].SetStyle(styleTitle1);
                    }
                }
            }
        }

        string FileName = "霾评分" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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