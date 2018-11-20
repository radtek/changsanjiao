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
using System.Text;
using MMShareBLL.DAL;
using Readearth.Data;
using System.IO;
using System.Reflection;

public partial class AQI_ForecastFilter : System.Web.UI.Page
{
    public string m_ForecastDate;
    public string m_ForecastEndDate;
    public string m_FirstTab;
    public string m_UserJson;
    private Database m_Database;
    public bool m_UnLogin;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            DateTime dtNow = DateTime.Now;
            m_Database = new Database();

            m_ForecastDate = dtNow.AddDays(-7).ToString("yyyy年MM月dd日");
            m_ForecastEndDate = dtNow.ToString("yyyy年MM月dd日");
            int backDays = int.Parse(ConfigurationManager.AppSettings["BackDays"]);
            H00.Value = m_ForecastDate;
            H01.Value = m_ForecastEndDate;
            H00.Attributes.Add("tag", backDays.ToString());
            CreateTable(dtNow, backDays);

        }

    }
     /// <summary>
    /// 根据当前的日期和返回的数量
    /// </summary>
    /// <param name="dtNow">当前时间</param>
    /// <param name="backCount">可参考的历史数据</param>
    /// <returns></returns>
    private void CreateTable(DateTime dtNow, int backDays)
    {
        Database db = m_Database;

        string strSQL = "SELECT DM FROM D_DurationTest WHERE CODE = '1'";
        strSQL = strSQL + ";SELECT DM,MC FROM D_ITEM WHERE DM>0";
        DataSet dtDicTables = db.GetDataset(strSQL);

        DataTable dicDuration = dtDicTables.Tables[0];
        DataTable dicItem = dtDicTables.Tables[1];

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
        sb.Append("<li><span id=\"AQI_0\"><a href=\"javascript:tabClick('AQI_0')\">AQI</a></span></li>");
        tabItem.InnerHtml = sb.ToString();
        sb.Length = 0;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string Content = Element.Value;
        string[] strElement;
        strElement = Content.Split('|');
        DateTime dtFrom = DateTime.Parse(strElement[0]);
        DateTime dtTo = DateTime.Parse(strElement[1]);
        string period = strElement[2];
        string duraion = strElement[3];
        string shiCe = strElement[4];
        string modual = strElement[5];
        ComForecast comForecast = new ComForecast();
        DataSet ds = comForecast.StrSQLString(strElement[0], strElement[1], period, duraion, shiCe, modual);
        ExcelInport(ds);
    }
    private void  ExcelInport(DataSet ds)
    {
        string fileName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "").Replace("/", "").Replace(":", "").Replace("-", "") + DateTime.Now.Millisecond.ToString();
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("日期,时段名称,时段区间,预报时效,(PM2.5浓度)实测,综合预报,CMAQ,WRF-CHEM,(PM2.5AQI)实测,综合预报,CMAQ,WRF-CHEM,(PM10浓度)实测,综合预报,CMAQ,WRF-CHEM,(PM10AQI)实测,综合预报,CMAQ,WRF-CHEM,(NO2浓度)实测,综合预报,CMAQ,WRF-CHEM,(NO2AQI)实测,综合预报,CMAQ,WRF-CHEM,(O3-1h浓度)实测,综合预报,CMAQ,WRF-CHEM,(O3-1hAQI)实测,综合预报,CMAQ,WRF-CHEM,(O3-8h浓度)实测,综合预报,CMAQ,WRF-CHEM,(O3-8hAQI)实测,综合预报,CMAQ,WRF-CHEM,(实测)AQI,首要污染物,(综合预报)AQI,首要污染物,(CMAQ)AQI,首要污染物,(WRF-CHEM)AQI,首要污染物");
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            string str="";
            for (int j = 0; j < 12; j++)
            {
                str=str+ds.Tables[0].Rows[i][j].ToString()+",";
            }
            for (int m = 1; m < 6;m++)
            {
                for (int k = 4; k < 12; k++)
                {
                    str=str+ds.Tables[m].Rows[i][k].ToString()+",";
                }
            }
            sb.AppendLine(str.Substring(0, str.Length - 1));

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
}
