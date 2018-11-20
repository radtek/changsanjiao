using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Readearth.Data;
using System.Text;
using System.IO;

public partial class AQI_DataShare : System.Web.UI.Page
{
    public string m_FromDate;
    public string m_ToDate;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DateTime dtNow = DateTime.Now;
            m_FromDate = dtNow.AddHours(-24).ToString("yyyy年MM月dd日 HH时");
            m_ToDate = dtNow.ToString("yyyy年MM月dd日 HH时");
        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string Content = Element.Value;
        string[] strElement;
        strElement = Content.Split('|');
        DateTime dtFrom = DateTime.Parse(strElement[0]);
        DateTime dtTo = DateTime.Parse(strElement[1]);
        Database m_Database = new Database();
        //dtFrom = DateTime.Parse("2014/2/23 16:00:00");
        //dtTo = DateTime.Parse("2014/2/26 23:00:00");
        string name = "";
        string strSQL = "";
        StringBuilder sb = new StringBuilder();
        string itemID = strElement[2];
        string itemStyle = strElement[3];
        if (itemStyle == "1")
        {

            name = returnName(itemID);
            strSQL = "SELECT collect_time,temperature,wind_direction,wind_speed,air_pressure,rain_sum,relativehumidity from T_NMElement WHERE station=" + itemID + "  AND collect_time BETWEEN '" + dtFrom + "' AND '" + dtTo + "' ORDER BY collect_time ASC";
            DataTable dt = m_Database.GetDataTable(strSQL);
            sb.AppendLine("日期,温度,风向,风速,气压,降雨量,相对湿度");
            foreach (DataRow dr in dt.Rows)
            {
                string dataLine = "";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dataLine = dataLine + dr[i] + ",";
                }
                sb.AppendLine(dataLine.Substring(0, dataLine.Length - 1));
            }
        }
        else if (itemStyle == "2")
        {
            //dtFrom = DateTime.Parse("2014/1/1 10:00:00");
            //dtTo = DateTime.Parse("2014/1/8 10:00:00");
            DataTable dSearch = new DataTable("T_Huanjing");
            dSearch.Columns.Add("日期", typeof(string));
            dSearch.Columns.Add("PM2.5", typeof(string));
            dSearch.Columns.Add("PM10", typeof(string));
            dSearch.Columns.Add("NO2", typeof(string));
            dSearch.Columns.Add("O3-1h", typeof(string));
            dSearch.Columns.Add("O3-8h", typeof(string));
            dSearch.Columns.Add("SO2", typeof(string));
            dSearch.Columns.Add("CO", typeof(string));

            int[] itemOrder = { 8, 3, 2, 6, 7, 1, 5 };
            strSQL = string.Format("SELECT LST,ITEMID,SITEID,CONVERT(decimal(10, 1), VALUE * 1000) AS VALUE FROM SEMC_DMC.DBO.LT_RT_SiteData WHERE LST BETWEEN '{0}' AND '{1}' AND ITEMID <=9 AND SITEID ='{2}' ORDER BY LST ASC", dtFrom, dtTo, strElement[2]);
            DataTable dtSiteData = m_Database.GetDataTable(strSQL);
            DataTable distinctLst = dtSiteData.DefaultView.ToTable(true, "LST");
            foreach (DataRow row in distinctLst.Rows)
            {
                DataRow newRow = dSearch.NewRow();
                newRow[0] = DateTime.Parse(row[0].ToString()).ToString("yyyy-MM-dd HH:mm").ToString();
                for (int i = 0; i < itemOrder.Length; i++)
                {
                    string filter = string.Format("LST = '{0}' AND ITEMID = {1}", row[0], itemOrder[i]);
                    DataRow[] rows = dtSiteData.Select(filter);
                    for (int j = 0; j < rows.Length; j++)
                    {
                        if (int.Parse(itemOrder[i].ToString()) == 5)
                            newRow[i + 1] = rows[j]["VALUE"].ToString() == "" ? "" : Math.Round(double.Parse(rows[j]["VALUE"].ToString()) / 1000, 1).ToString();
                        else
                            newRow[i + 1] = rows[j]["VALUE"];

                    }
                }
                dSearch.Rows.Add(newRow);
            }
            strSQL = "SELECT SITEID,NAME FROM T_SITE WHERE SITEID='" + strElement[2] + "'";
            DataTable dtSiteName = m_Database.GetDataTable(strSQL);
            name = dtSiteName.Rows[0][1].ToString();
            sb = new StringBuilder();
            sb.AppendLine("日期,PM2.5,PM10,NO2,O3-1h,O3-8h,SO2,CO");
            foreach (DataRow dr in dSearch.Rows)
            {
                string dataLine = "";
                for (int i = 0; i < dSearch.Columns.Count; i++)
                {
                    dataLine = dataLine + dr[i] + ",";
                }
                sb.AppendLine(dataLine.Substring(0, dataLine.Length - 1));
            }

        }
        else if (itemStyle == "3")
        {
            //dtFrom = DateTime.Parse("2014/2/23 16:00:00");
            //dtTo = DateTime.Parse("2014/2/26 23:00:00");
            DataTable dSearch = new DataTable("T_qixiang");
            dSearch.Columns.Add("日期", typeof(string));
            dSearch.Columns.Add("徐家汇", typeof(string));
            dSearch.Columns.Add("闵行", typeof(string));
            dSearch.Columns.Add("浦东", typeof(string));
            dSearch.Columns.Add("宝山", typeof(string));
            dSearch.Columns.Add("松江", typeof(string));
            dSearch.Columns.Add("嘉定", typeof(string));
            dSearch.Columns.Add("青浦", typeof(string));
            dSearch.Columns.Add("金山", typeof(string));
            dSearch.Columns.Add("奉贤", typeof(string));
            dSearch.Columns.Add("崇明", typeof(string));

            int[] itemOrder = { 58367, 58361, 58370, 58362, 58462, 58365, 58461, 58460, 58463, 58366 };

            strSQL = string.Format("SELECT collect_time,station,{2}  FROM T_NMElement WHERE collect_time BETWEEN '{0}' AND '{1}' ORDER BY collect_time ASC", dtFrom, dtTo, itemID);
            DataTable dtSiteData = m_Database.GetDataTable(strSQL);
            DataTable distinctLst = dtSiteData.DefaultView.ToTable(true, "collect_time");

            foreach (DataRow row in distinctLst.Rows)
            {
                DataRow newRow = dSearch.NewRow();
                newRow[0] = DateTime.Parse(row[0].ToString()).ToString("yyyy-MM-dd HH:mm").ToString();
                for (int i = 0; i < itemOrder.Length; i++)
                {
                    string filter = string.Format("collect_time = '{0}' AND station = {1}", row[0], itemOrder[i]);
                    DataRow[] rows = dtSiteData.Select(filter);
                    for (int j = 0; j < rows.Length; j++)
                    {
                        newRow[i + 1] = rows[j][2].ToString();

                    }
                }
                dSearch.Rows.Add(newRow);
            }
            name = itemID;
            sb = new StringBuilder();
            sb.AppendLine("日期,徐家汇,闵行,浦东,宝山,松江,嘉定,青浦,金山,奉贤,崇明");
            foreach (DataRow dr in dSearch.Rows)
            {
                string dataLine = "";
                for (int i = 0; i < dSearch.Columns.Count; i++)
                {
                    dataLine = dataLine + dr[i] + ",";
                }
                sb.AppendLine(dataLine.Substring(0, dataLine.Length - 1));
            }

        }
        else
        {
            //dtFrom = DateTime.Parse("2014/1/1 10:00:00");
            //dtTo = DateTime.Parse("2014/1/8 10:00:00");
            DataTable dSearch = new DataTable("T_Pollution");
            dSearch.Columns.Add("日期", typeof(string));
            dSearch.Columns.Add("静安监测站", typeof(string));
            dSearch.Columns.Add("卢湾师专附小", typeof(string));
            dSearch.Columns.Add("浦东川沙", typeof(string));
            dSearch.Columns.Add("浦东张江", typeof(string));
            dSearch.Columns.Add("普陀监测站", typeof(string));
            dSearch.Columns.Add("青浦淀山湖", typeof(string));
            dSearch.Columns.Add("徐汇上师大", typeof(string));
            dSearch.Columns.Add("杨浦四漂", typeof(string));
            dSearch.Columns.Add("虹口凉城", typeof(string));
            dSearch.Columns.Add("浦东监测站", typeof(string));
            int[] itemOrder = { 183, 185, 193, 195, 201, 203, 207, 209, 215, 228 };
            string itemTemp = "(183, 185, 193, 195, 201, 203, 207, 209, 215, 228)";

            strSQL = string.Format("SELECT LST,ITEMID,SITEID,CONVERT(decimal(10, 1), VALUE * 1000) AS VALUE FROM SEMC_DMC.DBO.LT_RT_SiteData WHERE LST BETWEEN '{0}' AND '{1}' AND ITEMID='{2}' AND SITEID in {3} ORDER BY LST ASC", dtFrom, dtTo, itemID, itemTemp);
            DataTable dtSiteData = m_Database.GetDataTable(strSQL);

            DataTable distinctLst = dtSiteData.DefaultView.ToTable(true, "LST");

          
            foreach (DataRow row in distinctLst.Rows)
            {
                DataRow newRow = dSearch.NewRow();
                newRow[0] = DateTime.Parse(row[0].ToString()).ToString("yyyy-MM-dd HH:mm").ToString();
                for (int i = 0; i < itemOrder.Length; i++)
                {
                    string filter = string.Format("LST = '{0}' AND SITEID = {1}", row[0], itemOrder[i]);
                    DataRow[] rows = dtSiteData.Select(filter);
                    for (int j = 0; j < rows.Length; j++)
                    {
                        if (itemID != "5")
                            newRow[i + 1] = rows[j]["VALUE"].ToString();
                        else
                            newRow[i + 1] = rows[j]["VALUE"].ToString() == "" ? "" : Math.Round(double.Parse(rows[j]["VALUE"].ToString()) / 1000, 1).ToString();

                    }
                }
                dSearch.Rows.Add(newRow);
            }
            name = returnNamePollution(itemID);
            sb = new StringBuilder();
            sb.AppendLine("日期,静安监测站,卢湾师专附小,浦东川沙,浦东张江,普陀监测站,青浦淀山湖,徐汇上师大,杨浦四漂,虹口凉城,浦东监测站");
            foreach (DataRow dr in dSearch.Rows)
            {
                string dataLine = "";
                for (int i = 0; i < dSearch.Columns.Count; i++)
                {
                    dataLine = dataLine + dr[i] + ",";
                }
                sb.AppendLine(dataLine.Substring(0, dataLine.Length - 1));
            }
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
    public string returnName(string itemID)
    {
        string name="";
        switch (itemID)
        {
            case "58367":
                name = "徐家汇";
                break;
            case "58361":
                name = "闵行";
                break;
            case "58370":
                name = "浦东";
                break;
            case "58362":
                name = "宝山";
                break;
            case "58462":
                name = "松江";
                break;
            case "58365":
                name = "嘉定";
                break;
            case "58461":
                name = "青浦";
                break;
            case "58460":
                name = "金山";
                break;
            case "58463":
                name = "奉贤";
                break;
            case "崇明":
                name = "徐家汇";
                break;
        }
        return name+"气象数据";
    }
    public string returnNamePollution(string itemID)
    {
        string name = "";
        switch (itemID)
        {
            case "5":
                name = "CO";
                break;
            case "1":
                name = "SO2";
                break;
            case "7":
                name = "O3-8h";
                break;
            case "6":
                name = "O3-1h";
                break;
            case "2":
                name = "NO2";
                break;
            case "3":
                name = "PM10";
                break;
            case "8":
                name = "PM2.5";
                break;
        }
        return name + "数据";
    }
}
