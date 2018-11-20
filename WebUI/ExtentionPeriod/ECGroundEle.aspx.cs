using Readearth.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ExtentionPeriod_ECGroundEle : System.Web.UI.Page
{
    public static Database m_Database;
    protected void Page_Load(object sender, EventArgs e)
    {
        m_Database = new Database("DBCONFIG");
    }
    [WebMethod]
    public static string GetTime() {
        string sql = "SELECT TOP 1 startTime FROM dbo.T_ECEleSH ORDER BY endTime DESC,period DESC ";
        DataTable dt = m_Database.GetDataTable(sql);
        DateTime date = DateTime.Now;
        if (dt != null && dt.Rows.Count > 0) {
            string time = dt.Rows[0]["startTime"].ToString();
            date = DateTime.Parse(time);
        }
        string Date = date.ToString("yyyy-MM-dd HH:00:00");
        return Date;
    }
    [WebMethod]
    public static string Query(string time) {
        string txt = "";
        string sql = "SELECT Ele,startTime,endTime,period,value FROM dbo.T_ECEleSH WHERE startTime='" + time+"' ORDER BY endTime ASC";
        int interval = 24,periods = 0;   //时效间隔，开始的时效
        try
        {
            DataTable dt = m_Database.GetDataTable(sql);
            DataTable newDT = dt.Clone();
            List<string> listEle = GetEle();
            if (dt != null && dt.Rows.Count > 0)
            {
                ProDataTable(time, listEle, interval, periods, dt, newDT);
                txt = JoinStr(newDT, listEle);
            }
        } catch (Exception e)
        {
            txt = "error" + e.Message;
        }
        return txt;
    }
    private static string JoinStr(DataTable dt, List<string> listEle) {
        string txt = "";
        if (dt != null && dt.Rows.Count > 0) {
            DateTime time = new DateTime(1970, 1, 1);
            StringBuilder json = new StringBuilder();
            foreach (string ele in listEle)
            {
                DataRow[] row = dt.Select("Ele='" + ele + "'");
                json.Clear();
                json.Append("[");
                foreach (DataRow dr in row)
                {
                    string endTime = (DateTime.Parse(dr["endTime"].ToString()) - time).TotalMilliseconds.ToString();
                    string value = dr["value"].ToString();
                    value = value == "-9999" ? "null" : value;
                    json.Append("[" + endTime + "," + value + "],");
                }
                json = json.Remove(json.Length - 1, 1);
                json.Append("]");
                txt += json.ToString() + "#" + GetEleStr(ele) + "&";
            }
            txt = txt.TrimEnd('&');
        }
        return txt;
    }
    private static string GetEleStr(string ele) {
        string str = "";
        switch (ele) {
            case "RH_2maboveground": str = "相对湿度";break;
            case "TMP_2maboveground": str = "2m温度"; break;
            case "wind_s_10": str = "风速"; break;
            case "wind_d_10": str = "方向"; break;
        }
        return str;
    }
    private static void ProDataTable(string time, List<string> listEle, int interval, int periods, DataTable dt,DataTable newDT)
    {
        DataTable tempDT = dt.Clone();
        try
        {
            foreach (string ele in listEle)
            {    //按要素筛选
                DataRow[] row = dt.Select("Ele='" + ele + "'");
                if (row.Length > 0)
                {
                    tempDT.Clear();
                    tempDT = row.CopyToDataTable();
                    DateTime maxDate = DateTime.Parse(tempDT.Rows[tempDT.Rows.Count - 1]["endTime"].ToString());
                    for (DateTime date = DateTime.Parse(time); date <= maxDate; date=date.AddHours(interval))   //根据日期遍历数据，没有就补-9999
                    {
                        float values = 0, val = 0;
                        DataRow[] dr = tempDT.Select("endTime='" + date.ToString("yyyy-MM-dd HH:00:00") + "'"); //有多个endtime，有的话取平均值，没有则补数据
                        DataRow newDR = newDT.NewRow();
                        if (dr.Length > 0)
                        {
                            foreach (DataRow _dr in dr)
                            {   //对多个endtime求平均
                                val += float.Parse(_dr.ItemArray[4].ToString());
                            }
                            values = val / dr.Length;
                        }
                        else
                        {   //没有这个时间的endtime数据，需要补数据
                            values = -9999;
                        }
                        newDR["Ele"] = ele;
                        newDR["startTime"] = time;
                        newDR["endTime"] = date.ToString("yyyy-MM-dd HH:00:00");
                        newDR["period"] = periods;
                        newDR["value"] = values.ToString();
                        newDT.Rows.Add(newDR);
                        periods += interval;
                    }
                }
            }
        }
        catch (Exception e) {
        }
        
    }

    public static List<string> GetEle() {
        List<string> list = new List<string>();
        string sql = "SELECT DISTINCT(ele) FROM dbo.T_ECEleSH ORDER BY Ele ASC";
        DataTable dt = m_Database.GetDataTable(sql);
        if (dt != null && dt.Rows.Count > 0) {
            foreach (DataRow row in dt.Rows) {
                string ele = row.ItemArray[0].ToString();
                //if (ele.IndexOf("wind") > -1) {
                //    continue;
                //}
                list.Add(ele);
            }
        }
        return list;
    }
}