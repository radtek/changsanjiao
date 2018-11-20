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

public partial class EvaluateHtml_UV : System.Web.UI.Page
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
        string strSQL = string.Format("SELECT LST,ForecastGrade16, ForecastGrade10, RTGrade, ForecastAccuracy16, ForecastAccuracy10, ScoreUV FROM T_UVEvaluate WHERE LST BETWEEN '{0}' and '{1}' ORDER by LST", dtFrom, dtTo);
        DataTable hazeTable = m_Database.GetDataTable(strSQL);
        StringBuilder sb = new StringBuilder();
        Workbook workbook = new Workbook(); //工作簿 
        Worksheet sheet = workbook.Worksheets[0]; //工作表
        Cells cells = sheet.Cells;//单元格
        Aspose.Cells.Style styleTitle1 = workbook.Styles[workbook.Styles.Add()];//新增样式 
        styleTitle1.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
        cells.Merge(0, 0, 2, 1);
        cells[0, 0].PutValue("日期");
        cells[0, 0].SetStyle(styleTitle1);
        cells.Merge(0, 1, 1, 3);
        cells[0, 1].PutValue("UV");
        cells[0, 1].SetStyle(styleTitle1);
        cells.Merge(0, 4, 2, 1);
        cells[0, 4].PutValue("16时评分");
        cells[0, 4].SetStyle(styleTitle1);
        cells.Merge(0, 5, 2, 1);
        cells[0, 5].PutValue("10时评分");
        cells[0, 5].SetStyle(styleTitle1);
        cells.Merge(0, 6, 2, 1);
        cells[0, 6].PutValue("评分");
        cells[0, 6].SetStyle(styleTitle1);
        cells[1, 1].PutValue("16时预报");
        cells[1, 1].SetStyle(styleTitle1);
        cells[1, 2].PutValue("10时预报");
        cells[1, 2].SetStyle(styleTitle1);
        cells[1, 3].PutValue("实况");
        cells[1, 3].SetStyle(styleTitle1);
        for (int i = 0; i < hazeTable.Rows.Count; i++)
        {
            for(int j=0;j<hazeTable.Columns.Count;j++)
            {
                cells[2 + i, j].SetStyle(styleTitle1);
                if(j==0)
                    cells[2 + i, j].PutValue(DateTime.Parse(hazeTable.Rows[i][j].ToString()).ToString("yyyy年MM月dd日"));
                else
                    cells[2 + i, j].PutValue(hazeTable.Rows[i][j]);
            }

        }
        string FileName = "逐日紫外线评分" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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