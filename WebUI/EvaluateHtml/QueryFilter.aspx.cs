using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Readearth.Data;
using System.Data;
using Aspose.Cells;

public partial class EvaluateHtml_QueryFilter : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //如果当前用户没有登录的话，那么返回到登录界面
        StringBuilder sb = new StringBuilder();
        DateTime nowTime=DateTime.Now.AddMonths(-1);
        sb.AppendLine(" <label class='cur-select'>"+nowTime.Year+"年</label>");
        sb.AppendLine(" <select id='yearID' >");
        for(int i=0;i>-5;i--)
        {
            if(i==0)
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
        string title = "";
        string strSQL="";
        //if (conStr[1] == "333")
        //{
        title = "个人评分排名";
        strSQL = string.Format("SELECT UserName,AVGSEMCScore,HazeScore, UVScore,PersonScore,RANK() OVER(ORDER BY PersonScore DESC) as PersonScoreRank,AvgChinaScore,RANK() OVER(ORDER BY AvgChinaScore DESC) as AvgChinaScoreRank FROM T_PersonScoreYear WHERE LST ='{0}' order by PersonScoreRank", time);
        DataTable dt = m_Database.GetDataTable(strSQL);
           
        cells.Merge(0, 0, 2, 1);
        cells[0, 0].PutValue("姓名");
        cells[0, 0].SetStyle(styleTitle1);
        cells.Merge(0, 1, 1, 5);
        cells[0, 1].PutValue("常规预报评分");
        cells[0, 1].SetStyle(styleTitle1);
        cells.Merge(0, 6, 1, 2);
        cells[0, 6].PutValue("国家局评分");
        cells[0, 6].SetStyle(styleTitle1);
        string[] titleName = { "分时段预报准确率", "霾预报准确率", "紫外线预报准确率", "个人总分", "名次", "国家局评分", "名次" };
        for (int i = 0; i < titleName.Length; i++)
        {
            cells[1, 1 + i].PutValue(titleName[i]);
            cells[1, 1 + i].SetStyle(styleTitle1);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                cells[2 + i, j].PutValue(dt.Rows[i][j]);
                cells[2 + i, j].SetStyle(styleTitle1);
            }
        }
        //}
        //else
        //{
        //    title = userName;
        //    string filter = "";
        //    DataRow[] dataRows;
        //    double totalScore = 0.0;
        //    string[] titileName = { "时间", "分时段预报准确率", "霾预报准确率", "紫外线预报准确率", "国家局准确率", "总分" };
        //    for (int i = 0; i < titileName.Length; i++)
        //    {
        //        cells[0, i].PutValue(titileName[i]);
        //        cells[0, i].SetStyle(styleTitle1);
        //    }
        //    string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
        //    string toTime = DateTime.Parse(dateTime).Date.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
         
        //    strSQL = string.Format("SELECT LST,AVG(F) as Score  FROM  T_DurationEvaluation WHERE  userID='{2}' and Module='Manual' GROUP BY LST  order by LST;", fromTime, toTime, userName);
        //    strSQL = strSQL + string.Format("SELECT LST,Score FROM T_HazeEvaluate WHERE  userID='{2}' order by LST;", fromTime, toTime, userName);
        //    strSQL = strSQL + string.Format("SELECT LST,Score FROM T_UVEvaluate WHERE  userID='{2}' order by LST;", fromTime, toTime, userName);
        //    strSQL = strSQL + string.Format("SELECT LST,S  as Score FROM T_ChinaEvaluation WHERE  userID='{2}' and Module='Manual' order by LST;", fromTime, toTime, userName);
        //    DataSet dtSet = m_Database.GetDataset(strSQL);
        //    for (int i = 0; i < dtSet.Tables[0].Rows.Count; i++)
        //    {
        //        totalScore = 0.0;
        //        DataRow rows=dtSet.Tables[0].Rows[i];
        //        cells[i + 1, 0].PutValue(DateTime.Parse(rows["LST"].ToString()).ToString("yyyy-MM-dd HH:00"));
        //        cells[i + 1, 1].PutValue(Math.Round(double.Parse(rows["Score"].ToString()), 1));
        //        totalScore = 0.6 * Math.Round(double.Parse(rows["Score"].ToString()), 1);
        //        filter = string.Format("LST='{0}'", rows["LST"].ToString());
        //        for (int j = 1; j < dtSet.Tables.Count; j++)
        //        {
        //            dataRows = dtSet.Tables[j].Select(filter);
        //            if (dataRows.Length > 0)
        //            {
        //                if (i != dtSet.Tables.Count - 1)
        //                    totalScore = totalScore + double.Parse(dataRows[0]["Score"].ToString());
        //                cells[i + 1, 1 + j].PutValue(dataRows[0]["Score"]);
        //            }
        //            else
        //                cells[i + 1, 1 + j].PutValue("-");
        //        }
        //        cells[i + 1, 5].PutValue(Math.Round(totalScore, 1));
        //    }
        //    strSQL = string.Format("SELECT AVGSEMCScore,HazeScore, UVScore,AvgChinaScore,PersonScore FROM T_PersonScore WHERE LST ='{0}' and UserName='{1}' ", fromTime, userName);
        //    DataTable dt = m_Database.GetDataTable(strSQL);
        //    if (dt.Rows.Count > 0)
        //    {
        //        cells[dtSet.Tables[0].Rows.Count + 1, 0].PutValue("当月平均");
        //        cells[dtSet.Tables[0].Rows.Count + 1, 1].PutValue(dt.Rows[0]["AVGSEMCScore"]);
        //        cells[dtSet.Tables[0].Rows.Count + 1, 2].PutValue(dt.Rows[0]["HazeScore"]);
        //        cells[dtSet.Tables[0].Rows.Count + 1, 3].PutValue(dt.Rows[0]["UVScore"]);
        //        cells[dtSet.Tables[0].Rows.Count + 1, 4].PutValue(dt.Rows[0]["AvgChinaScore"]);
        //        cells[dtSet.Tables[0].Rows.Count + 1, 5].PutValue(dt.Rows[0]["PersonScore"]);
        //    }
        //}
        string FileName = "个人评分排名" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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