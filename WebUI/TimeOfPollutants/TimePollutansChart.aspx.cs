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

public partial class TimePollutansChart : System.Web.UI.Page
{
    public string m_FromDate;
    public string m_ToDate;
    public string m_FirstTab;
    public string m_Station;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DateTime dtNow = DateTime.Now;
            m_FromDate = dtNow.AddDays(-1).ToString("yyyy年MM月dd日");
            m_ToDate = dtNow.AddDays(3).ToString("yyyy年MM月dd日");
            m_Station = Request["Station"];
        }
    }

    private void CreateTable(DateTime dtNow, int backDays)
    {
        Database db = new Database();

        string strSQL = "SELECT DM FROM D_Duration WHERE CODE = '1'";
        strSQL = strSQL + ";SELECT DM,MC FROM D_ItemCode";
        //strSQL = strSQL + ";SELECT DM FROM D_DATATYPE";
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
}
