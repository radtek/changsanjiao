using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Readearth.Data;
using System.Text;
using System.IO;

public partial class AQI_AQICompare : System.Web.UI.Page
{
    public string m_FromDate;
    public string m_ToDate;
    public string m_FirstTab;
    public bool m_UnLogin;
    public string m_PartID = "";
    public string m_MaxFrom="";
    public string SessionName = "";
    protected static readonly log4net.ILog m_Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    protected void Page_Load(object sender, EventArgs e)
    {
        //如果当前用户没有登录的话，那么返回到登录界面
        if (Request.Cookies["User"].Value == null)
        {
            m_UnLogin = true;
            return;
        }
        else
        {
            //SessionName=
            m_PartID = Request.Cookies["User"]["indexUser"];
            if (m_PartID == "2")
            {
                try
                {
                    int MaxMonth = -int.Parse(ConfigurationManager.AppSettings["CompareMaxMonth"]);
                    m_MaxFrom = ",minDate:'" +DateTime.Now.AddMonths(MaxMonth).ToString("yyyy年MM月dd日") + "'";
                }
                catch
                { }
            }
        }
        if (!Page.IsPostBack)
        {
            DateTime dtNow = DateTime.Now;
            m_FromDate = dtNow.AddMonths(-1).ToString("yyyy年MM月dd日");
            m_ToDate = dtNow.AddDays(2).ToString("yyyy年MM月dd日");
            m_FirstTab = "AQI";
        }
    }

    private void CreateTable(DateTime dtNow, int backDays)
    {
        Database db = new Database();

        string strSQL = "SELECT DM FROM D_DurationTest WHERE CODE = '1'";
        strSQL = strSQL + ";SELECT DM,MC FROM D_ITEM";
        DataSet dtDicTables = db.GetDataset(strSQL);

        DataTable dicDuration = dtDicTables.Tables[0];
        DataTable dicItem = dtDicTables.Tables[1];
        //DataTable dicItem = dtDicTables.Tables[2];

        StringBuilder sb = new StringBuilder();
        //创建预报污染物标签
        for (int i = 0; i < dicItem.Rows.Count; i++)
        {
            DataRow row = dicItem.Rows[i];
            if (i == 0)
            {
                sb.Append(string.Format("<li><span id=\"{0}_{1}\" class = \"tabHighlight\">{0}</span></li>", row[1], row[0]));
                m_FirstTab = string.Format("{0}_{1}", row[1], row[0]);
            }
            else
                sb.Append(string.Format("<li><span id=\"{0}_{1}\"><a href=\"javascript:tabClick('{0}_{1}')\">{0}</a></span></li>", row[1], row[0]));
        }
        tabItem.InnerHtml = sb.ToString();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            Database m_Database = new Database();
            DataTable dSearch = new DataTable("T_CompareExport");
            dSearch.Columns.Add("日期", typeof(string));
            dSearch.Columns.Add("实测", typeof(string));
            dSearch.Columns.Add("manual", typeof(string));
            dSearch.Columns.Add("WRF", typeof(string));
            dSearch.Columns.Add("CMAQ", typeof(string));
            string Content = Element.Value;
            string[] strElement;
            strElement = Content.Split('|');
            DateTime dtFrom = DateTime.Parse(strElement[0]);
            DateTime dtTo = DateTime.Parse(strElement[1]).AddHours(-1);
            int period = int.Parse(strElement[2].ToString());
            int itemID = int.Parse(strElement[3].ToString());
            string typeID = strElement[4].ToString();
            string module = "";
            if (typeID == "0,1")
                module = "('manual')";
            else
                module = "('manual','WRF','CMAQ')";
            string strWhere = " AND durationID IN(2,3,6)  AND LST BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
            string queryField = " LST,VALUE,PARAMETER";
            if (itemID == 0)
                queryField = " LST,AQI,PARAMETER ";
            string strSQL = "SELECT " + queryField + " ,'SC' AS module  FROM  T_ObsDataGroup WHERE ITEMID=" + itemID + strWhere + " union " + "SELECT" + queryField + ",module  FROM  T_ForecastGroup WHERE ITEMID=" + itemID + strWhere + " AND PERIOD=" + period + " AND module in " + module + " ORDER BY LST ASC";
            string[] itemOrder = { "SC", "Manual", "WRF", "CMAQ" };
            DataTable dtSiteData = m_Database.GetDataTable(strSQL);
            DataTable distinctLst = dtSiteData.DefaultView.ToTable(true, "LST");
            foreach (DataRow row in distinctLst.Rows)
            {
                DataRow newRow = dSearch.NewRow();
                newRow[0] = DateTime.Parse(row[0].ToString()).ToString("yyyy-MM-dd HH:mm").ToString();
                for (int i = 0; i < itemOrder.Length; i++)
                {
                    string filter = string.Format("LST = '{0}' AND module ='{1}'", row[0], itemOrder[i]);
                    DataRow[] rows = dtSiteData.Select(filter);
                    if (rows.Length > 0)
                    {
                        if (rows[0][1].ToString() != "")
                            newRow[i + 1] = Math.Round(double.Parse(rows[0][1].ToString()), 1).ToString();
                        else
                            newRow[i + 1] = "";
                    }
                    else
                        newRow[i + 1] = "";


                }
                dSearch.Rows.Add(newRow);
            }
            string name;
            if (itemID == 0)
                name = "AQI " + period + "小时数据";
            else if (itemID == 1)
                name = "PM2.5 " + period + "小时数据";
            else if (itemID == 2)
                name = "PM10 " + period + "小时数据";
            else if (itemID == 3)
                name = "NO2 " + period + "小时数据";
            else if (itemID == 4)
                name = "O3-1h " + period + "小时数据";
            else
                name = "O3-8h " + period + "小时数据";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("日期,实测,manual,WRF,CMAQ");
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
        catch (Exception ex)
        {
            m_Log.Error("Button1_Click", ex); 
        }
    }
}
