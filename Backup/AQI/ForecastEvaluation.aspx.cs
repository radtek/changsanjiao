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

public partial class AQI_ForecastEvaluation : System.Web.UI.Page
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
        sb.Append("<li style=\"visibility:hidden\"><span id=\"AQI_0\" class = \"tabHighlight\"><a href=\"javascript:tabClick('AQI_0')\">AQI</a></span></li>");
        tabItem.InnerHtml = sb.ToString();
        sb.Length = 0;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        ForecastEvaluation forecast = new ForecastEvaluation();
        DataTable ds = forecast.GetEvaluationExport(H00.Value.ToString(), H01.Value.ToString());
        ExcelInport(ds);
    }

    private void ExcelInport(DataTable ds)
    {
        string fileName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "").Replace("/", "").Replace(":", "").Replace("-", "") + DateTime.Now.Millisecond.ToString();
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("起报时间,夜间,上午,下午,日平均,总计得分");
        for (int i = 0; i < ds.Rows.Count; i++)
        {
            string str = "";
            for (int j = 0; j < ds.Columns.Count; j++)
            {
                str = str + ds.Rows[i][j].ToString() + ",";
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
