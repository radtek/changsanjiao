using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Readearth.Data;
using System.Data;
using Aspose.Cells;
using System.Text;

public partial class EvaluateHtml_UVYear : System.Web.UI.Page
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
        string strSQL = string.Format("SELECT CONVERT(VARCHAR(7),LST,120) as LST, AVG(ScoreUV) as  ScoreUV  FROM  T_UVEvaluate WHERE LST between '{0}' and '{1}' Group by CONVERT(VARCHAR(7),LST,120) ORDER BY LST DESC", dtFrom, dtTo);
        DataTable hazeTable = m_Database.GetDataTable(strSQL);
        Workbook workbook = new Workbook(); //工作簿 
        Worksheet sheet = workbook.Worksheets[0]; //工作表
        Cells cells = sheet.Cells;//单元格
        Aspose.Cells.Style styleTitle1 = workbook.Styles[workbook.Styles.Add()];//新增样式 
        styleTitle1.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
        cells[0, 0].PutValue("日期");
        cells[0, 0].SetStyle(styleTitle1);
        cells[0, 1].PutValue("紫外线");
        cells[0, 1].SetStyle(styleTitle1);
        for (int i = 0; i < hazeTable.Rows.Count; i++)
        {
            cells[i + 1, 0].PutValue(DateTime.Parse(hazeTable.Rows[i][0].ToString()).ToString("M月"));
            cells[i + 1, 1].PutValue(Math.Round(double.Parse(hazeTable.Rows[i][1].ToString()), 1).ToString());
            cells[i + 1, 0].SetStyle(styleTitle1);
            cells[i + 1, 1].SetStyle(styleTitle1);
        }
        string FileName = "紫外线年评分" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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