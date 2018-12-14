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

public partial class EvaluateHtml_DuratuibYear : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        H00.Value = DateTime.Now.AddYears(-1).ToString("yyyy年");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Database m_Database = new Database();
        string dateTime = Element.Value;
        string fromTime = DateTime.Parse(dateTime).ToString("yyyy-01-01 00:00:00");
        string toTime = DateTime.Parse(dateTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-01 23:59:59");
        string strSQL = string.Format("SELECT CONVERT(VARCHAR(7),LST,120) as LST, DurationID, ITEMID, Module, avg(Score) as  Score FROM  T_IAQIEvaluation WHERE LST between '{0}' and '{1}' and Module in ('WRF','Manual') Group by CONVERT(VARCHAR(7),LST,120), DurationID, ITEMID, Module ORDER BY LST ", fromTime, toTime);
        DataTable dt = m_Database.GetDataTable(strSQL);
        strSQL = string.Format("SELECT CONVERT(VARCHAR(7),LST,120) as LST, DurationID,Module,avg(F) as  Score  FROM T_DurationEvaluation WHERE LST between '{0}' and '{1}' and Module in ('WRF','Manual') Group by CONVERT(VARCHAR(7),LST,120), DurationID,Module ORDER BY LST ", fromTime, toTime);
        DataTable dk = m_Database.GetDataTable(strSQL);
        DataTable dtTime = dt.DefaultView.ToTable(true, "LST");
        string filter = "";
        DataRow[] newRow;
        string[] durationName = { "夜间", "上午", "下午" };
        int[] duration = { 6, 2, 3 };
        string[] module = { "WRF", "Manual" };
        string[] moduleName = { "WRF-chem", "主观预报" };
        Workbook workbook = new Workbook(); //工作簿 
        Worksheet sheet1 = workbook.Worksheets[0]; //工作表 
        
        Aspose.Cells.Style styleTitle1 = workbook.Styles[workbook.Styles.Add()];//新增样式 
        styleTitle1.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
        sheet1.Name = "分时段预报准确率逐月评分";
        Cells cells = sheet1.Cells;//单元格
        string[] titleName = { "PM2.5","PM10","NO2","O3","综合评分"};
        for (int i = 0; i < titleName.Length; i++)
        {
            cells[0, 3 + i].PutValue(titleName[i]);
            cells[0, 3 + i].SetStyle(styleTitle1);
        }
        for(int t=0;t<dtTime.Rows.Count;t++)
        {
            cells.Merge(1+t*6,0,6,1);
            cells[1+t*6,0].PutValue(DateTime.Parse(dtTime.Rows[t][0].ToString()).Month.ToString()+"月");
            cells[1+t*6,0].SetStyle(styleTitle1);
            for (int i = 0; i < duration.Length; i++)
            {
                cells.Merge(1 + t * 6+i*2, 1, 2, 1);
                cells[1 + t * 6 + i * 2, 1].PutValue(durationName[i]);
                cells[1 + t * 6 + i * 2, 1].SetStyle(styleTitle1);
                for (int j = 0; j < module.Length; j++)
                {
                    cells[1 + t * 6 + i * 2 + j, 2].PutValue(moduleName[j]); 
                    cells[1 + t * 6 + i * 2 + j, 2].SetStyle(styleTitle1);
                    for (int k = 1; k <= 4; k++)
                    {
                        filter = string.Format("LST='{0}' and Module='{1}' and DurationID='{2}' and ITEMID='{3}'", dtTime.Rows[t][0], module[j], duration[i], k);
                        newRow = dt.Select(filter);
                        if (newRow.Length > 0)
                        {
                            cells[1 + t * 6 + i * 2 + j, 2 + k].PutValue(Math.Round(double.Parse(newRow[0][4].ToString()), 1));
                            cells[1 + t * 6 + i * 2 + j, 2 + k].SetStyle(styleTitle1);
                        }
                    }
                    filter = string.Format("LST='{0}' and Module='{1}' and DurationID='{2}' ", dtTime.Rows[t][0], module[j], duration[i]);
                    newRow = dk.Select(filter);
                    if (newRow.Length > 0)
                    {
                        cells[1 + t * 6 + i * 2 + j, 7].PutValue(Math.Round(double.Parse(newRow[0][3].ToString()), 1));
                        cells[1 + t * 6 + i * 2 + j, 7].SetStyle(styleTitle1);
                    }
                }
            }
        }
        string FileName = "分时段预报准确率逐月评分" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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