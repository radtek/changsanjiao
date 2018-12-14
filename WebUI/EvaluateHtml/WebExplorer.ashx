<%@ WebHandler Language="C#" Class="WebExplorer" %>

using System;
using System.Web;
using System.IO;
using System.Collections.Specialized;
using System.Data.OleDb;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Aspose.Cells;
using System.Text.RegularExpressions;

public class WebExplorer : IHttpHandler {
    private Readearth.Data.Database m_Database;
    public void ProcessRequest (HttpContext Context) {
        m_Database = new Readearth.Data.Database();
        Context.Response.Buffer = true;//互不影响
        Context.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(0);
        Context.Response.Expires = 0;
        Context.Response.AddHeader("Pragma", "No-Cache");
        string action = Context.Request["action"];//获取操作类型
        switch (action)
        {
            case "upload": UploadData(Context); break;//获取文件列表
            case "dataExport":DataExport(Context);break;
        }
    }
    public void DataExport(HttpContext Context) {
        string date=Context.Request["date"];
        date = DateTime.Parse(date).ToString("yyyy-MM");
        string dtFrom = DateTime.Parse(date).ToString("yyyy-MM-01 00:00:00");
        string dtTo = DateTime.Parse(date).Date.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
        string strFirstTabSQL = string.Format("SELECT *,RANK() OVER(ORDER BY PersonScore DESC) as PersonScoreRank,RANK() OVER(ORDER BY AvgChinaScore DESC) AvgChinaScoreRank FROM T_PersonScore WHERE LST between  '{0}' AND '{1}' Order by PersonScore desc", dtFrom, dtTo);
        DataTable dtFirstEvalate = m_Database.GetDataTable(strFirstTabSQL);
        //StringBuilder sb = new StringBuilder();
        string strSecondTabSQL = "select [forecaster],[bc],[cnAQIsum],[cnAQIaverge],[totalEnvir],[totalPM25],[totalO31h],"+
            "[totalO38h],[totalPM10],[totalNO2],[totalHAZE],[totalUV],[singleEnvir],[singlePM25],[singleO31h],[singleO38h],"+
            "[singlePM10],[singleNO2],[singleHAZE],[singleUV],[periodNight],[periodMorning],[periodAfternoon],"+
            "[dayScore],[envirSumScore],[personScore] from [T_Evaluate] where time='"+date+"-01'";
        DataTable dtSecondEvalate = m_Database.GetDataTable(strSecondTabSQL);
        Workbook workbook = new Workbook(); //工作簿 
        workbook.Worksheets.Clear();
        Worksheet sheet1 = workbook.Worksheets.Add("Sheet1"); //工作表
        sheet1 = GetFirstTabEvaluate(sheet1,workbook,dtFirstEvalate);
        Worksheet sheet2 = workbook.Worksheets.Add("Sheet2");
        sheet2 = GetSecondTabEvaluate(sheet2,workbook,dtSecondEvalate);
        string FileName = "月统计评分" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
        string UserAgent = Context.Request.ServerVariables["http_user_agent"].ToLower();
        if (UserAgent.IndexOf("firefox") == -1)
            FileName = HttpUtility.UrlEncode(FileName, Encoding.UTF8);
        Context.Response.ContentType = "application/ms-excel";
        Context.Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        Context.Response.BinaryWrite(workbook.SaveToStream().ToArray());
        Context.Response.Flush();
        Context.Response.End();
    }
    public Worksheet GetSecondTabEvaluate(Worksheet sheet, Workbook workbook, DataTable dt)
    {
        Cells cells = sheet.Cells;
        Aspose.Cells.Style styleTitle1 = workbook.Styles[workbook.Styles.Add()];//新增样式 
        styleTitle1.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        cells.Merge(0, 0, 1, 2);
        cells[0, 0].PutValue("预报员及班次");
        cells[0, 0].SetStyle(styleTitle1);
        cells.Merge(0, 2, 1, 2);
        cells[0, 2].PutValue("上传国家局AQI评分");
        cells[0, 2].SetStyle(styleTitle1);
        cells.Merge(0, 4, 1, 8);
        cells[0, 4].PutValue("总分");
        cells[0, 4].SetStyle(styleTitle1);
        cells.Merge(0, 12, 1, 8);
        cells[0, 12].PutValue("各项平均分");
        cells[0, 12].SetStyle(styleTitle1);
        cells.Merge(0, 20, 1, 3);
        cells[0, 20].PutValue("分时段评分");
        cells[0, 20].SetStyle(styleTitle1);
        cells.Merge(0, 23, 1, 3);
        string[] title = { "预报员", "班次", "总分", "平均分", "环境", "PM2.5", "O3-1h", "O3-8h", "PM10", "NO2", "霾", "UV", "环境", "PM2.5", "O3-1h", "O3-8h", "PM10", "NO2", "霾", "UV", "夜间", "上午", "下午", "日评分", "环境总分", "个人成绩" };
        for (int col = 0; col < title.Length; col++)
        {   //添加表头
            cells[1, col].PutValue(title[col]);
            cells[1, col].SetStyle(styleTitle1);
        }
        for (int rowCount = 0; rowCount < dt.Rows.Count; rowCount++)
        {
            for (int colCount = 0; colCount < dt.Columns.Count; colCount++)
            {
                string val = dt.Rows[rowCount][colCount].ToString();
                string colName = dt.Columns[colCount].ColumnName;
                    string p = @"^(-?\d+)(\.\d+)?$";
                    Regex rgx = new Regex(p);
                    if (rgx.IsMatch(val))
                    {
                        cells[2 + rowCount, colCount].PutValue(float.Parse((double.Parse(val)).ToString("f2")));
                    }
                    else {
                        cells[2 + rowCount, colCount].PutValue(val);
                    }
              //  }
                cells[2 + rowCount, colCount].SetStyle(styleTitle1);
            }
        }
        return sheet;
    }
    public Worksheet GetFirstTabEvaluate(Worksheet sheet,Workbook workbook,DataTable dt) {
        Cells cells = sheet.Cells;//单元格
        Aspose.Cells.Style styleTitle1 = workbook.Styles[workbook.Styles.Add()];//新增样式 
        styleTitle1.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        cells.Merge(0, 0, 1, 2);
        cells[0, 0].PutValue("预报员及班次");
        cells[0, 0].SetStyle(styleTitle1);
        cells.Merge(0, 2, 1, 11);
        cells[0, 2].PutValue("常规预报评分");
        cells[0, 2].SetStyle(styleTitle1);
        cells.Merge(0, 13, 1, 3);
        cells[0, 13].PutValue("上传国家局AQI评分");
        cells[0, 13].SetStyle(styleTitle1);
        cells.Merge(0, 16, 1, 4);
        cells[0, 16].PutValue("附加分");
        cells[0, 16].SetStyle(styleTitle1);
        cells.Merge(0, 20, 1, 2);
        cells[0, 20].PutValue("个例总结");
        cells[0, 20].SetStyle(styleTitle1);
        cells[0, 22].PutValue("漏登记");
        cells[0, 22].SetStyle(styleTitle1);
        string[] titileName = { "预报员", "班次", "环境总分", "环境平均分", "PM2.5平均分", "PM10平均分", "NO2平均分", "O3-1h平均分", "O3-8h平均分", "霾平均分", "UV平均分", "个人总评分", "排名", "总分", "平均分", "排名", "担任科室工作加分", "扣分", "带班加分", "扣分原因", "应总结个例数", "完成个例数", "次" };
        for (int i = 0; i < titileName.Length; i++)
        {
            cells[1, i].PutValue(titileName[i]);
            cells[1, i].SetStyle(styleTitle1);
        }
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 1; j < dt.Columns.Count - 2; j++)
                {
                    string colName = dt.Columns[j].ColumnName;
                    string val = dt.Rows[i][j].ToString();
                    int aqi = 0;
                    cells[2 + i, j - 1].SetStyle(styleTitle1);
                    if (j < 12)
                    {
                        //if (colName.IndexOf("PM") > -1 || colName.IndexOf("O3") > -1 || colName.IndexOf("NO2") > -1)
                        //{
                        //    if (val != "")
                        //    {
                        //        if (colName.IndexOf("PM25") > -1)
                        //        {
                        //            aqi = ToAQI(val, "1");
                        //        }
                        //        else if (colName.IndexOf("PM10") > -1)
                        //        {
                        //            aqi = ToAQI(val, "2");
                        //        }
                        //        else if (colName.IndexOf("O31") > -1)
                        //        {
                        //            aqi = ToAQI(val, "4");
                        //        }
                        //        else if (colName.IndexOf("O38") > -1)
                        //        {
                        //            aqi = ToAQI(val, "5");
                        //        }
                        //        else if (colName.IndexOf("NO2") > -1)
                        //        {
                        //            aqi = ToAQI(val, "3");
                        //        }
                        //        val = val + "/" + aqi;
                        //    }
                        //}
                        string p = @"^(-?\d+)(\.\d+)?$";
                        Regex rgx = new Regex(p);
                        if (rgx.IsMatch(val))
                        {
                            cells[2 + i, j - 1].PutValue(float.Parse(val));
                        }
                        else
                        {
                            cells[2 + i, j - 1].PutValue(val);
                        }
                        //cells[2 + i, j - 1].PutValue(val);

                    }
                    else
                    {
                        if (j == 12)
                        {
                            cells[2 + i, j - 1].PutValue(dt.Rows[i][j]);
                            cells[2 + i, j].PutValue(dt.Rows[i]["PersonScoreRank"]);
                        }
                        else if (j == 13 || j == 14)
                        {
                            cells[2 + i, j].PutValue(dt.Rows[i][j]);
                            if (j == 14)
                            {
                                cells[2 + i, j + 1].PutValue(dt.Rows[i]["AvgChinaScoreRank"]);
                            }
                        }
                        else
                        {
                            cells[2 + i, j + 1].PutValue(dt.Rows[i][j]);
                        }
                    }
                }
            }
        }
        return sheet;
    }
    public void UploadData(HttpContext Context) {
        string path = Context.Server.MapPath(Context.Request["value"]);
        HttpFileCollection files = Context.Request.Files ;
        long allSize = 0;
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        for (int i = 0; i < files.Count; i++)
        {
            allSize += files[i].ContentLength;
        }
        if (allSize > 20 * 1024 * 1024)
        {
            Context.Response.Write("error");
            return;
        }


        for (int i = 0; i < files.Count; i++)
        {
            files[i].SaveAs(path + "\\"+Path.GetFileName(files[i].FileName));

        }
        string saveName = path +"\\"+ Path.GetFileName(files[0].FileName);

        StringCollection sheetNames = ExcelSheetName(saveName);
        DataTable dtBasic = ExecleDs(saveName, sheetNames[sheetNames.Count-1].ToString());
        dtBasic.Rows.RemoveAt(0);
        string insert = "insert into [T_EvaluateBasicData] values";

        string month = DateTime.Parse(dtBasic.Rows[0][0].ToString().Trim()).ToString("yyyy-MM");
        string days = DateTime.DaysInMonth(DateTime.Parse(month).Year,DateTime.Parse(month).Month).ToString();
        string delSql = "delete from [T_EvaluateBasicData] where date between '"+month+"-01 00:00:00' and '"+month+"-"+days+" 00:00:00'";
        for (int i = 0; i < dtBasic.Rows.Count; i++) {
            string date =DateTime.Parse(dtBasic.Rows[i][0].ToString().Trim()).ToString("yyyy-MM-dd");
            string num = dtBasic.Rows[i][1].ToString().Trim();
            string mn_PM25 = dtBasic.Rows[i][2].ToString().Trim();
            string mn_O31 = dtBasic.Rows[i][3].ToString().Trim();
            string mn_O38 = dtBasic.Rows[i][4].ToString().Trim();
            string mn_PM10 = dtBasic.Rows[i][5].ToString().Trim();
            string mn_NO2 = dtBasic.Rows[i][6].ToString().Trim();
            string an_PM25 = dtBasic.Rows[i][7].ToString().Trim();
            string an_O31 = dtBasic.Rows[i][8].ToString().Trim();
            string an_O38 = dtBasic.Rows[i][9].ToString().Trim();
            string an_PM10 = dtBasic.Rows[i][10].ToString().Trim();
            string an_NO2 = dtBasic.Rows[i][11].ToString().Trim();
            string m_PM25 = dtBasic.Rows[i][12].ToString().Trim();
            string m_O31 = dtBasic.Rows[i][13].ToString().Trim();
            string m_O38 = dtBasic.Rows[i][14].ToString().Trim();
            string m_PM10 = dtBasic.Rows[i][15].ToString().Trim();
            string m_NO2 = dtBasic.Rows[i][16].ToString().Trim();
            string a_PM25 = dtBasic.Rows[i][17].ToString().Trim();
            string a_O31 = dtBasic.Rows[i][18].ToString().Trim();
            string a_O38 = dtBasic.Rows[i][19].ToString().Trim();
            string a_PM10 = dtBasic.Rows[i][20].ToString().Trim();
            string a_NO2 = dtBasic.Rows[i][21].ToString().Trim();
            insert += "('" + date + "','" + num + "','" + mn_PM25 + "','" + mn_O31 + "','" + mn_O38 + "','" + mn_PM10 + "','" +
                    "" + mn_NO2 + "','" + an_PM25 + "','" + an_O31 + "','" + an_O38 + "','" + an_PM10 + "','" + an_NO2 + "'" +
                    ",'" + m_PM25 + "','" + m_O31 + "','" + m_O38 + "','" + m_PM10 + "','" + m_NO2 + "'" +
                    ",'" + a_PM25 + "','" + a_O31 + "','" + a_O38 + "','" + a_PM10 + "','" + a_NO2 + "'),";

        }
        insert = insert.TrimEnd(',');
        try
        {
            //先删除数据
            m_Database.Execute(delSql);
            m_Database.Execute(insert);
            Context.Response.Write("ok");
        }
        catch (Exception e) {
            Context.Response.Write("error" + e.Message);

        }

    }
    //把Excel中的基础数据处理成与数据库表相同的列
    public List<string> ProExcelData(DataTable dtExcel) {
        //获取列名
        List<string> colName = new List<string>();
        for (int col = 0; col < dtExcel.Columns.Count; col++) {
            colName.Add(dtExcel.Rows[0][col].ToString());
        }
        return colName;
    }
    /// <summary>
    /// 查询EXCEL电子表格添加到DATASET
    /// </summary>
    /// <param name="filenameurl">服务器路径</param>
    /// <param name="table">表名</param>
    /// <param name="SheetName">Sheet表名</param>
    /// <returns>读取的DataSet </returns>
    public DataTable ExecleDs(string filenameurl, string SheetName)
    {
        string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filenameurl + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";
        OleDbConnection conn = new OleDbConnection(strConn);
        conn.Open();
        DataTable dt = new DataTable();
        OleDbDataAdapter odda = new OleDbDataAdapter("select * from [" + SheetName + "]", conn);
        odda.Fill(dt);
        odda.Dispose();
        conn.Close();
        return dt;
    }
    public StringCollection ExcelSheetName(string filepath)
    {
        StringCollection names = new StringCollection();
        try
        {

            string strConn;
            strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable sheetNames = conn.GetOleDbSchemaTable
            (System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            conn.Close();
            foreach (DataRow dr in sheetNames.Rows)
            {
                names.Add(dr[2].ToString());
            }
        }
        catch (Exception e)
        {
            string tip =  e.Message;
        }
        return names;
    }
    //public int ToAQI(string value, string itemID)
    //{
    //    //string itemID = ItemToItemID(item.Trim());
    //    int AQIValue = 0;
    //    double inputValue = 0d;
    //    double.TryParse(value, out inputValue);
    //    inputValue = inputValue / 1000;
    //    switch (itemID)
    //    {
    //        case "1":
    //            AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 24, 11, 180);
    //            break;
    //        case "2":
    //            AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 7, 11, 180);
    //            break;
    //        case "3":
    //            AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 22, 10, 0);
    //            break;
    //        case "4":
    //            AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 8, 10, 0);
    //            break;
    //        case "5":
    //            AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 8, 16, 16);
    //            break;
    //    }
    //    return AQIValue;
    //}

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}