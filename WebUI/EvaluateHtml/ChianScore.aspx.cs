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
using System.Web.Services;

public partial class EvaluateHtml_ChianScore : System.Web.UI.Page
{
    private static Database m_Database;
    protected void Page_Load(object sender, EventArgs e)
    {
        m_Database = new Database();
        H00.Value = DateTime.Now.AddMonths(-1).Date.ToString("yyyy年MM月");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        //Database m_Database = new Database();
        string fileName = "";
        string dateTime = Element.Value;
        string type = Type.Value;
        StringBuilder sb = new StringBuilder();
        string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
        int totalDays = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).Day;
        string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
        if (type == "pf")
        {
            sb = GetEvaluateData(fromTime, totalDays, toTime);
            fileName = "国家局日评分";
        }
        else if (type == "yb")
        {
            sb = GetForecastData(fromTime, totalDays, toTime);
            fileName = "国家局日预报";
        }
        else if (type == "sk") {
            sb = GetRealData(fromTime, dateTime, toTime);
            fileName = "国家局日实况";
        }
        StringWriter SW = new StringWriter(sb);
        SW.Close();
        string saveAsFileName = string.Format("{0}.csv", fileName);
        Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(saveAsFileName));
        Response.ContentType = "application/ms-excel";
        Response.Charset = "GB2312";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.Write(SW);
        Response.Flush();
        Response.End();

    }
    private static StringBuilder GetForecastData(string fromTime, int totalDays, string toTime) {
        string strSQL = string.Format("select SUBSTRING(CONVERT(varchar(20), lst, 20), 0, 11) as lst, pm25, pm10, no2, O3 from T_ChinaValue "+
            "where  lst between '" + fromTime + "' and '"+toTime+"' and module='Manual' order by lst asc");
        DataTable dt = m_Database.GetDataTable(strSQL);
        StringBuilder sb = new StringBuilder();
        sb.Append("日期,,PM25,PM10,NO2,O3"+Environment.NewLine);
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int rowCount = 0; rowCount < dt.Rows.Count; rowCount++)
            {
                for (int colCount = 0; colCount < dt.Columns.Count; colCount++)
                {
                    string colName = dt.Columns[colCount].ColumnName;
                    string val = dt.Rows[rowCount][colCount].ToString();
                    int aqi = 0;
                    if (colName.IndexOf("pm") > -1 || colName.IndexOf("no") > -1 || colName.IndexOf("O3") > -1)
                    {
                        if (val != "")
                        {
                            if (colName == "pm25")
                            {
                                aqi = ToAQI(val, "1");
                            }
                            else if (colName == "pm10")
                            {
                                aqi = ToAQI(val, "2");
                            }
                            else if (colName == "no2")
                            {
                                aqi = ToAQI(val, "3");
                            }
                            else if (colName == "O3")
                            {
                                aqi = ToAQI(val, "4");
                            }
                            val = val + "/" + aqi;
                        }
                        sb.Append(""+val+",");
                    }
                    else
                    {
                        sb.Append("" + val + ",");
                    }
                    if (colCount == 0)
                    {
                        sb.Append("预报,");
                    }
                }
                sb.Append(Environment.NewLine);
            }
        }
        else {
            sb.Append("注：没有数据");
        }
        return sb;
    }
    private static StringBuilder GetRealData(string fromTime, string date, string toTime) {
        string strSQL = string.Format("select SUBSTRING(CONVERT(varchar(20), lst, 20), 0, 11) as lst, aqi, itemId from T_ChinaShiValue where lst between '" + fromTime + "' and '"+toTime+"'");
        DataTable dt = m_Database.GetDataTable(strSQL);
        StringBuilder sb = new StringBuilder();
        string[] itemId = { "1", "2", "3", "4", "5" };
        sb.Append("日期,,PM25,PM10,NO2,O3-1,O3-8" + Environment.NewLine);
        int days = DateTime.DaysInMonth(DateTime.Parse(date).Year, DateTime.Parse(date).Month);
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int day = 1; day <= days; day++)
            {
                string lst = DateTime.Parse(date + day).ToString("yyyy-MM-dd");
                sb.Append(lst + ",实况,");
                for (int i = 0; i < itemId.Length; i++)
                {
                    string filter = "ITEMID='" + itemId[i] + "' and lst='" + lst + "'";
                    DataRow[] row = dt.Select(filter);
                    sb.Append("" + row[0].ItemArray[1] + ",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(Environment.NewLine);
            }
        }
        else {
            sb.Append("注：没有数据");
        }
        return sb;
    }

    private static StringBuilder GetEvaluateData( string fromTime, int totalDays, string toTime)
    {
        string[] modle = { "Manual"};
        string[] modleName = { "主观预报" };
        string strSQL = string.Format("SELECT  LST,Module, f1, f2, f3, S from  T_ChinaEvaluation  Where LST between '{0}' and '{1}' and Module<>'RT' order by LST;", fromTime, toTime);
        DataTable dt = m_Database.GetDataTable(strSQL);
        DataTable timeTable = dt.DefaultView.ToTable(true, "LST");
        string filter = "";
        string lineStr = "";
        DataRow[] dataRows;
        int[] day = new int[1];
        for (int i = 0; i < modle.Length; i++)
        {
            filter = string.Format("Module='{0}'", modle[i]);
            dataRows = dt.Select(filter);
            day[i] = dataRows.Length;
        }
        StringBuilder sb = new StringBuilder();
        lineStr = "{0},{1},{2},{3},{4},{5}";
        sb.AppendLine("日期,,首要污染物正确性评分(f1),AQI预报级别正确性评分（f2）,AQI预报数值误差评分（f3）,空气质量预报精确度评分（S）");
        foreach (DataRow rows in timeTable.Rows)
        {
            for (int i = 0; i < modle.Length; i++)
            {
                filter = string.Format("Module='{0}' and LST='{1}'", modle[i], rows[0].ToString());
                dataRows = dt.Select(filter);
                if (dataRows.Length > 0)
                {
                    sb.AppendLine(string.Format(lineStr, DateTime.Parse(rows[0].ToString()).ToString("MM月dd日"), modleName[i], dataRows[0][2], dataRows[0][3], dataRows[0][4], dataRows[0][5]));

                }
                else
                {
                    sb.AppendLine(string.Format(lineStr, DateTime.Parse(rows[0].ToString()).ToString("MM月dd日"), modleName[i], "", "", "", ""));
                }
            }
        }
        int k = 0;
        string tempStr = "";
        for (int i = 0; i < day.Length; i++)
        {
            if (day[i] - totalDays < 0)
            {

                if (k == 0)
                {
                    if (modleName[i] == "WRF-chem")
                        tempStr = "注：WRF-chem模式预报数据缺" + (totalDays - day[i]).ToString() + "日";
                    else
                        tempStr = "注：" + modleName[i] + "数据缺" + (totalDays - day[i]).ToString() + "日";
                }
                else
                    tempStr = "   " + modleName[i] + "数据缺" + (totalDays - day[i]).ToString() + "日";
                k++;
                sb.AppendLine(tempStr);
            }
        }

        return sb;
    }

    [WebMethod]
    public static string Real(string date)
    {
        date = DateTime.Parse(date).ToString("yyyy-MM");
        string sql = "select SUBSTRING(CONVERT(varchar(20),lst, 20),0,11) as lst,aqi,itemId from T_ChinaShiValueII where  SUBSTRING(CONVERT(varchar(100),lst, 20),0,8) = '" + date + "'";
        string[] itemId = { "1", "2", "3", "4", "5" };
        DataTable dt = m_Database.GetDataTable(sql);
        StringBuilder sb = new StringBuilder();
        sb.Append("{");
        int days = DateTime.DaysInMonth(DateTime.Parse(date).Year, DateTime.Parse(date).Month);
        // "row1": [{ "val": "2018-3-1" }, { "val": "实况" }, { "val": 23 }, { "val": 23 }, { "val": 23 }, { "val": 23 }]
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int day = 1; day <= days; day++)
            {
                string lst = DateTime.Parse(date + "-" + day).ToString("yyyy-MM-dd");
                sb.Append("\"row" + (day-1) + "\":[{\"val\":\"" + lst + "\"},{\"val\":\"实况\"},");
                for (int i = 0; i < itemId.Length; i++)
                {
                    string filter = "ITEMID='" + itemId[i] + "' and lst='" + lst + "'";
                    DataRow[] row = dt.Select(filter);
                    sb.Append("{\"val\":\"" + row[0].ItemArray[1] + "\"},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("],");
            }
            sb.Remove(sb.Length - 1, 1);
        }
        else {
            sb.Append("\"row0\": [{ \"val\": \"没有数据\",\"colspan\": 7}]");
        }
        
        sb.Append("}");
        return sb.ToString();
    }
    [WebMethod]
    public static string Forecast(string date)
    {
        date = DateTime.Parse(date).ToString("yyyy-MM");
        string sql = "select SUBSTRING(CONVERT(varchar(20),lst, 20),0,11) as lst,pm25,pm10,no2,O3 from T_ChinaValue "+
            "where  SUBSTRING(CONVERT(varchar(100),lst, 20),0,8) = '" + date + "' and module='Manual' order by lst asc";
        DataTable dt = m_Database.GetDataTable(sql);
        StringBuilder sb = new StringBuilder();
        sb.Append("{");
        if (dt != null && dt.Rows.Count > 0)
        {
           
            for (int rowCount = 0; rowCount < dt.Rows.Count; rowCount++)
            {
                sb.Append("\"row" + rowCount + "\":[");
                for (int colCount = 0; colCount < dt.Columns.Count; colCount++)
                {
                    string colName = dt.Columns[colCount].ColumnName;
                    string val = dt.Rows[rowCount][colCount].ToString();
                    int aqi = 0;
                    if (colName.IndexOf("pm") > -1 || colName.IndexOf("no") > -1 || colName.IndexOf("O3") > -1)
                    {
                        if (val != "")
                        {
                            if (colName == "pm25")
                            {
                                aqi = ToAQI(val, "1");
                            }
                            else if (colName == "pm10")
                            {
                                aqi = ToAQI(val, "2");
                            }
                            else if (colName == "no2")
                            {
                                aqi = ToAQI(val, "3");
                            }
                            else if (colName == "O3")
                            {
                                aqi = ToAQI(val, "4");
                            }
                            val = val + "/" + aqi;
                        }
                        sb.Append("{\"val\":\"" + val + "\"},");
                    }
                    else
                    {
                        sb.Append("{\"val\":\"" + dt.Rows[rowCount][colCount] + "\"},");
                    }
                    if (colCount == 0)
                    {
                        sb.Append("{\"val\":\"预报\"},");
                    }
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("],");
            }
            sb.Remove(sb.Length - 1, 1);
        }
        else {
            sb.Append("\"row0\": [{ \"val\": \"没有数据\",\"colspan\": 6}]");
        }
        sb.Append("}");
        return sb.ToString();
    }
    public static int ToAQI(string value, string itemID)
    {
        //string itemID = ItemToItemID(item.Trim());
        int AQIValue = 0;
        double inputValue = 0d;
        double.TryParse(value, out inputValue);
        inputValue = inputValue / 1000;
        switch (itemID)
        {
            case "1":
                AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 24, 11, 180);
                break;
            case "2":
                AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 7, 11, 180);
                break;
            case "3":
                AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 22, 10, 0);
                break;
            case "4":
                AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 8, 10, 0);
                break;
            case "5":
                AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 8, 16, 16);
                break;
        }
        return AQIValue;
    }
}