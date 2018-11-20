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

public partial class LevelChart : System.Web.UI.Page
{
    public string m_FromDate;
    public string m_ToDate;
    public string m_FirstTab;
    public string m_Station;
    public string typeName = "儿童感冒";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DateTime dtNow = DateTime.Now;
            m_FromDate = dtNow.AddMonths(-1).ToString("yyyy年MM月dd日");
            m_ToDate = dtNow.AddDays(2).ToString("yyyy年MM月dd日");
            //m_Station = Request["Station"];
            if (Request["type"] != null)
            {
                typeName = Request["type"];
            }
        }
    }

    private void CreateTable(DateTime dtNow, int backDays)
    {
   
    }
}
