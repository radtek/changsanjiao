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

public partial class EvaluateHtml_UVAll : System.Web.UI.Page
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
        string strSQL = string.Format("SELECT count( RTGrade) as count ,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.RTGrade=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and RTGrade<5  group by RTGrade,D_UVGrade.DM ;", dtFrom, dtTo);
        strSQL = strSQL + string.Format("SELECT count( ForecastGrade16) as count ,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.ForecastGrade16=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and ForecastGrade16<5  group by ForecastGrade16,D_UVGrade.DM ;", dtFrom, dtTo);
        strSQL = strSQL + string.Format("SELECT count( ForecastGrade10) as count,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.ForecastGrade10=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and ForecastGrade10<5 group by ForecastGrade10,D_UVGrade.DM ;", dtFrom, dtTo);
        strSQL = strSQL + string.Format("SELECT count( *) as count,avg(ScoreUV) FROM T_UVEvaluate  WHERE LST BETWEEN '{0}' and '{1}'  ;", dtFrom, dtTo);
        DataSet hazeTable = m_Database.GetDataset(strSQL);
        StringBuilder sb = new StringBuilder();
        Workbook workbook = new Workbook(); //工作簿 
        Worksheet sheet = workbook.Worksheets[0]; //工作表
        Cells cells = sheet.Cells;//单元格
        Aspose.Cells.Style styleTitle1 = workbook.Styles[workbook.Styles.Add()];//新增样式 
        styleTitle1.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
        string[] name = { "实况", "预报（16时）", "预报（10时）", "评分" };
        for (int i = 1; i < 6; i++)
        {
            cells[0, i].PutValue(i.ToString()+"级");
            cells[0, i].SetStyle(styleTitle1);
        }
        DataTable dt = new DataTable();
        dt = hazeTable.Tables[3];
        int totalDays = 0;
        string score = "";
        string filter = "";
        int tempCOunt = 0;
        int count = 0;
        DataRow[] rows;
        if (dt.Rows.Count > 0)
        {
            totalDays = int.Parse(dt.Rows[0][0].ToString());
            //score = Math.Round(double.Parse(dt.Rows[0][1].ToString()), 1).ToString();
            score = Math.Round(double.Parse(dt.Rows[0][1].ToString() == "" ? "0" : dt.Rows[0][1].ToString()), 1).ToString();
        }
        for (int i = 0; i < hazeTable.Tables.Count; i++)
        {
            dt = hazeTable.Tables[i];
            count = 0;
            cells[i + 1, 0].PutValue(name[i]);
            cells[i + 1, 0].SetStyle(styleTitle1);
            if (i != hazeTable.Tables.Count - 1)
            {
                for (int j = 1; j < 6; j++)
                {
                    cells[i + 1, j].SetStyle(styleTitle1);
                    if (j != 5)
                    {
                        filter = "count='" + j + "'";
                        rows = dt.Select(filter);

                        if (rows.Length > 0)
                        {
                            count = count + int.Parse(rows[0][0].ToString());
                            cells[i + 1, j].PutValue(rows[0][0].ToString());
                        }
                        else
                            cells[i + 1, j].PutValue("0");
                    }
                    else
                    {
                        tempCOunt = totalDays - count;
                        cells[i + 1, j].PutValue(tempCOunt);
                    }

                }
            }
            else
            {
                cells.Merge(i + 1, 1, 1, 5);
                cells[i + 1, 1].PutValue(score);
                cells[i + 1, 1].SetStyle(styleTitle1);
            }
        }
        string FileName = "紫外线评分" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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