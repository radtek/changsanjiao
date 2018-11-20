using Readearth.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EvaluateHtml_WriteData : System.Web.UI.Page
{
    public static Database m_database;
    public static DataTable dtItem;
    public static DataTable dtDuration;
    string sqlItem = "SELECT DM,MC FROM dbo.D_Item ";
    string sqlDuration = "SELECT DM,[Description]FROM dbo.D_Duration";   //后面按条件查询要每次请求，在网络慢的时候很慢，这里就全局定义一个table做条件查询
    protected void Page_Load(object sender, EventArgs e)
    {
        m_database = new Database();
        dtItem = m_database.GetDataTable(sqlItem);
        dtDuration = m_database.GetDataTable(sqlDuration);
    }

    [WebMethod]
    public static string GetData(string time) {
        StringBuilder json = new StringBuilder();
        //这里用“#”相连，前台分割降低容错率。（某种类型的数据出问题的时候前台eval的时候不会影响其他几种数据类型）
        GetHaze(time, json);   //获取霾
        json.Append("#");
        GetUV(time, json);   //获取UV
        json.Append("#");
        GetCountry(time, json);  //获取国家局
        json.Append("#");
        GetPeriods(time, json);
        return json.ToString();
    }
    public static void GetPeriods(string time, StringBuilder json) {
        string[] itemIDs = { "1", "2", "3", "4", "5" };
        string[] durationIds = { "4", "1", "2", "3" };
        string url = GetDataUrl(time+" 18:00:00");
        string content = HttpGetContent(url, "");
        DataTable dt = JsonToDataTable(content);
        //string sqlPeriod = "SELECT  Value,itemID,durationId FROM [dbo].[T_ForecastGroup] WHERE Module = 'wrf' AND Period='24'  and ForecastDate between '"+time+" 00:00:00' and '"+time+" 23:59:59' ORDER BY ForecastDate DESC; ";
        //DataTable dt = m_database.GetDataTable(sqlPeriod);
        json.Append("{\'dataPeriod\':{");
        if (dt != null && dt.Rows.Count > 0)
        {
            dt = ProInterfaceTab(dt);
            foreach (var durationid in durationIds)
            {
                json.Append("\'" + GetDuration(durationid) + "\':{");
                foreach (var itemID in itemIDs)
                {
                    string condition = "itemID='" + itemID + "' and durationId='" + durationid + "' and period='24'";
                    DataRow[] row = dt.Select(condition);
                    if (row.Length > 0)
                    {
                        string val = row[0].ItemArray[6].ToString();
                        val = val == "null" ? "-" : val;
                        json.Append("\'" + GetItem(itemID) + "\':\'" + val + "\',");
                    }
                    else
                    {
                        json.Append("\'" + GetItem(itemID) + "\':\'-\',");
                    }
                }
                json.Remove(json.Length - 1, 1);
                json.Append("},");
            }
            json.Remove(json.Length - 1, 1);
        }
        json.Append("}}");
    }
    public static string GetItem(string itemid) {
        string item = "";
        DataRow[] row = dtItem.Select("DM='" + itemid + "'");
        item = row[0].ItemArray[1].ToString();
        return item;
    }
    public static string GetDuration(string durationId)
    {
        string duration = "";
        DataRow[] row = dtDuration.Select("DM='"+durationId+"'");
        duration = row[0].ItemArray[1].ToString();
        return duration;
    }
    public static void GetCountry(string time,StringBuilder json) {
        string sqlCountry = "SELECT PM25,PM10,NO2,O3 FROM T_ChinaValue WHERE LST='" + time + "'";
        DataTable dt = m_database.GetDataTable(sqlCountry);
        json.Append("{\'dataCountry\':{");
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    string val = row[col.ColumnName].ToString();
                    val = val == "" ? "-" : val;
                    json.Append("\'" + col.ColumnName + "\':" + "\'" + val + "\',");
                }
            }
            json.Remove(json.Length - 1, 1);
        }
        else
        {
            json.Append("\'PM25:\'\'\',\'PM10:\'\'\',\'NO2:\'\'\',\'O3:\'\'\'");
        }
        json.Append("}}");
    }
    public static void GetUV(string time,StringBuilder json) {
        string[] periods = { "10", "16"};
        string sqlUV = "SELECT [Index],ReTime FROM T_TbUVS WHERE reTime between '" + time + " 00:00:00' and '" + time + " 23:59:59' ORDER BY ReTime ASC";
        DataTable dt = m_database.GetDataTable(sqlUV);
        json.Append("{\'dataUV\':{");
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < periods.Length; i++)
            {
                string condition = "ReTime='" + time + " " + periods[i] + ":00:00'";
                DataRow[] row = dt.Select(condition);
                if (row.Length > 0)
                {
                    string val = row[0]["index"].ToString();
                    json.Append("\'" + periods[i] + "时值\':" + "\'" + (val==""?"-":val) + "\',");
                }
                else
                {
                    json.Append("\'" + periods[i] + "时值:\'\'\',");
                }
            }
            json.Remove(json.Length - 1, 1);
        }
        else {
            json.Append("\'10时值:\'\'\',\'16时值:\'\'\'");
        }
        json.Append("}}");

    }
    public static void GetHaze(string time,StringBuilder json) {
        string str05 = "\'05时值:\'\'\'";
        string str17= "\'17时值:\'\'\'";
        string[] periods = {"05","17" };
        string sqlHaze = "SELECT HAZE,ReTime FROM T_24Haze WHERE reTime between '" + time + " 00:00:00' and '"+time+" 23:59:59' ORDER BY ReTime ASC";
        DataTable dt = m_database.GetDataTable(sqlHaze);
        json.Append("{\'dataHaze\':{");
        if (dt != null && dt.Rows.Count > 0) {
            for (int i = 0; i < periods.Length; i++) {
                string condition = "ReTime='" + time + " " + periods[i] + ":00:00'";
                DataRow[] row = dt.Select(condition);
                if (row.Length > 0)
                {
                    json.Append("\'" + periods[i] + "时值\':" + "\'" + GetHazeGrade(row[0]["Haze"].ToString()) + "\',");
                }
                else {
                    json.Append("\'"+periods[i]+"时值:\'\'\',");
                }
            }
            json.Remove(json.Length - 1, 1);
        }
        else
        {
            json.Append(str05+","+str17);
        }
        json.Append("}}");
        //return json;
    }
    public static string GetHazeGrade(string grade) {
        string haze = "";
        switch (grade)
        {
            case "0":
                haze = "无霾";
                break;
            case "1":
                haze = "轻微霾";
                break;
            case "2":
                haze = "轻度霾";
                break;
            case "3":
                haze = "中度霾";
                break;
            case "4":
                haze = "重度霾";
                break;
            case "5":
                haze = "严重霾";
                break;
        }
        return haze;
    }
    [WebMethod]
    public static string Confirm() {
        DateTime dNow = DateTime.Now;
        string reTime = dNow.ToString("yyyy-MM-dd 17:00:00");
        string moduleType = "evaluateReport";
        string sqlDel = "delete dbo.T_State where reTime='"+reTime+ "' and ModuleType='"+ moduleType + "'";
        string insert = "INSERT INTO dbo.T_State(ModuleType,ReTime,DeadLine,State,Type) VALUES"+
                        "('"+ moduleType + "','"+ reTime + "', '"+dNow.ToString("yyyy-MM-dd HH:mm:ss")+"', 3, '2')";
        try {
            m_database.Execute(sqlDel);
            m_database.Execute(insert);
            return "ok";
        }
        catch (Exception e) {
            return "error";
        }
    }

    public static string GetDataUrl(string date)
    {
        string url = ConfigurationManager.AppSettings["AQIPeriodDataURL"];
        url = url.Replace("%", "&");
        string method = "forecastGroupEach";
        string module = "SMCSubmit";//SMCSubmit  气象局
        url = string.Format(url, method, module, date);
        return url;
    }
    public static string HttpGetContent(string url, string postDataStr)
    {
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "Get";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    public static DataTable JsonToDataTable(string strJson)
    {
        //转换json格式
        strJson = strJson.Replace(",\"", "*\"").Replace("\":", "\"#").ToString();
        //取出表名   
        var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
        string strName = rg.Match(strJson).Value;
        DataTable tb = null;
        //去除表名   
        strJson = strJson.Substring(strJson.IndexOf("[") + 1);
        strJson = strJson.Substring(0, strJson.IndexOf("]"));

        //获取数据   
        rg = new Regex(@"(?<={)[^}]+(?=})");
        MatchCollection mc = rg.Matches(strJson);
        for (int i = 0; i < mc.Count; i++)
        {
            string strRow = mc[i].Value;
            string[] strRows = strRow.Split('*');

            //创建表   
            if (tb == null)
            {
                tb = new DataTable();
                tb.TableName = strName;
                foreach (string str in strRows)
                {
                    var dc = new DataColumn();
                    string[] strCell = str.Split('#');

                    if (strCell[0].Substring(0, 1) == "\"")
                    {
                        int a = strCell[0].Length;
                        dc.ColumnName = strCell[0].Substring(1, a - 2);
                    }
                    else
                    {
                        dc.ColumnName = strCell[0];
                    }
                    tb.Columns.Add(dc);
                }
                tb.AcceptChanges();
            }

            //增加内容   
            DataRow dr = tb.NewRow();
            for (int r = 0; r < strRows.Length; r++)
            {
                dr[r] = strRows[r].Split('#')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
            }
            tb.Rows.Add(dr);
            tb.AcceptChanges();
        }

        return tb;
    }
    public static DataTable ProInterfaceTab(DataTable dt)
    {
        DataTable newTab = dt.Copy();
        newTab.Columns.Add("changedLST");
        foreach (DataRow row in newTab.Rows)
        {
            string durationId = row["durationID"].ToString();
            string lst = row["LST"].ToString();
            string changedLST = lst;
            if (durationId == "1")
            {
                changedLST = DateTime.Parse(lst).AddDays(-1).ToString("yyyy-MM-dd HH:00:00");
            }
            row["changedLST"] = changedLST;
        }
        return newTab;
    }
}