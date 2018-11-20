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

public partial class EvaluateHtml_DurationEvaluate : System.Web.UI.Page
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

        string[] modle = { "RT", "Manual", "ManualSubmit", "ManualCenter", "WRF" };
        string[] modleName = { "实况", "气象部门", "环保部门", "两家合作", "WRF-chem" };
        string strSQL = string.Format("SELECT DurationID,Module,Quality as MC,LST from  T_DurationEvaluation  Where LST between '{0}' and '{1}';select DurationID, Module,Parameter as MC,LST from  T_DurationEvaluation  Where LST between '{0}' and '{1}' ;", fromTime, toTime);
        DataSet gradeSet = m_Database.GetDataset(strSQL);
        string[][] para = new string[2][];
        para[0] = new string[6] { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染" };
        para[1] = new string[4] { "PM2.5", "PM10", "NO2", "O3" };
        string[] duration = { "6", "2", "3" };
        string[] durationName = { "夜间", "上午", "下午" };

        string filter = "";
        DataTable dt;
        DataRow[] dataRow;
        string[] sheetName = { "空气质量等级预报情况", "首要污染物预报情况", "空气质量等级预报情况_百分比", "首要污染物预报情况_百分比", "IAQI预报准确率及综合评分" };
        Workbook workbook = new Workbook(); //工作簿 
        workbook.Worksheets.Add();
        workbook.Worksheets.Add();
        workbook.Worksheets.Add();
        workbook.Worksheets.Add();
        Aspose.Cells.Style styleTitle1 = workbook.Styles[workbook.Styles.Add()];//新增样式 
        styleTitle1.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
        int count = 0;
        int otherCount = 0;

        for (int j = 0; j < 2; j++)
        {
            Worksheet sheet1 = workbook.Worksheets[j]; //工作表
            sheet1.Name = sheetName[j];
            Cells cells = sheet1.Cells;//单元格
            dt = gradeSet.Tables[j];
            for (int k = 0; k < para[j].Length; k++)
                cells[0, 2 + k].PutValue(para[j][k]);
            for (int m = 0; m < duration.Length; m++)
            {
                for (int i = 0; i < modleName.Length; i++)
                {
                    if (i == 0)
                    {
                        cells.Merge(5 * m + 1, 0, 5, 1);
                        cells[5 * m + 1, 0].PutValue(durationName[m]);

                    }
                    cells[5 * m + i + 1, 1].PutValue(modleName[i]);
                    for (int k = 0; k < para[j].Length; k++)
                    {
                        if (modle[i] == "RT")
                        {
                            filter = string.Format("Module='{0}' and MC='{1}' and DurationID='{2}'", modle[i], para[j][k], duration[m]);
                            count = int.Parse(dt.Compute("COUNT(LST)", filter).ToString());
                            cells[5 * m + i + 1, k + 2].PutValue(count);
                        }
                        else
                        {
                            if (j == 0)
                                strSQL = string.Format("SELECT COUNT(*) from  T_DurationEvaluation Where  Module='{0}' and Quality='{1}'  and DurationID='{2}' and LST in (SELECT LST from  T_DurationEvaluation where Module='RT' and DurationID='{2}' and Quality='{1}');", modle[i], para[j][k], duration[m]);
                            else
                                strSQL = string.Format("SELECT COUNT(*) from  T_DurationEvaluation Where  Module='{0}' and Parameter like '%{1}%' and AQI>50 and DurationID='{2}' and LST in (SELECT LST from  T_DurationEvaluation where Module='RT' and DurationID='{2}' and Parameter='{1}');", modle[i], para[j][k], duration[m]);
                            otherCount = int.Parse(m_Database.GetFirstValue(strSQL));
                            cells[5 * m + i + 1, k + 2].PutValue(otherCount);
                        }
                    }

                }
 
            }
    
        }
        for (int j = 0; j < 2; j++)
        {
            Worksheet sheet1 = workbook.Worksheets[j+2]; //工作表
            sheet1.Name = sheetName[j + 2];
            Cells cells = sheet1.Cells;//单元格
            dt = gradeSet.Tables[j];
            for (int k = 0; k < para[j].Length; k++)
                cells[0, 2 + k].PutValue(para[j][k]);
            for (int m = 0; m < duration.Length; m++)
            {
                for (int i = 1; i < modleName.Length; i++)
                {
                    if (i == 1)
                    {
                        cells.Merge(4 * m + 1, 0, 4, 1);
                        cells[4 * m + 1, 0].PutValue(durationName[m]);

                    }
                    cells[4 * m + i, 1].PutValue(modleName[i]);
                    for (int k = 0; k < para[j].Length; k++)
                    {
                        filter = string.Format("Module='{0}' and MC='{1}' and DurationID='{2}'", "RT", para[j][k], duration[m]);
                        count = int.Parse(dt.Compute("COUNT(LST)", filter).ToString());
                        if (j == 0)
                            strSQL = string.Format("SELECT COUNT(*) from  T_DurationEvaluation Where  Module='{0}' and Quality='{1}' and DurationID='{2}' and LST in (SELECT LST from  T_DurationEvaluation where Module='RT' and DurationID='{2}' and Quality='{1}');", modle[i], para[j][k], duration[m]);
                        else
                            strSQL = string.Format("SELECT COUNT(*) from  T_DurationEvaluation Where  Module='{0}' and Parameter like '%{1}%' and AQI>50 and DurationID='{2}' and LST in (SELECT LST from  T_DurationEvaluation where Module='RT' and DurationID='{2}' and Parameter='{1}');", modle[i], para[j][k], duration[m]);
                        otherCount = int.Parse(m_Database.GetFirstValue(strSQL));
                        cells[4 * m + i, k + 2].PutValue(otherCount);
                        if (count != 0)
                            cells[4 * m + i, k + 2].PutValue(Math.Round((otherCount / double.Parse(count.ToString())) * 100, 1));
                        else
                            cells[4 * m + i, k + 2].PutValue("0");

                    }
                }
            }
        }
            //IAQI主客观预报准确率及综合评分(准确率/单位：百分比）
            //每个分指数的平均值
        strSQL = string.Format("SELECT DurationID, ITEMID, Module, AVG(Score) AS Score FROM T_IAQIEvaluation join D_Item  on T_IAQIEvaluation.ITEMID=D_Item.DM   Where LST between '{0}' and '{1}' GROUP BY DurationID, ITEMID, Module;", fromTime, toTime);
        DataTable iAQIScore = m_Database.GetDataTable(strSQL);
        strSQL = string.Format("SELECT AVG(F) as F,Module,DurationID FROM T_DurationEvaluation where module<>'RT' and LST between '{0}' and '{1}' group by Module,DurationID ", fromTime, toTime);
        DataTable totalScore = m_Database.GetDataTable(strSQL);
        Worksheet sheet2 = workbook.Worksheets[4]; //工作表
        sheet2.Name = sheetName[4];
        Cells cells2 = sheet2.Cells;//单元格
        for (int k = 0; k < para[1].Length; k++)
            cells2[0, 2 + k].PutValue(para[1][k]);
        cells2[0, 6].PutValue("综合评分");
        for (int m = 0; m < duration.Length; m++)
        {
            for (int i = 1; i < modleName.Length; i++)
            {
                if (i == 1)
                {
                    cells2.Merge(4 * m + 1, 0, 4, 1);
                    cells2[4 * m + 1, 0].PutValue(durationName[m]);

                }
                cells2[4 * m + i, 1].PutValue(modleName[i]);
                for (int k = 0; k < para[1].Length; k++)
                {
                    filter = string.Format("Module='{0}' and ITEMID='{1}' and DurationID='{2}'", modle[i], k + 1, duration[m]);
                    dataRow = iAQIScore.Select(filter);
                    if (dataRow.Length > 0)
                        cells2[4 * m + i, k + 2].PutValue(Math.Round(double.Parse(dataRow[0][3].ToString()), 1));
                    else
                        cells2[4 * m + i, k + 2].PutValue("-");
                }
                filter = string.Format("Module='{0}'  and DurationID='{1}'", modle[i], duration[m]);
                dataRow = totalScore.Select(filter);
                if (dataRow.Length > 0)
                    cells2[4 * m + i,6].PutValue(Math.Round(double.Parse(dataRow[0][0].ToString()), 1));
                else
                    cells2[4 * m + i, 6].PutValue("-");
            }
        }

        string FileName = "分段预报评分" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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