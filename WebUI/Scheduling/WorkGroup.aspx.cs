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
using System.Collections.Generic;
using MMShareBLL.Model;
using MMShareBLL.BLL;

public partial class WorkGroup : System.Web.UI.Page
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
            //int backDays = int.Parse(ConfigurationManager.AppSettings["BackDays"]);
            CreateTable(dtNow);
        }
    }

     /// <summary>
    /// 根据当前的日期和返回的数量
    /// </summary>
    /// <param name="dtNow">当前时间</param>
    /// <returns></returns>
    private void CreateTable(DateTime dtNow)
    {
        Database db = m_Database;
        string strSQL = "SELECT DM FROM D_DurationTest WHERE CODE = '1'";
        //string strSQL = "SELECT DM FROM D_Duration WHERE CODE = '1'";
        DataSet dtDicTables = db.GetDataset(strSQL);
        List<string> lst = new List<string>();
        lst.Add("工作组管理");
        lst.Add("工作人员管理");
        //lst.Add("排班顺序管理");

        StringBuilder sb = new StringBuilder();
        //创建预报污染物标签
        for (int i = 0; i < lst.Count; i++)
        {
            if (i == 0)
            {
                m_FirstTab = string.Format("{1}_{0}", i, lst[i]);
                sb.Append(string.Format("<li><span id=\"{0}_{1}\" class=\"tabHighlight\" ><a style=\"color:white;\" href=\"javascript:tabClick('{0}_{1}')\">{0}</a></span></li>", lst[i], i));
            }
            else
            {
                sb.Append(string.Format("<li><span id=\"{0}_{1}\"><a href=\"javascript:tabClick('{0}_{1}')\">{0}</a></span></li>", lst[i], i));
            }
        }
        tabItem.InnerHtml = sb.ToString();
        sb.Length = 0;
    }
}
