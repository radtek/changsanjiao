using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Readearth.Data;
using System.Data;
using MMShareBLL.DAL;
using System.Text;
using System.IO;

public partial class EvaluateHtml_WRFDay : System.Web.UI.Page
{
    public bool m_UnLogin;
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime dtNow = DateTime.Now;

        H00.Value = dtNow.AddHours(-24).ToString("yyyy年MM月");
    }
    protected void Button1_Click1(object sender, EventArgs e)
    {
        Database m_Database = new Database();
        DataTable dSearch = new DataTable("T_AirQuality");
        dSearch.Columns.Add("日期", typeof(string));
        dSearch.Columns.Add("首要污染物", typeof(string));
        dSearch.Columns.Add("AQI", typeof(int));
        dSearch.Columns.Add("PM2.5", typeof(int));
        dSearch.Columns.Add("PM10", typeof(int));
        dSearch.Columns.Add("NO2", typeof(int));
        dSearch.Columns.Add("O3-1h", typeof(int));
        dSearch.Columns.Add("O3-8h", typeof(int));
        string fromDate = Element.Value;
        string parameter = "";
        int AQI = 0;
        int maxAQI = 0;
        string filter = "";
        DateTime dtFrom = DateTime.Parse(fromDate).Date;
        DateTime dtTo = DateTime.Parse(fromDate).Date.AddMonths(1);
        string strSQL = string.Format("select a.LST,a.ITEMID,a.Value,D_Item.MC from(SELECT LST,ITEMID,avg(Value) as Value,Module FROM  T_ForecastSite where  ITEMID <6 and  Site in  ( select  station_co FROM sta_reg_set WHERE (flag <= 10) and flag<> 7) and LST between '{0}' and '{1}' and Interval=24 and durationID=7 group by LST,ITEMID,Module )a left join D_Item on a.ITEMID=D_Item.DM order by LST ", dtFrom.ToString("yyyy-MM-dd 00:00:00"), dtTo.ToString("yyyy-MM-dd 23:59:59"));
        DataTable dtSiteData = m_Database.GetDataTable(strSQL);
        DataTable distinctLst = dtSiteData.DefaultView.ToTable(true, "LST");
        foreach (DataRow row in distinctLst.Rows)
        {
            DataRow newRow = dSearch.NewRow();
            maxAQI = 0;
            newRow[0] = DateTime.Parse(row[0].ToString()).ToString("yyyyMMdd").ToString();
            for (int i = 1; i < 6; i++)
            {
                filter = string.Format("LST = '{0}' AND ITEMID = {1}", row[0], i.ToString());
                DataRow[] rows = dtSiteData.Select(filter);
                for (int j = 0; j < rows.Length; j++)
                {
                    EvalutionCaculate EvalutionCaculate = new EvalutionCaculate();
                    AQI = EvalutionCaculate.CalcuDayAQI(rows[0][2].ToString(), i.ToString());
                    if (AQI > maxAQI)
                    {
                        parameter = rows[0][3].ToString() + " ";
                        maxAQI = AQI;

                    }
                    else if (AQI == maxAQI)
                        parameter = parameter + rows[0][3].ToString() + " ";
                    newRow[2 + i] = AQI;

                }
            }
            newRow[1] = parameter;
            newRow[2] = maxAQI;
            dSearch.Rows.Add(newRow);
        }
        string name = "模式日数据";
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("日期,PM2.5,PM10,NO2,O3-1h,O3-8h");
        foreach (DataRow dr in dSearch.Rows)
        {
            string dataLine = "";
            for (int i = 0; i < dSearch.Columns.Count; i++)
            {
                dataLine = dataLine + dr[i] + ",";
            }
            sb.AppendLine(dataLine.Substring(0, dataLine.Length - 1));
        }
        StringWriter SW = new StringWriter(sb);
        SW.Close();
        string saveAsFileName = string.Format("{0}.csv", name);
        Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(saveAsFileName));
        Response.ContentType = "application/ms-excel";
        Response.Charset = "GB2312";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.Write(SW);
        Response.Flush();
        Response.End();
    }
}