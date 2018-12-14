using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Readearth.Data;
using Aspose.Cells;
using System.Data;

public partial class EvaluateHtml_ChinaYear : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //如果当前用户没有登录的话，那么返回到登录界面
        StringBuilder sb = new StringBuilder();
        DateTime nowTime = DateTime.Now.AddMonths(-1);
        sb.AppendLine(" <label class='cur-select'>" + nowTime.Year + "年</label>");
        sb.AppendLine(" <select id='yearID' >");
        for (int i = 0; i > -5; i--)
        {
            if (i == 0)
                sb.AppendLine(string.Format(" <option value='{0}' selected='selected'>{0}</option>", nowTime.AddYears(i).Year + "年"));
            else
                sb.AppendLine(string.Format(" <option value='{0}'>{0}</option>", nowTime.AddYears(i).Year + "年"));
        }
        sb.AppendLine("</select>");
        yearSelect.InnerHtml = sb.ToString();

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Database m_Database = new Database();
        string content = Element.Value;
        string[] conStr = content.Split('|');
        string dateTime = conStr[0];
        string userName = conStr[2];
        Workbook workbook = new Workbook(); //工作簿 
        Worksheet sheet = workbook.Worksheets[0]; //工作表
        Cells cells = sheet.Cells;//单元格
        Aspose.Cells.Style styleTitle1 = workbook.Styles[workbook.Styles.Add()];//新增样式 
        styleTitle1.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
        styleTitle1.VerticalAlignment = TextAlignmentType.Center;
        string time = DateTime.Parse(dateTime).ToString("yyyy-01-01 00:00:00");
        string fromTime = time;
        string toTime = DateTime.Parse(fromTime).AddYears(1).AddDays(-1).ToString("yyyy-12-31 23:59:59");
        string strSQL = string.Format("SELECT userID,count(*) as count,avg(f1) as f1,avg(f2) as f2,avg(f3) as f3,avg( S) as totalScore,RANK() OVER(ORDER BY avg( S) DESC) as Rank FROM  T_ChinaEvaluation where Module ='Manual' and LST between '{0}' and '{1}'  group by userID order by Rank ", fromTime, toTime);
        DataTable dt = m_Database.GetDataTable(strSQL);
        string[] titleName = { "预报员", "班次", "首要污染物正确性评分", "AQI预报级别正确性评分", "AQI预报数值误差评分", "综合评分", "名次" };
        for (int i = 0; i < titleName.Length; i++)
        {
            cells[0, i].PutValue(titleName[i]);
            cells[0, i].SetStyle(styleTitle1);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            cells[1 + i, 0].PutValue(dt.Rows[i]["userID"].ToString());
            cells[1 + i, 1].PutValue(dt.Rows[i]["count"].ToString());
            cells[1 + i, 2].PutValue(Math.Round(double.Parse(dt.Rows[i]["f1"].ToString()), 1));
            cells[1 + i, 3].PutValue(Math.Round(double.Parse(dt.Rows[i]["f2"].ToString()), 1));
            cells[1 + i, 4].PutValue(Math.Round(double.Parse(dt.Rows[i]["f3"].ToString()), 1));
            cells[1 + i, 5].PutValue(Math.Round(double.Parse(dt.Rows[i]["totalScore"].ToString()), 1));
            cells[1 + i, 6].PutValue(dt.Rows[i]["Rank"].ToString());
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                cells[1 + i, j].SetStyle(styleTitle1);
            }
        }
        string FileName = "国家局年统计" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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