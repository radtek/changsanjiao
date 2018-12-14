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

public partial class EvaluateHtml_ChinaEvaluate : System.Web.UI.Page
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
        int totalDays = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).Day;
        string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
        string[] modle = { "RT", "Manual" ,"WRF"};
        string[] modleName = { "实况","主观预报", "WRF-chem",  };
        string strSQL = string.Format("SELECT  Module,COUNT(Quality) as count,D_Parameter.MC from  T_ChinaEvaluation as a join  D_Parameter  on a.Quality=D_Parameter.MC Where LST between '{0}' and '{1}' group by Module,Quality,MC;", fromTime, toTime);
        strSQL = strSQL + string.Format("select Module, count(Parameter) as count,D_Item.MC from  T_ChinaEvaluation as a join  D_Item  on a.Parameter=D_Item.MC Where LST between '{0}' and '{1}' group by Module,Parameter,MC;", fromTime, toTime);
        strSQL = strSQL + string.Format("SELECT Module,avg(f1) as f1,avg(f2) as f2,avg(f3) as f3,avg( S) as s FROM  T_ChinaEvaluation where Module<>'RT' and LST between '{0}' and '{1}'  group by Module order by Module desc;", fromTime, toTime);
        DataSet dSet = m_Database.GetDataset(strSQL);
        string[][] para = new string[3][];
        string[] sheetName = { "空气质量等级预报情况", "首要污染物预报情况", "AQI预报准确率及综合评分" };
        para[0] = new string[6] { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染" };
        para[1] = new string[4] { "PM2.5", "PM10", "NO2", "O3" };
        string [] other= new string[5] { "","首要污染物正确性评分", "AQI预报级别正确性评分", "AQI预报数值误差评分", "综合评分" };
        string filter = "";
        DataRow[] dataRows;
        DataTable dt;
        int[] day = new int[3];
        Workbook workbook = new Workbook(); //工作簿 
        workbook.Worksheets.Add();
        workbook.Worksheets.Add();
         
        Aspose.Cells.Style styleTitle1 = workbook.Styles[workbook.Styles.Add()];//新增样式 
        styleTitle1.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
       
     


        for (int j = 0; j < dSet.Tables.Count; j++)
        {
            Worksheet sheet1 = workbook.Worksheets[j]; //工作表
            sheet1.Name = sheetName[j];
            Cells cells = sheet1.Cells;//单元格
            dt = dSet.Tables[j];
            if (j == dSet.Tables.Count - 1)
            {
                for (int m = -1; m < dt.Rows.Count; m++)
                {
                    for (int t = 0; t < dt.Columns.Count; t++)
                    {
                        if (m == -1)
                        {
                            cells[0, t].PutValue(other[t]);
                        }
                        else
                        {
                            if (t == 0)
                                cells[m + 1, t].PutValue(modleName[t + 1]);
                            else
                            {
                                cells[m + 1, t].PutValue(Math.Round(double.Parse(dt.Rows[m][t].ToString()), 1));
                            }

                        }
                    }
                }
            }
            else
            {

                for (int i = -1; i < modleName.Length; i++)
                {
                    if (i != -1)
                        cells[i + 1, 0].PutValue(modleName[i]);
                    for (int k = 0; k < para[j].Length; k++)
                    {
                        if (i == -1)
                            cells[0, k + 1].PutValue(para[j][k]);
                        else
                        {
                            filter = string.Format("Module='{0}' and MC='{1}'", modle[i], para[j][k]);
                            dataRows = dt.Select(filter);
                            if (dataRows.Length > 0)
                                cells[i + 1, k + 1].PutValue(dataRows[0][1]);
                            else
                                cells[i + 1, k + 1].PutValue(0);
                        }

                    }
                }
                int t = 0;
                string tempStr = "";
                for (int i = 0; i < modle.Length; i++)
                {
                    filter = string.Format("Module='{0}'", modle[i]);
                    dataRows = dt.Select(filter);
                    if (dt.Compute("SUM(count)", filter).ToString() == "")
                        day[i] = 0;
                    else
                        day[i] = int.Parse(dt.Compute("SUM(count)", filter).ToString());
                    if (day[i] - totalDays < 0)
                    {
                        cells.Merge(4, 0, 1, para[j].Length);
                        if (t == 0)
                        {
                            
                            cells[4 + t, 0].PutValue("注：" + modleName[i] + "数据缺" + (totalDays - day[i]).ToString() + "日");
                        }
                        else
                            cells[4 + t, 0].PutValue("      " + modleName[i] + "数据缺" + (totalDays - day[i]).ToString() + "日");

                        if (t == 0)
                        {
                            if (modleName[i] == "WRF-chem")
                                tempStr = "注：WRF-chem模式预报数据缺" + (totalDays - day[i]).ToString() + "日";
                            else 
                                tempStr = "注：" + modleName[i] + "数据缺" + (totalDays - day[i]).ToString() + "日";
                        }
                        else
                            tempStr = "    " + modleName[i] + "数据缺" + (totalDays - day[i]).ToString() + "日";
                        t++;
                    }
                }
            }
        }

        string FileName = "国家局预报评分" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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